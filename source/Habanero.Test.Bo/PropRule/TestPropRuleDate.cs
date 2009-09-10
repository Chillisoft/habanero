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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
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
            Assert.AreEqual(DateTime.Today, ((PropRuleDate)rule).MaxValue.Date);

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
            Assert.AreEqual(DateTime.Today, ((PropRuleDate)rule).MaxValue.Date);

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
            Assert.AreEqual(DateTime.Today, ((PropRuleDate)rule).MaxValue.Date);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", DateTime.Today.AddDays(1), ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
        }
    }

}
