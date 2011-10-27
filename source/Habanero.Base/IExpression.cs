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
namespace Habanero.Base
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
        ///// <param name="dateTimeLeftSeparator">In MS Access the datetime 
        ///// fields are not surrounded by ' but instead by a hash (e.g. 
        ///// datetimeField >= #01 jan 2004#)</param>
        ///// <param name="dateTimeRightSeparator">In MS Access the datetime 
        ///// fields are not surrounded by ' but instead by a hash (e.g. 
        ///// datetimeField >= #01 jan 2004#)</param>
        ///// <example> With the separators as "[" and "]", the resulting
        ///// stringExpression for a text data type will be
        ///// [tableName].[fieldName] = 'value'
        ///// dateTimeLeftSeparator = "to_date(" and dateTimeRightSeparator = ,'dd/mm/yyyy')"
        ///// will result in [tableName].[fieldName] = to_date(value,'dd/mm/yyyy') 
        ///// if the field is set up as a date and 
        ///// [tableName].[fieldName] = 'value' otherwise.</example>
        ///// 
        //		SqlStatement SqlExpressionString(string tableNameFieldNameLeftSeparator,
        //		                           string tableNameFieldNameRightSeparator,
        //		                           string dateTimeLeftSeparator,
        //		                           string dateTimeRightSeparator);


        /// <summary>
        /// Copies across the parameterised sql info (see IParameterSqlInfo for
        /// more detail)
        /// </summary>
        /// <param name="info">The IParameterSqlInfo object</param>
        void SetParameterSqlInfo(IParameterSqlInfo info);

        ///<summary>
        /// Creates and returns a copy of this IExpression instance.
        ///</summary>
        ///<returns>Returns a copy of this IExpression instance.</returns>
        IExpression Clone();
    }
}