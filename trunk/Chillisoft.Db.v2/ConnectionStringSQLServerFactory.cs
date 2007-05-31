using System;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// Produces connection strings that are tailored for the SQLServer database
    /// </summary>
    public class ConnectionStringSQLServerFactory : ConnectionStringFactory
    {
        /// <summary>
        /// Constructor to initialise a new factory
        /// </summary>
        public ConnectionStringSQLServerFactory()
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
        /// <exception cref="ArgumentException">Thrown if any of the
        /// arguments provided are invalid</exception>
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
        protected override string CreateConnectionString(string server, string database, string username,
                                                         string password, string port)
        {
            if (password == "")
            {
                return String.Format("Server={0};Initial Catalog={1};User ID={2};", server, database, username);
            }
            else
            {
                return
                    String.Format("Server={0};Initial Catalog={1};User ID={2};password={3};", server, database, username,
                                  password);
            }
        }
    }
}