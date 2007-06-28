using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Bo;
using NUnit.Framework;

namespace Habanero.Test.Bo
{
    [TestFixture]
    public class TestPropRuleString
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
}
