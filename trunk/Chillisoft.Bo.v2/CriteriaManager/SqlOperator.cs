using System;
using Chillisoft.Generic.v2;

namespace Chillisoft.Bo.CriteriaManager.v2
{
    /// <summary>
    /// Provides a sql operator for use in building sql statements
    /// </summary>
    public class SqlOperator : IExpression
    {
        private string mOperator;

        /// <summary>
        /// Copies across the parameterised sql info (see IParameterSQLInfo for
        /// more detail)
        /// </summary>
        /// <param name="info">The IParameterSQLInfo object</param>
        /// <param name="tableName">The table name</param>
        public void SetParameterSqlInfo(IParameterSqlInfo info, String tableName)
        {
        }

        /// <summary>
        /// Constructor that sets the operator to that provided, ensuring it
        /// is converted to upper case
        /// </summary>
        /// <param name="sqlOperator">The new operator</param>
        public SqlOperator(string sqlOperator)
        {
            //TODO: Error check valid inputs
            mOperator = sqlOperator.ToUpper();
        }

        /// <summary>
        /// Returns the expression string, which in this case is the operator
        /// being represented
        /// </summary>
        /// <returns>Returns the operator</returns>
        public string ExpressionString()
        {
            return mOperator;
        }

        /// <summary>
        /// Adds the operator to the end of the given sql statement.
        /// See IExpression.SqlExpressionString for more detail on the
        /// format of the arguments.
        /// </summary>
        public void SqlExpressionString(ISqlStatement statement, string tableNameFieldNameLeftSeparator,
                                        string tableNameFieldNameRightSeparator)
        {
            statement.Statement.Append(mOperator);
        }

//		public string SqlExpressionString(string tableNameFieldNameLeftSeperator,
//		                                  string tableNameFieldNameRightSeperator,
//		                                  string DateTimeLeftSeperator,
//		                                  string DateTimeRightSeperator) {
//			return mOperator;
//		}
    }
}