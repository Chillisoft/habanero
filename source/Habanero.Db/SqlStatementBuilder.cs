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
using Habanero.Base;
using Habanero.Util;

namespace Habanero.DB
{
    ///<summary>
    /// Builds a SQL Select statement for a specific database type
    ///</summary>
    public class SqlStatementBuilder
    {
        private readonly IDatabaseConnection _connection;
        private const string SELECT_CLAUSE_TOKEN = "SELECT ";
        private const string DISTINCT_CLAUSE_TOKEN = " DISTINCT ";
        private const string FROM_CLAUSE_TOKEN = " FROM ";
        private const string JOIN_ON_TOKEN = " ON ";
        private const string WHERE_CLAUSE_TOKEN = " WHERE ";
        private const string AND_TOKEN = " AND ";
        private const string ORDER_BY_CLAUSE_TOKEN = " ORDER BY ";

        private readonly ISqlStatement _statement;
        ///<summary>
        /// Constructor for SQL Builder
        ///</summary>
        ///<param name="connection"></param>
        ///<param name="statement"></param>
        public SqlStatementBuilder(IDatabaseConnection connection, string statement)
        {
            _connection = connection;
            _statement = new SqlStatement(_connection, statement);
        }


        /// <summary>
        /// Appends a criteria clause to the sql statement.  " WHERE " (or
        /// " AND " where appropriate) is automatically prefixed by this
        /// method.
        /// </summary>
        /// <param name="criteria">The criteria clause</param>
        public void AppendCriteria(string criteria)
        {
            if (string.IsNullOrEmpty(criteria)) return;
            AppendWhere();
            this.Append(criteria);
        }


        /// <summary>
        /// Appends " WHERE " to the sql statement, or " AND " if a
        /// where-clause already exists
        /// </summary>
        public void AppendWhere()
        {
            int posWhere = FindStatementClauseToken(WHERE_CLAUSE_TOKEN);
            if (posWhere != -1)
            {
                this.Append(AND_TOKEN);
            }
            else
            {
                this.Append(WHERE_CLAUSE_TOKEN);
            }
        }

        ///<summary>
        /// Appends the appendStatement to the end of the statement.
        ///</summary>
        ///<param name="appendStatement"></param>
        public void Append(string appendStatement)
        {
            _statement.Statement.Append(appendStatement);
        }

        ///<summary>
        /// Returns the statement
        ///</summary>
        ///<returns></returns>
        public ISqlStatement GetStatement()
        {
            return _statement;
        }


        ///<summary>
        /// Adds a join clause to the sql statement
        ///</summary>
        ///<param name="joinType">The type of join to be created. eg. 'LEFT JOIN'</param>
        ///<param name="joinTable">The table to be joined to this sql statement</param>
        ///<param name="joinCriteria">The criteria on which the join is created</param>
        public void AddJoin(string joinType, string joinTable, string joinCriteria)
        {
            int posFrom = FindStatementClauseToken(FROM_CLAUSE_TOKEN);
            if (posFrom == -1)
            {
                throw new SqlStatementException("Cannot add a join clause to a SQL statement that does not contain a from clause.");
            }
            int posWhere = FindStatementClauseToken(WHERE_CLAUSE_TOKEN);
            if (posWhere == -1)
            {
                posWhere = _statement.Statement.Length;
            }
            joinType = joinType.Trim().ToUpper();
            string joinClause = " " + joinType + " " + SqlFormattingHelper.FormatTableName(joinTable, _connection) +
                                JOIN_ON_TOKEN + joinCriteria;
            int posExistingJoin = FindStatementClauseToken(" JOIN ");
            string closingBracket = ")";
            string openingBracket = "(";
            if (posExistingJoin == -1)
            {
                //Do not need brackets if there is not already a join.
                // Note_: This also prevents problems where SQL Server does not like brackets around just a table name.
                openingBracket = "";
                closingBracket = "";
            }
            _statement.Statement.Insert(posWhere, closingBracket + joinClause);
            _statement.Statement.Insert(posFrom + FROM_CLAUSE_TOKEN.Length, openingBracket);
            if (joinType != "INNER JOIN")
            {
                AddDistinct();
            }
        }

        private void AddDistinct()
        {
            int pos = FindStatementClauseToken(DISTINCT_CLAUSE_TOKEN);
            if (pos != -1) return;
            pos = FindStatementClauseToken(SELECT_CLAUSE_TOKEN);
            if (pos == -1)
            {
                return;
            }
            _statement.Statement.Insert(pos + SELECT_CLAUSE_TOKEN.Length, DISTINCT_CLAUSE_TOKEN.Trim() + " ");
        }




        /// <summary>
        /// Adds more fields to the select fields list in the statement.
        /// </summary>
        /// <param name="fields">The list of fields to add to the select statement</param>
        public void AddSelectFields(List<string> fields)
        {
            if (fields.Count == 0) return;
            int posFrom = FindStatementClauseToken(FROM_CLAUSE_TOKEN);
            if (posFrom == -1) return;
            int posSelectStart = FindStatementClauseToken(SELECT_CLAUSE_TOKEN);
            if (posSelectStart == -1) return;
            posSelectStart += SELECT_CLAUSE_TOKEN.Length;
            string currentSelectFields = _statement.Statement.ToString(posSelectStart, posFrom - posSelectStart);
            string fieldList = "";
            foreach (string field in fields)
            {
                string formattedField = SqlFormattingHelper.FormatFieldName(field, _connection);
                if (currentSelectFields.IndexOf(formattedField) == -1)
                {
                    fieldList += ", " + formattedField;
                    currentSelectFields += ", " + formattedField;
                }
            }
            _statement.Statement.Insert(posFrom, fieldList);
        }


        /// <summary>
        /// Appends an order-by clause to the sql statement. " ORDER BY " is
        /// automatically prefixed by this method.
        /// </summary>
        /// <param name="orderByCriteria">The order-by clause</param>
        public void AppendOrderBy(string orderByCriteria)
        {
            if (!string.IsNullOrEmpty(orderByCriteria))
            {
                _statement.Statement.Append(ORDER_BY_CLAUSE_TOKEN + orderByCriteria);
            }
        }


        private int FindStatementClauseToken(string token)
        {
            string statement = _statement.Statement.ToString();
            int posToken = -1;
            bool found = false;
            do
            {
                posToken = statement.IndexOf(token, posToken + 1, StringComparison.InvariantCultureIgnoreCase);
                if (posToken == -1)
                {
                    break;
                }
                int countQuotes = StringUtilities.CountOccurrences(statement, _connection.LeftFieldDelimiter, 0, posToken);
                if (_connection.LeftFieldDelimiter != _connection.RightFieldDelimiter)
                {
                    countQuotes += StringUtilities.CountOccurrences(statement, _connection.RightFieldDelimiter, 0, posToken);
                }
                if ((countQuotes % 2) != 0) continue;

                countQuotes = StringUtilities.CountOccurrences(statement, '\'', 0, posToken);
                if ((countQuotes % 2) == 0)
                {
                    found = true;
                }
            } while (!found);
            return posToken;
        }
    }
}