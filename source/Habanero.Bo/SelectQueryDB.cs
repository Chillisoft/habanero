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
using Habanero.BO.ClassDefinition;
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

        ///<summary>
        /// Creates a SelectQueryDB, wrapping an ISelectQuery (Decorator pattern)
        ///</summary>
        ///<param name="selectQuery"></param>
        public SelectQueryDB(ISelectQuery selectQuery)
        {
            _selectQuery = selectQuery;
        }

        #region ISelectQuery Members

        public Criteria Criteria
        {
            get { return _selectQuery.Criteria; }
            set { _selectQuery.Criteria = value; }
        }

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

        public OrderCriteria OrderCriteria
        {
            get { return _selectQuery.OrderCriteria; }
            set { _selectQuery.OrderCriteria = value; }
        }

        public int Limit
        {
            get { return _selectQuery.Limit; }
            set { _selectQuery.Limit = value; }
        }

        public IClassDef ClassDef
        {
            get { return _selectQuery.ClassDef; }
            set { _selectQuery.ClassDef = value; }
        }

        #endregion

        /// <summary>
        /// Creates an ISqlStatement out of the SelectQuery given in the constructor, to be used to load 
        /// from a database
        /// </summary>
        /// <returns>An ISqlStatement that can be executed against an IDatabaseConnection</returns>
        public ISqlStatement CreateSqlStatement()
        {
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection);
            StringBuilder builder = statement.Statement.Append("SELECT ");
            AppendLimitClauseAtBeginning(builder);
            AppendFields(builder);
            AppendFrom(builder);
            AppendJoins(builder);
            AppendWhereClause(builder, statement);
            AppendOrderByClause(builder);
            AppendLimitClauseAtEnd(builder);
            return statement;
        }

        private void AppendJoins(StringBuilder builder)
        {
            foreach (OrderCriteria.Field field in _selectQuery.OrderCriteria.Fields)
            {
                if (field.Source != null && !field.Source.Equals(this.Source))
                {
                    ClassDef currentClassDef = (ClassDef)this.ClassDef;
                    string relationshipJoinTable = AddRelationshipJoin(builder, currentClassDef, field, field.Source);
                    field.Source.EntityName = relationshipJoinTable;
                }
            }
        }

        private string AddRelationshipJoin(StringBuilder builder, ClassDef currentClassDef, QueryField field, Source fieldSource)
        {
            if (fieldSource == null || String.IsNullOrEmpty(fieldSource.Name))
            {
                IPropDef propDef = currentClassDef.GetPropDef(field.PropertyName, true);
                if (propDef != null)
                {
                    field.FieldName = propDef.DatabaseFieldName;
                } 
                return currentClassDef.GetTableName();
            }
            string[] parts = fieldSource.ToString().Split(new char[]{'.'}, StringSplitOptions.RemoveEmptyEntries);
            string relationshipName = parts[0];
            RelationshipDef relationshipDef = currentClassDef.GetRelationship(relationshipName);
            if (relationshipDef != null)
            {
                string joinString = GetJoinStringForRelationship(relationshipDef, currentClassDef);
                builder.Append(joinString);
                ClassDef relatedObjectClassDef = relationshipDef.RelatedObjectClassDef;
                if (relatedObjectClassDef != null)
                {
                    Source childSource = Source.FromString(string.Join(";", parts, 1, parts.Length - 1));
                    string relationshipJoinTable = AddRelationshipJoin(builder, relatedObjectClassDef, field, childSource);
                    if (String.IsNullOrEmpty(relationshipJoinTable))
                    {
                        relationshipJoinTable = relatedObjectClassDef.GetTableName();
                    }
                    return relationshipJoinTable;
                }
                else
                {
                    return currentClassDef.GetTableName();
                }
            }
            return currentClassDef.GetTableName();
        }

        private string GetJoinStringForRelationship(RelationshipDef relationshipDef)
        {
            return GetJoinStringForRelationship(relationshipDef, this.ClassDef);
        }

        private string GetJoinStringForRelationship(RelationshipDef relationshipDef, IClassDef classDef)
        {
            string joinString = "";
            foreach (RelPropDef relPropDef in relationshipDef.RelKeyDef)
            {
                ClassDef relatedClassDef = relationshipDef.RelatedObjectClassDef;
                IPropDef ownerPropDef = classDef.GetPropDef(relPropDef.OwnerPropertyName);
                IPropDef relatedPropDef = relatedClassDef.GetPropDef(relPropDef.RelatedClassPropName);
                joinString +=
                    GetJoinString(joinString, classDef.TableName, ownerPropDef.DatabaseFieldName,
                        relatedClassDef.TableName, relatedPropDef.DatabaseFieldName);
            }
            return joinString;
        }

        private string GetJoinString(string currentJointString, string joinFromTableName, string joinFromFieldName,
            string joinToTableName, string joinToFieldName)
        {
            string firstBit = "";
            if (String.IsNullOrEmpty(currentJointString))
            {
                firstBit += " JOIN " + DelimitTable(joinToTableName) + " ON";
            }
            else
            {
                firstBit += " AND";
            }
            string joinString = String.Format("{0} {1} = {2}",
                firstBit,
                DelimitField(new Source(joinFromTableName), joinFromFieldName),
                DelimitField(new Source(joinToTableName), joinToFieldName));
            return joinString;
        }


        private void AppendFrom(StringBuilder builder)
        {
            builder.AppendFormat(" FROM {0}", DelimitTable(_selectQuery.Source.EntityName));
            ClassDef currentClassDef = (ClassDef) _selectQuery.ClassDef;
            while (currentClassDef != null && currentClassDef.IsUsingClassTableInheritance())
            {
                ClassDef superClassClassDef = currentClassDef.SuperClassClassDef;
                IPropDef superClassPropDef = superClassClassDef.GetPrimaryKeyDef()[0];
                IPropDef thisClassPropDef = currentClassDef.GetPrimaryKeyDef()[0];
                builder.Append(
                    GetJoinString("", currentClassDef.GetTableName(), thisClassPropDef.DatabaseFieldName,
                        superClassClassDef.GetTableName(), superClassPropDef.DatabaseFieldName));
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
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

        private void AppendLimitClauseAtBeginning(StringBuilder builder)
        {
            if (_selectQuery.Limit == 0) return;

            string limitClauseAtBeginning =
                DatabaseConnection.CurrentConnection.GetLimitClauseForBeginning(_selectQuery.Limit);
            if (!String.IsNullOrEmpty(limitClauseAtBeginning))
            {
                builder.Append(limitClauseAtBeginning + " ");
            }
        }

        private void AppendLimitClauseAtEnd(StringBuilder builder)
        {
            if (_selectQuery.Limit == 0) return;

            string limitClauseAtEnd = DatabaseConnection.CurrentConnection.GetLimitClauseForEnd(_selectQuery.Limit);
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
                tableAndFieldName = DelimitField(orderField.Source, orderField.FieldName);
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

        //private string GetRelatedTableName(string relationshipName)
        //{
        //    RelationshipDef relationshipDef = ((ClassDef) this.ClassDef).GetRelationship(relationshipName);
        //    ClassDef relatedClassDef = relationshipDef.RelatedObjectClassDef;
        //    return DelimitTable(relatedClassDef.TableName);
        //}

        private void AppendWhereClause(StringBuilder builder, SqlStatement statement)
        {
            if (_selectQuery.Criteria != null)
            {
                builder.Append(" WHERE ");
                string whereClause =
                    _selectQuery.Criteria.ToString(delegate(string propName)
                    {
                        QueryField queryField = _selectQuery.Fields[propName];
                        return DelimitField(queryField.Source, queryField.FieldName);
                    }, delegate(object value)
                    {
                        string paramName = statement.ParameterNameGenerator.GetNextParameterName();
                        statement.AddParameter(paramName, value);
                        return paramName;
                    });
                builder.Append(whereClause);
            }
        }

        private string DelimitField(Source source, string fieldName)
        {
            if (source == null)
                return SqlFormattingHelper.FormatFieldName(fieldName, DatabaseConnection.CurrentConnection);
            return
                SqlFormattingHelper.FormatTableAndFieldName(source.EntityName, fieldName, DatabaseConnection.CurrentConnection);
        }

        private string DelimitTable(string tableName)
        {
            return SqlFormattingHelper.FormatTableName(tableName, DatabaseConnection.CurrentConnection);
        }
    }
}