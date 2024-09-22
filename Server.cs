using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lcs9sem5pr1_DBtest.Models
{
    internal class Server //я добавил  internal
    {
        //словарь для хранения адресов клиентов по их именам
        Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        //Объект для работы с UDP-сокетом
        UdpClient udpClient;
        //Метод для обработки регистрации нового клиента
        void Register(MessageUDP message, IPEndPoint fromep)
        {
            Console.WriteLine("Message Register, name =  (Сообщение зарегистрировано от:) " + message.FromName);
            //Добавляем клиента в словарь
            clients.Add(message.FromName, fromep);

            //Добавляем пользователя в БД если его ещё нет
            using (var ctx = new Context())   //было TestContext
            {
                if (ctx.Users.FirstOrDefault(x => x.Name == message.FromName) != null) return;

                ctx.Add(new User { Name = message.FromName });

                ctx.SaveChanges();

            }

        }
        //Метод для подтверждения получения сообщения 
        void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation (сообщение подтверждено от) id=" + id);
            //изменяем статус получения сообщения в базе данных
            using (var ctx = new Context())
            {
                var msg = ctx.Messages.FirstOrDefault(x => x.Id == id);
                if (msg != null)
                {
                    msg.Received = true;
                    ctx.SaveChanges();
                }
            }
        }
        //Метод для пересылки сообщения
        void RelyMessage(MessageUDP message)
        {
            int? id = null;
            if (clients.TryGetValue(message.ToName, out IPEndPoint ep))
            {
                //Добавляем сообщение в БД
                using (var ctx = new Context())
                {
                    var fromUser = ctx.Users.First(x => x.Name == message.FromName);
                    var toUser = ctx.Users.First(x => x.Name == message.ToName);
                    var msg = new Lcs9sem5pr1_DBtest.Models.Message { 
                        FromUser = fromUser,
                        ToUser = toUser, 
                        Received = false, 
                        Text = message.Text };
                    ctx.Messages.Add(msg);
                    ctx.SaveChanges();
                    id = msg.Id;

                }
                //Подготавливаем сообщение для пересылки
                var forwardMessageJson = new MessageUDP()
                {
                    Id = id,
                    Command = Command.Message,
                    ToName = message.ToName,
                    FromName = message.FromName,
                    Text = message.Text
                }.ToJson();
                byte[] forwardBytes = Encoding.ASCII.GetBytes(forwardMessageJson);
                //Отправляем сообщение клиенту
                udpClient.Send(forwardBytes, forwardBytes.Length, ep);
                Console.WriteLine($"Message Relied, from = {message.FromName} to = {message.ToName}");

            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }
        //метод для обработки полученного сообщения
        public void ProcessMessage(MessageUDP message, IPEndPoint fromep)
        {
            Console.WriteLine($"Получено сообщение от {message.FromName} для {message.ToName}  c командой {message.Command}: ");
            Console.WriteLine(message.Text);
            //обработка в зависимости от команды сообщения
            if (message.Command == Command.Register)
            {
                Register(message, new IPEndPoint(fromep.Address, fromep.Port));

            }
            if (message.Command == Command.Confirmation)
            {
                Console.WriteLine("Confirmation receiver (Получение подтверждено)");
                ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Message)
            {
                RelyMessage(message);
            }
        }
        //метод запуска работы сервера
        public void Work()
        {
            //Инициализация объекта для приёма данных по UDP
            IPEndPoint remoteEndPoint;
            udpClient = new UdpClient(5430); //был порт 12345
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("UDP Клиент ожидает сообщений...");

            //Бесконечный цикл приёма сообщений
            while (true)
            {
                byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
                string receivedData = Encoding.ASCII.GetString(receiveBytes);

                Console.WriteLine(receivedData);
                try
                {
                    //Десериализация полученного сообщения
                    var message = MessageUDP.FromJson(receivedData);

                    //Обработка сообщения
                    ProcessMessage(message, remoteEndPoint);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                }
            }
        }
    }
}
