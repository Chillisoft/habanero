using System;
using Habanero.Base;
using Habanero.BO;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectLoader
{
    [TestFixture]
    public class TestBusinessObjectLoaderInMemory : TestBusinessObjectLoader
    {
        private DataStoreInMemory _dataStore;

        protected override void SetupDataAccessor()
        {
            _dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(_dataStore);
        }

        protected override void DeleteEnginesAndCars()
        {
            // do nothing
        }

        [Test]
        public void Test_ReturnSameObjectFromBusinessObjectLoader()
        {
            //---------------Set up test pack-------------------
            //------------------------------Setup Test
            new Engine();
            new Car();
            ContactPerson originalContactPerson = new ContactPerson();
            originalContactPerson.Surname = "FirstSurname";
            originalContactPerson.Save();

            BORegistry.BusinessObjectManager.ClearLoadedObjects();

            //load second object from DB to ensure that it is now in the object manager
            ContactPerson myContact2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                    (originalContactPerson.ID);

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ContactPerson myContact3 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                    (originalContactPerson.ID);

            //---------------Test Result -----------------------
//                Assert.AreNotSame(originalContactPerson, myContact3);
            Assert.AreSame(myContact2, myContact3);
        }

        [Test]
        public void TestRefreshLoadedCollection_RemovedItem()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            ContactPersonTestBO cp2 = new ContactPersonTestBO();
            cp2.DateOfBirth = now;
            cp2.Surname = Guid.NewGuid().ToString("N");
            cp2.Save();
            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (criteria);

            dataStore.Remove(cp2);
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp1, col);
            //---------------Tear Down -------------------------
        }
    }
}