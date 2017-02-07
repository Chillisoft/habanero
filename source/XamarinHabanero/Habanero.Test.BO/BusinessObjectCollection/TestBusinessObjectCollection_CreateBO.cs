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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectCollection
{
    [TestFixture]
    public class TestBusinessObjectCollection_CreateBO //:TestBase
    {
        private bool _addedEventFired;
        private bool _removedEventFired;
        private DataAccessorInMemory _dataAccessor;
        private DataStoreInMemory _dataStore;

        #region SetupTearDown

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            _dataStore = new DataStoreInMemory();
            _dataAccessor = new DataAccessorInMemory(_dataStore);
            BORegistry.DataAccessor = _dataAccessor;
            ContactPersonTestBO.LoadDefaultClassDef();
        }

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //            ClassDef.ClassDefs.Clear();
            _dataStore.ClearAllBusinessObjects();
            TestUtil.WaitForGC();
        }

        #endregion

        #region CreateNewBusinessObject


        [Test]
        public void Test_CreateBusObject()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();
            RegisterForAddedEvent(cpCol);

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(_addedEventFired);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            Assert.Contains(newCP, cpCol.CreatedBusinessObjects);
            Assert.Contains(newCP, cpCol);
            Assert.IsFalse(cpCol.PersistedBusinessObjects.Contains(newCP));
            Assert.IsTrue(_addedEventFired);
        }

        [Test]
        public void Test_CreatedBusinessObject_SaveBO()
        {
            //Saving a created business object should remove the business
            // object from the created col. it should still exist in 
            // the main collection and should be added to the persisted collection
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();

            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();
            RegisterForAddedEvent(cpCol);

            //---------------Assert Preconditions --------------
            Assert.IsFalse(_addedEventFired);
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            newCP.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(_addedEventFired);
            Assert.Contains(newCP, cpCol);
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        //Persisting business object collections
        //A business object collection can be persisted via the .SaveAll method. 
        //All the added, removed, created and deleted business objects will be persisted and their collections cleared. 

        [Test]
        public void Test_CreatedBusinessObject_SaveAll()
        {
            //Saving a created business object should remove the business
            // object from the created col. it should still exist in 
            // the main collection and should be added to the persisted collection
            //---------------Set up test pack-------------------

            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();
            RegisterForAddedEvent(cpCol);

            //---------------Assert Preconditions --------------
            Assert.IsFalse(_addedEventFired);
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            Assert.IsFalse(_addedEventFired);
            Assert.Contains(newCP, cpCol);
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_Refresh_W_CreatedBOs_CreatedObjectsStillRespondToEvents()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();

            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = BOTestUtils.RandomString;
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.Refresh();
            createdCp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_CreatedBusinessObject_SavedTwice()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();
            newCP.Save();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //---------------Execute Test ----------------------
            newCP.Surname = TestUtil.GetRandomString();
            newCP.Save();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_CreatedBo_Remove()
        {
            //If you remove a created business object that is not yet persisted then
            //-- remove from the restored and saved event.
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = BOTestUtils.RandomString;
            RegisterForAddedAndRemovedEvents(cpCol);
            //---------------Assert Precondition----------------
            Assert.IsTrue(cpCol.Contains(createdCp));
            AssertTwoCurrentObjects_OnePsersisted_OneCreated(cpCol);
            //---------------Execute Test ----------------------
            cpCol.Remove(createdCp);

            //---------------Test Result -----------------------
            Assert.IsFalse(cpCol.Contains(createdCp));
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            AssertRemovedEventFired();
            AssertAddedEventNotFired();
        }

        [Test]
        public void Test_CreatedBO_Remove_DeregisteresFromRestoredEvent()
        {
            //If you remove a created business object that is not yet persisted then
            //-- remove from the restored and saved event.
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = BOTestUtils.RandomString;

            //---------------Assert Precondition----------------
            AssertTwoCurrentObjects_OnePsersisted_OneCreated(cpCol);
            Assert.IsTrue(cpCol.Contains(createdCp));

            //---------------Execute Test ----------------------
            cpCol.Remove(createdCp);
            createdCp.CancelEdits();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            Assert.IsFalse(cpCol.Contains(createdCp));
        }

        [Test]
        public void Test_CreatedBusinessObject_RestoreIndependently()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            newCP.CancelEdits();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
        }

        [Test]
        public void Test_CreatedBusinessObject_RestoredTwice()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();
            newCP.CancelEdits();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            newCP.CancelEdits();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
        }

        [Test]
        public void Test_CreatedBusinessObject_CancelEdits()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            cpCol.CancelEdits();

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            AssertRemovedEventFired();
        }

        [Test]
        public void Test_CreatedBusinessObject_ColMarkForDelete_ShouldBeRemovedFromTheCollection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(newCP);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(newCP.Status.IsDeleted);
        }

        [Test]
        public void Test_CreatedBusinessObject_MarkForDelete_ShouldBeRemovedFromTheCollection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            //---------------Execute Test ----------------------
            newCP.MarkForDelete();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(newCP.Status.IsDeleted);
        }

        [Test]
        public void Test_CreatedBusinessObject_Refresh()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(newCP);

            //---------------Test Result -----------------------            
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
        }

        [Test]
        public void Test_CreatedBusinessObject_Add()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.GetRandomString();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            cpCol.Add(newCP);

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_RefreshAll_W_CreatedBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.CreateBusinessObject();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_RemoveCreatedBo_DeregistersForSaveEvent()
        {
            //If you remove a created business object that is not yet persisted then
            //-- remove from the restored and saved event.
            //-- when the object is saved it should be independent of the collection.
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = BOTestUtils.RandomString;
            cpCol.Remove(createdCp);
            RegisterForAddedAndRemovedEvents(cpCol);
            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            Assert.IsFalse(cpCol.Contains(createdCp));

            //---------------Execute Test ----------------------
            createdCp.Save();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            Assert.IsFalse(cpCol.Contains(createdCp));
            AssertAddedAndRemovedEventsNotFired();
        }

        #endregion

        #region Helper Methods

        private static BusinessObjectCollection<ContactPersonTestBO> CreateCollectionWith_OneBO()
        {
            ContactPersonTestBO.CreateSavedContactPerson();
            return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
        }
        private void AssertAddedAndRemovedEventsNotFired()
        {
            AssertAddedEventNotFired();
            AssertRemovedEventNotFired();
        }

        private void AssertRemovedEventNotFired()
        {
            Assert.IsFalse(_removedEventFired);
        }

        private void AssertAddedEventNotFired()
        {
            Assert.IsFalse(_addedEventFired);
        }

        private void AssertRemovedEventFired()
        {
            Assert.IsTrue(_removedEventFired);
        }

        private void AssertAddedEventFired()
        {
            Assert.IsTrue(_addedEventFired);
        }

        private void RegisterForAddedAndRemovedEvents(IBusinessObjectCollection cpCol)
        {
            RegisterForAddedEvent(cpCol);
            RegisterForRemovedEvent(cpCol);
        }

        private void RegisterForRemovedEvent(IBusinessObjectCollection cpCol)
        {
            _removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { _removedEventFired = true; };
        }

        private void RegisterForAddedEvent(IBusinessObjectCollection cpCol)
        {
            _addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };
        }
        private static void AssertOneObjectInCurrentAndCreatedCollection
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }

        private static void AssertTwoCurrentObjects_OnePsersisted_OneCreated
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        }

        private static void AssertOneObjectInCurrentAndPersistedCollection
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        private static void AssertAllCollectionsHaveNoItems(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        #endregion

    }
}