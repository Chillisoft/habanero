//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using Habanero.Base;

namespace Habanero.BO.CriteriaManager
{
    /// <summary>
    /// Expression implements IExpression to manage an expression
    /// linked to possible left and right expressions
    /// </summary>
    public class Expression : IExpression
    {
        private readonly IExpression _leftExpression;
        private readonly SqlOperator _sqlOperator;
        private readonly IExpression _rightExpression;
        private static readonly string[] _operators = new string[] {" OR ", " AND "};

        /// <summary>
        /// Constructor to create a new expression.  If this is part of a linked
        /// list of expressions that make up a complete expression, then the
        /// expressions to the left and/or right can also be specified.
        /// </summary>
        /// <param name="leftExpression">The left expression if available</param>
        /// <param name="expressionSqlOperator">The sql operator between expressions</param>
        /// <param name="rightExpression">The right expression if available</param>
        public Expression(IExpression leftExpression, SqlOperator expressionSqlOperator, IExpression rightExpression)
        {
            //TODO: Error check valid inputs
            _leftExpression = leftExpression;
            _sqlOperator = expressionSqlOperator;
            _rightExpression = rightExpression;
        }

        /// <summary>
        /// Private constructor to create a new expression using the
        /// expression string provided
        /// </summary>
        /// <param name="expressionString">The expression string</param>
        private Expression(string expressionString)
        {
            //TODO: Error check valid inputs
            CriteriaExpression c = new CriteriaExpression(expressionString, _operators);

            //Create left expression
            if (c.IsLeaf())
            {
            }
            if (c.Left.IsLeaf())
            {
                _leftExpression = new Parameter(c.Left.CompleteExpression);
            }
            else
            {
                _leftExpression = new Expression(c.Left.CompleteExpression);
            }

            //Create operator
            _sqlOperator = new SqlOperator(c.Expression);

            //Create right expression
            if (c.Right.IsLeaf())
            {
                _rightExpression = new Parameter(c.Right.CompleteExpression);
            }
            else
            {
                _rightExpression = new Expression(c.Right.CompleteExpression);
            }
        }

        ///<summary>
        /// The IExpression representing the left part of this expression
        ///</summary>
        public IExpression LeftExpression
        {
            get { return _leftExpression; }
        }

        ///<summary>
        /// The SqlOperator representing the operator for this expression
        ///</summary>
        public SqlOperator SqlOperator
        {
            get { return _sqlOperator; }
        }

        ///<summary>
        /// The IExpression representing the right part of this expression
        ///</summary>
        public IExpression RightExpression
        {
            get { return _rightExpression; }
        }

        #region IExpression Interface Implementation

        /// <summary>
        /// Copies across the parameterised sql info (see IParameterSqlInfo for
        /// more detail)
        /// </summary>
        /// <param name="info">The IParameterSqlInfo object</param>
        public void SetParameterSqlInfo(IParameterSqlInfo info)
        {
            _leftExpression.SetParameterSqlInfo(info);
            _rightExpression.SetParameterSqlInfo(info);
        }

        ///<summary>
        /// Creates and returns a copy of this IExpression instance.
        ///</summary>
        ///<returns>Returns a copy of this IExpression instance.</returns>
        public IExpression Clone()
        {
            IExpression leftExpression = _leftExpression.Clone();
            SqlOperator sqlOperator = (SqlOperator)_sqlOperator.Clone();
            IExpression rightExpression = _rightExpression.Clone();
            return new Expression(leftExpression, sqlOperator, rightExpression);
        }
                
        /// <summary>
        /// Returns the full expression as a string, including left and right
        /// expressions, surrounded by round brackets.
        /// </summary>
        /// <returns>Returns the full expression string</returns>
        public string ExpressionString()
        {
            return "(" + _leftExpression.ExpressionString() + " " + _sqlOperator.ExpressionString().Trim() +
                   " " + _rightExpression.ExpressionString() + ")";
        }

        /// <summary>
        /// Creates the full expression in sql format.
        /// See IExpression.SqlExpressionString for more detail on the
        /// format of the arguments.
        /// </summary>
        public void SqlExpressionString(ISqlStatement statement, string tableNameFieldNameLeftSeparator,
                                        string tableNameFieldNameRightSeparator)
        {
            statement.Statement.Append("(");
            _leftExpression.SqlExpressionString(statement, tableNameFieldNameLeftSeparator,
                                                tableNameFieldNameRightSeparator);
            statement.Statement.Append(" " + _sqlOperator.ExpressionString().Trim() + " ");
            _rightExpression.SqlExpressionString(statement, tableNameFieldNameLeftSeparator,
                                                 tableNameFieldNameRightSeparator);
            statement.Statement.Append(")");
        }

//		public string SqlExpressionString(string tableNameFieldNameLeftSeperator,
//		                                  string tableNameFieldNameRightSeperator,
//		                                  string dateTimeLeftSeperator,
//		                                  string dateTimeRightSeperator) 
//      {
//			return "(" + _leftExpression.SqlExpressionString(tableNameFieldNameLeftSeperator,
//			                                                 tableNameFieldNameRightSeperator, dateTimeLeftSeperator, dateTimeRightSeperator) +
//				" " + _sqlOperator.ExpressionString() +
//				" " + _rightExpression.SqlExpressionString(tableNameFieldNameLeftSeperator,
//				                                           tableNameFieldNameRightSeperator, dateTimeLeftSeperator, dateTimeRightSeperator) + ")";
        //		}

        #endregion //IExpression Interface Implementation

        #region Static Members

        /// <summary>
        /// Creates a new IExpression object using the expression string provided.
        /// </summary>
        /// <param name="expressionClause">The expression string</param>
        /// <returns>If the expression is a leaf object (it has no left and right
        /// expressions), then a Parameter object is returned, otherwise an
        /// Expression object is returned.</returns>
        public static IExpression CreateExpression(string expressionClause)
        {
            CriteriaExpression c = new CriteriaExpression(expressionClause, _operators);
            if (c.IsLeaf())
            {
                return new Parameter(expressionClause);
            }
            return new Expression(expressionClause);
        }

        /// <summary>
        /// Appends a given expression string to the end of a given expression,
        /// separated by the given sql operator.
        /// </summary>
        /// <param name="leftExpression">The expression object to append to</param>
        /// <param name="expressionSqlOperator">The sql operator</param>
        /// <param name="expressionClause">The new expression clause</param>
        /// <returns>Returns the full expression object with the newly
        /// attached expression</returns>
        public static IExpression AppendExpression(IExpression leftExpression,
                                                   SqlOperator expressionSqlOperator,
                                                   string expressionClause)
        {
            IExpression expr = CreateExpression(expressionClause);
            return new Expression(leftExpression, expressionSqlOperator, expr);
        }

        #endregion //Static Members

    }
}