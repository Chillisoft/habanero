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
using System.Data;

namespace Habanero.DB
{
    /// <summary>
    /// A database connection customised for the SqlServer database
    /// </summary>
    public class DatabaseConnectionSqlServer : DatabaseConnection
    {
        /// <summary>
        /// Constructor to initialise the connection object with an
        /// assembly name and class name
        /// </summary>
        /// <param name="assemblyName">The assembly name</param>
        /// <param name="className">The class name</param>
        public DatabaseConnectionSqlServer(string assemblyName, string className) : base(assemblyName, className)
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
        public DatabaseConnectionSqlServer(string assemblyName, string className, string connectString)
            : base(assemblyName, className, connectString)
        {
        } //		protected override IDbConnection GetNewConnection() {

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
            using (IDataReader reader = LoadDataReader(String.Format("SELECT IDENT_CURRENT('{0}')", tableName))) {
                if (reader.Read()) {
                    id = Convert.ToInt64(reader.GetValue(0));
                }
            }
            return id;
        }

        /// <summary>
        /// Gets the IsolationLevel <see cref="IsolationLevel"/> to use for this connection.
        /// This is set to ReadUncommitted For SQL to overcome issues with trying to load children of a business
        /// object while the children are already in the Transaction to be deleted resulting in a deadlock.
        /// </summary>
        public override IsolationLevel IsolationLevel
        {
            get { return IsolationLevel.ReadUncommitted; }
        }
    }
}
