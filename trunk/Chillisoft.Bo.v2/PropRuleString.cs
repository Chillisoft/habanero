using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Chillisoft.Bo.v2
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
        protected readonly int mMaxLength = -1;
        protected readonly int mMinLength = -1;
        protected readonly string mPatternMatch = ""; //regex pattern match
        protected readonly string mPatternMatchErrMsg = "";

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="isCompulsory">Whether a value is compulsory and
        /// null values are invalid</param>
        /// <param name="minValue">The minimum length required for the string</param>
        /// <param name="maxValue">The maximum length allowed for the string</param>
        public PropRuleString(string ruleName,
                              bool isCompulsory,
                              int minLength,
                              int maxLength) : base(ruleName, isCompulsory, typeof (string))
        {
            mMaxLength = maxLength;
            mMinLength = minLength;
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="isCompulsory">Whether a value is compulsory and
        /// null values are invalid</param>
        /// <param name="minValue">The minimum length required for the string</param>
        /// <param name="maxValue">The maximum length allowed for the string</param>
        /// <param name="patternMatch">The pattern match as a regular
        /// expression that the string must conform to.  See 
        /// System.Text.RegularExpressions for more information on regular
        /// expression formatting.</param>
        /// <param name="patternMatchErrorMessage">The error message that must 
        /// be displayed if the pattern does not match</param>
        internal PropRuleString(string ruleName,
                                bool isCompulsory,
                                int minLength,
                                int maxLength,
                                string patternMatch,
                                string patternMatchErrorMessage) : this(ruleName, isCompulsory, minLength, maxLength)
        {
            //TODO_Err: how to test for a valid regexpression?
            mPatternMatch = patternMatch;
            mPatternMatchErrMsg = patternMatchErrorMessage;
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
            if (this.mPatternMatch.Length == 0)
            {
                return true;
            }
            if (!Regex.IsMatch((string) propValue, this.mPatternMatch))
            {
                if (mPatternMatchErrMsg.Length <= 0)
                {
                    errorMessage = propValue.ToString() +
                                   " is not valid for " + mRuleName +
                                   " it must match the pattern " +
                                   mPatternMatch;
                }
                else
                {
                    errorMessage = propValue.ToString() +
                                   " is not valid for " + mRuleName +
                                   "\n" + mPatternMatchErrMsg;
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
            if (mMinLength > 0 && ((string) propValue).Length < mMinLength)
            {
                errorMessage = propValue.ToString() +
                               " is not valid for " + mRuleName +
                               " it must be greater than or equal to " +
                               mMinLength;
                return false;
            }
            if (mMaxLength > 0 && ((string) propValue).Length > mMaxLength)
            {
                errorMessage = propValue.ToString() +
                               " is not valid for " + mRuleName +
                               " it must be less than or equal to " +
                               mMaxLength;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the maximum length the string can be
        /// </summary>
        public int MaxLength
        {
            get { return mMaxLength; }
        }

        /// <summary>
        /// Returns the minimum length the string can be
        /// </summary>
        public int MinLength
        {
            get { return mMinLength; }
        }

    }

    #region Tests

    [TestFixture]
    public class PropRuleStringTester
    {
        [Test]
        public void TestStringRule()
        {
            PropRuleString rule = new PropRuleString("Surname", true, 2, 50);

            string errorMessage = "";

            //try set the property to null.
            Assert.IsFalse(rule.isPropValueValid(null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
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

            rule = new PropRuleString("Surname", false, 10, 20);
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid(null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";
            Assert.IsFalse(rule.isPropValueValid("", ref errorMessage)); //test zero length strings
            Assert.IsTrue(errorMessage.Length > 0);
            errorMessage = "";

            //Test that it ignores negative max length
            rule = new PropRuleString("Surname", false, -10, -1);
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
            PropRuleString rule = new PropRuleString("Surname", false, 10, 20, @"^[a-zA-Z\- ]*$", "");
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