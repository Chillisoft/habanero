using System.Collections;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO.RelatedBusinessObjectCollection;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO
{
/// <summary>
/// •	A typical example of an associative relationship is a Car and its Drivers (assuming a car can have many drivers but a driver may only drive one car). A Driver can exist independently of any Car and a Car can exist independently of a driver. The Driver may however be associated with one car and later associated with a different car. 
///•	The rules for whether a car that is associated with one or more drivers can be deleted or not is dependent upon the rules configured for the Drivers relationship (i.e. a car’s drivers relationship could be marked prevent delete, dereference or do nothing). 
///•	An already persisted driver can be added to a car (In habanero a new driver can be added to a car).
///•	A driver can be removed from its related car. 
///•	A car can create a new driver via its Drivers Relationship (this is not a strict implementation of domain design but is allowed due to the convenience of this).
///•	A car is considered to be dirty only if it has added, created or removed dirty drivers. 
///•	If a car is persisted then it will only persist its driver’s relationship and will not persist a related driver that is dirty.
/// </summary>
    [TestFixture]
    public class TestRelatedBOCol_Association
    {
        private readonly TestUtilsRelated util = new TestUtilsRelated();

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
        }

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }

        [TearDown]
        public void TearDownTest()
        {
            TestUtil.WaitForGC();
        }

        [Test]
        public void Test_AddMethod_AddPersistedChild()
        {
            //An already persisted driver can be added to a car
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            cpCol.Add(contactPerson);

            //---------------Test Result -----------------------
            util.AssertAddedEventFired();
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.AreSame(contactPerson.Organisation, associationRelationship.OwningBO);
        }

        private MultipleRelationship<ContactPersonTestBO> GetAssociationRelationship(OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            MultipleRelationship<ContactPersonTestBO> associationRelationship = organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)associationRelationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            cpCol = associationRelationship.BusinessObjectCollection;
            return associationRelationship;
        }


        [Test]
        public void Test_AddMethod_AddNewChild()
        {
            //• (In habanero a new driver can be added to a car).
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            cpCol.Add(contactPerson);

            //---------------Test Result -----------------------
            util.AssertAddedEventFired();
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.AreSame(contactPerson.Organisation, associationRelationship.OwningBO);
        }


        [Test]
        public void Test_ResetParent_PersistedChild()
        {
            //A driver can be removed from its related car and transferred to another. 
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            OrganisationTestBO alternateOrganisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            
            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(contactPerson.Status.IsNew);
            Assert.AreSame(contactPerson.Organisation, organisationTestBO);
            // Assert.AreEqual(0, cpAltCol.Count);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = alternateOrganisationTestBO;

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(cpCol.Contains(contactPerson));
            util.AssertOneObjectInRemovedAndPersisted(cpCol);
            MultipleRelationship<ContactPersonTestBO> relationship = alternateOrganisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            BusinessObjectCollection<ContactPersonTestBO> cpAltCol = relationship.BusinessObjectCollection;
            Assert.AreSame(contactPerson.Organisation, relationship.OwningBO);
            Assert.AreSame(alternateOrganisationTestBO, contactPerson.Organisation);
            util.AssertOneObjectInCurrentAndAddedCollection(cpAltCol);
        }

        [Test]
        public void Test_ResetParent_NewChild_ReverseRelationship_Loaded()
        {
            //A driver can be removed from its related car and transferred to another. 
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = organisationTestBO;

            //---------------Test Result -----------------------
            Assert.AreEqual(contactPerson.OrganisationID, organisationTestBO.OrganisationID);
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsTrue(cpCol.Contains(contactPerson));
            util.AssertAddedEventFired();
        }

        [Test]
        public void Test_SetParentNull()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());

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
            //A driver can be removed from its related car
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            contactPerson.Save();
            cpCol.LoadAll();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);

            //---------------Execute Test ----------------------
            cpCol.Remove(contactPerson);

            //---------------Test Result -----------------------
            util.AssertOneObjectInRemovedAndPersisted(cpCol);
            util.AssertRemovedEventFired();
        }


        [Test]
        public void Test_ResetParent_NewChild_SetToNull()
        {
            //A driver can be removed from its related car.   This test is removing via the reverse relationship
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            contactPerson.OrganisationID = organisationTestBO.OrganisationID;
            contactPerson.Save();
            cpCol.LoadAll();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);

            //---------------Execute Test ----------------------

            contactPerson.Organisation = null;

            //---------------Test Result -----------------------
            Assert.IsNull(contactPerson.Organisation);
            util.AssertOneObjectInRemovedAndPersisted(cpCol);
            util.AssertRemovedEventFired();
        }

        /// <summary>
        /// A car is considered to be dirty only if it has added, created or removed dirty drivers. 
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.ContactPeople.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
        }

        /// <summary>
        /// A car is considered to be dirty only if it has added, created or removed dirty drivers. 
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
        }

        /// <summary>
        /// A car is considered to be dirty only if it has added, created or removed dirty drivers. 
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasRemovedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
        }


        /// <summary>
        /// A car is considered to be dirty only if it has added, created or removed dirty drivers. 
        /// </summary>
        [Test]
        public void Test_ParentNotDirtyIfHasEditedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
        }


        /// <summary>
        ///•	If a car is persisted then it will only persist its driver’s relationship and will not persist a 
        /// related driver that is dirty.
        /// </summary>
        [Test]
        public void Test_ParentDoesNotPersistDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();
            myBO.FirstName = TestUtil.CreateRandomString();
            organisationTestBO.Name = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty); 

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(organisationTestBO.Status.IsDirty); 
            Assert.IsTrue(myBO.Status.IsDirty);

        }



    }
}
