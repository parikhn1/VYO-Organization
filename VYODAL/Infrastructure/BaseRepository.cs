using System.Configuration;
using MySql.Data.MySqlClient;
using System;

namespace VYODAL.Infrastructure
{
    public  class BaseRepository
    {
        public BaseRepository() { }
        public static string ConnectionName { get; set; }
        public static string ConnectionString = string.Empty;
        public static MySqlConnection Connection = null;
        public static string GetConnectionString(string nameOfConnectionString)
        {
            //null indicates default web.config
            //you can set it to what ever config file in you need
            System.Configuration.Configuration config =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
            if (config.ConnectionStrings.ConnectionStrings.Count > 0)
            {
                var connString = config.ConnectionStrings.ConnectionStrings[nameOfConnectionString].ToString();
                return connString;
            }
            return "";
        }
        
        public static void SetConnection()
        {       
            try
            {
                // READ CONNECTION STRING FROM WEB.CONFIG
                ConnectionString = GetConnectionString("VYOConnection");
            }
            catch(Exception e)
            {
                Console.WriteLine("Error to connect database" + e.Message);
            }

            if (Connection != null)
                Connection.Close();

            Connection = new MySqlConnection(ConnectionString);
            Connection.Open();
            
        }
        public void CloseConnection()
        {
            if(Connection != null)
                Connection.Close();
        }

    }
}