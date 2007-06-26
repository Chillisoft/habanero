using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        protected readonly int _maxLength = -1;
        protected readonly int _minLength = 0;
    	protected readonly string _patternMatch = ""; //regex pattern match
    	protected readonly string _patternMatchErrorMessage = "";

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
            if (parameters.ContainsKey("patternMatch")) _patternMatch = Convert.ToString(parameters["patternMatch"]);
            if (parameters.ContainsKey("patternMatchErrorMessage"))
                _patternMatchErrorMessage = Convert.ToString(parameters["patternMatchErrorMessage"]);
            if (parameters.ContainsKey("minLength")) _minLength = Convert.ToInt32(parameters["minLength"]);
            if (parameters.ContainsKey("maxLength")) _maxLength = Convert.ToInt32(parameters["maxLength"]);
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
                               " is not valid for " + _name +
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
                                   " is not valid for " + _name +
                                   " it must match the pattern " +
                                   _patternMatch;
                }
                else
                {
                    errorMessage = propValue.ToString() +
                                   " is not valid for " + _name +
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
                               " is not valid for " + _name +
                               " it must be greater than or equal to " +
                               _minLength;
                return false;
            }
            if (_maxLength > 0 && ((string) propValue).Length > _maxLength)
            {
                errorMessage = propValue.ToString() +
                               " is not valid for " + _name +
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
        }

        /// <summary>
        /// Returns the minimum length the string can be
        /// </summary>
        public int MinLength
        {
            get { return _minLength; }
        }

		/// <summary>
		/// Returns the pattern match regular expression for the string
		/// </summary>
		public string PatternMatch
		{
			get { return _patternMatch; }
		}

		/// <summary>
		/// Returns the error message raised when the pattern is not matched
		/// </summary>
		public string PatternMatchErrorMessage
		{
			get { return _patternMatchErrorMessage; }
		}
		
    }

    #region Tests

    [TestFixture]
    public class PropRuleStringTester
    {
        [Test]
        public void TestStringRule()
        {
            PropRuleString rule = new PropRuleString("Surname", "Test", 2, 50, null, null);

            string errorMessage = "";

            //Test less than max length
            Assert.IsFalse(rule.isPropValueValid("", ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.isPropValueValid("fdfsdafasdfsdf", ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max length
            Assert.IsFalse(
                rule.isPropValueValid("MySurnameIsTooLongByFarThisWill Cause and Error in Bus object", ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test lengths and not compulsory

            rule = new PropRuleString("Surname", "Test", 10, 20, null, null);
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid(null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";
            Assert.IsFalse(rule.isPropValueValid("", ref errorMessage)); //test zero length strings
            Assert.IsTrue(errorMessage.Length > 0);
            errorMessage = "";

            //Test that it ignores negative max length
            rule = new PropRuleString("Surname", "Test", -10, -1, null, null);
            Assert.IsTrue(rule.isPropValueValid("", ref errorMessage)); //test zero length strings
            Assert.IsFalse(errorMessage.Length > 0);
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid("ffff", ref errorMessage)); //test zero length strings
            Assert.IsFalse(errorMessage.Length > 0);
            errorMessage = "";

            Assert.IsFalse(rule.isPropValueValid(11, ref errorMessage)); //test zero length strings
            Assert.IsTrue(errorMessage.Length > 0);
            errorMessage = "";
        }

        [Test]
        public void TestStringRulePatternMatch()
        {
            //Pattern match no numeric characters allowed
            string errorMessage = "";
            PropRuleString rule = new PropRuleString("Surname", "Test", 10, 20, @"^[a-zA-Z\- ]*$", "");
            Assert.IsFalse(rule.isPropValueValid("fdfasd 3dfasdf", ref errorMessage), "fdfasd 3dfasdf");
            Assert.IsTrue(errorMessage.Length > 0);
            errorMessage = "";
            Assert.IsTrue(rule.isPropValueValid("fdfasd-fdf asdf", ref errorMessage), "fdfasd fdfasdf");
            Assert.IsFalse(errorMessage.Length > 0);

            Assert.IsFalse(rule.isPropValueValid("fdfasd", ref errorMessage), "fdfasd");
            Assert.IsTrue(errorMessage.Length > 0);
        }
    }

    #endregion //Tests
}