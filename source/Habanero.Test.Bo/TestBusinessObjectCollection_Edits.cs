using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
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
        //will be reverted to its origional state i.e. the created Business Objects will be 
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

        //TODO: Test for refresh mark for delete
        //TODO: Mark 4 deletion with a collection of objects.


        private bool _addedEventFired;
        private DataAccessorInMemory _dataAccessor;
        private DataStoreInMemory _dataStore;
        private bool _removedEventFired;

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

        //TODO: Add tests for this and remove in derigister
        //   bo.ID.Updated += UpdateHashTable;

        #region CreateNewBusinessObject

        [Test]
        public void Test_CreateBusObject()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();
            _addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsFalse(_addedEventFired);
            Assert.AreEqual(0, cpCol.PersistedBOCol.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);

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
            //ContactPersonTestBO.LoadDefaultClassDef();
            _addedEventFired = false;

            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            //---------------Assert Preconditions --------------
            Assert.IsFalse(_addedEventFired);
            Assert.AreEqual(1, cpCol.Count);

            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            newCP.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(_addedEventFired);
            Assert.AreEqual(1, cpCol.Count);
            Assert.Contains(newCP, cpCol);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
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
            //ContactPersonTestBO.LoadDefaultClassDef();
            _addedEventFired = false;

            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            //---------------Assert Preconditions --------------
            Assert.IsFalse(_addedEventFired);
            Assert.AreEqual(1, cpCol.Count);

            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            Assert.IsFalse(_addedEventFired);
            Assert.AreEqual(1, cpCol.Count);
            Assert.Contains(newCP, cpCol);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        }

        [Test]
        public void Test_Refresh_W_CreatedBOs_CreatedObjectsStillRespondToEvents()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();

            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;

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
        }

        [Test]
        public void Test_CreatedBusinessObject_SavedTwice()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.Save();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
        }

        [Test]
        public void Test_CreatedBo_Remove()
        {
            //If you remove a created business object that is not yet persisted then
            //-- remove from the restored and saved event.
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();

            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsTrue(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.Remove(createdCp);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsFalse(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        }

        [Test]
        public void Test_CreatedBO_Remove_DeregisteresFromRestoredEvent()
        {
            //If you remove a created business object that is not yet persisted then
            //-- remove from the restored and saved event.
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsTrue(cpCol.Contains(createdCp));

            //---------------Execute Test ----------------------
            cpCol.Remove(createdCp);
            createdCp.Restore();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsFalse(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        }

        [Test]
        public void Test_CreatedBusinessObject_RestoreIndependently()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            newCP.Restore();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        }

        [Test]
        public void Test_CreatedBusinessObject_RestoredTwice()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.Restore();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);

            //---------------Execute Test ----------------------
            newCP.Restore();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
        }

        [Test]
        public void Test_CreatedBusinessObject_RestoredAll()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.RestoreAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        }

        [Test]
        public void Test_Refresh_W_CreatedBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.CreateBusinessObject();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);

            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
        }

        #endregion

        #region AddBO

        [Test]
        public void Test_AddMethod()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            //---------------Execute Test ----------------------
            col.Add(myBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count, "One object should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
        }

        [Test]
        public void Test_Add_NewBO_AddsToCreatedCollection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = ContactPersonTestBO.CreateUnsavedContactPerson
                (TestUtils.RandomString, TestUtils.RandomString);
            _addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.Add(newCP);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.Contains(newCP, cpCol);
            Assert.IsTrue(_addedEventFired);
        }

        [Test]
        public void Test_AddMethod_WithEnumerable_Collection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            Collection<ContactPersonTestBO> collection = new Collection<ContactPersonTestBO>();
            collection.Add(myBO);
            collection.Add(myBO2);
            collection.Add(myBO3);
            //---------------Execute Test ----------------------
            col.Add(collection);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
            Assert.AreEqual(myBO2, col[1], "Added object should be in the collection");
            Assert.AreEqual(myBO3, col[2], "Added object should be in the collection");
        }


        [Test]
        public void Test_AddMethod_WithEnumerable_List()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            List<ContactPersonTestBO> list = new List<ContactPersonTestBO>();
            list.Add(myBO);
            list.Add(myBO2);
            list.Add(myBO3);
            //---------------Execute Test ----------------------
            col.Add(list);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
            Assert.AreEqual(myBO2, col[1], "Added object should be in the collection");
            Assert.AreEqual(myBO3, col[2], "Added object should be in the collection");
        }

        [Test]
        public void Test_AddMethod_WithParamArray()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            //---------------Execute Test ----------------------
            col.Add(myBO, myBO2, myBO3);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
            Assert.AreEqual(myBO2, col[1], "Added object should be in the collection");
            Assert.AreEqual(myBO3, col[2], "Added object should be in the collection");
        }

        [Test]
        public void Test_Add_PersistedBO()
        {
            //---------------Set up test pack-------------------
            //The persisted business objects should not be added since this is a normal collection which does not 
            // modify the added objects alternate key etc unlike the RelatedBusinessObjectCollection
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO2 = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO3 = ContactPersonTestBO.CreateSavedContactPerson();
            _addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            //-------Assert Preconditions
            Assert.AreEqual(0, cpCol.Count, "Three objects should be in the collection");

            ///---------------Execute Test ----------------------
            cpCol.Add(myBO, myBO2, myBO3);

            //---------------Test Result ----------------------- - Result
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the copied collection");
            Assert.AreEqual
                (3, cpCol.AddedBusinessObjects.Count,
                 "The persisted business objects should not be in the AddedList since this is a normal collection which does not modify the added objects alternate key etc unlike the RelatedBusinessObjectCollection");
            Assert.IsTrue(_addedEventFired);
        }

        [Test]
        public void Test_AddMethod_WithCollection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            col.Add(myBO, myBO2, myBO3);
            //-------Assert Preconditions
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            ///---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> colCopied = new BusinessObjectCollection<ContactPersonTestBO>
                ();
            colCopied.Add(col);
            //---------------Test Result ----------------------- - Result
            Assert.AreEqual(3, colCopied.Count, "Three objects should be in the copied collection");
            Assert.AreEqual(3, colCopied.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, colCopied.AddedBusinessObjects.Count);
            Assert.AreEqual(myBO, colCopied[0], "Added object should be in the copied collection");
            Assert.AreEqual(myBO2, colCopied[1], "Added object should be in the copied collection");
            Assert.AreEqual(myBO3, colCopied[2], "Added object should be in the copied collection");
        }


        [Test]
        public void Test_Add_PersistedBO_RestoreAll()
        {
            //---------------Set up test pack-------------------
            //The persisted business objects should not be added since this is a normal collection which does not 
            // modify the added objects alternate key etc unlike the RelatedBusinessObjectCollection
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO2 = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO3 = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO, myBO2, myBO3);

            //-------Assert Preconditions
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the copied collection");
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count, "Three objects should be in the collection");

            ///---------------Execute Test ----------------------
            cpCol.RestoreAll();

            //---------------Test Result ----------------------- - Result
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the copied collection");
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
        }

        [Test]
        public void TestAddMethod_IgnoresAddWhenItemAlreadyExists()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            MyBO myBO2 = myBO;
            col.Add(myBO);

            //---------------Execute Test ----------------------
            col.Add(myBO2);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count, "One object should be in the collection");
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
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);

            //-----Run tests----------------------------
            cpCol.Remove(cp);

            ////-----Test results-------------------------
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
        }

        [Test]
        public void Test_RemoveAt_AddsToRemovedCollection()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);
            _removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { _removedEventFired = true; };
            //--------------Assert Preconditions--------
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsFalse(_removedEventFired);

            //-----Run tests----------------------------
            cpCol.RemoveAt(0);

            ////-----Test results-------------------------
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(_removedEventFired);
        }

        [Test]
        public void Test_Remove_AlreadyInRemoveCollection()
        {
            //-----Create Test pack---------------------
            ContactPersonTestBO cp;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCol_OneCP(out cp);

            //--------------Assert Preconditions--------
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);

            //-----Run tests----------------------------
            cpCol.Remove(cp);
            cpCol.Remove(cp);

            //-----Test results-------------------------
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
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
            _addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            //--------------Assert Preconditions--------
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.IsFalse(_addedEventFired);

            //-----Run tests----------------------------
            cpCol.RestoreAll();

            //-----Test results-------------------------
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.IsTrue(_addedEventFired);
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
            createdCp.Surname = TestUtils.RandomString;
            cpCol.Remove(createdCp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsFalse(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            createdCp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsFalse(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
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
            Assert.IsNotNull(cpCol.MarkForDeleteBusinessObjects);
        }

        [Test]
        public void Test_MarkForDeleteBO()
        {
            //A Business object that exists in the collection can be marked for deletion either as a bo or
            //  as an index in the collection
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO markForDeleteCP = cpCol[0];
            _removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { _removedEventFired = true; };

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.IsFalse(_removedEventFired);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(markForDeleteCP);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.IsTrue(markForDeleteCP.Status.IsDeleted);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.IsTrue(_removedEventFired);
        }

        [Test]
        public void Test_MarkForDeleteBO_MarkBODirectly()
        {
            //A Business object that exists in the collection can be marked for deletion either as a bo or
            //  as an index in the collection
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO markForDeleteCP = cpCol[0];
            _removedEventFired = false;
            cpCol.BusinessObjectRemoved += delegate { _removedEventFired = true; };
            //---------------Assert Precondition----------------
            Assert.IsFalse(markForDeleteCP.Status.IsDeleted);
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);

            //---------------Execute Test ----------------------
            markForDeleteCP.MarkForDelete();

            //---------------Test Result -----------------------
            Assert.IsTrue(markForDeleteCP.Status.IsDeleted);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
            Assert.IsTrue(_removedEventFired);
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
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.MarkForDeleteAt(0);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);
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
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);

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
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsTrue(cpCol.Contains(cp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(cp);
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBOCol.Count);
            Assert.IsFalse(cpCol.Contains(cp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
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
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsTrue(cpCol.Contains(cp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(cp);
            cp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBOCol.Count);
            Assert.IsFalse(cpCol.Contains(cp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
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
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);

            //---------------Execute Test ----------------------
            cpCol.RestoreAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsTrue(cpCol.Contains(cp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
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
            _addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsFalse(_addedEventFired);

            //---------------Execute Test ----------------------
            cp.Restore();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOCol.Count);
            Assert.IsTrue(cpCol.Contains(cp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.IsTrue(_addedEventFired);
        }

        #endregion

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

        #endregion
    }
}