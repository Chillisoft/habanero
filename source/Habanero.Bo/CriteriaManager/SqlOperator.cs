// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;

namespace Habanero.BO.CriteriaManager
{
    /// <summary>
    /// Provides a sql operator for use in building sql statements
    /// </summary>
    public class SqlOperator : IExpression
    {
        private string _operator;

        /// <summary>
        /// Copies across the parameterised sql info (see IParameterSqlInfo for
        /// more detail)
        /// </summary>
        /// <param name="info">The IParameterSqlInfo object</param>
        public void SetParameterSqlInfo(IParameterSqlInfo info)
        {
        }
        
        ///<summary>
        /// Creates and returns a copy of this IExpression instance.
        ///</summary>
        ///<returns>Returns a copy of this IExpression instance.</returns>
        public IExpression Clone()
        {
            return new SqlOperator(_operator);
        }

        /// <summary>
        /// Constructor that sets the operator to that provided, ensuring it
        /// is converted to upper case
        /// </summary>
        /// <param name="sqlOperator">The new operator</param>
        public SqlOperator(string sqlOperator)
        {
            //TODO: Error check valid inputs
            _operator = sqlOperator.ToUpper();
        }

        /// <summary>
        /// Returns the expression string, which in this case is the operator
        /// being represented
        /// </summary>
        /// <returns>Returns the operator</returns>
        public string ExpressionString()
        {
            return _operator;
        }

        /// <summary>
        /// Adds the operator to the end of the given sql statement.
        /// See IExpression.SqlExpressionString for more detail on the
        /// format of the arguments.
        /// </summary>
        public void SqlExpressionString(ISqlStatement statement, string tableNameFieldNameLeftSeparator,
                                        string tableNameFieldNameRightSeparator)
        {
            statement.Statement.Append(_operator);
        }

//		public string SqlExpressionString(string tableNameFieldNameLeftSeperator,
//		                                  string tableNameFieldNameRightSeperator,
//		                                  string DateTimeLeftSeperator,
//		                                  string DateTimeRightSeperator) {
//			return _operator;
//		}
    }
}