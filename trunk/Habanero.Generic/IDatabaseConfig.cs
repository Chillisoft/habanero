using System;
using System.Data;

namespace Habanero.Base
{
    /// <summary>
    /// An interface to model a class that stores database configuration 
    /// settings and creates connections using these settings
    /// </summary>
    public interface IDatabaseConfig
    {
        /// <summary>
        /// Gets and sets access to the database vendor setting
        /// </summary>
        string Vendor { get; set; }

        /// <summary>
        /// Gets and sets access to the database server setting
        /// </summary>
        string Server { get; set; }

        /// <summary>
        /// Gets and sets access to the database name setting
        /// </summary>
        string Database { get; set; }

        /// <summary>
        /// Gets and sets access to the username setting
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets and sets access to the password setting
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets and sets access to the port setting
        /// </summary>
        string Port { get; set; }

        /// <summary>
        /// Returns a connection string tailored for the database vendor,
        /// after appending an alternate assembly name
        /// </summary>
        /// <param name="alternateAssemblyName">The alternate assembly name</param>
        /// <returns>Returns a connection string</returns>
        String GetConnectionString(string alternateAssemblyName);

        /// <summary>
        /// Returns a connection string tailored for the database vendor
        /// </summary>
        /// <returns>Returns a connection string</returns>
        String GetConnectionString();

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDbConnection object</returns>
        IDbConnection GetConnection();

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored
        /// </summary>
        /// <returns>Returns an IDatabaseConnection object</returns>
        IDatabaseConnection GetDatabaseConnection();

        /// <summary>
        /// Creates a database connection using the configuration settings
        /// stored, along with the assembly name and full
        /// class name provided
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="fullClassName">The full class name</param>
        /// <returns>Returns an IDbConnection object</returns>
        IDbConnection GetConnection(string assemblyName, string fullClassName);
    }
}