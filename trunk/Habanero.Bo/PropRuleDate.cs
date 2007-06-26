using System;
using System.Collections.Generic;
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
            if (parameters.ContainsKey("min")) _minValue = Convert.ToDateTime(parameters["min"]);
            if (parameters.ContainsKey("max")) _maxValue = Convert.ToDateTime(parameters["max"]);
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
                               " is not valid for " + _name +
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

    #region Tests

    [TestFixture]
    public class PropRuleDateTester
    {
        [Test]
        public void TestDateRule()
        {
            //	PropDef lPropDef = new PropDef("Surname", typeof(string),PropReadWriteRule.ReadManyWriteMany);
            PropRuleDate rule =
                new PropRuleDate("BirthDate", "Test", new DateTime(1900, 01, 01), new DateTime(2010, 12, 31));

            string errorMessage = "";

            //Test less than max length
            Assert.IsFalse(rule.isPropValueValid(new DateTime(1891, 01, 14), ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.isPropValueValid(new DateTime(1991, 01, 14), ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max length
            Assert.IsFalse(rule.isPropValueValid(new DateTime(2091, 01, 14), ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);


            rule = new PropRuleDate("BirthDate", "Test");
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