using System;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.Bo
{
    /// <summary>
    /// Checks decimal values against property rules that test for validity
    /// </summary>
    /// TODO ERIC - where is validity checked?
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
            : base(ruleName, message, null)
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
			: base(name, message, parameters)
        {
            try
            {
                foreach (string key in parameters.Keys)
                {
                    switch (key)
                    {
                        case "min":
                            _minValue = Convert.ToDecimal(parameters["min"]);
                            break;
                        case "max":
                            _maxValue = Convert.ToDecimal(parameters["max"]);
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
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        protected internal override bool isPropValueValid(object propValue, ref string errorMessage)
        {
            bool valueValid = base.isPropValueValid(propValue, ref errorMessage);
            if (propValue is decimal)
            {
                decimal decimalPropRule = (decimal)propValue;
                if (decimalPropRule < _minValue)
                {
                    valueValid = false;
                    errorMessage += Environment.NewLine + "Please enter a value greater than " + _minValue + " for rule " + Name;
                }
                if (decimalPropRule > _maxValue)
                {
                    valueValid = false;
                    errorMessage += Environment.NewLine + "Please enter a value less than " + _maxValue + " for rule " + Name;
                }
            }
            return valueValid;

        }

    	protected internal override List<string> AvailableParameters()
    	{
			List<string> parameters = new List<string>();
			parameters.Add("min");
			parameters.Add("max");
			return parameters;
    	}
    }
}