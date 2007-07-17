using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Bo;
using NUnit.Framework;

namespace Habanero.Test.Bo
{
    [TestFixture]
    public class TestPropRuleDecimal
    {
        [Test]
        public void TestDecimalRule()
        {
            PropRuleDecimal rule = new PropRuleDecimal("num", "TestMessage", 5.32m, 10.11111m);

            string errorMessage = "";

            //Test less than min
            Assert.IsFalse(rule.isPropValueValid("Propname", 5.31116m, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.isPropValueValid("Propname", 6, ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max
            Assert.IsFalse(rule.isPropValueValid("Propname", 10.1111112m, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);

            rule = new PropRuleDecimal("num", "TestMessage", 5.32m, 10.11111m);
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";
            Assert.IsFalse(rule.isPropValueValid("Propname", -53444.33222m, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }
    }
}
