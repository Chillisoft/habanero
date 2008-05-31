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


using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// Summary description for TestControlMapper.
    /// </summary>
    public abstract class TestControlMapper : TestUsingDatabase
    {

        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestControlMapperWin : TestControlMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }

            [Test]
            public void TestNormalChangeValue_DoesUpdateWithoutCallingUpdate()
            {
                ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "ShapeName", false, GetControlFactory());
                mapperStub.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                _shape.ShapeName = "TestShapeName2";
//                _normalMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("TestShapeName2", _txtNormal.Text);
            }
            [Test]
            public void TestNormalChangeValue()
            {
                ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "ShapeName", false, GetControlFactory());
                mapperStub.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                _shape.ShapeName = "TestShapeName2";
                //                _normalMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("TestShapeName2", _txtNormal.Text);
            }

            [Test, Ignore("To implement RemoveCurrentBOPropHandlers")]
            public void TestEditsToOrigionalBusinessObjectDoesNotUpdateControlValue()
            {
                //---------------Set up test pack-------------------
                ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "ShapeName", false, GetControlFactory());
                mapperStub.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                _shape.ShapeName = "TestShapeName";

                Shape shape2 = new Shape();
                shape2.ShapeName = "Shape 2 Name";

                _normalMapper.BusinessObject = shape2;
                //--------------Assert PreConditions----------------            
                Assert.AreEqual(shape2.ShapeName, _txtNormal.Text);

                //---------------Execute Test ----------------------

                _shape.ShapeName = "New shape 1 name";

                //---------------Test Result -----------------------
                Assert.AreEqual(shape2.ShapeName, _txtNormal.Text);


                //---------------Tear Down -------------------------          
            }
                            
            [Test]
            public void TestNormalChangeBO_DoesUpdateWithoutCallingUpdate()
            {
                _normalMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                Shape shape2 = new Shape();
                shape2.ShapeName = "Different";
                _normalMapper.BusinessObject = shape2;
                Assert.AreEqual("Different", _txtNormal.Text);
                shape2.ShapeName = "Different2";
                //_normalMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("Different2", _txtNormal.Text);
            }
            [Test]
            public void TestNormalChangeBO()
            {
                _normalMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                Shape shape2 = new Shape();
                shape2.ShapeName = "Different";
                _normalMapper.BusinessObject = shape2;
                Assert.AreEqual("Different", _txtNormal.Text);
                shape2.ShapeName = "Different2";
                _normalMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("Different2", _txtNormal.Text);
            }
            [Test]
            public void TestReadOnlyChangeValue()
            {
                _readOnlyMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtReadonly.Text);
                _shape.ShapeName = "TestShapeName2";
                _readOnlyMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("TestShapeName2", _txtReadonly.Text);
            }
            [Test]
            public void TestReadOnlyChangeBO()
            {
                _readOnlyMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtReadonly.Text);
                Shape sh2 = new Shape();
                sh2.ShapeName = "Different";
                _readOnlyMapper.BusinessObject = sh2;
                Assert.AreEqual("Different", _txtReadonly.Text);
                sh2.ShapeName = "Different2";
                _readOnlyMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("Different2", _txtReadonly.Text);
            }
        }

        [TestFixture]
        public class TestControlMapperGiz : TestControlMapper
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }

            [Test]
            public void TestNormalChangeValue_DoesNotUpdateWithoutCallingMethod()
            {
                ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "ShapeName", false, GetControlFactory());
                mapperStub.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                _shape.ShapeName = "TestShapeName2";
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
            }

            [Test]
            public void TestNormalChangeValue()
            {
                _normalMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                _shape.ShapeName = "TestShapeName2";
                _normalMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("TestShapeName2", _txtNormal.Text);
            }


            [Test]
            public void TestNormalChangeBO_DoesNotUpdateWithoutCallingMethod()
            {
                _normalMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                Shape shape2 = new Shape();
                shape2.ShapeName = "Different";
                _normalMapper.BusinessObject = shape2;
                Assert.AreEqual("Different", _txtNormal.Text);
                shape2.ShapeName = "Different2";
                Assert.AreEqual("Different", _txtNormal.Text);
            }
            [Test]
            public void TestNormalChangeBO()
            {
                _normalMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                Shape shape2 = new Shape();
                shape2.ShapeName = "Different";
                _normalMapper.BusinessObject = shape2;
                Assert.AreEqual("Different", _txtNormal.Text);
                shape2.ShapeName = "Different2";
                _normalMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("Different2", _txtNormal.Text);
            }
            //TODO: All these have to follow pattern above
            [Test]
            public void TestReadOnlyChangeValue()
            {
                _readOnlyMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtReadonly.Text);
                _shape.ShapeName = "TestShapeName2";
                _readOnlyMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("TestShapeName2", _txtReadonly.Text);
            }
            [Test]
            public void TestReadOnlyChangeBO()
            {
                _readOnlyMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtReadonly.Text);
                Shape sh2 = new Shape();
                sh2.ShapeName = "Different";
                _readOnlyMapper.BusinessObject = sh2;
                Assert.AreEqual("Different", _txtReadonly.Text);
                sh2.ShapeName = "Different2";
                _readOnlyMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("Different2", _txtReadonly.Text);
            }
        }
        ITextBox _txtNormal;
        ITextBox _txtReadonly;
        ITextBox _txtReflectedProperty;
        Shape _shape;
        TextBoxMapper _normalMapper;
        TextBoxMapper _readOnlyMapper;
        TextBoxMapper _reflectedPropertyMapper;

        #region Setup for Tests

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            base.SetupDBConnection();
        }

        [SetUp]
        public void Setup()
        {
            _txtReadonly = GetControlFactory().CreateTextBox();
            _readOnlyMapper = new TextBoxMapper(_txtReadonly, "ShapeName", true, GetControlFactory());
            _txtReflectedProperty = GetControlFactory().CreateTextBox();
            _reflectedPropertyMapper = new TextBoxMapper(_txtReflectedProperty, "-ShapeNameGetOnly-", false, GetControlFactory());
            _txtNormal = GetControlFactory().CreateTextBox();
            _normalMapper = new TextBoxMapper(_txtNormal, "ShapeName", false, GetControlFactory());
            _shape = new Shape();
            _shape.ShapeName = "TestShapeName";
        }

        #endregion //Setup for Tests
		
        #region Test Mapper Creation

        [Test]
        public void TestCreateMapper()
        {
            ITextBox b = GetControlFactory().CreateTextBox();
            IControlMapper mapper = ControlMapper.Create("TextBoxMapper", "", b, "Test", false, GetControlFactory());
            Assert.AreSame(typeof (TextBoxMapper), mapper.GetType());
            Assert.AreSame(b, mapper.Control);
        }

        [Test]
        public void TestCreateMapperWithAssembly()
        {
            ITextBox b = GetControlFactory().CreateTextBox();
            IControlMapper mapper = ControlMapper.Create("Habanero.UI.Base.TextBoxMapper", "Habanero.UI.Base", b, "Test", false, GetControlFactory());
            Assert.AreSame(typeof(TextBoxMapper), mapper.GetType());
            Assert.AreSame(b, mapper.Control);
        }

        [Test]
        public void TestCreateMapperWithoutTypeOrAssembly()
        {
            ITextBox b = GetControlFactory().CreateTextBox();
            IControlMapper mapper = ControlMapper.Create(null, null, b, "Test", false, GetControlFactory());
            Assert.AreSame(typeof(TextBoxMapper), mapper.GetType());
            Assert.AreSame(b, mapper.Control);
        }

        #endregion //Test Mapper Creation

        #region Tests for normal mapper

        [Test]
        public void TestNormalEnablesControl()
        {
            Assert.IsFalse(_txtNormal.Enabled, "A normal control should be disabled before it gets and object");
            _normalMapper.BusinessObject = _shape;
            Assert.IsTrue(_txtNormal.Enabled, "A normal control should be editable once it has an object");
        }





        #endregion

        #region Tests for read-only mapper

        [Test]
        public void TestReadOnlyDisablesControl()
        {
            Assert.IsFalse(_txtReadonly.Enabled, "A read-only control should be disabled before it gets and object");
            _readOnlyMapper.BusinessObject = _shape;
            Assert.IsFalse(_txtReadonly.Enabled, "A read-only control should be disabled once it has an object");
        }


		


        #endregion

        #region Test Reflected Property Mapper

        [Test]
        public void TestReflectedWithNoSetDisablesControl()
        {
            Assert.IsFalse(_txtReflectedProperty.Enabled,
                           "A reflected property control should be disabled before it gets an object");
            _reflectedPropertyMapper.BusinessObject = _shape;
            Assert.IsFalse(_txtReflectedProperty.Enabled,
                           "A reflected property control should be disabled once it has an object");
        }

        [Test]
        public void TestReflectedWithSetEnablesControl()
        {
            ITextBox txtReflectedPropertyWithSet = GetControlFactory().CreateTextBox();
            TextBoxMapper reflectedPropertyWithSetMapper = new TextBoxMapper(txtReflectedPropertyWithSet, "-ShapeName-", false, GetControlFactory());
            Assert.IsFalse(txtReflectedPropertyWithSet.Enabled,
                           "A reflected property control should be disabled before it gets an object");
            reflectedPropertyWithSetMapper.BusinessObject = _shape;
            Assert.IsTrue(txtReflectedPropertyWithSet.Enabled,
                           "A reflected property control should be enabled once it has an object if the reflected property has a set");
        }

        [Test]
        public void TestReflectedChangeValue()
        {
            _reflectedPropertyMapper.BusinessObject = _shape;
            Assert.AreEqual("TestShapeName", _txtReflectedProperty.Text);
            _shape.ShapeName = "TestShapeName2";
            Assert.AreEqual("TestShapeName", _txtReflectedProperty.Text, 
                            "A Reflected property will not be able to pick up changes to the property.");
        }

        [Test]
        public void TestReflectedChangeBO()
        {
            _reflectedPropertyMapper.BusinessObject = _shape;
            Assert.AreEqual("TestShapeName", _txtReflectedProperty.Text);
            Shape sh2 = new Shape();
            sh2.ShapeName = "Different";
            _reflectedPropertyMapper.BusinessObject = sh2;
            Assert.AreEqual("Different", _txtReflectedProperty.Text, 
                            "A Reflected property should refresh the value when a new BO is loaded");
            sh2.ShapeName = "Different2";
            Assert.AreEqual("Different", _txtReflectedProperty.Text);
        }

        #endregion

        #region Test Null BO

        [Test]
        public void TestNullBoDisabled()
        {
            _normalMapper.BusinessObject = null;
            Assert.IsFalse(_txtNormal.Enabled,
                           "A control representing a null BO cannot be edited, so it should disable the control");
            _normalMapper.BusinessObject = _shape;
            Assert.IsTrue(_txtNormal.Enabled,
                          "A non read-only control representing a BO can be edited, so it should enable the control");
            _normalMapper.BusinessObject = null;
            Assert.IsFalse(_txtNormal.Enabled,
                           "A control changed to a null BO cannot be edited, so it should disable the control");
        }

        #endregion //Test Null BO
    }

    internal class ControlMapperStub:ControlMapper
    {

        public ControlMapperStub(IControlChilli ctl, string propName, bool isReadOnly, IControlFactory factory)
            : base(ctl, propName, isReadOnly, factory)
        {
        }

        public override void ApplyChangesToBusinessObject()
        {
            throw new System.NotImplementedException();
        }

        protected override void InternalUpdateControlValueFromBo()
        {
            this.Control.Text = (string) this.BusinessObject.GetPropertyValue(this.PropertyName);
        }
    }
}