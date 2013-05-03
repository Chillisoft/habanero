using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// Checks integer values against property rules that test for validity
    /// </summary>
    public class PropRuleShort : PropRuleBase, IPropRuleComparable<short>
    {
        //private int _minValue = int.MinValue;
        //private int _maxValue = int.MaxValue;

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
        public short MinValue
        {
            get { return Convert.ToInt16(Parameters["min"]); }
            set { Parameters["min"] = value; }
        }

        /// <summary>
        /// Gets and sets the maximum value that the integer can be assigned
        /// </summary>
        public short MaxValue
        {
            get
            {
                return Convert.ToInt16(Parameters["max"]);
            }
            set
            {
                Parameters["max"] = value;
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
        public override bool IsPropValueValid(string displayName, object propValue, ref string errorMessage)
        {
            bool valueValid = base.IsPropValueValid(displayName, propValue, ref errorMessage);
            if (propValue is int)
            {
                int intPropRule = (int) propValue;
                if (intPropRule < MinValue)
                {
                    errorMessage = GetBaseErrorMessage(propValue, displayName);
                    if (!String.IsNullOrEmpty(Message))
                    {
                        errorMessage += Message;
                    }
                    else
                    {
                        errorMessage += "The value cannot be less than " + MinValue + ".";
                    }
                    valueValid = false;
                }
                if (intPropRule > MaxValue)
                {
                    errorMessage = GetBaseErrorMessage(propValue, displayName);
                    if (!String.IsNullOrEmpty(Message))
                    {
                        errorMessage += Message;
                    }
                    else
                    {
                        errorMessage += "The value cannot be more than " + MaxValue + ".";
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
                List<string> parameters = new List<string>();
                parameters.Add("min");
                parameters.Add("max");
                return parameters;
            }
        }
    }
}