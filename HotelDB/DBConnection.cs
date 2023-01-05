using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;

namespace HotelDB
{
    public static class DBConnection
    {
        /// <summary>
        /// Change "connectionString" variable to connect to other database.
        /// </summary>
        private static readonly string connectionString = "server=localhost; port=3306; user id=root; password=root; database=marseille";

        private static readonly MySqlConnection connection = new MySqlConnection(connectionString);

        public static void Open()
        {
            connection.Open();
        }

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
        /// In this function we are getting password's hash from our database
        /// via searching by user login
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static string GetPasswordHash(string login)
        {
            Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT * FROM `users` WHERE `login` = @login";
            cmd.Parameters.AddWithValue("@login", login);
            string outString = "";
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