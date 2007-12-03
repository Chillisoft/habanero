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
using System.Data;
using System.Text;

namespace Habanero.DB
{
    
    /// <summary>
    /// A database connection customised for the SQLite database
    /// </summary>
    public class DatabaseConnectionSQLite : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionSQLite(string assemblyName, string className)
            : base(assemblyName, className)
        {
        }

        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name, class name and connection string
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        /// <param name="connectString">The connection string, which can be
        /// generated using ConnectionStringSqlServerFactory.CreateConnectionString()
        /// </param>
        public DatabaseConnectionSQLite(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        } 

        /// <summary>
        /// Gets the value of the last auto-incrementing number.  This called after doing an insert statement so that
        /// the inserted auto-number can be retrieved.  The table name, current IDbTransaction and IDbCommand are passed
        /// in so that they can be used if necessary.  
        /// </summary>
        /// <param name="tableName">The name of the table inserted into</param>
        /// <param name="tran">The current transaction, the one the insert was done in</param>
        /// <param name="command">The Command the did the insert statement</param>
        /// <returns></returns>
        public override long GetLastAutoIncrementingID(string tableName, IDbTransaction tran, IDbCommand command)
        {
            long id = 0;
            try
            {
                using (
                    IDataReader reader =
                        LoadDataReader(String.Format("Select seq from SQLITE_SEQUENCE where Upper(name) = Upper('{0}')", tableName)))
                {
                    if (reader.Read())
                    {
                        id = Convert.ToInt64(reader.GetValue(0));
                        id += 1;
                    }
                }
            } catch(Exception ex)
            {
                throw new Exception(String.Format("Please ensure that the table '{0}' has an auto incrementing field.",
                    tableName), ex);
            }
            return id;
        }


        /// <summary>
        /// Returns a double quote
        /// </summary>
        public override string LeftFieldDelimiter
        {
            get { return "\""; }
        }

        /// <summary>
        /// Returns a double quote
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
