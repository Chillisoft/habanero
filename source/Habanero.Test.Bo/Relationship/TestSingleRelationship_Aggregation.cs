using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public class TestSingleRelationship_Aggregation
    {
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse();
        }


        [Test]
        public void Test_SetChild_PersistedChild()
        {
            //An already persisted Heart can be set as the heart of a person (since you can transplant hearts)
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship =
                GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(contactPerson);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, relationship.GetRelatedObject());
        }

        [Test]
        public void Test_SetChild_AggregateDoesNotOwnForeignKey_PersistedChild()
        {
            //An already persisted Heart can be set as the heart of a person (since you can transplant hearts)
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            SingleRelationship<OrganisationTestBO> relationship =
                GetAggregationRelationshipOrganisation(contactPerson, "Organisation");
            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(organisationTestBO);
            OrganisationTestBO returnedOrganisationTestBO = relationship.GetRelatedObject();
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, returnedOrganisationTestBO);
        }

        [Test]
        public void Test_SetChild_NewChild()
        {
            //A new Heart can be set as the heart of a person.  This is allowed in Habanero for flexibility, but
            // it is rather recommended that the Person creates the Heart.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship =
                GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(contactPerson);
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, relationship.GetRelatedObject());
        }
        [Test]
        public void Test_SetChild_AggregateDoesNotOwnForeignKey_NewChild()
        {
            //A new Heart can be set as the heart of a person.  This is allowed in Habanero for flexibility, but
            // it is rather recommended that the Person creates the Heart.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            SingleRelationship<OrganisationTestBO> relationship = GetAggregationRelationshipOrganisation(contactPerson, "Organisation");
            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(organisationTestBO);
            //---------------Test Result -----------------------
            Assert.AreSame(organisationTestBO, relationship.GetRelatedObject());
        }

        [Test]
        public void Test_SetParent_PersistedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }
        [Test]
        public void Test_SetParent_AggregateDoesNotOwnForeignKey_PersistedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            GetAggregationRelationshipOrganisation(contactPerson, "Organisation");
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }

        [Test]
        public void Test_SetParent_NewChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }
        [Test]
        public void Test_SetParent_AggregateDoesNotOwnForeignKey_NewChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            GetAggregationRelationshipOrganisation(contactPerson, "Organisation");
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }


        [Test]
        public void Test_SetParentNull_PersistedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            contactPerson.Save();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
            Assert.IsNotNull(organisation.OrganisationID);
        }

        [Test]
        public void Test_ResetParent_NewChild_SetToNull()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
            Assert.IsNotNull(organisation.OrganisationID);
        }

        [Test]
        public void Test_SetParentNull()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            //---------------Assert Precondition----------------
            Assert.IsNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;

            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
        }


        [Test]
        public void Test_SetParent_PersistedChild_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Assert Precondition -----------------------
            Assert.IsNotNull(organisation.OrganisationID);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;
            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }

        [Test]
        public void Test_SetParent_NewChild_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisation;

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, organisation.ContactPerson);
            Assert.AreSame(organisation, contactPerson.Organisation);
        }


        [Test]
        public void Test_SetParentNull_PersistedChild_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            contactPerson.Save();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);
            AssertIsAggregateRelationship(organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
            Assert.IsNotNull(organisation.OrganisationID);
        }

        [Test]
        public void Test_ResetParent_NewChild_SetToNull_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.OwningBOHasForeignKey);
            Assert.IsNotNull(contactPerson.Organisation);
            Assert.IsNotNull(contactPerson.Organisation);
            AssertIsAggregateRelationship(organisation);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
            Assert.IsNotNull(organisation.OrganisationID);
            Assert.IsNull(organisation.ContactPerson);
        }
        [Test]
        public void Test_SetChildNull_NewChild_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisation, "ContactPerson");

            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.OwningBOHasForeignKey);
            Assert.IsNotNull(contactPerson.Organisation);
            Assert.IsNotNull(contactPerson.Organisation);
            Assert.IsNotNull(organisation.ContactPerson);
            AssertIsAggregateRelationship(organisation);
            //---------------Execute Test ----------------------
            organisation.ContactPerson = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
            Assert.IsNotNull(organisation.OrganisationID);
            Assert.IsNull(organisation.ContactPerson);
        }
        private static void AssertIsAggregateRelationship(BusinessObject organisation)
        {
            SingleRelationship<ContactPersonTestBO> aggregationRelationship =
                organisation.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            Assert.AreEqual(RelationshipType.Aggregation, aggregationRelationship.RelationshipDef.RelationshipType);
        }

        [Test]
        public void Test_SetParentNull_NonPersistedParent()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateUnsavedOrganisation();
            GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            //---------------Assert Precondition----------------
            SingleRelationship<ContactPersonTestBO> aggregationRelationship =
                organisation.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            Assert.IsNull(contactPerson.Organisation);
            Assert.AreEqual(RelationshipType.Aggregation, aggregationRelationship.RelationshipDef.RelationshipType);
            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;

            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
            Assert.IsNotNull(organisation.OrganisationID);
            Assert.IsNull(organisation.ContactPerson);
        }

        [Test]
        public void Test_RemoveMethod()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            relationship.SetRelatedObject(contactPerson);
            //---------------Assert Precondition----------------
            Assert.AreEqual(contactPerson.OrganisationID, organisation.OrganisationID);
            Assert.AreSame(organisation.ContactPerson, contactPerson);
            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(null);
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(organisation.ContactPerson);
            Assert.IsNotNull(organisation.OrganisationID);
            Assert.IsNull(contactPerson.Organisation);
        }

        [Test]
        public void Test_SetParentNull_NewChild_BotRelationshipSetUpAsOwning()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            SingleRelationship<OrganisationTestBO> relationshipOrganisation = GetAggregationRelationshipOrganisation(contactPerson, "Organisation");
            relationshipOrganisation.OwningBOHasForeignKey = true;

            SingleRelationship<ContactPersonTestBO> relationshipContactPerson = GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            relationshipContactPerson.OwningBOHasForeignKey = true;
            //---------------Assert Preconditon-----------------
            Assert.IsNull(organisation.ContactPerson);
            Assert.IsNull(contactPerson.Organisation);
            Assert.IsNotNull(organisation.OrganisationID);
            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = organisation;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The corresponding single (one to one) relationships Organisation ", ex.Message);
                StringAssert.Contains("cannot both be configured as having the foreign key", ex.Message);
            }
        }

        [Test]
        public void Test_DirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();


            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            myBO.Organisation = organisationTestBO;
            bool isDirty = relationship.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_Created()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO, (ICollection) dirtyChildren);
            Assert.AreEqual(1, dirtyChildren.Count);
        }

        [Test]
        public void Test_DirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            organisationTestBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            myBO.MarkForDelete();
            bool isDirty = relationship.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_MarkedForDelete()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            organisationTestBO.Save();
            myBO.MarkForDelete();

            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO, (ICollection) dirtyChildren);
            Assert.AreEqual(1, dirtyChildren.Count);
        }

        [Test]
        public void Test_DirtyIfHasRemovedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.ContactPerson = null;
            bool isDirty = relationship.IsDirty;

            //---------------Test Result -----------------------
            Assert.IsTrue(isDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_Removed()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            myBO.Save();
            organisationTestBO.ContactPerson = null;

            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO, (ICollection) dirtyChildren);
            Assert.AreEqual(1, dirtyChildren.Count);
        }


        [Test]
        public void Test_DirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");

            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Test Result -----------------------
            Assert.IsTrue(relationship.IsDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_Edited()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisationTestBO, "ContactPerson");
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(contactPerson, (ICollection) dirtyChildren);
            Assert.AreEqual(1, dirtyChildren.Count);
        }


        /// <summary>
        /// A Person is considered dirty if it has a dirty heart.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAggregationRelationshipContactPerson(organisation, "ContactPerson");
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            relationship.SetRelatedObject(contactPerson);
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Test Result -----------------------
            Assert.IsTrue(organisation.Status.IsDirty);
        }


        private static SingleRelationship<ContactPersonTestBO> GetAggregationRelationshipContactPerson
            (BusinessObject businessObject, string relationshipName)
        {
            const RelationshipType relationshipType = RelationshipType.Aggregation;
            SingleRelationship<ContactPersonTestBO> aggregationRelationship =
                businessObject.Relationships.GetSingle<ContactPersonTestBO>(relationshipName);
            RelationshipDef relationshipDef = (RelationshipDef) aggregationRelationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            return aggregationRelationship;
        }

        private static SingleRelationship<OrganisationTestBO> GetAggregationRelationshipOrganisation
            (BusinessObject businessObject, string relationshipName)
        {
            const RelationshipType relationshipType = RelationshipType.Aggregation;
            SingleRelationship<OrganisationTestBO> aggregationRelationship =
                businessObject.Relationships.GetSingle<OrganisationTestBO>(relationshipName);
            RelationshipDef relationshipDef = (RelationshipDef) aggregationRelationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            return aggregationRelationship;
        }
    }

    [TestFixture]
    public class TestSingleRelationship_Aggregation_DB : TestSingleRelationship_Aggregation
    {
        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            TestUsingDatabase.SetupDBDataAccessor();
        }
    }
}