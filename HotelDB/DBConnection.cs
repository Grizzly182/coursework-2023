using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace HotelDB
{
    /// <summary>
    /// Класс, отвечающий за подключение к базе данных
    /// </summary>
    public static class DBConnection
    {
        private static string _server = "localhost";
        private static string _port = "3306";
        private static string _userId = "root";
        private static string _password = "root";
        private static string _database = "marseille";

        /// <summary>
        /// Измените содержание этой строки чтобы подключиться к другой базе данных MySql
        /// </summary>
        private static readonly string connectionString = $"server={_server}; port={_port}; userid={_userId}; password={_password}; database={_database}";

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

        private static MySqlCommand CreateCommand(string query)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            return cmd;
        }

        private static MySqlCommand CreateCommandWithParameters(string query, Dictionary<string, string> parameters)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                cmd.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }
            return cmd;
        }

        /// <summary>
        /// Функция для проверки правильности введённых данных.
        /// </summary>
        /// <param name="login">Логин, введённый пользователем в окне входа.</param>
        /// <param name="password">Пароль, введённый пользователем в окне входа.</param>
        /// <returns>Возвращает <c>true</c>, если введённый пароль верен, иначе - <c>false</c>.</returns>
        public static bool Login(string login, string password)
        {
            string passFromDB;
            string saltFromDB;

            passFromDB = GetPasswordHashAndSaltFromDB(login, out saltFromDB);

            PasswordManager passwordManager = new PasswordManager();
            return passwordManager.IsPasswordsMatch(password, saltFromDB, passFromDB);
        }

        /// <summary>
        /// Эта функция запрашивет из базы данных пароль и соль определённого пользователя.
        /// Логин этого пользователя передаётся в аргументах.
        /// </summary>
        /// <param name="login">Логин необходимого пользователя</param>
        /// <param name="salt">В данный аргумент нужно передать переменную типа string, в неё запишется соль из Базы данных.</param>
        /// <returns>Возвращает строку хэша пароля и соль из базы данных</returns>
        private static string GetPasswordHashAndSaltFromDB(string login, out string salt)
        {
            //Пишем запрос в базу данных
            MySqlCommand cmd = CreateCommandWithParameters("SELECT `password`,`salt` FROM `users` WHERE `login` = @login", new Dictionary<string, string> { { "@login", login } });

            string password = "";

            salt = "";

            //Читаем из БД
            Open();

            using (MySqlDataReader reader = cmd.ExecuteReader())

            {
                while (reader.Read())
                {
                    password = reader.GetString("password");
                    salt = reader.GetString("salt");
                }
            }
            Close();

            return password;
        }

        //TODO: Comment this
        /// <summary>
        ///
        /// </summary>
        /// <param name="login"></param>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="patronymic"></param>
        /// <param name="role"></param>
        public static void GetUserData(string login, out string name, out string surname, out string patronymic, out int role)
        {
            MySqlCommand cmd = CreateCommandWithParameters("Select `name`,`surname`,`patronymic`,`role_id` from `users` where `login` = @login;",
                new Dictionary<string, string> { { "@login", login } });

            name = surname = patronymic = string.Empty;
            role = 1;

            Open();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    name = reader.GetString("name");
                    surname = reader.GetString("surname");
                    patronymic = reader.GetString("patronymic");
                    role = reader.GetInt32("role_id");
                }
            }

            Close();
        }

        //TODO: Delete this later
        /// <summary>
        ///
        /// </summary>
        /// <param name="password"></param>
        //public static void RegisterAdmin(string password)
        //{
        //    MySqlCommand cmd = connection.CreateCommand();
        //    PasswordManager passwordManager = new PasswordManager();
        //    string hashPassword = passwordManager.GeneratePasswordHash(password, out string salt);

        //    cmd.CommandText = $"INSERT INTO marseille.users (`login`, `password`, `salt`, `name`, `surname`, `patronymic`, `role_id`) VALUES ('admin', '{hashPassword}', '{salt}', 'Михаил', 'Белоусов', 'Анатольевич', 1);";
        //    Open();
        //    cmd.ExecuteNonQuery();
        //    Close();
        //}
    }
}