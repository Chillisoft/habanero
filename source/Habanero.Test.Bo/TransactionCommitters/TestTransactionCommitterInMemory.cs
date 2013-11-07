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
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;
using Habanero.Test.Structure;
using Habanero.Util;
using NUnit.Framework;
using Rhino.Mocks;

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

        [Test]
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
            try
            {
                committer.CommitTransaction();
                Assert.Fail("Expected to throw an BusObjPersistException");
            }
                //---------------Test Result -----------------------
            catch (BusObjPersistException ex)
            {
                StringAssert.Contains("You cannot delete ContactPersonTestBO", ex.Message);
            }
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
                ContactPersonTestBO.CreateContactPersonWithOneAddress_CascadeDelete(out address, TestUtil.GetRandomString());
            contactPersonTestBO.MarkForDelete();
            TransactionCommitterInMemory committer = new TransactionCommitterInMemory(dataStore);
            committer.AddBusinessObject(contactPersonTestBO);

            //---------------Execute Test ----------------------
            committer.CommitTransaction();

            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(contactPersonTestBO);
            AssertBOStateIsValidAfterDelete(address);

            AssertBusinessObjectNotInDataStore(contactPersonTestBO);
            AssertBusinessObjectNotInDataStore(address);
        }

        [Test]
        public void TestDeleteRelated_WhenCircularDelete_ShouldResolve()
        {
            //---------------Set up test pack-------------------
            DataStoreInMemory dataStore = new DataStoreInMemory();
            BORegistry.DataAccessor = new DataAccessorInMemory(dataStore);
            Entity.LoadDefaultClassDef_WithCircularDeleteRelatedToSelf();
            Entity entity1 = new Entity();
            Entity entity2 = new Entity();
            entity1.Relationships.SetRelatedObject("RelatedEntity", entity2);
            entity2.Relationships.SetRelatedObject("RelatedEntity", entity1);
            entity1.Save();
            entity2.Save();
            entity1.MarkForDelete();
            TransactionCommitterInMemory committer = new TransactionCommitterInMemory(dataStore);
            committer.AddBusinessObject(entity1);
            //---------------Execute Test ----------------------
            committer.CommitTransaction();
            //---------------Test Result -----------------------
            AssertBOStateIsValidAfterDelete(entity1);
            AssertBOStateIsValidAfterDelete(entity2);

            AssertBusinessObjectNotInDataStore(entity1);
            AssertBusinessObjectNotInDataStore(entity2);
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

        [Test]
        public void Test_DereferenceRelatedObjects_ForSingle_NonOwner_WhenHasNoRelatedBO()
        {
            //The Car has a single relationship to engine. The car->engine relationship is marked 
            // as a dereference related relationship.
            BORegistry.DataAccessor = new DataAccessorInMemory();
            //---------------Set up test pack-------------------
            Car car = new Car();
            car.SetPropertyValue("CarRegNo", "NP32459");
            car.Save();
            car.MarkForDelete();
            new Engine();

            //---------------Assert Precondition----------------
            SingleRelationshipDef relationshipDef = (SingleRelationshipDef)car.Relationships["Engine"].RelationshipDef;
            Assert.IsFalse(relationshipDef.OwningBOHasForeignKey);
            Assert.AreEqual(DeleteParentAction.DereferenceRelated, relationshipDef.DeleteParentAction);
            Assert.IsNull(car.GetEngine());
            //---------------Execute Test ----------------------
            car.Save();
            //---------------Test Result -----------------------
            Assert.IsNull(car.GetEngine());
            Assert.IsTrue(car.Status.IsNew && car.Status.IsDeleted);
        }

        [Test]
        public void Test_DereferenceRelatedObjects_ForSingle_IsOwner_WhenHasNoRelatedBO()
        {
            //The Car has a single relationship to engine. The car->engine relationship is marked 
            // as a dereference related relationship.
            BORegistry.DataAccessor = new DataAccessorInMemory();
            //---------------Set up test pack-------------------
            new Car();
            Engine engine = new Engine();
            engine.SetPropertyValue("EngineNo", "NO111");
            engine.Save();
            engine.MarkForDelete();
            SingleRelationshipDef relationshipDef = (SingleRelationshipDef)engine.Relationships["Car"].RelationshipDef;
            relationshipDef.DeleteParentAction = DeleteParentAction.DereferenceRelated;
            //---------------Assert Precondition----------------
            Assert.IsTrue(relationshipDef.OwningBOHasForeignKey);
            Assert.AreEqual(DeleteParentAction.DereferenceRelated, relationshipDef.DeleteParentAction);
            Assert.IsNull(engine.GetCar());
            //---------------Execute Test ----------------------
            engine.Save();
            //---------------Test Result -----------------------
            Assert.IsNull(engine.GetCar());
            Assert.IsTrue(engine.Status.IsNew && engine.Status.IsDeleted);
        }


        [Test]
        public void Test_CommitTransaction_WithAutoIncrementBo_ShouldAutoIncrementAfterInsert()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID(); 
            
            DataStoreInMemory dataStore = new DataStoreInMemory();
            ITransactionCommitter transactionCommitter = new TransactionCommitterInMemory(dataStore);

            TestAutoInc bo = new TestAutoInc();
            bo.SetPropertyValue("testfield", "testing 123");
            transactionCommitter.AddBusinessObject(bo);
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.TestAutoIncID.HasValue);
            //---------------Execute Test ----------------------
            transactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            Assert.IsNotNull(bo.TestAutoIncID);
            Assert.AreNotEqual(0, bo.TestAutoIncID);
            Assert.IsFalse(bo.Status.IsDirty);
        }

        [Test]
        public void Test_CommitTransaction_WithAutoIncrementBo_ShouldUseNumberGeneratorsInDatastore()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = TestAutoInc.LoadClassDefWithAutoIncrementingID();

            DataStoreInMemory dataStore = MockRepository.GeneratePartialMock<DataStoreInMemory>();
            dataStore.Replay();

            int nextAutoIncNumber = TestUtil.GetRandomInt();

            dataStore.Stub(t => t.GetNextAutoIncrementingNumber(classDef)).Return(nextAutoIncNumber);

            ITransactionCommitter transactionCommitter = new TransactionCommitterInMemory(dataStore);
            
            TestAutoInc bo = new TestAutoInc();
            bo.SetPropertyValue("testfield", "testing 123");
            transactionCommitter.AddBusinessObject(bo);
            //---------------Assert Precondition----------------
            Assert.IsFalse(bo.TestAutoIncID.HasValue);
            Assert.AreSame(classDef, bo.ClassDef);
            Assert.IsTrue(bo.Props.HasAutoIncrementingField);
            //---------------Execute Test ----------------------
            transactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            dataStore.AssertWasCalled(memory => memory.GetNextAutoIncrementingNumber(classDef));
            Assert.IsNotNull(bo.TestAutoIncID);
            Assert.AreEqual(nextAutoIncNumber, bo.TestAutoIncID);
            Assert.IsFalse(bo.Status.IsDirty);
        }

        [Test]
        public void Test_CommitTransaction_NonAutoIncrementingBo_ShouldNotCall_GetNextAutoIncrementingNumber()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO cp = GetContactPerson();
            ClassDef classDef = cp.ClassDef;

            DataStoreInMemory dataStore = MockRepository.GeneratePartialMock<DataStoreInMemory>();
            dataStore.Replay();
            dataStore.Stub(t => t.GetNextAutoIncrementingNumber(classDef)).Return(1);
            
            ITransactionCommitter transactionCommitter = new TransactionCommitterInMemory(dataStore);
            transactionCommitter.AddBusinessObject(cp);
            //---------------Assert Precondition----------------
            Assert.AreSame(classDef, cp.ClassDef);
            Assert.IsFalse(cp.Props.HasAutoIncrementingField);
            //---------------Execute Test ----------------------
            transactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------
            dataStore.AssertWasNotCalled(memory => memory.GetNextAutoIncrementingNumber(Arg<IClassDef>.Is.Anything));
        }

    }
}