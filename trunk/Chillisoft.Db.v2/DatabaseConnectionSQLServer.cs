namespace Chillisoft.Db.v2
{
    /// <summary>
    /// A database connection customised for the SQLServer database
    /// </summary>
    public class DatabaseConnectionSQLServer : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionSQLServer(string assemblyName, string className) : base(assemblyName, className)
        {
        }

        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name, class name and connection string
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string, which can be
        /// generated using ConnectionStringSQLServerFactory.CreateConnectionString()
        /// </param>
        public DatabaseConnectionSQLServer(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        } //		protected override IDbConnection GetNewConnection() {
    }
}