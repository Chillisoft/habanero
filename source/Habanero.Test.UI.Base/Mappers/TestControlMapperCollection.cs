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

using Habanero.BO.ClassDefinition;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestControlMapperCollection 
    {
        protected abstract IControlFactory GetControlFactory();
        private const string START_VALUE_1 = "StartValue1";
        private const string START_VALUE_2 = "StartValue2";
        private const string TEST_PROP_1 = "TestProp";
        private const string TEST_PROP_2 = "TestProp2";
        private const string CHANGED_VALUE_1 = "ChangedValue1";
        private const string CHANGED_VALUE_2 = "ChangedValue2";
 
        
        [TestFixture]
        public class TestControlMapperCollectionWin : TestControlMapperCollection
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.Win.ControlFactoryWin();
            }

            [Test]
            public void TestChangeControlValues_ChangesBusinessObjectValues()
            {
                //---------------Set up test pack-------------------
                MyBO.LoadDefaultClassDef();
                MyBO myBO = new MyBO();
                myBO.TestProp = START_VALUE_1;
                myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

                IPanelFactory factory = new PanelFactory(myBO, GetControlFactory());
                IPanelFactoryInfo panelInfo = factory.CreatePanel();

                //---------------Execute Test ----------------------
                ChangeValuesInControls(panelInfo);
                panelInfo.ControlMappers[TEST_PROP_1].ApplyChangesToBusinessObject();
                panelInfo.ControlMappers[TEST_PROP_2].ApplyChangesToBusinessObject();
                //---------------Test Result -----------------------

                Assert.AreEqual(CHANGED_VALUE_1, myBO.GetPropertyValue(TEST_PROP_1));
                Assert.AreEqual(CHANGED_VALUE_2, myBO.GetPropertyValue(TEST_PROP_2));

            }
        }

        [TestFixture]
        public class TestControlMapperCollectionGiz : TestControlMapperCollection
        {
            protected override IControlFactory GetControlFactory()
            {
                return new Habanero.UI.WebGUI.ControlFactoryGizmox();
            }

            [Test]
            public void TestChangeControlValues_DoesNotChangeBusinessObjectValues()
            {
                //---------------Set up test pack-------------------
                MyBO.LoadDefaultClassDef();
                MyBO myBO = new MyBO();
                myBO.TestProp = START_VALUE_1;
                myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

                IPanelFactory factory = new PanelFactory(myBO, GetControlFactory());
                IPanelFactoryInfo panelInfo = factory.CreatePanel();

                //---------------Execute Test ----------------------
                ChangeValuesInControls(panelInfo);
                //---------------Test Result -----------------------

                Assert.AreEqual(START_VALUE_1, myBO.GetPropertyValue(TEST_PROP_1));
                Assert.AreEqual(START_VALUE_2, myBO.GetPropertyValue(TEST_PROP_2));

            }

        }

        [SetUp]
        public void TestSetup()
        {
            ClassDef.ClassDefs.Clear();
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
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.TestProp = START_VALUE_1;
            myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);
            IPanelFactory factory = new PanelFactory(myBO, GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelFactoryInfo panelInfo = factory.CreatePanel();

            //---------------Test Result -----------------------
            Assert.AreEqual(START_VALUE_1, panelInfo.ControlMappers[TEST_PROP_1].Control.Text);
            Assert.AreEqual(START_VALUE_2, panelInfo.ControlMappers[TEST_PROP_2].Control.Text);
        }

        
        private void ChangeValuesInControls(IPanelFactoryInfo panelInfo)
        {
            panelInfo.ControlMappers[TEST_PROP_1].Control.Text = CHANGED_VALUE_1;
            panelInfo.ControlMappers[TEST_PROP_2].Control.Text = CHANGED_VALUE_2;
        }

        [Test]
        public void TestApplyChangesToBusinessObject()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.TestProp = START_VALUE_1;
            myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

            IPanelFactory factory = new PanelFactory(myBO, GetControlFactory());
            IPanelFactoryInfo panelInfo = factory.CreatePanel();
            ChangeValuesInControls(panelInfo);

            //---------------Execute Test ----------------------

            panelInfo.ControlMappers.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------

            Assert.AreEqual(CHANGED_VALUE_1, myBO.GetPropertyValue(TEST_PROP_1));
            Assert.AreEqual(CHANGED_VALUE_2, myBO.GetPropertyValue(TEST_PROP_2));
        }

        [Test]
        public void TestDisableControls()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            IPanelFactory factory = new PanelFactory(myBO, GetControlFactory());
            IPanelFactoryInfo panelInfo = factory.CreatePanel();

            //---------------Assert precondition----------------
            Assert.IsTrue(panelInfo.ControlMappers[TEST_PROP_1].Control.Enabled);
        
            //---------------Execute Test ----------------------
            panelInfo.ControlMappers.ControlsEnabled = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(panelInfo.ControlMappers[TEST_PROP_1].Control.Enabled);
            Assert.IsFalse(panelInfo.ControlMappers[TEST_PROP_2].Control.Enabled);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestEnableControls()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            IPanelFactory factory = new PanelFactory(myBO, GetControlFactory());
            IPanelFactoryInfo panelInfo = factory.CreatePanel();
            panelInfo.ControlMappers.ControlsEnabled = false;
             
            //---------------Execute Test ----------------------
            panelInfo.ControlMappers.ControlsEnabled = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(panelInfo.ControlMappers[TEST_PROP_1].Control.Enabled);
            Assert.IsTrue(panelInfo.ControlMappers[TEST_PROP_2].Control.Enabled);
            //---------------Tear Down -------------------------
        }
    }
}
