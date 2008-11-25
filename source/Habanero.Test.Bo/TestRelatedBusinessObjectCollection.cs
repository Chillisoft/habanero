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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestRelatedBusinessObjectCollection : TestUsingDatabase
    {
        [SetUp]
        public void TestSetup()
        {
            //Code that is run before every single test
            ClassDef.ClassDefs.Clear();
            BORegistry.DataAccessor = new DataAccessorDB();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            SetupDBConnection();
        }
        [TearDown]
        public void TestTearDown()
        {
            //Code that is executed after each and every test is executed in this fixture/class.
        }
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
            MyBO.LoadClassDefWithRelationship();
            MyRelatedBo.LoadClassDef();
            MyBO bo = new MyBO();
            MultipleRelationship rel = (MultipleRelationship) bo.Relationships["MyMultipleRelationship"];
            RelatedBusinessObjectCollection<MyRelatedBo> col = new RelatedBusinessObjectCollection<MyRelatedBo>(rel);

            //Run tests
            MyRelatedBo relatedBo = col.CreateBusinessObject();

            //Test results
            Assert.AreEqual(bo.MyBoID, relatedBo.MyBoID);
            Assert.IsTrue(relatedBo.Status.IsNew);
            Assert.AreEqual(1, col.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, col.Count);
        }
        //TODO: Test add new business object adds to created collection and sets the foreign key fields
        // composite and non composite.


//TODO: I think that the collection should show all loaded object less removed or deleted object not yet persisted
//     plus all created or added objects not yet persisted.

        [Test]
        public void Test_Refresh_PreservesCreateBusObjectCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO bo = new ContactPersonTestBO();
            RelatedBusinessObjectCollection<Address> addresses = bo.Addresses;
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
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //MultipleRelationship rel = (MultipleRelationship)bo.Relationships["MyMultipleRelationship"];
            //RelatedBusinessObjectCollection<MyRelatedBo> col = new RelatedBusinessObjectCollection<MyRelatedBo>(rel);

            //Run tests
            Address address = contactPersonTestBO.Addresses.CreateBusinessObject();

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
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //MultipleRelationship rel = (MultipleRelationship)bo.Relationships["MyMultipleRelationship"];
            //RelatedBusinessObjectCollection<MyRelatedBo> col = new RelatedBusinessObjectCollection<MyRelatedBo>(rel);

            //Run tests
            Address address = new Address();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.PersistedBOColl.Count);

            //---------------Execute Test ----------------------
            contactPersonTestBO.Addresses.Add(address);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.PersistedBOColl.Count);
            Assert.AreEqual(contactPersonTestBO.ContactPersonID, address.ContactPersonID);
        }


        [Test]
        public void TestRemoveRelatedObject()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address = contactPersonTestBO.Addresses.CreateBusinessObject();
            address.Save();
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
            //------Assert Preconditions
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.PersistedBOColl.Count);
            //-----Run tests----------------------------
            RelatedBusinessObjectCollection<Address> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);

            ////-----Test results-------------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.PersistedBOColl.Count);
        }

        [Test]
        public void TestRemoveRelatedObject_AsBusinessObjectCollection()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            Address address = contactPersonTestBO.Addresses.CreateBusinessObject();
            address.Save();
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //-----Run tests----------------------------
            BusinessObjectCollection<Address> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);

            ////-----Test results-------------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.IsNull(address.ContactPersonID);
            Assert.IsNull(address.ContactPerson);
            Assert.IsTrue(address.Status.IsDeleted);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.PersistedBOColl.Count);
        }

        [Test]
        public void TestRemoveRelatedObject_PersistColToDB()
        {
            //-----Create Test pack---------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            
            //-----Assert Precondition------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //-----Run tests----------------------------
            //Run tests
            BusinessObjectCollection<Address> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);
            addresses.SaveAll();

            ////-----Test results-------------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.IsTrue(address.Status.IsDeleted);
            Assert.IsTrue(address.Status.IsNew);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.PersistedBOColl.Count);
        }

        [Test]
        public void TestRemoveRelatedObject_PersistBOToDB()
        {
            //-----Create Test pack---------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            //-----Assert Precondition------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);

            //-----Run tests----------------------------
            BusinessObjectCollection<Address> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);
            address.Save();

            ////-----Test results-------------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
            Assert.IsTrue(address.Status.IsDeleted);
            Assert.IsTrue(address.Status.IsNew);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.PersistedBOColl.Count);
        }

        [Test]
        public void TestRemoveAddress_AlreadyInRemoveCollection()
        {
            //-----Create Test pack---------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            //-----Run tests----------------------------
            contactPersonTestBO.Addresses.Remove(address);
            contactPersonTestBO.Addresses.Remove(address);

            //-----Test results-------------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
        }
    }
}
