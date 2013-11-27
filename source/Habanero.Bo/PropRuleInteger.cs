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
using Habanero.Util;

namespace Habanero.BO
{
    /// <summary>
    /// Checks integer values against property rules that test for validity
    /// </summary>
    public class PropRuleInteger : PropRuleIntegerBase<int>, IPropRuleComparable<int>
    {
        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param> 
        /// <param name="min">The minimum value allowed for the integer</param>
        /// <param name="max">The maximum value allowed for the integer</param>
        public PropRuleInteger(string name, string message, int min, int max) : base(name, message)
        {
            InitialiseParameters(min, max);
        }

        private void InitialiseParameters(int min, int max)
        {
            MinValue = min;
            MaxValue = max;
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        public PropRuleInteger(string name, string message) : base(name, message)
        {
            InitialiseParameters(int.MinValue, int.MaxValue);
        }

        /// <summary>
        /// Sets up the parameters to the rule, that is the individual pairs
        /// of rule type and rule value that make up the composite rule
        /// </summary>
        protected internal override void SetupParameters()
        {
            try
            {
                var keys = new string[_parameters.Keys.Count];
                _parameters.Keys.CopyTo(keys, 0);
                foreach (string key in keys)
                {
                    object value = _parameters[key];

                    switch (key)
                    {
                        case "min":
                            if (value is string && string.IsNullOrEmpty((string)value))
                                MinValue = Int32.MinValue;
                            else MinValue = Convert.ToInt32(value);
                            break;
                        case "max":
                            if (value is string && string.IsNullOrEmpty((string)value))
                                MaxValue = Int32.MaxValue;
                            else MaxValue = Convert.ToInt32(value);
                            break;
                        default:
                            throw new InvalidXmlDefinitionException
                                (String.Format
                                     ("The rule type '{0}' for integers does not exist. "
                                      + "Check spelling and capitalisation, or see the "
                                      + "documentation for existing options or ways to " + "add options of your own.",
                                      key));
                    }
                }
            }
            catch (InvalidXmlDefinitionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException
                    ("An error occurred " + "while processing the property rules for an integer.  The "
                     + "likely cause is that one of the attributes in the 'add' "
                     + "element of the class definitions has an invalid value.", ex);
            }
        }

        /// <summary>
        /// Gets and sets the minimum value that the integer can be assigned
        /// </summary>
        public override int MinValue
        {
            get { return Convert.ToInt32(Parameters["min"]); }
            set { Parameters["min"] = value; }
        }

        /// <summary>
        /// Gets and sets the maximum value that the integer can be assigned
        /// </summary>
        public override int MaxValue
        {
            get { return Convert.ToInt32(Parameters["max"]); }
            set { Parameters["max"] = value; }
        }

        /// <summary>
        /// Returns whether the supplied value is less than the MinValue
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        protected override bool IsLessThanMinValue(int value)
        {
            return value < MinValue;
        }

        /// <summary>
        /// Returns whether the supplied value is greater than the MaxValue
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        protected override bool IsGreaterThanMaxValue(int value)
        {
            return value > MaxValue;
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
            var valueValid = base.IsPropValueValid(displayName, propValue, ref errorMessage);
            if (propValue == null) return true;
            if (propValue.GetType().IsInteger())
            {
                try
                {
                    var intValue = Convert.ToInt32(propValue);
                    valueValid = CheckValueAgainstAcceptedRange(displayName, intValue, ref errorMessage);
                }
                catch (OverflowException)
                {
                    errorMessage += string.Format("{0} value of {1} too large or small for an int", displayName, propValue);
                    return false;
                }
            }
            return valueValid;
        }
    }
}