using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.Util;

namespace Habanero.BO
{
    /// <summary>
    /// Checks integer values against property rules that test for validity
    /// </summary>
    public class PropRuleShort : PropRuleIntegerBase<short>, IPropRuleComparable<short>
    {
        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param> 
        /// <param name="min">The minimum value allowed for the integer</param>
        /// <param name="max">The maximum value allowed for the integer</param>
        public PropRuleShort(string name, string message, short min, short max) : base(name, message)
        {
            InitialiseParameters(min, max);
        }

        private void InitialiseParameters(short min, short max)
        {
            MinValue = min;
            MaxValue = max;
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        public PropRuleShort(string name, string message) : base(name, message)
        {
            InitialiseParameters(short.MinValue, short.MaxValue);
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
                                MinValue = short.MinValue;
                            else MinValue = Convert.ToInt16(value);
                            break;
                        case "max":
                            if (value is string && string.IsNullOrEmpty((string)value))
                                MaxValue = short.MaxValue;
                            else MaxValue = Convert.ToInt16(value);
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
        public override short MinValue
        {
            get { return Convert.ToInt16(Parameters["min"]); }
            set { Parameters["min"] = value; }
        }

        /// <summary>
        /// Gets and sets the maximum value that the integer can be assigned
        /// </summary>
        public override short MaxValue
        {
            get { return Convert.ToInt16(Parameters["max"]); }
            set { Parameters["max"] = value; }
        }
        
        /// <summary>
        /// Returns whether the supplied value is less than the MinValue
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        protected override bool IsLessThanMinValue(short value)
        {
            return value < MinValue;
        }

        /// <summary>
        /// Returns whether the supplied value is greater than the MaxValue
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        protected override bool IsGreaterThanMaxValue(short value)
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
                    var value = Convert.ToInt16(propValue);
                    valueValid = CheckValueAgainstAcceptedRange(displayName, value, ref errorMessage);
                }
                catch (OverflowException)
                {
                    errorMessage += string.Format("{0} value of {1} too large or small for a short", displayName, propValue);
                    return false;
                }
            }
            return valueValid;
        }
    }
}