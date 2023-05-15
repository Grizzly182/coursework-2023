using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Xml.Linq;

namespace HotelDB
{
    /// <summary>
    /// Класс, отвечающий за подключение к базе данных
    /// </summary>
    public static class DBConnection
    {
        private static readonly string _server = "localhost";
        private static readonly string _port = "3306";
        private static readonly string _userId = "root";
        private static readonly string _password = "root";
        private static readonly string _database = "marseille";

        /// <summary>
        /// Измените содержание полей чтобы подключиться к другой базе данных MySql
        /// </summary>
        private static readonly string connectionString = $"server={_server}; port={_port}; userid={_userId}; password={_password}; database={_database}";

        /// <summary>
        /// Статическое поле, является подключением
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
        public static void GetUserData(string login, out uint id, out string name, out string surname, out string patronymic, out int role)
        {
            MySqlCommand cmd = CreateCommandWithParameters("Select `id`,`name`,`surname`,`patronymic`,`role_id` from `users` where `login` = @login;",
                new Dictionary<string, string> { { "@login", login } });

            name = surname = patronymic = string.Empty;
            role = 1;
            id = 0;

            Open();

            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.ReadAsync().Result)
                {
                    id = reader.GetUInt32("id");
                    name = reader.GetString("name");
                    surname = reader.GetString("surname");
                    patronymic = reader.GetString("patronymic");
                    role = reader.GetInt32("role_id");
                }
            }

            Close();
        }

        //TODO: Comment this later
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllUsersLogin()
        {
            MySqlCommand cmd = CreateCommand("Select `login` from `users`;");
            List<string> users = new List<string>();

            Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(reader.GetString("login"));
                }
            }
            Close();

            return users;
        }

        public static List<uint> GetRoomIds()
        {
            MySqlCommand cmd = CreateCommand("SELECT `id` from `rooms`;");

            List<uint> ids = new List<uint>();
            Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ids.Add(reader.GetUInt32("id"));
                }
            }
            Close();

            return ids;
        }

        public static Dictionary<uint, string> GetRoomFeatures(uint id)
        {
            Dictionary<uint, string> features = new Dictionary<uint, string>();

            MySqlCommand cmd = CreateCommand($"SELECT room_features.`id`, room_features.`name` FROM room_features " +
                $"JOIN rooms_has_room_features ON room_features.`id` = rooms_has_room_features.room_feature_id AND rooms_has_room_features.room_id = {id};");

            Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                uint idKey = 0; ;
                string feature = string.Empty;
                while (reader.Read())
                {
                    idKey = reader.GetUInt32("id");
                    feature = reader.GetString("name");
                    features.Add(idKey, feature);
                }
            }
            Close();

            return features;
        }

        public static Dictionary<uint, string> GetAllRoomFeatures()
        {
            Dictionary<uint, string> features = new Dictionary<uint, string>();

            MySqlCommand cmd = CreateCommand($"Select * from room_features;");

            Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    uint id = reader.GetUInt32("id");
                    string name = reader.GetString("name");
                    features.Add(id, name);
                }
            }
            Close();
            return features;
        }

        public static void GetRoomData(uint id, out uint number, out decimal cost, out int beds, out string type, out string description, out int status)
        {
            number = 0;
            status = 0;
            beds = 0;
            cost = 0;
            type = description = string.Empty;

            MySqlCommand cmd = CreateCommand($"Select `number`,`cost`,`beds_count`,`type`, `description`, `status_id` from `rooms` where (`id` = '{id}');");

            Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    number = reader.GetUInt32("number");
                    cost = reader.GetDecimal("cost");
                    beds = reader.GetInt32("beds_count");
                    type = reader.GetString("type");
                    description = reader.GetString("description");
                    status = reader.GetInt32("status_id");
                }
            }
            Close();
        }

        //TODO: comment this
        /// <summary>
        ///
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="surname"></param>
        /// <param name="role"></param>
        /// <param name="patronymic"></param>
        public static void CreateUser(string login, string password, string name, string surname, int role, string patronymic = "")
        {
            PasswordManager passwordManager = new PasswordManager();
            string hashPassword = passwordManager.GeneratePasswordHash(password, out string salt);
            MySqlCommand cmd = CreateCommand(
                $"INSERT INTO marseille.users (`login`, `password`, `salt`, `name`, `surname`, `patronymic`, `role_id`) " +
                $"VALUES ('{login}', '{hashPassword}', '{salt}', '{name}', '{surname}', '{patronymic}', {role});");

            Open();
            cmd.ExecuteNonQuery();
            Close();
        }

        public static void EditUser(uint id, string login, string name, string surname, int role, string patronymic)
        {
            MySqlCommand cmd = CreateCommand($"UPDATE `marseille`.`users` SET `name` = '{name}', `surname` = '{surname}'," +
                $" `patronymic` = '{patronymic}', `login` = '{login}', `role_id` = {role} WHERE (`id` = '{id}');");

            Open();
            cmd.ExecuteNonQuery();
            Close();
        }

        public static void EditUser(uint id, string login, string name, string surname, int role, string patronymic, string password)
        {
            PasswordManager passwordManager = new PasswordManager();
            string hashPassword = passwordManager.GeneratePasswordHash(password, out string salt);
            MySqlCommand cmd = CreateCommand($"UPDATE `marseille`.`users` SET `name` = '{name}', `surname` = '{surname}'," +
                $" `patronymic` = '{patronymic}', `login` = '{login}', `role_id` = {role}, `password` = '{hashPassword}', `salt` = '{salt}' WHERE (`id` = '{id}');");

            Open();
            cmd.ExecuteNonQuery();
            Close();
        }

        // TODO: comment this
        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteUser(uint id)
        {
            MySqlCommand cmd = CreateCommand($"DELETE FROM `marseille`.`users` WHERE (`id` = '{id}');");

            Open();
            cmd.ExecuteNonQuery();
            Close();
        }

        // TODO: Comment this later
        /// <summary>
        ///
        /// </summary>
        /// <param name="number"></param>
        /// <param name="cost"></param>
        /// <param name="bedsCount"></param>
        /// <param name="type"></param>
        /// <param name="description"></param>
        /// <param name="statusId"></param>
        public static void CreateRoom(uint number, decimal cost, int bedsCount, string type, string description, int statusId, Dictionary<uint, string> features)
        {
            MySqlCommand cmd = CreateCommand(
                $"INSERT INTO marseille.rooms (`number`, `cost`, `beds_count`, `type`, `description`, `status_id`) " +
            $"VALUES ('{number}', '{cost}', '{bedsCount}', '{type}', '{description}', '{statusId}');");

            Open();
            cmd.ExecuteNonQuery();
            Close();

            cmd = CreateCommand($"Select `id` from marseille.rooms where `number` = '{number}';");

            uint id = 0;
            Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = reader.GetUInt32("id");
                }
            }
            Close();

            foreach (var feature in features)
            {
                cmd = CreateCommand($"INSERT INTO `marseille`.`rooms_has_room_features` (`room_id`, `room_feature_id`) VALUES ('{id}', '{feature.Key}');");

                Open();
                cmd.ExecuteNonQuery();
                Close();
            }
        }

        public static void EditRoom(uint id, uint number, decimal cost, int bedsCount, string type, string description, int statusId, Dictionary<uint, string> features)
        {
            MySqlCommand cmd;

            cmd = CreateCommand($"DELETE FROM `marseille`.`rooms_has_room_features` WHERE (`room_id` = '{id}');");

            Open();
            cmd.ExecuteNonQuery();
            Close();

            Open();
            foreach (var feature in features)
            {
                cmd = CreateCommand($"INSERT INTO `marseille`.`rooms_has_room_features` (`room_id`, `room_feature_id`) VALUES ('{id}', '{feature.Key}');");

                cmd.ExecuteNonQuery();
            }
            Close();

            cmd = CreateCommand($"UPDATE `marseille`.`rooms` SET `number` = '{number}', `cost` = '{cost}', `beds_count` = '{bedsCount}', " +
                $"`type` = '{type}', `description` = '{description}', `status_id` = '{statusId}' " +
                $"WHERE (`id` = '{id}');");

            Open();
            cmd.ExecuteNonQuery();
            Close();
        }

        public static void DeleteRoom(uint id)
        {
            MySqlCommand cmd = CreateCommand($"DELETE FROM `marseille`.`rooms` WHERE (`id` = '{id}');");

            Open();
            cmd.ExecuteNonQuery();
            Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="number">Номер комнаты</param>
        /// <returns></returns>
        public static bool ContainsRoom(uint number)
        {
            MySqlCommand cmd = CreateCommand($"SELECT count(`id`) FROM marseille.rooms where `number` = '{number}';;");

            int count = 0;
            Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    count = reader.GetInt32("count(`id`)");
                }
            }
            Close();
            return count == 1;
        }

        public static bool ContainsFeature(string name)
        {
            MySqlCommand cmd = CreateCommand($"SELECT count(`name`) FROM marseille.room_features where `name` = '{name}';");

            int count = 0;
            Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    count = reader.GetInt32("count(`name`)");
                }
            }
            Close();
            return count == 1;
        }

        public static bool ContainsUser(string login)
        {
            MySqlCommand cmd = CreateCommand($"SELECT count(`login`) FROM marseille.users where `login` = '{login}';");

            int count = 0;
            Open();
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    count = reader.GetInt32("count(`login`)");
                }
            }
            Close();
            return count == 1;
        }

        public static void CreateFeature(string name)
        {
            MySqlCommand cmd = CreateCommand($"Insert into `marseille`.`room_features` (`name`) Values ('{name}');");

            Open();
            cmd.ExecuteNonQuery();
            Close();
        }

        public static void DeleteFeature(uint id)
        {
            MySqlCommand cmd = CreateCommand($"DELETE FROM `marseille`.`room_features` WHERE (`id` = '{id}');");

            Open();
            cmd.ExecuteNonQuery();
            Close();
        }

        //TODO: Delete this later
        /// <summary>
        ///
        /// </summary>
        /// <param name="password"></param>
        //public static void RegisterAdmin(string login, string password)
        //{
        //    MySqlCommand cmd = connection.CreateCommand();
        //    PasswordManager passwordManager = new PasswordManager();
        //    string hashPassword = passwordManager.GeneratePasswordHash(password, out string salt);

        //    cmd.CommandText = $"INSERT INTO marseille.users (`login`, `password`, `salt`, `name`, `surname`, `patronymic`, `role_id`) VALUES ('{login}', '{hashPassword}', '{salt}', 'Михаил', 'Белоусов', 'Анатольевич', 1);";
        //    Open();
        //    cmd.ExecuteNonQuery();
        //    Close();
        //}

        //public static void RegisterManager(string login, string password)
        //{
        //    MySqlCommand cmd = connection.CreateCommand();
        //    PasswordManager passwordManager = new PasswordManager();
        //    string hashPassword = passwordManager.GeneratePasswordHash(password, out string salt);

        //    cmd.CommandText = $"INSERT INTO marseille.users (`login`, `password`, `salt`, `name`, `surname`, `patronymic`, `role_id`) VALUES ('{login}', '{hashPassword}', '{salt}', 'Владимир', 'Поликарпов', 'Владимирович', 2);";
        //    Open();
        //    cmd.ExecuteNonQuery();
        //    Close();
        //}
    }
}