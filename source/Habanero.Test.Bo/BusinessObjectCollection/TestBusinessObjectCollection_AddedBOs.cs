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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectCollection
{
    [TestFixture]
    public class TestBusinessObjectCollection_AddedBOs //:TestBase
    {
        private bool _addedEventFired;
        private bool _removedEventFired;
        private DataAccessorInMemory _dataAccessor;
        private DataStoreInMemory _dataStore;

        #region Setup

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


        #region Added Business object in current cpCollection

        [Test]
        public void Test_AddMethod()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            RegisterForAddedEvent(cpCol);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.IsFalse(_addedEventFired);
            //---------------Execute Test ----------------------
            cpCol.Add(myBO);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count, "One object should be in the cpCollection");
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsTrue(_addedEventFired);
            Assert.AreEqual(myBO, cpCol[0], "Added object should be in the cpCollection");
        }

        [Test]
        public void Test_Add_NewBO_AddsToCreatedCollection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = ContactPersonTestBO.CreateUnsavedContactPerson
                (BOTestUtils.RandomString, BOTestUtils.RandomString);
            RegisterForAddedEvent(cpCol);

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
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            Collection<ContactPersonTestBO> cpCollection = new Collection<ContactPersonTestBO>();
            cpCollection.Add(myBO);
            cpCollection.Add(myBO2);
            cpCollection.Add(myBO3);
            //---------------Execute Test ----------------------
            cpCol.Add(cpCollection);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the cpCollection");
            Assert.AreEqual(myBO, cpCol[0], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO2, cpCol[1], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO3, cpCol[2], "Added object should be in the cpCollection");
        }


        [Test]
        public void Test_AddMethod_WithEnumerable_List()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            List<ContactPersonTestBO> list = new List<ContactPersonTestBO>();
            list.Add(myBO);
            list.Add(myBO2);
            list.Add(myBO3);
            //---------------Execute Test ----------------------
            cpCol.Add(list);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the cpCollection");
            Assert.AreEqual(myBO, cpCol[0], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO2, cpCol[1], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO3, cpCol[2], "Added object should be in the cpCollection");
        }

        [Test]
        public void Test_AddMethod_WithParamArray()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            //---------------Execute Test ----------------------
            cpCol.Add(myBO, myBO2, myBO3);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the cpCollection");
            Assert.AreEqual(myBO, cpCol[0], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO2, cpCol[1], "Added object should be in the cpCollection");
            Assert.AreEqual(myBO3, cpCol[2], "Added object should be in the cpCollection");
        }

        [Test]
        public void Test_AddMethod_WithCollection()
        {
            //---------------Set up test pack-------------------
            //ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = new ContactPersonTestBO();
            ContactPersonTestBO myBO2 = new ContactPersonTestBO();
            ContactPersonTestBO myBO3 = new ContactPersonTestBO();
            cpCol.Add(myBO, myBO2, myBO3);
            //-------Assert Preconditions
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the cpCollection");
            ///---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> cpColCopied =
                new BusinessObjectCollection<ContactPersonTestBO>();
            cpColCopied.Add(cpCol);
            //---------------Test Result ----------------------- - Result
            Assert.AreEqual(3, cpColCopied.Count, "Three objects should be in the copied cpCollection");
            Assert.AreEqual(3, cpColCopied.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpColCopied.AddedBusinessObjects.Count);
            Assert.AreEqual(myBO, cpColCopied[0], "Added object should be in the copied cpCollection");
            Assert.AreEqual(myBO2, cpColCopied[1], "Added object should be in the copied cpCollection");
            Assert.AreEqual(myBO3, cpColCopied[2], "Added object should be in the copied cpCollection");
        }

        [Test]
        public void Test_Add_PersistedBO()
        {
            //---------------Set up test pack-------------------
            //The persisted business objects should not be added since this is a normal cpCollection which does not 
            // modify the added objects alternate key etc unlike the RelatedBusinessObjectCollection
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO2 = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO3 = ContactPersonTestBO.CreateSavedContactPerson();
            RegisterForAddedEvent(cpCol);

            //-------Assert Preconditions
            Assert.AreEqual(0, cpCol.Count, "Three objects should be in the cpCollection");

            ///---------------Execute Test ----------------------
            cpCol.Add(myBO, myBO2, myBO3);

            //---------------Test Result ----------------------- - Result
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the copied cpCollection");
            Assert.AreEqual
                (3, cpCol.AddedBusinessObjects.Count,
                 "The persisted business objects should not be in the AddedList since this is a normal cpCollection which does not modify the added objects alternate key etc unlike the RelatedBusinessObjectCollection");
            Assert.IsTrue(_addedEventFired);
        }


        [Test]
        public void Test_Add_PersistedBO_RestoreAll()
        {
            //---------------Set up test pack-------------------
            //The persisted objects are added to the added cpCollection
            // when restore is called the added objects should be removed from the cpCollection.
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO2 = ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO myBO3 = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO, myBO2, myBO3);
            RegisterForRemovedEvent(cpCol);

            //-------Assert Preconditions
            Assert.AreEqual(3, cpCol.Count, "Three objects should be in the copied cpCollection");
            Assert.AreEqual(3, cpCol.AddedBusinessObjects.Count, "Three objects should be in the cpCollection");
            Assert.IsFalse(_removedEventFired);
            ///---------------Execute Test ----------------------
            cpCol.RestoreAll();

            //---------------Test Result ----------------------- - Result
            AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsTrue(_removedEventFired);
        }


        [Test]
        public void TestAddMethod_IgnoresAddWhenItemAlreadyExists()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> cpCol = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            cpCol.Add(myBO);
            RegisterForAddedEvent(cpCol);
            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsFalse(_addedEventFired);

            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsFalse(_addedEventFired);
        }

        [Test]
        public void TestAddMethod_PersistedObject_IgnoresAddWhenItemAlreadyExists()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            RegisterForAddedEvent(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(_addedEventFired);
            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(_addedEventFired);
        }

        [Test]
        public void TestAddMethod_SaveBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            myBO.Surname = TestUtil.GetRandomString();
            RegisterForAddedEvent(cpCol);
            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(_addedEventFired);

            //---------------Execute Test ----------------------
            myBO.Save();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentPersistedAndAddedCollection(cpCol);
            Assert.IsFalse(_addedEventFired);
        }


        [Test]
        public void TestAddMethod_SaveAllBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            myBO.Surname = TestUtil.GetRandomString();
            RegisterForAddedEvent(cpCol);
            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            Assert.IsFalse(_addedEventFired);
            //---------------Execute Test ----------------------
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentPersistedAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            Assert.IsFalse(_addedEventFired);
        }

        private static void AssertOneObjectInCurrentAndAddedCollection(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count, "One object should be in the cpCollection");
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        private static void AssertOneObjectInCurrentAndCreatedCollection(IBusinessObjectCollection cpCol)
        {
            Assert.AreEqual(1, cpCol.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count, "One object should be in the cpCollection");
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }

        [Test]
        public void TestAddMethod_RestoreBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            myBO.Surname = TestUtil.GetRandomString();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();

            //---------------Execute Test ----------------------
            myBO.CancelEdits();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void TestAddMethod_RefreshAllBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();

            cpCol.Add(myBO);
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();

            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentPersistedAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
        }

 

        [Test]
        public void TestAddMethod_Refresh_LoadWithCriteria_BusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.Load("Surname='bbb'", "");
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("aaa");

            cpCol.Add(myBO);
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void TestAddMethod_Remove_Added_NonPersisted_BusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateUnsavedContactPerson
                (TestUtil.GetRandomString(), TestUtil.GetRandomString());
            cpCol.Add(myBO);
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndCreatedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertRemovedEventFired();
            AssertAddedEventNotFired();
        }

        [Test]
        public void TestAddMethod_RemoveAddedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertRemovedEventFired();
            AssertAddedEventNotFired();
        }

        [Test]
        public void TestAddMethod_MarkForDeleteAddedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            cpCol.Add(myBO);
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
            //---------------Execute Test ----------------------
            cpCol.MarkForDelete(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertRemovedEventFired();
        }

        #endregion

        #region Added Business Objects Marked for Deletion

        [Test]
        public void Test_MarkForDelete_Added_RefreshAll()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.Load(new Criteria("Surname", Criteria.ComparisonOp.Equals, TestUtil.GetRandomString()), "" );
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_MarkForDelete_Added_LoadWCriteria_RefreshAll()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.Load("Surname=cc", "");
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Refresh();

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_MarkForDelete_Added_RestoreBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();

            //---------------Execute Test ----------------------
            myBO.CancelEdits();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedEventFired();
            AssertRemovedEventNotFired();
        }

        [Test]
        public void Test_MarkForDelete_Added_RestoreBO_LoadWCriteria()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.Load("Surname=cc", "");
            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            myBO.CancelEdits();

            //---------------Test Result -----------------------
            AssertOneObjectInCurrentAndAddedCollection(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedEventFired();
            AssertRemovedEventNotFired();
        }

        [Test]
        public void Test_MarkForDelete_Added_RemoveBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            RegisterForAddedAndRemovedEvents(cpCol);
            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Remove(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_MarkForDelete_Added_MarkForDeleteBO()
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
        public void Test_MarkForDelete_Added_BO_MarkForDelete()
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
        public void Test_MarkForDelete_Added_AddBo()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.Add(myBO);

            //---------------Test Result -----------------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);
            AssertAddedAndRemovedEventsNotFired();
        }

        [Test]
        public void Test_MarkForDelete_Added_RestoreAll()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO myBO = ContactPersonTestBO.CreateSavedContactPerson("BB");
            cpCol.Add(myBO);
            myBO.MarkForDelete();
            RegisterForAddedAndRemovedEvents(cpCol);

            //---------------Assert Precondition----------------
            AssertOneObjectInMarkForDeleteAndAddedCollection(cpCol);
            Assert.IsTrue(myBO.Status.IsDirty);

            //---------------Execute Test ----------------------
            cpCol.RestoreAll();

            //---------------Test Result -----------------------
            AssertAllCollectionsHaveNoItems(cpCol);
            Assert.IsFalse(myBO.Status.IsDirty);
            AssertAddedEventFired();
            AssertRemovedEventFired();
        }

        [Test]
        public void Test_MarkForDelete_Added_SaveAll()
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
        public void Test_MarkForDelete_Added_boSave()
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
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
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

        private static void AssertOneObjectInCurrentPersistedAndAddedCollection
            (BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        private void AssertAddedAndRemovedEventsNotFired()
        {
            AssertAddedEventNotFired();
            AssertRemovedEventNotFired();
        }

        private void AssertRemovedEventNotFired()
        {
            Assert.IsFalse(_removedEventFired, "Removed event should not be fired");
        }

        private void AssertAddedEventNotFired()
        {
            Assert.IsFalse(_addedEventFired, "Added event should not be fired");
        }

        private void AssertRemovedEventFired()
        {
            Assert.IsTrue(_removedEventFired, "Removed event should be fired");
        }

        private void AssertAddedEventFired()
        {
            Assert.IsTrue(_addedEventFired, "Added event should be fired");
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
    }
}