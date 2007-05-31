using System;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// Produces connection strings that are tailored for the Oracle database
    /// </summary>
    public class ConnectionStringOracleFactory : ConnectionStringFactory
    {
        /// <summary>
        /// Constructor to initialise a new factory
        /// </summary>
        public ConnectionStringOracleFactory()
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
            if (database == "" || username == "")
            {
                throw new ArgumentException("The database and username of an Oracle connect string can never be empty.");
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
            if (password == "")
            {
                return String.Format("Data Source={0};user ID={1};", database, username);
            }
            else
            {
                return String.Format("Data Source={0};user ID={1};Password={2};", database, username, password);
            }
        }
    }
}