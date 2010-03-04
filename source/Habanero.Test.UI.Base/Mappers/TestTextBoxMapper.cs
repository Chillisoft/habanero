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

using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestTextBoxMapper.
    /// </summary>
    public abstract class TestTextBoxMapper : TestMapperBase
    {

        protected abstract IControlFactory GetControlFactory();


        protected ITextBox _textBox;
        protected TextBoxMapper _mapper;
        protected Shape _shape;

        [SetUp]
        public void SetupTest()
        {
            _textBox = GetControlFactory().CreateTextBox();
            _mapper = new TextBoxMapper(_textBox,  "ShapeName", false, GetControlFactory());
            _shape = new Shape();
        }

        [Test]
        public void TestConstructor()
        {
            Assert.AreSame(_textBox, _mapper.Control);
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
            Assert.AreEqual("TestShapeName", _textBox.Text, "TextBox value is not set when bo is set.");
        }




        [Test]
        public void TestSettingToAnotherBusinessObject()
        {
            _shape.ShapeName = "TestShapeName";
            _mapper.BusinessObject = _shape;
            Shape sh2 = new Shape();
            sh2.ShapeName = "Different";
            _mapper.BusinessObject = sh2;
            Assert.AreEqual("Different", _textBox.Text, "Setting to another bo doesn't work.");
            _shape.ShapeName = "TestShapeName2";
            Assert.AreEqual("Different", _textBox.Text,
                            "Setting to another bo doesn't remove the property updating event handler of the first.");
        }



        [Test]
        public void TestSettingTextBoxValueWhenBOIsNotSet()
        {
            _shape.ShapeName = "TestShapeName";
            _textBox.Text = "Changed";
            Assert.AreEqual("TestShapeName", _shape.ShapeName,
                            "BO property value shouldn't change since bo of mapper wasn't set.");
            Assert.AreEqual("Changed", _textBox.Text, "Textbox value shouldn't change.");
        }

        [Test]
        public void TestDisplayingRelatedProperty()
        {
            SetupClassDefs("MyValue");
            _mapper = new TextBoxMapper(_textBox, "MyRelationship.MyRelatedTestProp", true, GetControlFactory());
            _mapper.BusinessObject = itsMyBo;
            Assert.AreEqual("MyValue", _textBox.Text);
        }

        [Test]
        public void TestValueWhenUpdatingBOValue()
        {
            _shape.ShapeName = "TestShapeName";
            _mapper.BusinessObject = _shape;
            _shape.ShapeName = "TestShapeName2";
            _mapper.UpdateControlValueFromBusinessObject();
            Assert.AreEqual("TestShapeName2", _textBox.Text, "Text property of textbox is not working.");
        }
        [Test]
        public void TestSettingTextBoxValueUpdatesBO()
        {
            _shape.ShapeName = "TestShapeName";
            _mapper.BusinessObject = _shape;
            _textBox.Text = "Changed";
            _mapper.ApplyChangesToBusinessObject();
            Assert.AreEqual("Changed", _shape.ShapeName, "BO property value isn't changed when textbox text is changed.");
        }
        //[Test]
        //public void TestSettingTextBox_InvalidDataType_RaisesError()
        //{
        //    ClassDef.ClassDefs.Clear();
        //    Shape.CreateTestMapperClassDef();
        //    Shape newShape = new Shape();
        //    ITextBox tbShapeValue = GetControlFactory().CreateTextBox();
        //    TextBoxMapper mapperShapeValue = new TextBoxMapper(tbShapeValue, "ShapeValue", false, GetControlFactory());
        //    newShape.SetPropertyValue("ShapeValue", "111");
        //    mapperShapeValue.BusinessObject = newShape;
        //    tbShapeValue.Text = "Changed";
        //    try
        //    {
        //        mapperShapeValue.ApplyChangesToBusinessObject();
        //        ClassDef.ClassDefs.Clear();
        //        Assert.Fail("should raise error");
        //    }
        //    catch (BusObjectInAnInvalidStateException ex)
        //    {
        //        ClassDef.ClassDefs.Clear();
        //        StringAssert.Contains("could not be updated since the value 'Changed' is not valid for the property 'ShapeValue'", ex.Message);
        //    }
        //}
    }
}