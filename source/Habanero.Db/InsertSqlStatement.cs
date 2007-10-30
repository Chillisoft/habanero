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
