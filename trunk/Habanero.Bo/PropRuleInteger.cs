using System;

namespace Habanero.Bo
{
    /// <summary>
    /// Checks integer values against property rules that test for validity
    /// </summary>
    public class PropRuleInteger : PropRuleBase
    {
        private int _minValue;
        private int _maxValue;

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="isCompulsory">Whether a value is compulsory and
        /// null values are invalid</param>
        /// <param name="minValue">The minimum value allowed for the integer</param>
        /// <param name="maxValue">The maximum value allowed for the integer</param>
        public PropRuleInteger(string ruleName, bool isCompulsory, int minValue, int maxValue) : base(ruleName, isCompulsory, typeof (int))
        {
            _minValue = minValue;
            _maxValue = maxValue;
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
                    errorMessage += Environment.NewLine + "Please enter a value greater than " + _minValue + " for rule " + RuleName;
                }
                if (intPropRule > _maxValue)
                {
                    valueValid = false;
                    errorMessage += Environment.NewLine + "Please enter a value less than " + _maxValue + " for rule " + RuleName;
                }
            }
            return valueValid;
            
        }
    }
}