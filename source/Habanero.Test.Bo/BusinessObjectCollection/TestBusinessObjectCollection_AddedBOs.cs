using System.Collections.Generic;
using System.Collections.ObjectModel;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.TestBusinessObjectCollection
{
    [TestFixture]
    public class TestBusinessObjectCollection_AddedBOs //:TestBase
    {
        private bool _addedEventFired;
        private DataAccessorInMemory _dataAccessor;
        private DataStoreInMemory _dataStore;

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

        #region Added Business object in current collection

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
        public void Test_Add_PersistedBO_RestoreAll()
        {
            //---------------Set up test pack-------------------
            //The persisted objects are added to the added collection
            // when restore is called the added objects should be removed from the collection.
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO2 = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO3 = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO, myBO2, myBO3);

            //-------Assert Preconditions
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the copied collection");
            Assert.AreEqual(3, cpCol.AddedBusinessObjects.Count, "Three objects should be in the collection");

            ///---------------Execute Test ----------------------
            cpCol.RestoreAll();

            //---------------Test Result ----------------------- - Result
            AssertAllCollectionsHaveNoItems(cpCol);
        }


        [Test]
        public void TestAddMethod_IgnoresAddWhenItemAlreadyExists()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            col.Add(myBO);
            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(col);

            //---------------Execute Test ----------------------
            col.Add(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndCreatedCollection(col);
        }

        [Test]
        public void TestAddMethod_PersistedObject_IgnoresAddWhenItemAlreadyExists()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            col.Add(myBO);
            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(col);
            //---------------Execute Test ----------------------
            col.Add(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(col);
        }

        [Test]
        public void TestAddMethod_SaveBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            col.Add(myBO);
            myBO.Surname = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(col);

            //---------------Execute Test ----------------------
            myBO.Save();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentPersistedAndAddedCollection(col);
        }


        [Test]
        public void TestAddMethod_SaveAllBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            col.Add(myBO);
            myBO.Surname = TestUtil.CreateRandomString();

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(col);
            Assert.IsTrue(myBO.Status.IsDirty);
            //---------------Execute Test ----------------------
            col.SaveAll();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentPersistedAndAddedCollection(col);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        private static void AssertOneObjectInCurrentAndAddedCollection(IBusinessObjectCollection col)
        {
            Assert.AreEqual(1, col.Count, "One object should be in the collection");
            Assert.AreEqual(1, col.AddedBOCol.Count, "One object should be in the collection");
            Assert.AreEqual(0, col.RemovedBOCol.Count);
            Assert.AreEqual(0, col.PersistedBOCol.Count);
            Assert.AreEqual(0, col.MarkForDeletionBOs.Count);
            Assert.AreEqual(0, col.CreatedBOCol.Count);
        }

        private static void AssertOneObjectInCurrentAndCreatedCollection(IBusinessObjectCollection col)
        {
            Assert.AreEqual(1, col.Count, "One object should be in the collection");
            Assert.AreEqual(0, col.AddedBOCol.Count, "One object should be in the collection");
            Assert.AreEqual(0, col.RemovedBOCol.Count);
            Assert.AreEqual(0, col.PersistedBOCol.Count);
            Assert.AreEqual(0, col.MarkForDeletionBOs.Count);
            Assert.AreEqual(1, col.CreatedBOCol.Count);
        }

        [Test]
        public void TestAddMethod_RestoreBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            col.Add(myBO);
            myBO.Surname = TestUtil.CreateRandomString();
            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(col);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.Restore();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(col);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        [Test]
        public void TestAddMethod_RefreshAllBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();

            col.Add(myBO);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(col);
            Assert.IsFalse(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentPersistedAndAddedCollection(col);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        [Test]
        public void TestAddMethod_Refresh_LoadWithCriteria_BusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("Surname='bbb'", "");
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("aaa");

            col.Add(myBO);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(col);
            Assert.IsFalse(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(col);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        [Test]
        public void TestAddMethod_Remove_Added_NonPersisted_BusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson
                (TestUtil.CreateRandomString(), TestUtil.CreateRandomString());
            col.Add(myBO);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(col);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            col.Remove(myBO);

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(col);
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        [Test]
        public void TestAddMethod_RemoveAddedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            col.Add(myBO);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(col);
            Assert.IsFalse(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            col.Remove(myBO);

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(col);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        [Test]
        public void TestAddMethod_MarkForDeleteAddedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            col.Add(myBO);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(col);
            Assert.IsFalse(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            col.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(col);
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        #endregion

        #region Added Business Objects Marked for Deletion

        [Test]
        public void Test_Mark4Delete_Added_RefreshAll()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_LoadWCriteria_RefreshAll()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.Load("Surname=cc", "");
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_RestoreBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.Restore();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_RestoreBO_LoadWCriteria()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.Load("Surname=cc", "");
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.Restore();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_RemoveBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
        }
        [Test]
        public void Test_Mark4Delete_Added_Mark4DeleteBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_BO_Mark4Delete()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.MarkForDelete();

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_AddBo()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
        }
        [Test]
        public void Test_Mark4Delete_Added_RestoreAll()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.RestoreAll();

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
        }
        [Test]
        public void Test_Mark4Delete_Added_SaveAll()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
        }

        [Test]
        public void Test_Mark4Delete_Added_boSave()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.Save();

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
        }
        #endregion

        private static void AssertOneObjectInMarkForDeleteAndAddedCollection
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        private static void AssertAllCollectionsHaveNoItems(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        private static void AssertOneObjectInCurrentPersistedAndAddedCollection
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }
    }
}