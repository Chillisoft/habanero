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

using System.Text;
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.BO.SqlGeneration
{
    /// <summary>
    /// Generates "delete" sql statements to delete a specified business
    /// object from the database
    /// </summary>
    public class DeleteStatementGenerator
    {
        private readonly BusinessObject _bo;
        private readonly IDatabaseConnection _connection;
        
        /// <summary>
        /// Constructor to initialise the generator
        /// </summary>
        /// <param name="bo">The business object to be deleted</param>
        /// <param name="connection">The database connection</param>
        public DeleteStatementGenerator(IBusinessObject bo, IDatabaseConnection connection)
        {
            _bo = (BusinessObject) bo;
            _connection = connection;
        }

        /// <summary>
        /// Generates a collection of sql statements to delete the business
        /// object from the database
        /// </summary>
        /// <returns>Returns a sql statement collection</returns>
        public SqlStatementCollection Generate()
        {
            SqlStatementCollection statementCollection = new SqlStatementCollection();

            //AddRelationshipDeleteStatements(statementCollection);

            SqlStatement deleteSql = new SqlStatement(_connection);
            deleteSql.Statement = new StringBuilder(
                @"DELETE FROM " + SqlFormattingHelper.FormatTableName(_bo.TableName, _connection) +
                                                    " WHERE " + _bo.ID.PersistedDatabaseWhereClause(deleteSql));
            statementCollection.Add(deleteSql);
            ClassDef currentClassDef = _bo.ClassDef;
            while (currentClassDef.IsUsingClassTableInheritance())
            {
                while (currentClassDef.SuperClassClassDef.SuperClassDef != null &&
                    currentClassDef.SuperClassClassDef.SuperClassDef.ORMapping == ORMapping.SingleTableInheritance)
                {
                    currentClassDef = currentClassDef.SuperClassClassDef;
                }
                deleteSql = new SqlStatement(_connection);
                deleteSql.Statement.Append(
                    "DELETE FROM " +
                    SqlFormattingHelper.FormatTableName(currentClassDef.SuperClassClassDef.TableName, _connection) +
                    " WHERE " +
                    BOPrimaryKey.GetSuperClassKey(currentClassDef, _bo).PersistedDatabaseWhereClause(deleteSql));
                statementCollection.Add(deleteSql);
                currentClassDef = currentClassDef.SuperClassClassDef;
            }
            return statementCollection;
        }
    }
}
