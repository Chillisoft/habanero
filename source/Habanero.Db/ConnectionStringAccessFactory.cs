using System;

namespace Habanero.DB
{
    /// <summary>
    /// Produces connection strings that are tailored for the Microsoft
    /// Access database
    /// </summary>
    public class ConnectionStringAccessFactory : ConnectionStringFactory
    {
        /// <summary>
        /// Constructor to initialise a new factory
        /// </summary>
        public ConnectionStringAccessFactory()
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
        /// <param name="userName">The userName</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <exception cref="ArgumentException">Thrown if any of the
        /// arguments provided are invalid</exception>
        protected override void CheckArguments(string server, string database, string userName, string password,
                                               string port)
        {
            if (database == "") // || userName == "") 
            {
                throw new ArgumentException("The database of an access connect string can never be empty.");
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
        protected override string CreateConnectionString(string server, string database, string userName,
                                                         string password, string port)
        {
            return
                String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};User ID={1};password={2}", database,
                              userName, password);
        }
    }
}