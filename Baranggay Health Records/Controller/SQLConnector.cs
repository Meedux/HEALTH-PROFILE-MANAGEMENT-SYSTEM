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
            //Live Database
            //_connection = new MySqlConnection("Server=MYSQL5026.site4now.net;Database=db_aa39da_bhr;Uid=aa39da_bhr;Pwd=bcbrecords123");

            //Test Database
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
