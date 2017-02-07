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
using System.Data;
using Habanero.Base;

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
            _sqlFormatter = new SqlFormatter("\"", "\"", "", "limit");
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
            _sqlFormatter = new SqlFormatter("\"", "\"", "", "limit");
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
                using (IDataReader reader = LoadDataReader(String.Format("Select max(ROWID) from {0}", tableName), tran))
                    //IDataReader reader =
                    //    LoadDataReader(String.Format("Select seq from SQLITE_SEQUENCE where Upper(name) = Upper('{0}')", tableName), tran))
                {
                    if (reader.Read())
                    {
                        id = Convert.ToInt64(reader.GetValue(0));
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
        /// Creates an <see cref="IParameterNameGenerator"/> for this database connection.  This is used to create names for parameters
        /// added to an <see cref="ISqlStatement"/> because each database uses a different naming convention for their parameters.
        /// </summary>
        /// <returns>The <see cref="IParameterNameGenerator"/> valid for this <see cref="IDatabaseConnection"/></returns>
        public override IParameterNameGenerator CreateParameterNameGenerator()
        {
            return new ParameterNameGenerator(":");
        }

        /// <summary>
        /// SQLLite uses the Serializable IsolationLevel.
        /// </summary>
        public override IsolationLevel IsolationLevel
        {
            get { return System.Data.IsolationLevel.Serializable; }
        }

    }
}
