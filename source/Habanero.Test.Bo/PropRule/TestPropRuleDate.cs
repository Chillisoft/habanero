#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestPropRuleDate
    {
        [Test]
        public void TestDateRule()
        {
            //	PropDef lPropDef = new PropDef("Surname", typeof(string),PropReadWriteRule.ReadWrite);
            PropRuleDate rule =
                new PropRuleDate("BirthDate", "Test", new DateTime(1900, 01, 01), new DateTime(2010, 12, 31));

            string errorMessage = "";

            //Test less than max length
            Assert.IsFalse(rule.IsPropValueValid("Propname", new DateTime(1891, 01, 14), ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.IsPropValueValid("Propname", new DateTime(1991, 01, 14), ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max length
            Assert.IsFalse(rule.IsPropValueValid("Propname", new DateTime(2091, 01, 14), ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);


            rule = new PropRuleDate("BirthDate", "Test");
            errorMessage = "";

            Assert.IsTrue(rule.IsPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";

            //Test valid data
            Assert.IsTrue(rule.IsPropValueValid("Propname", new DateTime(1991, 01, 14), ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);

            //Test valid data
            Assert.IsFalse(rule.IsPropValueValid("Propname","should be false", ref errorMessage),"Should get data type mismatch test");
            Assert.IsTrue(errorMessage.Length > 0);
            StringAssert.Contains("It is not a type of DateTime", errorMessage);
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
            var minValue = new DateTime(1900, 01, 01);
            var maxValue = new DateTime(2010, 12, 31);
            IPropRuleComparable<DateTime> rule =
               new PropRuleDate("BirthDate", "Test", minValue, maxValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(minValue, rule.MinValue);
            Assert.AreEqual(maxValue.AddDays(1).AddMilliseconds(-1), rule.MaxValue);
        }
        [Test]
        public void TestPropRuleDate_MaxValue_Today()
        {
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            IPropRule rule = loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""01 Feb 2004"" />
                            <add key=""max"" value=""Today"" />
                        </rule>                          
                ");
            //-----------------Assert Preconditions ---------------------------
            Assert.AreEqual(new DateTime(2004, 02, 01), ((PropRuleDate)rule).MinValue);
            Assert.AreEqual(DateTime.Today.AddDays(1).AddMilliseconds(-1), ((PropRuleDate)rule).MaxValue);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", DateTime.Today.AddDays(-1), ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsTrue( isValid);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        public void TestPropRuleDate_MaxValue_Today_ActualValueToday()
        {
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            IPropRule rule = loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""01 Feb 2004"" />
                            <add key=""max"" value=""Today"" />
                        </rule>                          
                ");
            //-----------------Assert Preconditions ---------------------------
            Assert.AreEqual(new DateTime(2004, 02, 01), ((PropRuleDate)rule).MinValue);
            Assert.AreEqual(DateTime.Today.AddDays(1).AddMilliseconds(-1), ((PropRuleDate)rule).MaxValue);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", DateTime.Today, ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsTrue(isValid);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }
        [Test]
        public void TestPropRuleDate_MaxValue_Today_ActualValueGTToday()
        {
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            IPropRule rule = loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""01 Feb 2004"" />
                            <add key=""max"" value=""Today"" />
                        </rule>                          
                ");
            //-----------------Assert Preconditions ---------------------------
            Assert.AreEqual(new DateTime(2004, 02, 01), ((PropRuleDate)rule).MinValue);
            Assert.AreEqual(DateTime.Today.AddDays(1).AddMilliseconds(-1), ((PropRuleDate)rule).MaxValue);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", DateTime.Today.AddDays(2), ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        public void Test_MaxAndMinValue_WhenYesterday_ShouldRetYesterday()
        {
            //---------------Set up test pack-------------------
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            PropRuleDate rule = (PropRuleDate) loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""yesterday"" />
                            <add key=""max"" value=""yesterday"" />
                        </rule>                          
                ");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime maxDateTime = rule.MaxValue;
            DateTime minDateTime = rule.MinValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTimeToday.Value.AddMilliseconds(-1), maxDateTime);
            Assert.AreEqual(DateTimeToday.Value.AddDays(-1), minDateTime);
        }
        [Test]
        public void Test_MaxAndMinValue_WhenToday_ShouldRetToday()
        {
            //---------------Set up test pack-------------------
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            PropRuleDate rule = (PropRuleDate) loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""today"" />
                            <add key=""max"" value=""today"" />
                        </rule>                          
                ");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime maxDateTime = rule.MaxValue;
            DateTime minDateTime = rule.MinValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTimeToday.Value.AddDays(1).AddMilliseconds(-1), maxDateTime);
            Assert.AreEqual(DateTimeToday.Value, minDateTime);
        }

        [Test]
        public void Test_MaxAndMinValue_WhenTomorrow_ShouldRetTomorrow()
        {
            //---------------Set up test pack-------------------
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            PropRuleDate rule = (PropRuleDate) loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""tomorrow"" />
                            <add key=""max"" value=""tomorrow"" />
                        </rule>                          
                ");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime maxDateTime = rule.MaxValue;
            DateTime minDateTime = rule.MinValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTimeToday.Value.AddDays(2).AddMilliseconds(-1), maxDateTime);
            Assert.AreEqual(DateTimeToday.Value.AddDays(1), minDateTime);
        }

        [Test]
        public void Test_LoadRule_WhenMaxToday_ShouldUseStringMaxValueExpression()
        {
            //---------------Set up test pack-------------------
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PropRuleDate rule = (PropRuleDate) loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""today"" />
                            <add key=""max"" value=""today"" />
                        </rule>                          
                ");
            //---------------Test Result -----------------------
            Assert.AreEqual("today", rule.Parameters["max"]);
            Assert.AreEqual("today", rule.Parameters["min"]);
        }
        [Test]
        public void Test_LoadRule_WhenMaxYesterday_ShouldUseStringMaxValueExpression()
        {
            //---------------Set up test pack-------------------
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PropRuleDate rule = (PropRuleDate)loader.LoadRule(typeof(DateTime).Name,
                @"<rule name=""TestDate""  >
                            <add key=""min"" value=""yesterday"" />
                            <add key=""max"" value=""yesterday"" />
                        </rule>                          
                ");
            //---------------Test Result -----------------------
            Assert.AreEqual("yesterday", rule.Parameters["max"]);
            Assert.AreEqual("yesterday", rule.Parameters["min"]);
        }

        [Test]
        public void Test_WhenMinBlank_ShouldUseDateTimeMinValue()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            PropRuleDate rule = new PropRuleDate("MyRule", "MyMessage");
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.MinValue, rule.MinValue);
        }

        [Test]
        public void Test_WhenMaxBlank_ShouldUseDateTimeMaxValue()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            PropRuleDate rule = new PropRuleDate("MyRule", "MyMessage");
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.MaxValue, rule.MaxValue);
        }

        [Test]
        public void Test_MaxValue_WhenDateTimeMaxValueLess1Minute_ShouldNotRaiseError()
        {
            //---------------Set up test pack-------------------
            PropRuleDate rule = new PropRuleDate("MyRule", "MyMessage");
            var maxDateTime = DateTime.MaxValue.AddMilliseconds(-1);
            rule.Parameters["max"] = maxDateTime;
            //---------------Assert Precondition----------------
            Assert.AreEqual(maxDateTime, rule.Parameters["max"]);
            //---------------Execute Test ----------------------
            var actualMaxValue = rule.MaxValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(maxDateTime, actualMaxValue);
        }
    }

}
