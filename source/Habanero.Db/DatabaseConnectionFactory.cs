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
using Habanero.Base;

namespace Habanero.DB
{
    public interface IDatabaseConnectionFactory {
        /// <summary>   
        /// Creates a new database connection with the configuration
        /// provided
        /// </summary>
        /// <param name="config">The database access configuration</param>
        /// <returns>Returns a new database connection</returns>
        IDatabaseConnection CreateConnection(IDatabaseConfig config);
    }

    /// <summary>
    /// Creates new database connections, tailored to the vendor specified
    /// in the database access configuration
    /// </summary>
    public class DatabaseConnectionFactory : IDatabaseConnectionFactory
    {
        /// <summary>   
        /// Creates a new database connection with the configuration
        /// provided
        /// </summary>
        /// <param name="config">The database access configuration</param>
        /// <returns>Returns a new database connection</returns>
        public IDatabaseConnection CreateConnection(IDatabaseConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (string.Compare(config.Vendor, DatabaseConfig.MySql, true) == 0)
            {
                return String.IsNullOrEmpty(config.AssemblyName)
                    ? new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection", config.GetConnectionString())
                    : new DatabaseConnectionMySql(config.AssemblyName, config.FullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.SqlServer, true) == 0)
            {
                return String.IsNullOrEmpty(config.AssemblyName)
                    ? new DatabaseConnectionSqlServer("System.Data", "System.Data.SqlClient.SqlConnection", config.GetConnectionString())
                    : new DatabaseConnectionSqlServer(config.AssemblyName, config.FullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.Oracle, true) == 0)
            {
                return String.IsNullOrEmpty(config.AssemblyName)
                    ? new DatabaseConnectionOracle("System.Data.OracleClient, Version=2.0.0.0, Culture=neutral,PublicKeyToken=b77a5c561934e089", "System.Data.OracleClient.OracleConnection",
                                                                                         config.GetConnectionString())
                    : new DatabaseConnectionOracle(config.AssemblyName, config.FullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.Access, true) == 0)
            {
                return String.IsNullOrEmpty(config.AssemblyName)
                    ? new DatabaseConnectionAccess("System.Data", "System.Data.OleDb.OleDbConnection", config.GetConnectionString())
                    : new DatabaseConnectionAccess(config.AssemblyName, config.FullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.PostgreSql, true) == 0)
            {
                return String.IsNullOrEmpty(config.AssemblyName)
                    ? new DatabaseConnectionPostgreSql("Npgsql", "Npgsql.NpgsqlConnection", config.GetConnectionString())
                    : new DatabaseConnectionPostgreSql(config.AssemblyName, config.FullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.SQLite, true) == 0)
            {
                return String.IsNullOrEmpty(config.AssemblyName)
                    ? new DatabaseConnectionSQLite("System.Data.SQLite", "System.Data.SQLite.SQLiteConnection", config.GetConnectionString())
                    : new DatabaseConnectionSQLite(config.AssemblyName, config.FullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.Firebird, true) == 0)
            {
                return String.IsNullOrEmpty(config.AssemblyName)
                    ? new DatabaseConnectionFirebird("FirebirdSql.Data.FirebirdClient", "FirebirdSql.Data.FirebirdClient.FbConnection",
                                                                                           config.GetConnectionString())
                    : new DatabaseConnectionFirebird(config.AssemblyName, config.FullClassName, config.GetConnectionString());
            }
            if (string.Compare(config.Vendor, DatabaseConfig.FirebirdEmbedded, true) == 0)
            {
                return String.IsNullOrEmpty(config.AssemblyName)
                    ? new DatabaseConnectionFirebird("FirebirdSql.Data.FirebirdClient", "FirebirdSql.Data.FirebirdClient.FbConnection",
                                                                                           config.GetConnectionString())
                    : new DatabaseConnectionFirebird(config.AssemblyName, config.FullClassName, config.GetConnectionString());
            }
            return null;
        } 
    }
}