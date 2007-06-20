namespace Habanero.Db
{
    /// <summary>
    /// Creates new database connections, tailored to the vendor specified
    /// in the database access configuration
    /// </summary>
    public class DatabaseConnectionFactory
    {
        /// <summary>
        /// Constructor to initialise a new factory
        /// </summary>
        public DatabaseConnectionFactory()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        //		public ConnectionStringFactory CreateConnectionStringFactory(string assemblyName) {
        //			if (assemblyName == "MySql.Data.MySqlClient") {
        //				return new ConnectionStringMySQLFactory();
        //			} else {
        //				throw new NotSupportedException(assemblyName + " is not a supported database connection assembly.");
        //			}
        //		}


        /// <summary>
        /// Creates a new database connection with the configuration
        /// provided
        /// </summary>
        /// <param name="config">The database access configuration</param>
        /// <returns>Returns a new database connection</returns>
        public DatabaseConnection CreateConnection(DatabaseConfig config)
        {
            return CreateConnection(config, "", "");
        }

        /// <summary>
        /// Creates a new database connection using the configuration
        /// provided, along with an assembly name and full class name
        /// </summary>
        /// <param name="config">The database access configuration</param>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="fullClassName">The full class name</param>
        /// <returns>Returns a new database connection</returns>
        public DatabaseConnection CreateConnection(DatabaseConfig config, string assemblyName, string fullClassName)
        {
            if (config.Vendor.ToUpper() == DatabaseConfig.MySQL.ToUpper())
            {
                if (assemblyName == "")
                {
                    return
                        new DatabaseConnectionMySQL("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection",
                                                    config.GetConnectionString());
                }
                else
                {
                    return
                        new DatabaseConnectionMySQL(assemblyName, fullClassName,
                                                    config.GetConnectionString(assemblyName));
                }
            }
            else if (config.Vendor.ToUpper() == DatabaseConfig.SQLServer.ToUpper())
            {
                if (assemblyName == "")
                {
                    return
                        new DatabaseConnectionSQLServer("System.Data", "System.Data.SqlClient.SqlConnection",
                                                        config.GetConnectionString());
                }
                else
                {
                    return new DatabaseConnectionSQLServer(assemblyName, fullClassName, config.GetConnectionString());
                }
            }
            else if (config.Vendor.ToUpper() == DatabaseConfig.Oracle.ToUpper())
            {
                if (assemblyName == "")
                {
                    return
                        new DatabaseConnectionOracle("System.Data.OracleClient", "System.Data.OracleClient.OracleConnection",
                                                     config.GetConnectionString());
//                        new DatabaseConnectionOracle("Oracle.DataAccess", "Oracle.DataAccess.Client.OracleConnection",
//                                                     config.GetConnectionString());
                }
                else
                {
                    return new DatabaseConnectionOracle(assemblyName, fullClassName, config.GetConnectionString());
                }
            }
            else if (config.Vendor.ToUpper() == DatabaseConfig.Access.ToUpper())
            {
                if (assemblyName == "")
                {
                    return
                        new DatabaseConnectionAccess("System.Data", "System.Data.OleDb.OleDbConnection",
                                                     config.GetConnectionString());
                }
                else
                {
                    return new DatabaseConnectionOracle(assemblyName, fullClassName, config.GetConnectionString());
                }
            }
            else
            {
                return null;
            }
        }
    }
}