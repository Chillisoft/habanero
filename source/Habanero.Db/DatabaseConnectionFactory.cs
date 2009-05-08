//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;

namespace Habanero.DB
{
    /// <summary>
    /// Creates new database connections, tailored to the vendor specified
    /// in the database access configuration
    /// </summary>
    public class DatabaseConnectionFactory
    {
        private DatabaseConnectionFactory() {}

        /// <summary>   
        /// Creates a new database connection with the configuration
        /// provided
        /// </summary>
        /// <param name="config">The database access configuration</param>
        /// <returns>Returns a new database connection</returns>
        public static DatabaseConnection CreateConnection(DatabaseConfig config)
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
        public static DatabaseConnection CreateConnection(DatabaseConfig config, string assemblyName, string fullClassName)
        {
            if (config == null) {
                throw new ArgumentNullException("config");
            }
            if (string.Compare(config.Vendor, DatabaseConfig.MySql, true) == 0)
            {
                return String.IsNullOrEmpty(assemblyName) 
                    ? new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection", config.GetConnectionString()) 
                    : new DatabaseConnectionMySql(assemblyName, fullClassName,config.GetConnectionString(assemblyName));
            }
            if (string.Compare(config.Vendor, DatabaseConfig.SqlServer, true) == 0)
            {
                return String.IsNullOrEmpty(assemblyName) 
                    ? new DatabaseConnectionSqlServer("System.Data", "System.Data.SqlClient.SqlConnection", config.GetConnectionString()) 
                    : new DatabaseConnectionSqlServer(assemblyName, fullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.Oracle, true) == 0)
            {
                return String.IsNullOrEmpty(assemblyName) 
                    ? new DatabaseConnectionOracle("System.Data.OracleClient", "System.Data.OracleClient.OracleConnection",
                                                                                         config.GetConnectionString()) 
                    : new DatabaseConnectionOracle(assemblyName, fullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.Access, true) == 0)
            {
                return String.IsNullOrEmpty(assemblyName) 
                    ? new DatabaseConnectionAccess("System.Data", "System.Data.OleDb.OleDbConnection",config.GetConnectionString()) 
                    : new DatabaseConnectionAccess(assemblyName, fullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.PostgreSql, true) == 0)
            {
                return String.IsNullOrEmpty(assemblyName) 
                    ? new DatabaseConnectionPostgreSql("Npgsql", "Npgsql.NpgsqlConnection", config.GetConnectionString()) 
                    : new DatabaseConnectionPostgreSql(assemblyName, fullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.SQLite, true) == 0)
            {
                return String.IsNullOrEmpty(assemblyName) 
                    ? new DatabaseConnectionSQLite("System.Data.SQLite", "System.Data.SQLite.SQLiteConnection", config.GetConnectionString()) 
                    : new DatabaseConnectionSQLite(assemblyName, fullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.Firebird, true) == 0)
            {
                return String.IsNullOrEmpty(assemblyName) 
                    ? new DatabaseConnectionFirebird("FirebirdSql.Data.FirebirdClient", "FirebirdSql.Data.FirebirdClient.FbConnection",
                                                                                           config.GetConnectionString()) 
                    : new DatabaseConnectionFirebird(assemblyName, fullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.FirebirdEmbedded, true) == 0)
            {
                return String.IsNullOrEmpty(assemblyName) 
                    ? new DatabaseConnectionFirebird("FirebirdSql.Data.FirebirdClient", "FirebirdSql.Data.FirebirdClient.FbConnection",
                                                                                           config.GetConnectionString()) 
                    : new DatabaseConnectionFirebird(assemblyName, fullClassName, config.GetConnectionString());
            }
            return null;
        }
    }
}