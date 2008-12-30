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
//    •	A typical example of an aggregation relationship is a Car and its Tyres. A Tyre can exist independently of its Car and a Tyre can only belong to a single Car at any point in time. The Tyre may however be transferred from one car to another. 
//•	The Car that has tyres cannot be deleted without it deleting or removing its tyres. The car’s Tyres relationship would be marked as either prevent delete, dereference tyres, delete tyres or do nothing. 
//•	An already persisted tyre can be added to a car (In habanero a new tyre can be added to a car). A tyre can be removed from its car. 
//•	A car can create a new tyre via its Tyres Relationship.
//•	A car is considered to be dirty if it has any dirty tyres. A dirty tyre would include a newly created tyre, an added tyre, a removed tyre or a tyre that has been marked for deletion.
//•	If an car is persisted then it must persist all its tyres.


        [TestFixture]
    public class TestRelatedBOCol_Aggregation
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
            //•	An already persisted tyre can be added to a car 
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship aggregateRelationship =  (Relationship)organisationTestBO.Relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;
            
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>) aggregateRelationship.GetRelatedBusinessObjectCol(); ;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            cpCol.Add(contactPerson);

            //---------------Test Result -----------------------
            util.AssertAddedEventFired();
            util.AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.AreSame(contactPerson.Organisation, aggregateRelationship.OwningBO);
        }


        [Test]
        public void Test_AddMethod_AddNewChild()
        {
            //•(In habanero a new tyre can be added to a car).
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship aggregateRelationship =  (Relationship)organisationTestBO.Relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;
            
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>) aggregateRelationship.GetRelatedBusinessObjectCol(); ;
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateUnsavedContactPerson();
            util.RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            util.AssertAllCollectionsHaveNoItems(cpCol);

            //---------------Execute Test ----------------------
            cpCol.Add(contactPerson);

            //---------------Test Result -----------------------
            util.AssertAddedEventFired();
            util.AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.AreSame(contactPerson.Organisation, aggregateRelationship.OwningBO);
        }

        
        [Test]
        public void Test_ResetParent_PersistedChild()
        {
            //The Tyre may however be transferred from one car to another. 
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship aggregateRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;

            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol(); ;
            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            OrganisationTestBO alternateOrganisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            
            //---------------Assert Precondition----------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(contactPerson.Status.IsNew);
            Assert.AreSame(contactPerson.Organisation, organisationTestBO);

            //---------------Execute Test ----------------------
            contactPerson.Organisation = alternateOrganisationTestBO;

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(cpCol.Contains(contactPerson));
            util.AssertOneObjectInRemovedAndPersisted(cpCol);
            Relationship relationship = (Relationship)alternateOrganisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpAltCol =
                    (RelatedBusinessObjectCollection<ContactPersonTestBO>)relationship.GetRelatedBusinessObjectCol(); ;
            Assert.AreSame(contactPerson.Organisation, relationship.OwningBO);
            Assert.AreSame(alternateOrganisationTestBO, contactPerson.Organisation);
            util.AssertOneObjectInCurrentAndAddedCollection(cpAltCol);
        }

        [Test]
        public void Test_ResetParent_NewChild_ReverseRelationship_Loaded()
        {
            //The Tyre may however be transferred from one car to another. 
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship aggregateRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;

            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();
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
            //A tyre can be removed from its car. 
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship aggregateRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;

            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();

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
            //A tyre can be removed from its car.   This test is removing via the reverse relationship
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship aggregateRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;

            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();

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
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship aggregateRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;

            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasMarkForDeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            Relationship aggregateRelationship = (Relationship)organisationTestBO.Relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;

            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_ParentDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship aggregateRelationship = (Relationship)relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.IsFalse(relationships.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_GetDirtyChildren_ReturnDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship aggregateRelationship = (Relationship)relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = aggregateRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_GetDirtyChildren_ReturnCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship aggregateRelationship = (Relationship)relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = aggregateRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_GetDirtyChildren_ReturnMark4DeleteChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship aggregateRelationship = (Relationship)relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            cpCol.MarkForDelete(contactPerson);

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = aggregateRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_GetDirtyChildren_ReturnAllDirty()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship aggregateRelationship = (Relationship)relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();

            ContactPersonTestBO myBO_delete = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            cpCol.MarkForDelete(myBO_delete);
            ContactPersonTestBO myBO_Edited = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            myBO_Edited.Surname = TestUtil.CreateRandomString();

            ContactPersonTestBO myBo_Created = ContactPersonTestBO.CreateUnsavedContactPerson_AsChild(cpCol);

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = aggregateRelationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO_delete, (ICollection)dirtyChildren);
            Assert.Contains(myBO_Edited, (ICollection)dirtyChildren);
            Assert.Contains(myBo_Created, (ICollection)dirtyChildren);
            Assert.AreEqual(3, dirtyChildren.Count);
        }

        /// <summary>
        /// A car is considered to be dirty if it has any dirty tyres. 
        /// A dirty tyre would include a newly created tyre, an added tyre, 
        /// a removed tyre or a tyre that has been marked for deletion.
        /// </summary>
        [Test]
        public void Test_Relationships_GetDirtyChildren_ReturnAllDirty()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship aggregateRelationship = (Relationship)relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();

            ContactPersonTestBO myBO_delete = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            cpCol.MarkForDelete(myBO_delete);
            ContactPersonTestBO myBO_Edited = ContactPersonTestBO.CreateSavedContactPerson_AsChild(cpCol);
            myBO_Edited.Surname = TestUtil.CreateRandomString();

            ContactPersonTestBO myBo_Created = ContactPersonTestBO.CreateUnsavedContactPerson_AsChild(cpCol);

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);

            //---------------Execute Test ----------------------
            IList<IBusinessObject> dirtyChildren = relationships.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.Contains(myBO_delete, (ICollection)dirtyChildren);
            Assert.Contains(myBO_Edited, (ICollection)dirtyChildren);
            Assert.Contains(myBo_Created, (ICollection)dirtyChildren);
            Assert.AreEqual(3, dirtyChildren.Count);
        }

        /// <summary>
        /// • If a car is persisted then it must persist all its tyres.
        /// </summary>
        [Test]
        public void Test_ParentPersistsDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            Relationship aggregateRelationship = (Relationship)relationships["ContactPeople"];
            aggregateRelationship.RelationshipDef.RelationshipType = RelationshipType.Aggregation;
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol =
                (RelatedBusinessObjectCollection<ContactPersonTestBO>)aggregateRelationship.GetRelatedBusinessObjectCol();

            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.CreateRandomString();
            myBO.FirstName = TestUtil.CreateRandomString();
            myBO.Save();
            myBO.FirstName = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.IsTrue(aggregateRelationship.IsDirty);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsTrue(relationships.IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            util.AssertOneObjectInCurrentPersistedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            Assert.IsFalse(aggregateRelationship.IsDirty);
            Assert.IsFalse(relationships.IsDirty);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
        }



    }
}
