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
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.DB;
using Habanero.Util;

namespace Habanero.BO
{
    ///<summary>
    /// A class representing a Database SelectQuery.  Wraps an ISelectQuery (Decorator pattern)
    ///</summary>
    public class SelectQueryDB : ISelectQuery
    {
        private readonly ISelectQuery _selectQuery;
        private SqlFormatter _sqlFormatter;

        ///<summary>
        /// Creates a SelectQueryDB, wrapping an ISelectQuery (Decorator pattern)
        ///</summary>
        ///<param name="selectQuery"></param>
        public SelectQueryDB(ISelectQuery selectQuery)
        {
            _selectQuery = selectQuery;
        }

        #region ISelectQuery Members

        /// <summary>
        /// The Criteria to use when loading. Only objects that match these criteria will be loaded.
        /// </summary>
        public Criteria Criteria
        {
            get { return _selectQuery.Criteria; }
            set { _selectQuery.Criteria = value; }
        }

        /// <summary>
        /// The fields to load from the data store.
        /// </summary>
        public Dictionary<string, QueryField> Fields
        {
            get { return _selectQuery.Fields; }
        }

        /// <summary>
        /// The source of the data. In a database query this would be the first table listed in the FROM clause.
        /// </summary>
        public Source Source
        {
            get { return _selectQuery.Source; }
            set { _selectQuery.Source = value; }
        }

        /// <summary>
        /// The fields to use to order a collection of objects when loading them.
        /// </summary>
        public OrderCriteria OrderCriteria
        {
            get { return _selectQuery.OrderCriteria; }
            set { _selectQuery.OrderCriteria = value; }
        }

        /// <summary>
        /// The number of objects to load
        /// </summary>
        public int Limit
        {
            get { return _selectQuery.Limit; }
            set { _selectQuery.Limit = value; }
        }

        /// <summary>
        /// The classdef this select query corresponds to. This can be null if the select query is being used
        /// without classdefs, but if it is built using the QueryBuilder 
        /// </summary>
        public IClassDef ClassDef
        {
            get { return _selectQuery.ClassDef; }
            set { _selectQuery.ClassDef = value; }
        }

        ///<summary>
        ///</summary>
        public Criteria DiscriminatorCriteria
        {
            get { return _selectQuery.DiscriminatorCriteria; }
            set { _selectQuery.DiscriminatorCriteria = value; }
        }

        ///<summary>
        /// Gets and sets the first record to be loaded by the select query.
        ///</summary>
        public int FirstRecordToLoad
        {
            get { return _selectQuery.FirstRecordToLoad; }
            set { _selectQuery.FirstRecordToLoad = value; }
        }

        #endregion

        /// <summary>
        /// Creates an ISqlStatement out of the SelectQuery given in the constructor, to be used to load 
        /// from a database
        /// </summary>
        /// <returns>An ISqlStatement that can be executed against an IDatabaseConnection</returns>
        public ISqlStatement CreateSqlStatement()
        {
            IDatabaseConnection databaseConnection = DatabaseConnection.CurrentConnection;

            if (databaseConnection == null)
            {
                throw new HabaneroDeveloperException("The Sql cannot be created because the database connection is not set up. Please contact your system administrator", "");
            }

            return CreateSqlStatement(databaseConnection.SqlFormatter);
//            return CreateSqlStatement(new SqlFormatter(databaseConnection.LeftFieldDelimiter, databaseConnection.RightFieldDelimiter, databaseConnection.GetLimitClauseForBeginning()));
        }

        /// <summary>
        /// Creates an ISqlStatement out of the SelectQuery given in the constructor, to be used to load 
        /// from a database
        /// </summary>
        /// <returns>An ISqlStatement that can be executed against an IDatabaseConnection</returns>
        public ISqlStatement CreateSqlStatement(SqlFormatter sqlFormatter)
        {
            _sqlFormatter = sqlFormatter;
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection);

            StringBuilder builder = statement.Statement;

            if (this.FirstRecordToLoad > 0)
            {
                builder.Append("SELECT ");
                foreach (QueryField field in this.Fields.Values)
                {
                    builder.AppendFormat("{0}, ", DelimitField("SecondSelect", field.FieldName));
                } 
                builder.Remove(builder.Length - 2, 2);
//                AppendFields(builder);
                builder.Append(" FROM (SELECT ");
                AppendNoOfRecordsClauseAtBeginning(builder);
                foreach (QueryField field in this.Fields.Values)
                {
                    builder.AppendFormat("{0}, ", DelimitField("FirstSelect", field.FieldName));
                } 
                builder.Remove(builder.Length - 2, 2);
//                AppendFields(builder);
                builder.Append(" FROM (");
            }
            builder.Append("SELECT ");
            AppendLimitClauseAtBeginning(builder);
            AppendFields(builder);
            AppendFrom(builder);
            AppendWhereClause(builder, statement);
            AppendOrderByClause(builder);
            AppendLimitClauseAtEnd(builder);
            //" ORDER BY SecondSelect.MyBOID ASC";
            if (this.FirstRecordToLoad > 0)
            {
                builder.Append(") As FirstSelect");
                builder.Append(" ORDER BY ");
                foreach (OrderCriteria.Field field in this.OrderCriteria.Fields)
                {
                    builder.Append("FirstSelect.").Append(field.FieldName);
                    if (field.SortDirection == OrderCriteria.SortDirection.Ascending)
                    {
                        builder.Append(" DESC");
                    } else
                    {
                        builder.Append(" ASC");
                    }
                }
                builder.Append(" ");
                AppendNoOfRecordsClauseAtEnd(builder);
                builder.Append(") AS SecondSelect");
                builder.Append(" ORDER BY ");
                foreach (OrderCriteria.Field field in this.OrderCriteria.Fields)
                {
                    builder.Append("SecondSelect.").Append(field.FieldName);
                    if (field.SortDirection == OrderCriteria.SortDirection.Ascending)
                    {
                        builder.Append(" ASC");
                    }
                    else
                    {
                        builder.Append(" DESC");
                    }
                }
            }
            return statement;
        }

        private void AppendNoOfRecordsClauseAtEnd(StringBuilder builder)
        {
            string limitClauseAtEnd = _sqlFormatter.GetLimitClauseCriteriaForEnd(this.Limit);
            if (!String.IsNullOrEmpty(limitClauseAtEnd))
            {
                builder.Append(limitClauseAtEnd + " ");
            }
        }

        private void AppendFrom(StringBuilder builder)
        {
            SourceDB source = new SourceDB(_selectQuery.Source);
            builder.AppendFormat(" FROM {0}", source.CreateSQL(_sqlFormatter));
            if (_selectQuery.OrderCriteria == null) return;
        }


        private void AppendFields(StringBuilder builder)
        {
            //QueryBuilder.IncludeFieldsFromOrderCriteria(_selectQuery);
            foreach (QueryField field in _selectQuery.Fields.Values)
            {
                builder.AppendFormat("{0}, ", DelimitField(field.Source, field.FieldName));
            }
            builder.Remove(builder.Length - 2, 2);
        }


        private void AppendNoOfRecordsClauseAtBeginning(StringBuilder builder)
        {
            string limitClauseAtBeginning = _sqlFormatter.GetLimitClauseCriteriaForBegin(this.Limit);
            if (!String.IsNullOrEmpty(limitClauseAtBeginning))
            {
                builder.Append(limitClauseAtBeginning + " ");
            }
        }

        private void AppendLimitClauseAtBeginning(StringBuilder builder)
        {
            if (_selectQuery.Limit < 0) return;

//            string limitClauseAtBeginning =
//                DatabaseConnection.CurrentConnection.GetLimitClauseForBeginning(_selectQuery.Limit);
            string limitClauseAtBeginning = _sqlFormatter.GetLimitClauseCriteriaForBegin(this.Limit + this.FirstRecordToLoad);
            if (!String.IsNullOrEmpty(limitClauseAtBeginning))
            {
                builder.Append(limitClauseAtBeginning + " ");
            }
        }

        private void AppendLimitClauseAtEnd(StringBuilder builder)
        {
            if (_selectQuery.Limit < 0) return;

//            string limitClauseAtEnd = DatabaseConnection.CurrentConnection.GetLimitClauseForEnd(_selectQuery.Limit);
            string limitClauseAtEnd = _sqlFormatter.GetLimitClauseCriteriaForEnd(this.Limit + this.FirstRecordToLoad);
            if (!String.IsNullOrEmpty(limitClauseAtEnd))
            {
                builder.Append(" " + limitClauseAtEnd);
            }
        }

        private void AppendOrderByClause(StringBuilder builder)
        {
            if (_selectQuery.OrderCriteria == null || _selectQuery.OrderCriteria.Fields.Count == 0) return;

            builder.Append(" ORDER BY ");
            StringBuilder orderByClause = new StringBuilder();
            foreach (OrderCriteria.Field orderField in _selectQuery.OrderCriteria.Fields)
            {
                AppendOrderByField(orderByClause, orderField);
            }

            builder.Append(orderByClause.ToString());
        }

        private void AppendOrderByField(StringBuilder orderByClause, OrderCriteria.Field orderField)
        {
            string direction = orderField.SortDirection == OrderCriteria.SortDirection.Ascending ? "ASC" : "DESC";

            string tableAndFieldName;
            if (orderField.Source != null)
            {
                tableAndFieldName = DelimitField(orderField.Source.ChildSourceLeaf, orderField.FieldName);
            }
            else
            {
                if (Fields.ContainsKey(orderField.PropertyName))
                {
                    QueryField queryField = Fields[orderField.PropertyName];
                    tableAndFieldName = DelimitField(queryField.Source, queryField.FieldName);
                } else
                {
                    tableAndFieldName = DelimitField(this.Source, orderField.FieldName);
                }
            }
            StringUtilities.AppendMessage(orderByClause, tableAndFieldName + " " + direction, ", ");
        }

        private void AppendWhereClause(StringBuilder builder, SqlStatement statement)
        {
            Criteria fullCriteria = Criteria.MergeCriteria(_selectQuery.Criteria, _selectQuery.DiscriminatorCriteria);

            if (fullCriteria == null) return;
            builder.Append(" WHERE ");
            CriteriaDB criteriaDB = new CriteriaDB(fullCriteria);
            string whereClause =
                criteriaDB.ToString(_sqlFormatter, delegate(object value)
                       {
                           string paramName = statement.ParameterNameGenerator.GetNextParameterName();
                           if (value == null) value = "NULL";
                           if (value is DateTimeToday)
                           {
                               value = DateTimeToday.Value;
                           }
                           if (value is DateTimeNow)
                           {
                               value = DateTimeNow.Value;
                           }
                           statement.AddParameter(paramName, value);
                           return paramName;
                       });

            builder.Append(whereClause);
        }
        private string DelimitField(Source source, string fieldName)
        {
            return source == null ? _sqlFormatter.DelimitField(fieldName) : DelimitField(source.EntityName, fieldName);
        }

        private string DelimitField(string entityName, string fieldName)
        {
            if (string.IsNullOrEmpty(entityName)) return _sqlFormatter.DelimitField(fieldName);
            return string.Format("{0}.{1}", _sqlFormatter.DelimitTable(entityName), _sqlFormatter.DelimitField(fieldName));
        }
    }
}
