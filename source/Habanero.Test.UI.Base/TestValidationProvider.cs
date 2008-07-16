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
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestValidationProvider
    {
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestValidationProviderWin : TestValidationProvider
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestValidationProviderGiz : TestValidationProvider
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
        }

        [Test]
        public void TestOneValidationRule()
        {
            //---------------Set up test pack-------------------
            ValidationProvider validationProvider = new ValidationProvider(GetControlFactory().CreateErrorProvider());
            ITextBox textBox1 = GetControlFactory().CreateTextBox();
            ValidationRule validationRule1 = new ValidationRule();
            //---------------Execute Test ----------------------
            validationProvider.SetValidationRule(textBox1,validationRule1);
            //---------------Test Result -----------------------
            Assert.AreEqual(1,validationProvider.GetValidationRules(textBox1).Count);
           // Assert.AreEqual(validationRule1,textBox1Rule);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestMultipleRulesForOneControl()
        {
            //---------------Set up test pack-------------------
            ValidationProvider validationProvider = new ValidationProvider(GetControlFactory().CreateErrorProvider());
            ITextBox textBox1 = GetControlFactory().CreateTextBox();
            ValidationRule validationRule1 = new ValidationRule();
            ValidationRule validationRule2 = new ValidationRule();
            ValidationRule validationRule3 = new ValidationRule();
            //---------------Execute Test ----------------------
            validationProvider.SetValidationRule(textBox1, validationRule1);
            validationProvider.SetValidationRule(textBox1, validationRule2);
            validationProvider.SetValidationRule(textBox1, validationRule3);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, validationProvider.GetValidationRules(textBox1).Count);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestValidation_RequiredFieldOnOneControl()
        {
            //---------------Set up test pack-------------------
            ValidationProvider validationProvider = new ValidationProvider(GetControlFactory().CreateErrorProvider());
            ITextBox textBox1 = GetControlFactory().CreateTextBox();
            ValidationRule validationRule1 = new ValidationRule();
            validationRule1.IsRequired = true;
            validationRule1.DataType = ValidationDataType.Integer;
            validationRule1.MinimumValue = Convert.ToString(2);
            validationRule1.MaximumValue = Convert.ToString(10);
            validationProvider.SetValidationRule(textBox1, validationRule1);
            bool result = false;
            //-------------------Test PreConditions------------
            Assert.IsFalse(result);
            //---------------Execute Test ----------------------
            textBox1.Text = Convert.ToString(5);
            result = validationProvider.Validate();
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestValidation_RequiredFieldFails()
        {
            //---------------Set up test pack-------------------
            ValidationProvider validationProvider = new ValidationProvider(GetControlFactory().CreateErrorProvider());
            ITextBox textBox1 = GetControlFactory().CreateTextBox();
            ValidationRule validationRule1 = new ValidationRule();
            validationRule1.IsRequired = true;
            validationProvider.SetValidationRule(textBox1, validationRule1);
            bool result = true;
            //-------------------Test PreConditions------------
            Assert.IsTrue(result);
            //---------------Execute Test ----------------------
            result = validationProvider.Validate();
            //---------------Test Result -----------------------
            Assert.IsFalse(result);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestValidation_RequiredField_TwoControls()
        {
            //---------------Set up test pack-------------------
            ValidationProvider validationProvider = new ValidationProvider(GetControlFactory().CreateErrorProvider());
            ITextBox textBox1 = GetControlFactory().CreateTextBox();
            ITextBox textBox2 = GetControlFactory().CreateTextBox();
            ValidationRule validationRule1 = new ValidationRule();
            validationRule1.IsRequired = true;
            validationProvider.SetValidationRule(textBox1, validationRule1);
            validationProvider.SetValidationRule(textBox2, validationRule1);
            bool result = false;
            //-------------------Test PreConditions------------
            Assert.IsFalse(result);
            //---------------Execute Test ----------------------
            textBox1.Text = "Hello";
            textBox2.Text = "World";
            result = validationProvider.Validate();
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestValidation_RequiredField_Fails_TwoControls_OneControl_IsIncorrect()
        {
            //---------------Set up test pack-------------------
            ValidationProvider validationProvider = new ValidationProvider(GetControlFactory().CreateErrorProvider());
            ITextBox textBox1 = GetControlFactory().CreateTextBox();
            ITextBox textBox2 = GetControlFactory().CreateTextBox();
            ValidationRule validationRule1 = new ValidationRule();
            validationRule1.IsRequired = true;
            validationProvider.SetValidationRule(textBox1, validationRule1);
            validationProvider.SetValidationRule(textBox2, validationRule1);
            bool result = true;
            //-------------------Test PreConditions------------
            Assert.IsTrue(result);
            //---------------Execute Test ----------------------
            textBox1.Text = "";
            textBox2.Text = "World";
            result = validationProvider.Validate();
            //---------------Test Result -----------------------
            Assert.IsFalse(result);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestValidation_MultipleRules_MultipleControls()
        {
            //---------------Set up test pack-------------------
            ValidationProvider validationProvider = new ValidationProvider(GetControlFactory().CreateErrorProvider());
            ITextBox textBox1 = GetControlFactory().CreateTextBox();
            ITextBox textBox2 = GetControlFactory().CreateTextBox();
            ValidationRule validationRule1 = new ValidationRule();
            ValidationRule validationRule2 = new ValidationRule();
            validationRule2.DataType = ValidationDataType.Integer;
            validationRule2.MinimumValue = Convert.ToString(2);
            validationRule2.MaximumValue = Convert.ToString(10);
            validationRule1.IsRequired = true;
            validationProvider.SetValidationRule(textBox1, validationRule1);
            validationProvider.SetValidationRule(textBox2, validationRule1);
            validationProvider.SetValidationRule(textBox2,validationRule2);
            bool result = false;
            //------------------Test PreConditions-------------
            Assert.IsFalse(result);
            //---------------Execute Test ----------------------
            textBox1.Text = "Hello";
            textBox2.Text = "5";
            result = validationProvider.Validate();
            //---------------Test Result -----------------------
            Assert.IsTrue(result);
            //---------------Tear down -------------------------
        }
    }
}
