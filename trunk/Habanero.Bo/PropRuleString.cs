using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Bo
{
    /// <summary>
    /// Checks string values against property rules that test for validity.
    /// See System.Text.RegularExpressions for more information on regular
    /// expression formatting.
    /// </summary>
    /// TODO ERIC - include property for pattern
    /// - include sets for max/min values
    public class PropRuleString : PropRuleBase
    {
        private int _maxLength = -1;
        private int _minLength = 0;
    	private string _patternMatch = ""; //regex pattern match
    	private string _patternMatchErrorMessage = "";

        ///// <summary>
        ///// Constructor to initialise a new rule
        ///// </summary>
        ///// <param name="ruleName">The rule name</param>
        ///// <param name="isCompulsory">Whether a value is compulsory and
        ///// null values are invalid</param>
        ///// <param name="minLength">The minimum length required for the string</param>
        ///// <param name="maxLength">The maximum length allowed for the string</param>
        //public PropRuleString(string ruleName,
        //                      int minLength,
        //                      int maxLength) : base(ruleName, typeof (string))
        //{
        //    _maxLength = maxLength;
        //    _minLength = minLength;
        //}

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="name">The rule name</param>
        /// <param name="message">This rule's failure message</param>
        /// <param name="parameters">The parameters for this rule.</param>
        public PropRuleString(string name, string message, Dictionary<string, object> parameters)
			: base(name, message, parameters)
        {
            try
            {
                foreach (string key in parameters.Keys)
                {
                    switch (key)
                    {
                        case "patternMatch":
                            _patternMatch = Convert.ToString(parameters["patternMatch"]);
                            break;
                        case "patternMatchErrorMessage":
                            _patternMatchErrorMessage = Convert.ToString(parameters["patternMatchErrorMessage"]);
                            break;
                        case "minLength":
                            _minLength = Convert.ToInt32(parameters["minLength"]);
                            break;
                        case "maxLength":
                            _maxLength = Convert.ToInt32(parameters["maxLength"]);
                            break;
                        default:
                            throw new InvalidXmlDefinitionException(String.Format(
                                "The rule type '{0}' for strings does not exist. " +
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
                    "while processing the property rules for a string.  The " +
                    "likely cause is that one of the attributes in the 'add' " +
                    "element has an invalid value.", ex);
            }
        }


        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="message">The rule failure message</param>
        /// <param name="minLength">The minimum length required for the string</param>
        /// <param name="maxLength">The maximum length allowed for the string</param>
        /// <param name="patternMatch">The pattern match as a regular
        /// expression that the string must conform to.  See 
        /// System.Text.RegularExpressions for more information on regular
        /// expression formatting.</param>
        /// <param name="patternMatchErrorMessage">The error message that must 
        /// be displayed if the pattern does not match</param>
        public PropRuleString(string ruleName,
                               string message,
                                int minLength,
                                int maxLength,
                                string patternMatch,
                                string patternMatchErrorMessage) : base(ruleName, message, null)
        {
            //TODO_Err: how to test for a valid regexpression?
            _minLength = minLength;
            _maxLength = maxLength;
            _patternMatch = patternMatch ?? "";
            _patternMatchErrorMessage = patternMatchErrorMessage ?? "";
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
            //Check if propertyValue is of the correct type.
            if (propValue == null)
            {
                return true;
            }
            if (!(propValue is string))
            {
                errorMessage = propValue +
                               " is not valid for " + Name +
                               " since it is not of type String";
                return false;
            }
            if (!base.isPropValueValid(propValue, ref errorMessage))
            {
                return false;
            }
 
            if (!CheckLengthRule(propValue, ref errorMessage))
            {
                return false;
            }

            if (!CheckPatternMatchRule(propValue, ref errorMessage))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if the value matches the pattern set required. If no
        /// expression was specified, then the method will return true
        /// </summary>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        protected bool CheckPatternMatchRule(Object propValue,
                                             ref string errorMessage)
        {
            if (this._patternMatch.Length == 0)
            {
                return true;
            }
            if (!Regex.IsMatch((string) propValue, this._patternMatch))
            {
                if (_patternMatchErrorMessage.Length <= 0)
                {
                    errorMessage = propValue.ToString() +
								   " is not valid for " + Name +
                                   " it must match the pattern " +
                                   _patternMatch;
                }
                else
                {
                    errorMessage = propValue.ToString() +
								   " is not valid for " + Name +
                                   "\n" + _patternMatchErrorMessage;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks that the string length falls within the length range
        /// specified
        /// </summary>
        /// <param name="propValue">The value to check</param>
        /// <param name="errorMessage">A string to amend with an error
        /// message indicating why the value might have been invalid</param>
        /// <returns>Returns true if valid</returns>
        protected bool CheckLengthRule(Object propValue,
                                       ref string errorMessage)
        {
            //Check the appropriate length rules
            if (_minLength > 0 && ((string) propValue).Length < _minLength)
            {
                errorMessage = propValue.ToString() +
							   " is not valid for " + Name +
                               " it must be greater than or equal to " +
                               _minLength;
                return false;
            }
            if (_maxLength > 0 && ((string) propValue).Length > _maxLength)
            {
                errorMessage = propValue.ToString() +
							   " is not valid for " + Name +
                               " it must be less than or equal to " +
                               _maxLength;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the maximum length the string can be
        /// </summary>
        public int MaxLength
        {
			get { return _maxLength; }
			protected set { _maxLength = value; }
        }

        /// <summary>
        /// Returns the minimum length the string can be
        /// </summary>
        public int MinLength
        {
			get { return _minLength; }
			protected set { _minLength = value; }
        }

		/// <summary>
		/// Returns the pattern match regular expression for the string
		/// </summary>
		public string PatternMatch
		{
			get { return _patternMatch; }
			protected set { _patternMatch = value; }
		}

		/// <summary>
		/// Returns the error message raised when the pattern is not matched
		/// </summary>
		public string PatternMatchErrorMessage
		{
			get { return _patternMatchErrorMessage; }
			protected set { _patternMatchErrorMessage = value; }
		}
		
    }

}