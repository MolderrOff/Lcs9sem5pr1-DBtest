using System.Net;

namespace Lcs9sem5pr1_DBtest.Abstraction
{
    //IMessageSource определяет методы отправки и получения сообщений

    public interface IMessageSource
    {
        void SendMessage(MessageUDP message, IPEndPoint endPoint);
        MessageUDP ReceiveMessage(ref IPEndPoint endPoint);
    }
}

