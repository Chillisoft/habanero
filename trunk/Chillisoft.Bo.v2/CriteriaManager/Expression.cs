using System;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.CriteriaManager.v2
{
    /// <summary>
    /// Expression implements IExpression to manage an expression
    /// linked to possible left and right expressions
    /// </summary>
    /// TODO ERIC - review
    public class Expression : IExpression
    {
        private IExpression mLeftExpression;
        private SqlOperator mSqlOperator;
        private IExpression mRightExpression;
        private static string[] Operators = new string[] {" OR ", " AND "};

        /// <summary>
        /// Copies across the parameterised sql info (see IParameterSQLInfo for
        /// more detail)
        /// </summary>
        /// <param name="info">The IParameterSQLInfo object</param>
        /// <param name="tableName">The table name</param>
        public void SetParameterSqlInfo(IParameterSqlInfo info, String tableName)
        {
            mLeftExpression.SetParameterSqlInfo(info, tableName);
            mRightExpression.SetParameterSqlInfo(info, tableName);
        }

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
            mLeftExpression = leftExpression;
            mSqlOperator = expressionSqlOperator;
            mRightExpression = rightExpression;
        }

        /// <summary>
        /// Creates a new IExpression object using the expression string provided.
        /// </summary>
        /// <param name="expressionClause">The expression string</param>
        /// <returns>If the expression is a leaf object (it has no left and right
        /// expressions), then a Parameter object is returned, otherwise an
        /// Expression object is returned.</returns>
        public static IExpression CreateExpression(string expressionClause)
        {
            CriteriaExpression c = new CriteriaExpression(expressionClause, Operators);
            if (c.IsLeaf())
            {
                return new Parameter(expressionClause);
            }
            else
            {
                return new Expression(expressionClause);
            }
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

        /// <summary>
        /// Private constructor to create a new expression using the
        /// expression string provided
        /// </summary>
        /// <param name="expressionString">The expression string</param>
        private Expression(string expressionString)
        {
            //TODO: Error check valid inputs
            CriteriaExpression c = new CriteriaExpression(expressionString, Operators);

            //Create left expression
            if (c.IsLeaf())
            {
            }
            if (c.Left.IsLeaf())
            {
                mLeftExpression = new Parameter(c.Left.CompleteExpression);
            }
            else
            {
                mLeftExpression = new Expression(c.Left.CompleteExpression);
            }

            //Create operator
            mSqlOperator = new SqlOperator(c.Expression);

            //Create right expression
            if (c.Right.IsLeaf())
            {
                mRightExpression = new Parameter(c.Right.CompleteExpression);
            }
            else
            {
                mRightExpression = new Expression(c.Right.CompleteExpression);
            }
        }

        /// <summary>
        /// Returns the full expression as a string, including left and right
        /// expressions, surrounded by round brackets.
        /// </summary>
        /// <returns>Returns the full expression string</returns>
        public string ExpressionString()
        {
            return "(" + mLeftExpression.ExpressionString() + " " + mSqlOperator.ExpressionString() +
                   " " + mRightExpression.ExpressionString() + ")";
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
            mLeftExpression.SqlExpressionString(statement, tableNameFieldNameLeftSeparator,
                                                tableNameFieldNameRightSeparator);
            statement.Statement.Append(" " + mSqlOperator.ExpressionString() + " ");
            mRightExpression.SqlExpressionString(statement, tableNameFieldNameLeftSeparator,
                                                 tableNameFieldNameRightSeparator);
            statement.Statement.Append(")");
        }

//		public string SqlExpressionString(string tableNameFieldNameLeftSeperator,
//		                                  string tableNameFieldNameRightSeperator,
//		                                  string dateTimeLeftSeperator,
//		                                  string dateTimeRightSeperator) {
//			return "(" + mLeftExpression.SqlExpressionString(tableNameFieldNameLeftSeperator,
//			                                                 tableNameFieldNameRightSeperator, dateTimeLeftSeperator, dateTimeRightSeperator) +
//				" " + mSqlOperator.ExpressionString() +
//				" " + mRightExpression.SqlExpressionString(tableNameFieldNameLeftSeperator,
//				                                           tableNameFieldNameRightSeperator, dateTimeLeftSeperator, dateTimeRightSeperator) + ")";
//		}
    }
}