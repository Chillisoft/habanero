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
        [Test]
        public void TestCreateBusObjectCollection()
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
        }

        [Test]
        public void TestCreateBusObjectCollectiongetCollectionFromParentMultipleTimes()
        {
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
        public void TestPersistOfCreatedBusinessObjects()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();
            newCP.FirstName = Guid.NewGuid().ToString();

            //---------------Execute Test ----------------------
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
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

            //-----Run tests----------------------------
            //Run tests

            BusinessObjectCollection<Address> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);
            ////-----Test results-------------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
        }



        [Test]
        public void TestRemoveRelatedObject_PersistToDB()
        {
            //-----Create Test pack---------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            //-----Run tests----------------------------
            //Run tests
            BusinessObjectCollection<Address> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);
            addresses.SaveAll();
            ////-----Test results-------------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
        }



        [Test]
        public void TestRemoveGuestAttendee_AlreadyInRemoveCollection()
        {
            //-----Create Test pack---------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);

            //-----Run tests----------------------------
            contactPersonTestBO.Addresses.Remove(address);
            contactPersonTestBO.Addresses.Remove(address);
            //eveBooking.GuestAttendees.SaveAll();
            //-----Test results-------------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
        }


    }
}
