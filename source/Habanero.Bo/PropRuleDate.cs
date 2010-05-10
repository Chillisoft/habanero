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
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Util;

namespace Habanero.BO
{
    /// <summary>
    /// Checks date values against property rules that test for validity
    /// </summary>
    public class PropRuleDate : PropRuleBase, IPropRuleComparable<DateTime>
    {
        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        public PropRuleDate(string ruleName,
                            string message) 
			: base(ruleName, message)
        {
            InitialiseParameters(DateTime.MinValue, DateTime.MaxValue);
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
                            DateTime minValue,
							DateTime maxValue)
			: base(ruleName, message)
        {
            InitialiseParameters(minValue, maxValue);
        }

        private void InitialiseParameters(DateTime minValue, DateTime maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        /// <summary>
        /// Sets up the parameters to the rule, that is the individual pairs
        /// of rule type and rule value that make up the composite rule
        /// </summary>
		protected internal override void SetupParameters()
		{
            try
            {
                string[] keys = new string[_parameters.Keys.Count];
                _parameters.Keys.CopyTo(keys, 0);
                foreach (string key in keys)
                {
                    object value = Parameters[key];
                    if (value == null) continue;
                    string valueExpression = Convert.ToString(value);
                    if (value is string)
                    {
                        if (string.IsNullOrEmpty(valueExpression)) return;
                    }
                    switch (key)
                    {
                        case "min":
                            if (value is string && string.IsNullOrEmpty((string)value))
                                MinValueExpression = "";
                            else
                            {
                                DateTimeUtilities.ParseToDate(valueExpression);
                                    //The output from this is not used but if the valueExpression
                                // cannot be parsed to any valid datetime then an error will be raised. It is preferable that the error 
                                // is raised early
                                MinValueExpression = valueExpression;
                            }
                            break;
                        case "max":
                            if (value is string && string.IsNullOrEmpty((string)value))
                                MaxValueExpression = "";
                            else
                            {
                                DateTimeUtilities.ParseToDate(valueExpression);
                                    //The output from this is not used but if the valueExpression
                                // cannot be parsed to any valid datetime then an error will be raised. It is preferable that the error 
                                // is raised early
                                MaxValueExpression = valueExpression;
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
/*
        private static bool IsIResolvableToValue(object value)
        {
            string valueAsString = Convert.ToString(value);
            return valueAsString == TODAY_STRING || valueAsString == NOW_STRING || valueAsString == YESTERDAY_STRING;
        }*/

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
            if (!base.IsPropValueValid(displayName, propValue, ref errorMessage)) return false;
            //It is not necessary to check compulsory rules since these are checked in the base class (see previous line)
            if (propValue == null) return true;
            if (!IsPropValueADate(displayName, propValue, out errorMessage)) return false;
            return IsPropValueLtMinValue(displayName, propValue, out errorMessage) && IsPropValueGtMaxValue(displayName, propValue, out errorMessage);
        }

        private bool IsPropValueADate(string displayName, object propValue, out string errorMessage)
        {
            errorMessage = "";
            if (!(propValue is DateTime))
            {
                errorMessage = GetBaseErrorMessage(propValue, displayName)
                               + "It is not a type of DateTime.";
                return false;
            }
            return true;
        }

        private bool IsPropValueGtMaxValue(string displayName, object propValue, out string errorMessage)
        {
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
            errorMessage = "";
            return true;
        }

        private bool IsPropValueLtMinValue(string displayName, object propValue, out string errorMessage)
        {
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
            errorMessage = "";
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
                object minValue = Parameters["min"];
                if (minValue is string && String.IsNullOrEmpty((string)minValue)) return DateTime.MinValue;
                return DateTimeUtilities.ParseToDate(minValue);
            }
            protected set { _parameters["min"] = value; }
		}

        /// <summary>
        /// Returns the maximum value the date can be
        /// </summary>
        public DateTime MaxValue
        {
            get
            {
                object maxValue = Parameters["max"];
                try
                {
                    if (maxValue is string && String.IsNullOrEmpty((string)maxValue)) return DateTime.MaxValue;
                    DateTime dateTime = DateTimeUtilities.ParseToDate(maxValue);
                    return dateTime >= DateTime.MaxValue.AddDays(-1) ? dateTime : dateTime.AddDays(1).AddMilliseconds(-1);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException("The value '" + maxValue + "' could not be converted to a valid MaxValue");
                }
            }
            protected set { _parameters["max"] = value; }
        }

        private string MaxValueExpression
        {
            set { _parameters["max"] = value; }
        }
        private string MinValueExpression
        {
            set { _parameters["min"] = value; }
        }

    }

}