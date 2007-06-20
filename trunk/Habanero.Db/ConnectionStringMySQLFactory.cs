using System;

namespace Habanero.Db
{
    /// <summary>
    /// Produces connection strings that are tailored for the MySQL database
    /// </summary>
    public class ConnectionStringMySQLFactory : ConnectionStringFactory
    {
        /// <summary>
        /// Constructor to initialise a new factory
        /// </summary>
        public ConnectionStringMySQLFactory()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Checks that each of the arguments provided are valid
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        protected override void CheckArguments(string server, string database, string username, string password,
                                               string port)
        {
            if (server == "" || database == "" || username == "")
            {
                throw new ArgumentException("The server, database and username of a connect string can never be empty.");
            }
        }

        /// <summary>
        /// Creates a connection string from the arguments provided
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <returns>Returns the connection string</returns>
        /// <exception cref="ArgumentException">Thrown if any of the
        /// arguments provided are invalid</exception>
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
                    String.Format("Username={2}; Host={0}; Port={4}; Database={1}; Password={3};", server, database,
                                  username, password, port);
            }
            else
            {
                return
                    String.Format("Username={2}; Host={0}; Port={3}; Database={1};", server, database, username, port);
            }
        }
    }
}