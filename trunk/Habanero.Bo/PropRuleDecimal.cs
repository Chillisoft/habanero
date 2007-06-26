using System;
using System.Collections.Generic;

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
            if (parameters.ContainsKey("min")) _minValue = Convert.ToDecimal(parameters["min"]);
            if (parameters.ContainsKey("max")) _maxValue = Convert.ToDecimal(parameters["max"]);
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
    }
}