//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.DB
{
    /// <summary>
    /// Produces connection strings that are tailored for the SQLite database
    /// </summary>
    public class ConnectionStringSQLiteFactory : ConnectionStringFactory
    {
        /// <summary>
        /// Constructor to initialise a new factory
        /// </summary>
        public ConnectionStringSQLiteFactory()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
                throw new ArgumentException("The database parameter of a connect string can never be empty.");
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
            if (password == "")
            {
                return String.Format("Data Source={0};BinaryGUID=False", database);
            }
            else
            {
                return
                    String.Format("Data Source={0};Password={1};BinaryGUID=False", database, password);
            }
        }
    }
}
