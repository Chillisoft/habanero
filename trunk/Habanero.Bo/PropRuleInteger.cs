using System;
using System.Collections.Generic;

namespace Habanero.Bo
{
    /// <summary>
    /// Checks integer values against property rules that test for validity
    /// </summary>
    public class PropRuleInteger : PropRuleBase
    {
        private int _minValue = int.MinValue;
        private int _maxValue = int.MaxValue;

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param> 
        /// <param name="min">The minimum value allowed for the integer</param>
        /// <param name="max">The maximum value allowed for the integer</param>
        public PropRuleInteger(string name, string message, int min, int max) : base(name, message)
        {
            _minValue = min;
            _maxValue = max;
        }


        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        /// <param name="parameters">The parameters for this rule.  Valid parameters are "min" and "max"</param>
        public PropRuleInteger(string name, string message, Dictionary<string, object> parameters)
            : base(name, message)
        {
            if (parameters.ContainsKey("min")) _minValue = Convert.ToInt32(parameters["min"]);
            if (parameters.ContainsKey("max")) _maxValue = Convert.ToInt32(parameters["max"]);
        }



        /// <summary>
        /// Gets and sets the minimum value that the integer can be assigned
        /// </summary>
        public int MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        /// <summary>
        /// Gets and sets the maximum value that the integer can be assigned
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
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
            if (propValue is int)
            {
                int intPropRule = (int)propValue;
                if (intPropRule < _minValue)
                {
                    valueValid = false;
                    errorMessage += Environment.NewLine + "Please enter a value greater than " + _minValue + " for rule " + Name;
                }
                if (intPropRule > _maxValue)
                {
                    valueValid = false;
                    errorMessage += Environment.NewLine + "Please enter a value less than " + _maxValue + " for rule " + Name;
                }
            }
            return valueValid;
            
        }
    }
}