using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{ 
    [TestFixture]
    public class TestTransactionalSingleRelationship
    {

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            OrganisationTestBO.LoadDefaultClassDef();
        }

        [Test]
        public void Test_TransactionID()
        {
            //---------------Set up test pack-------------------
            TransactionalSingleRelationship_Added transactionalSingleRelationship1 = new TransactionalSingleRelationship_Added(null);
            TransactionalSingleRelationship_Added transactionalSingleRelationship2 = new TransactionalSingleRelationship_Added(null);
            
            //---------------Execute Test ----------------------
            string transactionID1 = transactionalSingleRelationship1.TransactionID();
            string transactionID2 = transactionalSingleRelationship2.TransactionID();

            //---------------Test Result -----------------------
            Assert.AreEqual(transactionID1, transactionalSingleRelationship1.TransactionID()); 
            Assert.AreEqual(transactionID2, transactionalSingleRelationship2.TransactionID());

            Assert.AreNotEqual(transactionID1, transactionID2);
        }

        [Test]
        public void Test_Constructor()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            SingleRelationship<OrganisationTestBO> singleRelationship = contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");

            //---------------Execute Test ----------------------
            TransactionalSingleRelationship_Added tsr = new TransactionalSingleRelationship_Added(singleRelationship);
            //---------------Test Result -----------------------

            Assert.AreSame(singleRelationship, tsr.Relationship);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_UpdateStateAsCommitted()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();

            SingleRelationship<OrganisationTestBO> singleRelationship = contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            singleRelationship.SetRelatedObject(organisationTestBO);
            TransactionalSingleRelationship_Added tsr = new TransactionalSingleRelationship_Added(singleRelationship);
            IBOProp relationshipProp = contactPersonTestBO.Props["OrganisationID"];
            
            //---------------Assert PreConditions--------------- 
            Assert.IsTrue(relationshipProp.IsDirty);
            Assert.AreNotEqual(relationshipProp.Value, relationshipProp.PersistedPropertyValue);
            
            //---------------Execute Test ----------------------
            tsr.UpdateStateAsCommitted();
            
            //---------------Test Result -----------------------
            Assert.IsFalse(relationshipProp.IsDirty);
            Assert.AreEqual(relationshipProp.Value, relationshipProp.PersistedPropertyValue);
        }

        [Test]
        public void Test_UpdateStateAsCommitted_Added_UpdatesCols()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPerson();
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();

            SingleRelationship<OrganisationTestBO> singleRelationship = contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            BusinessObjectCollection<ContactPersonTestBO> people = organisationTestBO.ContactPeople;
            contactPersonTestBO.Organisation = organisationTestBO;
            TransactionalSingleRelationship_Added tsr = new TransactionalSingleRelationship_Added(singleRelationship);
            //---------------Assert PreConditions--------------- 

            Assert.AreEqual(1, people.AddedBusinessObjects.Count);
            Assert.AreEqual(0, people.PersistedBusinessObjects.Count);
            //---------------Execute Test ----------------------

            tsr.UpdateStateAsCommitted();
            //---------------Test Result -----------------------

            Assert.AreEqual(0, people.AddedBusinessObjects.Count);
            Assert.AreEqual(1, people.PersistedBusinessObjects.Count);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void Test_UpdateStateAsCommitted_Removed_UpdatesCols()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPerson();
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();

            SingleRelationship<OrganisationTestBO> singleRelationship = contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            BusinessObjectCollection<ContactPersonTestBO> people = organisationTestBO.ContactPeople;
            contactPersonTestBO.Organisation = organisationTestBO;
            organisationTestBO.Save();
            contactPersonTestBO.Organisation = null;
            TransactionalSingleRelationship_Removed tsr = new TransactionalSingleRelationship_Removed(singleRelationship);
            //---------------Assert PreConditions--------------- 

            Assert.AreEqual(1, people.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, people.PersistedBusinessObjects.Count);
            Assert.IsTrue(singleRelationship.IsRemoved);
            //---------------Execute Test ----------------------

            tsr.UpdateStateAsCommitted();
            //---------------Test Result -----------------------

            Assert.AreEqual(0, people.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, people.PersistedBusinessObjects.Count);
            Assert.IsFalse(singleRelationship.IsRemoved);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_UpdateStateAsCommitted_UpdateDirtyStatus_IDOnly()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPerson();
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();

            SingleRelationship<OrganisationTestBO> singleRelationship = contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            singleRelationship.SetRelatedObject(organisationTestBO);
            
            TransactionalSingleRelationship_Added tsr = new TransactionalSingleRelationship_Added(singleRelationship);
            
            //---------------Assert PreConditions--------------- 
            Assert.IsTrue(contactPersonTestBO.Status.IsDirty);
            
            //---------------Execute Test ----------------------
            tsr.UpdateStateAsCommitted();
            
            //---------------Test Result -----------------------
            Assert.IsFalse(contactPersonTestBO.Status.IsDirty);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_UpdateStateAsCommitted_UpdateDirtyStatus_OtherDirtyField()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPerson();
            OrganisationTestBO organisationTestBO = new OrganisationTestBO();

            SingleRelationship<OrganisationTestBO> singleRelationship = contactPersonTestBO.Relationships.GetSingle<OrganisationTestBO>("Organisation");
            singleRelationship.SetRelatedObject(organisationTestBO);
            TransactionalSingleRelationship_Added tsr = new TransactionalSingleRelationship_Added(singleRelationship);

            contactPersonTestBO.Surname = TestUtil.CreateRandomString();
            
            //---------------Assert PreConditions--------------- 
            Assert.IsTrue(contactPersonTestBO.Status.IsDirty);
            
            //---------------Execute Test ----------------------
            tsr.UpdateStateAsCommitted();
            
            //---------------Test Result -----------------------
            Assert.IsTrue(contactPersonTestBO.Status.IsDirty);
            //---------------Tear Down -------------------------          
        }

    }
}
