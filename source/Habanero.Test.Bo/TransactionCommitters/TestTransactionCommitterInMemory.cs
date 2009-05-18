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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO.TransactionCommitters
{
    [TestFixture]
    public class TestTransactionCommitterInMemory
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();


        }
        [TearDown]
        public void TearDown()
        {
            
        }

        [Test]
        public void TestInsert()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp = GetContactPerson();

            DataStoreInMemory dataStore = new DataStoreInMemory();
            IBusinessObjectLoader loader = new BusinessObjectLoaderInMemory(dataStore);
            ITransactionCommitter transactionCommitter = new TransactionCommitterInMemory(dataStore);
            //---------------Execute Test ----------------------
            transactionCommitter.AddBusinessObject(cp);
            transactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreSame(cp, loader.GetBusinessObject<ContactPersonTestBO>(cp.ID));
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestUpdate()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp = GetContactPerson();

            DataStoreInMemory dataStore = new DataStoreInMemory();
            IBusinessObjectLoader loader = new BusinessObjectLoaderInMemory(dataStore);
            ITransactionCommitter firstTransactionCommitter = new TransactionCommitterInMemory(dataStore);
            firstTransactionCommitter.AddBusinessObject(cp);
            firstTransactionCommitter.CommitTransaction();

            //---------------Execute Test ----------------------
            cp.Surname = Guid.NewGuid().ToString("N");
            ITransactionCommitter secondTransactionCommitter = new TransactionCommitterInMemory(dataStore);
            secondTransactionCommitter.AddBusinessObject(cp);
            secondTransactionCommitter.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataStore.Count);
            Assert.AreSame(cp, loader.GetBusinessObject<ContactPersonTestBO>(cp.ID));
            Assert.IsFalse(cp.Status.IsDirty);
            //---------------Tear Down -------------------------
        }

        private static ContactPersonTestBO GetContactPerson()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            return cp;
        }

        [Test]
        public void TestDelete()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp = GetContactPerson();

            DataStoreInMemory dataStore = new DataStoreInMemory();
            ITransactionCommitter firstTransactionCommitter = new TransactionCommitterInMemory(dataStore);
            firstTransactionCommitter.AddBusinessObject(cp);
            firstTransactionCommitter.CommitTransaction();

            //---------------Assert Preconditions--------------
            Assert.AreEqual(1, dataStore.Count);

            //---------------Execute Test ----------------------
            cp.MarkForDelete();
            ITransactionCommitter secondTransactionCommitter = new TransactionCommitterInMemory(dataStore);
            secondTransactionCommitter.AddBusinessObject(cp);
            secondTransactionCommitter.CommitTransaction();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, dataStore.Count);
//            Assert.IsNull(loader.GetBusinessObject<ContactPersonTestBO>(cp.PrimaryKey));

        }

        [Test, ExpectedException(typeof(BusObjPersistException))]
        public void TestPreventDelete()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStoreInMemory = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStoreInMemory);
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_PreventDelete(out address);
//            contactPersonTestBO.MarkForDelete();
            ReflectionUtilities.SetPropertyValue(contactPersonTestBO.Status, "IsDeleted", true);
            ITransactionCommitter committer = new TransactionCommitterInMemory(dataStoreInMemory);
            committer.AddBusinessObject(contactPersonTestBO);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
        }

        [Test]
        public void TestCheckForDuplicate()
        {

            DataStoreInMemory dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
            ContactPersonTestBO.LoadClassDefWithCompositeAlternateKey();
            ContactPersonTestBO contactPerson = GetSavedContactPerson(dataStore);
            ContactPersonTestBO duplicateContactPerson = new ContactPersonTestBO();
            duplicateContactPerson.Surname = contactPerson.Surname;
            duplicateContactPerson.FirstName = contactPerson.FirstName;
            TransactionCommitterInMemory committer = new TransactionCommitterInMemory(dataStore);
            committer.AddBusinessObject(duplicateContactPerson);
            //---------------Execute Test ----------------------
            try
            {
                committer.CommitTransaction();
                Assert.Fail("Commit should have failed due to duplicate key violation");
            }
            //---------------Test Result -----------------------
            catch (BusObjDuplicateConcurrencyControlException ex)
            {
                StringAssert.Contains("Surname", ex.Message);
                StringAssert.Contains("FirstName", ex.Message);
            }
           
        }

        [Test]
        public void TestDeleteRelated()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
            AddressTestBO address;
            ContactPersonTestBO contactPersonTestBO =
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address);
            contactPersonTestBO.MarkForDelete();
            TransactionCommitterInMemory committer = new TransactionCommitterInMemory(dataStore);
            committer.AddBusinessObject(contactPersonTestBO);

            //---------------Execute Test ----------------------
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            AssertBOStateIsValidAfterDelete(address);

            AssertBusinessObjectNotInDataStore(contactPersonTestBO);
            AssertBusinessObjectNotInDataStore( address);
        }


        private static ContactPersonTestBO GetSavedContactPerson(DataStoreInMemory dataStore)
        {
            ContactPersonTestBO contactPersonCompositeKey = GetUnsavedContactPerson();
            TransactionCommitterInMemory committer = new TransactionCommitterInMemory(dataStore);
            committer.AddBusinessObject(contactPersonCompositeKey);
            committer.CommitTransaction();
            return contactPersonCompositeKey;
        }

        private static ContactPersonTestBO GetUnsavedContactPerson()
        {
            ContactPersonTestBO contactPersonCompositeKey = new ContactPersonTestBO();
            contactPersonCompositeKey.Surname = "Somebody";
            contactPersonCompositeKey.FirstName = "Else";
            return contactPersonCompositeKey;
        }

        private static void AssertBusinessObjectNotInDataStore(IBusinessObject bo)
        {
            try
            {
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(bo.ClassDef, bo.ID);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                StringAssert.Contains("A Error has occured since the object you are trying to refresh has been ", ex.Message);
            }
        }

        private static void AssertBOStateIsValidAfterDelete(IBusinessObject bo)
        {
            Assert.IsTrue(bo.Status.IsNew);
            Assert.IsTrue(bo.Status.IsDeleted);
        }

        [Test]
        public void TestDereferenceRelatedObjects()
        {
            //The Car has a single relationship to engine. The car->engine relationship is marked 
            // as a dereference related relationship.
            BORegistry.DataAccessor = new DataAccessorInMemory();
            //---------------Set up test pack-------------------

            Car car = new Car();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.Save();

            Engine engine = new Engine();

            engine.SetPropertyValue("EngineNo", "NO111");
            const string carIDProp = "CarID";
            engine.SetPropertyValue(carIDProp, car.GetPropertyValue(carIDProp));
            engine.Save();

            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(engine);
            Assert.AreSame(engine.GetCar(), car);

            //---------------Execute Test ----------------------
            car.MarkForDelete();
            car.Save();

            //---------------Test Result -----------------------
            Assert.IsNull(engine.GetPropertyValue(carIDProp));
            Assert.IsNull(engine.GetCar());
            //---------------Test TearDown -----------------------
        }


    }
}