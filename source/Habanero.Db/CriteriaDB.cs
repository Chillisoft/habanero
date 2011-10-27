#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.Base
{
    ///<summary>
    /// This class inherits from the <see cref="Criteria"/> class and implements a
    /// <see cref="ToString(SqlFormatter, AddParameterDelegate)"/> behaviour.
    /// This allows the formatting of a criteria object into a format specific for the database.
    ///</summary>
    public class CriteriaDB : Criteria
    {
        #region Delegates

        /// <summary>
        /// For details of what this delegate is used for, see <see cref="PropNameConverterDelegate"/>
        /// 
        /// This delegate is designed to format a value of a criteria into a datasource equivalent. For example, in a database context,
        /// this delegate might be used to return a parameter name and store the parameter value so that the criteria can be used as part
        /// of a where clause in a parametrized SQL statement.
        /// </summary>
        /// <param name="parameterValue">The value of the criteria given, as an object. </param>
        /// <returns>The string format of this criteria to append to the string.</returns>
        public delegate string AddParameterDelegate(object parameterValue);

        ///<summary>
        /// When converting the Criteria expression to a string there are contexts that must be taken into account. The default implementation
        /// (<see cref="Criteria.ToString()"/>) will not do any conversions of property names or values given other than to convert
        /// DateTimes and Guids to sensible, universal string equivalents (see <see cref="Criteria.DATE_FORMAT"/>).
        /// 
        /// This delegate is designed to map a property name to a datasource name (such as to a field name on table).
        ///</summary>
        ///<param name="propName">The property name to map to the datasource equivalent</param>
        /// <returns>The mapped property name ie the datasource equivalent of the property name (perhaps the field name on a database table)</returns>
        public delegate string PropNameConverterDelegate(string propName);

        #endregion

        private readonly Criteria _criteria;

        ///<summary>
        /// Constructor for a Database critieria. takes the Criteria object that it wraps as a parameter.
        ///</summary>
        ///<param name="criteria">The criteria object being wrapped.</param>
        public CriteriaDB(Criteria criteria)
        {
            _criteria = criteria;
            if (criteria == null) _criteria = new Criteria(null, LogicalOp.And, null);
        }

        ///<summary>
        ///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override string ToString()
        {
            return _criteria.ToString();
        }

        ///<summary>
        /// Gets the query Field being used by this criteria object (Where the criteria object is a leaf).
        /// the query field is a query field object representing the objects property as defined in the 
        /// Constructor [Criteria(string propName, ComparisonOp op, object value)].
        ///</summary>
        public override QueryField Field
        {
            get { return _criteria.Field; }
        }

        ///<summary>
        /// Gets the left critieria object (If this is not a leaf criteria)
        ///</summary>
        public override Criteria LeftCriteria
        {
            get { return _criteria.LeftCriteria; }
        }

        ///<summary>
        /// Gets the logical operator being used for this criteria object (If this is not a leaf criteria)
        ///</summary>
        public override LogicalOp LogicalOperator
        {
            get { return _criteria.LogicalOperator; }
        }

        ///<summary>
        /// Gets the right criteria object (If this is not a leaf criteria)
        ///</summary>
        public override Criteria RightCriteria
        {
            get { return _criteria.RightCriteria; }
        }

        ///<summary>
        /// Gets or sets the field value being compared to for this criteria object (If this is a leaf criteria)
        ///</summary>
        public override object FieldValue
        {
            get { return _criteria.FieldValue; }
        }

        ///<summary>
        /// Gets the comparison operator being used by this Criteria object (If this is a leaf criteria)
        ///</summary>
        public override ComparisonOp ComparisonOperator
        {
            get { return _criteria.ComparisonOperator; }
        }

        public string ToString(ISqlFormatter formatter, AddParameterDelegate addParameter)
        {
            return ToString(formatter, addParameter, new Dictionary<string, string>());
        }

        /// <summary>
        /// Converts this Criteria object to a string, using field names instead of property names and entity names instead of
        /// source names. The <see cref="AddParameterDelegate"/> allows a database query builder to create a parameter value
        /// when adding the value to the string for use with parametrized SQL.  Also see <see cref="ISqlStatement"/>.
        /// 
        /// The <see cref="ToString()"/> method uses this method with a simple delegate that converts DateTimes and Guids 
        /// to sensible string representations and to 
        /// </summary>
        /// See <see cref="PropNameConverterDelegate"/>
        /// <param name="formatter">A formatter for any specific database <see cref="SqlFormatter"/></param>
        /// <param name="addParameter">The delegate to use to convert the value in object form to a value in string form. 
        /// See <see cref="AddParameterDelegate"/></param>
        /// <returns>The Criteria in string form.</returns>
        public string ToString(ISqlFormatter formatter, AddParameterDelegate addParameter, IDictionary<string, string> aliases)
        {
            if (IsComposite())
            {
                string rightCriteriaAsString;
                if (LogicalOperator == LogicalOp.Not)
                {
                    rightCriteriaAsString = new CriteriaDB(RightCriteria).ToString(formatter, addParameter, aliases);
                    return string.Format("{0} ({1})", _logicalOps[(int)LogicalOperator], rightCriteriaAsString);
                } 
                string leftCriteriaAsString = new CriteriaDB(LeftCriteria).ToString(formatter, addParameter, aliases);
                rightCriteriaAsString = new CriteriaDB(RightCriteria).ToString(formatter, addParameter, aliases);
                return string.Format("({0}) {1} ({2})", leftCriteriaAsString, _logicalOps[(int)LogicalOperator],
                                     rightCriteriaAsString);
            }
            string valueString;
            string comparisonOperator = ComparisonOperatorString();
            if (_criteria.CanBeParametrised())
            {
                valueString = addParameter(FieldValue);
            } else
            {
                if (FieldValue == null)
                {
                    valueString = "NULL";
                    if (this.ComparisonOperator == ComparisonOp.Equals) comparisonOperator = "IS";
                    if (this.ComparisonOperator == ComparisonOp.NotEquals) comparisonOperator = "IS NOT";
                }
                else
                {
                    valueString = Convert.ToString(FieldValue);
                }

            }
            string sourceEntityName = "";
            string separator = "";
            if (Field.Source != null)
            {
            	var fieldSourceName = Field.Source.ChildSourceLeaf.ToString();
            	if (!aliases.ContainsKey(fieldSourceName))
				{
					var userMessage = string.Format("The source '{0}' for the property '{1}' " 
						+ "in the given criteria does not have an alias provided for it.",
						Field.Source, Field.PropertyName);
					var developerMessage = userMessage
						+ " The criteria object may have not been prepared correctly before the aliases were set up.";
					throw new HabaneroDeveloperException(userMessage, developerMessage);
				}
				sourceEntityName = aliases[fieldSourceName];
                separator = ".";
            }
            string fieldNameString = formatter.DelimitField(Field.FieldName);
            return string.Format("{0}{1}{2} {3} {4}", sourceEntityName, separator, fieldNameString, comparisonOperator, valueString);
        }
    }
}