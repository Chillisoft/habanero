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

using System.Data;
using Habanero.Base;

namespace Habanero.DB
{
    public class InsertSqlStatement : SqlStatement
    {
        private string _tableName;
        private ISupportsAutoIncrementingField _supportsAutoIncrementingFIELD;

        /// <summary>
        /// Constructor to initialise a new insert sql statement
        /// </summary>
        /// <param name="connection">The database connection used for the statement</param>
        /// <param name="statement">The statement in string form</param>
        public InsertSqlStatement(IDatabaseConnection connection, string statement) : base(connection, statement) {}

        /// <summary>
        /// Constructor to initialise a new insert sql statement
        /// </summary>
        /// <param name="connection">The database connection used for the statement</param>
        public InsertSqlStatement(IDatabaseConnection connection) : base(connection) {}

        /// <summary>
        /// The name of the table to insert into
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        /// <summary>
        /// Whether an auto-incrementing field is supported
        /// </summary>
        public ISupportsAutoIncrementingField SupportsAutoIncrementingField
        {
            get { return _supportsAutoIncrementingFIELD; }
            set { _supportsAutoIncrementingFIELD = value; }
        }

        /// <summary>
        /// Provides steps to carry out after execution of the statement
        /// </summary>
        internal override void DoAfterExecute(DatabaseConnection conn, IDbTransaction tran, IDbCommand command)
        {
            if (_supportsAutoIncrementingFIELD != null && _tableName != null) {
                _supportsAutoIncrementingFIELD.SetAutoIncrementingFieldValue(conn.GetLastAutoIncrementingID(_tableName, tran, command));
            }
        }

    }
}
