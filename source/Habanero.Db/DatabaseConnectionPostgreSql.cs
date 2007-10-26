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

