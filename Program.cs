using Lcs9sem5pr1_DBtest.Models;
using System;
using Lcs9sem5pr1_DBtest;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//Семинар 5. Сервер, чат. Собираемся добавить поддержку работы с бд в наше приложение чата
//Code First. В messages Должны храниться сообщения
//в users список пользователей. Разработать модель т. о. чтобы учесть что в сообщениях
//есть не только автор, но и адресат и статус получения им сообщения

/*
 Разработать серверное приложение для обработки сообщений в чате с использованием базы
данных. Необходимо реализовать функционал регистрации новых клиентов, отправки
сообщений между клиентами с подтверждением доставки, а также хранение сообщений и
пользователей в базе данных. Приложение должно использовать UDP-сокеты для обмена
сообщениями и иметь возможность сериализации и десериализации сообщений в формате JSON.
Каждое сообщение должно содержать уникальный идентификатор и иметь возможность быть
адресованным как конкретному пользователю, так и всем пользователям чата
 */


/*
 1. Создать модель данных для хранения сообщений и пользователей. В модели должны быть учтены поля,
соответствующие требованиям задачи: сообщение, отправитель, получатель, статус получения и т.д.
2. Создать класс сервера, который будет обрабатывать входящие сообщения. Этот класс должен иметь методы
для регистрации новых клиентов, подтверждения получения сообщений и пересылки сообщений другим
клиентам.
3. Настроить сервер для приема входящих UDP-пакетов. При получении пакета, сервер должен десериализовать
сообщение из JSON, определить его тип (регистрация, подтверждение или обычное сообщение) и вызвать
соответствующий метод для обработки.
4. Использовать Entity Framework Core для взаимодействия с базой данных. При регистрации нового клиента
сервер должен добавить его в базу данных. При получении нового сообщения сервер должен сохранить его в
базу данных.
5. При получении сообщения сервер должен определить получателя и переслать ему сообщение. Пересылка
должна осуществляться через UDP.
6. Добавить обработку исключений для предотвращения падения сервера при возникновении ошибок.
Реализовать логирование действий сервера для отслеживания работы и выявления проблем.
7. Провести тестирование работы сервера с использованием различных сценариев взаимодействия. Отладить
код для выявления и исправления возможных ошибок.
 */

namespace Lcs9sem5pr1_DBtest
{
    internal class Program
    {
        static async Task Main(string[] args) //было без  async Task - void
        {
            if (args.Length == 0)
            {
                var s = new Server();
                s.Work();
                //тестовые данные 
                TestRegisterMessage(s);
                TestConfirmationMessage(s);
                TestRelayMessage(s);
                TestGetUnreadMessages(s);
                Console.WriteLine("Запущен сервер!");
            }
            else
            {
                await Client.SendMsg(args[0]);
            }
        }
        static void TestRegisterMessage(Server s)
        {
            //Тестовое сообщение для регистрации
            var registerMessage = new MessageUDP
            {
                Command = Command.Register,
                FromName = "User1"
            };
            //Отправляем тестовое сообщение на сервер для регистрации
            s.ProcessMessage(registerMessage, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 5430));
            Console.WriteLine("Отправляем тестовое сообщение на сервер для регистрации");
        }
        static void TestConfirmationMessage(Server s)
        {
            //Тестовое сообщение подтверждения
            var confirmationMessage = new MessageUDP
            {
                Command = Command.Confirmation,
                Id = 1 //идентификатор 1-го сообщения который нужо подтвердить
            };
            //отправляем тестовое сообщение на сервер для подтверждения
            s.ProcessMessage(confirmationMessage, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 5430));
        }
        static void TestRelayMessage(Server s)
        {
            //тестовое сообщение для пересылки
            var relayMessage = new MessageUDP
            {
                Command = Command.Message,
                FromName = "User1",
                ToName = "User2",
                Text = "Пробное сообщение 1."
            };
            //Отправляем тестовое сообщение на сревер для пересылки
            s.ProcessMessage(relayMessage, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 5430));
                           
        }
        static void TestGetUnreadMessages(Server s)
        {
            //Тестовое сообщение для получения непрочитанных сообщений
            var getUnreadMessages = new MessageUDP
            {
                Command = Command.GetUnreadMessages,
                FromName = "User2"
            };
            //Отправляем тестовое сообщение для получения непрочитанных сообщений
            s.ProcessMessage(getUnreadMessages, new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 5430));
        }
    }
}
