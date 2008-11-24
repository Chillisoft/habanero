using System.Collections.Generic;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestPanelInfo 
    {
        private IControlFactory _controlFactory = new ControlFactoryVWG();
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [Test]
        public void TestPanel()
        {
            //---------------Set up test pack-------------------
            IPanelInfo panelInfo = new PanelInfo();
            IPanel panel = _controlFactory.CreatePanel();
            //---------------Assert Precondition----------------
            Assert.IsNull(panelInfo.Panel);
            //---------------Execute Test ----------------------
            panelInfo.Panel = panel;
            //---------------Test Result -----------------------
            Assert.AreSame(panel, panelInfo.Panel);

        }

        [Test]
        public void TestFieldInfos()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = new PanelInfo();
            //---------------Test Result -----------------------
            Assert.IsNotNull(panelInfo.FieldInfos);
            Assert.AreEqual(0, panelInfo.FieldInfos.Count);
        }

        [Test, ExpectedException(typeof(InvalidPropertyNameException))]
        public void TestFieldInfos_WrongPropertyNameGivesUsefulError()
        {
            //---------------Set up test pack-------------------
            IPanelInfo panelInfo = new PanelInfo();

            //---------------Execute Test ----------------------
            PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos["invalidPropName"];
        }

        [Test]
        public void TestFieldInfo_Constructor()
        {
            //---------------Set up test pack-------------------
            ILabel label = _controlFactory.CreateLabel();
            string propertyName = TestUtil.CreateRandomString();
            ITextBox tb = _controlFactory.CreateTextBox();
            IControlMapper controlMapper = new TextBoxMapper(tb, propertyName, false, _controlFactory);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PanelInfo.FieldInfo fieldInfo = new PanelInfo.FieldInfo(propertyName, label, controlMapper);

            //---------------Test Result -----------------------

            Assert.AreEqual(propertyName, fieldInfo.PropertyName);
            Assert.AreSame(label, fieldInfo.Label);
            Assert.AreSame(controlMapper, fieldInfo.ControlMapper);
            Assert.AreSame(tb, fieldInfo.InputControl);
        }

        [Test]
        public void TestSetBusinessObjectUpdatesControlMappers()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            IPanelInfo panelInfo = new PanelInfo();
            panelInfo.FieldInfos.Add(CreateFieldInfo("SampleText"));
            panelInfo.FieldInfos.Add(CreateFieldInfo("SampleInt"));
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Sample sampleBO = new Sample();
            panelInfo.BusinessObject = sampleBO;
            //---------------Test Result -----------------------
            Assert.AreSame(sampleBO, panelInfo.BusinessObject);
            Assert.AreSame(sampleBO, panelInfo.FieldInfos[0].ControlMapper.BusinessObject);
            Assert.AreSame(sampleBO, panelInfo.FieldInfos[1].ControlMapper.BusinessObject);
        }

        [Test]
        public void TestApplyChangesToBusinessObject()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            Sample sampleBO = new Sample();
            const string startText = "startText";
            const string endText = "endText";
            sampleBO.SampleText = startText;
            sampleBO.SampleInt = 1;

            IPanelInfo panelInfo = new PanelInfo();
            PanelInfo.FieldInfo sampleTextFieldInfo = CreateFieldInfo("SampleText");
            PanelInfo.FieldInfo sampleIntFieldInfo = CreateFieldInfo("SampleInt");
            panelInfo.FieldInfos.Add(sampleTextFieldInfo);
            panelInfo.FieldInfos.Add(sampleIntFieldInfo);
            panelInfo.BusinessObject = sampleBO;

            sampleTextFieldInfo.InputControl.Text = endText;
            //---------------Assert Precondition----------------
            Assert.AreEqual(startText, sampleBO.SampleText);
            //---------------Execute Test ----------------------
            panelInfo.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(endText, sampleBO.SampleText);
            Assert.AreEqual(1, sampleBO.SampleInt);
        }

        [Test]
        public void TestControlsEnabled()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            PanelBuilder panelBuilder = new PanelBuilder(_controlFactory);
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(classDef.UIDefCol["default"].UIForm[0]);
            panelInfo.BusinessObject = new Sample();
            //---------------Assert Precondition----------------
            Assert.IsTrue(panelInfo.FieldInfos[0].InputControl.Enabled);
            Assert.IsFalse(panelInfo.FieldInfos[1].InputControl.Enabled);
            //---------------Execute Test ----------------------
            panelInfo.ControlsEnabled = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(panelInfo.FieldInfos[0].InputControl.Enabled);
            Assert.IsFalse(panelInfo.FieldInfos[1].InputControl.Enabled);

        }

        [Test]
        public void TestPanelInfos()
        {
            //---------------Execute Test ----------------------
            IPanelInfo panelInfo = new PanelInfo();
            //---------------Test Result -----------------------
            Assert.IsNotNull(panelInfo.PanelInfos);
            Assert.AreEqual(0, panelInfo.PanelInfos.Count);

        }

        [Test]
        public void TestClearErrorProviders()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneCompulsory();
            PanelBuilder panelBuilder = new PanelBuilder(_controlFactory);
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab(classDef.UIDefCol["default"].UIForm[0]);
            Sample businessObject = new Sample();
            panelInfo.BusinessObject = businessObject;

            //businessObject.SetPropertyValue("SampleText2", "sdlkfj");
            PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos["SampleText2"];
            panelInfo.ApplyChangesToBusinessObject();
            IErrorProvider errorProvider = fieldInfo.ControlMapper.ErrorProvider;

            //---------------Assert Precondition----------------
            Assert.IsTrue(errorProvider.GetError(fieldInfo.InputControl).Length > 0);
            //---------------Execute Test ----------------------
            panelInfo.ClearErrorProviders();
            //---------------Test Result -----------------------
            Assert.IsFalse(errorProvider.GetError(fieldInfo.InputControl).Length > 0);
        }

        private PanelInfo.FieldInfo CreateFieldInfo(string propertyName)
        {
            ILabel label = _controlFactory.CreateLabel();
            ITextBox tb = _controlFactory.CreateTextBox();
            IControlMapper controlMapper = new TextBoxMapper(tb, propertyName, false, _controlFactory);
            IErrorProvider errorProvider = _controlFactory.CreateErrorProvider();
            return  new PanelInfo.FieldInfo(propertyName, label, controlMapper);
        }

        private PanelInfo.FieldInfo CreateFieldInfo()
        {
            return CreateFieldInfo(TestUtil.CreateRandomString());
        }
    }
    
}