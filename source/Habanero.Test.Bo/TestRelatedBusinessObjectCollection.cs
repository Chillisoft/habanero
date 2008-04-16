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
            Assert.IsTrue(relatedBo.State.IsNew);
            Assert.AreEqual(1, col.CreatedBusinessObjects.Count);
        }



        [Test]
        public void TestCreateBusObjectCollectiongetCollectionFromParentMultipleTimes()
        {
            //SetupTests
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            //MultipleRelationship rel = (MultipleRelationship)bo.Relationships["MyMultipleRelationship"];
            //RelatedBusinessObjectCollection<MyRelatedBo> col = new RelatedBusinessObjectCollection<MyRelatedBo>(rel);

            //Run tests
            Address address = contactPersonTestBO.Addresses.CreateBusinessObject();

            //Test results
            Assert.AreEqual(contactPersonTestBO.ContactPersonID, address.ContactPersonID);
            Assert.IsTrue(address.State.IsNew);
            Assert.AreEqual(1, contactPersonTestBO.Addresses.CreatedBusinessObjects.Count);
        }
        [Test]
        public void TestPersistOfCreatedBusinessObjects()
        {

            ContactPersonTestBO.LoadClassDefWithAddressesRelationship();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();
            newCP.FirstName = Guid.NewGuid().ToString();
            cpCol.SaveAll();
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }
        [Test]
        public void TestRemoveRelatedObject()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
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
            ContactPersonTestBO contactPersonTestBO = CreateContactPersonWithOneAddress(out address);

            //-----Run tests----------------------------
            //Run tests
            BusinessObjectCollection<Address> addresses = contactPersonTestBO.Addresses;
            addresses.Remove(address);
            addresses.SaveAll();
            ////-----Test results-------------------------
            Assert.AreEqual(0, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, contactPersonTestBO.Addresses.Count);
        }

        private ContactPersonTestBO CreateContactPersonWithOneAddress(out Address address)
        {
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship();
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            address = contactPersonTestBO.Addresses.CreateBusinessObject();
            address.Save();
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
            return contactPersonTestBO;
        }

        [Test]
        public void TestRemoveGuestAttendee_AlreadyInREmoveCollection()
        {
            //-----Create Test pack---------------------
            Address address;
            ContactPersonTestBO contactPersonTestBO = CreateContactPersonWithOneAddress(out address);

            //-----Run tests----------------------------
            contactPersonTestBO.Addresses.Remove(address);
            contactPersonTestBO.Addresses.Remove(address);
            //eveBooking.GuestAttendees.SaveAll();
            //-----Test results-------------------------
            Assert.AreEqual(1, contactPersonTestBO.Addresses.RemovedBusinessObjects.Count);
        }
    }
}
