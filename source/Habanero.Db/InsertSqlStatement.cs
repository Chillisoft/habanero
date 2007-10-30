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
using Habanero.Base;

namespace Habanero.DB
{
    public class InsertSqlStatement : SqlStatement {
        private string _tableName;
        private ISupportsAutoIncrementingField _supportsAutoIncrementingFIELD;

        public InsertSqlStatement(IDbConnection connection, string statement) : base(connection, statement) {}
        public InsertSqlStatement(IDbConnection connection) : base(connection) {}

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public ISupportsAutoIncrementingField SupportsAutoIncrementingField
        {
            get { return _supportsAutoIncrementingFIELD; }
            set { _supportsAutoIncrementingFIELD = value; }
        }


        internal override void DoAfterExecute(DatabaseConnection conn, IDbTransaction tran, IDbCommand command)
        {
            if (_supportsAutoIncrementingFIELD != null && _tableName != null) {
                _supportsAutoIncrementingFIELD.SetAutoIncrementingFieldValue(conn.GetLastAutoIncrementingID(_tableName, tran, command));
            }
        }

    }
}
