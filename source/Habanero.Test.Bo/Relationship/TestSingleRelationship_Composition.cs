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
    public class TestSingleRelationship_Composition
    {
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_SingleReverse();
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
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
            //An already persi`sted Brain cannot be set as the brain of a person.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = GetCompositionRelationship(organisation);

            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            try
            {
                compositionRelationship.SetRelatedObject(contactPerson);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be added since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
        }

        [Test]
        public void Test_SetChild_NewChild()
        {
            //A new Brain can be set as the brain of a person.  This is allowed in Habanero for flexibility, but
            // it is rather recommended that the Person creates the Brain.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = GetCompositionRelationship(organisation);

            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //---------------Execute Test ----------------------
            compositionRelationship.SetRelatedObject(contactPerson);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, compositionRelationship.GetRelatedObject());
        }

        [Test]
        public void Test_SetParent_PersistedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef) compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = organisation;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be added since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
            //---------------Test Result -----------------------
        }

        [Test]
        public void Test_SetParent_NewChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
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
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            contactPerson.Save();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = null;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be removed since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }

        }

        [Test]
        public void Test_ResetParent_NewChild_SetToNull()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            contactPerson.Organisation = organisation;
            //---------------Assert Precondition----------------
            Assert.IsNotNull(contactPerson.Organisation);

            //---------------Execute Test ----------------------
            try
            {
                contactPerson.Organisation = null;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains
                    ("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains
                    ("could not be removed since the " + compositionRelationship.RelationshipName
                     + " relationship is set up as ", ex.Message);
            }
        }
        [Test]
        public void Test_SetParentNull()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
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
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            compositionRelationship.SetRelatedObject(contactPerson);

            //---------------Assert Precondition----------------
            Assert.AreEqual(contactPerson.OrganisationID, organisation.OrganisationID);
            Assert.AreSame(organisation.ContactPerson, contactPerson);
            Assert.AreEqual(RelationshipType.Composition, compositionRelationship.RelationshipDef.RelationshipType);

            //---------------Execute Test ----------------------
            try
            {
                compositionRelationship.SetRelatedObject(null);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("The " + compositionRelationship.RelationshipDef.RelatedObjectClassName, ex.Message);
                StringAssert.Contains("could not be removed since the " + compositionRelationship.RelationshipName + " relationship is set up as ", ex.Message);
            }
        }

        [Test]
        public void Test_DirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO);
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
            SingleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO, (ICollection)dirtyChildren);
            Assert.AreEqual(1, dirtyChildren.Count);
        }
        

        [Test]
        public void Test_DirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO);
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
            SingleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Organisation = organisationTestBO;
            organisationTestBO.Save();
            myBO.MarkForDelete();

            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO, (ICollection)dirtyChildren);
            Assert.AreEqual(1, dirtyChildren.Count);
        }
        


        [Test]
        public void Test_DirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO);

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
            SingleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisationTestBO);
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(contactPerson, (ICollection)dirtyChildren);
            Assert.AreEqual(1, dirtyChildren.Count);
        }


        /// <summary>
        /// An invoice is considered to be dirty if it has any dirty invoice line. 
        ///   A dirty invoice line would be any invoice line that is dirty and would include a newly created invoice line 
        ///   and an invoice line that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetCompositionRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            relationship.SetRelatedObject(contactPerson);
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(relationship.IsDirty);
            Assert.IsFalse(organisation.Relationships.IsDirty);
            Assert.IsFalse(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.GetRandomString();

            //---------------Test Result -----------------------
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(relationship.IsDirty);
            Assert.IsTrue(organisation.Relationships.IsDirty);
            Assert.IsTrue(organisation.Status.IsDirty);
        }

        private SingleRelationship<ContactPersonTestBO> GetCompositionRelationship(OrganisationTestBO organisationTestBO)
        {
            RelationshipType relationshipType = RelationshipType.Composition;
            SingleRelationship<ContactPersonTestBO> compositionRelationship =
                organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            return compositionRelationship;
        }
    }

    [TestFixture]
    public class TestSingleRelationship_Composition_DB : TestSingleRelationship_Composition
    {
        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            TestUsingDatabase.SetupDBDataAccessor();
        }
    }
}