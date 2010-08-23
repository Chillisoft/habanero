using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestPropRuleLong
    {
        [Test]
        public void Test_IsPropValueValid_ShouldBeFalse_WhenValueIsLessThanMin()
        {
            //---------------Set up test pack-------------------
            PropRuleLong rule = new PropRuleLong("num", "TestMessage", 10, 200);
            string errorMessage = "";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool valid = rule.IsPropValueValid("Propname", 9, ref errorMessage);
            //---------------Test Result -----------------------
            Assert.IsFalse(valid);
            Assert.IsTrue(errorMessage.Length > 0);
        }

        [Test]
        public void Test_IsPropValueValid_ShouldBeTrue_WhenValueIsWithinRange()
        {
            //---------------Set up test pack-------------------
            PropRuleLong rule = new PropRuleLong("num", "TestMessage", 10, 200);
            string errorMessage = "";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool valid = rule.IsPropValueValid("Propname", 11, ref errorMessage);
            //---------------Test Result -----------------------
            Assert.IsTrue(valid);
            Assert.IsFalse(errorMessage.Length > 0);
        }

        [Test]
        public void Test_IsPropValueValid_ShouldBeTrue_WhenValueIsEqualToMin()
        {
            //---------------Set up test pack-------------------
            PropRuleLong rule = new PropRuleLong("num", "TestMessage", 10, 200);
            string errorMessage = "";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool valid = rule.IsPropValueValid("Propname", 10, ref errorMessage);
            //---------------Test Result -----------------------
            Assert.IsTrue(valid);
            Assert.IsFalse(errorMessage.Length > 0);
        }

        [Test]
        public void Test_IsPropValueValid_ShouldBeTrue_WhenValueIsEqualToMax()
        {
            //---------------Set up test pack-------------------
            PropRuleLong rule = new PropRuleLong("num", "TestMessage", 10, 200);
            string errorMessage = "";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool valid = rule.IsPropValueValid("Propname", 200, ref errorMessage);
            //---------------Test Result -----------------------
            Assert.IsTrue(valid);
            Assert.IsFalse(errorMessage.Length > 0);
        }

        [Test]
        public void Test_IsPropValueValid_ShouldBeFalse_WhenValueIsGreaterThanMax()
        {
            //---------------Set up test pack-------------------
            PropRuleLong rule = new PropRuleLong("num", "TestMessage", 10, 200);
            string errorMessage = "";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool valid = rule.IsPropValueValid("Propname", 201, ref errorMessage);
            //---------------Test Result -----------------------
            Assert.IsFalse(valid);
            Assert.IsTrue(errorMessage.Length > 0);
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }


        [Test]
        public void Test_PropRuledateMax_ViaInterface()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const Int64 minValue = Int64.MinValue;
            const Int64 maxValue = Int64.MaxValue;
            IPropRuleComparable<System.Int64> rule =
                new PropRuleLong("fdsafasd", "Test", minValue, maxValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(minValue, rule.MinValue);
            Assert.AreEqual(maxValue, rule.MaxValue);
        }
    }
}