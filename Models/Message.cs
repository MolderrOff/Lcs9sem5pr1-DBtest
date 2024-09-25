using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcs9sem5pr1_DBtest.Models
{
    public partial class Message
    {
        public int Id { get; set; } //Уникальный идентификатор сообщения
        public string Text { get; set; } //Текст сообщения в конце приписать =null!
        public bool Received { get; set; } //Флаг, указывающий было ли сообщение получено
        public int? ToUserId { get; set; } //Идентификатор получателя
        public int? FromUserId { get; set; }//Идентификатор отправителя
        public virtual User? ToUser { get; set; }//Навигационное свойство для получателя
        public virtual User? FromUser { get; set; } // Навигационное свойство для отправителя
        //public List<string>? UnreadMessages { get; set; } // Список непрочитанных сообщений



    }
}
