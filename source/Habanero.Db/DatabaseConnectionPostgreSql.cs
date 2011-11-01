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
using Habanero.Base;

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
            _sqlFormatter = new SqlFormatter("\"", "\"", "", "limit");
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
            _sqlFormatter = new SqlFormatter("\"", "\"", "", "limit");
        }


	    /// <summary>
	    /// Creates an <see cref="IParameterNameGenerator"/> for this database connection.  This is used to create names for parameters
	    /// added to an <see cref="ISqlStatement"/> because each database uses a different naming convention for their parameters.
	    /// </summary>
	    /// <returns>The <see cref="IParameterNameGenerator"/> valid for this <see cref="IDatabaseConnection"/></returns>
	    public override IParameterNameGenerator CreateParameterNameGenerator()
        {
            return new ParameterNameGenerator(":");
        }
	}
}



////Top 3 differences between PostgreSQL and MS SQL
////databases

////I recently switched a database server from Microsoft SQL Server over to PostgreSQL. Here are the top three differences in SQL:

////    * NO TOP, so SELECT TOP 10 * FROM table, becomes SELECT * FROM table LIMIT 10 you can also use the maxrows attribute of CFQUERY to do this, if you want cross db code (which is good). MySQL also uses the LIMIT sytax, but Oracle uses yet another syntax
////    * LIKE statements are case sensitive in postgresql, they can be made case insensitive like this: SELECT * FROM table WHERE LOWER(column) LIKE '%#LCase(var)#%' (Or you can use the ILIKE operator)
////    * The plus operator cannot be used for concatination so SELECT firstname + ' ' + lastname AS fullname becomes SELECT firstname || ' ' || lastname AS fullname this way works on both servers.

