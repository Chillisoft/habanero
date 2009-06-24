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

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestPanelInfo
    {
        private readonly IControlFactory _controlFactory = new ControlFactoryVWG();

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
#pragma warning disable 168
        [Test, ExpectedException(typeof (InvalidPropertyNameException))]
        public void TestFieldInfos_WrongPropertyNameGivesUsefulError()
        {
            //---------------Set up test pack-------------------
            IPanelInfo panelInfo = new PanelInfo();

            //---------------Execute Test ----------------------

            PanelInfo.FieldInfo fieldInfo = panelInfo.FieldInfos["invalidPropName"];
        }
#pragma warning restore 168

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

        [Test]
        public void TestFieldInfo_Constructor()
        {
            //---------------Set up test pack-------------------
            ILabel label = _controlFactory.CreateLabel();
            string propertyName = TestUtil.GetRandomString();
            ITextBox tb = _controlFactory.CreateTextBox();
            IControlMapper controlMapper = new TextBoxMapper(tb, propertyName, false, _controlFactory);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            PanelInfo.FieldInfo fieldInfo = new PanelInfo.FieldInfo(propertyName, label, controlMapper);

            //---------------Test Result -----------------------

            Assert.AreEqual(propertyName, fieldInfo.PropertyName);
            Assert.AreSame(label, fieldInfo.LabelControl);
            Assert.AreSame(controlMapper, fieldInfo.ControlMapper);
            Assert.AreSame(tb, fieldInfo.InputControl);
        }

        [Test]
        public void TestSetBusinessObjectUpdatesControlMappers()
        {
            //---------------Set up test pack-------------------
            Sample.CreateClassDefWithTwoPropsOneInteger();
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
            Sample.CreateClassDefWithTwoPropsOneInteger();
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
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) classDef.UIDefCol["default"].UIForm[0]);
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
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) classDef.UIDefCol["default"].UIForm[0]);
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

        private IControlFactory GetControlFactory()
        {
            return _controlFactory;
        }

        [Test]
        public void Test_UIFormTab()
        {
            //--------------- Set up test pack ------------------
            ClassDef classDef = Sample.CreateClassDefWithTwoPropsOneInteger();
            PanelBuilder panelBuilder = new PanelBuilder(_controlFactory);
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) classDef.UIDefCol["default"].UIForm[0]);

            //--------------- Test Result -----------------------
            Assert.IsNotNull(panelInfo.UIFormTab);
            Assert.AreEqual(panelInfo.UIFormTab.Name, panelInfo.PanelTabText);
        }

        private PanelInfo.FieldInfo CreateFieldInfo(string propertyName)
        {
            ILabel label = _controlFactory.CreateLabel();
            ITextBox tb = _controlFactory.CreateTextBox();
            IControlMapper controlMapper = new TextBoxMapper(tb, propertyName, false, _controlFactory);
            _controlFactory.CreateErrorProvider();
            return new PanelInfo.FieldInfo(propertyName, label, controlMapper);
        }

        [Test]
        public void Test_UpdateErrorProviderError_WhenBOInvalid_ShouldSetErrorMessage()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) person.ClassDef.UIDefCol["default"].UIForm[0]);
            person.Surname = TestUtil.GetRandomString();
            panelInfo.BusinessObject = person;
            IControlMapper SurnameControlMapper = panelInfo.FieldInfos["Surname"].ControlMapper;
            person.Surname = "";
            //---------------Assert Precondition----------------
            Assert.IsFalse(person.Status.IsValid());
            Assert.AreEqual("", SurnameControlMapper.GetErrorMessage());
            //---------------Execute Test ----------------------
            panelInfo.UpdateErrorProvidersErrorMessages();
            //---------------Test Result -----------------------
            Assert.AreNotEqual("", SurnameControlMapper.GetErrorMessage());
        }

        [Test]
        public void Test_UpdateErrorProviderError_WhenBOValid_ShouldClearErrorMessage()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson("", "");
            PanelBuilder panelBuilder = new PanelBuilder(GetControlFactory());
            IPanelInfo panelInfo = panelBuilder.BuildPanelForTab((UIFormTab) person.ClassDef.UIDefCol["default"].UIForm[0]);
            panelInfo.BusinessObject = person;
            IControlMapper SurnameControlMapper = panelInfo.FieldInfos["Surname"].ControlMapper;
            panelInfo.UpdateErrorProvidersErrorMessages();
            //---------------Assert Precondition----------------
            Assert.AreNotEqual("", SurnameControlMapper.GetErrorMessage());
            //---------------Execute Test ----------------------
            person.Surname = "SomeValue";
            panelInfo.UpdateErrorProvidersErrorMessages();
            //---------------Test Result -----------------------
            Assert.AreEqual("", SurnameControlMapper.GetErrorMessage());
        }

    }
}