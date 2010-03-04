using System;
using Habanero.BO;
using Habanero.Test.BO;
using Habanero.Test.UI.Base.Mappers;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    [TestFixture]
    public class TestRelationshipComboBoxMapperWin : TestRelationshipComboBoxMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            if (_controlFactory == null) CreateControlFactory();
            return _controlFactory;
        }

        protected override void CreateControlFactory()
        {
            _controlFactory = new ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = _controlFactory;
        }

        [Test]
        public void Test_WhenSetInvalidPropertyValue_ShouldUpdateItemInComboToBlank()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            string origToString = organisationTestBO.ToString();
            Guid newToString = Guid.NewGuid();
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Assert precondition----------------
            Assert.AreEqual(organisationTestBO.ToString(), origToString);
            Assert.AreEqual(origToString.ToString(), cmbox.Text);
            //---------------Execute Test ----------------------
            person.OrganisationID = newToString;
            //---------------Test Result -----------------------
            Assert.AreNotEqual(origToString, newToString);
            Assert.AreEqual("", cmbox.Text);
        }


        [Test]
        public void Test_WhenChangePropValue_ShouldUpdateControlValue_WithoutCallingUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Assert precondition----------------
            Assert.AreSame(organisationTestBO, person.Organisation);
            Assert.AreSame(organisationTestBO, cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            person.Organisation = newBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation);
            Assert.AreSame(newBO, cmbox.SelectedItem, "Value is not set after changing bo relationship");
        }

        [Test]
        public void Test_MustDeregisterForEvents_WhenSetBOToNull_ThenChangePropValue_ShouldNotUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Assert precondition----------------
            Assert.AreSame(organisationTestBO, person.Organisation);
            Assert.AreSame(organisationTestBO, cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            person.Organisation = newBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation);
            Assert.AreSame(null, cmbox.SelectedItem, "Value is not set after changing bo relationship");
        }

        [Test]
        public void Test_MustDeregisterForEvents_WhenSetBOToAnotherBO_ThenChangePropValue_ShouldNotUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            ContactPersonTestBO newCP = new ContactPersonTestBO();
            //---------------Assert precondition----------------
            Assert.AreSame(organisationTestBO, person.Organisation);
            Assert.AreSame(organisationTestBO, cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = newCP;
            person.Organisation = newBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation);
            Assert.AreSame(null, newCP.Organisation);
            Assert.AreSame(null, cmbox.SelectedItem, "Value is not set after changing bo relationship");
        }

        //TODO Brett 24 Mar 2009: Test Changing BO's removes event handlers.

        [Test]
        public override void Test_ChangeComboBoxSelected_ShouldNotUpdatePropValue_VWGOnly()
        {
            //For Windows the value should be changed.
            Assert.IsTrue
                (true,
                 "For Windows the value should be changed. See TestChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges");
        }

        [Test]
        public void Test_ChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newBO;
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation, "For Windows the value should be changed.");
        }

        [Test]
        public void Test_KeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.Text = newBO.ToString();
            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation, "For Windows the value should be changed.");
        }

        [Test]
        public void Test_KeyPressStrategy_UpdatesBusinessObject_WhenEnterKeyPressed()
        {
            //---------------Set up test pack-------------------
            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            cmbox.Text = newBO.ToString();
            //---------------Assert Precondition----------------
            Assert.AreNotSame(newBO, person.Organisation, "For Windows the value should be changed.");
            //---------------Execute Test ----------------------
            cmbox.CallSendKeyBob();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreSame(newBO, person.Organisation, "For Windows the value should be changed.");
        }


        [Test]
        public void Test_KeyPressStrategy_DoesNotUpdateBusinessObject_SelectedIndexChanged()
        {
            //---------------Set up test pack-------------------

            ComboBoxWinStub cmbox = new ComboBoxWinStub();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            mapper.MapperStrategy = GetControlFactory().CreateLookupKeyPressMapperStrategy();
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newBO;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(ComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreNotSame(newBO, person.Organisation, "For Windows the value should be changed.");
            Assert.AreSame(organisationTestBO, person.Organisation, "For Windows the value should be changed.");
        }

        private class ComboBoxWinStub : ComboBoxWin
        {
            public void CallSendKeyBob()
            {
                this.OnKeyPress(new System.Windows.Forms.KeyPressEventArgs((char)13));
            }
        }
    }
}