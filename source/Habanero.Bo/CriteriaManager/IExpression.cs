using System;
using Habanero.Base;

namespace Habanero.BO.CriteriaManager
{
    /// <summary>
    /// IExpression is an interface for an Expression tree consisting of 
    /// one or more Expressions arranged in a binary tree. The Expression may 
    /// be terminal (i.e. it has no child expression) or it may 
    /// be a node in the expression tree (i.e. it consists of two 
    /// expressions and an operator).
    /// </summary>
    public interface IExpression
    {
        /// <summary>Return an Expression string consisting of the 
        /// parameter name operator and parameter value.</summary>
        /// <returns>Returns the expression string</returns>
        string ExpressionString();

        /// <summary>
        /// Creates a valid sql expression, e.g. for a "where" clause
        /// </summary>
        /// <param name="statement">The statement to append the new
        /// expression to</param>
        /// <param name="tableNameFieldNameLeftSeparator">The left field 
        /// separator used when building up a database "where" clause. This 
        /// is different for different databases, but is used to ensure that 
        /// the where clause is always valid, even if the fieldname is a 
        /// reserved word for the particular database. (This is usually '[' 
        /// for sql databases) </param>
        /// <param name="tableNameFieldNameRightSeparator">The right field 
        /// separator used when building up a database "where" clause. This 
        /// is different for different databases, but is used to ensure that 
        /// the where clause is always valid, even if the fieldname is a 
        /// reserved word for the particular database. (This is usually ']' 
        /// for sql databases) </param>
        /// <returns>Returns a string with syntax tableFieldNameLeftSeparator  
        /// tableName  tableFieldNameRightSeparator  dot  tableFieldNameLeftSeparator 
        /// fieldName  tableFieldNameRightSeparator = value</returns>
        /// <example>With the separators as "[" and "]", the resulting
        /// stringExpression for a text data type would be
        /// [tableName].[fieldName] = 'value'</example>
        void SqlExpressionString(ISqlStatement statement, string tableNameFieldNameLeftSeparator,
                                 string tableNameFieldNameRightSeparator);


        //  Previous form of above - I've left the old comments here in case - ERIC
        /// <param name="dateTimeLeftSeparator">In MS Access the datetime 
        /// fields are not surrounded by ' but instead by a hash (e.g. 
        /// datetimeField >= #01 jan 2004#)</param>
        /// <param name="dateTimeRightSeparator">In MS Access the datetime 
        /// fields are not surrounded by ' but instead by a hash (e.g. 
        /// datetimeField >= #01 jan 2004#)</param>
        /// <example> With the separators as "[" and "]", the resulting
        /// stringExpression for a text data type will be
        /// [tableName].[fieldName] = 'value'
        /// dateTimeLeftSeparator = "to_date(" and dateTimeRightSeparator = ,'dd/mm/yyyy')"
        /// will result in [tableName].[fieldName] = to_date(value,'dd/mm/yyyy') 
        /// if the field is set up as a date and 
        /// [tableName].[fieldName] = 'value' otherwise.</example>
        /// 
        //		SqlStatement SqlExpressionString(string tableNameFieldNameLeftSeparator,
        //		                           string tableNameFieldNameRightSeparator,
        //		                           string dateTimeLeftSeparator,
        //		                           string dateTimeRightSeparator);


        /// <summary>
        /// Copies across the parameterised sql info (see IParameterSqlInfo for
        /// more detail)
        /// </summary>
        /// <param name="info">The IParameterSqlInfo object</param>
        /// <param name="tableName">The table name</param>
        void SetParameterSqlInfo(IParameterSqlInfo info, String tableName);
    }
}