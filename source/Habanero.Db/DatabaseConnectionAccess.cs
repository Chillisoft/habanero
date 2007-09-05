namespace Habanero.DB
{
    /// <summary>
    /// A database connection customised for the Microsoft Access database
    /// </summary>
    public class DatabaseConnectionAccess : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionAccess(string assemblyName, string className) : base(assemblyName, className)
        {
        }

        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name, class name and connection string
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string, which can be
        /// generated using ConnectionStringAccessFactory.CreateConnectionString()
        /// </param>
        public DatabaseConnectionAccess(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        }
    }
}