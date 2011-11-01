#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;

namespace Habanero.DB
{
    /// <summary>
    /// Produces connection strings that are tailored for the SqlServer database
    /// </summary>
    public class ConnectionStringSqlServerCeFactory : ConnectionStringFactory
    {
        /// <summary>
        /// Checks that each of the arguments provided are valid
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The userName</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <exception cref="ArgumentException">Thrown if any of the
        /// arguments provided are invalid</exception>
        protected override void CheckArguments(string server, string database, string userName, string password,
                                               string port)
        {
            if (String.IsNullOrEmpty(database))
            {
				throw new ArgumentException("The database file name of a SQL Server Compact Edition connect string can never be empty.");
            }
        }

        /// <summary>
        /// Creates a connection string from the arguments provided
        /// </summary>
        /// <param name="server">The database server</param>
        /// <param name="database">The database name</param>
        /// <param name="userName">The userName</param>
        /// <param name="password">The password</param>
        /// <param name="port">The port</param>
        /// <returns>Returns the connection string</returns>
        protected override string CreateConnectionString(string server, string database, string userName,
                                                         string password, string port)
        {
        	const string additionalProperties = "Persist Security Info=False;SSCE:Max Database Size=4091;";
        	return String.IsNullOrEmpty(password)
				? String.Format("Data Source='{1}';{4}", server, database, userName, password, additionalProperties) 
                : String.Format("Data Source='{1}';Password={3};{4}",
									server, database, userName, password, additionalProperties);
        }
    }
}