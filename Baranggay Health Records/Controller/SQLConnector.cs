using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;

namespace Baranggay_Health_Records.Controller
{
    public class SQLConnector : IDisposable
    {
        private readonly MySqlConnection _connection;

        public SQLConnector(IConfiguration configuration)
        {
            _connection = new MySqlConnection("Server=127.0.0.1;Port=3306;Database=bhr;User=bhrs;Password=bhrs;");
            Console.WriteLine("Database Connected!");
        }

        public MySqlConnection GetConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();
            return _connection;
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
