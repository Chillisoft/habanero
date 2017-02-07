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
using System.Data;
using Habanero.Base;

namespace Habanero.DB
{
    ///<summary>
    /// This class inherits from <see cref="SqlStatement"/> and is used for inserting 
    ///   new objects into the database. It is used because it supports the updating of 
    ///   an autoincrementing field.
    ///</summary>
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
        /// Carries out instructions after execution of the sql statement
        /// </summary>
        /// <param name="databaseConnection">The <see cref="DatabaseConnection"/> that executed the statement.</param>
        /// <param name="transaction">The <see cref="IDbTransaction"/> under which the <see cref="SqlStatement"/>'s command was run.</param>
        /// <param name="command">The <see cref="IDbCommand"/> that was used to execute the <see cref="SqlStatement"/>.</param>
        internal override void DoAfterExecute(DatabaseConnection databaseConnection, IDbTransaction transaction, IDbCommand command)
        {
            if (_supportsAutoIncrementingFIELD != null && _tableName != null)
            {
                var lastAutoIncrementingID = databaseConnection.GetLastAutoIncrementingID(_tableName, transaction, command);
                _supportsAutoIncrementingFIELD.SetAutoIncrementingFieldValue(lastAutoIncrementingID);
            }
        }

    }
}
