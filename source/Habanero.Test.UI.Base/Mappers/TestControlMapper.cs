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
using System.Windows.Forms;
using Habanero.BO.ClassDefinition;
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
                ControlMapperStub mapperStub =
                    new ControlMapperStub(_txtNormal, "ShapeName", false, GetControlFactory());
                mapperStub.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                _shape.ShapeName = "TestShapeName2";
                Assert.AreEqual("TestShapeName2", _txtNormal.Text);
            }

            [Test]
            public void TestNormalChangeValue()
            {
                ControlMapperStub mapperStub =
                    new ControlMapperStub(_txtNormal, "ShapeName", false, GetControlFactory());
                mapperStub.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                _shape.ShapeName = "TestShapeName2";
                Assert.AreEqual("TestShapeName2", _txtNormal.Text);
            }

            [Test]
            public void TestEditsToOrigionalBusinessObjectDoesNotUpdateControlValue()
            {
                //---------------Set up test pack-------------------
                ControlMapperStub mapperStub =
                    new ControlMapperStub(_txtNormal, "ShapeName", false, GetControlFactory());
                mapperStub.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtNormal.Text);
                //_shape.ShapeName = "TestShapeName";

                Shape shape2 = new Shape();
                shape2.ShapeName = "Shape 2 Name";

                mapperStub.BusinessObject = shape2;
                //--------------Assert PreConditions----------------            
                Assert.AreEqual(shape2.ShapeName, _txtNormal.Text);

                //---------------Execute Test ----------------------
                bool controlUpdatedFromBusinessObject = false;
                mapperStub.OnUpdateControlValueFromBusinessObject +=
                    delegate { controlUpdatedFromBusinessObject = true; };
                _shape.ShapeName = "New original shape name";

                //---------------Test Result -----------------------
                Assert.IsFalse
                    (controlUpdatedFromBusinessObject,
                     "Control Should not have been updated when the original prop was changed.");
                Assert.AreEqual(shape2.ShapeName, _txtNormal.Text);
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
//                _normalMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("Different2", _txtNormal.Text);
            }

            [Test]
            public void TestReadOnlyChangeValue()
            {
                _readOnlyMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtReadonly.Text);
                _shape.ShapeName = "TestShapeName2";
//                _readOnlyMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("TestShapeName2", _txtReadonly.Text);
            }

//            [Test]
//            public void TestReadOnlyChangeBO()
//            {
//                _readOnlyMapper.BusinessObject = _shape;
//                Assert.AreEqual("TestShapeName", _txtReadonly.Text);
//                Shape sh2 = new Shape();
//                sh2.ShapeName = "Different";
//                _readOnlyMapper.BusinessObject = sh2;
//                Assert.AreEqual("Different", _txtReadonly.Text);
//                sh2.ShapeName = "Different2";
////                _readOnlyMapper.UpdateControlValueFromBusinessObject();
//                Assert.AreEqual("Different2", _txtReadonly.Text);
//            }

            //test compulsory string property, compu decimal etcetc nb combo box
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
                ControlMapperStub mapperStub =
                    new ControlMapperStub(_txtNormal, "ShapeName", false, GetControlFactory());
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

            [Test]
            public void TestReadOnlyChangeValue()
            {
                _readOnlyMapper.BusinessObject = _shape;
                Assert.AreEqual("TestShapeName", _txtReadonly.Text);
                _shape.ShapeName = "TestShapeName2";
                _readOnlyMapper.UpdateControlValueFromBusinessObject();
                Assert.AreEqual("TestShapeName2", _txtReadonly.Text);
            }
        }

        private ITextBox _txtNormal;
        private ITextBox _txtReadonly;
        private ITextBox _txtReflectedProperty;
        private Shape _shape;
        private TextBoxMapper _normalMapper;
        private TextBoxMapper _readOnlyMapper;
        private TextBoxMapper _reflectedPropertyMapper;

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
            _reflectedPropertyMapper =
                new TextBoxMapper(_txtReflectedProperty, "-ShapeNameGetOnly-", false, GetControlFactory());
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
            IControlMapper mapper =
                ControlMapper.Create
                    ("Habanero.UI.Base.TextBoxMapper", "Habanero.UI.Base", b, "Test", false, GetControlFactory());
            Assert.AreSame(typeof (TextBoxMapper), mapper.GetType());
            Assert.AreSame(b, mapper.Control);
        }

        [Test]
        public void TestCreateMapperWithoutTypeOrAssembly()
        {
            ITextBox b = GetControlFactory().CreateTextBox();
            IControlMapper mapper = ControlMapper.Create(null, null, b, "Test", false, GetControlFactory());
            Assert.AreSame(typeof (TextBoxMapper), mapper.GetType());
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
            Assert.IsFalse
                (_txtReflectedProperty.Enabled,
                 "A reflected property control should be disabled before it gets an object");
            _reflectedPropertyMapper.BusinessObject = _shape;
            Assert.IsFalse
                (_txtReflectedProperty.Enabled, "A reflected property control should be disabled once it has an object");
        }

        [Test]
        public void TestReflectedWithSetEnablesControl()
        {
            ITextBox txtReflectedPropertyWithSet = GetControlFactory().CreateTextBox();
            TextBoxMapper reflectedPropertyWithSetMapper =
                new TextBoxMapper(txtReflectedPropertyWithSet, "-ShapeName-", false, GetControlFactory());
            Assert.IsFalse
                (txtReflectedPropertyWithSet.Enabled,
                 "A reflected property control should be disabled before it gets an object");
            reflectedPropertyWithSetMapper.BusinessObject = _shape;
            Assert.IsTrue
                (txtReflectedPropertyWithSet.Enabled,
                 "A reflected property control should be enabled once it has an object if the reflected property has a set");
        }

        [Test]
        public void TestReflectedChangeValue()
        {
            _reflectedPropertyMapper.BusinessObject = _shape;
            Assert.AreEqual("TestShapeName", _txtReflectedProperty.Text);
            _shape.ShapeName = "TestShapeName2";
            Assert.AreEqual
                ("TestShapeName", _txtReflectedProperty.Text,
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
            Assert.AreEqual
                ("Different", _txtReflectedProperty.Text,
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
            Assert.IsFalse
                (_txtNormal.Enabled,
                 "A control representing a null BO cannot be edited, so it should disable the control");
            _normalMapper.BusinessObject = _shape;
            Assert.IsTrue
                (_txtNormal.Enabled,
                 "A non read-only control representing a BO can be edited, so it should enable the control");
            _normalMapper.BusinessObject = null;
            Assert.IsFalse
                (_txtNormal.Enabled, "A control changed to a null BO cannot be edited, so it should disable the control");
        }

        #endregion //Test Null BO

        #region TestIntRules

        [Test]
        public void Test_ErrorProvider_HasCorrectMessage_ForIntegerDataType_NoRule()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("a");

            //---------------Test Result -----------------------
            StringAssert.Contains("It is not a type of Int32", mapperStub.ErrorProvider.GetError(_txtNormal));
        }

        [Test]
        public void Test_ErrorProvider_HasCorrectMessage_ForIntegerDataType()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("a");

            //---------------Test Result -----------------------
            StringAssert.Contains("It is not a type of Int32", mapperStub.ErrorProvider.GetError(_txtNormal));
        }

        [Test]
        public void Test_ErrorProvider_ValidIntegerString_HasNoErrorMessage()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("3");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        public void Test_ErrorProvider_NoError_Null_ForInt_WithRule()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(null);

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        public void Test_ErrorProvider_NoError_NullString_ForInt_WithRule()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        public void Test_ErrorProvider_HasCorrectMessage_ForInt_LT_Min()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("1");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            StringAssert.Contains("The value cannot be less than 2", errorMessage);
        }

        [Test]
        public void Test_ErrorProvider_HasCorrectMessage_ForInt_GT_Max()
        {
            //---------------Set up test pack-------------------

            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("7");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            StringAssert.Contains("The value cannot be more than 5", errorMessage);
        }

        [Test]
        public void Test_ErrorProvider_ClearsErrorMessage_AfterValidValueIsSet()
        {
            //---------------Set up test pack-------------------

            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithIntegerRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            mapperStub.TestSetPropertyValue("7");

            //---------------Assert Precondition----------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            StringAssert.Contains("The value cannot be more than 5", errorMessage);

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("3");

            //---------------Test Result -----------------------
            errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage),errorMessage);
        }

        #endregion //TestIntRules

        #region TestDecimalRules

        [Test]
        public void Test_ErrorProvider_HasCorrectMessage_ForDecimalDataType_NoRule()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDecimalRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("a");

            //---------------Test Result -----------------------
            StringAssert.Contains("It is not a type of Decimal", mapperStub.ErrorProvider.GetError(_txtNormal));
        }

        [Test]
        public void Test_ErrorProvider_HasCorrectMessage_ForDecimalDataType()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDecimalRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("a");

            //---------------Test Result -----------------------
            StringAssert.Contains("It is not a type of Decimal", mapperStub.ErrorProvider.GetError(_txtNormal));
        }

        [Test]
        public void Test_ErrorProvider_ValidDecimalString_HasNoErrorMessage()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDecimalRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("3.03");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage));
        }

        [Test]
        public void Test_ErrorProvider_Decimal_HasCorrectMessage_LT_Min()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDecimalRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("1.05");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            StringAssert.Contains("The value cannot be less than 2", errorMessage);
        }

        [Test]
        public void Test_ErrorProvider_Decimal_HasCorrectMessage_GT_Max()
        {
            //---------------Set up test pack-------------------

            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDecimalRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("7.02");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            StringAssert.Contains("The value cannot be more than 5", errorMessage);
        }

        #endregion //TestDecimalRules

        #region TestDateTimeRules

        [Test]
        public void Test_ErrorProvider_HasCorrectMessage_ForDateTimeDataType_SetToString_NoRule()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestDateTime", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("Error");

            //---------------Test Result -----------------------
            StringAssert.Contains("It is not a type of DateTime", mapperStub.ErrorProvider.GetError(_txtNormal));
        }

        [Test]
        public void Test_ErrorProvider_HasCorrectMessage_ForDateTimeDataType_SetToInt_NoRule()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestDateTime", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(5);

            //---------------Test Result -----------------------
            StringAssert.Contains("It is not a type of DateTime", mapperStub.ErrorProvider.GetError(_txtNormal));
        }

        [Test]
        public void TestCanSetNonCompulsoryDateTime_ToNullString()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestDateTime", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void TestCanSetNonCompulsoryDateTime_ToNull()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestDateTime", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(null);

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Error returned : " + errorMessage);
        }

        [Test]
        public void Test_DateTime_LT_CorrectErrorMessage()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub =
                new ControlMapperStub(_txtNormal, "TestDateTime2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("2005/05/05");

            //---------------Test Result -----------------------
            StringAssert.Contains("The date cannot be before", mapperStub.ErrorProvider.GetError(_txtNormal));
        }

        [Test]
        public void Test_DateTime_NoErrorMessage_passesRules()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub =
                new ControlMapperStub(_txtNormal, "TestDateTime2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("2005/06/12");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Error returned : " + errorMessage);
        }

        #endregion //TestDateTime

        #region TestString
        [Test]
        public void TestCanSetStringProp_ToGuid()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(Guid.NewGuid());

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void TestCanSetStringProp_ToDecimal()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(10.22544m);

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void Test_ErrorProvider_HasCorrectMessage_ForStringDataType_HasRule()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithStringRule();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(5);

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void TestCanSetStringProp_ToInt_NoRule()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(10);

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void TestCanSetStringProp_ToDateTime()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithDateTime();
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(DateTime.Now);

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }
        #endregion //TestString
        #region LookupList

        [Test]
        public void TestCanSetIntProp_ItemInList()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("1");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void TestCanSetGuidProp_ItemInList()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithLookup(); //valid values s1, s2
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("s1");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void TestCanSetIntProp_NullString_Compulsory()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            StringAssert.Contains
                ("is a compulsory field and has no value", errorMessage,
                 "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void TestCanSetIntProp_Null_Compulsory()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(null);

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            StringAssert.Contains
                ("is a compulsory field and has no value", errorMessage,
                 "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void TestCanSetIntProp_NullString_NotCompulsory()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub =
                new ControlMapperStub(_txtNormal, "SimpleLookupNotCompulsory", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test]
        public void TestCanSetIntProp_Null_NotCompulsory()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub =
                new ControlMapperStub(_txtNormal, "SimpleLookupNotCompulsory", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;
            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue(null);

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test, Ignore("Need to move tests to include BO lookups and then refactor.")]
        public void TestCanSetIntProp_ItemNotInList()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithSimpleIntegerLookup(); //valid values 1, 2, 3
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Assert Precondition---- ------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("5");

            //---------------Test Result -----------------------
            StringAssert.Contains("is not in list", mapperStub.ErrorProvider.GetError(_txtNormal));
        }

        [Test]
        public void Test_CanSetGuidToStringLookupValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithLookup(); //Guid Lookup valid s1 and s2
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("s1");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            Assert.IsTrue(string.IsNullOrEmpty(errorMessage), "Should have no error. Error was : " + errorMessage);
        }

        [Test, Ignore("Need to move tests to include BO lookups and then refactor.")]
        public void Test_NotCanSetGuidToStringLookupValue_InvalidValue()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithLookup(); //Guid Lookup valid s1 and s2
            MyBO testBo = new MyBO();
            ControlMapperStub mapperStub = new ControlMapperStub(_txtNormal, "TestProp2", false, GetControlFactory());
            mapperStub.BusinessObject = testBo;

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            mapperStub.TestSetPropertyValue("invalid");

            //---------------Test Result -----------------------
            string errorMessage = mapperStub.ErrorProvider.GetError(_txtNormal);
            StringAssert.Contains("is not in list", errorMessage);
        }
        #endregion //LookupList

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

    internal class ControlMapperStub : ControlMapper
    {
        public ControlMapperStub(IControlChilli ctl, string propName, bool isReadOnly, IControlFactory factory)
            : base(ctl, propName, isReadOnly, factory)
        {
        }

        private MethodInvoker _onUpdateControlValueFromBusinessObject;


        public MethodInvoker OnUpdateControlValueFromBusinessObject
        {
            get { return _onUpdateControlValueFromBusinessObject; }
            set { _onUpdateControlValueFromBusinessObject = value; }
        }

        public override void UpdateControlValueFromBusinessObject()
        {
            if (_onUpdateControlValueFromBusinessObject != null)
            {
                _onUpdateControlValueFromBusinessObject();
            }
            base.UpdateControlValueFromBusinessObject();
        }

        public override void ApplyChangesToBusinessObject()
        {
            throw new NotImplementedException();
        }

        protected override void InternalUpdateControlValueFromBo()
        {
            this.Control.Text = Convert.ToString(this.BusinessObject.GetPropertyValue(this.PropertyName));
        }

        public void TestSetPropertyValue(object value)
        {
            SetPropertyValue(value);
        }
    }
}