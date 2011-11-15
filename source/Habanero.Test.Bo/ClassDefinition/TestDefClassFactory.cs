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
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestDefClassFactory
    {
        // ReSharper disable InconsistentNaming
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


        [Test]
        public void Test_CreatePropRuleInt32_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string expectedRuleName = "MyRule";
            var createdPropRule = defFactory.CreatePropRuleInteger(expectedRuleName, "fdafasdf");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleInteger>(createdPropRule);
            Assert.AreEqual(expectedRuleName, createdPropRule.Name);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsString_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            const string expectedRuleName = "MyRule";
            var createdPropRule = defFactory.CreatePropRuleString(expectedRuleName, "fdafasd");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleString>(createdPropRule);
            Assert.AreEqual(expectedRuleName, createdPropRule.Name);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsDateTime_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = defFactory.CreatePropRuleDate("fdasfd", "fdsafasd");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleDate>(createdPropRule);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsDecimalShouldConstruct()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = defFactory.CreatePropRuleDecimal("fdasfd", "fdsafasd");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleDecimal>(createdPropRule);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsDouble_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = defFactory.CreatePropRuleDouble("fdasfd", "fdsafasd");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleDouble>(createdPropRule);
        }
        [Test]
        public void Test_CreatePropRule_WhenTypeIsSingle_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = defFactory.CreatePropRuleSingle("fdasfd", "fdsafasd");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleSingle>(createdPropRule);
        }

        [Test]
        public void Test_CreatePropRule_WhenTypeIsInt64_ShouldConstruct()
        {
            //---------------Set up test pack-------------------
            IDefClassFactory defFactory = new DefClassFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var createdPropRule = defFactory.CreatePropRuleLong("fdasfd", "fdsafasd");
            //---------------Test Result -----------------------
            Assert.IsNotNull(createdPropRule);
            Assert.IsInstanceOf<PropRuleLong>(createdPropRule);
        }

    }
}