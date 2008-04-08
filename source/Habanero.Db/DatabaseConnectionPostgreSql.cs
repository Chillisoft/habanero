//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Collections.Generic;
using System.Text;

namespace Habanero.DB
{
	/// <summary>
	/// A database connection customised for the PostgreSql database
	/// </summary>
	public class DatabaseConnectionPostgreSql : DatabaseConnection
	{
		/// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionPostgreSql(string assemblyName, string className) : base(assemblyName, className)
        {
        }

        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name, class name and connection string
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string, which can be
        /// generated using ConnectionStringPostgreSqlFactory.CreateConnectionString()
        /// </param>
		public DatabaseConnectionPostgreSql(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        }

        /// <summary>
		/// Returns a double quote character
        /// </summary>
        public override string LeftFieldDelimiter
        {
            get { return "\""; }
        }

        /// <summary>
        /// Returns a double quote character
        /// </summary>
        public override string RightFieldDelimiter
        {
            get { return "\""; }
        }

        /// <summary>
        /// Returns an empty string in this implementation
        /// </summary>
        /// <param name="limit">The limit - not relevant in this
        /// implementation</param>
        /// <returns>Returns an empty string in this implementation</returns>
        public override string GetLimitClauseForBeginning(int limit)
        {
            return "";
        }

        /// <summary>
        /// Creates a limit clause from the limit provided, in the format of:
        /// "limit [limit]" (eg. "limit 3")
        /// </summary>
        /// <param name="limit">The limit - the maximum number of rows that
        /// can be affected by the action</param>
        /// <returns>Returns a string</returns>
        public override string GetLimitClauseForEnd(int limit)
        {
            return "limit " + limit;
        }

	}
}



////Top 3 differences between PostgreSQL and MS SQL
////databases

////I recently switched a database server from Microsoft SQL Server over to PostgreSQL. Here are the top three differences in SQL:

////    * NO TOP, so SELECT TOP 10 * FROM table, becomes SELECT * FROM table LIMIT 10 you can also use the maxrows attribute of CFQUERY to do this, if you want cross db code (which is good). MySQL also uses the LIMIT sytax, but Oracle uses yet another syntax
////    * LIKE statements are case sensitive in postgresql, they can be made case insensitive like this: SELECT * FROM table WHERE LOWER(column) LIKE '%#LCase(var)#%' (Or you can use the ILIKE operator)
////    * The plus operator cannot be used for concatination so SELECT firstname + ' ' + lastname AS fullname becomes SELECT firstname || ' ' || lastname AS fullname this way works on both servers.

