//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlPropertyRuleLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlPropertyRuleLoader
    {
        private XmlRuleLoader _loader;

        [SetUp]
        public void Initialise()
        {
            _loader = new XmlRuleLoader();
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestNoKeyAttribute()
        {
            _loader.LoadRule(typeof (int).Name,
                @"
                <rule name=""TestRule"" message=""Test Message"">
                    <add value=""1"" />
                </rule>");
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestNoValueAttribute()
        {
            _loader.LoadRule(typeof (int).Name,
                @"
                <rule name=""TestRule"" message=""Test Message"">
                    <add key=""max"" />
                </rule>");
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestNoAddElements()
        {
            _loader.LoadRule(typeof (int).Name,
                @"
                <rule name=""TestRule"" message=""Test Message"">
                </rule>");
        }

        [Test]
        public void TestRuleOfInteger()
        {
            PropRuleBase rule =
                _loader.LoadRule(typeof (int).Name,
                    @"<rule name=""Test Rule"" message=""Test Message""><add key=""min"" value=""2""/><add key=""max"" value=""10"" /></rule>");
            Assert.AreEqual("PropRuleInteger", rule.GetType().Name, "Incorrect rule type created.");
            Assert.AreEqual("Test Rule", rule.Name, "Name name is not being read from xml correctly.");
            Assert.AreEqual("Test Message", rule.Message, "Message is not being read from xml correctly.");
            //Assert.AreSame(typeof(int), rule.PropertyType,
            //                   "A propRuleInteger should have int as its property type.");
            Assert.AreEqual(2, ((PropRuleInteger) rule).MinValue);
            Assert.AreEqual(10, ((PropRuleInteger) rule).MaxValue);
        }

        [Test]
        public void TestPropRuleIntegerNoValues()
        {
            PropRuleBase rule =
                _loader.LoadRule(typeof (int).Name,
                    @"<rule name=""TestRule"" message=""Test Message""><add key=""max"" value=""1""/></rule>");
            Assert.AreEqual(int.MinValue, ((PropRuleInteger) rule).MinValue);
            rule =
                _loader.LoadRule(typeof (int).Name,
                    @"<rule name=""TestRule"" message=""Test Message""><add key=""min"" value=""1""/></rule>");
            Assert.AreEqual(int.MaxValue, ((PropRuleInteger) rule).MaxValue);
        }

        //[Test]
        //public void TestIsCompulsory()
        //{
        //    PropRuleBase propRule =
        //        _loader.LoadPropertyRule(
        //            @"<propertyRuleInteger name=""TestInt"" isCompulsory=""true""></propertyRuleInteger>");
        //    Assert.IsTrue(propRule.IsCompulsory, "Property should be compulsory as defined in the xml");
        //}

        [Test]
        public void TestPropRuleString()
        {
            PropRuleBase rule =
                _loader.LoadRule(typeof (string).Name,
                    @"<rule name=""TestString"" message=""String Test Message""><add key=""maxLength"" value=""100""/></rule>");

            Assert.AreEqual("PropRuleString", rule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual("TestString", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual("String Test Message", rule.Message, "Message is not being read from xml correctly");
            //Assert.AreSame(typeof(string), propRule.PropertyType,
            //               "A PropRuleString should have string as its property type.");
            Assert.AreEqual("", ((PropRuleString) rule).PatternMatch,
                "An empty string should be the default pattern match string according to the dtd.");
            Assert.AreEqual(0, ((PropRuleString) rule).MinLength,
                "0 should be the default minlength according to the dtd.");
            rule =
                _loader.LoadRule(typeof (string).Name,
                    @"<rule name=""TestString"" message=""String Test Message""><add key=""minLength"" value=""1""/></rule>");
            Assert.AreEqual(-1, ((PropRuleString) rule).MaxLength,
                "-1 should be the default maxlength according to the dtd.");
        }

        [Test]
        public void TestPropRuleStringAttributes()
        {
            PropRuleBase rule = _loader.LoadRule(typeof (string).Name,
                @"<rule name=""TestString"" message=""String Test Message"" >
                            <add key=""patternMatch"" value=""Test Pattern"" />
                            <add key=""minLength"" value=""5"" />          
                            <add key=""maxLength"" value=""10"" />
                        </rule>                          
");

            Assert.AreEqual("PropRuleString", rule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual("Test Pattern", ((PropRuleString) rule).PatternMatch);
            Assert.AreEqual(5, ((PropRuleString) rule).MinLength);
            Assert.AreEqual(10, ((PropRuleString) rule).MaxLength);
        }

        [Test]
        public void TestPropRuleDate()
        {
            PropRuleBase rule = _loader.LoadRule(typeof (DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""01 Feb 2004"" />
                            <add key=""max"" value=""09 Oct 2004"" />
                        </rule>                          
");
            Assert.AreEqual("PropRuleDate", rule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            //Assert.AreSame(typeof(DateTime), rule.PropertyType,
            //               "A PropRuleDate should have DateTime as its property type.");
            Assert.AreEqual(new DateTime(2004, 02, 01), ((PropRuleDate) rule).MinValue);
            Assert.AreEqual(new DateTime(2004, 10, 09), ((PropRuleDate) rule).MaxValue);
        }

        [Test]
        public void TestPropRuleDate_Today()
        {
            PropRuleBase rule = _loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""Today"" />
                            <add key=""max"" value=""Today"" />
                        </rule>                          
");
            Assert.AreEqual("PropRuleDate", rule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual(DateTime.Today, ((PropRuleDate)rule).MinValue.Date);
            Assert.AreEqual(DateTime.Today, ((PropRuleDate)rule).MaxValue.Date);
        }
        [Test]
        public void TestPropRuleDate_MinValue_Today()
        {
            PropRuleBase rule = _loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""Today"" />
                            <add key=""max"" value=""09 Oct 2004"" />
                        </rule>                          
");
            Assert.AreEqual("PropRuleDate", rule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual(DateTime.Today, ((PropRuleDate)rule).MinValue.Date);
            Assert.AreEqual(new DateTime(2004, 10, 09), ((PropRuleDate)rule).MaxValue);
        }
        [Test]
        public void TestPropRuleDate_MaxValue_Today()
        {
            PropRuleBase rule = _loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""01 Feb 2004"" />
                            <add key=""max"" value=""Today"" />
                        </rule>                          
");
            Assert.AreEqual("PropRuleDate", rule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual(new DateTime(2004, 02, 01), ((PropRuleDate)rule).MinValue);
            Assert.AreEqual(DateTime.Today, ((PropRuleDate)rule).MaxValue.Date);
        }
        [Test]
        public void TestPropRuleDate_Now()
        {
            PropRuleBase rule = _loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""Now"" />
                            <add key=""max"" value=""Now"" />
                        </rule>                          
");
            Assert.AreEqual("PropRuleDate", rule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual(DateTime.Today, ((PropRuleDate)rule).MinValue.Date);
            Assert.AreEqual(DateTime.Today, ((PropRuleDate)rule).MaxValue.Date);
        }

        [Test]
        public void TestPropRuleDecimal()
        {
            PropRuleBase rule = _loader.LoadRule(typeof (Decimal).Name,
                @"<rule name=""TestDec"" >
                            <add key=""min"" value=""1.5"" />
                            <add key=""max"" value=""8.2"" />
                        </rule>                          
");
            Assert.AreEqual("PropRuleDecimal", rule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual("TestDec", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual(1.5, ((PropRuleDecimal) rule).MinValue);
            Assert.AreEqual(8.2, ((PropRuleDecimal) rule).MaxValue);
        }

        [Test]
        public void TestPropRuleDecimalNoValues()
        {
            PropRuleBase rule =
                _loader.LoadRule(typeof (Decimal).Name, @"<rule name=""TestDec""><add key=""max"" value=""1""/></rule>");
            Assert.AreEqual(Decimal.MinValue, ((PropRuleDecimal) rule).MinValue);
            rule =
                _loader.LoadRule(typeof (Decimal).Name, @"<rule name=""TestDec""><add key=""min"" value=""1""/></rule>");
            Assert.AreEqual(Decimal.MaxValue, ((PropRuleDecimal) rule).MaxValue);
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestNoRuleForCustomType()
        {
            PropRuleBase rule = _loader.LoadRule(typeof (TimeSpan).Name,
                @"<rule name=""TestCustom"">
                    <add key=""bob"" value=""billy"" />
                </rule>");
        }

        [Test]
        public void TestCustomRuleClass()
        {
            PropRuleBase rule = _loader.LoadRule("CustomProperty",
                @"<rule name=""TestCustom"" class=""Habanero.Test.BO.Loaders.MyRule"" assembly=""Habanero.Test.BO"">
                            <add key=""bob"" value=""billy"" />
                        </rule>");
            Assert.AreEqual("MyRule", rule.GetType().Name, "Incorrect property rule type created.");
            Assert.AreEqual("billy", ((MyRule) rule).Bob);
        }

        [Test, ExpectedException(typeof (TypeLoadException))]
        public void TestCustomRuleMustInheritFromPropRuleBase()
        {
            PropRuleBase rule = _loader.LoadRule(typeof (TimeSpan).Name,
                @"<rule name=""TestCustom"" class=""Habanero.Test.MyBO"" assembly=""Habanero.Test"">
                    <add key=""bob"" value=""billy"" />
                </rule>");
        }
    }

    public class MyRule : PropRuleBase
    {
        private string _bob;

        public MyRule(string name, string message, Dictionary<string, object> parameters)
            : base(name, message)
        {
        }

        protected internal override void SetupParameters()
        {
            object value = Parameters["bob"];
            if (value != null)
            {
                _bob = (string) value;
            }
        }

        public string Bob
        {
            get { return _bob; }
        }

        protected internal override List<string> AvailableParameters()
        {
            List<string> parameters = new List<string>();
            parameters.Add("bob");
            return parameters;
        }
    }
}