using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lcs9sem5pr1_DBtest.Models
{
    public partial class Context: DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; } //набор данных для сообщений
        public Context() { }
        //конфигурация подключения к базе данных и настройка опций
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .LogTo(Console.WriteLine) //логгирование запросов
                .UseLazyLoadingProxies() //использование ленивой загрузки
                .UseNpgsql("Host=localhost;Username=postgres;Password=postgres;Database=ChatDB");
        }
        // определение отношений между сущностями и настройка схемы базы данных
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                //определение первичного ключа и имени таблицы для сообщений
                entity.HasKey(x => x.Id).HasName("message_pkey"); //установили первичный ключ
                entity.ToTable("Messages");        //установили название нашей таблицы
                //определение колонок таблицы сообщений и их соответствующих свойств в классе Message
                entity.Property(x => x.Id).HasColumnName("id");          //каждому полю будем имена давать
                entity.Property(x => x.Text).HasColumnName("text");
                entity.Property(x => x.FromUserId).HasColumnName("from_user_id");
                entity.Property(x => x.ToUserId).HasColumnName("to_user_id");

                entity.HasOne(d => d.FromUser)
                .WithMany(p => p.FromMessages)
                .HasForeignKey(e => e.FromUserId)
                .HasConstraintName("messages_from_user_id_fkey");//messages_from_user_id_fkey

                entity.HasOne(d => d.ToUser)
                .WithMany(p => p.ToMessages)
                .HasForeignKey(e => e.ToUserId)
                .HasConstraintName("messages_to_user_id_fkey");//messages_from_user_id_fkey

            });
            modelBuilder.Entity<User>(entity => {
                entity.HasKey(x => x.Id).HasName("user_pkey"); //установили первичный ключ
                entity.ToTable("Users"); //установили название нашей таблицы
                //определение колонок таблицы пользователей и их соответствующих свойств в классе User
                entity.Property(x => x.Id).HasColumnName("id");   //каждому полю будем имена давать
                entity.Property(x => x.Name)
                .HasMaxLength(255)
                .HasColumnName("name");

            });
            base.OnModelCreating(modelBuilder); //Дополнительные настройки модели базы данных
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder); // Частичный метод для дополнительной Это добавил
        //конфигурации модели
    }
}
