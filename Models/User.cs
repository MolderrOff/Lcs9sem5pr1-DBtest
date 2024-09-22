using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcs9sem5pr1_DBtest.Models
{
    public partial class User
    {
        public int Id { get; set; } //Уникальный идентификатор пользователя
        public string Name { get; set; }
        //Навигационные свойства для связанных сообщений
        public virtual ICollection<Message> ToMessages { get; set; } = new List<Message>(); //для связи один ко многим. Сообщения полуенные пользователем
                                                                                            // добавить =new List<Message>()
        public virtual ICollection<Message> FromMessages { get; set; } = new List<Message>(); //Сообщения, отправленные пользователем
                                                                      // добавить =new List<Message>()
    }
}
