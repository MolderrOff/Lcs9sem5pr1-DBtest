using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lcs9sem5pr1_DBtest
{
    // перечисление для определения типа комманды
    public enum Command
    {
        Register,  // регистрация пользователя
        Message,    // сообщение
        Confirmation, //подтверждение
        GetUnreadMessages //непрочитанные сообщения //добавлено для ДЗ Реализуйте тип сообщений List
    }
    // Класс для представления сетевого сообщения
    public class MessageUDP
    {
        public Command Command { get; set; } // тип комманды
        public int? Id { get; set; }             // идентификатор сообщения
        public string FromName { get; set; } //имя отправителя
        public string ToName { get; set; } // имя получателя
        public string Text { get; set; } // текст сообщени        //Метод для сериализации в JSON
        public List<string> UnreadMessages { get; set; } //добавлено для ДЗ Реализуйте тип сообщений List
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        // Статический метод для десериализации Json в объект MyMessage
        public static MessageUDP FromJson(string json)
        {
            return JsonSerializer.Deserialize<MessageUDP>(json);
        }

        //Дальше временные данные
        //public MessageUDP(string fromName, string toName, Command r) // 
        //{
        //    FromName = fromName;
        //    ToName = toName;
           
        //}
        public MessageUDP()
        {

        }
    }
}
