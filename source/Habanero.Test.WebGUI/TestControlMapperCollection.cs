using System;
using System.Collections.Generic;
using System.Text;
using Habanero.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.WebGUI
{
    [TestFixture]
    public class TestControlMapperCollection 
    {
        private const string START_VALUE_1 = "StartValue1";
        private const string START_VALUE_2 = "StartValue2";
        private const string TEST_PROP_1 = "TestProp";
        private const string TEST_PROP_2 = "TestProp2";
        private const string CHANGED_VALUE_1 = "ChangedValue1";
        private const string CHANGED_VALUE_2 = "ChangedValue2";
        private MyBO _mybo;
        private PanelFactoryInfo _panelInfo;

        [SetUp]
        public void TestSetup()
        {
            // set up a business object with some values and a ui def
            MyBO.LoadDefaultClassDefGizmox();
            _mybo = new MyBO();
            _mybo.TestProp = START_VALUE_1;
            _mybo.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

            //set up a panel from a panel factory using the bo
            PanelFactory factory = new PanelFactory(_mybo);
            _panelInfo = factory.CreatePanel();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }
        [Test]
        public void TestPanelSetup()
        {
            // check the values in the controls
            Assert.AreEqual(START_VALUE_1, _panelInfo.ControlMappers[TEST_PROP_1].Control.Text);
            Assert.AreEqual(START_VALUE_2, _panelInfo.ControlMappers[TEST_PROP_2].Control.Text);
        }

        [Test]
        public void TestChangeControlValues()
        {
            // change values in the controls
            ChangeValuesInControls();

            // check the values on the bo have not changed
            Assert.AreEqual(START_VALUE_1, _mybo.GetPropertyValue(TEST_PROP_1));
            Assert.AreEqual(START_VALUE_2, _mybo.GetPropertyValue(TEST_PROP_2));
           
            
        }

        private void ChangeValuesInControls()
        {
            _panelInfo.ControlMappers[TEST_PROP_1].Control.Text = CHANGED_VALUE_1;
            _panelInfo.ControlMappers[TEST_PROP_2].Control.Text = CHANGED_VALUE_2;
        }

        [Test]
        public void TestApplyChangesToBusinessObject()
        {
            ChangeValuesInControls();
            
            // call ApplyChangesToBusinessObject
            _panelInfo.ControlMappers.ApplyChangesToBusinessObject();
            // check the valueson the bo have changed

            Assert.AreEqual(CHANGED_VALUE_1, _mybo.GetPropertyValue(TEST_PROP_1));
            Assert.AreEqual(CHANGED_VALUE_2, _mybo.GetPropertyValue(TEST_PROP_2));
        }
    }
}
