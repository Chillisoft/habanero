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
using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// Checks decimal values against property rules that test for validity
    /// </summary>
    public class PropRuleDecimal : PropRuleBase
    {
        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        /// <param name="minValue">The minimum value allowed for the decimal</param>
        /// <param name="maxValue">The maximum value allowed for the decimal</param>
        public PropRuleDecimal(string ruleName, string message, decimal minValue, decimal maxValue)
            : base(ruleName, message)
        {
            InitialiseParameters(minValue, maxValue);
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        public PropRuleDecimal(string name, string message)
			: base(name, message)
		{
            InitialiseParameters(Decimal.MinValue, Decimal.MaxValue);
		}

        private void InitialiseParameters(Decimal minValue, Decimal maxValue)
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
                    object value = _parameters[key];
                    if (value == null) return;
                    if (value is string)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(value))) return;
                    }
                    switch (key)
                    {
                        case "min":
                            MinValue = Convert.ToDecimal(value);
                            break;
                        case "max":
                            MaxValue = Convert.ToDecimal(value);
                            break;
                        default:
                            throw new InvalidXmlDefinitionException
                                (String.Format
                                     ("The rule type '{0}' for decimals does not exist. "
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
                    "while processing the property rules for a decimal.  The " +
                    "likely cause is that one of the attributes in the 'add' " +
                    "element of the class definitions has an invalid value.", ex);
            }
        }

        /// <summary>
        /// Gets and sets the minimum value that the Double can be assigned
        /// </summary>
        public Decimal MinValue
        {
            get { return Convert.ToDecimal(_parameters["min"]); }
            protected set { _parameters["min"] = value; }
        }

        /// <summary>
        /// Gets and sets the maximum value that the Double can be assigned
        /// </summary>
        public Decimal MaxValue
        {
            get { return Convert.ToDecimal(_parameters["max"]); }
            protected set { _parameters["max"] = value; }
        }

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="displayName">The property name being checked</param>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        public override bool IsPropValueValid(string displayName, object propValue, ref string errorMessage)
        {
            bool valueValid = base.IsPropValueValid(displayName, propValue, ref errorMessage);
            if (propValue is decimal)
            {
                decimal decimalPropRule = (decimal)propValue;
                if (decimalPropRule < MinValue)
                {
                    errorMessage = GetBaseErrorMessage(propValue, displayName);
                    if (!String.IsNullOrEmpty(Message))
                    {
                        errorMessage += Message;
                    }
                    else
                    {
                        errorMessage += "The value cannot be less than " + MinValue + " .";
                    }
                    valueValid = false;
                }
                if (decimalPropRule > MaxValue)
                {
                    errorMessage = GetBaseErrorMessage(propValue, displayName);
                    if (!String.IsNullOrEmpty(Message))
                    {
                        errorMessage += Message;
                    }
                    else
                    {
                        errorMessage += "The value cannot be more than " + MaxValue + " .";
                    }
                    valueValid = false;
                }
            }
            return valueValid;

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
    }
}