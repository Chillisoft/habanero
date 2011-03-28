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
    /// Summary description for TestXmlRuleLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlRuleLoader
    {
        // ReSharper disable InconsistentNaming
        private XmlRuleLoader _loader;

        [SetUp]
        public virtual void SetupTest()
        {
            Initialise();
                        GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected void Initialise()
        {
            _loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestNoKeyAttribute()
        {
            try
            {
                _loader.LoadRule(typeof (int).Name,
                                 @"
                <rule name=""TestRule"" message=""Test Message"">
                    <add value=""1"" />
                </rule>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An 'add' attribute in the class definitions was missing the required 'key' attribute, which specifies the name of the rule to check", ex.Message);
            }
        }

        [Test]
        public void TestNoValueAttribute()
        {
            try
            {
                _loader.LoadRule(typeof (int).Name,
                                 @"
                <rule name=""TestRule"" message=""Test Message"">
                    <add key=""max"" />
                </rule>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An 'add' attribute in the class definitions was missing the required 'value' attribute, which specifies the value to compare with for the ", ex.Message);
            }
        }

        [Test]
        public void TestNoAddElements()
        {
            try
            {
                _loader.LoadRule(typeof (int).Name,
                                 @"
                <rule name=""TestRule"" message=""Test Message"">
                </rule>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("A 'rule' element in the class definitions must contain at least one 'add' element for each component of the rule, such as the minimum ", ex.Message);
            }
        }
        
        [Test]
        public void TestRuleOfInteger()
        {
            //---------------Execute Test ----------------------
            IPropRule rule =
                _loader.LoadRule(typeof(int).Name,
                                 @"<rule name=""Test Rule"" message=""Test Message""><add key=""min"" value=""2""/><add key=""max"" value=""10"" /></rule>");
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(IPropRule), rule);
            Assert.AreEqual("Test Rule", rule.Name, "Name name is not being read from xml correctly.");
            Assert.AreEqual("Test Message", rule.Message, "Message is not being read from xml correctly.");
            Assert.AreEqual(2, Convert.ToInt32(rule.Parameters["min"]));
            Assert.AreEqual(10, Convert.ToInt32(rule.Parameters["max"]));
        }
        
        [Test]
        public void TestPropRuleIntegerNoMinValue()
        {
            //---------------Execute Test ----------------------
            IPropRule rule =
                _loader.LoadRule(typeof(int).Name,
                                 @"<rule name=""TestRule"" message=""Test Message""><add key=""max"" value=""1""/></rule>");
            //---------------Test Result -----------------------
            Assert.AreEqual(int.MinValue, Convert.ToInt32(rule.Parameters["min"]));

        }
        [Test]
        public void TestPropRuleIntegerNoMaxValue()
        {
            //---------------Execute Test ----------------------
            IPropRule rule =
                _loader.LoadRule(typeof(int).Name,
                                 @"<rule name=""TestRule"" message=""Test Message""><add key=""min"" value=""1""/></rule>");
            //---------------Test Result -----------------------
            Assert.AreEqual(int.MaxValue, Convert.ToInt32(rule.Parameters["max"]));

        }

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
            //---------------Execute Test ----------------------
            IPropRule rule = _loader.LoadRule(typeof (string).Name,
                @"<rule name=""TestString"" message=""String Test Message"" >
                            <add key=""patternMatch"" value=""Test Pattern"" />
                            <add key=""minLength"" value=""5"" />          
                            <add key=""maxLength"" value=""10"" />
                        </rule>                          
");
            //---------------Test Result -----------------------
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
            //---------------Execute Test ----------------------
            IPropRule rule = _loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""Now"" />
                            <add key=""max"" value=""Now"" />
                        </rule>                          
");
            //---------------Test Result -----------------------
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
        public void TestPropRuleDecimalNoMinValue()
        {
            //---------------Set up test pack-------------------
            IPropRule rule =
                _loader.LoadRule(typeof (Decimal).Name, @"<rule name=""TestDec""><add key=""max"" value=""1""/></rule>");
            //---------------Execute Test ----------------------
            Assert.AreEqual(Decimal.MinValue, Convert.ToDecimal(rule.Parameters["min"]));
        }
        
        [Test]
        public void TestPropRuleDecimalNoMaxValue()
        {
            //---------------Set up test pack-------------------
            IPropRule rule =
                _loader.LoadRule(typeof (Decimal).Name, @"<rule name=""TestDec""><add key=""min"" value=""1""/></rule>");
            //---------------Execute Test ----------------------
            Assert.AreEqual(Decimal.MaxValue, Convert.ToDecimal(rule.Parameters["max"]));
        }

        [Test]
        public void TestNoRuleForCustomType()
        {
            try
            {
                _loader.LoadRule(typeof (TimeSpan).Name,
                                 @"<rule name=""TestCustom"">
                    <add key=""bob"" value=""billy"" />
                </rule>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("Could not load the Property Rule for this type('TimeSpan'", ex.Message);
            }
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

        [Test]
        public void TestCustomRuleMustInheritFromPropRuleBase()
        {
            try
            {
                _loader.LoadRule(typeof (TimeSpan).Name,
                                 @"<rule name=""TestCustom"" class=""Habanero.Test.MyBO"" assembly=""Habanero.Test"">
                    <add key=""bob"" value=""billy"" />
                </rule>");
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (TypeLoadException ex)
            {
                StringAssert.Contains("The prop rule 'TestCustom' must inherit from PropRuleBase", ex.Message);
            }
        }

        [Test]
        public void Test_CreatePropRule_WhenTypeIsInt32_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            var xmlRuleLoaderSpy = new XmlRuleLoaderSpy();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string expectedRuleName = "MyRule";
            var createdPropRule = xmlRuleLoaderSpy.CallCreatePropRule("System.Int32", expectedRuleName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleInteger>(createdPropRule);
            Assert.AreEqual(expectedRuleName,createdPropRule.Name);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsString_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            var xmlRuleLoaderSpy = new XmlRuleLoaderSpy();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string expectedRuleName = "MyRule";
            var createdPropRule = xmlRuleLoaderSpy.CallCreatePropRule("System.String", expectedRuleName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleString>(createdPropRule);
            Assert.AreEqual(expectedRuleName, createdPropRule.Name);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsDateTime_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            var xmlRuleLoaderSpy = new XmlRuleLoaderSpy();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = xmlRuleLoaderSpy.CallCreatePropRule("System.DateTime");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleDate>(createdPropRule);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsDecimalShouldConstruct()
        {
            //---------------Set up test pack-------------------
            var xmlRuleLoaderSpy = new XmlRuleLoaderSpy();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = xmlRuleLoaderSpy.CallCreatePropRule("System.Decimal");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleDecimal>(createdPropRule);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsDouble_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            var xmlRuleLoaderSpy = new XmlRuleLoaderSpy();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = xmlRuleLoaderSpy.CallCreatePropRule("System.Double");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleDouble>(createdPropRule);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsSingle_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            var xmlRuleLoaderSpy = new XmlRuleLoaderSpy();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = xmlRuleLoaderSpy.CallCreatePropRule("System.Single");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleSingle>(createdPropRule);
        }

        [Test]
        public void Test_CreatePropRule_WhenTypeIsInt64_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            var xmlRuleLoaderSpy = new XmlRuleLoaderSpy();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = xmlRuleLoaderSpy.CallCreatePropRule("System.Int64");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleLong>(createdPropRule);
        }
    }

// ReSharper disable UnusedMember.Global
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
    public class XmlRuleLoaderSpy : XmlRuleLoader
    {
        public XmlRuleLoaderSpy()
            : base(new DtdLoader(), new DefClassFactory())
        {
        }
        public IPropRule CallCreatePropRule(string propTypeName, string name, string message)
        {
            return this.CreatePropRule(propTypeName, name, message);
        }

        public IPropRule CallCreatePropRule(string propTypeName, string name)
        {
            return CallCreatePropRule(propTypeName, name, "SomeMessage");
        }
        public IPropRule CallCreatePropRule(string propTypeName)
        {
            return CallCreatePropRule(propTypeName, "SomeRuleName", "SomeMessage");
        }
    }
    // ReSharper restore UnusedMember.Global
}