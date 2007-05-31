using System;
using NUnit.Framework;

namespace Chillisoft.Bo.v2
{
    /// <summary>
    /// Checks date values against property rules that test for validity
    /// </summary>
    /// TODO ERIC - includes sets for min/max
    public class PropRuleDate : PropRuleBase
    {
        protected readonly DateTime mMaxValue = DateTime.MaxValue;
        protected readonly DateTime mMinValue = DateTime.MinValue;

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="isCompulsory">Whether a value is compulsory and
        /// null values are invalid</param>
        public PropRuleDate(string ruleName,
                            bool isCompulsory) : base(ruleName, isCompulsory, typeof (DateTime))
        {
        }

        /// <summary>
        /// Constructor to initialise a new rule
        /// </summary>
        /// <param name="ruleName">The rule name</param>
        /// <param name="isCompulsory">Whether a value is compulsory and
        /// null values are invalid</param>
        /// <param name="minValue">The minimum date that can be set</param>
        /// <param name="maxValue">The maximum date that can be set</param>
        public PropRuleDate(string ruleName,
                            bool isCompulsory,
                            DateTime minValue,
                            DateTime maxValue) : this(ruleName, isCompulsory)
        {
            mMaxValue = maxValue;
            mMinValue = minValue;
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
            if ((DateTime) propValue < mMinValue || (DateTime) propValue > mMaxValue)
            {
                errorMessage = propValue.ToString() +
                               " is not valid for " + mRuleName +
                               " since it is not of type DateTime";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the maximum value the date can be
        /// </summary>
        public DateTime MaxValue
        {
            get { return mMaxValue; }
        }

        /// <summary>
        /// Returns the minimum value the date can be
        /// </summary>
        public DateTime MinValue
        {
            get { return mMinValue; }
        }
    }

    #region Tests

    [TestFixture]
    public class PropRuleDateTester
    {
        [Test]
        public void TestDateRule()
        {
            //	PropDef lPropDef = new PropDef("Surname", typeof(string),cbsPropReadWriteRule.ReadManyWriteMany);
            PropRuleDate rule =
                new PropRuleDate("BirthDate", true, new DateTime(1900, 01, 01), new DateTime(2010, 12, 31));

            string errorMessage = "";

            //try set the property to null.
            Assert.IsFalse(rule.isPropValueValid(null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test less than max length
            Assert.IsFalse(rule.isPropValueValid(new DateTime(1891, 01, 14), ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.isPropValueValid(new DateTime(1991, 01, 14), ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max length
            Assert.IsFalse(rule.isPropValueValid(new DateTime(2091, 01, 14), ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);


            rule = new PropRuleDate("BirthDate", false);
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid(null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";

            //Test valid data
            Assert.IsTrue(rule.isPropValueValid(new DateTime(1991, 01, 14), ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
        }
    }

    #endregion //Tests
}