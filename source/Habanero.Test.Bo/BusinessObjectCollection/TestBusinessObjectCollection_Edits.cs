using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectCollection
{
    [TestFixture]
    public class TestBusinessObjectCollection_Edits
    {
        //Making edits to a Business Object Collection
        //In addition to loading a collection the developer can manipulate the collection in a number of ways. 
        //The principles of how Habanero manages editing a Business Object Collection are similar to editing 
        //a Business Object. 
        //To achieve this functionality the Business Object collection maintains 4 collections namely 
        //-	The CreatedBusinessObjects, a list of business objects created by the collection. 
        //-	The RemovedBusinessObjects, a list of business objects removed from the collection.
        //-	The PersistedBusinessObjects, a list of business objects representing the list of business objects as 
        //per the last time the collection loaded from the database or persisted to the database.
        //-	The primary collection, a collection showing the PersistedBusinessObjects plus the 
        //CreatedBusinessObjects Less the RemovedBusinessObjects.
        //Creating business object
        //The business object collection can create a business object (customers.CreateBusinessObject()). 
        //The business object of the appropriate type will be created and added to a the 
        //business objects internal list of created business objects (customers.CreatedBusinessObjects). 
        //The Business Object will also be added to the main collection and will thus be available for 
        //viewing/editing in grids, lists etc. 
        //If the collection is saved then these created business objects will be saved (customers.SaveAll() 
        //and customers.SaveAllInTransaction(transaction)) to the database.
        //If the created Business Object is saved independently it will be removed from the created 
        //list and added to the Persisted list
        //If the collection is restored (all edits cancelled. Customers.RestoreAll) then the collection 
        //will be reverted to its original state i.e. the created Business Objects will be 
        //cleared and the main collection will be restored from the PersistedBusinessObjects.
        //Add a business object
        //The developer can add an existing business object to a business object collection. 
        //If the business object is new (customer.Status.IsNew) then the business object will be added to 
        //the CreatedBusinessObjects list as well as the primary list. (see test??).
        //If the business object is persisted (!customer.Status.IsNew) then the business object will be 
        //added to the Main collection only. 
        //Remove a Business Object
        //The developer can also remove a business object from a business object collection (Remove() 
        //and RemoveAt()). 
        //If the business object is new then it is removed from the main collection and from the 
        //CreatedBusinessObjects list.
        //If the business object is not new then it is removed from the main list and added to the 
        //RemovedBusinessObjects list See test
        //MarkForDelete a Business Object
        //The developer can also mark a Business Objects for deletion from a business object collection. 
        //If the business object is new then it is removed from the main collection and from the 
        //CreatedBusinessObjects list and MarkedForDeletion. See test
        //If the business object is not new then it is removed from the main list, marked for deletion and 
        //added to the DeletedBusinessObjects list See test
        //If a BusinessObject in the collection is Updated(i.e the BO is saved, or edits are cancelled), then 
        //  the BusinessObjectUpdated event on the collection is fired.
        //If a BusinessObject in the collection has a property that is Updated(i.e the property is edited), then 
        //  the BusinessObjectPropertyUpdated event on the collection is fired.
        //If a BusinessObject in the collection has an ID that is Updated(i.e one of the properties of the ID is edited), then 
        //  the BusinessObjectIDUpdated event on the collection is fired.

        //See Also TestBusinessObjectCollection_AddedBos
        //         TestBusinessObjectCollection_CreatedBo

        //TODO: Test for refresh mark for delete
        //TODO: Mark 4 deletion with a collection of objects.


        //private DataAccessorInMemory _dataAccessor;
        //private DataStoreInMemory _dataStore;
        private ClassDef _cpDefaultClassDef;

        #region SetupTearDown

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            //_dataStore = new DataStoreInMemory();
            //_dataAccessor = new DataAccessorInMemory(_dataStore);
            //BORegistry.DataAccessor = _dataAccessor;
            _cpDefaultClassDef = ContactPersonTestBO.LoadDefaultClassDef();
        }

        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
//            ClassDef.ClassDefs.Clear();
            //  _dataStore.ClearAllBusinessObjects();
            TestUtil.WaitForGC();
        }

        #endregion

        //TODO: Add tests for this and remove in derigister
        //   bo.ID.Updated += UpdateHashTable;

        #region Custom Asserts

        private static void AssertAllCollectionsHaveNoItems(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        private static void AssertOneObjectInCurrentAndPersistedCollection
            (IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        private static void AssertOneObjectInRemovedAndPersistedCollection
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        private static void AssertOnePersisted_OneMark4Delete(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        }

        #endregion

        #region Methods On loaded collection

        [Test]
        public void Test_LoadBoCol_Generic()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_LoadBoCol()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.CreateSavedContactPerson();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IBusinessObjectCollection cpCol = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection
                (_cpDefaultClassDef, "");

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_LoadedBo_Add()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            //--------------Assert Preconditions--------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            cpCol.Add(cp);

            ////-----Test results-------------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_loadedBo_Save()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cp.FirstName = TestUtil.GetRandomString();

            //--------------Assert Preconditions--------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            cp.Save();

            ////-----Test results-------------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_loadedBo_Restore()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cp.FirstName = TestUtil.GetRandomString();

            //--------------Assert Preconditions--------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            cp.CancelEdits();

            ////-----Test results-------------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_loadedBo_RestoreAll()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cp.FirstName = TestUtil.GetRandomString();

            //--------------Assert Preconditions--------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            cp.CancelEdits();

            ////-----Test results-------------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_loadedBo_Refresh()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
//            cp.FirstName = TestUtil.GetRandomString();

            //--------------Assert Preconditions--------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cp);

            ////-----Test results-------------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_loadedBo_RefreshAll()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cp.FirstName = TestUtil.GetRandomString();

            //--------------Assert Preconditions--------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            cpCol.Refresh();

            ////-----Test results-------------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        #endregion

        #region Remove

        [Test]
        public void Test_Remove_AddsToRemovedCollection()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            //--------------Assert Preconditions--------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            cpCol.Remove(cp);

            ////-----Test results-------------------------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_RemoveAt_AddsToRemovedCollection()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { removedEventFired = true; };
            //--------------Assert Preconditions--------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            Assert.IsFalse(removedEventFired);

            //-----Run tests----------------------------
            cpCol.RemoveAt(0);

            ////-----Test results-------------------------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
            Assert.IsTrue(removedEventFired);
        }

        [Test]
        public void Test_Remove_AlreadyInRemoveCollection()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cpCol.Remove(cp);
            bool removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { removedEventFired = true; };
            //--------------Assert Preconditions--------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
            Assert.IsFalse(removedEventFired);

            //-----Run tests----------------------------
            cpCol.Remove(cp);

            //-----Test results-------------------------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
            Assert.IsFalse(removedEventFired);
        }

        //Persisting business object collections
        //A business object collection can be persisted via the .SaveAll method. 
        //All the added, removed, created and deleted business objects will be persisted and their collections cleared. 

        [Test]
        public void Test_Remove_SaveAll()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);

            //--------------Assert Preconditions--------
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            //-----Run tests----------------------------

            cpCol.Remove(cp);
            cpCol.SaveAll();

            ////-----Test results-------------------------
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        }

        [Test]
        public void Test_Remove_RestoreAll()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cpCol.Remove(cp);
            bool addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { addedEventFired = true; };

            //--------------Assert Preconditions--------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
            Assert.IsFalse(addedEventFired);

            //-----Run tests----------------------------
            cpCol.CancelEdits();

            //-----Test results-------------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            Assert.IsTrue(addedEventFired);
        }

        [Test]
        public void Test_Remove_RefreshAll()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cpCol.Remove(cp);

            //--------------Assert Preconditions--------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            cpCol.Refresh();

            ////-----Test results-------------------------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_Remove_Refresh()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cpCol.Remove(cp);

            //--------------Assert Preconditions--------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cp);

            ////-----Test results-------------------------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_Remove_BoCancelEdits()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cpCol.Remove(cp);

            //--------------Assert Preconditions--------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            cp.CancelEdits();

            ////-----Test results-------------------------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_Remove_Add()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cpCol.Remove(cp);

            //--------------Assert Preconditions--------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);

            //-----Run tests----------------------------
            cpCol.Add(cp);

            ////-----Test results-------------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
        }

        [Test]
        public void Test_Remove_ColMark4Delete()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cpCol.Remove(cp);
            bool removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { removedEventFired = true; };
            //--------------Assert Preconditions--------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
            Assert.IsFalse(removedEventFired);

            //-----Run tests----------------------------
            cpCol.MarkForDelete(cp);

            ////-----Test results-------------------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsFalse(removedEventFired);
        }

        [Test]
        public void Test_Remove_BOMark4Delete()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cpCol.Remove(cp);
            bool removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { removedEventFired = true; };
            //--------------Assert Preconditions--------
            AssertOneObjectInRemovedAndPersistedCollection(cpCol);
            Assert.IsFalse(removedEventFired);

            //-----Run tests----------------------------
            cp.MarkForDelete();

            ////-----Test results-------------------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsFalse(removedEventFired);
        }

        #endregion

        #region MarkForDelete

        [Test]
        public void Test_MarkForDeleteBOList()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            //---------------Test Result -----------------------
            Assert.IsNotNull(cpCol.MarkedForDeleteBusinessObjects);
        }

        [Test]
        public void Test_MarkForDeleteBO_MarkBODirectly()
        {
            //A Business object that exists in the collection can be marked for deletion either as a bo or
            //  as an index in the collection
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO markForDeleteCP = cpCol[0];
            bool removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { removedEventFired = true; };
            //---------------Assert Precondition----------------
            Assert.IsFalse(markForDeleteCP.Status.IsDeleted);
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            //---------------Execute Test ----------------------
            markForDeleteCP.MarkForDelete();

            //---------------Test Result -----------------------
            Assert.IsTrue(markForDeleteCP.Status.IsDeleted);
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsTrue(removedEventFired);
        }

        [Test]
        public void Test_ColMarkForDeleteBO()
        {
            //A Business object that exists in the collection can be marked for deletion either as a bo or
            //  as an index in the collection
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO markForDeleteCP = cpCol[0];
            bool removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { removedEventFired = true; };

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            Assert.IsFalse(removedEventFired);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(markForDeleteCP);

            //---------------Test Result -----------------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsTrue(markForDeleteCP.Status.IsDeleted);
            Assert.IsTrue(removedEventFired);
        }

        [Test]
        public void Test_MarkForDeleteBO_Remove_RemovedNotFired()
        {
            //A Business object that exists in the collection can be marked for deletion either as a bo or
            //  as an index in the collection
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO markForDeleteCP = cpCol[0];
            cpCol.MarkForDelete(markForDeleteCP);
            bool removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { removedEventFired = true; };

            //---------------Assert Precondition----------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsFalse(removedEventFired);

            //---------------Execute Test ----------------------
            cpCol.Remove(markForDeleteCP);

            //---------------Test Result -----------------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsTrue(markForDeleteCP.Status.IsDeleted);
            Assert.IsFalse(removedEventFired);
        }

        [Test]
        public void Test_MarkForDeleteBO_Mark4Delete_RemovedNotFired()
        {
            //A Business object that exists in the collection can be marked for deletion either as a bo or
            //  as an index in the collection
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO markForDeleteCP = cpCol[0];
            cpCol.MarkForDelete(markForDeleteCP);
            bool removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { removedEventFired = true; };

            //---------------Assert Precondition----------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsFalse(removedEventFired);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(markForDeleteCP);

            //---------------Test Result -----------------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsTrue(markForDeleteCP.Status.IsDeleted);
            Assert.IsFalse(removedEventFired);
        }

        [Test]
        public void Test_MarkForDeleteBO_BOMark4Delete_RemovedNotFired()
        {
            //A Business object that exists in the collection can be marked for deletion either as a bo or
            //  as an index in the collection
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO markForDeleteCP = cpCol[0];
            cpCol.MarkForDelete(markForDeleteCP);
            bool removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { removedEventFired = true; };

            //---------------Assert Precondition----------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsFalse(removedEventFired);

            //---------------Execute Test ----------------------
            markForDeleteCP.MarkForDelete();

            //---------------Test Result -----------------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsTrue(markForDeleteCP.Status.IsDeleted);
            Assert.IsFalse(removedEventFired);
        }

        //TODO: Throw error if bo that is not in the col is handed to mark for delete
        [Test]
        public void Test_MarkForDeleteBO_At()
        {
            //A Business object that exists in the collection can be marked for deletion either as a bo or
            //  as an index in the collection
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO markForDeleteCP = cpCol[0];

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.MarkForDeleteAt(0);

            //---------------Test Result -----------------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsTrue(markForDeleteCP.Status.IsDeleted);
        }

        [Test]
        public void Test_MarkForDeleteBO_At_IndexNotExist()
        {
            //A Business object that exists in the collection can be marked for deletion either as a bo or
            //  as an index in the collection
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);

            //---------------Execute Test ----------------------
            try
            {
                cpCol.MarkForDeleteAt(1);
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (ArgumentOutOfRangeException ex)
            {
                StringAssert.Contains("Index was out of range", ex.Message);
            }
            catch (Exception)
            {
                Assert.Fail("ArgumentOutOfRangeException not thrown");
            }
        }

        // SaveAll deleted bos
        [Test]
        public void Test_MarkForDeleteBO_SaveAll()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO cp = cpCol[0];

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            Assert.IsTrue(cpCol.Contains(cp));

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(cp);
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(cpCol.Contains(cp));
            Assert.IsTrue(cp.Status.IsDeleted);
            Assert.IsTrue(cp.Status.IsNew);
        }

        // SaveAll deleted bos
        [Test]
        public void Test_MarkForDeleteBO_SaveBO()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO cp = cpCol[0];

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);

            Assert.IsTrue(cpCol.Contains(cp));

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(cp);
            cp.Save();

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(cpCol.Contains(cp));
            Assert.IsTrue(cp.Status.IsDeleted);
            Assert.IsTrue(cp.Status.IsNew);
        }

        // Restore all deleted bos
        [Test]
        public void Test_MarkForDeleteBO_RestoreAll()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO cp = cpCol[0];
            cpCol.MarkForDelete(cp);

            //---------------Assert Precondition----------------
            AssertOnePersisted_OneMark4Delete(cpCol);

            //---------------Execute Test ----------------------
            cpCol.CancelEdits();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            Assert.IsTrue(cpCol.Contains(cp));
        }

        //Restore bo independently of col
        [Test]
        public void Test_MarkForDeleteBO_RestoreBOndepedently()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO cp = cpCol[0];
            cpCol.MarkForDelete(cp);
            bool addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { addedEventFired = true; };
            //---------------Assert Precondition----------------
            AssertOnePersisted_OneMark4Delete(cpCol);
            Assert.IsFalse(addedEventFired);

            //---------------Execute Test ----------------------
            cp.CancelEdits();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndPersistedCollection(cpCol);
            Assert.IsTrue(cpCol.Contains(cp));
            Assert.IsTrue(addedEventFired);
        }

        [Test, ExpectedException(typeof (HabaneroDeveloperException))]
        public void Test_MarkForDeleteBO_Refresh()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO cp = cpCol[0];
            cpCol.MarkForDelete(cp);

            //---------------Assert Precondition----------------
            AssertOnePersisted_OneMark4Delete(cpCol);

            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cp);
        }

        #endregion

        #region BusinessObject events

        //If a BusinessObject in the collection is Updated(i.e the BO is saved, or edits are cancelled), then 
        //  the BusinessObjectUpdated event on the collection is fired.
        [Test]
        public void Test_BusinessObjectUpdated_Fired()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool updatedEventFired = false;
            IBusinessObject eventBo = null;
            cpCol.BusinessObjectUpdated += delegate(object sender, BOEventArgs e)
            {
                eventBo = e.BusinessObject;
                updatedEventFired = true;
            };
            cp.Surname = "NewSurname";
            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedEventFired);
            Assert.IsNull(eventBo);
            //---------------Execute Test ----------------------
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(updatedEventFired, "BusinessObjectUpdated event should be fired");
            Assert.IsNotNull(eventBo);
            Assert.AreSame(cp, eventBo);
        }

        [Test]
        public void Test_BusinessObjectUpdated_NotFired_AfterColCleared()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool updatedEventFired = false;
            cpCol.BusinessObjectUpdated += delegate {
                updatedEventFired = true;
            };
            cp.Surname = "NewSurname";
            cpCol.Clear();
            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedEventFired);
            //---------------Execute Test ----------------------
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(updatedEventFired, "BusinessObjectUpdated event should not be fired if the bo is not in the collection");
        }

        [Test]
        public void Test_BusinessObjectUpdated_NotFired_AfterBoRemoved()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool updatedEventFired = false;
            cpCol.BusinessObjectUpdated += delegate {
                updatedEventFired = true;
            };
            cpCol.Remove(cp);
            cp.Surname = "NewSurname";
            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedEventFired);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            //---------------Execute Test ----------------------
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(updatedEventFired, "BusinessObjectUpdated event should not be fired if the bo is not in the collection");
        }    
        [Test]
        public void Test_BusinessObjectUpdated_NotFired_AfterBoMark4Delete()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool updatedEventFired = false;
            cpCol.BusinessObjectUpdated += delegate {
                updatedEventFired = true;
            };
            cpCol.MarkForDelete(cp);
            cp.Surname = "NewSurname";
            //---------------Assert Precondition----------------
            Assert.IsFalse(updatedEventFired);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsFalse(updatedEventFired, "BusinessObjectUpdated event should not be fired if the bo is not in the collection");
        }

        //If a BusinessObject in the collection has a property that is Updated(i.e the property is edited), then 
        //  the BusinessObjectPropertyUpdated event on the collection is fired.
        [Test]
        public void Test_BusinessObjectPropertyUpdated_Fired()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool propertyUpdatedEventFired = false;
            IBusinessObject eventBo = null;
            IBOProp eventProp = null;
            cpCol.BusinessObjectPropertyUpdated += delegate(object sender, BOEventArgs boEventArgs, BOPropEventArgs boPropEventArgs)
            {
                eventBo = boEventArgs.BusinessObject;
                eventProp = boPropEventArgs.Prop;
                propertyUpdatedEventFired = true;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(propertyUpdatedEventFired);
            Assert.IsNull(eventBo);
            Assert.IsNull(eventProp);
            //---------------Execute Test ----------------------
            cp.Surname = "NewSurname";
            //---------------Test Result -----------------------
            Assert.IsTrue(propertyUpdatedEventFired, "BusinessObjectPropertyUpdated event should be fired");
            Assert.IsNotNull(eventBo);
            Assert.AreSame(cp, eventBo);
            Assert.AreSame(cp.Props["Surname"], eventProp);
        }

        [Test]
        public void Test_BusinessObjectPropertyUpdated_NotFired_AfterColCleared()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool propertyUpdatedEventFired = false;
            cpCol.BusinessObjectPropertyUpdated += delegate
            {
                propertyUpdatedEventFired = true;
            };
            cpCol.Clear();
            //---------------Assert Precondition----------------
            Assert.IsFalse(propertyUpdatedEventFired);
            //---------------Execute Test ----------------------
            cp.Surname = "NewSurname";
            //---------------Test Result -----------------------
            Assert.IsFalse(propertyUpdatedEventFired, "BusinessObjectPropertyUpdated event should not be fired if the bo is not in the collection");
        }

        [Test]
        public void Test_BusinessObjectPropertyUpdated_NotFired_AfterBoRemoved()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool propertyUpdatedEventFired = false;
            cpCol.BusinessObjectPropertyUpdated += delegate
            {
                propertyUpdatedEventFired = true;
            };
            cpCol.Remove(cp);
            //---------------Assert Precondition----------------
            Assert.IsFalse(propertyUpdatedEventFired);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            //---------------Execute Test ----------------------
            cp.Surname = "NewSurname";
            //---------------Test Result -----------------------
            Assert.IsFalse(propertyUpdatedEventFired, "BusinessObjectPropertyUpdated event should not be fired if the bo is not in the collection");
        }
        [Test]
        public void Test_BusinessObjectPropertyUpdated_NotFired_AfterBoMark4Delete()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool propertyUpdatedEventFired = false;
            cpCol.BusinessObjectPropertyUpdated += delegate
            {
                propertyUpdatedEventFired = true;
            };
            cpCol.MarkForDelete(cp);
            //---------------Assert Precondition----------------
            Assert.IsFalse(propertyUpdatedEventFired);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            cp.Surname = "NewSurname";
            //---------------Test Result -----------------------
            Assert.IsFalse(propertyUpdatedEventFired, "BusinessObjectPropertyUpdated event should not be fired if the bo is not in the collection");
        }
        
        //If a BusinessObject in the collection has an ID that is Updated(i.e one of the properties of the ID is edited), then 
        //  the BusinessObjectIDUpdated event on the collection is fired.
        [Test]
        public void Test_BusinessObjectIDUpdated_Fired()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool businessObjectIDUpdatedEventFired = false;
            IBusinessObject eventBo = null;
            cpCol.BusinessObjectIDUpdated += delegate(object sender, BOEventArgs boEventArgs)
            {
                eventBo = boEventArgs.BusinessObject;
                businessObjectIDUpdatedEventFired = true;
            };
            //---------------Assert Precondition----------------
            Assert.IsFalse(businessObjectIDUpdatedEventFired);
            Assert.IsNull(eventBo);
            //---------------Execute Test ----------------------
            cp.ContactPersonID = Guid.NewGuid();
            //---------------Test Result -----------------------
            Assert.IsTrue(businessObjectIDUpdatedEventFired, "BusinessObjectIDUpdated event should be fired");
            Assert.IsNotNull(eventBo);
            Assert.AreSame(cp, eventBo);
        }

        [Test]
        public void Test_BusinessObjectIDUpdated_NotFired_AfterColCleared()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool businessObjectIDUpdatedEventFired = false;
            cpCol.BusinessObjectIDUpdated += delegate
            {
                businessObjectIDUpdatedEventFired = true;
            };
            cpCol.Clear();
            //---------------Assert Precondition----------------
            Assert.IsFalse(businessObjectIDUpdatedEventFired);
            //---------------Execute Test ----------------------
            cp.ContactPersonID = Guid.NewGuid();
            //---------------Test Result -----------------------
            Assert.IsFalse(businessObjectIDUpdatedEventFired, "BusinessObjectIDUpdated event should not be fired if the bo is not in the collection");
        }

        [Test]
        public void Test_BusinessObjectIDUpdated_NotFired_AfterBoRemoved()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool businessObjectIDUpdatedEventFired = false;
            cpCol.BusinessObjectIDUpdated += delegate
            {
                businessObjectIDUpdatedEventFired = true;
            };
            cpCol.Remove(cp);
            //---------------Assert Precondition----------------
            Assert.IsFalse(businessObjectIDUpdatedEventFired);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            //---------------Execute Test ----------------------
            cp.ContactPersonID = Guid.NewGuid();
            //---------------Test Result -----------------------
            Assert.IsFalse(businessObjectIDUpdatedEventFired, "BusinessObjectIDUpdated event should not be fired if the bo is not in the collection");
        }

        [Test]
        public void Test_BusinessObjectIDUpdated_NotFired_AfterBoMark4Delete()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            bool businessObjectIDUpdatedEventFired = false;
            cpCol.BusinessObjectIDUpdated += delegate
            {
                businessObjectIDUpdatedEventFired = true;
            };
            cpCol.MarkForDelete(cp);
            //---------------Assert Precondition----------------
            Assert.IsFalse(businessObjectIDUpdatedEventFired);
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            cp.ContactPersonID = Guid.NewGuid();
            //---------------Test Result -----------------------
            Assert.IsFalse(businessObjectIDUpdatedEventFired, "BusinessObjectIDUpdated event should not be fired if the bo is not in the collection");
        }

        #endregion

        [Test]
        public void Test_Clear_DeRegisters_BoSavedEvent_FIXBUG()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cp.Surname = "NewSurname";
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            cpCol.Clear();
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not throw error");
        }

        [Test]
        public void Test_Clear_DeRegisters_BoSavedEvent_BOInRemoved_FIXBUG()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cp.Surname = "NewSurname";
            cpCol.Remove(cp);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            cpCol.Clear();
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not throw error");
        }

        [Test]
        public void Test_Clear_DeRegisters_BoSavedEvent_BOInMarkForDeletion_FIXBUG()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cp.Surname = "NewSurname";
            cpCol.MarkForDelete(cp);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            cpCol.Clear();
            cp.CancelEdits();
            cp.Surname = "fdafads";
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not throw error");
        }

        [Test]
        public void Test_Clear_DeRegisters_BoDeletedEvent_FIXBUG()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            cpCol.Clear();
            cp.MarkForDelete();
            cp.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(true, "Should not throw error");
        }


        #region Utils

//        private static void CreateSavedContactPerson()
//        {
//            ContactPersonTestBO.CreateSavedContactPerson();
//        }

        private static BusinessObjectCollection<ContactPersonTestBO> CreateCol_OneCP(out ContactPersonTestBO cp)
        {
            cp = ContactPersonTestBO.CreateSavedContactPerson();
            return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
        }

//
//        private static void CreateTwoSavedContactPeople()
//        {
//            CreateSavedContactPerson();
//            CreateSavedContactPerson();
//        }

        private static BusinessObjectCollection<ContactPersonTestBO> CreateCollectionWith_OneBO()
        {
            ContactPersonTestBO.CreateSavedContactPerson();
            return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
        }

        [Test]
        public void Test_Refresh_W_RemovedBOs()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            cpCol.Remove(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
        }

        #endregion
    }
}