using Lcs9sem5pr1_DBtest.Abstraction;
using System;
using System.Net;

namespace Lcs9sem5pr1_DBtest
{
    public class Client
    {
        private readonly string _name; 
        private readonly IMessageSource _messageSource;
        private readonly IPEndPoint _peerEndpoint;
        public Client()
        {

        }
        public Client(IMessageSource messageSource, IPEndPoint peerEndPoint, string name)
        {
            _messageSource = messageSource;
            _peerEndpoint = peerEndPoint;
            _name = name;
        }
        //метод для регистрации клиента
        private void Regestred()
        {
            var messageJson = new MessageUDP()
            {
                Command = Command.Register,
                FromName = _name
            };
            _messageSource.SendMessage(messageJson, _peerEndpoint);
        }
        public void ClientSendler()
        {
            Regestred();
            while (true)
            {
                Console.WriteLine("Enter message: ");
                string text = Console.ReadLine();
                Console.WriteLine("Enter name: ");
                string name = Console.ReadLine();
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
                var messageJson = new MessageUDP()
                {
                    Text = text,
                    FromName = _name,
                    ToName = name
                };

                _messageSource.SendMessage(messageJson, _peerEndpoint);

            }
        }
        public void ClientListener()
        {
            Regestred();
            IPEndPoint ep = new IPEndPoint(_peerEndpoint.Address, _peerEndpoint.Port);
            while (true)
            {                
                MessageUDP message = _messageSource.ReceiveMessage(ref ep);
                Console.WriteLine(message.ToString());
            }

        }
    }
}
