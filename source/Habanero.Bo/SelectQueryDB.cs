using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using Habanero.Util;

namespace Habanero.BO
{
    public class SelectQueryDB : ISelectQuery
    {
        private readonly SelectQuery _selectQuery;

        public SelectQueryDB(SelectQuery selectQuery) 
        {
            _selectQuery = selectQuery;
        }

        #region ISelectQuery Members

        Criteria ISelectQuery.Criteria
        {
            get { return _selectQuery.Criteria; }
            set { _selectQuery.Criteria = value; }
        }

        Dictionary<string, QueryField> ISelectQuery.Fields
        {
            get { return _selectQuery.Fields; }
        }

        /// <summary>
        /// The source of the data. In a database query this would be the first table listed in the FROM clause.
        /// </summary>
        string ISelectQuery.Source
        {
            get { return _selectQuery.Source; }
            set { _selectQuery.Source = value; }
        }

        OrderCriteria ISelectQuery.OrderCriteria
        {
            get { return _selectQuery.OrderCriteria; }
            set { _selectQuery.OrderCriteria = value; }
        }

        #endregion

        public ISqlStatement CreateSqlStatement()
        {
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection);
            StringBuilder builder = statement.Statement.Append("SELECT ");
            foreach (QueryField field in _selectQuery.Fields.Values)
            {
                builder.Append(field.FieldName + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append(" FROM ");
            builder.Append(_selectQuery.Source);
            if (_selectQuery.Criteria != null)
            {
                builder.Append(" WHERE ");
                builder.Append(_selectQuery.Criteria.ToString(delegate(string propName) { return _selectQuery.Fields[propName].FieldName; }));
            }
            if (_selectQuery.OrderCriteria != null)
            {
                builder.Append(" ORDER BY ");
                StringBuilder orderByClause = new StringBuilder();
                foreach (string orderField in _selectQuery.OrderCriteria.Fields)
                {
                    StringUtilities.AppendMessage(orderByClause, _selectQuery.Fields[orderField].FieldName, ", ");
                }
                builder.Append(orderByClause.ToString());
            }
            return statement;
        }
    }
}