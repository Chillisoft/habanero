using System;

namespace Habanero.Base
{
    /// <summary>
    /// An interface describing a class that creates a connection string given various parameters
    /// </summary>
    public interface IConnectionStringFactory {
        /// <summary>
        /// Returns a connection string built from the arguments provided
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The userName</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <returns>Returns the connection string</returns>
        String GetConnectionString(String server, String database, String userName, String password,
                                   String port);
    }
}