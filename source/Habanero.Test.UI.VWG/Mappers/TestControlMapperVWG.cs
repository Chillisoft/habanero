using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Mappers
{
    [TestFixture]
    public class TestControlMapperVWG : TestControlMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }

        [Test]
        public void TestNormalChangeValue_DoesNotUpdateWithoutCallingMethod()
        {
            ControlMapperStub mapperStub = new ControlMapperStub
                (_txtNormal, "ShapeName", false, GetControlFactory());
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

        [Test]
        public void Test_UpdateErrorProviderError_IfControlHasErrors_WhenBOValid_ShouldClearErrorMessage()
        {
            //---------------Set up test pack-------------------
            Shape shape;
            ControlMapperStub mapperStub;
            ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out mapperStub);
            mapperStub.BusinessObject = shape;
            mapperStub.UpdateErrorProviderErrorMessage();
            shape.ShapeName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            Assert.IsTrue(mapperStub.BusinessObject.IsValid());
            Assert.AreNotEqual("", mapperStub.ErrorProvider.GetError(textBox));
            //---------------Execute Test ----------------------
            mapperStub.UpdateErrorProviderErrorMessage();
            //---------------Test Result -----------------------
            Assert.IsTrue(mapperStub.BusinessObject.IsValid());
            Assert.AreEqual("", mapperStub.ErrorProvider.GetError(textBox));
        }

        [Test]
        public void Test_UpdateErrorProviderError_IfControlHasNoErrors_WhenBOInvalid_ShouldSetsErrorMessage()
        {
            //---------------Set up test pack-------------------
            Shape shape;
            ControlMapperStub textBoxMapper;
            ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out textBoxMapper);
            shape.ShapeName = TestUtil.GetRandomString();
            textBoxMapper.BusinessObject = shape;
            shape.ShapeName = "";
            //---------------Assert Precondition----------------
            Assert.IsFalse(shape.Status.IsValid());
            Assert.AreEqual("", textBoxMapper.ErrorProvider.GetError(textBox));
            //---------------Execute Test ----------------------
            textBoxMapper.UpdateErrorProviderErrorMessage();
            //---------------Test Result -----------------------
            Assert.AreNotEqual("", textBoxMapper.ErrorProvider.GetError(textBox));
        }

    }
}