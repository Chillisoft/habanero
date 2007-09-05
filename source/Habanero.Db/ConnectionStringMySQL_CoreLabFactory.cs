using System;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// Summary description for ConnectionStringMySQL_CoreLabFactory.
    /// </summary>
    public class ConnectionStringMySQL_CoreLabFactory : ConnectionStringFactory
    {
        public ConnectionStringMySQL_CoreLabFactory()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        protected override void CheckArguments(string server, string database, string username, string password,
                                               string port)
        {
            if (server == "" || database == "" || username == "")
            {
                throw new ArgumentException("The server, database and username of a connect string can never be empty.");
            }
        }

        protected override string CreateConnectionString(string server, string database, string username,
                                                         string password, string port)
        {
            if (port == "")
            {
                port = "3306";
            }
            if (password != "")
            {
                return
                    String.Format("User={2}; Host={0}; Port={4}; Database={1}; Password={3};", server, database,
                                  username, password, port);
            }
            else
            {
                return String.Format("User={2}; Host={0}; Port={3}; Database={1};", server, database, username, port);
            }
        }
    }
}