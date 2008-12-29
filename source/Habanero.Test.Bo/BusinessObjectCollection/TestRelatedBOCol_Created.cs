using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.RelatedBusinessObjectCollection
{
    [TestFixture]
    public class TestRelatedBOCol_Created //:TestBase
    {
        private bool _addedEventFired;
        private bool _removedEventFired;
        private DataAccessorInMemory _dataAccessor;
        private DataStoreInMemory _dataStore;
        #region SetupTeardown
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
            ClassDef.ClassDefs.Clear();
            _dataStore = new DataStoreInMemory();
            _dataAccessor = new DataAccessorInMemory(_dataStore);
            BORegistry.DataAccessor = _dataAccessor;
            ContactPersonTestBO.LoadClassDefOrganisationTestBORelationship();
            OrganisationTestBO.LoadDefaultClassDef();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            _dataStore.ClearAllBusinessObjects();
            TestUtil.WaitForGC();
        }
        #endregion


        #region CreateNewBusinessObject

        [Test]
        public void Test_LoadRelatedBoCol_Generic()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship cpRelationship = (MultipleRelationship)organisationTestBO.Relationships["ContactPeople"];
            ContactPersonTestBO cp = (ContactPersonTestBO) cpRelationship.GetRelatedBusinessObjectCol().CreateBusinessObject();
            cp.FirstName = TestUtil.CreateRandomString();
            cp.Surname = TestUtil.CreateRandomString();
            cp.Save();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<ContactPersonTestBO>(cpRelationship);

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }
        [Test]
        public void Test_LoadRelatedBoCol()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship cpRelationship = (MultipleRelationship)organisationTestBO.Relationships["ContactPeople"];
            ContactPersonTestBO cp = (ContactPersonTestBO)cpRelationship.GetRelatedBusinessObjectCol().CreateBusinessObject();
            cp.FirstName = TestUtil.CreateRandomString();
            cp.Surname = TestUtil.CreateRandomString();
            cp.Save();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBusinessObjectCollection cpCol = BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection(typeof(ContactPersonTestBO), cpRelationship);

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }
        [Test]
        public void Test_CreateBusObject()
        {
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship cpRelationship = (MultipleRelationship)organisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = new RelatedBusinessObjectCollection<ContactPersonTestBO>(cpRelationship);
            cpCol.LoadAll();
            RegisterForAddedEvent(cpCol);

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(_addedEventFired);
            Assert.AreEqual(0, cpCol.PersistedBOCol.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            Assert.Contains(newCP, cpCol.CreatedBusinessObjects);
            Assert.Contains(newCP, cpCol);
            Assert.IsFalse(cpCol.PersistedBOCol.Contains(newCP));
            Assert.IsTrue(_addedEventFired);
        }

        [Test]
        public void Test_CreatedBusinessObject_SaveBO()
        {
            //Saving a created business object should remove the business
            // object from the created col. it should still exist in 
            // the main collection and should be added to the persisted collection
            //---------------Set up test pack-------------------
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship cpRelationship = (MultipleRelationship)organisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = new RelatedBusinessObjectCollection<ContactPersonTestBO>(cpRelationship);
            cpCol.LoadAll();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.FirstName = TestUtil.CreateRandomString();
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

            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship cpRelationship = (MultipleRelationship)organisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = new RelatedBusinessObjectCollection<ContactPersonTestBO>(cpRelationship);
            cpCol.LoadAll();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.FirstName = TestUtil.CreateRandomString();
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
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();

            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;
            createdCp.FirstName = TestUtils.RandomString;
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);

            //---------------Execute Test ----------------------
            cpCol.Refresh();
            createdCp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.PersistedBOCol.Count);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_CreatedBusinessObject_SavedTwice()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.FirstName = TestUtil.CreateRandomString();
            newCP.Save();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //---------------Execute Test ----------------------
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.Save();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        private static RelatedBusinessObjectCollection<ContactPersonTestBO> CreateRelatedCPCol()
        {
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship cpRelationship = (MultipleRelationship)organisationTestBO.Relationships["ContactPeople"];
            return new RelatedBusinessObjectCollection<ContactPersonTestBO>(cpRelationship);
        }

        [Test]
        public void Test_CreatedBo_Remove()
        {
            //If you remove a created business object that is not yet persisted then
            //-- remove from the restored and saved event.
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;
            createdCp.FirstName = TestUtils.RandomString;
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
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;
            createdCp.FirstName = TestUtils.RandomString;

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
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.FirstName = TestUtils.RandomString;

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
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.CancelEdits();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            newCP.CancelEdits();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
        }

        [Test]
        public void Test_CreatedBusinessObject_RestoredAll()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            cpCol.RestoreAll();

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            AssertRemovedEventFired();
        }

        [Test, ExpectedException(typeof(HabaneroDeveloperException))]
        public void Test_CreatedBusinessObject_ColMark4Delete()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(newCP);

            //---------------Test Result -----------------------
        }

        [Test, ExpectedException(typeof(HabaneroDeveloperException))]
        public void Test_CreatedBusinessObject_Mark4Delete()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);

            //---------------Execute Test ----------------------
            newCP.MarkForDelete();

            //---------------Test Result -----------------------
        }

        [Test]
        public void Test_CreatedBusinessObject_Refresh()
        {
            //---------------Set up test pack-------------------
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();

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
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
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
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateRelatedCPCol();
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
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;
            createdCp.FirstName = TestUtils.RandomString;
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

        private static RelatedBusinessObjectCollection<ContactPersonTestBO> CreateCollectionWith_OneBO()
        {
            OrganisationTestBO organisationTestBO = OrganisationTestBO.CreateSavedOrganisation();
            MultipleRelationship cpRelationship = (MultipleRelationship)organisationTestBO.Relationships["ContactPeople"];
            RelatedBusinessObjectCollection<ContactPersonTestBO> cpCol = new RelatedBusinessObjectCollection<ContactPersonTestBO>(cpRelationship);

            ContactPersonTestBO cp = cpCol.CreateBusinessObject();
            cp.FirstName = TestUtil.CreateRandomString();
            cp.Surname = TestUtil.CreateRandomString();
            cp.Save();
            return BORegistry.DataAccessor.BusinessObjectLoader.GetRelatedBusinessObjectCollection<ContactPersonTestBO>(cpRelationship);
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

//        private void AssertAddedEventFired()
//        {
//            Assert.IsTrue(_addedEventFired);
//        }

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
            (IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBOCol.Count);
            Assert.AreEqual(0, cpCol.RemovedBOCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBOCol.Count);
            Assert.AreEqual(0, cpCol.MarkForDeletionBOCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBOCol.Count);
        }

        private static void AssertTwoCurrentObjects_OnePsersisted_OneCreated
            (IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBOCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.AreEqual(0, cpCol.RemovedBOCol.Count);
            Assert.AreEqual(0, cpCol.AddedBOCol.Count);
        }

        private static void AssertOneObjectInCurrentAndPersistedCollection
            (IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBOCol.Count);
            Assert.AreEqual(0, cpCol.RemovedBOCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.AreEqual(0, cpCol.MarkForDeletionBOCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBOCol.Count);
        }

        private static void AssertAllCollectionsHaveNoItems(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBOCol.Count);
            Assert.AreEqual(0, cpCol.RemovedBOCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBOCol.Count);
            Assert.AreEqual(0, cpCol.MarkForDeletionBOCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBOCol.Count);
        }

        #endregion

    }
}