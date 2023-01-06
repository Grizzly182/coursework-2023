using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace HotelDB
{
    /// <summary>
    /// Класс, отвечающий за подключение к базе данных
    /// </summary>
    public static class DBConnection
    {
        /// <summary>
        /// Измените содержание этой строки чтобы подключиться к другой базе данных MySql
        /// </summary>
        private static readonly string connectionString = "server=localhost; port=3306; user id=root; password=root; database=marseille";

        /// <summary>
        /// Статическое поле, является объект класса MySqlConnection, является подключением
        /// </summary>
        private static readonly MySqlConnection connection = new MySqlConnection(connectionString);

        /// <summary>
        /// Данный метод запускает соединение с БД
        /// </summary>
        public static void Open()
        {
            connection.Open();
        }

        /// <summary>
        /// Данный метод прекращает соединение с БД
        /// </summary>
        public static void Close()
        {
            connection.Close();
        }

        //Test Method
        public static bool FindUser(string login)
        {
            Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM `users` WHERE `login` = @login";
            cmd.Parameters.AddWithValue("@login", login);
            string username = "";
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    username = reader.GetString("login");
                }
            }
            Close();
            return username == login;
        }

        /// <summary>
        /// С помощью этой функции осуществляется поиск пользователя по логину,
        /// который задаётся в аргументах функции и получение хэша пароля данного пользователя.
        /// </summary>
        /// <param name="login">Логин необходимого пользователя</param>
        /// <returns>Возвращает строку хэша пароля</returns>
        public static string GetPasswordHash(string login)
        {
            //Пишем запрос в базу данных
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM `users` WHERE `login` = @login";
            cmd.Parameters.AddWithValue("@login", login);
            string outString = "";

            Open();
            //Читаем из БД
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    outString = reader.GetString("password");
                }
            }
            Close();
            return outString;
        }

        public static void RegisterAdmin(string password)
        {
            MySqlCommand cmd = connection.CreateCommand();
            byte[] byteData = Encoding.ASCII.GetBytes(password);
            //string stringPassword = password.ComputeHash(password);
            cmd.CommandText = "INSERT INTO users (`login`, `password`, `salt`, `role`, `name`, `surname`, `patronymic`) VALUES (\"admin\", SHA1(UNHEX(SHA1(\"35z663y31\"))), RANDOM_BYTES(8), 1, \"Михаил\", \"Белоусов\", \"Анатольевич\");";
            Open();
            cmd.ExecuteNonQuery();
            Close();
        }

        //TODO: Move this to class PasswordValidator
        public static string ComparePasswords(string first, string second)
        {
            return first + " " + second;
        }
    }
}