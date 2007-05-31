namespace Chillisoft.Db.v2
{
    /// <summary>
    /// A database connection customised for the Oracle database
    /// </summary>
    public class DatabaseConnectionOracle : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionOracle(string assemblyName, string className)
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
        /// generated using ConnectionStringOracleFactory.CreateConnectionString()
        /// </param>
        public DatabaseConnectionOracle(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        }

        /// <summary>
        /// Returns an empty string
        /// </summary>
        public override string LeftFieldDelimiter
        {
            get { return ""; }
        }

        /// <summary>
        /// Returns an empty string
        /// </summary>
        public override string RightFieldDelimiter
        {
            get { return ""; }
        }
    }
}