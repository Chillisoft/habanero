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

using System.Collections;
using System.Data;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;

namespace Habanero.BO.SqlGeneration
{
    /// <summary>
    /// Generates "update" sql statements to update a specified business
    /// object's properties in the database
    /// </summary>
    public class UpdateStatementGenerator
    {
        private BusinessObject _bo;
        private IDbConnection _conn;
        private SqlStatementCollection _statementCollection;
        private SqlStatement _updateSql;

        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object whose properties are to
        /// be updated</param>
        /// <param name="conn">A database connection</param>
        public UpdateStatementGenerator(BusinessObject bo, IDbConnection conn)
        {
            _bo = bo;
            _conn = conn;
        }

        /// <summary>
        /// Generates a collection of sql statements to update the business
        /// object's properties in the database
        /// </summary>
        /// <returns>Returns a sql statement collection</returns>
        public SqlStatementCollection Generate()
        {
            _statementCollection = new SqlStatementCollection();
            BOPropCol propsToInclude;
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
            tableName = _bo.TableName;
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
        private void GenerateSingleUpdateStatement(string tableName, BOPropCol propsToInclude,
                                                   bool isSuperClassStatement, ClassDef currentClassDef)
        {
            _updateSql = new SqlStatement(_conn);
            _updateSql.Statement.Append(@"UPDATE " + tableName + " SET ");
            int includedProps = 0;
            foreach (BOProp prop in _bo.Props.SortedValues)
            {
                if (propsToInclude.Contains(prop.PropertyName))
                {
                    PrimaryKeyDef primaryKeyDef = _bo.ClassDef.PrimaryKeyDef;
                    if (primaryKeyDef == null) primaryKeyDef = (PrimaryKeyDef) _bo.ID.KeyDef;
                    if (prop.IsDirty &&
                        ((primaryKeyDef.IsObjectID && !primaryKeyDef.Contains(prop.PropertyName)) ||
                         !primaryKeyDef.IsObjectID))
                    {
                        includedProps++;
                        _updateSql.Statement.Append(prop.DatabaseFieldName);
                        _updateSql.Statement.Append(" = ");
                        _updateSql.AddParameterToStatement(prop.Value);
                        //_updateSql.AddParameterToStatement(DatabaseUtil.PrepareValue(prop.PropertyValue));
                        _updateSql.Statement.Append(", ");
                    }
                }
            }
            _updateSql.Statement.Remove(_updateSql.Statement.Length - 2, 2); //remove the last ", "
            if (isSuperClassStatement)
            {
                _updateSql.Statement.Append(" WHERE " + _bo.WhereClauseForSuperClass(_updateSql, currentClassDef));
            }
            else
            {
                _updateSql.Statement.Append(" WHERE " + _bo.WhereClause(_updateSql));
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
        private BOPropCol GetPropsToInclude(ClassDef currentClassDef)
        {
            BOPropCol propsToInclude = currentClassDef.PropDefcol.CreateBOPropertyCol(true);

            while (currentClassDef.IsUsingSingleTableInheritance() ||
                currentClassDef.IsUsingConcreteTableInheritance())
            {
                propsToInclude.Add(currentClassDef.SuperClassClassDef.PropDefcol.CreateBOPropertyCol(true));
                currentClassDef = currentClassDef.SuperClassClassDef;
            }

            return propsToInclude;
        }
    }
}
