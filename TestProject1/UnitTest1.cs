using Lcs9sem5pr1_DBtest;
using Lcs9sem5pr1_DBtest.Abstraction;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using Moq;

namespace TestProject1
{
    public class Tests
    {
        IMessageSource _source;
        IPEndPoint _endPoint;

        [SetUp]
        public void Setup()
        {

            _endPoint = new IPEndPoint(IPAddress.Any, 0); 
        }

        [Test]
        public void TestReciveMessage()
        {
            _source = new MockMessageSource();
            var result = _source.ReceiveMessage(ref _endPoint);
            Assert.IsNotNull(result);
            Assert.IsNull(result.Text);
            Assert.IsNotNull(result.FromName);
            Assert.That("Вася", Is.EqualTo(result.FromName));
            Assert.That(Command.Register, Is.EqualTo(result.Command));
        }

    }
}
public class MockMessageSource : IMessageSource
{
    private Queue<MessageUDP> messages = new();//очередь сообщений для имитации приёма сообщений
    //конструктор класса, который инициализирует начальные сообщения в очереди
    public MockMessageSource()
    {
        messages.Enqueue(new MessageUDP { Command = Command.Register, FromName = "Вася"});
        messages.Enqueue(new MessageUDP { Command = Command.Register, FromName = "Юля" });
        messages.Enqueue(new MessageUDP { Command = Command.Message, FromName = "Юля", ToName = "Вася", Text = "От Юли" });
        messages.Enqueue(new MessageUDP { Command = Command.Message, FromName = "Вася", ToName = "Юля", Text = "От Васи" });
        
    }
    public MessageUDP ReceiveMessage(ref IPEndPoint endPoint)
    {
        if (messages.Count == 0)
        {
            return null;
        }
        var msg = messages.Dequeue();
        return msg;
        //return messages.Peek();//Peek()
    }
    public void SendMessage(MessageUDP message, IPEndPoint endPoint)
    {
        messages.Enqueue(message);
    }
}