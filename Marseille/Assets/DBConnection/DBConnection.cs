using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Marseille
{
    public static class DBConnection
    {
        private static readonly string server = "localhost";
        private static readonly string database = "marseille";
        private static readonly string user = "root";
        private static readonly string password = "root";
        private static readonly string port = "3306";

        private static readonly string connectionString = $"server={server};port={port};user id={user}; password={password}; database={database}";

        private static MySqlConnection connection = new MySqlConnection(connectionString);

        public static void Open()
        {
            connection.Open();
        }

        public static void Close()
        {
            connection.Close();
        }
    }
}