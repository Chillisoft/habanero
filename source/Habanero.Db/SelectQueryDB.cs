// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using System.Linq;

namespace Habanero.DB
{
    ///<summary>
    /// A class representing a Database SelectQuery.  Wraps an ISelectQuery (Decorator pattern)
    ///</summary>
    public class SelectQueryDB : ISelectQuery
    {
        private readonly ISelectQuery _selectQuery;
        private readonly IDatabaseConnection _databaseConnection;
        private ISqlFormatter _sqlFormatter;

        ///<summary>
        /// Creates a SelectQueryDB, wrapping an ISelectQuery (Decorator pattern)
        ///</summary>
        ///<param name="selectQuery"></param>
        ///<param name="databaseConnection"></param>
        public SelectQueryDB(ISelectQuery selectQuery, IDatabaseConnection databaseConnection)
        {
            _selectQuery = selectQuery;
            _databaseConnection = databaseConnection;
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
        public IOrderCriteria OrderCriteria
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

        public IDictionary<string, string> Aliases
        {
            get { return _selectQuery.Aliases; }
        }

        #endregion

        /// <summary>
        /// Creates an ISqlStatement out of the SelectQuery given in the constructor, to be used to load 
        /// from a database
        /// </summary>
        /// <returns>An ISqlStatement that can be executed against an IDatabaseConnection</returns>
        public ISqlStatement CreateSqlStatement()
        {
            if (_databaseConnection == null)
            {
                throw new HabaneroDeveloperException("The Sql cannot be created because the database connection is not set up. Please contact your system administrator", "");
            }

            return CreateSqlStatement(_databaseConnection.SqlFormatter);
//            return CreateSqlStatement(new SqlFormatter(databaseConnection.LeftFieldDelimiter, databaseConnection.RightFieldDelimiter, databaseConnection.GetLimitClauseForBeginning()));
        }

        /// <summary>
        /// Creates an ISqlStatement out of the SelectQuery given in the constructor, to be used to load 
        /// from a database
        /// </summary>
        /// <returns>An ISqlStatement that can be executed against an IDatabaseConnection</returns>
        public ISqlStatement CreateSqlStatement(ISqlFormatter sqlFormatter)
        {
            _sqlFormatter = sqlFormatter;
            SqlStatement statement = new SqlStatement(_databaseConnection);
            StringBuilder builder = statement.Statement;
            CheckRecordOffSetAndAppendFields(builder);
            AppendMainSelectClause(statement, builder);
            if (this.FirstRecordToLoad > 0)
            {
                AppendOrderByFirstSelect(builder);
                builder.Append(" ");
                AppendNoOfRecordsClauseAtEnd(builder);
                AppendOrderBySecondSelect(builder);
            }
            return statement;
        }

        private void AppendMainSelectClause(SqlStatement statement, StringBuilder builder)
        {
            builder.Append("SELECT ");
            AppendLimitClauseAtBeginning(builder);
            AppendFields(builder);
            AppendFrom(builder);
            AppendWhereClause(builder, statement);
            AppendOrderByClause(builder);
            AppendLimitClauseAtEnd(builder);
        }

        private void CheckRecordOffSetAndAppendFields(StringBuilder builder)
        {
            if (this.FirstRecordToLoad > 0)
            {
                builder.Append("SELECT ");
                foreach (QueryField field in this.Fields.Values)
                {
                    builder.AppendFormat("{0}, ", DelimitField("SecondSelect", field.FieldName));
                } 
                builder.Remove(builder.Length - 2, 2);
                builder.Append(" FROM (SELECT ");
                AppendNoOfRecordsClauseAtBeginning(builder);
                foreach (QueryField field in this.Fields.Values)
                {
                    builder.AppendFormat("{0}, ", DelimitField("FirstSelect", field.FieldName));
                } 
                builder.Remove(builder.Length - 2, 2);
                builder.Append(" FROM (");
            }
        }

        private void AppendOrderBySecondSelect(StringBuilder builder)
        {
/*            builder.Append(") AS " + DelimitTable("SecondSelect"));
            builder.Append(" ORDER BY ");
            foreach (OrderCriteriaField field in this.OrderCriteria.Fields)
            {
                string orderByFieldName = GetOrderByFieldName(field);
                builder.Append(DelimitField("SecondSelect", orderByFieldName));
                if (field.SortDirection == SortDirection.Ascending)
                {
                    AppendAsc(builder);
                }
                else
                {
                    AppendDesc(builder);
                }
            }*/
            AppendOrderBy(builder, "SecondSelect", true);
        }

        private static void AppendDesc(StringBuilder builder)
        {
            builder.Append(" DESC");
        }

        private static void AppendAsc(StringBuilder builder)
        {
            builder.Append(" ASC");
        }

        private void AppendOrderByFirstSelect(StringBuilder builder)
        {
            AppendOrderBy(builder, "FirstSelect", false);
        }

        private void AppendOrderBy(StringBuilder builder, string selectName, bool reverseSortDirection)
        {
            builder.Append(") As " + DelimitTable(selectName));
            builder.Append(" ORDER BY ");
            int i = 0;
            foreach (OrderCriteriaField field in this.OrderCriteria.Fields)
            {
                string orderByFieldName = GetOrderByFieldName(field);
                if (i > 0) builder.Append(", "); //Append a seperator if composite sort criteria.
                builder.Append(DelimitField(selectName, orderByFieldName));
                if (field.SortDirection == SortDirection.Ascending && !reverseSortDirection)
                {
                    AppendDesc(builder);
                } else
                {
                    AppendAsc(builder);
                }
                i++;
            }
        }


        private string GetOrderByFieldName(OrderCriteriaField orderOrderCriteriaField)
        {
            if (Fields.ContainsKey(orderOrderCriteriaField.PropertyName))
            {
                QueryField queryField = Fields[orderOrderCriteriaField.PropertyName];
                return queryField.FieldName;
            }
            return orderOrderCriteriaField.FieldName;
        }

        private void AppendNoOfRecordsClauseAtEnd(StringBuilder builder)
        {
            string limitClauseAtEnd = _sqlFormatter.GetLimitClauseCriteriaForEnd(this.Limit);
            if (!String.IsNullOrEmpty(limitClauseAtEnd))
            {
                builder.Append(limitClauseAtEnd);
            }
        }

        private void AppendFrom(StringBuilder builder)
        {
            SourceDB source = new SourceDB(_selectQuery.Source);
            //if (Aliases.Count > 0)
            //{
            //    builder.AppendFormat(" FROM {0} {1}", source.CreateSQL(_sqlFormatter, Aliases), Aliases[source]);
            //}
            //else
            //{
                builder.AppendFormat(" FROM {0}", source.CreateSQL(_sqlFormatter, Aliases));
            //}
        }


        private void AppendFields(StringBuilder builder)
        {
            if (Aliases.Count > 0)
            {
                var fields = from field in _selectQuery.Fields.Values
                             select String.Format("{0}.{1}", _selectQuery.Aliases[field.Source.ToString()], DelimitFieldName(field.FieldName));
                builder.AppendFormat(String.Join(", ", fields.ToArray()));
            }
            else
            {
                var fields = from field in _selectQuery.Fields.Values
                          select DelimitField(field.Source, field.FieldName);
                builder.AppendFormat(String.Join(", ", fields.ToArray()));
            }
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

            string limitClauseAtBeginning = _sqlFormatter.GetLimitClauseCriteriaForBegin(this.Limit + this.FirstRecordToLoad);
            if (!String.IsNullOrEmpty(limitClauseAtBeginning))
            {
                builder.Append(limitClauseAtBeginning + " ");
            }
        }

        private void AppendLimitClauseAtEnd(StringBuilder builder)
        {
            if (_selectQuery.Limit < 0) return;

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
            foreach (OrderCriteriaField orderField in _selectQuery.OrderCriteria.Fields)
            {
                AppendOrderByField(orderByClause, orderField);
            }

            builder.Append(orderByClause.ToString());
        }

        private void AppendOrderByField(StringBuilder orderByClause, OrderCriteriaField orderOrderCriteriaField)
        {
            string direction = orderOrderCriteriaField.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";

            string tableAndFieldName;
            if (orderOrderCriteriaField.Source != null)
            {
                if (this.Aliases.Count>0)
                {
                    tableAndFieldName = this.Aliases[orderOrderCriteriaField.Source.ChildSourceLeaf.ToString()] + "." + DelimitFieldName(orderOrderCriteriaField.FieldName); 
                }
                else
                {
                    tableAndFieldName = DelimitField(orderOrderCriteriaField.Source.ChildSourceLeaf, orderOrderCriteriaField.FieldName);
                }
            }
            else
            {
                if (Fields.ContainsKey(orderOrderCriteriaField.PropertyName))
                {
                    QueryField queryField = Fields[orderOrderCriteriaField.PropertyName];
                    if (this.Aliases.Count>0)
                    {
                        tableAndFieldName = this.Aliases[queryField.Source.ToString()] + "." + DelimitFieldName(queryField.FieldName); 
                    }
                    else
                    {
                        tableAndFieldName = DelimitField(queryField.Source, queryField.FieldName);
                    }
                    //tableAndFieldName = DelimitField(queryField.Source, queryField.FieldName);
                } else
                {
                    if (this.Aliases.Count > 0)
                    {
                        tableAndFieldName = this.Aliases[Source.ToString()] + "." + DelimitFieldName(orderOrderCriteriaField.FieldName);
                    }
                    else
                    {
                        tableAndFieldName = DelimitField(Source, orderOrderCriteriaField.FieldName);
                    }
                    //tableAndFieldName = DelimitField(this.Source, orderOrderCriteriaField.FieldName);
                }
            }
            StringUtilities.AppendMessage(orderByClause, tableAndFieldName + " " + direction, ", ");
        }

        private void AppendWhereClause(StringBuilder builder, SqlStatement statement)
        {
            Criteria fullCriteria = Criteria.MergeCriteria(_selectQuery.Criteria, _selectQuery.DiscriminatorCriteria);
            ClassDef classDef = (ClassDef) _selectQuery.ClassDef;
            if (classDef != null && classDef.ClassID.HasValue)
            {
                Criteria classIDCriteria = new Criteria("DMClassID", Criteria.ComparisonOp.Equals, classDef.ClassID.Value);
                fullCriteria = Criteria.MergeCriteria(fullCriteria, classIDCriteria);
            }

            if (fullCriteria == null) return;
            builder.Append(" WHERE ");
            CriteriaDB criteriaDB = new CriteriaDB(fullCriteria);
            

            string whereClause = criteriaDB.ToString(_sqlFormatter, value => AddParameter(value, statement),Aliases);

            builder.Append(whereClause);
        }

        private string AddParameter(object value, SqlStatement statement)
        {
            if (value == null) value = "NULL";
            var resolvableValue = value as IResolvableToValue<DateTime>;
            if (resolvableValue != null)
            {
                value = resolvableValue.ResolveToValue();
            }
            if (value is Criteria.CriteriaValues)
            {
                return CreateInClause(statement, value);
            }
            string paramName = statement.ParameterNameGenerator.GetNextParameterName();
            statement.AddParameter(paramName, value);
            return paramName;

        }

        private string CreateInClause(SqlStatement statement, object value)
        {
            string paramName = statement.ParameterNameGenerator.GetNextParameterName();
            string inClause = "(";
            Criteria.CriteriaValues criteriaValues = (Criteria.CriteriaValues)value;
            int i = 0;
            foreach (var paramValue in criteriaValues)
            {
                statement.AddParameter(paramName, paramValue);
                inClause += paramName;
                if (i < criteriaValues.Count - 1)
                {
                    inClause += ", ";
                    paramName = statement.ParameterNameGenerator.GetNextParameterName();
                }
                i++;
            }
            inClause += ")";
            return inClause;
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

        private string DelimitTable(string tableName)
        {
            return _sqlFormatter.DelimitTable(tableName);
        }

        private string DelimitFieldName(string fieldName)
        {
            return _sqlFormatter.DelimitField(fieldName);
        }

        private int aliasCount;
        /// <summary>
        /// Sets up the aliases to use for each of the sources in this select query.
        /// </summary>
        public void SetupAliases()
        {
            aliasCount = 0;
            AddAliasForSource(this.Source);
 }


        private void AddAliasForSource(Source source)
        {
            if (source == null) return;
            var sourceName = source.ToString();
            if (this.Aliases.ContainsKey(sourceName)) return;

            aliasCount = aliasCount + 1;

            var sourceParts = sourceName.Split('.').ToList();
            Queue<string> queue = new Queue<string>();
            sourceParts.ForEach(queue.Enqueue);

            string subSourceName = "";
            while (queue.Count > 0)
            {
                if (!String.IsNullOrEmpty(subSourceName)) subSourceName += ".";
                subSourceName += queue.Dequeue();
                if (!this.Aliases.ContainsKey(subSourceName))
                    this.Aliases.Add(subSourceName, "a" + aliasCount);
            }

            if (source.ChildSource != null) AddAliasForSource(source.ChildSource);
            if (source.ChildSourceLeaf != null) AddAliasForSource(source.ChildSourceLeaf);
            foreach (var queryField in Fields)
            {
                AddAliasForSource(queryField.Value.Source);
            }
            source.InheritanceJoins.ForEach(@join =>
            {
                if (!this.Aliases.ContainsKey(@join.ToString())) AddAliasForSource(@join.FromSource);
                if (!this.Aliases.ContainsKey(@join.ToSource.ToString())) AddAliasForSource(@join.ToSource);
            });
            source.Joins.ForEach(@join =>
            {
                if (!this.Aliases.ContainsKey(@join.FromSource.ToString())) AddAliasForSource(@join.FromSource);
                if (!this.Aliases.ContainsKey(@join.ToSource.ToString())) AddAliasForSource(@join.ToSource);
            });
        }
    }
}