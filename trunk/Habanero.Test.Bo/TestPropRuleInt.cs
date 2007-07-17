using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Bo;
using NUnit.Framework;

namespace Habanero.Test.Bo
{
    [TestFixture]
    public class TestPropRuleInt
    {
        [Test]
        public void TestIntRule()
        {
            PropRuleInteger rule = new PropRuleInteger("num", "TestMessage", 5, 10);
            
            string errorMessage = "";

            //Test less than min
            Assert.IsFalse(rule.isPropValueValid("Propname", 1, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.isPropValueValid("Propname", 6, ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max
            Assert.IsFalse(rule.isPropValueValid("Propname", 12, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);

            rule = new PropRuleInteger("num", "TestMessage", 5, 10);
            errorMessage = "";

            Assert.IsTrue(rule.isPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";
            Assert.IsFalse(rule.isPropValueValid("Propname", -5, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }
    }
}
