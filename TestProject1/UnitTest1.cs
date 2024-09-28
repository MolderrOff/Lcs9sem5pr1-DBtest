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
            Assert.That("����", Is.EqualTo(result.FromName));
            Assert.That(Command.Register, Is.EqualTo(result.Command));
        }

    }
}
public class MockMessageSource : IMessageSource
{
    private Queue<MessageUDP> messages = new();//������� ��������� ��� �������� ����� ���������
    //����������� ������, ������� �������������� ��������� ��������� � �������
    public MockMessageSource()
    {
        messages.Enqueue(new MessageUDP { Command = Command.Register, FromName = "����"});
        messages.Enqueue(new MessageUDP { Command = Command.Register, FromName = "���" });
        messages.Enqueue(new MessageUDP { Command = Command.Message, FromName = "���", ToName = "����", Text = "�� ���" });
        messages.Enqueue(new MessageUDP { Command = Command.Message, FromName = "����", ToName = "���", Text = "�� ����" });
        
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