//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestTextBoxMapper.
    /// </summary>
    public abstract class TestTextBoxMapper : TestMapperBase
    {


        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestTextBoxMapperWin : TestTextBoxMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }

            [Test]
            public void TestIsValidChar_ReturnsTrueIfBOPropNotSet()
            {
                //---------------Set up test pack-------------------
                TextBoxMapperStrategyWin strategy =
                    (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();

                //---------------Assert pre-condition---------------
                //---------------Execute Test ----------------------

                //---------------Test Result -----------------------
                Assert.IsTrue(strategy.IsValidCharacter('a'));
                Assert.IsTrue(strategy.IsValidCharacter(' '));
                Assert.IsTrue(strategy.IsValidCharacter('.'));
                Assert.IsTrue(strategy.IsValidCharacter('-'));
                //---------------Tear down -------------------------
            }

            [Test]
            public void TestIsValidChar_WithString_ReturnsTrueForNonNumericTypes()
            {
                //---------------Set up test pack-------------------
                TextBoxMapperStrategyWin strategy =
                    (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
                BOProp boProp = CreateBOPropForType(typeof (string));
                strategy.AddKeyPressEvents(_mapper, boProp);
                //---------------Execute Test ----------------------

                //---------------Test Result -----------------------
                Assert.IsTrue(strategy.IsValidCharacter('a'));
                Assert.IsTrue(strategy.IsValidCharacter(' '));
                Assert.IsTrue(strategy.IsValidCharacter('.'));
                Assert.IsTrue(strategy.IsValidCharacter('-'));
                //---------------Tear down -------------------------
            }

            [Test]
            public void TestIsValidChar_WithInt_ReturnsTrueForNumber()
            {
                //---------------Set up test pack-------------------
                TextBoxMapperStrategyWin strategy =
                    (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
                BOProp boProp = CreateBOPropForType(typeof(int));
                strategy.AddKeyPressEvents(_mapper, boProp);
                //---------------Execute Test ----------------------

                //---------------Test Result -----------------------
                Assert.IsTrue(strategy.IsValidCharacter('0'));
                Assert.IsTrue(strategy.IsValidCharacter('9'));
                Assert.IsTrue(strategy.IsValidCharacter('-'));
                Assert.IsTrue(strategy.IsValidCharacter(Convert.ToChar(8)));
                //---------------Tear down -------------------------
            }

            [Test]
            public void TestIsValidChar_WithInt_ReturnsFalseForNonNumber()
            {
                //---------------Set up test pack-------------------
                TextBoxMapperStrategyWin strategy =
                    (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
                BOProp boProp = CreateBOPropForType(typeof(int));
                strategy.AddKeyPressEvents(_mapper, boProp);
                //---------------Execute Test ----------------------

                //---------------Test Result -----------------------
                Assert.IsFalse(strategy.IsValidCharacter('a'));
                Assert.IsFalse(strategy.IsValidCharacter('A'));
                Assert.IsFalse(strategy.IsValidCharacter('+'));
                Assert.IsFalse(strategy.IsValidCharacter(Convert.ToChar(7)));
                //---------------Tear down -------------------------
            }

            [Test, Ignore("Need to do the negative sign test. Negative not allowed when selection start > 0.  Need to finish this true case where allowed, then do false case where inserted at selection start > 0.")]
            public void TestIsValidChar_WithInt_ReturnsTrueForNegativeAtStart()
            {
                //---------------Set up test pack-------------------
                TextBoxMapperStrategyWin strategy =
                    (TextBoxMapperStrategyWin)GetControlFactory().CreateTextBoxMapperStrategy();
                BOProp boProp = CreateBOPropForType(typeof(int));
                strategy.AddKeyPressEvents(_mapper, boProp);
                _mapper.Control.Text = "123";
                ((TextBoxWin) _mapper.Control).SelectionStart = 0;
                //---------------Execute Test ----------------------

                //---------------Test Result -----------------------
                Assert.IsTrue(strategy.IsValidCharacter('-'));
                //---------------Tear down -------------------------
            }

            //TODO test decimals now
            //TODO then look at the value changed stuff (see original texboxmapper)

            private static BOProp CreateBOPropForType(Type type)
            {
                PropDef propDef = new PropDef("Prop", type, PropReadWriteRule.ReadWrite, null);
                return new BOProp(propDef);
            }
        }

        [TestFixture]
        public class TestTextBoxMapperGiz : TestTextBoxMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }
        }

        private ITextBox _tb;
        private TextBoxMapper _mapper;
        private Shape _shape;

        [SetUp]
        public void SetupTest()
        {
            _tb = GetControlFactory().CreateTextBox();
            _mapper = new TextBoxMapper(_tb,  "ShapeName", false, GetControlFactory());
            _shape = new Shape();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(_tb, _mapper.Control);
            Assert.AreSame("ShapeName", _mapper.PropertyName);
        }

        [Test]
        public void TestBusinessObject()
        {
            _mapper.BusinessObject = _shape;
            Assert.AreSame(_shape, _mapper.BusinessObject);
        }

        [Test]
        public void TestValueWhenSettingBO()
        {
            _shape.ShapeName = "TestShapeName";
            _mapper.BusinessObject = _shape;
            Assert.AreEqual("TestShapeName", _tb.Text, "TextBox value is not set when bo is set.");
        }




        [Test]
        public void TestSettingToAnotherBusinessObject()
        {
            _shape.ShapeName = "TestShapeName";
            _mapper.BusinessObject = _shape;
            Shape sh2 = new Shape();
            sh2.ShapeName = "Different";
            _mapper.BusinessObject = sh2;
            Assert.AreEqual("Different", _tb.Text, "Setting to another bo doesn't work.");
            _shape.ShapeName = "TestShapeName2";
            Assert.AreEqual("Different", _tb.Text,
                            "Setting to another bo doesn't remove the property updating event handler of the first.");
        }



        [Test]
        public void TestSettingTextBoxValueWhenBOIsNotSet()
        {
            _shape.ShapeName = "TestShapeName";
            _tb.Text = "Changed";
            Assert.AreEqual("TestShapeName", _shape.ShapeName,
                            "BO property value shouldn't change since bo of mapper wasn't set.");
            Assert.AreEqual("Changed", _tb.Text, "Textbox value shouldn't change.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs("MyValue");
            _mapper = new TextBoxMapper(_tb, "MyRelationship.MyRelatedTestProp", true, GetControlFactory());
            _mapper.BusinessObject = itsMyBo;
            Assert.AreEqual("MyValue", _tb.Text);
        }

        [Test]
        public void TestValueWhenUpdatingBOValue()
        {
            _shape.ShapeName = "TestShapeName";
            _mapper.BusinessObject = _shape;
            _shape.ShapeName = "TestShapeName2";
            _mapper.UpdateControlValueFromBusinessObject();
            Assert.AreEqual("TestShapeName2", _tb.Text, "Text property of textbox is not working.");
        }
        [Test]
        public void TestSettingTextBoxValueUpdatesBO()
        {
            _shape.ShapeName = "TestShapeName";
            _mapper.BusinessObject = _shape;
            _tb.Text = "Changed";
            _mapper.ApplyChangesToBusinessObject();
            Assert.AreEqual("Changed", _shape.ShapeName, "BO property value isn't changed when textbox text is changed.");
        }
        [Test]
        public void TestSettingTextBox_InvalidDataType_RaisesError()
        {
            ClassDef.ClassDefs.Clear();
            Shape.CreateTestMapperClassDef();
            Shape newShape = new Shape();
            ITextBox tbShapeValue = GetControlFactory().CreateTextBox();
            TextBoxMapper mapperShapeValue = new TextBoxMapper(tbShapeValue, "ShapeValue", false, GetControlFactory());
            newShape.SetPropertyValue("ShapeValue", "111");
            mapperShapeValue.BusinessObject = newShape;
            tbShapeValue.Text = "Changed";
            try
            {
                mapperShapeValue.ApplyChangesToBusinessObject();
                ClassDef.ClassDefs.Clear();
                Assert.Fail("should raise error");
            }
            catch (BusObjectInAnInvalidStateException ex)
            {
                ClassDef.ClassDefs.Clear();
                StringAssert.Contains("could not be updated since the value 'Changed' is not valid for the property 'ShapeValue'", ex.Message);
            }
        }
    }
}