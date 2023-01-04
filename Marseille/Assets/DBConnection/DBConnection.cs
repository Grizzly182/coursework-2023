using MySql.Data.MySqlClient;

namespace Marseille
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
    }
}