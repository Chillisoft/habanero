using System;

namespace Habanero.DB
{
    /// <summary>
    /// Produces connection strings that are tailored for a Firebird database
    /// </summary>
    public class ConnectionStringFirebirdFactory : ConnectionStringFactory
    {
        private readonly string _server;

        /// <summary>
        /// Constructor to initialise a new factory
        /// </summary>
        public ConnectionStringFirebirdFactory(bool isEmbeddedServer)
        {
            _server = isEmbeddedServer ? "1" : "0";
        }

        /// <summary>
        /// Checks that each of the arguments provided are valid
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The userName</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <exception cref="ArgumentException">Thrown if any of the
        /// arguments provided are invalid</exception>
        protected override void CheckArguments(string server, string database, string userName, string password, string port)
        {
            if (String.IsNullOrEmpty(server) || String.IsNullOrEmpty(database) || String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            {
                throw new ArgumentException("The server, database, password and userName of a connect string can never be empty.");
            }
        }

        /// <summary>
        /// Creates a connection string from the arguments provided
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The userName</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <returns>Returns the connection string</returns>
        protected override string CreateConnectionString(string server, string database, string userName, string password, string port)
        {
            return String.Format("Server={0};User={1};Password={2};Database={3};ServerType={4}", server,userName,password,database,_server);
        }
    }
}