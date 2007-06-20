using System;
using System.Collections;
using Habanero.Db;

namespace Chillisoft.Test.General.v2
{
    /// <summary>
    /// Summary description for MyDBConnection.
    /// </summary>
    public class MyDBConnection
    {
        public static String GetConnectionString()
        {
            return GetDatabaseConfig().GetConnectionString();
        }

        public static DatabaseConfig GetDatabaseConfig()
        {
            IDictionary settings = new Hashtable();

            settings.Add("vendor", DatabaseConfig.MySQL);
            settings.Add("server", "localhost");
            settings.Add("database", "HabaneroTest");
            settings.Add("username", "root");
            settings.Add("password", "root");
            settings.Add("port", "3306");

            DatabaseConfig config = new DatabaseConfig(settings);
            return config;
        }
    }
}