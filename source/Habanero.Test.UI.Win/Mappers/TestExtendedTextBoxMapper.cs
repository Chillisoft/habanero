using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    [TestFixture]
    public class TestExtendedTextBoxMapper
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
            ControlFactoryWin factory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        [Test]
        public void Test_Construct()
        {
            //--------------- Set up test pack ------------------
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxWin extendedTextBoxWin = new ExtendedTextBoxWin(controlFactory);
            string propName = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxWin, propName, false, controlFactory);
            //--------------- Test Result -----------------------
            Assert.IsInstanceOf(typeof(IControlMapper), mapper);
            Assert.AreSame(extendedTextBoxWin, mapper.Control);
            Assert.AreEqual(propName, mapper.PropertyName);
            Assert.AreEqual(false, mapper.IsReadOnly);
            Assert.AreEqual(controlFactory, mapper.ControlFactory);
            ExtendedTextBoxMapper lookupComboBoxMapper = mapper;
            Assert.IsNotNull(lookupComboBoxMapper);
            Assert.AreSame(extendedTextBoxWin, lookupComboBoxMapper.Control);
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
            ExtendedTextBoxWin extendedTextBoxWin = new ExtendedTextBoxWin(controlFactory);
            string propName = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxWin, propName, true, controlFactory);
            //--------------- Test Result -----------------------
            Assert.IsInstanceOf(typeof(IControlMapper), mapper);
            Assert.AreEqual(true, mapper.IsReadOnly);
            ExtendedTextBoxMapper lookupComboBoxMapper = mapper;
            Assert.IsNotNull(lookupComboBoxMapper);
            Assert.AreEqual(true, lookupComboBoxMapper.IsReadOnly);
        }

        [Test]
        public void Test_SetBusinessObject()
        {
            //--------------- Set up test pack ------------------
            ExtendedTextBoxMapper mapper = CreateExtendedLookupComboBoxMapper("TestProp");
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
            ExtendedTextBoxWin extendedTextBoxWin = new ExtendedTextBoxWin(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxWin, propName, true, controlFactory);
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
            ExtendedTextBoxMapper mapper = CreateExtendedLookupComboBoxMapper("Surname");
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
            ExtendedTextBoxMapper mapper = CreateExtendedLookupComboBoxMapper("Surname");
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.BusinessObject);
            Assert.IsNull(mapper.BusinessObject);
            //--------------- Execute Test ----------------------
            mapper.BusinessObject = null;
            //--------------- Test Result -----------------------
            Assert.AreSame(null, mapper.BusinessObject);
        }

        private static ExtendedTextBoxMapper CreateExtendedLookupComboBoxMapper(string propertyName)
        {
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxWin extendedTextBoxWin = new ExtendedTextBoxWin(controlFactory);
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxWin, propertyName, true, controlFactory);
            return mapper;
        }

        [Test]
        public void Test_ShowGridAndBOEditorControlWinOnClick()
        {
            //--------------- Set up test pack ------------------
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOs = CreateSavedOrganisationTestBOsCollection();
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxWin extendedTextBoxWin = new ExtendedTextBoxWin(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxWin, propName, true, controlFactory);
            mapper.BusinessObject = new ContactPersonTestBO();
            // mapper.RelatedBusinessObject = OrganisationTestBO.CreateSavedOrganisation();
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.PopupForm);
            //--------------- Execute Test ----------------------
            //extendedTextBoxWin.Button.PerformClick();
            mapper.ShowPopupForm();
            //--------------- Test Result -----------------------
            Assert.IsNotNull(mapper.PopupForm);
            IFormHabanero form = mapper.PopupForm;
            Assert.AreEqual(800, form.Width);
            Assert.AreEqual(600, form.Height);
            Assert.AreEqual(1, form.Controls.Count);
            Assert.AreEqual(DockStyle.Fill, form.Controls[0].Dock);

            Assert.IsInstanceOf(typeof(IBOGridAndEditorControl), form.Controls[0]);
            Assert.IsInstanceOf(typeof(BOGridAndEditorControlWin), form.Controls[0]);
            BOGridAndEditorControlWin andBOGridAndEditorControlWin = (BOGridAndEditorControlWin)form.Controls[0];
            //Assert.AreSame(mapper.BusinessObject, BOGridAndEditorControlWin.BOEditorControlWin.BusinessObject);
            Assert.IsTrue(andBOGridAndEditorControlWin.GridControl.IsInitialised);
            IBusinessObjectCollection collection = andBOGridAndEditorControlWin.GridControl.Grid.BusinessObjectCollection;
            Assert.IsNotNull(collection);
            Assert.AreEqual(organisationTestBOs.Count, collection.Count);
            Assert.AreEqual(organisationTestBOs.Count, mapper.LookupList.Count);
        }

        [Test]
        public void Test_ShowGridAndBOEditorControlWinWithSuperClassDef()
        {
            //--------------- Set up test pack ------------------
            ClassDef.ClassDefs.Clear();
            PersonTestBO.LoadDefaultClassDefWithTestOrganisationBOLookup();
            ContactPersonTestBO.LoadDefaultClassDefWithPersonTestBOSuperClass();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOs = CreateSavedOrganisationTestBOsCollection();
            
            
            IControlFactory controlFactory = GetControlFactory();
            ExtendedTextBoxWin extendedTextBoxWin = new ExtendedTextBoxWin(controlFactory);
            const string propName = "OrganisationID";
            ExtendedTextBoxMapper mapper = new ExtendedTextBoxMapper(
                extendedTextBoxWin, propName, true, controlFactory);
            mapper.BusinessObject = new ContactPersonTestBO();
            // mapper.RelatedBusinessObject = OrganisationTestBO.CreateSavedOrganisation();
            //--------------- Test Preconditions ----------------
            Assert.IsNull(mapper.PopupForm);
            //--------------- Execute Test ----------------------
            //extendedTextBoxWin.Button.PerformClick();
            mapper.ShowPopupForm();
            //--------------- Test Result -----------------------
            Assert.IsNotNull(mapper.PopupForm);
            IFormHabanero form = mapper.PopupForm;
            Assert.AreEqual(800, form.Width);
            Assert.AreEqual(600, form.Height);
            Assert.AreEqual(1, form.Controls.Count);
            Assert.AreEqual(DockStyle.Fill, form.Controls[0].Dock);

            Assert.IsInstanceOf(typeof(IBOGridAndEditorControl), form.Controls[0]);
            Assert.IsInstanceOf(typeof(BOGridAndEditorControlWin), form.Controls[0]);
            BOGridAndEditorControlWin andBOGridAndEditorControlWin = (BOGridAndEditorControlWin)form.Controls[0];
            //Assert.AreSame(mapper.BusinessObject, BOGridAndEditorControlWin.BOEditorControlWin.BusinessObject);
            Assert.IsTrue(andBOGridAndEditorControlWin.GridControl.IsInitialised);
            IBusinessObjectCollection collection = andBOGridAndEditorControlWin.GridControl.Grid.BusinessObjectCollection;
            Assert.IsNotNull(collection);
            Assert.AreEqual(organisationTestBOs.Count, collection.Count);
            Assert.AreEqual(organisationTestBOs.Count, mapper.LookupList.Count);
        }


        private static BusinessObjectCollection<OrganisationTestBO> CreateSavedOrganisationTestBOsCollection()
        {
            OrganisationTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<OrganisationTestBO> organisationTestBOs = new BusinessObjectCollection<OrganisationTestBO>
                                                                                   {
                                                                                       OrganisationTestBO.CreateSavedOrganisation(),
                                                                                       OrganisationTestBO.CreateSavedOrganisation(),
                                                                                       OrganisationTestBO.CreateSavedOrganisation(),
                                                                                       OrganisationTestBO.CreateSavedOrganisation()
                                                                                   };
            return organisationTestBOs;
        }
    }
}