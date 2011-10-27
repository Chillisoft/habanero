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
using System;
using Habanero.Base;
using Habanero.Util;

//using Habanero.BO;

namespace Habanero.BO.CriteriaManager
{
    
    /// <summary>
    /// The parameter is a terminal expression that is just a specialised 
    /// IExpression.  It was created separately from Expression so that it 
    /// could store the additional parameters of the field name, table name 
    /// and parameter type.  These are necessary so that the criteria manager 
    /// component can return a valid SqlExpressionString when required.
    /// </summary>
    /// <remarks> 
    /// The reason for storing this additional info in the expression manager 
    /// and not parsing it every time a sql statement was required is partly 
    /// down to performance and partly down to ease of use.
    /// </remarks>
    /// Future Enhancements:
    /// - add error checking such that only valid sqlOperators can be passed 
    /// in and s.t. the value is valid, e.g. if operator is "IS" then value 
    /// must be null or not null
    public class Parameter : IExpression
    {
        private readonly string _parameterName;
        private string _tableName = "";
        private string _fieldName;
        private readonly string _sqlOperator;
        private readonly string _parameterValue;
        private ParameterType _parameterType = ParameterType.String;
        private const string _defaultValueSeperator = "'";

        #region Constructors

        /// <summary>
        /// Constructor that creates a parameter based on the parameter clause
        /// provided
        /// </summary>
        /// <param name="parameterClause">A clause for a single parameter. 
        /// This must have the syntax:<br/>
        /// <code>parameterName sqlOperator parameterValue</code>
        /// (e.g. <code>Field1 >= 3</code>)<br/> 
        /// NOTE_: The parameter value should not be enclosed in inverted 
        /// commas.</param>
        public Parameter(string parameterClause)
        {
            CriteriaExpression c = new CriteriaExpression(parameterClause);
            _parameterName = c.Left.Expression;
            _fieldName = _parameterName;
            _sqlOperator = c.Expression;
            _parameterValue = c.Right.Expression;
        }

        /// <summary>
        /// Constructor that creates a parameter with the parameter name, 
        /// sql operator and parameter value provided.
        /// It is assumed that the database field name is equal to the 
        /// parameter name unless it is reset later.
        /// </summary>
        /// <param name="parameterName">The property name of the parameter</param>
        /// <param name="sqlOperator">The sql operator</param>
        /// <param name="parameterValue">This should be a parameter as per a sql 
        /// "where" clause. NOTE_: Do not parse out a ' as '' since the criteria 
        /// manager will do this.</param>
        public Parameter(string parameterName, string sqlOperator, string parameterValue)
        {
            //TODO: Error check valid inputs
            _parameterName = parameterName;
            _sqlOperator = sqlOperator.ToUpper();
            _parameterValue = parameterValue;
            _fieldName = parameterName;
        }

        /// <summary>
        /// A constructor as before, except that the field name is explicitly
        /// provided
        /// </summary>
        public Parameter(string parameterName,
                         string fieldName, string sqlOperator,
                         string parameterValue) : this(parameterName, sqlOperator, parameterValue)
        {
            //TODO: Error check valid inputs
            _fieldName = fieldName;
        }

        /// <summary>
        /// A constructor as before, except that the table name and field name
        /// are explicitly provided.  For other constructors, the table
        /// name is initialised as an empty string.
        /// </summary>
        public Parameter(string parameterName,
                         string tableName,
                         string fieldName,
                         string sqlOperator,
                         string parameterValue) : this(parameterName, sqlOperator, parameterValue)
        {
            //TODO: Error check valid inputs
            _tableName = tableName;
            _fieldName = fieldName;
        }

        /// <summary>
        /// A constructor as before, except that the parameter type is
        /// explicitly provided.  For all other constructors, this is
        /// initialised as a string type.
        /// </summary>
        public Parameter(string parameterName,
                         string tableName,
                         string fieldName,
                         string sqlOperator,
                         string parameterValue,
                         ParameterType parameterType)
            : this(parameterName, tableName, fieldName, sqlOperator, parameterValue)
        {
            //TODO: Error check valid inputs
            _parameterType = parameterType;
        }

        #endregion //Constructors
        
        #region IExpression Interface Implementation

        /// <summary>
        /// Returns an expression string consisting of the parameter 
        /// name, operator and parameter value
        /// </summary>
        /// <returns>Returns the expression string</returns>
        public string ExpressionString()
        {
            return
                _parameterName + " " + _sqlOperator + " " + _defaultValueSeperator + _parameterValue +
                _defaultValueSeperator;
        }
        
        /// <summary>
        /// Creates a valid sql expression, e.g. for a "where" clause.
        /// See IExpression.SqlExpressionString for more detail.
        /// </summary>
        public void SqlExpressionString(ISqlStatement statement, string tableNameFieldNameLeftSeparator,
                                        string tableNameFieldNameRightSeparator)
        {
            statement.Statement.Append(
                FieldFullName(tableNameFieldNameLeftSeparator, tableNameFieldNameRightSeparator) + GetSqlOperator());
            if (this.DoesntRequireParametrisedValue())
            {
                statement.Statement.Append(GetSqlStringWithNoParameters());
            }
            else
            {
                statement.AddParameterToStatement(GetParameterValueAsObject());
            }
        }

        #region SqlExpressionString Utility Methods

        /// <summary>
        /// Returns the full field name, including table name (where applicable),
        /// field name and surrounding separators.  For instance, with the
        /// separators as "[" and "]", the output would be:<br/>
        /// <code>[tablename].[fieldname]</code><br/>
        /// See IExpression.SqlExpressionString for more detail.
        /// </summary>
        /// <returns>Returns a string with the full name</returns>
        internal string FieldFullName(string tableFieldNameLeftSeperator,
                                     string tableFieldNameRightSeperator)
        {
            if (_tableName.Length > 0)
            {
                return tableFieldNameLeftSeperator + _tableName + tableFieldNameRightSeperator + "." +
                       tableFieldNameLeftSeperator + _fieldName + tableFieldNameRightSeperator;
            }
            return tableFieldNameLeftSeperator + _fieldName + tableFieldNameRightSeperator;
        }

        /// <summary>
        /// Returns the parameter value in valid sql format
        /// </summary>
        /// <returns>Returns a string</returns>
        internal string GetSqlStringWithNoParameters()
        {
            string strOp = _sqlOperator.ToUpper().Trim();
            if (strOp == "IS" || strOp == "IS NOT" || strOp == "NOT IS")
            {
                return _parameterValue.ToUpper(); //return either NULL or NOT NULL 
            }
            if (strOp == "IN" || strOp == "NOT IN")
            {
                return _parameterValue;
            }
            return "";
        }

        /// <summary>
        /// Converts the parameter value into an object, based on its
        /// specified parameter type (see the ParameterType enumeration for
        /// more detail).  For instance, a number type will be converted to
        /// a Decimal object.
        /// </summary>
        /// <returns>Returns the value as an object</returns>
        internal object GetParameterValueAsObject()
        {
            switch (_parameterType)
            {
                case ParameterType.Bool:
                    return Convert.ToBoolean(_parameterValue);
                case ParameterType.Date:
                     return DateTimeUtilities.ParseToDate(_parameterValue);
                case ParameterType.Number:
                    return Convert.ToDecimal(_parameterValue);
                case ParameterType.String:
                    return _parameterValue;
            }
            return _parameterValue;
        }
        
        /// <summary>
        /// Indicates whether the sql operator is some variant of "IS" or "IN",
        /// in which case a parameterised value is not required
        /// </summary>
        /// <returns>True if not required, false if required</returns>
        public bool DoesntRequireParametrisedValue()
        {
            string strOp = _sqlOperator.ToUpper().Trim();
            return (strOp == "IS" || strOp == "IS NOT" || strOp == "NOT IS" || strOp == "IN" || strOp == "NOT IN");
        }

        #endregion //SqlExpressionString Utility Methods

        //		/// <summary>
//		/// Create a valid sql expression e.g. for a whereclause.
//		/// </summary>
//		/// <param name="tableFieldNameLeftSeperator">The left field seperator used
//		/// when building up a database where clause this is different in different databases but
//		/// is used to ensure that the where clause is always valid even if the fieldname is a 
//		/// reserved word for the particular database. (This is usually '[' for sql etc databases) </param>
//		/// <param name="tableFieldNameRightSeperator">The right field seperator used
//		/// when building up a database where clause this is different in different databases but
//		/// is used to ensure that the where clause is always valid even if the fieldname is a 
//		/// reserved word for the particular database. (This is usually ']' for sql etc databases) </param>
//		/// <param name="DateTimeLeftSeperator">In MS Access the datatime fields are not surrounded by ' but instead by
//		/// # e.g. datetimeField >= #01 jan 2004#</param>
//		/// <param name="DateTimeLeftSeperator">In MS Access the datatime fields are not surrounded by ' but instead by
//		/// # e.g. datetimeField >= #01 jan 2004#. Oracle 8i requires date fields to be surrounded by
//		/// TO_DATE("01 Jan 2004", "dd/mm/yyy") etc</param>
//		/// <returns> returns a string with syntax tableFieldNameLeftSeperator  
//		/// tableName  tableFieldNameRightSeperator  tableFieldNameLeftSeperator 
//		/// fieldName  tableFieldNameRightSeperator = Value</returns>
//		/// <example> tableFieldNameLeftSeperator = "["
//		/// tableFieldNameRightSeperator = "]" will result in the following stringExpression
//		/// for a text data type
//		/// [tableName].[fieldName] = 'value'</example>
//		public string SqlExpressionString(string tableFieldNameLeftSeperator,
//		                                  string tableFieldNameRightSeperator,
//		                                  string DateTimeLeftSeperator,
//		                                  string DateTimeRightSeperator) {
//			return FieldFullName(tableFieldNameLeftSeperator, tableFieldNameRightSeperator) +
//				GetSqlOperator() + SqlParameterValue(DateTimeLeftSeperator, DateTimeRightSeperator);
//		}

        /// <summary>
        /// Copies across the parameterised sql info (see IParameterSqlInfo for
        /// more detail)
        /// </summary>
        /// <param name="info">The IParameterSqlInfo object</param>
        public void SetParameterSqlInfo(IParameterSqlInfo info)
        {
            if (info.ParameterName.ToUpper() == this._parameterName.ToUpper())
            {
                _tableName = info.TableName;
                _fieldName = info.FieldName;
                _parameterType = info.ParameterType;
            }
        }

        ///<summary>
        /// Creates and returns a copy of this IExpression instance.
        ///</summary>
        ///<returns>Returns a copy of this IExpression instance.</returns>
        public IExpression Clone()
        {
            return new Parameter(_parameterName, _tableName, _fieldName, _sqlOperator, _parameterValue, _parameterType);
        }

        #endregion //IExpression Interface Implementation

        /// <summary>
        /// Returns the sql operator, with a space before it
        /// </summary>
        /// <returns>Returns the operator as a string</returns>
        private string GetSqlOperator()
        {
            return " " + _sqlOperator + " ";
        }
        
        ///<summary>
        /// The parameter name for this Parameter
        ///</summary>
        public string ParameterName
        {
            get { return _parameterName; }
        }
    }
}