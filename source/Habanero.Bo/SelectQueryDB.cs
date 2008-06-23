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
        private readonly ISelectQuery _selectQuery;

        public SelectQueryDB(ISelectQuery selectQuery) 
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
                builder.Append(GetFieldNameWithDelimiters(field.SourceName, field.FieldName) + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
            builder.Append(" FROM ");
            builder.Append(GetFieldNameWithDelimiters(_selectQuery.Source));
            if (_selectQuery.Criteria != null)
            {
                builder.Append(" WHERE ");
                string whereClause =
                    _selectQuery.Criteria.ToString(delegate(string propName)
                    {
                        QueryField queryField = _selectQuery.Fields[propName];
                        return GetFieldNameWithDelimiters(queryField.SourceName, queryField.FieldName);
                    });
                builder.Append(whereClause);
            }
            if (_selectQuery.OrderCriteria != null)
            {
                builder.Append(" ORDER BY ");
                StringBuilder orderByClause = new StringBuilder();
                foreach (OrderCriteria.Field orderField in _selectQuery.OrderCriteria.Fields)
                {
                    string direction = orderField.SortDirection == OrderCriteria.SortDirection.Ascending
                                           ? "ASC"
                                           : "DESC";
                    QueryField queryField = _selectQuery.Fields[orderField.Name];
                    StringUtilities.AppendMessage(orderByClause, GetFieldNameWithDelimiters(queryField.SourceName, queryField.FieldName) + " " + direction, ", ");
                }
           
                builder.Append(orderByClause.ToString());
            }
            return statement;
        }

        private string GetFieldNameWithDelimiters(string sourceName, string fieldName)
        {
            if (string.IsNullOrEmpty(sourceName)) return GetFieldNameWithDelimiters(fieldName);
            return GetFieldNameWithDelimiters(sourceName) + "." + GetFieldNameWithDelimiters(fieldName);
        }

        protected virtual string GetFieldNameWithDelimiters(string fieldName)
        {
            return
                DatabaseConnection.CurrentConnection.LeftFieldDelimiter + fieldName +
                DatabaseConnection.CurrentConnection.RightFieldDelimiter;
            
        }
    }
}