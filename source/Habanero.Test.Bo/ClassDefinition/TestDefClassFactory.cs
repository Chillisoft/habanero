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
            PropRuleDecimal rule = (PropRuleDecimal) defClassFactory.CreatePropRuleDecimal("SomeName", "SomeMessage");
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
            PropRuleSingle rule = (PropRuleSingle) defClassFactory.CreatePropRuleSingle("SomeName", "SomeMessage");
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
            PropRuleDouble rule = (PropRuleDouble) defClassFactory.CreatePropRuleDouble("SomeName", "SomeMessage");
            //---------------Test Result -----------------------
            Assert.AreEqual("SomeName", rule.Name);
            Assert.AreEqual("SomeMessage", rule.Message);
            Assert.AreEqual(Double.MinValue, rule.MinValue);
            Assert.AreEqual(Double.MaxValue, rule.MaxValue);
        }

        [Test]
        public void Test_CreatUIFormProperty()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defClassFactory = new DefClassFactory();
            var label = TestUtil.GetRandomString();

            var propertyName = TestUtil.GetRandomString();
            var controlTypeName = TestUtil.GetRandomString();
            var controlAssembly = TestUtil.GetRandomString();
            var mapperTypeName = TestUtil.GetRandomString();
            var mapperAssembly = TestUtil.GetRandomString();
            var editable = TestUtil.GetRandomBoolean();
            const bool showAsCompulsory = true;
            var toolTipText = TestUtil.GetRandomString();
            const LayoutStyle layoutStyle = LayoutStyle.Label;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var uiFormField = defClassFactory.CreateUIFormProperty(label, propertyName
                                                                      , controlTypeName, controlAssembly, mapperTypeName,
                                                                      mapperAssembly, editable, showAsCompulsory, toolTipText, null, layoutStyle);
            //---------------Test Result -----------------------
            Assert.AreEqual(label, uiFormField.Label);
            Assert.AreEqual(propertyName, uiFormField.PropertyName);
            Assert.AreEqual(controlTypeName, uiFormField.ControlTypeName);
            Assert.AreEqual(controlAssembly, uiFormField.ControlAssemblyName);
            Assert.AreEqual(mapperTypeName, uiFormField.MapperTypeName);
            Assert.AreEqual(editable, uiFormField.Editable);
            Assert.AreEqual(showAsCompulsory, uiFormField.IsCompulsory);
            Assert.AreEqual(toolTipText, uiFormField.ToolTipText);
            Assert.AreEqual(layoutStyle, uiFormField.Layout);
        }
    }
}