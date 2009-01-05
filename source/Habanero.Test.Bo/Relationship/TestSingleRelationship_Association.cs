using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestSingleRelationship_Association
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            OrganisationTestBO.LoadDefaultClassDef_WithSingleRelationship();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
        }


        [Test]
        public void Test_SetChild_PersistedChild()
        {
            //An already persisted contactperson can be set as the contact person of an organisation
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();

            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(contactPerson);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, relationship.GetRelatedObject());
        }

        [Test]
        public void Test_SetChild_NewChild()
        {
            //A new ContactPerson can be set as the contact person of an organisation.
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();

            //---------------Execute Test ----------------------
            relationship.SetRelatedObject(contactPerson);

            //---------------Test Result -----------------------
            Assert.AreSame(contactPerson, relationship.GetRelatedObject());
        }

        [Test]
        public void Test_SetParent_PersistedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
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
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
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
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
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
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
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
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
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
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            relationship.OwningBOHasForeignKey = false;
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
        }
        
        [Test]
        public void Test_DirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();


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
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
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
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Organisation = organisationTestBO;
            myBO.Save();
            myBO = organisationTestBO.ContactPerson;

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
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
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
        public void Test_DirtyIfHasRemoveChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
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
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            relationship.OwningBOHasForeignKey = false;
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Organisation = organisationTestBO;
            myBO.Save();
            organisationTestBO.ContactPerson = null;

            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO, (ICollection)dirtyChildren);
            Assert.AreEqual(1, dirtyChildren.Count);
        }


        [Test]
        public void Test_NotDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);

            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsFalse(relationship.IsDirty);
        }

        [Test]
        public void Test_GetDirtyChildren_Edited()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO);
            ContactPersonTestBO contactPerson = new ContactPersonTestBO();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Organisation = organisationTestBO;
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Execute Test ----------------------

            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
        }

        /// <summary>
        /// An Organisation is not considered dirty if it has a dirty contact person.
        /// </summary>
        [Test]
        public void Test_ParentNotDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisation = OrganisationTestBO.CreateSavedOrganisation();
            SingleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisation);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            relationship.SetRelatedObject(contactPerson);
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisation.Status.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsFalse(organisation.Status.IsDirty);
        }


        private SingleRelationship<ContactPersonTestBO> GetAssociationRelationship(OrganisationTestBO organisationTestBO)
        {
            RelationshipType relationshipType = RelationshipType.Association;
            SingleRelationship<ContactPersonTestBO> compositionRelationship =
                organisationTestBO.Relationships.GetSingle<ContactPersonTestBO>("ContactPerson");
            RelationshipDef relationshipDef = (RelationshipDef)compositionRelationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            return compositionRelationship;
        }
    }
}
