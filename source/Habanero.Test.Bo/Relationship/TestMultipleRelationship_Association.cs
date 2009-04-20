//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Db4objects.Db4o;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB4O;
using NUnit.Framework;

namespace Habanero.Test.BO.Relationship
{
    [TestFixture]
    public class TestMultipleRelationship_Association
    {
        [SetUp]
        public virtual void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship_MultipleReverse();
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
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
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
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
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
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
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
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(relationship.IsDirty);

            //---------------Execute Test ----------------------
            contactPerson.FirstName = TestUtil.GetRandomString();

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
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Save();

            //---------------Assert Precondition----------------
            Assert.IsFalse(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.FirstName = TestUtil.GetRandomString();

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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();
            contactPerson.FirstName = TestUtil.GetRandomString();

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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();

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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();
            cpCol.Remove(contactPerson);

            //---------------Execute Test ----------------------
            IList<ContactPersonTestBO> dirtyChildren = relationship.GetDirtyChildren();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dirtyChildren.Count);
        }

        /// <summary>
        ///�	If a car is persisted then it will only persist its driver�s relationship and will not persist a 
        /// related driver that is dirty.
        /// </summary>
        [Test]
        public void Test_PersistParent_DoesNotPersistDirtyChildren()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipCol relationships = organisationTestBO.Relationships;
            BusinessObjectCollection<ContactPersonTestBO> cpCol;
            MultipleRelationship<ContactPersonTestBO> relationship = GetAssociationRelationship(organisationTestBO, out cpCol);

            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            myBO.Save();
            myBO.FirstName = TestUtil.GetRandomString();
            organisationTestBO.Name = TestUtil.GetRandomString();

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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();

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
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
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
        public virtual void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            RelationshipDef relationshipDef = (RelationshipDef) organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            organisationTestBO.ContactPeople.Add(contactPerson);
            contactPerson.Surname = TestUtil.GetRandomString();
            
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

        /// <summary>
        /// Added child (ie an already persisted object that has been added to the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// </summary>
        [Test]
        public void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves_DB_CompositeKey()
        {
            //---------------Set up test pack-------------------
            TestUsingDatabase.SetupDBDataAccessor();
            Car car = new Car();
            car.Save();

            ContactPersonCompositeKey contactPerson = new ContactPersonCompositeKey();
            contactPerson.PK1Prop1 = TestUtil.GetRandomString();
            contactPerson.PK1Prop2 = TestUtil.GetRandomString();
            contactPerson.Save();

            contactPerson.GetCarsDriven().Add(car);

            //---------------Assert PreConditions---------------            
            Assert.AreEqual(contactPerson.PK1Prop1, car.DriverFK1);
            Assert.AreEqual(contactPerson.PK1Prop2, car.DriverFK2);

            //---------------Execute Test ----------------------
            contactPerson.Save();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            Car loadedCar = Broker.GetBusinessObject<Car>(car.ID);

            //---------------Test Result -----------------------
            Assert.AreEqual(contactPerson.PK1Prop1, loadedCar.DriverFK1);
            Assert.AreEqual(contactPerson.PK1Prop2, loadedCar.DriverFK2);

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
        public virtual void Test_RemovedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef)organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            ContactPersonTestBO contactPerson = organisationTestBO.ContactPeople.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            contactPerson.Surname = TestUtil.GetRandomString();
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
        public virtual void Test_AddDirtyChildrenToTransactionCommitter_AddedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship<ContactPersonTestBO> relationship = organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;

            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            relationship.BusinessObjectCollection.Add(contactPerson);
            SingleRelationship<OrganisationTestBO> reverseRelationship = contactPerson.Relationships.GetSingle<OrganisationTestBO>("Organisation");


            TransactionCommitter tc = (TransactionCommitter) BORegistry.DataAccessor.CreateTransactionCommitter();

            //---------------Assert PreConditions---------------            
            Assert.AreEqual(0, tc.OriginalTransactions.Count);
            Assert.IsNotNull(reverseRelationship);

            //---------------Execute Test ----------------------
            relationship.AddDirtyChildrenToTransactionCommitter(tc);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, tc.OriginalTransactions.Count);

            Assert.IsInstanceOfType(typeof(TransactionalSingleRelationship_Added), tc.OriginalTransactions[0]);
            Assert.AreSame(relationship, ((TransactionalSingleRelationship_Added)tc.OriginalTransactions[0]).Relationship);    
        }

        private static MultipleRelationship<ContactPersonTestBO> GetAssociationRelationship(OrganisationTestBO organisationTestBO, out BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            const RelationshipType relationshipType = RelationshipType.Association;
            MultipleRelationship<ContactPersonTestBO> relationship =
                organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = relationshipType;
            cpCol = relationship.BusinessObjectCollection;
            return relationship;
        }
    }

    [TestFixture]
    public class TestMultipleRelationship_Association_DB : TestMultipleRelationship_Association
    {
        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            TestUsingDatabase.SetupDBDataAccessor();
        }
    }

        [TestFixture]
    public class TestMultipleRelationship_Association_DB4O : TestMultipleRelationship_Association
    {
        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            if (DB4ORegistry.DB != null) DB4ORegistry.DB.Close();
            const string db4oFileStore = "DataStore.db4o";
            if (File.Exists(db4oFileStore)) File.Delete(db4oFileStore);
            DB4ORegistry.DB = Db4oFactory.OpenFile(db4oFileStore);
            BORegistry.DataAccessor = new DataAccessorDB4O(DB4ORegistry.DB);
        }

        /// <summary>
        /// Added child (ie an already persisted object that has been added to the relationship): 
        ///     the related properties (ie those in the relkey) are persisted, and the status of the child is updated.
        /// Note: for DB4O it was decided to make the entire object persist as you cannot simply persist a property or two easily.
        /// </summary>
        [Test]
        public override void Test_AddedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            RelationshipDef relationshipDef = (RelationshipDef)organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            organisationTestBO.ContactPeople.Add(contactPerson);
            contactPerson.Surname = TestUtil.GetRandomString();

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
        /// Note: for DB4O it was decided to make the entire object persist as you cannot simply persist a property or two easily.
        /// </summary>
        [Test]
        public override void Test_RemovedChild_UpdatesRelatedPropertiesOnlyWhenParentSaves()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            RelationshipDef relationshipDef = (RelationshipDef)organisationTestBO.Relationships["ContactPeople"].RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;
            ContactPersonTestBO contactPerson = organisationTestBO.ContactPeople.CreateBusinessObject();
            contactPerson.Surname = TestUtil.GetRandomString();
            contactPerson.FirstName = TestUtil.GetRandomString();
            contactPerson.Save();

            contactPerson.Surname = TestUtil.GetRandomString();
            organisationTestBO.ContactPeople.Remove(contactPerson);

            //---------------Assert PreConditions---------------            
            Assert.IsTrue(contactPerson.Status.IsDirty);
            Assert.IsTrue(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsTrue(organisationTestBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            organisationTestBO.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(contactPerson.Status.IsDirty);
            Assert.IsFalse(contactPerson.Props["OrganisationID"].IsDirty);
            Assert.IsNull(contactPerson.Props["OrganisationID"].Value);
            Assert.IsFalse(organisationTestBO.Status.IsDirty);
        }

        /// Note: for DB4O it was decided to make the entire object persist as you cannot simply persist a property or two easily.
        [Test]
        public override void Test_AddDirtyChildrenToTransactionCommitter_AddedChild()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship<ContactPersonTestBO> relationship = organisationTestBO.Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople");
            RelationshipDef relationshipDef = (RelationshipDef)relationship.RelationshipDef;
            relationshipDef.RelationshipType = RelationshipType.Association;

            ContactPersonTestBO contactPerson = ContactPersonTestBO.CreateSavedContactPerson();
            relationship.BusinessObjectCollection.Add(contactPerson);
            SingleRelationship<OrganisationTestBO> reverseRelationship = contactPerson.Relationships.GetSingle<OrganisationTestBO>("Organisation");


            TransactionCommitter tc = (TransactionCommitter)BORegistry.DataAccessor.CreateTransactionCommitter();

            //---------------Assert PreConditions---------------            
            Assert.AreEqual(0, tc.OriginalTransactions.Count);
            Assert.IsNotNull(reverseRelationship);

            //---------------Execute Test ----------------------
            relationship.AddDirtyChildrenToTransactionCommitter(tc);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, tc.OriginalTransactions.Count);

            Assert.IsInstanceOfType(typeof(TransactionalBusinessObject), tc.OriginalTransactions[0]);
            Assert.IsInstanceOfType(typeof(ContactPersonTestBO), ((TransactionalBusinessObject)tc.OriginalTransactions[0]).BusinessObject);
        }
    }



    
          

}
