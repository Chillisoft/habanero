using System;

namespace Chillisoft.Db.v2
{
    /// <summary>
    /// A super-class for a factory that produces connection strings for
    /// different database vendors
    /// </summary>
    public abstract class ConnectionStringFactory
    {
        /// <summary>
        /// Returns a connection string built from the arguments provided
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <returns>Returns the connection string</returns>
        public virtual String GetConnectionString(String server, String database, String username, String password,
                                                  String port)
        {
            CheckArguments(server, database, username, password, port);
            return CreateConnectionString(server, database, username, password, port);
        }

        /// <summary>
        /// Checks that each of the arguments provided are valid
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        protected abstract void CheckArguments(string server, string database, string username, string password,
                                               string port);

        /// <summary>
        /// Creates a connection string from the arguments provided
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <returns>Returns the connection string</returns>
        protected abstract string CreateConnectionString(string server, string database, string username,
                                                         string password, string port);

        /// <summary>
        /// Returns a connection string factory that is tailored to the
        /// database vendor specified
        /// </summary>
        /// <param name="vendor">The database vendor - use the string
        /// options provided under DatabaseConfig (eg. DatabaseConfig.MySQL)</param>
        /// <returns>Returns a ConnectionStringFactory object, or null
        /// if the vendor string could not be matched up</returns>
        public static ConnectionStringFactory GetFactory(string vendor)
        {
            switch (vendor.ToUpper())
            {
                case DatabaseConfig.MySQL:
                    return new ConnectionStringMySQLFactory();
                case DatabaseConfig.SQLServer:
                    return new ConnectionStringSQLServerFactory();
                case DatabaseConfig.Oracle:
                    return new ConnectionStringOracleFactory();
                case DatabaseConfig.Oracle + "_SYSTEM.DATA.ORACLECLIENT":
                    return new ConnectionStringOracleFactory();
                case DatabaseConfig.Access:
                    return new ConnectionStringAccessFactory();
                default:
                    return null;
            }
        }
    }
}