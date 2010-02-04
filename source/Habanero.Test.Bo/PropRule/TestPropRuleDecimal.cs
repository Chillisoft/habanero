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
    public class TestPropRuleDecimal
    {
        [Test]
        public void TestDecimalRule()
        {
            PropRuleDecimal rule = new PropRuleDecimal("num", "TestMessage", 5.32m, 10.11111m);

            string errorMessage = "";

            //Test less than min
            Assert.IsFalse(rule.IsPropValueValid("Propname", 5.31116m, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.IsPropValueValid("Propname", 6, ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max
            Assert.IsFalse(rule.IsPropValueValid("Propname", 10.1111112m, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);

            rule = new PropRuleDecimal("num", "TestMessage", 5.32m, 10.11111m);
            errorMessage = "";

            Assert.IsTrue(rule.IsPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";
            Assert.IsFalse(rule.IsPropValueValid("Propname", -53444.33222m, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }
        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestPropRuleDecimal_MaxValue_ActualValueLT()
        {
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            IPropRule rule = loader.LoadRule(typeof(Decimal).Name,
                                                @"<rule name=""TestDecimal""  >
                            <add key=""min"" value=""12.22"" />
                            <add key=""max"" value=""15.51"" />
                        </rule>                          
");
            //-----------------Assert Preconditions ---------------------------
            Assert.AreEqual(12.22d, ((PropRuleDecimal)rule).MinValue);
            Assert.AreEqual(15.51d, ((PropRuleDecimal)rule).MaxValue);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", 13.1d, ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsTrue(isValid);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        public void TestPropRuleDecimal_MaxValue_ActualValueEquals()
        {
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            IPropRule rule = loader.LoadRule(typeof(Decimal).Name,
                                                @"<rule name=""TestDecimal""  >
                            <add key=""min"" value=""12.22"" />
                            <add key=""max"" value=""15.51"" />
                        </rule>                          
");
            //-----------------Assert Preconditions ---------------------------
            Assert.AreEqual(12.22d, ((PropRuleDecimal)rule).MinValue);
            Assert.AreEqual(15.51d, ((PropRuleDecimal)rule).MaxValue);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", 15.51d, ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsTrue(isValid);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        public void TestPropRuleDecimal_MaxValue_ActualValueGT()
        {
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            IPropRule rule = loader.LoadRule(typeof(Decimal).Name,
                                                @"<rule name=""TestDecimal""  >
                            <add key=""min"" value=""12.22"" />
                            <add key=""max"" value=""15.51"" />
                        </rule>                          
");
            //-----------------Assert Preconditions ---------------------------
            Assert.AreEqual(12.22D, ((PropRuleDecimal)rule).MinValue);
            Assert.AreEqual(15.51D, ((PropRuleDecimal)rule).MaxValue);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", new Decimal(15.56), ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
        }
    }
}
