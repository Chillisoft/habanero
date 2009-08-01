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

using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// Checks date values against property rules that test for validity
    /// </summary>
    public class PropRuleDate : PropRuleBase
    {
        private DateTime _minValue = DateTime.MinValue;
		private DateTime _maxValue = DateTime.MaxValue;
        private string _minValueExpression;
        private string _maxValueExpression;
        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        public PropRuleDate(string ruleName,
                            string message) 
			: this(ruleName, message, null, null)
        {
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        /// <param name="minValue">The minimum date that can be set</param>
        /// <param name="maxValue">The maximum date that can be set</param>
        public PropRuleDate(string ruleName,
                            string message,
                            DateTime? minValue,
							DateTime? maxValue)
			: base(ruleName, message)
        {
			// if the nullable minvalue is null, then set it to DateTime.MinValue.
			_minValue = minValue ?? DateTime.MinValue;
			// if the nullable maxValue is null, then set it to DateTime.MaxValue.
			_maxValue = maxValue ?? DateTime.MaxValue;
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        /// <param name="parameters">The parameters for this rule.</param>
        public PropRuleDate(string name, string message, Dictionary<string, object> parameters)
			: base(name, message)
        {
        	base.Parameters = parameters;
        }

        /// <summary>
        /// Sets up the parameters to the rule, that is the individual pairs
        /// of rule type and rule value that make up the composite rule
        /// </summary>
		protected internal override void SetupParameters()
		{
            try
            {
                foreach (string key in _parameters.Keys)
                {
                    object value = Parameters[key];
                    if (value == null) continue;
                    if (value is string)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(value))) return;
                    }
                    switch (key)
                    {
                        case "min":
                            if (Convert.ToString(value) == "Today" || Convert.ToString(value) == "Now")
                            {
                                _minValueExpression = Convert.ToString(value);
                            }
                            else
                            {
                                _minValue = Convert.ToDateTime(value);
                            }
                            break;
                        case "max":
                            if (Convert.ToString(value) == "Today" || Convert.ToString(value) == "Now")
                            {
                                _maxValueExpression = Convert.ToString(value);
                            }
                            else
                            {
                                _maxValue = Convert.ToDateTime(value);
                            }
                            break;
                        default:
                            throw new InvalidXmlDefinitionException
                                (String.Format
                                     ("The rule type '{0}' for dates does not exist. "
                                      + "Check spelling and capitalisation, or see the "
                                      + "documentation for existing options or ways to "
                                      + "add options of your own.", key));
                    }
                }
            }
            catch (InvalidXmlDefinitionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("An error occurred " +
                    "while processing the property rules for a date.  The " +
                    "likely cause is that one of the attributes in the 'add' " +
                    "element of the class definitions has an invalid value.", ex);
            }
		}

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="displayName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        public override bool IsPropValueValid(string displayName, Object propValue,
                                                          ref string errorMessage)
        {
            errorMessage = "";
            if (!base.IsPropValueValid(displayName, propValue, ref errorMessage))
            {
                return false;
            }
            if (propValue == null)
            {
                return true;
            }
            if (!(propValue is DateTime))
            {
                errorMessage = GetBaseErrorMessage(propValue, displayName)
                        + "It is not a type of DateTime.";
                return false;
            }
            if ((DateTime) propValue < MinValue)
            {
                errorMessage = GetBaseErrorMessage(propValue, displayName);
                if (!String.IsNullOrEmpty(Message))
                {
                    errorMessage += Message;
                }
                else
                {
                    errorMessage += "The date cannot be before " + MinValue + ".";
                }
                return false;
            }
            if ((DateTime) propValue > MaxValue)
            {
                errorMessage = GetBaseErrorMessage(propValue, displayName);
                if (!String.IsNullOrEmpty(Message))
                {
                    errorMessage += Message;
                }
                else
                {
                    errorMessage += "The date cannot be after " + MaxValue + ".";
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the list of available parameter names for the rule.
        /// </summary>
        /// <returns>A list of the parameters that this rule uses</returns>
        public override List<string> AvailableParameters
        {
            get
            {
                List<string> parameters = new List<string> { "min", "max" };
                return parameters;
            }
        }


    	/// <summary>
        /// Returns the minimum value the date can be
        /// </summary>
        public DateTime MinValue
        {
            get
            {
                return GetValue(_minValueExpression, _minValueExpression == "Now" ? DateTime.Now : _minValue);
            }
			protected set { _minValue = value; }
		}

       /// <summary>
        /// Returns the maximum value the date can be
        /// </summary>
        public DateTime MaxValue
        {
            get
            {
                return GetValue(_maxValueExpression, _maxValueExpression == "Now" ? DateTime.Now : _maxValue);
            }
           protected set { _maxValue = value; }
        }

        private static DateTime GetValue(string valueExpression, DateTime value)
        {
            if (valueExpression == "Today")
            {
                return DateTime.Today;
            }
            return value;
        }
    }

}