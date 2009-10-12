using System.Data;
using Habanero.Base;

namespace Habanero.DB
{
    /// <summary>
    /// A database connection customised for the Microsoft Access database
    /// </summary>
    public class DatabaseConnectionAccess2007 : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionAccess2007(string assemblyName, string className) : base(assemblyName, className)
        {
            _sqlFormatter = new Base.SqlFormatter("[", "]", "TOP", "");
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
        public DatabaseConnectionAccess2007(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
            _sqlFormatter = new SqlFormatter("[", "]", "TOP", "");
        }

        /// <summary>
        /// Gets the IsolationLevel to use for this connection
        /// </summary>
        public override IsolationLevel IsolationLevel
        {
            get { return IsolationLevel.ReadUncommitted; }
        }

        /// <summary>
        /// Creates an <see cref="IParameterNameGenerator"/> for this database connection.  This is used to create names for parameters
        /// added to an <see cref="ISqlStatement"/> because each database uses a different naming convention for their parameters.
        /// </summary>
        /// <returns>The <see cref="IParameterNameGenerator"/> valid for this <see cref="IDatabaseConnection"/></returns>
        public override IParameterNameGenerator CreateParameterNameGenerator() {
            return new ParameterNameGenerator("@");
        }
    }
}