using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestPropRuleDouble
    {
        [Test]
        public void TestDoubleRule()
        {
            PropRuleDouble rule = new PropRuleDouble("num", "TestMessage", 5.32d, 10.11111d);

            string errorMessage = "";

            //Test less than min
            Assert.IsFalse(rule.IsPropValueValid("Propname", 5.31116d, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
            //Test valid data
            Assert.IsTrue(rule.IsPropValueValid("Propname", 6, ref errorMessage));
            Assert.IsFalse(errorMessage.Length > 0);
            //test greater than max
            Assert.IsFalse(rule.IsPropValueValid("Propname", 10.1111112d, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);

            rule = new PropRuleDouble("num", "TestMessage", 5.32d, 10.11111d);
            errorMessage = "";

            Assert.IsTrue(rule.IsPropValueValid("Propname", null, ref errorMessage));
            Assert.IsTrue(errorMessage.Length == 0);
            errorMessage = "";
            Assert.IsFalse(rule.IsPropValueValid("Propname", -53444.33222d, ref errorMessage));
            Assert.IsTrue(errorMessage.Length > 0);
        }
        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }


        [Test]
        public void TestPropRuleDouble_MaxValue_ActualValueLT()
        {
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            IPropRule rule = loader.LoadRule(typeof(Double).Name,
                                                @"<rule name=""TestDouble""  >
                            <add key=""min"" value=""12.22"" />
                            <add key=""max"" value=""15.51"" />
                        </rule>                          
");
            //-----------------Assert Preconditions ---------------------------
            Assert.AreEqual(12.22d, ((PropRuleDouble)rule).MinValue);
            Assert.AreEqual(15.51d, ((PropRuleDouble)rule).MaxValue);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", 13.1d, ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsTrue(isValid);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        public void TestPropRuleDouble_MaxValue_ActualValueEquals()
        {
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            IPropRule rule = loader.LoadRule(typeof(Double).Name,
                                                @"<rule name=""TestDouble""  >
                            <add key=""min"" value=""12.22"" />
                            <add key=""max"" value=""15.51"" />
                        </rule>                          
");
            //-----------------Assert Preconditions ---------------------------
            Assert.AreEqual(12.22d, ((PropRuleDouble)rule).MinValue);
            Assert.AreEqual(15.51d, ((PropRuleDouble)rule).MaxValue);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", 15.51d, ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsTrue(isValid);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }
        [Test]
        public void TestPropRuleDouble_MaxValue_ActualValueGT()
        {
            XmlRuleLoader loader = new XmlRuleLoader(new DtdLoader(), GetDefClassFactory());
            IPropRule rule = loader.LoadRule(typeof(Double).Name,
                                                @"<rule name=""TestDouble""  >
                            <add key=""min"" value=""12.22"" />
                            <add key=""max"" value=""15.51"" />
                        </rule>                          
");
            //-----------------Assert Preconditions ---------------------------
            Assert.AreEqual(12.22d, ((PropRuleDouble)rule).MinValue);
            Assert.AreEqual(15.51d, ((PropRuleDouble)rule).MaxValue);

            //---------------Execute ------------------------------------------
            string errorMessage = "";
            bool isValid = rule.IsPropValueValid("Propname", 15.56d, ref errorMessage);

            //--------------Verify Result -------------------------------------
            Assert.IsFalse(isValid);
            Assert.IsFalse(string.IsNullOrEmpty(errorMessage));
        }
    }
}