using Gizmox.WebGUI.Forms;
using Habanero.BO.ClassDefinition;
using Habanero.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.WebGUI
{
    [TestFixture]
    public class TestTextBoxMapper
    {
        private const string START_VALUE = "TestStart";
        private TextBox _tb = new TextBox();
        private MyBO _bo;
        private TextBoxMapper _mapper;

        [SetUp]
        public void TestSetup()
        {
            _bo = new MyBO();
            _bo.TestProp = START_VALUE;

            string propName = "TestProp";
            _mapper = new TextBoxMapper(_tb, propName, false);
            _mapper.BusinessObject = _bo;
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
           

            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }
        [Test]
        public void TestSettingBusinessObject()
        {
            _mapper.BusinessObject = _bo;
            Assert.AreEqual(_bo.TestProp, _tb.Text);
        }

        [Test]
        public void TestTextBoxValueNotUpdated()
        {
            _bo.TestProp = "NextValue";
            Assert.AreEqual(START_VALUE, _tb.Text);
            Assert.AreNotEqual(_bo.TestProp, _tb.Text);
        }

        [Test]
        public void TestValueUpdated()
        {
            _bo.TestProp = "NextValue";
           _mapper.ValueUpdated();
            Assert.AreEqual("NextValue", _tb.Text);
            Assert.AreEqual(_bo.TestProp, _tb.Text);
        }

        [Test]
        public void TestApplyChangesToBusinessObject()
        {
            _tb.Text = "NewValue";
            Assert.AreNotEqual("NewValue", _bo.TestProp);
            _mapper.ApplyChangesToBusinessObject();
            Assert.AreEqual("NewValue", _bo.TestProp);
        }
    
    }
}
