using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestPropRuleLong
    {
        [Test]
        public void TestLongRule()
        {
            PropRuleLong rule = new PropRuleLong("num", "TestMessage", 5, 10);
            
            string errorMessage = "";

            //Test less than min
            Assert.IsFalse(rule.IsPropValueValid("Propname", 1, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.IsPropValueValid("Propname", 6, ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max
            Assert.IsFalse(rule.IsPropValueValid("Propname", 12, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);

            rule = new PropRuleLong("num", "TestMessage", 5, 10);
            errorMessage = "";

            Assert.IsTrue(rule.IsPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";
            Assert.IsFalse(rule.IsPropValueValid("Propname", -5, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }

        [Test]
        public void Test_MaxMinUsingInterface()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPropRuleComparable<long> rule = new PropRuleLong("num", "TestMessage", 5, 10);
            //---------------Test Result -----------------------
            Assert.AreEqual(5, rule.MinValue);
            Assert.AreEqual(10, rule.MaxValue);
        }
    }
}