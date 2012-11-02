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
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.DB
{
    /// <summary>
    /// Generates "update" sql statements to update a specified business
    /// object's properties in the database
    /// </summary>
    public class UpdateStatementGenerator : ModifyStatementGenerator
    {
        private readonly BusinessObject _bo;
        private readonly IDatabaseConnection _connection;
        private IList<ISqlStatement> _statements;
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
        public IEnumerable<ISqlStatement> Generate()
        {
            _statements = new List<ISqlStatement>();
            IBOPropCol propsToInclude;
            string tableName;
            ClassDef currentClassDef = _bo.ClassDef;

            while (currentClassDef.IsUsingClassTableInheritance())
            {
                var superClassClassDef = (ClassDef) currentClassDef.SuperClassClassDef;
                propsToInclude = GetPropsToInclude(superClassClassDef);
                if (propsToInclude.Count > 0)
                {
                    tableName = superClassClassDef.InheritedTableName;
                    GenerateSingleUpdateStatement(tableName, propsToInclude, true, currentClassDef);
                }
                currentClassDef = superClassClassDef;
            }
            propsToInclude = GetPropsToInclude(_bo.ClassDef);
            tableName = StatementGeneratorUtils.GetTableName(_bo);
            GenerateSingleUpdateStatement(tableName, propsToInclude, false, _bo.ClassDef);
            return _statements;
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
                @"UPDATE " + _connection.SqlFormatter.DelimitTable(tableName) + " SET ");
            int includedProps = 0;
            foreach (BOProp prop in _bo.Props.SortedValues)
            {
                if (propsToInclude.Contains(prop.PropertyName))
                {
                    PrimaryKeyDef primaryKeyDef = (PrimaryKeyDef)_bo.ClassDef.PrimaryKeyDef ?? (PrimaryKeyDef)_bo.ID.KeyDef;
                    if (prop.IsDirty &&
                        ((primaryKeyDef.IsGuidObjectID && !primaryKeyDef.Contains(prop.PropertyName)) ||
                         !primaryKeyDef.IsGuidObjectID))
                    {
                        includedProps++;
                        _updateSql.Statement.Append(_connection.SqlFormatter.DelimitField(prop.DatabaseFieldName));
                        _updateSql.Statement.Append(" = ");
                        //prop.PropDef.GetDataMapper().GetDatabaseValue(prop.Value);
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
                _statements.Add(_updateSql);
            }
        }




       

        ///<summary>
        /// Generate SqlStatementCollection for the Relationsp
        ///</summary>
        ///<param name="relationship"></param>
        ///<param name="relatedBusinessObject"></param>
        ///<returns></returns>
        public IEnumerable<ISqlStatement> GenerateForRelationship(IRelationship relationship, IBusinessObject relatedBusinessObject)
        {
            _statements = new List<ISqlStatement>();
            var propsToInclude = new BOPropCol();
            IBOProp oneProp = null;
            foreach (var propDef in relationship.RelationshipDef.RelKeyDef)
            {
                oneProp = relatedBusinessObject.Props[propDef.RelatedClassPropName];
                propsToInclude.Add(oneProp);
            }

            if (oneProp == null) return _statements;
            IClassDef classDef = relatedBusinessObject.ClassDef;
            string tableName = classDef.GetTableName(oneProp.PropDef);
            GenerateSingleUpdateStatement(tableName, propsToInclude, false, (ClassDef) classDef);

            return _statements;
        }
    }
}