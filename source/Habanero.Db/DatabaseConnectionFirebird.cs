using Habanero.DB;

namespace Habanero.DB
{
    /// <summary>
    /// A super-class to manage a database connection and execute sql commands
    /// </summary>
    public class DatabaseConnectionFirebird : DatabaseConnection
    {
        /// <summary>
        /// Constructor that allows an assembly name and class name to
        /// be specified
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The database class name</param>
        public DatabaseConnectionFirebird(string assemblyName, string className)
            : base(assemblyName, className)
        {
        }
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name, class name and connection string
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string, which can be
        /// generated using ConnectionStringMySqlFactory.CreateConnectionString()
        /// </param>
        public DatabaseConnectionFirebird(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        }

    }
}