//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
    /// Checks decimal values against property rules that test for validity
    /// </summary>
    public class PropRuleDecimal : PropRuleBase
    {
        private decimal _minValue = Decimal.MinValue;
        private decimal _maxValue = Decimal.MaxValue;

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
            _minValue = minValue;
            _maxValue = maxValue;
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        /// <param name="parameters">The parameters for this rule.</param>
        public PropRuleDecimal(string name, string message, Dictionary<string, object> parameters)
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
                    object value = _parameters[key];
					if (value != null)
					{
						switch (key)
						{
							case "min":
								_minValue = Convert.ToDecimal(value);
								break;
							case "max":
								_maxValue = Convert.ToDecimal(value);
								break;
							default:
								throw new InvalidXmlDefinitionException(String.Format(
                                	"The rule type '{0}' for decimals does not exist. " +
                                	"Check spelling and capitalisation, or see the " +
                                	"documentation for existing options or ways to " +
                                	"add options of your own.", key));
						}
					}
                }
            }
            catch (InvalidXmlDefinitionException ex)
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
        /// Gets and sets the minimum value that the decimal can be assigned
        /// </summary>
        public decimal MinValue
        {
            get { return _minValue; }
        	protected set { _minValue = value; }
        }

        /// <summary>
        /// Gets and sets the maximum value that the decimal can be assigned
        /// </summary>
        public decimal MaxValue
        {
            get { return _maxValue; }
        	protected set { _maxValue = value; }
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
                if (decimalPropRule < _minValue)
                {
                    errorMessage = GetBaseErrorMessage(propValue, displayName);
                    if (!String.IsNullOrEmpty(Message))
                    {
                        errorMessage += Message;
                    }
                    else
                    {
                        errorMessage += "The value cannot be less than " + _minValue + " .";
                    }
                    valueValid = false;
                }
                if (decimalPropRule > _maxValue)
                {
                    errorMessage = GetBaseErrorMessage(propValue, displayName);
                    if (!String.IsNullOrEmpty(Message))
                    {
                        errorMessage += Message;
                    }
                    else
                    {
                        errorMessage += "The value cannot be more than " + _maxValue + " .";
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
    	protected internal override List<string> AvailableParameters()
    	{
			List<string> parameters = new List<string>();
			parameters.Add("min");
			parameters.Add("max");
			return parameters;
    	}
    }
}