using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    [TestFixture]
    public class TestControlMapperWin : TestControlMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void TestNormalChangeValue_DoesUpdateWithoutCallingUpdate()
        {
            ControlMapperStub mapperStub = new ControlMapperStub
                (_txtNormal, "ShapeName", false, GetControlFactory());
            mapperStub.BusinessObject = _shape;
            Assert.AreEqual("TestShapeName", _txtNormal.Text);
            _shape.ShapeName = "TestShapeName2";
            Assert.AreEqual("TestShapeName2", _txtNormal.Text);
        }

        [Test]
        public void TestNormalChangeValue()
        {
            ControlMapperStub mapperStub = new ControlMapperStub
                (_txtNormal, "ShapeName", false, GetControlFactory());
            mapperStub.BusinessObject = _shape;
            Assert.AreEqual("TestShapeName", _txtNormal.Text);
            _shape.ShapeName = "TestShapeName2";
            Assert.AreEqual("TestShapeName2", _txtNormal.Text);
        }

        [Test]
        public void TestEditsToOrigionalBusinessObjectDoesNotUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            ControlMapperStub mapperStub = new ControlMapperStub
                (_txtNormal, "ShapeName", false, GetControlFactory());
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

        //This test is different from VWG because an edit to the properties in Windows updates the 
        // control and must therefore update the Error provider whereas for VWG it is done only when you
        // specifically update the control with the BO Values.
        [Test]
        public void Test_EditBusinessObjectProp_IfControlHasErrors_WhenBOValid_ShouldClearErrorMessage()
        {
            //---------------Set up test pack-------------------
            Shape shape;
            ControlMapperStub mapperStub;
            ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out mapperStub);
            mapperStub.BusinessObject = shape;
            //---------------Assert Precondition----------------
            Assert.IsFalse(mapperStub.BusinessObject.IsValid());
            Assert.AreNotEqual("", mapperStub.ErrorProvider.GetError(textBox));
            //---------------Execute Test ----------------------
            shape.ShapeName = TestUtil.GetRandomString();
            //---------------Test Result -----------------------
            Assert.IsTrue(mapperStub.BusinessObject.IsValid());
            Assert.AreEqual("", mapperStub.ErrorProvider.GetError(textBox));
        }
        //This test is different from VWG because an edit to the properties in Windows updates the 
        // control and must therefore update the Error provider whereas for VWG it is done only when you
        // specifically update the control with the BO values
        [Test]
        public void Test_UpdateErrorProviderError_IfControlHasNoErrors_WhenBOInvalid_ShouldSetsErrorMessage()
        {
            //---------------Set up test pack-------------------
            Shape shape;
            ControlMapperStub textBoxMapper;
            ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out textBoxMapper);
            shape.ShapeName = TestUtil.GetRandomString();
            textBoxMapper.BusinessObject = shape;

            //---------------Assert Precondition----------------
            Assert.IsTrue(shape.Status.IsValid());
            Assert.AreEqual("", textBoxMapper.ErrorProvider.GetError(textBox));
            //---------------Execute Test ----------------------
            shape.ShapeName = "";
            //---------------Test Result -----------------------
            Assert.IsFalse(shape.Status.IsValid());
            Assert.AreNotEqual("", textBoxMapper.ErrorProvider.GetError(textBox));
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
}