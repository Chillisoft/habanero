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

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectCollection
{
    /// <summary>
    /// Tests loading, refreshing and editing a collection of related business objects.
    /// </summary>
    [TestFixture]
    public class TestRelatedBusinessObjectCollection //: TestUsingDatabase
    {

        #region SetupTeardown

        [SetUp]
        public void TestSetup()
        {
            //Code that is run before every single test
            BORegistry.DataAccessor = new DataAccessorInMemory();

        }
        //[TestFixtureSetUp]
        //public void TestFixtureSetup()
        //{
        //    SetupDBConnection();
        //}
        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }

        #endregion

        //Load a collection from the database.
        // Create a new business object.
        // The related collection will now contain the newly added business object.
        // Remove a business object or mark a business object as deleted.
        //  A loaded business object collection will remove the business object from the collection and will
        //    add it to its Deleted Collection.
        // A related collection will be dirty if it has any removed items, created items or deleted items.
        // A related collection will be dirty if it has any dirty objects.
        // A business object will be dirty if it has a dirty related collection.
        [Test]
        public void TestCreateBusObject_AddedToTheCollection()
        {
            //SetupTests
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = new MyBO();
            IMultipleRelationship rel = bo.Relationships.GetMultiple("MyMultipleRelationship");
            RelatedBusinessObjectCollection<MyRelatedBo> col = new RelatedBusinessObjectCollection<MyRelatedBo>(rel);

            //Run tests
            MyRelatedBo relatedBo = col.CreateBusinessObject();

            //Test results
            Assert.AreEqual(bo.MyBoID, relatedBo.MyBoID, "The foreign key should eb set");
            Assert.IsTrue(relatedBo.Status.IsNew);
            Assert.AreEqual(1, col.CreatedBusinessObjects.Count, "The created BOs should be added");
            Assert.AreEqual(0, col.AddedBusinessObjects.Count);
            Assert.AreEqual(1, col.Count);
        }

        [Test]
        public void Test_NewBusObject_Added()
        {
            //Test add new business object adds to created collection and sets the foreign key fields
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = new MyBO();
            IMultipleRelationship rel = bo.Relationships.GetMultiple("MyMultipleRelationship");
            RelatedBusinessObjectCollection<MyRelatedBo> col = new RelatedBusinessObjectCollection<MyRelatedBo>(rel);
            MyRelatedBo relatedBo = new MyRelatedBo();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            col.Add(relatedBo);

            //---------------Test Result -----------------------
            Assert.AreEqual(bo.MyBoID, relatedBo.MyBoID, "The foreign key should eb set");
            Assert.IsTrue(relatedBo.Status.IsNew);
            Assert.AreEqual(1, col.CreatedBusinessObjects.Count, "The created BOs should be added");
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(0, col.AddedBusinessObjects.Count);
        }
        //TODO: Test add new business object adds to created collection and sets the foreign key fields composite .

        [Test]
        public void Test_Refresh_PreservesCreateBusObjectCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO bo = new ContactPersonTestBO();
            RelatedBusinessObjectCollection<AddressTestBO> addresses = bo.Addresses;
            addresses.CreateBusinessObject();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, addresses.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, addresses.Count);
            Assert.AreEqual(0, addresses.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            addresses.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, addresses.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, addresses.Count);
            Assert.AreEqual(0, addresses.PersistedBusinessObjects.Count);
        }

        [Test]
        public void TestCreateBusObjectCollection_ForeignKeySetUp()
        {
            //The Foreign Key (address.ContactPersonId) should be set up to be 
            // equal to the contactPerson.ContactPersonID
            //SetupTests
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //MultipleRelationship rel = (MultipleRelationship)bo.Relationships["MyMultipleRelationship"];
            //RelatedBusinessObjectCollection<MyRelatedBo> col = new RelatedBusinessObjectCollection<MyRelatedBo>(rel);

            //Run tests
            AddressTestBO address = contactPersonTestBO.Addresses.CreateBusinessObject();

            //Test results
            Assert.AreEqual(contactPersonTestBO.ContactPersonID, address.ContactPersonID);
            Assert.IsTrue(address.Status.IsNew);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.CreatedBusinessObjects.Count);
        }

        [Test]
        public void Test_AddNewObject_AddsObjectToCollection_SetsUpForeignKey()
        {
            //---------------Set up test pack-------------------
            //The Foreign Key (address.ContactPersonId) should be set up to be 
            // equal to the contactPerson.ContactPersonID
            //SetupTests
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();

            //Run tests
            AddressTestBO address = new AddressTestBO();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            contactPersonTestBO.Addresses.Add(address);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.AddedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.PersistedBusinessObjects.Count);
            Assert.AreEqual(contactPersonTestBO.ContactPersonID, address.ContactPersonID);
        }

        [Test]
        public void Test_AddPersistedObject_AddsObjectToCollection_SetsUpForeignKey()
        {
            //---------------Set up test pack-------------------
            //The Foreign Key (address.ContactPersonId) should be set up to be 
            // equal to the contactPerson.ContactPersonID
            //SetupTests
//            ContactPersonTestBO.LoadClassDefWithAddresBOsRelationship_AddressReverseRelationshipConfigured();

            //Run tests
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO.CreateContactPersonWithOneAddressTestBO(out address);
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, contactPersonTestBO.AddressTestBOs.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.AddressTestBOs.Count);
            Assert.AreEqual(0, contactPersonTestBO.AddressTestBOs.PersistedBusinessObjects.Count);
            Assert.IsNotNull(address.ContactPersonTestBO);
            Assert.IsNotNull(address.ContactPersonID);

            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<AddressTestBO> addressTestBOS = contactPersonTestBO.AddressTestBOs;
            addressTestBOS.Add(address);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, addressTestBOS.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, addressTestBOS.AddedBusinessObjects.Count);
            Assert.AreEqual(1, addressTestBOS.Count);
            Assert.AreEqual(0, addressTestBOS.PersistedBusinessObjects.Count);
            Assert.AreEqual(contactPersonTestBO.ContactPersonID, address.ContactPersonID);
            Assert.AreSame(contactPersonTestBO, address.ContactPersonTestBO);
        }

        [Test]
        public void Test_AddPersistedObject_AddsTo_AddedCollection()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO.CreateContactPersonWithOneAddressTestBO(out address);
            ContactPersonTestBO newContactPerson = new ContactPersonTestBO();
            RelatedBusinessObjectCollection<AddressTestBO> addressTestBOS = newContactPerson.AddressTestBOs;
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, addressTestBOS.AddedBusinessObjects.Count);
            Assert.AreEqual(0, addressTestBOS.Count);
            Assert.AreEqual(0, addressTestBOS.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            addressTestBOS = newContactPerson.AddressTestBOs;
            addressTestBOS.Add(address);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, addressTestBOS.AddedBusinessObjects.Count);
            Assert.AreEqual(0, addressTestBOS.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, addressTestBOS.Count);
            Assert.AreEqual(0, addressTestBOS.PersistedBusinessObjects.Count);
        }
        //TODO: With composite keys should set up foreign key again when parent bo is saved in case
        //  parent foreign key edited or child added before parent foreign key is set.
        // other option which is probably better is that the foreign key props reference the actual
        //  props of the parents.

        [Test]
        public void Test_AddPersistedObject_AddsObjectToCollection_SurvivesRefresh()
        {
            //---------------Set up test pack-------------------
            //The Foreign Key (address.ContactPersonId) should be set up to be 
            // equal to the contactPerson.ContactPersonID
            //Test that using relationship from contact person so that overcome issues 
            //   with reloading all the time.
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO.CreateContactPersonWithOneAddressTestBO(out address);
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();

            RelatedBusinessObjectCollection<AddressTestBO> addressTestBOS = contactPersonTestBO.AddressTestBOs;
            addressTestBOS.Add(address);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, addressTestBOS.AddedBusinessObjects.Count);
            Assert.AreEqual(1, addressTestBOS.Count);
            Assert.AreEqual(0, addressTestBOS.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            addressTestBOS.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, addressTestBOS.AddedBusinessObjects.Count);
            Assert.AreEqual(1, addressTestBOS.Count);
            Assert.AreEqual(0, addressTestBOS.PersistedBusinessObjects.Count);
        }

        //TODO: Do all these tests with composite foreign keys (i.e. add, create and remove related
        //  bo and ensure that foreign key and related bo's set up correctly.

        //TODO: Should remove itself from existing relationship collection

        //TODO: does not put in added collection if already related to collection.

        //Remove sets foreign key to null or deletes depending upon strategy.
        [Test]
        public void Test_AddPersistedObject_ForeignKeyMatchesCollection_NotAddsObjectToCollection()
        {
            //---------------Set up test pack-------------------
            //The Foreign Key (address.ContactPersonId) should be set up to be 
            // equal to the contactPerson.ContactPersonID
            //Test that using relationship from contact person so that overcome issues 
            //   with reloading all the time.
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO.CreateContactPersonWithOneAddressTestBO(out address);
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();


            RelatedBusinessObjectCollection<AddressTestBO> addressTestBOS = contactPersonTestBO.AddressTestBOs;

//            address.ContactPersonTestBO = contactPersonTestBO;
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, addressTestBOS.AddedBusinessObjects.Count);
            Assert.AreEqual(0, addressTestBOS.Count);
            Assert.AreEqual(0, addressTestBOS.PersistedBusinessObjects.Count);
//            Assert.AreEqual(contactPersonTestBO.ContactPersonID, address.ContactPersonID);
            //Assert.AreSame(contactPersonTestBO, address.ContactPersonTestBO);
            Assert.IsFalse(address.Status.IsDirty);
            Assert.IsFalse(address.Status.IsNew);

            //---------------Execute Test ----------------------
            addressTestBOS.Add(address);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, addressTestBOS.Count);
            Assert.AreEqual(1, addressTestBOS.AddedBusinessObjects.Count);
            Assert.AreEqual(0, addressTestBOS.PersistedBusinessObjects.Count);
            Assert.AreEqual(contactPersonTestBO.ContactPersonID, address.ContactPersonID);
            Assert.AreSame(contactPersonTestBO, address.ContactPersonTestBO);
        }

        [Test]
        public void TestRemoveRelatedObject()
        {
            //-----Create Test pack---------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            RelatedBusinessObjectCollection<AddressTestBO> addresses1 = contactPersonTestBO.Addresses;
            AddressTestBO address = addresses1.CreateBusinessObject();
            address.Save();

            //------Assert Preconditions
            Assert.AreEqual(0, addresses1.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, addresses1.Count);
            Assert.AreEqual(1, addresses1.PersistedBusinessObjects.Count);

            //-----Run tests----------------------------
            RelatedBusinessObjectCollection<AddressTestBO> addresses = addresses1;
            addresses.Remove(address);

            ////-----Test results-------------------------
            Assert.AreEqual(1, addresses1.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, addresses1.Count);
            Assert.AreEqual(1, addresses1.PersistedBusinessObjects.Count);
            Assert.IsNull(address.ContactPersonTestBO);
            Assert.IsNull(address.ContactPersonID);
        }

        [Test]
        public void TestRemoveRelatedObject_usingRelationship()
        {
            //-----Create Test pack---------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            AddressTestBO address = contactPersonTestBO.Addresses.CreateBusinessObject();
            address.Save();
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //------Assert Preconditions
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.PersistedBusinessObjects.Count);

            //-----Run tests----------------------------
            RelatedBusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);

            ////-----Test results-------------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.PersistedBusinessObjects.Count);
            Assert.IsNull(address.ContactPersonTestBO);
            Assert.IsNull(address.ContactPersonID);
        }

        [Test]
        public void TestRemoveRelatedObject_AsBusinessObjectCollection()
        {
            //-----Create Test pack---------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            AddressTestBO address = contactPersonTestBO.Addresses.CreateBusinessObject();
            address.Save();
            //-------Assert Precondition ------------------------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //-----Run tests----------------------------
            BusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);

            ////-----Test results-------------------------
            Assert.AreEqual(1, addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, addresses.Count);
            Assert.IsNull(address.ContactPersonID);
            Assert.IsNull(address.ContactPersonTestBO);
//            Assert.IsTrue(address.Status.IsDeleted);
            Assert.AreEqual(1, addresses.PersistedBusinessObjects.Count);
        }

//        [Test]
//        public void TestRemoveRelatedObject_AsBusinessObjectCollection_WithRelationshipRefreshing()
//        {
//            //-----Create Test pack---------------------
//            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
//            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
//            AddressTestBO address = contactPersonTestBO.Addresses.CreateBusinessObject();
//            address.Save();
//            //-------Assert Precondition ------------------------------------
//            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
//
//            //-----Run tests----------------------------
//            BusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
//            addresses.Remove(address);
//
//            ////-----Test results-------------------------
        //            Assert.AreEqual(1, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
//            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
//            Assert.IsNull(address.ContactPersonID);
//            Assert.IsNull(address.ContactPersonTestBO);
//            Assert.IsTrue(address.Status.IsDeleted);
        //            Assert.AreEqual(1, contactPersonTestBO.Addresses.PersistedBOCol.Count);
//        }

        [Test]
        public void TestRemoveRelatedObject_PersistColToDataStore()
        {
            //-----Create Test pack---------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            
            //-----Assert Precondition------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //-----Run tests----------------------------
            //Run tests
            BusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);
            addresses.SaveAll();

            //-----Test results-------------------------
            Assert.AreEqual(0, addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, addresses.Count);
            Assert.AreEqual(0, addresses.PersistedBusinessObjects.Count);
        }

        [Test]
        public void TestRemoveRelatedObject_PersistColToDataStore_usingRelationshipRefreshing()
        {
            //-----Create Test pack---------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            //-----Assert Precondition------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //-----Run tests----------------------------
            //Run tests
            BusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);
            addresses.SaveAll();

            ////-----Test results-------------------------
            Assert.IsFalse(address.Status.IsDirty);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);

            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.PersistedBusinessObjects.Count);
        }

        [Test]
        public void TestRemoveRelatedObject_PersistBOToDataStore_usngRelationshipRefreshing()
        {
            //-----Create Test pack---------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            //-----Assert Precondition------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //-----Run tests----------------------------
            BusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);
            address.Save();

            ////-----Test results-------------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.PersistedBusinessObjects.Count);
        }


        [Test]
        public void TestRemoveRelatedObject_PersistBOToDataStore()
        {
            //-----Create Test pack---------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            //-----Assert Precondition------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //-----Run tests----------------------------
            BusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);
            address.Save();

            ////-----Test results-------------------------
            Assert.AreEqual(0, addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, addresses.Count);
            Assert.AreEqual(0, addresses.PersistedBusinessObjects.Count);
        }

        [Test]
        public void TestRemoveAddress_AlreadyInRemoveCollection_usingRelationship()
        {
            //-----Create Test pack---------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            //-----Run tests----------------------------
            contactPersonTestBO.Addresses.Remove(address);
            contactPersonTestBO.Addresses.Remove(address);

            //-----Test results-------------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);

        }

        [Test]
        public void TestRemoveAddress_AlreadyInRemoveCollection()
        {
            //-----Create Test pack---------------------
            ClassDef.ClassDefs.Clear();
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            RelatedBusinessObjectCollection<AddressTestBO> addresses = contactPersonTestBO.Addresses;

            //-----Run tests----------------------------
            addresses.Remove(address);
            addresses.Remove(address);

            //-----Test results-------------------------
            Assert.AreEqual(1, addresses.RemovedBusinessObjects.Count);
        }
        //TODO: Test remove dereferences new bo and persisted bo

        [Test]
        public void Test_TestCreatedChildBO_ParentSet()
        {
            //Test that the parent business object is set for a 
            // child bo that is created by the collection
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefWithAddresBOsRelationship_AddressReverseRelationshipConfigured();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();

            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            AddressTestBO address = contactPersonTestBO.AddressTestBOs.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(address.ContactPersonTestBO);
            Assert.IsNotNull(address.ContactPersonID);
        }
        [Test]
        public void Test_TestCreatedChildBO_ParentNotPersisted_ParentSet()
        {
            //Test that the parent business object is set for a 
            // child bo that is created by the collection
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefWithAddresBOsRelationship_AddressReverseRelationshipConfigured();
            ContactPersonTestBO contactPersonTestBO = 
                ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(),TestUtil.CreateRandomString());

            //---------------Assert Precondition----------------
            
            //---------------Execute Test ----------------------
            AddressTestBO address = contactPersonTestBO.AddressTestBOs.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.IsNotNull(address.ContactPersonID);
            Assert.IsNotNull(address.ContactPersonTestBO);
        }
        [Test]
        public void Test_TestAddNewChildBO_ParentNotPersisted_ParentSet()
        {
            //Test that the parent business object is set for a 
            // child bo that is created by the collection
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefWithAddresBOsRelationship_AddressReverseRelationshipConfigured();
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            AddressTestBO address = new AddressTestBO();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            contactPersonTestBO.AddressTestBOs.Add(address);

            //---------------Test Result -----------------------
            Assert.IsNotNull(address.ContactPersonID);
            Assert.IsNotNull(address.ContactPersonTestBO);
        }
        //TODO: Add non new object should set up foreign keys and reverse related object
        //TODO: what must we do if you add a business object to a relationship but the foreign key does not match?
        // Possibly this is a strategy must look at this so that extendable
        // (see similar issue for remove below)


        [Test]
        public void Test_TestGetReverseRelationship()
        {
            //This is probably a temporary test as this method is hacked together due
            // to the fact that reverse relationships are not currently defined In Habanero.
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadClassDefWithAddresBOsRelationship_AddressReverseRelationshipConfigured();
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateUnsavedContactPerson(TestUtil.CreateRandomString(), TestUtil.CreateRandomString());

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            AddressTestBO address = contactPersonTestBO.AddressTestBOs.CreateBusinessObject();

            //---------------Test Result -----------------------
            IRelationship relationship = contactPersonTestBO.AddressTestBOs.GetReverseRelationship(address);
            Assert.IsNotNull(relationship);
            Assert.AreSame(address.Relationships["ContactPersonTestBO"], relationship);
        }
    }
}