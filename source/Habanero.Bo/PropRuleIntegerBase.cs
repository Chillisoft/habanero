using System;
using System.Collections.Generic;

namespace Habanero.BO
{
    /// <summary>
    /// Base class for all integer rules (such as PropRuleInt, PropRuleLong and PropRuleShort)
    /// </summary>
    /// <typeparam name="T">The type of integer type this rule checks</typeparam>
    public abstract class PropRuleIntegerBase<T>  : PropRuleBase
    {
        /// <summary>
        /// Standard constructor
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">The custom message</param>
        protected PropRuleIntegerBase(string name, string message) : base(name, message)
        {
        }

        /// <summary>
        /// Gets and sets the maximum value that the integer can be assigned
        /// </summary>
        public abstract T MaxValue { get; set; }

        /// <summary>
        /// Gets and sets the minimum value that the integer can be assigned
        /// </summary>
        public abstract T MinValue { get; set; }

        /// <summary>
        /// Returns whether the supplied value is less than the MinValue
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        protected abstract bool IsLessThanMinValue(T value);

        /// <summary>
        /// Returns whether the supplied value is greater than the MaxValue
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <returns></returns>
        protected abstract bool IsGreaterThanMaxValue(T value);

        /// <summary>
        /// Compares the value against the min and max values. If it falls outside the accepted range
        /// it returns false and updates the error message.
        /// </summary>
        /// <param name="displayName">The name of the property (for use in the error message)</param>
        /// <param name="value">The value to check</param>
        /// <param name="errorMessage">The error message - this is updated with any further messages based on this check</param>
        /// <returns>Whether the check passed or failed (ie true if the value falls within the min/max range</returns>
        protected bool CheckValueAgainstAcceptedRange(string displayName, T value, ref string errorMessage)
        {
            if (IsLessThanMinValue(value))
            {
                errorMessage = GetBaseErrorMessage(value, displayName);
                if (!String.IsNullOrEmpty(Message))
                {
                    errorMessage += Message;
                }
                else
                {
                    errorMessage += "The value cannot be less than " + MinValue + ".";
                }
                return false;
            }
            if (IsGreaterThanMaxValue(value))
            {
                errorMessage = GetBaseErrorMessage(value, displayName);
                if (!String.IsNullOrEmpty(Message))
                {
                    errorMessage += Message;
                }
                else
                {
                    errorMessage += "The value cannot be more than " + MaxValue + ".";
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the list of available parameter names for the rule.
        /// </summary>
        /// <returns>A list of the parameters that this rule uses</returns>
        public override List<string> AvailableParameters
        {
            get
            {
                return new List<string> {"min", "max"};
            }
        }
    }
}