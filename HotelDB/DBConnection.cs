using MySql.Data.MySqlClient;

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
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM `users` WHERE `login` = @login";
            cmd.Parameters.AddWithValue("@login", login);
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

        //TODO: Delete this later
        /*public static void RegisterAdmin(string password)
        {
            MySqlCommand cmd = connection.CreateCommand();
            PasswordManager passwordManager = new PasswordManager();
            string salt = null;
            string hashPassword = passwordManager.GeneratePasswordHash(password, out salt);

            cmd.CommandText = $"INSERT INTO marseille.users (`login`, `password`, `salt`, `name`, `surname`, `patronymic`) VALUES ('admin', '{hashPassword}', '{salt}', 'Михаил', 'Белоусов', 'Анатольевич');";
            Open();
            cmd.ExecuteNonQuery();
            Close();
        }*/
    }
}