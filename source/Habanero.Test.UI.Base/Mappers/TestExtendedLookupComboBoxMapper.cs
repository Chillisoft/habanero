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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    [TestFixture]
    public class TestExtendedLookupComboBoxMapper
    {
        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //   ContactPersonTestBO.CreateSampleData();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
        }

        private static IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void Test_Construct()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            string propName = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ExtendedComboBoxMapper mapper = new ExtendedComboBoxMapper(
                extendedComboBox, propName, false, controlFactory);
            //--------------- Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IControlMapper), mapper);
            Assert.AreSame(extendedComboBox, mapper.Control);
            Assert.AreEqual(propName, mapper.PropertyName);
            Assert.AreEqual(false, mapper.IsReadOnly);
            Assert.AreEqual(controlFactory, mapper.ControlFactory);
            ExtendedComboBoxMapper lookupComboBoxMapper = mapper;
            Assert.IsNotNull(lookupComboBoxMapper);
//            Assert.AreSame(extendedComboBox.ComboBox, lookupComboBoxMapper.Control);
            Assert.AreSame(extendedComboBox, lookupComboBoxMapper.Control);
            Assert.AreEqual(propName, lookupComboBoxMapper.PropertyName);
            Assert.AreEqual(false, lookupComboBoxMapper.IsReadOnly);
            Assert.AreEqual(controlFactory, lookupComboBoxMapper.ControlFactory);
            Assert.AreEqual(lookupComboBoxMapper.ErrorProvider, mapper.ErrorProvider);
        }

        [Test]
        public void Test_Construct_ReadOnly()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            string propName = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ExtendedComboBoxMapper mapper = new ExtendedComboBoxMapper(
                extendedComboBox, propName, true, controlFactory);
            //--------------- Test Result -----------------------
            Assert.IsInstanceOfType(typeof(IControlMapper), mapper);
            Assert.AreEqual(true, mapper.IsReadOnly);
            ExtendedComboBoxMapper lookupComboBoxMapper = mapper;
            Assert.IsNotNull(lookupComboBoxMapper);
            Assert.AreEqual(true, lookupComboBoxMapper.IsReadOnly);
        }

        [Test]
        public void Test_SetBusinessObject()
        {
            //--------------- Set up test pack ------------------
            ExtendedComboBoxMapper mapper = CreateExtendedLookupComboBoxMapper("TestProp");
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsNull(mapper.BusinessObject);
            MyBO.LoadClassDefWithBOLookup();
            MyBO myBO = new MyBO();
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = myBO;
            //--------------- Test Result -----------------------
            Assert.AreSame(myBO, mapper.BusinessObject);
            Assert.AreSame(myBO, mapper.BusinessObject);
        }

        [Test]
        public void Test_ItemsShowingInComboBox()
        {
            //--------------- Set up test pack ------------------

            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = TestUtil.GetRandomString();
            contactPersonTestBO.FirstName = TestUtil.GetRandomString();
            OrganisationTestBO.LoadDefaultClassDef();
            OrganisationTestBO.CreateSavedOrganisation();
            OrganisationTestBO.CreateSavedOrganisation();

            IControlFactory controlFactory = GetControlFactory();
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            const string propName = "OrganisationID";
            ExtendedComboBoxMapper mapper = new ExtendedComboBoxMapper(
                extendedComboBox, propName, true, controlFactory);
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsNull(mapper.BusinessObject);
          
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = contactPersonTestBO;
            //--------------- Test Result -----------------------
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            Assert.AreSame(contactPersonTestBO, mapper.BusinessObject);
            Assert.AreEqual(2, mapper.LookupList.Count);
        }

        [Test]
        public void Test_SetBusinessObject_OnInternalLookupComboBoxMapper()
        {
            //--------------- Set up test pack ------------------
            ExtendedComboBoxMapper mapper = CreateExtendedLookupComboBoxMapper("Surname");
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsNull(mapper.BusinessObject);
            ContactPersonTestBO businessObjectInfo = new ContactPersonTestBO();
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = businessObjectInfo;
            //--------------- Test Result -----------------------
            Assert.AreSame(businessObjectInfo, mapper.BusinessObject);
        }
        [Test]
        public void Test_SetBusinessObject_ToNull_OnInternalLookupComboBoxMapper()
        {
            //--------------- Set up test pack ------------------
            ExtendedComboBoxMapper mapper = CreateExtendedLookupComboBoxMapper("Surname");
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsNull(mapper.BusinessObject);
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = null;
            //--------------- Test Result -----------------------
            Assert.AreSame(null, mapper.BusinessObject);
        }

        private static ExtendedComboBoxMapper CreateExtendedLookupComboBoxMapper(string propertyName)
        {
            IControlFactory controlFactory = GetControlFactory();
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            ExtendedComboBoxMapper mapper = new ExtendedComboBoxMapper(
                extendedComboBox, propertyName, true, controlFactory);
            return mapper;
        }

        //[Test, Ignore("The Mocks stuff not working")]
        //public void Test_ApplyChangesToBusinessObject()
        //{
        //    //--------------- Set up test pack ------------------
        //    IControlFactory controlFactory = GetControlFactory();
        //    ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
        //    string propName = TestUtil.GetRandomString();
        //    ExtendedComboBoxMapper<IBusinessObject> mapper = new ExtendedComboBoxMapperStub<IBusinessObject>(
        //        extendedComboBox, propName, true, controlFactory);
        //    MockRepository mock = new MockRepository();
        //    LookupComboBoxMapper lookupComboBoxMapperMock = mock.DynamicMock<LookupComboBoxMapper>(extendedComboBox.ComboBox, propName, true, controlFactory);
        //    //Expect.Call(lookupComboBoxMapperMock.ApplyChangesToBusinessObject()).Repeat.Once();
        //    mapper = lookupComboBoxMapperMock;
        //    mock.ReplayAll();
        //    //--------------- Test Preconditions ----------------

        //    //--------------- Execute Test ----------------------
        //    mapper.ApplyChangesToBusinessObject();
        //    //--------------- Test Result -----------------------
        //    mock.VerifyAll();
        //}

        //[Test]
        //public void Test_UpdateControlValueFromBusinessObject()
        //{
        //    //--------------- Set up test pack ------------------
        //    IControlFactory controlFactory = GetControlFactory();
        //    ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
        //    string propName = TestUtil.GetRandomString();
        //    ExtendedComboBoxMapper mapper = new ExtendedComboBoxMapper(
        //        extendedComboBox, propName, true, controlFactory);
        //    MockRepository mock = new MockRepository();
        //    LookupComboBoxMapper lookupComboBoxMapperMock = mock.DynamicMock<LookupComboBoxMapper>(extendedComboBox.ComboBox, propName, true, controlFactory);
        //    Expect.Call(delegate { lookupComboBoxMapperMock.UpdateControlValueFromBusinessObject(); }).Repeat.Once();
        //    mapper.LookupComboBoxMapper = lookupComboBoxMapperMock;
        //    mock.ReplayAll();
        //    //--------------- Test Preconditions ----------------

        //    //--------------- Execute Test ----------------------
        //    mapper.UpdateControlValueFromBusinessObject();
        //    //--------------- Test Result -----------------------
        //    mock.VerifyAll();
        //}

        [Test]
        public void Test_ShowGridAndBOEditorControlWinOnClick()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection();
            IControlFactory controlFactory = GetControlFactory();
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            const string propName = "OrganisationID";
            ExtendedComboBoxMapper mapper = new ExtendedComboBoxMapper(
                extendedComboBox, propName, true, controlFactory);
            mapper.BusinessObject = new ContactPersonTestBO();
           // mapper.RelatedBusinessObject = OrganisationTestBO.CreateSavedOrganisation();
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.PopupForm);
            //--------------- Execute Test ----------------------
            //extendedComboBox.Button.PerformClick();
            mapper.ShowPopupForm();
            //--------------- Test Result -----------------------
            Assert.IsNotNull(mapper.PopupForm);
            IFormHabanero form = mapper.PopupForm;
            Assert.AreEqual(800, form.Width);
            Assert.AreEqual(600, form.Height);
            Assert.AreEqual(1, form.Controls.Count);
            Assert.AreEqual(DockStyle.Fill, form.Controls[0].Dock);

            Assert.IsInstanceOfType(typeof(IBOSelectorAndEditor), form.Controls[0]);
            Assert.IsInstanceOfType(typeof(BOSelectorAndBOEditorControlWin), form.Controls[0]);
            BOSelectorAndBOEditorControlWin boSelectorAndBOEditorControlWin = (BOSelectorAndBOEditorControlWin)form.Controls[0];
            //Assert.AreSame(mapper.BusinessObject, BOSelectorAndBOEditorControlWin.BusinessObjectControlWin.BusinessObject);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.GridControl.IsInitialised);
            IBusinessObjectCollection collection = boSelectorAndBOEditorControlWin.GridControl.Grid.BusinessObjectCollection;
            Assert.IsNotNull(collection);
            Assert.AreEqual(organisationTestBOS.Count, collection.Count);
            Assert.AreEqual(organisationTestBOS.Count, mapper.LookupList.Count);
        }

        [Test]
        public void Test_ShowGridAndBOEditorControlWinWithSuperClassDef()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            PersonTestBO.LoadDefaultClassDefWithTestOrganisationBOLookup();
            ContactPersonTestBO.LoadDefaultClassDefWithPersonTestBOSuperClass();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = CreateSavedOrganisationTestBOSCollection();
            
            
            IControlFactory controlFactory = GetControlFactory();
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            const string propName = "OrganisationID";
            ExtendedComboBoxMapper mapper = new ExtendedComboBoxMapper(
                extendedComboBox, propName, true, controlFactory);
            mapper.BusinessObject = new ContactPersonTestBO();
           // mapper.RelatedBusinessObject = OrganisationTestBO.CreateSavedOrganisation();
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.PopupForm);
            //--------------- Execute Test ----------------------
            //extendedComboBox.Button.PerformClick();
            mapper.ShowPopupForm();
            //--------------- Test Result -----------------------
            Assert.IsNotNull(mapper.PopupForm);
            IFormHabanero form = mapper.PopupForm;
            Assert.AreEqual(800, form.Width);
            Assert.AreEqual(600, form.Height);
            Assert.AreEqual(1, form.Controls.Count);
            Assert.AreEqual(DockStyle.Fill, form.Controls[0].Dock);

            Assert.IsInstanceOfType(typeof(IBOSelectorAndEditor), form.Controls[0]);
            Assert.IsInstanceOfType(typeof(BOSelectorAndBOEditorControlWin), form.Controls[0]);
            BOSelectorAndBOEditorControlWin boSelectorAndBOEditorControlWin = (BOSelectorAndBOEditorControlWin)form.Controls[0];
            //Assert.AreSame(mapper.BusinessObject, BOSelectorAndBOEditorControlWin.BusinessObjectControlWin.BusinessObject);
            Assert.IsTrue(boSelectorAndBOEditorControlWin.GridControl.IsInitialised);
            IBusinessObjectCollection collection = boSelectorAndBOEditorControlWin.GridControl.Grid.BusinessObjectCollection;
            Assert.IsNotNull(collection);
            Assert.AreEqual(organisationTestBOS.Count, collection.Count);
            Assert.AreEqual(organisationTestBOS.Count, mapper.LookupList.Count);
        }

        [Test]
        public void Test_ShowGridAndBOEditorControlWinWithSuperClassDef_DatabaseLookupList()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            OrganisationTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.LoadDefaultClassDefWithPersonTestBOSuperClass();
            PersonTestBO.LoadDefaultClassDefWithTestOrganisationBOLookup_DatabaseLookupList();

            IControlFactory controlFactory = GetControlFactory();
            ExtendedComboBoxWin extendedComboBox = new ExtendedComboBoxWin(controlFactory);
            const string propName = "OrganisationID";
            ExtendedComboBoxMapper mapper = new ExtendedComboBoxMapper(
                extendedComboBox, propName, true, controlFactory);
            DatabaseConfig databaseConfig = TestUtil.GetDatabaseConfig();
            DatabaseConnection.CurrentConnection = databaseConfig.GetDatabaseConnection();
            mapper.BusinessObject = new ContactPersonTestBO();
           // mapper.RelatedBusinessObject = OrganisationTestBO.CreateSavedOrganisation();
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.PopupForm);
            //--------------- Execute Test ----------------------
            //extendedComboBox.Button.PerformClick();
            mapper.ShowPopupForm();
            //--------------- Test Result -----------------------
            Assert.IsNotNull(mapper.PopupForm);
        }

        private static BusinessObjectCollection<OrganisationTestBO> CreateSavedOrganisationTestBOSCollection()
        {
            OrganisationTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOS = new BusinessObjectCollection<OrganisationTestBO>();
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            organisationTestBOS.Add(OrganisationTestBO.CreateSavedOrganisation());
            return organisationTestBOS;
        }
    }
}
