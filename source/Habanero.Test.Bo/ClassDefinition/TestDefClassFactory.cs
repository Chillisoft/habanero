using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestDefClassFactory
    {

        [Test]
        public void Test_Construct_ConstructedCorrectly()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IDefClassFactory defClassFactory = new DefClassFactory();
            //---------------Test Result -----------------------
            Assert.IsNotNull(defClassFactory);
        }

        [Test] 
        public void Test_CreatePropRuleDecimal()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defClassFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PropRuleDecimal rule = defClassFactory.CreatePropRuleDecimal("SomeName", "SomeMessage");
            //---------------Test Result -----------------------
            Assert.AreEqual("SomeName", rule.Name);
            Assert.AreEqual("SomeMessage", rule.Message);
            Assert.AreEqual(Decimal.MinValue, rule.MinValue);
            Assert.AreEqual(Decimal.MaxValue, rule.MaxValue);

        }

        [Test] public void Test_CreatePropRuleSingle()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defClassFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PropRuleSingle rule = defClassFactory.CreatePropRuleSingle("SomeName", "SomeMessage");
            //---------------Test Result -----------------------
            Assert.AreEqual("SomeName", rule.Name);
            Assert.AreEqual("SomeMessage", rule.Message);
            Assert.AreEqual(Single.MinValue, rule.MinValue);
            Assert.AreEqual(Single.MaxValue, rule.MaxValue);
        }

        [Test] public void Test_CreatePropRuleDouble()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defClassFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PropRuleDouble rule = defClassFactory.CreatePropRuleDouble("SomeName", "SomeMessage");
            //---------------Test Result -----------------------
            Assert.AreEqual("SomeName", rule.Name);
            Assert.AreEqual("SomeMessage", rule.Message);
            Assert.AreEqual(Double.MinValue, rule.MinValue);
            Assert.AreEqual(Double.MaxValue, rule.MaxValue);
        }
    }
}