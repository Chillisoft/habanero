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
    public class TestMultipleRelationship_Association
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            OrganisationTestBO.LoadDefaultClassDef_PreventAddChild();
        }

        [Test]
        public void Test_DirtyIfHasCreatedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            //---------------Assert Precondition----------------
            Assert.IsFalse(associationRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsTrue(associationRelationship.IsDirty);
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

        [Test]
        public void Test_DirtyIfHasAddedChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> associationRelationship = GetAssociationRelationship(organisationTestBO, out cpCol);
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Assert Precondition----------------
            Assert.IsFalse(associationRelationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Add(contactPerson);

            //---------------Test Result -----------------------
            Assert.IsTrue(associationRelationship.IsDirty);
        }

        [Test]
        public void Test_DirtyIfHasMarkForDeleteChildren()
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
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            Assert.IsTrue(relationship.IsDirty);
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

        [Test]
        public void Test_DirtyIfHasRemoveChildren()
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
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            Assert.IsTrue(relationship.IsDirty);
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



        [Test]
        public void Test_NotDirtyIfHasDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Test Result -----------------------
            Assert.IsFalse(relationship.IsDirty);
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




        [Test]
        public void Test_GetDirtyChildren_Edited()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
        }

        /// <summary>
        ///  Created child (ie a new object in the association relationship): saved when the parent is saved.
        /// Hence it should be in the GetDirtyChildren collection
        /// </summary>
        [Test]
        public void Test_GetDirtyChildren_Created()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }


        /// <summary>
        /// Marked for Delete child (ie an object in the association relationship that is marked for delete): deleted when the parent is saved
        /// Hence it should be in the GetDirtyChildren collection
        /// </summary>
        [Test]
        public void Test_GetDirtyChildren_MarkedForDelete()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            cpCol.MarkForDelete(contactPerson);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dirtyChildren.Count);
            Assert.IsTrue(dirtyChildren.Contains(contactPerson));
        }

        /// <summary>
        /// Added child (ie an already persisted object that has been added to the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// Hence it will not be in the GetDirtyChildren collection
        /// </summary>
        [Test]
        public void Test_GetDirtyChildren_Added()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(contactPerson);
            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
        }

        /// <summary>
        /// Removed child (ie an already persisted object that has been removed from the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// Hence it will not be in the GetDirtyChildren collection
        /// </summary>
        [Test]
        public void Test_GetDirtyChildren_Removed()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO contactPerson = cpCol.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            cpCol.Remove(contactPerson);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
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
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        /// <summary>
        /// Created child (ie a new object in the association relationship): saved when the parent is saved.
        /// </summary>
        [Test]
        public void Test_CreatedChild_SavesWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = organisationTestBO.ContactPeople.CreateBusinessObject();
            RelationshipDef relationshipDef = (RelationshipDef)organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
        }

        /// <summary>
        /// Marked for Delete child (ie an object in the association relationship that is marked for delete): deleted when the parent is saved
        /// </summary>
        [Test]
        public void Test_MarkedForDeleteChild_SavesWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = organisationTestBO.ContactPeople.CreateBusinessObject();
            RelationshipDef relationshipDef = (RelationshipDef)organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();
            contactPerson.MarkForDelete();

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            BOTestUtils.AssertBOStateIsValidAfterDelete(contactPerson);
        }

        /// <summary>
        /// Added child (ie an already persisted object that has been added to the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// </summary>
        [Test]
        public void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            RelationshipDef relationshipDef = (RelationshipDef) organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            organisationTestBO.ContactPeople.Add(contactPerson);
            contactPerson.Surname = TestUtil.CreateRandomString();
            
            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves_OnlyIDChanged()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            RelationshipDef relationshipDef = (RelationshipDef)organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            organisationTestBO.ContactPeople.Add(contactPerson);

            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Tear Down -------------------------          
        }

        /// <summary>
        /// Removed child (ie an already persisted object that has been removed from the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// </summary>
        [Test]
        public void Test_RemovedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef)organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            ContactPersonTestBO contactPerson = organisationTestBO.ContactPeople.CreateBusinessObject();
            contactPerson.Surname = TestUtil.CreateRandomString();
            contactPerson.FirstName = TestUtil.CreateRandomString();
            contactPerson.Save();

            contactPerson.Surname = TestUtil.CreateRandomString();
            organisationTestBO.ContactPeople.Remove(contactPerson);

            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
        }

        [Test]
        public void Test_AddDirtyChildrenToTransactionCommitter_AddedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship<ContactPersonTestBO> relationship = organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;

            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            relationship.BusinessObjectCollection.Add(contactPerson);
            SingleRelationship<OrganisationTestBO> reverseRelationship = contactPerson.Relationships.GetSingle<OrganisationTestBO>("Organisation");

            
            TransactionCommitterStub tc = new TransactionCommitterStub();

            //---------------Assert PreConditions---------------            
            Assert.AreEqual(0, tc.OriginalTransactions.Count);
            Assert.IsNotNull(reverseRelationship);

            //---------------Execute Test ----------------------
            relationship.AddDirtyChildrenToTransactionCommitter(tc);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, tc.OriginalTransactions.Count);

            Assert.IsInstanceOfType(typeof(TransactionalSingleRelationship_Added), tc.OriginalTransactions[0]);
            Assert.AreSame(reverseRelationship, ((TransactionalSingleRelationship_Added)tc.OriginalTransactions[0]).Relationship);

            //---------------Tear Down -------------------------          
        }

        private MultipleRelationship<ContactPersonTestBO> GetAssociationRelationship(OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            RelationshipType relationshipType = RelationshipType.Association;
            MultipleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            cpCol = relationship.BusinessObjectCollection;
            return relationship;
        }
    }
}
