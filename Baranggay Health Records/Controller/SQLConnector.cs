using MySql.Data.MySqlClient;
using System;

namespace Baranggay_Health_Records.Controller
{
    public class SQLConnector
    {
        public MySqlConnection connection;
        public string connectionString;

        //Constructor
        public SQLConnector(string server, string database, string username, string password) {
            connectionString = $"Server={server};Database={database};User ID={username};Password={password};";
            connection = new MySqlConnection(connectionString);
        }
        ~SQLConnector() {
            connection.Close();
        }

        public void OpenConnection()
        {
            try
            {
                if(connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch(Exception ex) {
                Console.WriteLine("Error opening the connection: " + ex.Message);
            }
        }

        public void CloseConnection() {
            try
            {
                if(connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Error closing the connection: " + ex.Message);
            }
        }
    }
}
