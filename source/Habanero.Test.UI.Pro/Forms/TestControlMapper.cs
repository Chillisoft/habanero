using System.Windows.Forms;
using Habanero.Test.General;
using Habanero.UI.Forms;
using NUnit.Framework;

namespace Habanero.Test.UI.Forms
{
    /// <summary>
    /// Summary description for TestControlMapper.
    /// </summary>
    [TestFixture]
    public class TestControlMapper : TestUsingDatabase
    {
        TextBox _txtNormal;
        TextBox _txtReadonly;
        TextBox _txtReflectedProperty;
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
            _txtReadonly = new TextBox();
            _readOnlyMapper = new TextBoxMapper(_txtReadonly, "ShapeName", true);
            _txtReflectedProperty = new TextBox();
            _reflectedPropertyMapper = new TextBoxMapper(_txtReflectedProperty, "-ShapeName-", false);
            _txtNormal = new TextBox();
            _normalMapper = new TextBoxMapper(_txtNormal, "ShapeName", false);
            _shape = new Shape();
            _shape.ShapeName = "TestShapeName";
        }

        #endregion //Setup for Tests
		
        #region Test Mapper Creation

        [Test]
        public void TestCreateMapper()
        {
            TextBox b = new TextBox();
            ControlMapper mapper = ControlMapper.Create("TextBoxMapper", "", b, "Test", false);
            Assert.AreSame(typeof (TextBoxMapper), mapper.GetType());
            Assert.AreSame(b, mapper.Control);
        }

        [Test]
        public void TestCreateMapperWithAssembly()
        {
            TextBox b = new TextBox();
            ControlMapper mapper = ControlMapper.Create("Habanero.UI.Forms.TextBoxMapper", "Habanero.UI.Pro", b, "Test", false);
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

        [Test]
        public void TestNormalChangeValue()
        {
            _normalMapper.BusinessObject = _shape;
            Assert.AreEqual("TestShapeName", _txtNormal.Text);
            _shape.ShapeName = "TestShapeName2";
            Assert.AreEqual("TestShapeName2", _txtNormal.Text);
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
            Assert.AreEqual("Different2", _txtNormal.Text);
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

        [Test]
        public void TestReadOnlyChangeValue()
        {
            _readOnlyMapper.BusinessObject = _shape;
            Assert.AreEqual("TestShapeName", _txtReadonly.Text);
            _shape.ShapeName = "TestShapeName2";
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
            Assert.AreEqual("Different2", _txtReadonly.Text);
        }

        #endregion

        #region Test Reflected Property Mapper
		
        [Test]
        public void TestReflectedDisablesControl()
        {
            Assert.IsFalse(_txtReflectedProperty.Enabled,
                           "A reflected property control should be disabled before it gets and object");
            _reflectedPropertyMapper.BusinessObject = _shape;
            Assert.IsFalse(_txtReflectedProperty.Enabled,
                           "A reflected property control should be disabled once it has an object");
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
}