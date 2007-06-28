using System;
using System.Collections.Generic;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Bo
{
    /// <summary>
    /// Checks date values against property rules that test for validity
    /// </summary>
    /// TODO ERIC - includes sets for min/max
    public class PropRuleDate : PropRuleBase
    {
        private DateTime _minValue = DateTime.MinValue;
		private DateTime _maxValue = DateTime.MaxValue;

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        public PropRuleDate(string ruleName,
                            string message) 
			: this(ruleName, message, null, null)
        {
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        /// <param name="minValue">The minimum date that can be set</param>
        /// <param name="maxValue">The maximum date that can be set</param>
        public PropRuleDate(string ruleName,
                            string message,
                            DateTime? minValue,
							DateTime? maxValue)
			: base(ruleName, message, null)
        {
			// if the nullable minvalue is null, then set it to DateTime.MinValue.
			_minValue = minValue ?? DateTime.MinValue;
			// if the nullable maxValue is null, then set it to DateTime.MaxValue.
			_maxValue = maxValue ?? DateTime.MaxValue;
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        /// <param name="parameters">The parameters for this rule.</param>
        public PropRuleDate(string name, string message, Dictionary<string, object> parameters)
			: base(name, message, parameters)
        {
            try
            {
                if (parameters.ContainsKey("min")) _minValue = Convert.ToDateTime(parameters["min"]);
                if (parameters.ContainsKey("max")) _maxValue = Convert.ToDateTime(parameters["max"]);
            }
            catch (Exception ex)
            {
                throw new InvalidXmlDefinitionException("An error occurred " +
                    "while processing the property rules for a date.  The " +
                    "likely cause is that one of the attributes in the 'add' " +
                    "element of the class definitiosn has an invalid value.", ex);
            }
        }

        /// <summary>
        /// Indicates whether the property value is valid against the rules
        /// </summary>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        protected internal override bool isPropValueValid(Object propValue,
                                                          ref string errorMessage)
        {
            errorMessage = "";
            if (!base.isPropValueValid(propValue, ref errorMessage))
            {
                return false;
            }
            if (propValue == null)
            {
                return true;
            }
            if ((DateTime) propValue < _minValue || (DateTime) propValue > _maxValue)
            {
                errorMessage = propValue.ToString() +
							   " is not valid for " + Name +
                               " since it is not of type DateTime";
                return false;
            }
            return true;
        }

         /// <summary>
        /// Returns the minimum value the date can be
        /// </summary>
        public DateTime MinValue
        {
            get { return _minValue; }
			protected set { _minValue = value; }
		}

       /// <summary>
        /// Returns the maximum value the date can be
        /// </summary>
        public DateTime MaxValue
        {
            get { return _maxValue; }
			protected set { _maxValue = value; }
        }
    }

}