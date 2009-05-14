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
                Habanero.UI.Win.ControlFactoryWin factory = new Habanero.UI.Win.ControlFactoryWin();
                GlobalUIRegistry.ControlFactory = factory;
                return factory;
            }

            [Test]
            public void TestChangeControlValues_ChangesBusinessObjectValues()
            {
                //---------------Set up test pack-------------------
                MyBO.LoadDefaultClassDef();
                MyBO myBO = new MyBO();
                myBO.TestProp = START_VALUE_1;
                myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

                PanelBuilder factory = new PanelBuilder(GetControlFactory());
                IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
                panelInfo.BusinessObject = myBO;
                //---------------Execute Test ----------------------
                ChangeValuesInControls(panelInfo);
                panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.ApplyChangesToBusinessObject();
                panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.ApplyChangesToBusinessObject();
                //---------------Test Result -----------------------

                Assert.AreEqual(CHANGED_VALUE_1, myBO.GetPropertyValue(TEST_PROP_1));
                Assert.AreEqual(CHANGED_VALUE_2, myBO.GetPropertyValue(TEST_PROP_2));

            }
        }

        [TestFixture]
        public class TestControlMapperCollectionVWG : TestControlMapperCollection
        {
            protected override IControlFactory GetControlFactory()
            {
                Habanero.UI.VWG.ControlFactoryVWG factory = new  Habanero.UI.VWG.ControlFactoryVWG();
                GlobalUIRegistry.ControlFactory = factory;
                return factory;
            }

            [Test]
            public void TestChangeControlValues_DoesNotChangeBusinessObjectValues()
            {
                //---------------Set up test pack-------------------
                MyBO.LoadDefaultClassDef();
                MyBO myBO = new MyBO();
                myBO.TestProp = START_VALUE_1;
                myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

                PanelBuilder factory = new PanelBuilder(GetControlFactory());
                IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
                panelInfo.BusinessObject = myBO;
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
            PanelBuilder factory = new PanelBuilder(GetControlFactory());

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(START_VALUE_1, panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Text);
            Assert.AreEqual(START_VALUE_2, panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.Control.Text);
        }

        
        private static void ChangeValuesInControls(IPanelInfo panelInfo)
        {
            panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Text = CHANGED_VALUE_1;
            panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.Control.Text = CHANGED_VALUE_2;
        }

        [Test]
        public void TestApplyChangesToBusinessObject()
        {
            //---------------Set up test pack-------------------

            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            myBO.TestProp = START_VALUE_1;
            myBO.SetPropertyValue(TEST_PROP_2, START_VALUE_2);

            PanelBuilder factory = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            ChangeValuesInControls(panelInfo);

            //---------------Execute Test ----------------------

            panelInfo.ApplyChangesToBusinessObject();
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
            PanelBuilder factory = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            //---------------Assert precondition----------------
            Assert.IsTrue(panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Enabled);
        
            //---------------Execute Test ----------------------
            panelInfo.ControlsEnabled = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Enabled);
            Assert.IsFalse(panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.Control.Enabled);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestEnableControls()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO myBO = new MyBO();
            PanelBuilder factory = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = factory.BuildPanelForForm(myBO.ClassDef.UIDefCol["default"].UIForm);
            panelInfo.BusinessObject = myBO;
            panelInfo.ControlsEnabled = false;
             
            //---------------Execute Test ----------------------
            panelInfo.ControlsEnabled = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(panelInfo.FieldInfos[TEST_PROP_1].ControlMapper.Control.Enabled);
            Assert.IsTrue(panelInfo.FieldInfos[TEST_PROP_2].ControlMapper.Control.Enabled);
            //---------------Tear Down -------------------------
        }
    }
}
