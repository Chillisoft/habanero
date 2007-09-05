namespace Habanero.DB
{
    /// <summary>
    /// A database connection customised for the SqlServer database
    /// </summary>
    public class DatabaseConnectionSqlServer : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionSqlServer(string assemblyName, string className) : base(assemblyName, className)
        {
        }

        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name, class name and connection string
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string, which can be
        /// generated using ConnectionStringSqlServerFactory.CreateConnectionString()
        /// </param>
        public DatabaseConnectionSqlServer(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        } //		protected override IDbConnection GetNewConnection() {
    }
}