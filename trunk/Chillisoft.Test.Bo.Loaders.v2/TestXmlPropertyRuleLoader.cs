using System;
using Chillisoft.Bo.Loaders.v2;
using Chillisoft.Bo.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.Loaders.v2
{
    /// <summary>
    /// Summary description for TestXmlPropertyRuleLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlPropertyRuleLoader
    {
        [Test]
        public void TestPropRuleInteger()
        {
            XmlPropertyRuleLoader loader = new XmlPropertyRuleIntegerLoader();
            PropRuleBase propRule =
                loader.LoadPropertyRule(@"<propertyRuleInteger name=""TestInt"" minValue=""1"" maxValue=""5""></propertyRuleInteger>");
            Assert.AreEqual("PropRuleInteger", propRule.GetType().Name, "Incorrect property rule type created.");
            Assert.IsFalse(propRule.IsCompulsory, "Not compulsory should be the default behaviour");
            Assert.AreEqual("TestInt", propRule.RuleName, "Rule name is not being read from xml correctly.");
            Assert.AreSame(typeof (int), propRule.PropertyType,
                           "A propRuleInteger should have int as its property type.");
            Assert.AreEqual(1, ((PropRuleInteger) propRule).MinValue);
            Assert.AreEqual(5, ((PropRuleInteger) propRule).MaxValue);
        }

        [Test]
        public void TestPropRuleIntegerNoValues()
        {
            XmlPropertyRuleLoader loader = new XmlPropertyRuleIntegerLoader();
            PropRuleBase propRule =
                loader.LoadPropertyRule(@"<propertyRuleInteger name=""TestInt""></propertyRuleInteger>");
            Assert.AreEqual(int.MinValue , ((PropRuleInteger)propRule).MinValue);
            Assert.AreEqual(int.MaxValue , ((PropRuleInteger)propRule).MaxValue);
        }

        [Test]
        public void TestIsCompulsory()
        {
            XmlPropertyRuleLoader loader = new XmlPropertyRuleIntegerLoader();
            PropRuleBase propRule =
                loader.LoadPropertyRule(
                    @"<propertyRuleInteger name=""TestInt"" isCompulsory=""true""></propertyRuleInteger>");
            Assert.IsTrue(propRule.IsCompulsory, "Property should be compulsory as defined in the xml");
        }

        [Test]
        public void TestPropRuleString()
        {
            XmlPropertyRuleLoader loader = new XmlPropertyRuleStringLoader();
            PropRuleBase propRule =
                loader.LoadPropertyRule(@"<propertyRuleString name=""TestString""></propertyRuleString>");
            Assert.AreEqual("PropRuleString", propRule.GetType().Name, "Incorrect property rule type created.");
            Assert.IsFalse(propRule.IsCompulsory, "Not compulsory should be the default behaviour");
            Assert.AreEqual("TestString", propRule.RuleName, "Rule name is not being read from xml correctly.");
            Assert.AreSame(typeof (string), propRule.PropertyType,
                           "A PropRuleString should have string as its property type.");
            Assert.AreEqual(0, ((PropRuleString) propRule).MinLength,
                            "0 should be the default minlength according to the dtd.");
            Assert.AreEqual(-1, ((PropRuleString) propRule).MaxLength,
                            "-1 should be the default maxlength according to the dtd.");
        }

        [Test]
        public void TestPropRuleStringAttributes()
        {
            XmlPropertyRuleLoader loader = new XmlPropertyRuleStringLoader();
            PropRuleBase propRule =
                loader.LoadPropertyRule(
                    @"<propertyRuleString name=""TestString"" minLength=""5"" maxLength=""10""></propertyRuleString>");
            Assert.AreEqual("PropRuleString", propRule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual(5, ((PropRuleString) propRule).MinLength);
            Assert.AreEqual(10, ((PropRuleString) propRule).MaxLength);
        }

        [Test]
        public void TestPropRuleDate()
        {
            XmlPropertyRuleLoader loader = new XmlPropertyRuleDateLoader();
            PropRuleBase propRule =
                loader.LoadPropertyRule(
                    @"<propertyRuleDate name=""TestDate"" minValue=""01 Feb 2004"" maxValue=""09 Oct 2004""></propertyRuleDate>");
            Assert.AreEqual("PropRuleDate", propRule.GetType().Name, "Incorrect property rule type created.");
            Assert.IsFalse(propRule.IsCompulsory, "Not compulsory should be the default behaviour");
            Assert.AreEqual("TestDate", propRule.RuleName, "Rule name is not being read from xml correctly.");
            Assert.AreSame(typeof (DateTime), propRule.PropertyType,
                           "A PropRuleDate should have DateTime as its property type.");
            Assert.AreEqual(new DateTime(2004, 02, 01), ((PropRuleDate) propRule).MinValue);
            Assert.AreEqual(new DateTime(2004, 10, 09), ((PropRuleDate) propRule).MaxValue);
        }

        [Test]
        public void TestPropRuleDecimal()
        {
            XmlPropertyRuleLoader loader = new XmlPropertyRuleDecimalLoader();
            PropRuleBase propRule =
                loader.LoadPropertyRule(@"<propertyRuleDecimal name=""TestDec"" minValue=""1.5"" maxValue=""8.2""></propertyRuleDecimal>");
            Assert.AreEqual("PropRuleDecimal", propRule.GetType().Name, "Incorrect property rule type created.");
            Assert.IsFalse(propRule.IsCompulsory, "Not compulsory should be the default behaviour");
            Assert.AreEqual("TestDec", propRule.RuleName, "Rule name is not being read from xml correctly.");
            Assert.AreSame(typeof(decimal), propRule.PropertyType,
                           "A propRuleDecimal should have int as its property type.");
            Assert.AreEqual(1.5, ((PropRuleDecimal)propRule).MinValue);
            Assert.AreEqual(8.2, ((PropRuleDecimal)propRule).MaxValue);
        }

        [Test]
        public void TestPropRuleDecimalNoValues()
        {
            XmlPropertyRuleLoader loader = new XmlPropertyRuleIntegerLoader();
            PropRuleBase propRule =
                loader.LoadPropertyRule(@"<propertyRuleInteger name=""TestInt""></propertyRuleInteger>");
            Assert.AreEqual(int.MinValue, ((PropRuleInteger)propRule).MinValue);
            Assert.AreEqual(int.MaxValue, ((PropRuleInteger)propRule).MaxValue);
        }        
    }
}