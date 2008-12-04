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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestBusinessObjectCollection.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectCollection : TestUsingDatabase
    {
        #region Setup/Teardown

        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
        }

        #endregion

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            SetupDBConnection();
        }
        [Test]

        private static ContactPersonTestBO CreateContactPersonTestBO()
        {
            ContactPersonTestBO bo = new ContactPersonTestBO();
            string newSurname = Guid.NewGuid().ToString();
            bo.Surname = newSurname;
            bo.Save();
            return bo;
        }

        private bool _addedEventFired;

        public class MyDatabaseConnectionStub : DatabaseConnection
        {
            public MyDatabaseConnectionStub() : base("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection")
            {
            }

            public override string LeftFieldDelimiter
            {
                get { return ""; }
            }

            public override string RightFieldDelimiter
            {
                get { return ""; }
            }

            public override string GetLimitClauseForBeginning(int limit)
            {
                return "TOP " + limit;
            }
        }

        private static void AssertNotContains(ContactPersonTestBO cp1, List<ContactPersonTestBO> col)
        {
            col.ForEach(delegate(ContactPersonTestBO bo)
                    {
                        if (ReferenceEquals(bo, cp1)) Assert.Fail("Should not contain object");
                    });
        }

        //Load a collection from the database.
        // Create a new business object.
        // The related collection will now contain the newly added business object.
        // Remove a business object or mark a business object as deleted.
        //  A loaded business object collection will remove the business object from the collection and will
        //    add it to its Deleted Collection.
        // A related collection will be dirty if it has any removed items, created items or deleted items.
        // A related collection will be dirty if it has any dirty objects.
        // A business object will be dirty if it has a dirty related collection.

        //The added event should be fired when a Business object is created
        //The added event should be fired when a busienss object is added to the collection.
        //Collection that represents the state in the BO is called the persisted collection.
        [Test]
        public void Test_LoadAll_LoadsAllCollectionsAppropriately_OnePerson()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO.DeleteAllContactPeople();
            CreateSavedContactPerson();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBOColl.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            //Assert.AreEqual(0, cpCol..Count); TODO: Removed/deleted bo should be 0
            //---------------Execute Test ----------------------
            cpCol.LoadAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(cpCol.Count, cpCol.PersistedBOColl.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            //Assert.AreEqual(0, cpCol..Count); TODO: Removed/deleted bo should be 0
        }

        [Test]
        public void Test_LoadAll_LoadsAllCollectionsAppropriately_TwoPerson()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO.DeleteAllContactPeople();
            CreateSavedContactPerson();
            CreateSavedContactPerson();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBOColl.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            //Assert.AreEqual(0, cpCol..Count); TODO: Removed/deleted bo should be 0
            //---------------Execute Test ----------------------
            cpCol.LoadAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(cpCol.Count, cpCol.PersistedBOColl.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            //Assert.AreEqual(0, cpCol..Count); TODO: Removed/deleted bo should be 0
        }

        [Test]
        public void Test_ClearClearsPersistedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO.DeleteAllContactPeople();
            CreateTwoSavedContactPeople();
            cpCol.LoadAll();

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.PersistedBOColl.Count);

            //---------------Execute Test ----------------------
            cpCol.Clear();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBOColl.Count);
        }
        [Test]
        public void Test_ClearClearsCreatedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO.DeleteAllContactPeople();
            CreateTwoSavedContactPeople();
            cpCol.CreateBusinessObject();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.Clear();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        [Test]
        public void Test_Add_NewBO_AddsToCreatedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO.DeleteAllContactPeople();
            CreateTwoSavedContactPeople();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateUnsavedContactPerson(TestUtils.RandomString, TestUtils.RandomString);

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.Add(cp);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }

        private static void CreateSavedContactPerson()
        {
            ContactPersonTestBO.CreateSavedContactPerson();
        }

        private static void CreateTwoSavedContactPeople()
        {
            CreateSavedContactPerson();
            CreateSavedContactPerson();
        }

        [Test]
        public void Test_CreateBusObject_AddedToTheCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();
            int beginningCount = cpCol.Count;
            _addedEventFired = false;
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            //---------------Assert Precondition----------------
            Assert.AreEqual(beginningCount, cpCol.Count);
            Assert.IsFalse(_addedEventFired);
            Assert.AreEqual(beginningCount, cpCol.PersistedBOColl.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.AreEqual(beginningCount + 1, cpCol.Count);
            Assert.AreEqual(beginningCount, cpCol.PersistedBOColl.Count);
            Assert.IsFalse(cpCol.PersistedBOColl.Contains(newCP));
            Assert.Contains(newCP, cpCol);
            Assert.IsTrue(_addedEventFired);
        }

        [Test]
        public void Test_AddedEvent_FiringDoesNotFire_WhenSavingCreatedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            _addedEventFired = false;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();
            int beginningCount = cpCol.Count;

            //---------------Assert Preconditions --------------
            Assert.IsFalse(_addedEventFired);
            Assert.Contains(newCP, cpCol);

            //---------------Execute Test ----------------------
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };
            newCP.Save();

            //---------------Test Result -----------------------
            Assert.IsFalse(_addedEventFired);
            Assert.AreEqual(beginningCount, cpCol.Count);
        }

        [Test]
        public void Test_AddedEvent_NotFiringWhenRefreshing()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            _addedEventFired = false;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();
            newCP.Save();

            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };
            //---------------Assert Preconditions --------------
            Assert.IsFalse(_addedEventFired);

            //---------------Execute Test ----------------------
            cpCol.Refresh();
            //---------------Test Result -----------------------
            Assert.IsFalse(_addedEventFired);
        }

        [Test]
        public void Test_AddMethod()
        {
            //Setup
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            //Fixture
            col.Add(myBO);
            //Assert
            Assert.AreEqual(1, col.Count, "One object should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
        }

        [Test]
        public void Test_AddMethod_WithEnumerable_Collection()
        {
            //Setup
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            MyBO myBO3 = new MyBO();
            Collection<MyBO> collection = new Collection<MyBO>();
            collection.Add(myBO);
            collection.Add(myBO2);
            collection.Add(myBO3);
            //Fixture
            col.Add(collection);
            //Assert
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
            Assert.AreEqual(myBO2, col[1], "Added object should be in the collection");
            Assert.AreEqual(myBO3, col[2], "Added object should be in the collection");
        }

        [Test]
        public void Test_AddMethod_WithEnumerable_List()
        {
            //Setup
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            MyBO myBO3 = new MyBO();
            List<MyBO> list = new List<MyBO>();
            list.Add(myBO);
            list.Add(myBO2);
            list.Add(myBO3);
            //Fixture
            col.Add(list);
            //Assert
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
            Assert.AreEqual(myBO2, col[1], "Added object should be in the collection");
            Assert.AreEqual(myBO3, col[2], "Added object should be in the collection");
        }

        [Test]
        public void Test_AddMethod_WithParamArray()
        {
            //Setup
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            MyBO myBO3 = new MyBO();
            //Fixture
            col.Add(myBO, myBO2, myBO3);
            //Assert
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            Assert.AreEqual(myBO, col[0], "Added object should be in the collection");
            Assert.AreEqual(myBO2, col[1], "Added object should be in the collection");
            Assert.AreEqual(myBO3, col[2], "Added object should be in the collection");
        }

        [Test]
        public void Test_AddMethod_WithCollection()
        {
            //Setup
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO myBO = new MyBO();
            MyBO myBO2 = new MyBO();
            MyBO myBO3 = new MyBO();
            col.Add(myBO, myBO2, myBO3);
            //-------Assert Preconditions
            Assert.AreEqual(3, col.Count, "Three objects should be in the collection");
            //Execute test
            BusinessObjectCollection<MyBO> colCopied = new BusinessObjectCollection<MyBO>();
            colCopied.Add(col);
            //Assert - Result
            Assert.AreEqual(3, colCopied.Count, "Three objects should be in the copied collection");
            Assert.AreEqual(myBO, colCopied[0], "Added object should be in the copied collection");
            Assert.AreEqual(myBO2, colCopied[1], "Added object should be in the copied collection");
            Assert.AreEqual(myBO3, colCopied[2], "Added object should be in the copied collection");
        }

        [Test]
        public void Test_CreateBusinessObject()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            Assert.IsTrue(newCP.Status.IsNew);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }

        [Test]
        public void Test_CreateBusinessObject_AlternateClassDef()
        {
            //---------------Set up test pack-------------------
            AddressTestBO addressTestBO = new AddressTestBO();

            ClassDef classDef = addressTestBO.ClassDef;
            ClassDef alternateClassDef = classDef.Clone();
            alternateClassDef.TypeParameter = TestUtil.CreateRandomString();
            BusinessObjectCollection<AddressTestBO> addressCol = new BusinessObjectCollection<AddressTestBO>();
            addressCol.ClassDef = alternateClassDef;

            //---------------Execute Test ----------------------

            //this should work because AddressTestBO has a constructor that takes a ClassDef as parameter
            AddressTestBO newCP = addressCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.AreEqual(alternateClassDef, newCP.ClassDef);
        }

        [Test, ExpectedException(typeof(HabaneroDeveloperException))]
        public void Test_CreateBusinessObject_AlternateClassDef_NoConstructor()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ClassDef alternateClassDef = classDef.Clone();
            alternateClassDef.TypeParameter = TestUtil.CreateRandomString();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.ClassDef = alternateClassDef;

            //---------------Execute Test ----------------------

            //this should not work because ContactPersonTestBO does not have a constructor that takes a ClassDef as parameter
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.AreEqual(alternateClassDef, newCP.ClassDef);
        }

        [Test]
        public void Test_FindByGuid()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo1 = new MyBO();
            col.Add(bo1);
            col.Add(new MyBO());
            Assert.AreSame(bo1, col.FindByGuid(bo1.MyBoID));
        }

        [Test]
        public void TestInstantiate()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            Assert.AreSame(ClassDef.ClassDefs[typeof (MyBO)], col.ClassDef);
        }
        public void TestPersistOfCreatedBusinessObjects()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_DeleteRelated();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();
            newCP.FirstName = Guid.NewGuid().ToString();

            //---------------Execute Test ----------------------
            cpCol.SaveAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);

        }
        [Test]
        public void Test_CreatedBusinessObject_Persist()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();

            newCP.Save();
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);
        }

        [Test]
        public void Test_CreatedBusinessObjects_Saved_NotRegisteredForevents()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.Save();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);

            //---------------Execute Test ----------------------
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.Save();            

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
        }

        [Test]
        public void Test_CreatedBusinessObjects_Restored_NotRegisteredForevents()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = TestUtil.CreateRandomString();
            newCP.Restore();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);

            //---------------Execute Test ----------------------
            newCP.Restore();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
        }

        [Test]
        public void Test_RefreshCollectionDoesNotRefreshDirtyOject()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorDB();
            ContactPersonTestBO.DeleteAllContactPeople();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO cp1 = CreateContactPersonTestBO();
            CreateContactPersonTestBO();
            CreateContactPersonTestBO();
            col.LoadAll();
            string newSurname = Guid.NewGuid().ToString();

            //--------------------Assert Preconditions----------
            Assert.AreEqual(3, col.Count);

            //---------------Execute Test ----------------------
            cp1.Surname = newSurname;
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreEqual(newSurname, cp1.Surname);
            Assert.IsTrue(cp1.Status.IsDirty);
        }

        [Test]
        public void Test_RefreshCollectionRefreshesNonDirtyObjects()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorDB();
            ContactPersonTestBO.DeleteAllContactPeople();

            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            ContactPersonTestBO cp1 = CreateContactPersonTestBO();
            BusinessObjectManager.Instance.ClearLoadedObjects();

            CreateContactPersonTestBO();
            CreateContactPersonTestBO();
            col.LoadAll();
            string newSurname = Guid.NewGuid().ToString();
            cp1.Surname = newSurname;
            cp1.Save();
            ContactPersonTestBO secondInstanceOfCP1 = col.FindByGuid(cp1.ContactPersonID);

            //--------------------Assert Preconditions----------
            AssertNotContains(cp1, col);
            Assert.AreEqual(3, col.Count);
            Assert.AreEqual(newSurname, cp1.Surname);
            Assert.AreNotSame(secondInstanceOfCP1, cp1);
            Assert.AreNotEqual(newSurname, secondInstanceOfCP1.Surname);
            Assert.IsFalse(cp1.Status.IsDirty);
            //---------------Execute Test ----------------------
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(3, col.Count);
            Assert.AreNotSame(secondInstanceOfCP1, cp1);
            Assert.AreEqual(newSurname, secondInstanceOfCP1.Surname);
        }

        [Test]
        public void Test_RestoreAll()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contact1 = new ContactPersonTestBO();
            contact1.Surname = "Soap";
            ContactPersonTestBO contact2 = new ContactPersonTestBO();
            contact2.Surname = "Hope";
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Add(contact1);
            col.Add(contact2);
            col.SaveAll();

            Assert.AreEqual("Soap", col[0].Surname);
            Assert.AreEqual("Hope", col[1].Surname);

            contact1.Surname = "Cope";
            contact2.Surname = "Pope";
            Assert.AreEqual("Cope", col[0].Surname);
            Assert.AreEqual("Pope", col[1].Surname);

            col.RestoreAll();
            Assert.AreEqual("Soap", col[0].Surname);
            Assert.AreEqual("Hope", col[1].Surname);

            contact1.Delete();
            contact2.Delete();
            col.SaveAll();
            Assert.AreEqual(0, col.Count);
        }

        [Test]
        public void Test_RestoreOfACreatedBusinessObject_RemovesItFromTheCurrentAndCreatedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            //TODO: Persisted col should be 0
            //---------------Execute Test ----------------------
            newCP.Restore();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
        }

        [Test]
        public void Test_FindIncludesCreatedBusinessObjects()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
            ContactPersonTestBO cp2 = cpCol.CreateBusinessObject();

            //---------------Execute Test ----------------------
            ContactPersonTestBO foundCp = cpCol.Find(cp2.ID.ToString());

            //---------------Test Result -----------------------
            Assert.IsNotNull(foundCp);
            Assert.AreSame(cp2, foundCp);
        }

        [Test]
        public void Test_CloneBusinessObject()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
           
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);

            //---------------Execute Test ----------------------
            //TODO: persisted, created, deleted, and removed BO's should be cloned. 
            BusinessObjectCollection<ContactPersonTestBO> clone = cpCol.Clone();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, clone.Count);
            Assert.AreEqual(0, clone.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, clone.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, clone.PersistedBOColl.Count);
        }

        [Test]
        public void Test_CloneBusinessObject_WithCreateObjects()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");

            cpCol.CreateBusinessObject();

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);

            //---------------Execute Test ----------------------
            //TODO: persisted, created, deleted, and removed BO's should be cloned. 
            BusinessObjectCollection<ContactPersonTestBO> clone = cpCol.Clone();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, clone.Count);
            Assert.AreEqual(1, clone.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, clone.PersistedBOColl.Count);
        }

        [Test]
        public void Test_CloneBusinessObject_WithRemovedObjects()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO.CreateSavedContactPerson();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");

            cpCol.Remove(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.PersistedBOColl.Count);

            //---------------Execute Test ----------------------
            //TODO: persisted, created, deleted, and removed BO's should be cloned. 
            BusinessObjectCollection<ContactPersonTestBO> clone = cpCol.Clone();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, clone.Count);
            Assert.AreEqual(1, clone.RemovedBusinessObjects.Count);
            Assert.AreEqual(2, clone.PersistedBOColl.Count);
        }

        [Test]
        public void Test_CloneBusinessObject_WithCreateObjects_CreatedObjectsStillRespondToEvents()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");

            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;

            //TODO: persisted, created, deleted, and removed BO's should be cloned. 
            BusinessObjectCollection<ContactPersonTestBO> clone = cpCol.Clone();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);

            //---------------Execute Test ----------------------
            createdCp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.PersistedBOColl.Count);
        }

        [Test]
        public void Test_RefreshBusinessObject_WithCreateObjects_CreatedObjectsStillRespondToEvents()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");

            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;

            //TODO: persisted, created, deleted, and removed BO's should be cloned. 
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);

            //---------------Execute Test ----------------------
            cpCol.Refresh();
            createdCp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.PersistedBOColl.Count);
        }

        [Test]
        public void Test_SaveCreatedBo_UpdatesBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");

            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);

            //---------------Execute Test ----------------------
            createdCp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.PersistedBOColl.Count);
        }

        [Test]
        public void Test_RemoveCreatedBo()
        {
            //If you remove a created business object that is not yet persisted then
            //-- remove from the restored and saved event.
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();

            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);
            Assert.IsTrue(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.Remove(createdCp);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);
            Assert.IsFalse(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        }

        [Test]
        public void Test_BusinessObjectDeleted_IndependentlyOfCollection()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO cp = cpCol[0];

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);
            Assert.IsTrue(cpCol.Contains(cp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cp.Delete();
            cp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.PersistedBOColl.Count);
            Assert.IsFalse(cpCol.Contains(cp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        }

        [Test]
        public void Test_RemoveCreatedBo_DeregistersForSaveEvent()
        {
            //If you remove a created business object that is not yet persisted then
            //-- remove from the restored and saved event.
            //-- when the object is saved it should be independent of the collection.
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = CreateCollectionWith_OneBO();
            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;
            cpCol.Remove(createdCp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);
            Assert.IsFalse(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            createdCp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);
            Assert.IsFalse(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        }

        private static BusinessObjectCollection<ContactPersonTestBO> CreateCollectionWith_OneBO()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.CreateSavedContactPerson();
            return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
        }

        [Test]
        public void Test_RemoveCreatedBO_DeregisteresFromRestoredEvent()
        {
            //If you remove a created business object that is not yet persisted then
            //-- remove from the restored and saved event.
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
            ContactPersonTestBO createdCp = cpCol.CreateBusinessObject();
            createdCp.Surname = TestUtils.RandomString;

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);
            Assert.IsTrue(cpCol.Contains(createdCp));

            //---------------Execute Test ----------------------
            cpCol.Remove(createdCp);
            createdCp.Restore();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBOColl.Count);
            Assert.IsFalse(cpCol.Contains(createdCp));
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
        }

        [Test]
        public void Test_Remove_AddsToRemovedCollection()
        {
            //-----Create Test pack---------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
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
        public void Test_RemoveRelatedObject_PersistToDB()
        {
            //-----Create Test pack---------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
            
            //--------------Assert Preconditions--------
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);

            //-----Run tests----------------------------
            cpCol.Remove(cp);
            cpCol.SaveAll();

            ////-----Test results-------------------------
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
        }

        [Test]
        public void Test_Refresh_WithRemovedBOs()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
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

        [Test]
        public void Test_RemoveGuestAttendee_AlreadyInRemoveCollection()
        {
            //-----Create Test pack---------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");

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
    }
}