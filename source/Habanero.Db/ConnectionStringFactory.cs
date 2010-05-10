// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using Habanero.Base;

namespace Habanero.DB
{
    /// <summary>
    /// A super-class for a factory that produces connection strings for
    /// different database vendors
    /// </summary>
    public abstract class ConnectionStringFactory : IConnectionStringFactory
    {
        /// <summary>
        /// Returns a connection string built from the arguments provided
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The userName</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <returns>Returns the connection string</returns>
        public virtual String GetConnectionString(String server, String database, String userName, String password,
                                                  String port)
        {
            CheckArguments(server, database, userName, password, port);
            return CreateConnectionString(server, database, userName, password, port);
        }

        /// <summary>
        /// Checks that each of the arguments provided are valid
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The userName</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        protected abstract void CheckArguments(string server, string database, string userName, string password,
                                               string port);

        /// <summary>
        /// Creates a connection string from the arguments provided
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The userName</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <returns>Returns the connection string</returns>
        protected abstract string CreateConnectionString(string server, string database, string userName,
                                                         string password, string port);

        ///// <summary>
        ///// Returns a connection string factory that is tailored to the
        ///// database vendor specified
        ///// </summary>
        ///// <param name="vendor">The database vendor - use the string
        ///// options provided under DatabaseConfig (eg. DatabaseConfig.MySql)</param>
        ///// <returns>Returns a ConnectionStringFactory object, or null
        ///// if the vendor string could not be matched up</returns>
        //public static ConnectionStringFactory GetFactory(string vendor)
        //{
        //    if (vendor == null) throw new ArgumentNullException("vendor");
        //    switch (vendor.ToUpper())
        //    {
        //        case DatabaseConfig.MySql:
        //            return new ConnectionStringMySqlFactory();
        //        case DatabaseConfig.SqlServer:
        //            return new ConnectionStringSqlServerFactory();
        //        case DatabaseConfig.Oracle:
        //            return new ConnectionStringOracleFactory();
        //        case DatabaseConfig.Oracle + "_SYSTEM.DATA.ORACLECLIENT":
        //            return new ConnectionStringOracleFactory();
        //        case DatabaseConfig.Access:
        //            return new ConnectionStringAccessFactory();
        //        case DatabaseConfig.PostgreSql:
        //            return new ConnectionStringPostgreSqlFactory();
        //        case DatabaseConfig.SQLite:
        //            return new ConnectionStringSQLiteFactory();
        //        case DatabaseConfig.Firebird:
        //            return new ConnectionStringFirebirdFactory(false);
        //        case DatabaseConfig.FirebirdEmbedded:
        //            return new ConnectionStringFirebirdFactory(true);                
        //        case DatabaseConfig.DB4O:
        //            return new ConnectionStringDB4OFactory();
        //        default:
        //            return null;
        //    }
        //}
    }
}