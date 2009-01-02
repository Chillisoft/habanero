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
    public class TestSingleRelationship_Composition
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
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
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
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
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
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
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef) compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
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
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
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
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
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
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
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
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
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
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
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

        /// <summary>
        /// An invoice is considered to be dirty if it has any dirty invoice line. 
        ///   A dirty invoice line would be any invoice line that is dirty and would include a newly created invoice line 
        ///   and an invoice line that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            compositionRelationship.SetRelatedObject(contactPerson);
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(compositionRelationship.IsDirty);
            Assert.IsFalse(organisation.Relationships.IsDirty);
            Assert.IsFalse(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(compositionRelationship.IsDirty);
            Assert.IsTrue(organisation.Relationships.IsDirty);
            Assert.IsTrue(organisation.Status.IsDirty);
        }
        [Test]
        public void Test_GetDirtyChildren_ReturnAllDirty()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            ISingleRelationship compositionRelationship = (ISingleRelationship)organisation.Relationships["ContactPerson"];
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Composition;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            compositionRelationship.SetRelatedObject(contactPerson);
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            contactPerson.Surname = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(contactPerson.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = compositionRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.Contains(contactPerson, (ICollection)dirtyChildren);
        }

        private static ISingleRelationship GetCompositionRelationship(OrganisationTestBO organisation)
        {
            RelKeyDef def = new RelKeyDef();
            def.Add(new RelPropDef(organisation.Props["OrganisationID"].PropDef, "OrganisationID"));
            SingleRelationshipDef relationshipDef =
                new SingleRelationshipDef(TestUtil.CreateRandomString(), "Habanero.Test.BO",
                    "ContactPersonTestBO", def, false, DeleteParentAction.DeleteRelated,
                    RelationshipType.Composition);
            return (ISingleRelationship)relationshipDef.CreateRelationship(organisation, organisation.Props);
        }


    }
}