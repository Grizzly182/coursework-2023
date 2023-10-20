using System;

namespace Marseille
{
    public class Client
    {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string Patronymic { get; private set; }
        public DateTime Birthday { get; private set; }
        public string Phone { get; private set; }

        public Client(uint id, DateTime birthday, string name, string surname, string patronymic = "", string phone = "")
        {
            Id = id;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Birthday = birthday;
            Phone = phone;
        }

        public Client(DateTime birthday, string name, string surname, string patronymic = "", string phone = "")
        {
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Birthday = birthday;
            Phone = phone;
        }

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}\n{Birthday.ToShortDateString()}\n{Phone}\n";
        }
    }
}