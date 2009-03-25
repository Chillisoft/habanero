using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Mappers
{
    [TestFixture]
    public class TestRelationshipComboBoxMapperVWG
    {
        protected IControlFactory _controlFactory;
        private ClassDef _cpClassDef;
        private ClassDef _orgClassDef;

        protected virtual IControlFactory GetControlFactory()
        {
            if (_controlFactory == null) CreateControlFactory();
            return _controlFactory;
        }

        protected virtual void CreateControlFactory()
        {
            _controlFactory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = _controlFactory;
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            CreateControlFactory();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            _cpClassDef = ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
            _orgClassDef = OrganisationTestBO.LoadDefaultClassDef();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        [Test]
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            //---------------Execute Test ----------------------
            const string relationshipName = "Organisation";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper
                (cmbox, relationshipName, false, GetControlFactory());
            //---------------Test Result -----------------------
            Assert.AreSame(cmbox, mapper.Control);
            Assert.AreSame(relationshipName, mapper.RelationshipName);
            Assert.AreEqual(false, mapper.IsReadOnly);
            Assert.AreSame(GetControlFactory(), mapper.ControlFactory);
        }

        [Test]
        public void Test_Constructor_ComboBoxNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            const string relationshipName = "RelationshipDoesNotExist";
            try
            {
                new RelationshipComboBoxMapper(null, relationshipName, false, GetControlFactory());
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("comboBox", ex.ParamName);
            }
        }

        [Test]
        public void Test_Constructor_ControlFactoryNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            //---------------Execute Test ----------------------
            const string relationshipName = "RelationshipDoesNotExist";
            try
            {
                new RelationshipComboBoxMapper(cmbox, relationshipName, false, null);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("controlFactory", ex.ParamName);
            }
        }

        [Test]
        public void Test_Constructor_relationshipNameNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            //---------------Execute Test ----------------------
            try
            {
                new RelationshipComboBoxMapper(cmbox, null, false, GetControlFactory());
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("relationshipName", ex.ParamName);
            }
        }

        [Test]
        public void Test_Constructor_WhenRelationshipDoesNotExistInClassDef_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string relationshipName = "RelationshipDoesNotExist";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
            //---------------Execute Test ----------------------
            
            try
            {
                mapper.BusinessObject = new ContactPersonTestBO();
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("The relationship '" + relationshipName + "' does not exist on the ClassDef '"
                     + _cpClassDef.ClassNameFull + "'", ex.Message);
            }
        }

        [Test]
        public void Test_Constructor_WhenRelationshipNotSingle_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string relationshipName = "ContactPeople";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
            //---------------Execute Test ----------------------
            try
            {
                mapper.BusinessObject = new OrganisationTestBO();
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("The relationship '" + relationshipName + "' for the ClassDef '" + _orgClassDef.ClassNameFull
                     +
                     "' is not a single relationship. The 'RelationshipComboBoxMapper' can only be used for single relationships",
                     ex.Message);
            }
        }

        [Test]
        public void Test_SetBOCollection_WhenEmpty_ShouldSetItemsInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string relationshipName = "Organisation";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper
                (cmbox, relationshipName, false, GetControlFactory());
            IBusinessObjectCollection boCol = new BusinessObjectCollection<OrganisationTestBO>();
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = boCol;
            //---------------Test Result -----------------------
            Assert.AreSame(boCol, mapper.BusinessObjectCollection);
            Assert.AreEqual(1, cmbox.Items.Count, "Should have blank Item");
        }

        [Test]
        public void Test_SetBOCollection_WhenOneItem_ShouldSetItemsInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            const string relationshipName = "Organisation";
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper
                (cmbox, relationshipName, false, GetControlFactory());
            IBusinessObjectCollection boCol = GetBoColWithOneItem();
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, boCol.Count);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = boCol;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, cmbox.Items.Count, "Should have one object and blank item");
            Assert.IsInstanceOfType(typeof (IBusinessObject), cmbox.Items[1]);
            Assert.IsTrue(cmbox.Items.Contains(boCol[0]));
            Assert.IsNull(cmbox.SelectedItem);
        }

        [Test]
        public void Test_ResetSetBOCollection_WhenHasOneItem_ShouldResetSetItemsInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, cmbox.Items.Count, "Should have one object and blank item");
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = new BusinessObjectCollection<OrganisationTestBO>();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cmbox.Items.Count, "Should have blank item");
            Assert.IsNull(cmbox.SelectedItem);
        }

        [Test]
        public void Test_SetBusinessObj_ShouldSelectRelatedItemInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(person.Organisation, cmbox.SelectedItem);
        }

        [Test]
        public void Test_SetBusinessObj_WithIncorrectType_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            mapper.ClassDef = _cpClassDef;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            try
            {
                mapper.BusinessObject = new OrganisationTestBO();
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("You cannot set the Business Object to the 'OrganisationTestBO' identified as '", ex.Message);
                StringAssert.Contains
                    ("since it is not of the appropriate type ('ContactPersonTestBO') for this '", ex.Message);
                StringAssert.Contains("RelationshipComboBoxMapper", ex.Message);
            }
        }

        [Test]
        public void Test_SetBusinessObjectCollection_WithIncorrectType_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            mapper.ClassDef = _cpClassDef;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            try
            {
                mapper.BusinessObjectCollection = new BusinessObjectCollection<ContactPersonTestBO>();
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("You cannot set the Business Object Collection to the 'ContactPersonTestBO'", ex.Message);
                StringAssert.Contains
                    ("since it is not of the appropriate type ('OrganisationTestBO') for this '", ex.Message);
                StringAssert.Contains("RelationshipComboBoxMapper", ex.Message);
            }
        }

        [Test]
        public void Test_SetBusinessObj_WhenCollectionHas2Items_ShouldSelectRelatedItemInComboBox()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreEqual(3, mapper.Control.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(person.Organisation, cmbox.SelectedItem);
        }

        [Test]
        public void
            Test_SetBusinessObj_WhenCollectionHas2ItemsAndRelatedBODoesNotExistInCollection_ShouldAddRelatedItemToComboBox
            ()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = new OrganisationTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreEqual(3, mapper.Control.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreEqual(4, mapper.Control.Items.Count);
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(person.Organisation, cmbox.SelectedItem);
        }

        [Test]
        public void Test_SetBusinessObj_WithNullRelatedObject_WhenItemAlreadySelected_ShouldSelectNoItemInList()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            boCol.CreateBusinessObject();
            ContactPersonTestBO origCP = new ContactPersonTestBO();
            origCP.Organisation = boCol[0];
            mapper.BusinessObject = origCP;

            ContactPersonTestBO person = new ContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.IsNull(person.Organisation);
            Assert.AreSame(boCol[0], cmbox.SelectedItem);
            Assert.AreNotEqual("", cmbox.Text);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.AreEqual(null, cmbox.SelectedItem);
            Assert.AreEqual("", cmbox.Text);
        }

        [Test]
        public void Test_GetRelatedObject_ShouldReturnBusinessObjectsRelatedObject()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsNotNull(person.Organisation);
            //---------------Execute Test ----------------------
            object businessObject = mapper.GetRelatedBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreSame(person.Organisation, businessObject);
        }

        [Test]
        public void Test_GetRelatedObject_BODoesNotHaveRelatedBO_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreSame(person, mapper.BusinessObject);
            Assert.IsNull(person.Organisation);
            //---------------Execute Test ----------------------
            object businessObject = mapper.GetRelatedBusinessObject();
            //---------------Test Result -----------------------
            Assert.IsNull(businessObject);
        }

        [Test]
        public void Test_GetRelatedObject_WhenNullBo_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(mapper.BusinessObject);
            //---------------Execute Test ----------------------
            object relatedBO = mapper.GetRelatedBusinessObject();
            //---------------Test Result -----------------------
            Assert.IsNull(relatedBO);
        }


        protected static ContactPersonTestBO CreateCPWithRelatedOrganisation(OrganisationTestBO organisationTestBO)
        {
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson();
            person.Organisation = organisationTestBO;
            return person;
        }

        [Test]
        public virtual void Test_AddBOToCol_UpdatesItems()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, mapper.Control.Items.Count);
            //---------------Execute Test ----------------------
            OrganisationTestBO newBO = new OrganisationTestBO();
            boCol.Add(newBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, boCol.Count);
            Assert.AreEqual(3, mapper.Control.Items.Count);
        }

        [Test]
        public virtual void Test_AddBOToCol_NullOrEmptyToStringRaisesError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            RelationshipComboBoxMapper mapper1 = GetMapper(cmbox);
            BusinessObjectCollection<ContactPersonTestBO> boCol = new BusinessObjectCollection<ContactPersonTestBO> { new ContactPersonTestBO() };
            mapper1.BusinessObjectCollection = boCol;
            RelationshipComboBoxMapper mapper = mapper1;
            ContactPersonTestBO newBO = new ContactPersonTestBO();
            newBO.Surname = "";
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, mapper.Control.Items.Count);
            Assert.IsNull(newBO.ToString());
            //---------------Execute Test ----------------------

            try
            {
                boCol.Add(newBO);
                Assert.Fail("expected Err");
            }
           //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                string message = string.Format("Cannot add a business object of type '{0}' "
                        + "to the 'ComboBoxCollectionSelector' if its ToString is null or zero length"
                        , newBO.ClassDef.ClassName);
                StringAssert.Contains(message, ex.Message);
            }
        }

        [Test]
        public virtual void Test_WhenSetNull_DoesNotRaiseError()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, mapper.Control.Items.Count);
            //---------------Execute Test ----------------------
            mapper.BusinessObjectCollection = null;
            //---------------Test Result -----------------------
            Assert.IsNull(mapper.BusinessObjectCollection);
            Assert.AreEqual(1, mapper.Control.Items.Count);
        }

        [Test]
        public virtual void Test_RemoveBOToCol_UpdatesItems()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO bo = boCol[0];
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(2, mapper.Control.Items.Count);

            //---------------Execute Test ----------------------
            boCol.Remove(bo);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, boCol.Count);
            Assert.AreEqual(1, mapper.Control.Items.Count);
        }

        [Test]
        public void TestSetBusinessObject_Null_DoesNotRaiseError_BUGFIX()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(cmbox.SelectedItem)));
        }

        [Test]
        public void TestSetBusinessObject_Null_SetBusinessObject_FillsList_BUGFIX()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            mapper.BusinessObject = null;
            //---------------Assert Precondition----------------
            Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(cmbox.SelectedItem)));
            Assert.AreEqual(2, cmbox.Items.Count);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, boCol.Count);
            Assert.AreEqual(1 + 1, cmbox.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
        }

        [Test]
        public void TestSetBusinessObject_Null_NullLookupListSet_DoesNotRaiseError_BUGFIX()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            mapper.BusinessObjectCollection = null;
            //---------------Assert Precondition----------------
            Assert.IsNull(mapper.BusinessObjectCollection);
            Assert.AreEqual(1, cmbox.Items.Count);
            Assert.IsNull(cmbox.SelectedItem);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = null;
            //---------------Test Result -----------------------
            Assert.IsNull(cmbox.SelectedItem);
            Assert.AreEqual(1, cmbox.Items.Count, "Should have only the null item in it.");
        }

        [Test]
        public void Test_ApplyChangesToBusObj_WhenAnItemIsSelectedAndRelatedBONull_ShouldUpdatesTheBusinessObjectWithTheSelectedValue
            ()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.IsNull(person.Organisation);
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newOrganisation;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreSame(newOrganisation, cmbox.SelectedItem);
            Assert.AreSame(newOrganisation, person.Organisation);
        }

        [Test]
        public void Test_ApplyChangesToBusObj_WhenAnItemIsSelected_ShouldUpdatesTheBusinessObjectWithTheSelectedValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO relatedBo = boCol[0];
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = relatedBo;
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.AreNotSame(newOrganisation, person.Organisation);
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newOrganisation;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreSame(newOrganisation, cmbox.SelectedItem);
            Assert.AreSame(newOrganisation, person.Organisation);
        }

        [Test]
        public void Test_ApplyChangesToBusObj_WhenNoItemIsSelected_ShouldUpdatesTheBusinessObjectWithNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO relatedBo = boCol[0];
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = relatedBo;
            mapper.BusinessObject = person;
            //---------------Assert Precondition----------------
            Assert.AreNotSame(newOrganisation, person.Organisation);
            //---------------Execute Test ----------------------
            cmbox.SelectedIndex = 0;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(null, cmbox.SelectedItem);
            Assert.IsNull(person.Organisation);
        }

        [Test]
        public void Test_LookupList_AddItemToComboBox_SelectAdditionalItem_SetsBOPropValueToNull()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = boCol[0];
            mapper.BusinessObject = person;
            cmbox.Items.Add("SomeItem");
            //---------------Assert Preconditions---------------
            Assert.AreEqual(3, cmbox.Items.Count);
            Assert.AreEqual("SomeItem", LastComboBoxItem(cmbox).ToString());
            //---------------Execute Test ----------------------
            cmbox.SelectedIndex = cmbox.Items.Count - 1;
            mapper.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.IsNull(person.Organisation);
        }

        [Test]
        public void Test_WhenChangePropValue_WithNullCurrentValue_ShouldUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = new ContactPersonTestBO();
            mapper.BusinessObject = person;
            //---------------Test Preconditions-------------------
            Assert.AreEqual(1, boCol.Count);
            Assert.IsNotNull(mapper.BusinessObjectCollection);
            Assert.IsNull(cmbox.SelectedItem, "There should be no selected item to start with");
            //---------------Execute Test ----------------------

            person.Organisation = organisationTestBO;
            mapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(organisationTestBO, cmbox.SelectedItem, "Value is not set after changing bo prop Value");
        }

        [Test]
        public void Test_WhenChangePropValue_ShouldUpdateControlValue()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO newBO = boCol.CreateBusinessObject();
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = organisationTestBO;
            mapper.BusinessObject = person;
            //---------------Test Preconditions-------------------
            Assert.AreEqual(2, boCol.Count);
            Assert.IsNotNull(mapper.BusinessObjectCollection);
            Assert.IsNotNull(cmbox.SelectedItem, "There should be a selected item to start with");
            //---------------Execute Test ----------------------

            person.Organisation = newBO;
            mapper.UpdateControlValueFromBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(cmbox.SelectedItem);
            Assert.AreEqual(newBO, cmbox.SelectedItem, "Value is not set after changing bo prop Value");
        }

        [Test]
        public virtual void TestChangeComboBoxDoesntUpdateBusinessObject()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = GetControlFactory().CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO relatedBo = boCol[0];
            OrganisationTestBO newOrganisation = boCol.CreateBusinessObject();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Organisation = relatedBo;
            mapper.BusinessObject = person;
            //---------------Execute Test ----------------------
            cmbox.SelectedItem = newOrganisation;
            //---------------Test Result -----------------------
            Assert.AreNotSame(newOrganisation, person.Organisation);
            Assert.AreSame(relatedBo, person.Organisation);
        }
        [Test]
        public void Test_ControlIsEditable_WhenComposition_AndBusinessObjectNew_True()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            IRelationship relationship = person.Relationships["Organisation"];
            relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;
            mapper.BusinessObject = person;
            //---------------Assert Preconditions---------------
            Assert.IsTrue(person.Status.IsNew);
            Assert.AreEqual(RelationshipType.Composition, relationship.RelationshipDef.RelationshipType);
            //---------------Execute Test ----------------------
            bool enabled = cmbox.Enabled;
            //---------------Test Result -----------------------
            Assert.IsTrue(enabled);
        }
        [Test]
        public void Test_ControlIsEditable_WhenComposition_AndBusinessObjectNotNew_False()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            IRelationship relationship = person.Relationships["Organisation"];
            relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;
            person.Surname = TestUtil.GetRandomString();
            person.Save();
            mapper.BusinessObject = person;
            //---------------Assert Preconditions---------------
            Assert.IsFalse(person.Status.IsNew);
            Assert.AreEqual(RelationshipType.Composition, relationship.RelationshipDef.RelationshipType);
            //---------------Execute Test ----------------------
            bool enabled = cmbox.Enabled;
            //---------------Test Result -----------------------
            Assert.IsFalse(enabled);
        }

        [Test]
        public void Test_ControlIsEditable_WhenComposition_AndBusinessObjectSaved_ShouldDisable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            BusinessObjectCollection<OrganisationTestBO> boCol;
            RelationshipComboBoxMapper mapper = GetMapperBoColHasOneItem(cmbox, out boCol);
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            IRelationship relationship = person.Relationships["Organisation"];
            relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;
            person.Surname = TestUtil.GetRandomString();

            mapper.BusinessObject = person;
            //---------------Assert Preconditions---------------
            Assert.IsTrue(person.Status.IsNew);
            Assert.AreEqual(RelationshipType.Composition, relationship.RelationshipDef.RelationshipType);
            person.Save();
            //---------------Execute Test ----------------------
            bool enabled = cmbox.Enabled;
            //---------------Test Result -----------------------
            Assert.IsFalse(person.Status.IsNew);
            Assert.IsFalse(enabled);
        }

        [Test]
        public void Test_WhenNotIsReadOnly_ControlShouldBeEditable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            const string relationshipName = "Organisation";
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            RelationshipComboBoxMapper mapper1 = new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
            //---------------Test Result -----------------------
            Assert.IsFalse(mapper1.IsReadOnly);
            Assert.IsTrue(mapper1.Control.Enabled);
        }
        [Test]
        public void Test_WhenIsReadOnly_ControlShouldNotBeEditable_()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            const string relationshipName = "Organisation";
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            RelationshipComboBoxMapper mapper1 = new RelationshipComboBoxMapper(cmbox,  relationshipName, true, GetControlFactory());
            //---------------Test Result -----------------------
            Assert.IsTrue(mapper1.IsReadOnly);
            Assert.IsFalse(mapper1.Control.Enabled);
        }

        [Test]
        public void Test_ControlIsEditable_WhenNotIsReadOnly_AndComposition_AndBusinessObjectSaved_ShouldDisable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            const string relationshipName = "Organisation";
            BusinessObjectCollection<OrganisationTestBO> boCol = GetBoColWithOneItem();
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
            mapper.BusinessObjectCollection = boCol;
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);
            IRelationship relationship = person.Relationships["Organisation"];
            relationship.RelationshipDef.RelationshipType = RelationshipType.Composition;
            person.Surname = TestUtil.GetRandomString();

            mapper.BusinessObject = person;
            //---------------Assert Preconditions---------------
            Assert.IsFalse(mapper.IsReadOnly);
            Assert.IsTrue(person.Status.IsNew);
            Assert.AreEqual(RelationshipType.Composition, relationship.RelationshipDef.RelationshipType);
            Assert.IsTrue(cmbox.Enabled);
            //---------------Execute Test ----------------------
            person.Save();
            bool enabled = cmbox.Enabled;
            //---------------Test Result -----------------------
            Assert.IsFalse(person.Status.IsNew);
            Assert.IsFalse(enabled);
        }
        [Test]
        public void Test_ControlIsEditable_WhenIsReadOnly_SetBusinessObject_ShouldNotMakeEditable()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            const string relationshipName = "Organisation";
            BusinessObjectCollection<OrganisationTestBO> boCol = GetBoColWithOneItem();
            RelationshipComboBoxMapper mapper = new RelationshipComboBoxMapper(cmbox, relationshipName, true, GetControlFactory());
            mapper.BusinessObjectCollection = boCol;
            OrganisationTestBO organisationTestBO = boCol[0];
            ContactPersonTestBO person = CreateCPWithRelatedOrganisation(organisationTestBO);

            //---------------Assert Preconditions---------------
            Assert.IsTrue(mapper.IsReadOnly);
            Assert.IsTrue(person.Status.IsNew);
            Assert.IsFalse(cmbox.Enabled);
            //---------------Execute Test ----------------------
            mapper.BusinessObject = person;
            bool enabled = cmbox.Enabled;
            //---------------Test Result -----------------------
            Assert.IsFalse(enabled);
        }

        [Test]
        public void Test_CreateControlMapper()
        {
            //---------------Set up test pack-------------------
            IComboBox cmbox = _controlFactory.CreateComboBox();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IControlMapper controlMapper = ControlMapper.Create
                ("RelationshipComboBoxMapper", "Habanero.UI.Base", cmbox, TestUtil.GetRandomString(), false,
                 GetControlFactory());
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(RelationshipComboBoxMapper), controlMapper);
        }


//        //This test is different from VWG because an edit to the properties in Windows updates the 
//        // control and must therefore update the Error provider whereas for VWG it is done only when you
//        // specifically update the control with the BO Values.
//        [Test]
//        public void Test_EditBusinessObjectProp_IfControlHasErrors_WhenBOValid_ShouldClearErrorMessage()
//        {
//            //---------------Set up test pack-------------------
//            Shape shape;
//            ControlMapperStub mapperStub;
//            ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out mapperStub);
//            mapperStub.BusinessObject = shape;
//            //---------------Assert Precondition----------------
//            Assert.IsFalse(mapperStub.BusinessObject.IsValid());
//            Assert.AreNotEqual("", mapperStub.ErrorProvider.GetError(textBox));
//            //---------------Execute Test ----------------------
//            shape.ShapeName = TestUtil.GetRandomString();
//            //---------------Test Result -----------------------
//            Assert.IsTrue(mapperStub.BusinessObject.IsValid());
//            Assert.AreEqual("", mapperStub.ErrorProvider.GetError(textBox));
//        }
//        //This test is different from VWG because an edit to the properties in Windows updates the 
//        // control and must therefore update the Error provider whereas for VWG it is done only when you
//        // specifically update the control with the BO values
//        [Test]
//        public void Test_UpdateErrorProviderError_IfControlHasNoErrors_WhenBOInvalid_ShouldSetsErrorMessage()
//        {
//            //---------------Set up test pack-------------------
//            Shape shape;
//            ControlMapperStub textBoxMapper;
//            ITextBox textBox = GetTextBoxForShapeNameWhereShapeNameCompulsory(out shape, out textBoxMapper);
//            shape.ShapeName = TestUtil.GetRandomString();
//            textBoxMapper.BusinessObject = shape;
//
//            //---------------Assert Precondition----------------
//            Assert.IsTrue(shape.IsValid());
//            Assert.AreEqual("", textBoxMapper.ErrorProvider.GetError(textBox));
//            //---------------Execute Test ----------------------
//            shape.ShapeName = "";
//            //---------------Test Result -----------------------
//            Assert.IsFalse(shape.IsValid());
//            Assert.AreNotEqual("", textBoxMapper.ErrorProvider.GetError(textBox));
//        }
        protected RelationshipComboBoxMapper GetMapperBoColHasOneItem
            (IComboBox cmbox, out BusinessObjectCollection<OrganisationTestBO> boCol)
        {
            RelationshipComboBoxMapper mapper = GetMapper(cmbox);
            boCol = GetBoColWithOneItem();
            mapper.BusinessObjectCollection = boCol;
            return mapper;
        }

        private static BusinessObjectCollection<OrganisationTestBO> GetBoColWithOneItem()
        {
            return new BusinessObjectCollection<OrganisationTestBO> {new OrganisationTestBO()};
        }

        private RelationshipComboBoxMapper GetMapper(IComboBox cmbox)
        {
            const string relationshipName = "Organisation";
            return new RelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
        }

        private static object LastComboBoxItem(IComboBox cmbox)
        {
            return cmbox.Items[cmbox.Items.Count - 1];
        }

        //TODO Brett 24 Mar 2009: Implement this functionality
        //[Test]
        //public void TestCustomiseLookupList_Add()
        //{
        //    //---------------Set up test pack-------------------
        //    IComboBox cmbox = GetControlFactory().CreateComboBox();
        //    const string relationshipName = "SampleLookup2ID";
        //    CustomAddRelationshipComboBoxMapper mapper = new CustomAddRelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
        //    Sample sample = new Sample();

        //    //---------------Execute Test ----------------------
        //    mapper.BusinessObject = sample;

        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(5, cmbox.Items.Count);
        //    Assert.AreEqual("ExtraLookupItem", LastComboBoxItem(cmbox).ToString());

        //    //---------------Tear Down -------------------------
        //    // This test changes the static class def, so force a reload
        //    ClassDef.ClassDefs.Remove(typeof(Sample));
        //}

//        [Test]
//        public void TestCustomiseLookupList_Add_SelectAdded()
//        {
//            //---------------Set up test pack-------------------
//            IComboBox cmbox = GetControlFactory().CreateComboBox();
//            const string relationshipName = "SampleLookup2ID";
//            CustomAddRelationshipComboBoxMapper mapper = new CustomAddRelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
//            Sample sample = new Sample();
//            mapper.BusinessObject = sample;
//            //---------------Assert Preconditions---------------
//            Assert.AreEqual(5, cmbox.Items.Count);
//            Assert.AreEqual("ExtraLookupItem", LastComboBoxItem(cmbox).ToString());
//            //---------------Execute Test ----------------------
//            cmbox.SelectedIndex = cmbox.Items.Count - 1;
//            mapper.ApplyChangesToBusinessObject();
//            //---------------Test Result -----------------------
//            object value = sample.GetPropertyValue(relationshipName);
//            Assert.IsNotNull(value);
//
//            //---------------Tear Down -------------------------
//            // This test changes the static class def, so force a reload
//            ClassDef.ClassDefs.Remove(typeof(Sample));
//        }
//        [Test]
//        public void TestCustomiseLookupList_Remove()
//        {
//            //---------------Set up test pack-------------------
//            IComboBox cmbox = GetControlFactory().CreateComboBox();
//            const string relationshipName = "SampleLookup2ID";
//            CustomRemoveRelationshipComboBoxMapper mapper = new CustomRemoveRelationshipComboBoxMapper(cmbox, relationshipName, false, GetControlFactory());
//            Sample sample = new Sample();
//
//            //---------------Execute Test ----------------------
//            mapper.BusinessObject = sample;
//
//            //---------------Test Result -----------------------
//            Assert.AreEqual(3, cmbox.Items.Count);
//
//            //---------------Tear Down -------------------------
//            // This test changes the static class def, so force a reload
//            ClassDef.ClassDefs.Remove(typeof(Sample));
//        }
    }

    [TestFixture]
    public class TestRelationshipComboBoxMapperWin : TestRelationshipComboBoxMapperVWG
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
        public void Test_EditItemFromCollection_UpdatesItemInCombo()
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
        public void Test_SetBOToNull_MustDeregisterForEvents_WhenChangePropValue_ShouldNotUpdateControlValue()
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
        public void Test_MustDeregisterForEvents_WhenSetBOToAnotherBo_AndChangePropValue_ShouldNotUpdateControlValue()
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
            ContactPersonTestBO newCP = new ContactPersonTestBO();
            mapper.BusinessObject = newCP;
            person.Organisation = newBO;

            //---------------Test Result -----------------------
            Assert.AreSame(newBO, person.Organisation);
            Assert.AreSame(null, newCP.Organisation);
            Assert.AreSame(null, cmbox.SelectedItem, "Value is not set after changing bo relationship");
        }

        //TODO Brett 24 Mar 2009: Test Changing BO's removes event handlers.

        [Test]
        public override void TestChangeComboBoxDoesntUpdateBusinessObject()
        {
            //For Windows the value should be changed.
            Assert.IsTrue
                (true,
                 "For Windows the value should be changed. See TestChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges");
        }

        [Test]
        public void TestChangeComboBoxUpdatesBusinessObject_WithoutCallingApplyChanges()
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
        public void TestKeyPressEventUpdatesBusinessObject_WithoutCallingApplyChanges()
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
            Assert.IsInstanceOfType(typeof (LookupComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
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
            Assert.IsInstanceOfType(typeof (LookupComboBoxKeyPressMapperStrategyWin), mapper.MapperStrategy);
            Assert.AreNotSame(newBO, person.Organisation, "For Windows the value should be changed.");
            Assert.AreSame(organisationTestBO, person.Organisation, "For Windows the value should be changed.");
        }
   
        private class ComboBoxWinStub : ComboBoxWin
        {
            public void CallSendKeyBob()
            {
                this.OnKeyPress(new System.Windows.Forms.KeyPressEventArgs((char) 13));
            }
        }
    }


//
//    internal class CustomAddRelationshipComboBoxMapper : RelationshipComboBoxMapper
//    {
//        public CustomAddRelationshipComboBoxMapper(IComboBox cbx, string relationshipName, bool isReadOnly, IControlFactory factory)
//            : base(cbx, relationshipName, isReadOnly, factory) { }
//
//        protected override void CustomiseLookupList(Dictionary<string, string> col)
//        {
//            Sample additionalBO = new Sample { SampleText = "ExtraLookupItem" };
//            col.Add(additionalBO.SampleText, additionalBO.ToString());
//        }
//    }
//
//    internal class CustomRemoveRelationshipComboBoxMapper : RelationshipComboBoxMapper
//    {
//        public CustomRemoveRelationshipComboBoxMapper(IComboBox cbx, string relationshipName, bool isReadOnly, IControlFactory factory)
//            : base(cbx, relationshipName, isReadOnly, factory) { }
//
//        protected override void CustomiseLookupList(Dictionary<string, string> col)
//        {
//            string lastKey = "";
//            foreach (string key in col.Keys)
//            {
//                lastKey = key;
//            }
//
//            col.Remove(lastKey);
//        }
//    }
}