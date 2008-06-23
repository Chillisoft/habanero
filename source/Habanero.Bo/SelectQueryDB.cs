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

        public int Limit
        {
            get { return _selectQuery.Limit;  }
            set { _selectQuery.Limit = value; }
        }

        #endregion

        public ISqlStatement CreateSqlStatement()
        {
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection);
            StringBuilder builder = statement.Statement.Append("SELECT ");
            AppendLimitClauseAtBeginning(builder);
            AppendFields(builder);
            AppendFrom(builder);
            AppendWhereClause(builder, statement);
            AppendOrderByClause(builder);
            AppendLimitClauseAtEnd(builder);
            return statement;
        }

        private void AppendFrom(StringBuilder builder)
        {
            builder.Append(" FROM ");
            builder.Append(SqlFormattingHelper.FormatTableName(_selectQuery.Source, DatabaseConnection.CurrentConnection));
        }

        private void AppendFields(StringBuilder builder)
        {
            foreach (QueryField field in _selectQuery.Fields.Values)
            {
                builder.Append(GetFieldNameWithDelimiters(field.SourceName, field.FieldName) + ", ");
            }
            builder.Remove(builder.Length - 2, 2);
        }

        private void AppendLimitClauseAtBeginning(StringBuilder builder)
        {
            if (_selectQuery.Limit > 0)
            {
                string limitClauseAtBeginning =
                    DatabaseConnection.CurrentConnection.GetLimitClauseForBeginning(_selectQuery.Limit);
                if (!String.IsNullOrEmpty(limitClauseAtBeginning))
                {
                    builder.Append(limitClauseAtBeginning + " ");
                }
            }
        }

        private void AppendLimitClauseAtEnd(StringBuilder builder)
        {
            if (_selectQuery.Limit > 0)
            {
                string limitClauseAtEnd =
                    DatabaseConnection.CurrentConnection.GetLimitClauseForEnd(_selectQuery.Limit);
                if (!String.IsNullOrEmpty(limitClauseAtEnd))
                {
                    builder.Append(" " + limitClauseAtEnd);
                }
            }
        }

        private void AppendOrderByClause(StringBuilder builder)
        {
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
        }

        private void AppendWhereClause(StringBuilder builder, SqlStatement statement)
        {
            if (_selectQuery.Criteria != null)
            {
                builder.Append(" WHERE ");
                string whereClause =
                    _selectQuery.Criteria.ToString(delegate(string propName)
                    {
                        QueryField queryField = _selectQuery.Fields[propName];
                        return GetFieldNameWithDelimiters(queryField.SourceName, queryField.FieldName);
                    }, delegate(object value)
                    {
                        string paramName = statement.ParameterNameGenerator.GetNextParameterName();
                        statement.AddParameter(paramName, value);
                        return paramName;
                        
                    });
                builder.Append(whereClause);
            }
        }

        private string GetFieldNameWithDelimiters(string sourceName, string fieldName)
        {

            if (string.IsNullOrEmpty(sourceName)) return SqlFormattingHelper.FormatFieldName(fieldName, DatabaseConnection.CurrentConnection);
            return SqlFormattingHelper.FormatTableAndFieldName(sourceName, fieldName, DatabaseConnection.CurrentConnection);
        }

    }
}