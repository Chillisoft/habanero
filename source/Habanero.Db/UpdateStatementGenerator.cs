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

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.DB
{
    /// <summary>
    /// Generates "update" sql statements to update a specified business
    /// object's properties in the database
    /// </summary>
    public class UpdateStatementGenerator
    {
        private readonly BusinessObject _bo;
        private readonly IDatabaseConnection _connection;
        private SqlStatementCollection _statementCollection;
        private SqlStatement _updateSql;

        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object whose properties are to
        /// be updated</param>
        /// <param name="connection">A database connection</param>
        public UpdateStatementGenerator(IBusinessObject bo, IDatabaseConnection connection)
        {
            _bo = (BusinessObject) bo;
            _connection = connection;
        }

        /// <summary>
        /// Generates a collection of sql statements to update the business
        /// object's properties in the database
        /// </summary>
        /// <returns>Returns a sql statement collection</returns>
        public SqlStatementCollection Generate()
        {
            _statementCollection = new SqlStatementCollection();
            IBOPropCol propsToInclude;
            string tableName;
            ClassDef currentClassDef = _bo.ClassDef;

            while (currentClassDef.IsUsingClassTableInheritance())
            {
                propsToInclude = GetPropsToInclude(currentClassDef.SuperClassClassDef);
                if (propsToInclude.Count > 0)
                {
                    tableName = currentClassDef.SuperClassClassDef.InheritedTableName;
                    GenerateSingleUpdateStatement(tableName, propsToInclude, true, currentClassDef);
                }
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            propsToInclude = GetPropsToInclude(_bo.ClassDef);
            tableName = StatementGeneratorUtils.GetTableName(_bo);
            GenerateSingleUpdateStatement(tableName, propsToInclude, false, _bo.ClassDef);
            return _statementCollection;
        }

        /// <summary>
        /// Generates an "update" sql statement for the properties in the
        /// business object
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="propsToInclude">A collection of properties to update,
        /// if the previous include-all boolean was not set to true</param>
        /// <param name="isSuperClassStatement">Whether a super-class is involved</param>
        /// <param name="currentClassDef">The current class definition</param>
        private void GenerateSingleUpdateStatement(string tableName, IBOPropCol propsToInclude,
                                                   bool isSuperClassStatement, ClassDef currentClassDef)
        {
            _updateSql = new SqlStatement(_connection);
            _updateSql.Statement.Append(
                @"UPDATE " + SqlFormattingHelper.FormatTableName(tableName, _connection) + " SET ");
            int includedProps = 0;
            foreach (BOProp prop in _bo.Props.SortedValues)
            {
                if (propsToInclude.Contains(prop.PropertyName))
                {
                    PrimaryKeyDef primaryKeyDef = _bo.ClassDef.GetPrimaryKeyDef() ?? (PrimaryKeyDef) _bo.ID.KeyDef;
                    if (prop.IsDirty &&
                        ((primaryKeyDef.IsGuidObjectID && !primaryKeyDef.Contains(prop.PropertyName)) ||
                         !primaryKeyDef.IsGuidObjectID))
                    {
                        includedProps++;
                        _updateSql.Statement.Append(SqlFormattingHelper.FormatFieldName(prop.DatabaseFieldName, _connection));
                        _updateSql.Statement.Append(" = ");
                        _updateSql.AddParameterToStatement(prop.Value);
                        _updateSql.Statement.Append(", ");
                    }
                }
            }
            _updateSql.Statement.Remove(_updateSql.Statement.Length - 2, 2); //remove the last ", "
            if (isSuperClassStatement)
            {
                _updateSql.Statement.Append(" WHERE " + StatementGeneratorUtils.PersistedDatabaseWhereClause(BOPrimaryKey.GetSuperClassKey(currentClassDef, _bo), _updateSql));
            }
            else
            {
                _updateSql.Statement.Append(" WHERE " + StatementGeneratorUtils.PersistedDatabaseWhereClause((BOKey) _bo.ID, _updateSql));
            }
            if (includedProps > 0)
            {
                _statementCollection.Add(_updateSql);
            }
        }

        /// <summary>
        /// Builds a collection of properties to include in the update,
        /// depending on the inheritance type
        /// </summary>
        private static IBOPropCol GetPropsToInclude(ClassDef currentClassDef)
        {
            IBOPropCol propsToIncludeTemp = currentClassDef.PropDefcol.CreateBOPropertyCol(true);

            IBOPropCol propsToInclude = new BOPropCol();

            foreach (BOProp prop in propsToIncludeTemp)
            {
                if (prop.PropDef.Persistable) propsToInclude.Add(prop);
            }

            while (currentClassDef.IsUsingSingleTableInheritance() ||
                   currentClassDef.IsUsingConcreteTableInheritance())
            {
                propsToInclude.Add(currentClassDef.SuperClassClassDef.PropDefcol.CreateBOPropertyCol(true));
                currentClassDef = currentClassDef.SuperClassClassDef;
            }

            return propsToInclude;
        }

        ///<summary>
        /// Generate SqlStatementCollection for the Relationsp
        ///</summary>
        ///<param name="relationship"></param>
        ///<param name="relatedBusinessObject"></param>
        ///<returns></returns>
        public ISqlStatementCollection GenerateForRelationship(IRelationship relationship, IBusinessObject relatedBusinessObject)
        {
            _statementCollection = new SqlStatementCollection();
            BOPropCol propsToInclude = new BOPropCol();
            IBOProp oneProp = null;
            foreach (IRelPropDef propDef in relationship.RelationshipDef.RelKeyDef)
            {
                oneProp = relatedBusinessObject.Props[propDef.RelatedClassPropName];
                propsToInclude.Add(oneProp);
            }

            if (oneProp == null) return _statementCollection;
            IClassDef classDef = relatedBusinessObject.ClassDef;
            string tableName = classDef.GetTableName(oneProp.PropDef);
            GenerateSingleUpdateStatement(tableName, propsToInclude, false, (ClassDef) classDef);

            return _statementCollection;
        }
    }
}