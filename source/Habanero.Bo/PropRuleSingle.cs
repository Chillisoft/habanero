using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;

namespace Habanero.BO
{
    /// <summary>
    /// Checks Single values against property rules that test for validity
    /// </summary>
    public class PropRuleSingle : PropRuleBase
    {
        private Single _minValue = Single.MinValue;
        private Single _maxValue = Single.MaxValue;

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
            _minValue = minValue;
            _maxValue = maxValue;
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        /// <param name="parameters">The parameters for this rule.</param>
        public PropRuleSingle(string name, string message, Dictionary<string, object> parameters)
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
                    if (value == null) return;
                    if (value is string)
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(value))) return;
                    }
                    switch (key)
                    {
                        case "min":
                            _minValue = Convert.ToSingle(value);
                            break;
                        case "max":
                            _maxValue = Convert.ToSingle(value);
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
        /// Gets and sets the minimum value that the Single can be assigned
        /// </summary>
        public Single MinValue
        {
            get { return _minValue; }
            protected set { _minValue = value; }
        }

        /// <summary>
        /// Gets and sets the maximum value that the Single can be assigned
        /// </summary>
        public Single MaxValue
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
            if (propValue is Single)
            {
                Single SinglePropRule = (Single)propValue;
                if (SinglePropRule < _minValue)
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
                if (SinglePropRule > _maxValue)
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