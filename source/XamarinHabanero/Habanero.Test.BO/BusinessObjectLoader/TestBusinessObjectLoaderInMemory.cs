#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.Base.Exceptions;
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

            FixtureEnvironment.ClearBusinessObjectManager();

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
            ContactPersonTestBO.LoadDefaultClassDef();
            DateTime now = DateTime.Now;
            var cp1 = CreateContactPerson(now);
            var cp2 = CreateContactPerson(now);
            var criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            BusinessObjectCollection<ContactPersonTestBO> col =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>
                    (criteria);

            _dataStore.Remove(cp2);
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.Contains(cp1, col);
            //---------------Tear Down -------------------------
        }

        private ContactPersonTestBO CreateContactPerson(DateTime now)
        {
            ContactPersonTestBO cp1 = new ContactPersonTestBO();
            cp1.DateOfBirth = now;
            cp1.Surname = Guid.NewGuid().ToString("N");
            cp1.Save();
            return cp1;
        }

        [Test]
        public void Test_Refresh_WithDuplicateObjectsInPersistedCollection_ShouldThrowHabaneroDeveloperException()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            bo.Save();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO>();
            collection.Load("MyBoID = '" + bo.MyBoID + "'", "");
            collection.PersistedBusinessObjects.Add(bo);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, collection.PersistedBusinessObjects.Count);
            Assert.AreSame(collection.PersistedBusinessObjects[0], collection.PersistedBusinessObjects[1]);
            //---------------Execute Test ----------------------
            try
            {
                collection.Refresh();
                //---------------Test Result -----------------------
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<HabaneroDeveloperException>(ex, "Should have thrown a HabaneroDeveloperException because of the duplicate item in the PersistedBusinessObjects collection");
                StringAssert.Contains("A duplicate Business Object was found in the persisted objects collection of the BusinessObjectCollection during a reload", ex.Message);
                StringAssert.Contains("MyBO", ex.Message);
                StringAssert.Contains(bo.MyBoID.Value.ToString("B"), ex.Message);
            }

        }

    }
}