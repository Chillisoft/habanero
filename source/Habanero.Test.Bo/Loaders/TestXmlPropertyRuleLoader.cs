//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
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
        public virtual void SetupTest()
        {
            Initialise();
        }

        protected void Initialise()
        {
            _loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
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
            IPropRule rule =
                _loader.LoadRule(typeof (int).Name,
                    @"<rule name=""Test Rule"" message=""Test Message""><add key=""min"" value=""2""/><add key=""max"" value=""10"" /></rule>");
            Assert.IsInstanceOfType(typeof(IPropRule), rule);
            Assert.AreEqual("Test Rule", rule.Name, "Name name is not being read from xml correctly.");
            Assert.AreEqual("Test Message", rule.Message, "Message is not being read from xml correctly.");
            //Assert.AreSame(typeof(int), rule.PropertyType,
            //                   "A propRuleInteger should have int as its property type.");
            Assert.AreEqual("2", ((IPropRule)rule).Parameters["min"]);
            Assert.AreEqual("10", ((IPropRule)rule).Parameters["max"]);
        }

        [Test]
        public void TestPropRuleIntegerNoValues()
        {
            IPropRule rule =
                _loader.LoadRule(typeof (int).Name,
                    @"<rule name=""TestRule"" message=""Test Message""><add key=""max"" value=""1""/></rule>");
            Assert.AreEqual(int.MinValue, Convert.ToInt32(rule.Parameters["min"]));
            rule =
                _loader.LoadRule(typeof (int).Name,
                    @"<rule name=""TestRule"" message=""Test Message""><add key=""min"" value=""1""/></rule>");
            Assert.AreEqual(int.MaxValue, Convert.ToInt32(rule.Parameters["max"]));
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
            IPropRule rule =
                _loader.LoadRule(typeof (string).Name,
                    @"<rule name=""TestString"" message=""String Test Message""><add key=""maxLength"" value=""100""/></rule>");

            Assert.AreEqual("TestString", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual("String Test Message", rule.Message, "Message is not being read from xml correctly");
            //Assert.AreSame(typeof(string), propRule.PropertyType,
            //               "A PropRuleString should have string as its property type.");
            Assert.IsTrue(string.IsNullOrEmpty((string) rule.Parameters["patternMatch"]),
                "An empty string should be the default pattern match string according to the dtd.");
            Assert.AreEqual(0, Convert.ToInt32(rule.Parameters["minLength"]),
                "0 should be the default minlength according to the dtd.");
            rule =
                _loader.LoadRule(typeof (string).Name,
                    @"<rule name=""TestString"" message=""String Test Message""><add key=""minLength"" value=""1""/></rule>");
            Assert.AreEqual(-1, Convert.ToInt32(rule.Parameters["maxLength"]),
                "-1 should be the default maxlength according to the dtd.");
        }

        [Test]
        public void TestPropRuleStringAttributes()
        {
            IPropRule rule = _loader.LoadRule(typeof (string).Name,
                @"<rule name=""TestString"" message=""String Test Message"" >
                            <add key=""patternMatch"" value=""Test Pattern"" />
                            <add key=""minLength"" value=""5"" />          
                            <add key=""maxLength"" value=""10"" />
                        </rule>                          
");

            Assert.AreEqual("Test Pattern", rule.Parameters["patternMatch"]);
            Assert.AreEqual(5, Convert.ToInt32(rule.Parameters["minLength"]));
            Assert.AreEqual(10, Convert.ToInt32(rule.Parameters["maxLength"]));
        }

        [Test]
        public void TestPropRuleDate()
        {
            IPropRule rule = _loader.LoadRule(typeof (DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""01 Feb 2004"" />
                            <add key=""max"" value=""09 Oct 2004"" />
                        </rule>                          
");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            //Assert.AreSame(typeof(DateTime), rule.PropertyType,
            //               "A PropRuleDate should have DateTime as its property type.");
            Assert.AreEqual(new DateTime(2004, 02, 01), Convert.ToDateTime(rule.Parameters["min"]));
            Assert.AreEqual(new DateTime(2004, 10, 09), Convert.ToDateTime(rule.Parameters["max"]));
        }

        [Test]
        public void TestPropRuleDate_Today()
        {
            IPropRule rule = _loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""Today"" />
                            <add key=""max"" value=""Today"" />
                        </rule>                          
");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual("Today", rule.Parameters["min"]);
            Assert.AreEqual("Today", rule.Parameters["max"]);
        }
        [Test]
        public void TestPropRuleDate_MinValue_Today()
        {
            IPropRule rule = _loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""Today"" />
                            <add key=""max"" value=""09 Oct 2004"" />
                        </rule>                          
");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual("Today", rule.Parameters["min"]);
            Assert.AreEqual(new DateTime(2004, 10, 09), Convert.ToDateTime(rule.Parameters["max"]));
        }
        [Test]
        public void TestPropRuleDate_MaxValue_Today()
        {
            IPropRule rule = _loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""01 Feb 2004"" />
                            <add key=""max"" value=""Today"" />
                        </rule>                          
");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual(new DateTime(2004, 02, 01), Convert.ToDateTime(rule.Parameters["min"]));
            Assert.AreEqual("Today", rule.Parameters["max"]);
        }
        [Test]
        public void TestPropRuleDate_Now()
        {
            IPropRule rule = _loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""Now"" />
                            <add key=""max"" value=""Now"" />
                        </rule>                          
");
            Assert.AreEqual("TestDate", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual("Now", rule.Parameters["min"]);
            Assert.AreEqual("Now", rule.Parameters["max"]);
        }

        [Test]
        public void TestPropRuleDecimal()
        {
            IPropRule rule = _loader.LoadRule(typeof (Decimal).Name,
                @"<rule name=""TestDec"" >
                            <add key=""min"" value=""1.5"" />
                            <add key=""max"" value=""8.2"" />
                        </rule>                          
");
            Assert.AreEqual("TestDec", rule.Name, "Rule name is not being read from xml correctly.");
            Assert.AreEqual(1.5, Convert.ToDecimal(rule.Parameters["min"]));
            Assert.AreEqual(8.2, Convert.ToDecimal(rule.Parameters["max"]));
        }

        [Test]
        public void TestPropRuleDecimalNoValues()
        {
            IPropRule rule =
                _loader.LoadRule(typeof (Decimal).Name, @"<rule name=""TestDec""><add key=""max"" value=""1""/></rule>");
            Assert.AreEqual(Decimal.MinValue, Convert.ToDecimal(rule.Parameters["min"]));
            rule =
                _loader.LoadRule(typeof (Decimal).Name, @"<rule name=""TestDec""><add key=""min"" value=""1""/></rule>");
            Assert.AreEqual(Decimal.MaxValue, Convert.ToDecimal(rule.Parameters["max"]));
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException))]
        public void TestNoRuleForCustomType()
        {
            _loader.LoadRule(typeof (TimeSpan).Name,
                @"<rule name=""TestCustom"">
                    <add key=""bob"" value=""billy"" />
                </rule>");
        }

        [Test]
        public void TestCustomRuleClass()
        {
            IPropRule rule = _loader.LoadRule("CustomProperty",
                @"<rule name=""TestCustom"" class=""Habanero.Test.BO.Loaders.MyRule"" assembly=""Habanero.Test.BO"">
                            <add key=""bob"" value=""billy"" />
                        </rule>");
            Assert.AreEqual("billy", rule.Parameters["bob"]);
        }

        [Test, ExpectedException(typeof (TypeLoadException))]
        public void TestCustomRuleMustInheritFromPropRuleBase()
        {
            _loader.LoadRule(typeof (TimeSpan).Name,
                             @"<rule name=""TestCustom"" class=""Habanero.Test.MyBO"" assembly=""Habanero.Test"">
                    <add key=""bob"" value=""billy"" />
                </rule>");
        }
    }

    public class MyRule : PropRuleBase
    {
        public MyRule(string name, string message)
            : base(name, message)
        {
            Bob = "";
        }

        protected internal override void SetupParameters()
        {
            object value = Parameters["bob"];
            if (value != null)
            {
                Bob = (string)value;
            }
        }

        public string Bob
        {
            get { return (string)_parameters["bob"]; }
            set { _parameters["bob"] = value; }
        }

        public override List<string> AvailableParameters
        {
            get
            {
                List<string> parameters = new List<string>();
                parameters.Add("bob");
                return parameters;
            }
        }
    }
}