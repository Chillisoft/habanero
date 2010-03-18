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
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// Checks Single values against property rules that test for validity
    /// </summary>
    public class PropRuleSingle : PropRuleBase, IPropRuleComparable<Single>
    {
        //private Single _minValue = Single.MinValue;
        //private Single _maxValue = Single.MaxValue;

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        /// <param name="minValue">The minimum value allowed for the Single</param>
        /// <param name="maxValue">The maximum value allowed for the Single</param>
        public PropRuleSingle(string ruleName, string message, Single minValue, Single maxValue)
            : base(ruleName, message)
        {
           InitialiseParameters(minValue, maxValue);
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        public PropRuleSingle(string name, string message)
            : base(name, message)
        {
            InitialiseParameters(Single.MinValue, Single.MaxValue);
        }

        private void InitialiseParameters(Single minValue, Single maxValue)
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
                            MinValue = Convert.ToSingle(value);
                            break;
                        case "max":
                            MaxValue = Convert.ToSingle(value);
                            break;
                        default:
                            throw new InvalidXmlDefinitionException
                                (String.Format
                                     ("The rule type '{0}' for Singles does not exist. "
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
                                                        "while processing the property rules for a Single.  The " +
                                                        "likely cause is that one of the attributes in the 'add' " +
                                                        "element of the class definitions has an invalid value.", ex);
            }
        }

        /// <summary>
        /// Gets and sets the minimum value that the Double can be assigned
        /// </summary>
        public Single MinValue
        {
            get { return Convert.ToSingle(_parameters["min"]); }
            protected set { _parameters["min"] = value; }
        }

        /// <summary>
        /// Gets and sets the maximum value that the Double can be assigned
        /// </summary>
        public Single MaxValue
        {
            get { return Convert.ToSingle(_parameters["max"]); }
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
            if (propValue is Single)
            {
                Single SinglePropRule = (Single)propValue;
                if (SinglePropRule < MinValue)
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
                if (SinglePropRule > MaxValue)
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