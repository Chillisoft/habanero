using Gizmox.WebGUI.Forms;
using Habanero.BO.ClassDefinition;
using Habanero.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.WebGUI
{
    [TestFixture]
    public class TestCheckBoxMapper
    {
        private const bool START_VALUE = false;
        private CheckBox _cb = new CheckBox();
        private MyBO _bo;
        private CheckBoxMapper _mapper;
        private string _propName = "TestBoolean";

        [SetUp]
        public void TestSetup()
        {
            _bo = new MyBO();
            _bo.SetPropertyValue(_propName, START_VALUE);

  
            _mapper = new CheckBoxMapper(_cb, _propName, false);
            _mapper.BusinessObject = _bo;
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();


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
            Assert.AreEqual(_bo.GetPropertyValue(_propName), _cb.Checked);
        }

        [Test]
        public void TestCheckBoxValueNotUpdated()
        {
            _bo.SetPropertyValue(_propName, true);
                
            Assert.AreEqual(START_VALUE, _cb.Checked);
            Assert.AreNotEqual(_bo.TestProp, _cb.Checked);
        }

        [Test]
        public void TestValueUpdated()
        {
            _bo.SetPropertyValue(_propName, true);
            _mapper.ValueUpdated();
            Assert.AreEqual(true, _cb.Checked);
            Assert.AreEqual(_bo.GetPropertyValue(_propName), _cb.Checked);
        }
        [Test]
        public void Test_BONotUpdatedIfNotApplyChanges()
        {
            Assert.AreEqual(false, _bo.GetPropertyValue(_propName));
            _cb.Checked = true;
            Assert.AreNotEqual(true, _bo.GetPropertyValue(_propName));
        }
        [Test]
        public void TestApplyChangesToBusinessObject()
        {
            _cb.Checked = true;
            Assert.AreNotEqual(true, _bo.GetPropertyValue(_propName));
            _mapper.ApplyChangesToBusinessObject();
            Assert.AreEqual(true, _bo.GetPropertyValue(_propName));
        }

    }
}
