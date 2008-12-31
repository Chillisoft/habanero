using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestSingleRelationship_Aggregation
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        [Test]
        public void Test_SetChild_PersistedChild()
        {
            //An already persisted Heart can be set as the heart of a person (since you can transplant hearts)
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef) aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            aggregateRelationship.SetRelatedObject(contactPerson);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, aggregateRelationship.GetRelatedObject());
        }

        [Test]
        public void Test_SetChild_NewChild()
        {
            //A new Heart can be set as the heart of a person.  This is allowed in Habanero for flexibility, but
            // it is rather recommended that the Person creates the Heart.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //---------------Execute Test ----------------------
            aggregateRelationship.SetRelatedObject(contactPerson);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, aggregateRelationship.GetRelatedObject());
        }

        [Test]
        public void Test_SetParent_PersistedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisation.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

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
            RelationshipCol relationships = organisation.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

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
            RelationshipCol relationships = organisation.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            contactPerson.Save();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);

        }

        [Test]
        public void Test_ResetParent_NewChild_SetToNull()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisation.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;
            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
        }
        [Test]
        public void Test_SetParentNull()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisation.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            //---------------Assert Precondition----------------
            Assert.IsNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = null;

            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);

        }

        [Test]
        public void Test_RemoveMethod()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisation.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            aggregateRelationship.SetRelatedObject(contactPerson);

            //---------------Assert Precondition----------------
            Assert.AreEqual(contactPerson.OrganisationID, organisation.OrganisationID);
            Assert.AreSame(organisation.ContactPerson, contactPerson);

            //---------------Execute Test ----------------------
            aggregateRelationship.SetRelatedObject(null);

            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.OrganisationID);
            Assert.IsNull(organisation.ContactPerson);
            Assert.IsNotNull(organisation.OrganisationID);
        }

        /// <summary>
        /// A Person is considered dirty if it has a dirty heart.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisation.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            aggregateRelationship.SetRelatedObject(contactPerson);
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.IsFalse(organisation.Relationships.IsDirty);
            Assert.IsFalse(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(organisation.Relationships.IsDirty);
            Assert.IsTrue(organisation.Status.IsDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_ReturnAllDirty()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisation.Relationships;
            ISingleRelationship aggregateRelationship = (ISingleRelationship)relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)aggregateRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Aggregation;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            aggregateRelationship.SetRelatedObject(contactPerson);
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            contactPerson.Surname = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPerson.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = aggregateRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.Contains(contactPerson, (ICollection)dirtyChildren);
        }
    }
}