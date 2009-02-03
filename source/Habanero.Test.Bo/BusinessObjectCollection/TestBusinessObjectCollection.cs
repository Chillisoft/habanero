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
using System.Collections;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.BO.BusinessObjectCollection
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
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        #endregion

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            SetupDBConnection();
        }

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
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count); 
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count); 
            //---------------Execute Test ----------------------
            cpCol.LoadAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(cpCol.Count, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count); 
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
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count); 
            //---------------Execute Test ----------------------
            cpCol.LoadAll();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(cpCol.Count, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count); 
        }



        //
        //        Clear a business object collection
        //This clears the current collection, persisted, removed, deleted and created list
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
            Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            cpCol.Clear();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.PersistedBusinessObjects.Count);
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
        public void Test_Clear_Clears_AddedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateUnsavedContactPerson(BOTestUtils.RandomString, BOTestUtils.RandomString);
            
            cpCol.AddedBusinessObjects.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
            //---------------Execute Test ----------------------
            cpCol.Clear();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
        }
        [Test]
        public void Test_Clear_Clears_DeletedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateUnsavedContactPerson(BOTestUtils.RandomString, BOTestUtils.RandomString);

            cpCol.MarkedForDeleteBusinessObjects.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.MarkedForDeleteBusinessObjects.Count);
            //---------------Execute Test ----------------------
            cpCol.Clear();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.MarkedForDeleteBusinessObjects.Count);
        }
        [Test]
        public void Test_Clear_Clears_RemovedCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO.DeleteAllContactPeople();
            CreateTwoSavedContactPeople();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateUnsavedContactPerson(BOTestUtils.RandomString, BOTestUtils.RandomString);
            cpCol.LoadAll();
            cpCol.Remove(cp);

            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(2, keyObjectHashTable.Count);

            //---------------Execute Test ----------------------
            cpCol.Clear();

            //---------------Test Result -----------------------
            Assert.AreEqual(0, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, keyObjectHashTable.Count);
        }

        public Hashtable GetKeyObjectHashTable(IBusinessObjectCollection cpCol)
        {
            return (Hashtable) ReflectionUtilities.GetPrivatePropertyValue(cpCol, "KeyObjectHashTable");
        }

        [Test]
        public void Test_ClearCurrentCollection_usingReflection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.DeleteAllContactPeople();
            IBusinessObjectCollection cpCol = CreateCollectionWith_OneBO();
            CreateTwoSavedContactPeople();
            cpCol.LoadAll();
            cpCol.CreateBusinessObject();
            cpCol.RemoveAt(0);
            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(3, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(3, cpCol.Count);
            Assert.AreEqual(3, keyObjectHashTable.Count);

            //---------------Execute Test ----------------------
            //cpCol.ClearCurrentCollection();
            ReflectionUtilities.ExecutePrivateMethod(cpCol, "ClearCurrentCollection");

            //---------------Test Result -----------------------
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.RemovedBusinessObjects.Count);
            Assert.AreEqual(3, cpCol.PersistedBusinessObjects.Count);
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, keyObjectHashTable.Count);
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
        public void Test_CreateBusinessObject_AlternateClassDef()
        {
            //---------------Set up test pack-------------------
            AddressTestBO addressTestBO = new AddressTestBO();

            ClassDef classDef = addressTestBO.ClassDef;
            ClassDef alternateClassDef = classDef.Clone();
            alternateClassDef.TypeParameter = TestUtil.GetRandomString();
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
            alternateClassDef.TypeParameter = TestUtil.GetRandomString();
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
            Assert.AreSame(bo1, col.FindByGuid(bo1.MyBoID.Value));
        }

        [Test]
        public void TestInstantiate()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            Assert.AreSame(ClassDef.ClassDefs[typeof (MyBO)], col.ClassDef);
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

            col.CancelEdits();
            Assert.AreEqual("Soap", col[0].Surname);
            Assert.AreEqual("Hope", col[1].Surname);

            contact1.MarkForDelete();
            contact2.MarkForDelete();
            col.SaveAll();
            Assert.AreEqual(0, col.Count);
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
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> clone = cpCol.Clone();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, clone.Count);
            Assert.AreEqual(0, clone.CreatedBusinessObjects.Count);
            Assert.AreEqual(0, clone.RemovedBusinessObjects.Count);
            Assert.AreEqual(1, clone.PersistedBusinessObjects.Count);
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
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> clone = cpCol.Clone();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, clone.Count);
            Assert.AreEqual(1, clone.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, clone.PersistedBusinessObjects.Count);
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
            Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> clone = cpCol.Clone();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, clone.Count);
            Assert.AreEqual(1, clone.RemovedBusinessObjects.Count);
            Assert.AreEqual(2, clone.PersistedBusinessObjects.Count);
        }

        [Test]
        public void Test_CloneBusinessObject_WithAddedObjects()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            cpCol.AddedBusinessObjects.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> clone = cpCol.Clone();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, clone.AddedBusinessObjects.Count);
        }

        [Test]
        public void Test_CloneBusinessObject_WithAddedObjects_AddedObjectsAlreadyAdded()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            cpCol.Add(cp);

            //---------------Assert Precondition----------------
            Assert.AreEqual(1, cpCol.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);

            //---------------Execute Test ----------------------
            BusinessObjectCollection<ContactPersonTestBO> clone = cpCol.Clone();

            //---------------Test Result -----------------------
            Assert.AreEqual(1, clone.AddedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.Count);
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
            createdCp.Surname = BOTestUtils.RandomString;

            cpCol.Clone();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(1, cpCol.PersistedBusinessObjects.Count);

            //---------------Execute Test ----------------------
            createdCp.Save();

            //---------------Test Result -----------------------
            Assert.AreEqual(2, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
            Assert.AreEqual(2, cpCol.PersistedBusinessObjects.Count);

        }



        private static BusinessObjectCollection<ContactPersonTestBO> CreateCollectionWith_OneBO()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO.CreateSavedContactPerson();
            return BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
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
    }
}