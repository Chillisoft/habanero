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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Base.Util;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;
// ReSharper disable InconsistentNaming
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
            var asm = Assembly.GetExecutingAssembly();
            ConfigurationManager.Initialise(asm);

            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        #endregion

        private static ContactPersonTestBO CreateContactPersonTestBO()
        {
            ContactPersonTestBO bo = new ContactPersonTestBO();
            string newSurname = Guid.NewGuid().ToString();
            bo.Surname = newSurname;
            bo.Save();
            return bo;
        }

        private bool _addedEventFired;

/*        public class MyDatabaseConnectionStub : DatabaseConnection
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

            [Obsolete("please use the SqlFormatter directly")]
            public override string GetLimitClauseForBeginning(int limit)
            {
                return "TOP " + limit;
            }

            public override IParameterNameGenerator CreateParameterNameGenerator() {
                return new ParameterNameGenerator("?");
            }
        }*/


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
            var cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
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
        //
        //        Clear a business object collection
        //This clears the current collection, persisted, removed, deleted and created list
        [Test]
        public void Test_Clear_ShouldClearTheTimeLastLoaded()
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
            Assert.IsNull(cpCol.TimeLastLoaded);
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

        private static Hashtable GetKeyObjectHashTable(IBusinessObjectCollection cpCol)
        {
            return (Hashtable) ReflectionUtilities.GetPrivatePropertyValue(cpCol, "KeyObjectHashTable");
        }

        [Test]
        public void Test_ClearCurrentCollection_UsingReflection()
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
            //ReflectionUtilities.ExecutePrivateMethod(cpCol, "ClearCurrentCollection");
            BOColLoaderHelper.ClearCurrentCollection(cpCol);
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
            BusinessObjectCollection<AddressTestBO> addressCol = new BusinessObjectCollection<AddressTestBO> {ClassDef = alternateClassDef};

            //---------------Execute Test ----------------------

            //this should work because AddressTestBO has a constructor that takes a ClassDef as parameter
            AddressTestBO newCP = addressCol.CreateBusinessObject();

            //---------------Test Result -----------------------
            Assert.AreEqual(alternateClassDef, newCP.ClassDef);
        }

        [Test]
        public void Test_CreateBusinessObject_AlternateClassDef_NoConstructor_ShouldThrowException()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = (ClassDef) OrganisationTestBO.LoadDefaultClassDef();
            ClassDef alternateClassDef = classDef.Clone();
            alternateClassDef.TypeParameter = TestUtil.GetRandomString();
            BusinessObjectCollection<OrganisationTestBO> orgCol = new BusinessObjectCollection<OrganisationTestBO>
                                                                      {ClassDef = alternateClassDef};

            //---------------Execute Test ----------------------
            try
            {

                //this should not work because OrganisationTestBO does not have a constructor that takes a ClassDef as parameter
                orgCol.CreateBusinessObject();

                Assert.Fail("Expected to throw an HabaneroDeveloperException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("There was a problem creating a Habanero.Test.BO.OrganisationTestBO", ex.Message);
            }
        }

        [Test]
        public void Test_FindByGuid()
        {
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo1 = new MyBO();
            col.Add(bo1);
            col.Add(new MyBO());
            Assert.AreSame(bo1, col.Find(bo1.MyBoID.GetValueOrDefault()));
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
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            
            ContactPersonTestBO cp1 = CreateContactPersonTestBO();
            col.Add(cp1);
            string newSurname = Guid.NewGuid().ToString();
            //--------------------Assert Preconditions----------
            Assert.Contains(cp1 , col, "cp1 should be saved to the database");
            //---------------Execute Test ----------------------
            cp1.Surname = newSurname;
            col.Refresh();

            //---------------Test Result -----------------------
            Assert.AreEqual(newSurname, cp1.Surname);
            Assert.IsTrue(cp1.Status.IsDirty);
        }

        [Test]
        public void Test_RestoreAll()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contact1 = new ContactPersonTestBO {Surname = "Soap"};
            ContactPersonTestBO contact2 = new ContactPersonTestBO {Surname = "Hope"};
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO> {contact1, contact2};
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
            IBusinessObject foundCp = cpCol.Find(cp2.ID.ObjectID);

            //---------------Test Result -----------------------
            Assert.IsNotNull(foundCp);
            Assert.AreSame(cp2, foundCp);
        } 
        [Test]
        public void Test_FindIncludesCreatedBusinessObjects_UsingGeneric()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
            ContactPersonTestBO cp2 = cpCol.CreateBusinessObject();

            //---------------Execute Test ----------------------
            ContactPersonTestBO foundCp = cpCol.Find(cp2.ID.ObjectID);

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
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO> {cp};

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
        

        [Test]
        public void Test_Add_UnsavedBusinessObjectToCollection()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            
            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, keyObjectHashTable.Count);
            //--------------- Execute Test ----------------------
            cpCol.Add(person);
            //--------------- Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, keyObjectHashTable.Count);
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ID.ObjectID));
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ID.PreviousObjectID));
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ContactPersonID));
        }

        [Test]
        public void Test_Contains_True_UnsavedBusinessObject_InCollection()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            cpCol.Add(person);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ID.ObjectID));
            //--------------- Execute Test ----------------------
            bool contains = cpCol.Contains(person);
            //--------------- Test Result -----------------------
            Assert.IsTrue(contains);
        }

        [Test]
        public void Test_Contains_False_UnsavedBusinessObject_NotInCollection()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            cpCol.Add(ContactPersonTestBO.CreateUnsavedContactPerson());
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsFalse(keyObjectHashTable.ContainsKey(person.ID.ObjectID));
            //--------------- Execute Test ----------------------
            bool contains = cpCol.Contains(person);
            //--------------- Test Result -----------------------
            Assert.IsFalse(contains);
        }

        [Test]
        public void Test_Remove_UnsavedBusinessObjectFromCollection()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            cpCol.Add(person);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, keyObjectHashTable.Count);
            Assert.IsTrue(cpCol.Contains(person));
            //--------------- Execute Test ----------------------
            cpCol.Remove(person);
            //--------------- Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, keyObjectHashTable.Count);
        }

        [Test]
        public void Test_Add_UnsavedBusinessObjectToCollection_ChangePrimaryKeyPropertyValue()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            cpCol.Add(person);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, keyObjectHashTable.Count);
            Assert.IsTrue(cpCol.Contains(person));
            //--------------- Execute Test ----------------------
            person.ContactPersonID = Guid.NewGuid();
            //--------------- Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, keyObjectHashTable.Count);
            Assert.IsTrue(cpCol.Contains(person));
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ID.ObjectID));
            Assert.IsFalse(keyObjectHashTable.ContainsKey(person.ID.PreviousObjectID));
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ContactPersonID));
        }

        [Test]
        public void Test_MarkForDelete_RemovesFromCollection()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO person = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.Add(person);
            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, keyObjectHashTable.Count);
            //--------------- Execute Test ----------------------
            person.MarkForDelete();
            //--------------- Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, keyObjectHashTable.Count);
        }



        [Test]
        public void Test_Add_UnsavedBusinessObjectToCollection_CompositeKey()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson_NoFirstNameProp();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, keyObjectHashTable.Count);
            //--------------- Execute Test ----------------------
            cpCol.Add(person);
            //--------------- Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, keyObjectHashTable.Count);
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ID.ObjectID));
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ID.PreviousObjectID));
        }

        [Test]
        public void Test_Contains_True_UnsavedBusinessObject_InCollection_CompositeKey()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson_NoFirstNameProp();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            cpCol.Add(person);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ID.ObjectID));
            //--------------- Execute Test ----------------------
            bool contains = cpCol.Contains(person);
            //--------------- Test Result -----------------------
            Assert.IsTrue(contains);
        }

        [Test]
        public void Test_Contains_False_UnsavedBusinessObject_NotInCollection_CompositeKey()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson_NoFirstNameProp();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            cpCol.Add(ContactPersonTestBO.CreateUnsavedContactPerson_NoFirstNameProp());
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.IsFalse(keyObjectHashTable.ContainsKey(person.ID.ObjectID));
            //--------------- Execute Test ----------------------
            bool contains = cpCol.Contains(person);
            //--------------- Test Result -----------------------
            Assert.IsFalse(contains);
        }

        [Test]
        public void Test_Remove_UnsavedBusinessObjectFromCollection_CompositeKey()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson_NoFirstNameProp();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            cpCol.Add(person);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, keyObjectHashTable.Count);
            Assert.IsTrue(cpCol.Contains(person));
            //--------------- Execute Test ----------------------
            cpCol.Remove(person);
            //--------------- Test Result -----------------------
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, keyObjectHashTable.Count);
        }

        [Test]
        public void Test_Add_UnsavedBusinessObjectToCollection_ChangePrimaryKeyPropertyValue_CompositeKey()
        {
            //--------------- Set up test pack ------------------
            ContactPersonTestBO.LoadClassDefWithCompositePrimaryKey();
            ContactPersonTestBO person = ContactPersonTestBO.CreateUnsavedContactPerson_NoFirstNameProp();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();

            Hashtable keyObjectHashTable = GetKeyObjectHashTable(cpCol);
            cpCol.Add(person);
            //--------------- Test Preconditions ----------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, keyObjectHashTable.Count);
            Assert.IsTrue(cpCol.Contains(person));
            //--------------- Execute Test ----------------------
            person.ContactPersonID = Guid.NewGuid();
            //--------------- Test Result -----------------------
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(1, keyObjectHashTable.Count);
            Assert.IsTrue(cpCol.Contains(person));
            Assert.IsTrue(keyObjectHashTable.ContainsKey(person.ID.ObjectID));
        }

        [Test]
        public void Test_TotalCountAvailableForPaging_OriginalValue()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            //-------------Assert Preconditions -------------

            //---------------Execute Test ----------------------
            int totalNumberOfRecords = cpCol.TotalCountAvailableForPaging;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, totalNumberOfRecords);
        }

        [Test]
        public void Test_TotalCountAvailableForPaging_SetAndGet()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            int totalNumberOfRecords = TestUtil.GetRandomInt();
            //-------------Assert Preconditions -------------
            Assert.AreEqual(0, cpCol.TotalCountAvailableForPaging);
            //---------------Execute Test ----------------------
            cpCol.TotalCountAvailableForPaging = totalNumberOfRecords;
            //---------------Test Result -----------------------
            Assert.AreEqual(totalNumberOfRecords, cpCol.TotalCountAvailableForPaging);
        }


        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAfterStart_LimitBeyondEnd_RefreshWithAdditionalBO()
        {
            const int totalRecords = 5;
            const int firstRecord = 3;
            const int limit = 4;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            ContactPersonTestBO[] contactPersonTestBOsPlusOne = new ContactPersonTestBO[totalRecords + 1];
            contactPersonTestBOs.CopyTo(contactPersonTestBOsPlusOne, 0);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            contactPersonTestBOsPlusOne[totalRecords] = ContactPersonTestBO.CreateSavedContactPerson
                ("ZZZZZZZZZZZZZZZZZ");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.AreEqual(totalRecords + 1, contactPersonTestBOsPlusOne.Length);
            //---------------Execute Test ----------------------
            col.Refresh();
            //---------------Test Result -----------------------
            totalNoOfRecords++;
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords + 1, 2 + 1, contactPersonTestBOsPlusOne, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAfterStart_LimitNegative()
        {
            const int totalRecords = 7;
            const int firstRecord = 3;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 4, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAfterStart_LimitZero()
        {
            const int totalRecords = 7;
            const int firstRecord = 3;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAtEnd_LimitEqualsEnd()
        {
            const int totalRecords = 4;
            const int firstRecord = totalRecords - 1;
            const int limit = 1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAtEnd_LimitBeyondEnd()
        {
            const int totalRecords = 5;
            const int firstRecord = totalRecords - 1;
            const int limit = 3;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 1, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAtEnd_LimitBeyondEnd_RefreshWithAdditionalBO()
        {
            const int totalRecords = 5;
            const int firstRecord = totalRecords - 1;
            const int limit = 3;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            ContactPersonTestBO[] contactPersonTestBOsPlusOne = new ContactPersonTestBO[totalRecords + 1];
            contactPersonTestBOs.CopyTo(contactPersonTestBOsPlusOne, 0);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            contactPersonTestBOsPlusOne[totalRecords] = ContactPersonTestBO.CreateSavedContactPerson
                ("ZZZZZZZZZZZZZZZZZ");
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, col.Count);
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            Assert.AreEqual(totalRecords + 1, contactPersonTestBOsPlusOne.Length);
            //---------------Execute Test ----------------------
            col.Refresh();
            //---------------Test Result -----------------------
            totalNoOfRecords++;
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords + 1, 2, contactPersonTestBOsPlusOne, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAtEnd_LimitNegative()
        {
            const int totalRecords = 7;
            const int firstRecord = totalRecords - 1;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 1, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAtEnd_LimitZero()
        {
            const int totalRecords = 7;
            const int firstRecord = totalRecords - 1;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, limit, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAfterEnd_LimitBeyondEnd()
        {
            const int totalRecords = 3;
            const int firstRecord = 5;
            const int limit = 2;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAfterEnd_LimitNegative()
        {
            const int totalRecords = 4;
            const int firstRecord = 4;
            const int limit = -1;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_LoadWithLimit_FirstAfterEnd_LimitZero()
        {
            const int totalRecords = 3;
            const int firstRecord = 4;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            int totalNoOfRecords;
            col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
            //---------------Test Result -----------------------
            AssertLimitedResultsCorrect
                (firstRecord, limit, totalRecords, 0, contactPersonTestBOs, col, totalNoOfRecords);
        }

        [Test]
        public void Test_LoadWithLimit_FirstNegative_ThrowsError()
        {
            const int totalRecords = 3;
            const int firstRecord = -1;
            const int limit = 0;
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO[] contactPersonTestBOs = CreateSavedSortedContactPeople(totalRecords);
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(totalRecords, contactPersonTestBOs.Length);
            //---------------Execute Test ----------------------
            try
            {
                int totalNoOfRecords;
                col.LoadWithLimit("", "Surname", firstRecord, limit, out totalNoOfRecords);
                //---------------Test Result -----------------------
                Assert.Fail("IndexOutOfRangeException exception expected");
            }
            catch (IndexOutOfRangeException ex)
            {
                Assert.AreEqual("FirstRecordToLoad should not be negative.", ex.Message);
            }
        }


        [Test]
        public void Test_SetTimeLastLoaded_ShouldSetTimeLastLoaded()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.IsNull(col.TimeLastLoaded);
            //---------------Execute Test ----------------------
            DateTime expectedLastLoaded = DateTime.Now;
            col.TimeLastLoaded = expectedLastLoaded;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedLastLoaded, col.TimeLastLoaded);
        }

        [Test]
        public void Test_RefreshCollection_ShouldSetDateTimeLastLoaded()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.IsNull(col.TimeLastLoaded);
            //---------------Execute Test ----------------------
            col.Refresh();
            //---------------Test Result -----------------------
            Assert.IsNotNull(col.TimeLastLoaded);
        }
        [Test]
        public void Test_LoaderRefresh_ShouldSetDateTimeLastLoaded()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.IsNull(col.TimeLastLoaded);
            //---------------Execute Test ----------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(col);
            //---------------Test Result -----------------------
            Assert.IsNotNull(col.TimeLastLoaded);
        }

        [Test]
        public void Test_Sort_WithoutOrderCriteria_ShouldDoNothing()
        {

            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "");
            col.Sort("Surname", true, false);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0], "Collection should be in Surname Desc Order");
            Assert.AreSame(cp2, col[1], "Collection should be in Surname Desc Order");
            //---------------Execute Test ----------------------
            col.Sort();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0], "Collection Should not change the order since no default Sort criteria where set up");
            Assert.AreSame(cp2, col[1]);
        }

        [Test]
        public void Test_Sort_WhenOrderCriteriaSetUp_ShouldResortTheCollectionByTheOrderCriteria()
        {
            //---------------Set up test pack-------------------

            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");
            col.Sort("Surname", true, false);
            //---------------Assert Precondition----------------

            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0], "Collection should be in Surname Desc Order");
            Assert.AreSame(cp2, col[1], "Collection should be in Surname Desc Order");
            //---------------Execute Test ----------------------

            col.Sort();
            //---------------Test Result -----------------------



            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0], "Collection should b sorted by the Surname Property as per the origional collection.Load");
            Assert.AreSame(cp1, col[1]);
        }
        [Test]
        public void Test_CreateBusinessObject_ShouldAddToEndOfCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            CreateTwoSavedContactPeople();
            IBusinessObjectCollection col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.LoadAll("Surname");

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            //---------------Execute Test ----------------------
            IBusinessObject businessObject = col.CreateBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreSame(businessObject, col[2]);
        }      

        
        [Test]
        public void Test_Sort_WhenOrderCriteriaSetup_AndCollectionSorted_ShouldNotChangeOrder()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");
            col.Sort("Surname", true, true);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0], "Collection should be in Surname Asc Order");
            Assert.AreSame(cp1, col[1], "Collection should be in Surname Asc Order");
            //---------------Execute Test ----------------------
            col.Sort();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0], "Collection should b sorted by the Surname Property as per the origional collection.Load");
            Assert.AreSame(cp1, col[1]);
        }  
      
        [Test]
        public void Test_Sort_WithExpression_WhenOrderCriteriaSetup_AndCollectionSorted_ShouldNotChangeOrder()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            var cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            var cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            var col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");
            col.Sort(contact => contact.Surname, true, true);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0], "Collection should be in Surname Asc Order");
            Assert.AreSame(cp1, col[1], "Collection should be in Surname Asc Order");
            //---------------Execute Test ----------------------
            col.Sort();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0], "Collection should b sorted by the Surname Property as per the origional collection.Load");
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_Sort_WithExpression_ShouldSortCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            var cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            var cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            var col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0], "Collection should be in Surname Asc Order");
            Assert.AreSame(cp1, col[1], "Collection should be in Surname Asc Order");
            //---------------Execute Test ----------------------
            col.Sort(contact => contact.Surname, true, false);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0], "Collection should be sorted by the Surname Property DESC");
            Assert.AreSame(cp2, col[1]);
        }

        [Test]
        public void Test_Sort_WithExpression_AndReflectiveProperty_ShouldSortCollection()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            var cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            cp1.ReflectiveProp = "ccc";
            var cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            cp2.ReflectiveProp = "bbb";
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            var col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname DESC");
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp1, col[0], "Collection should be in Surname DESC Order");
            Assert.AreSame(cp2, col[1], "Collection should be in Surname DESC Order");

            //---------------Execute Test ----------------------
            col.Sort(contact => contact.ReflectiveProp, false,  true);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0], "Collection should be sorted by the ReflectiveProp Property ASC");
            Assert.AreSame(cp1, col[1]);
        }

        [Test]
        public void Test_SelectQuery_WhenSetToNull_ShouldRaiseError()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                col.SelectQuery = null;
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("A collection's select query cannot be set to null", ex.Message);
                StringAssert.Contains("A collection's select query cannot be set to null", ex.DeveloperMessage);
            }
        }
        
        [Test]
        public void Test_Find_ShouldReturnObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            //            DateTime now = DateTime.Now;
            const string firstName = "abab";
            ContactPersonTestBO cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzzz", firstName);
            ContactPersonTestBO cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            //            Criteria criteria = new Criteria("DateOfBirth", Criteria.ComparisonOp.Equals, now);
            const string criteria = "FirstName = '" + firstName + "'";
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load(criteria, "Surname");
            col.Sort("Surname", true, true);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, col.Count);
            Assert.AreSame(cp2, col[0], "Collection should be in Surname Asc Order");
            Assert.AreSame(cp1, col[1], "Collection should be in Surname Asc Order");
            //---------------Execute Test ----------------------
            ContactPersonTestBO foundCp = col.Find(bo => bo.Surname == "zzzz");
            //---------------Test Result -----------------------
            Assert.IsNotNull(foundCp);
            Assert.AreSame(cp1, foundCp);
        }

        [Test]
        public void Test_FindAll_ShouldReturnAllObject()
        {
            //---------------Set up test pack-------------------
            string firstName = TestUtil.GetRandomString();
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(firstName, out cp1, out cp2, out cp3);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            List<ContactPersonTestBO> foundCps = col.FindAll(bo => bo.FirstName == firstName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(foundCps);
            Assert.AreEqual(2, foundCps.Count);
            Assert.IsTrue(foundCps.Contains(cp1));
            Assert.IsTrue(foundCps.Contains(cp2));
        }

        [Test]
        public void Test_Foreach_ShouldDoActionOnAllItems()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            const string newName = "AAAA";
            col.ForEach(bo => bo.FirstName = newName);
            //---------------Test Result -----------------------
            Assert.AreEqual(newName, cp1.FirstName);
            Assert.AreEqual(newName, cp2.FirstName);
            Assert.AreEqual(newName, cp3.FirstName);
        }

        [Test]
        public void Test_RemoveAll_ShouldRemoveMatchingItems()
        {
            //---------------Set up test pack-------------------
            string firstName = TestUtil.GetRandomString();
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(firstName, out cp1, out cp2, out cp3);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            col.RemoveAll(bo => bo.FirstName == firstName);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.Count);
            Assert.IsTrue(col.Contains(cp3));
        }

        [Test]
        public void Test_Add_IListImplementedMethod_ShouldAddToBOCol()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            ContactPersonTestBO newPerson = ContactPersonTestBO.CreateSavedContactPerson("fdasfasd", "FirstfafdsName");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            ((IList)col).Add(newPerson);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, col.Count);
            Assert.IsTrue(col.Contains(newPerson));
        }

        [Test]
        public void Test_Add_Enumerable_WhenNull_ShouldRaiseException()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            var col = new BusinessObjectCollection<ContactPersonTestBO>();
            IList<ContactPersonTestBO> addedCol = null;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                col.Add(addedCol);
                Assert.Fail("expected ArgumentNullException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                StringAssert.Contains("Value cannot be null", ex.Message);
                StringAssert.Contains("col", ex.ParamName);
            }
        }

        [Test]
        public void Test_Contains_IListImplementedMethod_WhenContains_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            bool contains = ((IList)col).Contains(cp3);
            //---------------Test Result -----------------------
            Assert.IsTrue(contains);
        }

        [Test]
        public void Test_Contains_IListImplementedMethod_WhenNotContains_ShouldBFalse()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            ContactPersonTestBO newPerson = ContactPersonTestBO.CreateSavedContactPerson("fdasfasd", "FirstfafdsName");
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            bool contains = ((IList)col).Contains(newPerson);
            //---------------Test Result -----------------------
            Assert.IsFalse(contains);
        }

        [Test]
        public void Test_ToArray_ShouldContainAllItems()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            ContactPersonTestBO[] array = col.ToArray();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, array.Length);
            Assert.Contains(cp1, array);
            Assert.Contains(cp2, array);
            Assert.Contains(cp3, array);
        }

        [Test]
        public void Test_CopyToArrayOfTBusinessObjects_ShouldContainAllItems()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            var contactPersonsArray = new ContactPersonTestBO[3];
            col.CopyTo(contactPersonsArray, 0);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, contactPersonsArray.Length);
            Assert.Contains(cp1, contactPersonsArray);
            Assert.Contains(cp2, contactPersonsArray);
            Assert.Contains(cp3, contactPersonsArray);
        }
        [Test]
        public void Test_CopyToArray_ShouldContainAllItems()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            Array contactPersonsArray = new ContactPersonTestBO[3];
            col.CopyTo(contactPersonsArray, 0);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, contactPersonsArray.Length);
            Assert.Contains(cp1, contactPersonsArray);
            Assert.Contains(cp2, contactPersonsArray);
            Assert.Contains(cp3, contactPersonsArray);
        }

        [Test]
        public void Test_CopyToArray_WhenIndexNonZero_ShouldContainAllItems()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            Array contactPersonsArray = new ContactPersonTestBO[4];
            col.CopyTo(contactPersonsArray, 1);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, contactPersonsArray.Length, "Since start copying at index 1 should copy 3 items into index 1,2,3");
            Assert.Contains(cp1, contactPersonsArray);
            Assert.Contains(cp2, contactPersonsArray);
            Assert.Contains(cp3, contactPersonsArray);
        }

        [Test]
        public void Test_ConvertAll_ShouldCallConversionDelegateForEachObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            var called = new List<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            List<string> converted = col.ConvertAll(input =>
            {
                called.Add(input);
                return input.ToString();
            });
            //---------------Test Result -----------------------
            Assert.AreEqual(3, converted.Count);
            Assert.AreEqual(3, called.Count);
            Assert.Contains(cp1, called);
            Assert.Contains(cp2, called);
            Assert.Contains(cp3, called);
            Assert.Contains(cp1.ToString(), converted);
            Assert.Contains(cp2.ToString(), converted);
            Assert.Contains(cp3.ToString(), converted);
        }

        [Test]
        public void Test_Exists_WhenFound_ShouldBeTrue()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            bool exists = col.Exists(bo => bo == cp2);
            //---------------Test Result -----------------------
            Assert.IsTrue(exists);
        }

        [Test]
        public void Test_Exists_WhenNotFound_ShouldBeFalse()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> col = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            ContactPersonTestBO cp4 = CreateContactPersonTestBO();
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, col.Count);
            //---------------Execute Test ----------------------
            bool exists = col.Exists(bo => bo == cp4);
            //---------------Test Result -----------------------
            Assert.IsFalse(exists);
        }

        [Test]
        public void Test_AddRange_ShouldAddItemsToCol()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> colOrig = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            List<ContactPersonTestBO> list = new List<ContactPersonTestBO>(colOrig);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, colOrig.Count);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(0, col.Count);
            //---------------Execute Test ----------------------
            col.AddRange(list);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, colOrig.Count);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(3, col.Count);
        }

        [Test]
        public void Test_AddRange_ShouldFireEventsForEachItemAdded()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO cp1;
            ContactPersonTestBO cp2;
            ContactPersonTestBO cp3;
            BusinessObjectCollection<ContactPersonTestBO> colOrig = CreateCollectionWithTestPack(out cp1, out cp2, out cp3);
            List<ContactPersonTestBO> list = new List<ContactPersonTestBO>(colOrig);
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            //---------------Assert Precondition----------------
            Assert.AreNotEqual(0, list.Count);
            //---------------Execute Test ----------------------

            int totalObjectAddedEventsFired = 0;
            col.BusinessObjectAdded += (sender, args) => totalObjectAddedEventsFired++;
            col.AddRange(list);
            //---------------Test Result -----------------------
            Assert.AreEqual(list.Count, totalObjectAddedEventsFired);
        }

        [Test]
        public void IBusinessObjectCollection_AsEnumerable_T()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectCollection col = CreateCollectionWith_OneBO();
            //---------------Execute Test ----------------------
            var list = col.AsEnumerable<ContactPersonTestBO>();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, list.Count());
        }

        [Test]
        public void IBusinessObjectCollection_AsEnumerable_IBusinessObject()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectCollection col = CreateCollectionWith_OneBO();
            //---------------Execute Test ----------------------
            var list = col.AsEnumerable<IBusinessObject>();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, list.Count());
        }

        /// <summary>
        /// Asserts that the results for the collection are as expected
        /// </summary>
        /// <param name="expectedFirstRecord">The expected index for the first record to load.</param>
        /// <param name="expectedLimit">The expected limit for the Collection's Query.</param>
        /// <param name="expectedTotal">The expected total number of Bo's available</param>
        /// <param name="expectedCount">The expected count of the returned collection</param>
        /// <param name="orderedPeople">An ordered array that will be used to validate the items of the collection</param>
        /// <param name="actualCol">The actual Collection</param>
        /// <param name="returnedTotalNoOfRecords">The returned total Number of records to check</param>
        private static void AssertLimitedResultsCorrect
            (int expectedFirstRecord, int expectedLimit, int expectedTotal, int expectedCount,
             ContactPersonTestBO[] orderedPeople, IBusinessObjectCollection actualCol,
             int returnedTotalNoOfRecords)
        {
            Assert.AreEqual
                (expectedTotal, returnedTotalNoOfRecords, "The returned total number of availabe records is incorrect");
            Assert.AreEqual
                (expectedFirstRecord, actualCol.SelectQuery.FirstRecordToLoad,
                 "Collection query FirstRecordToLoad does not match expectation.");
            Assert.AreEqual
                (expectedLimit, actualCol.SelectQuery.Limit, "Collection query limit does not match expectation.");
            Assert.AreEqual(expectedCount, actualCol.Count, "Collection size does not match expectation.");
            int index = expectedFirstRecord;
            foreach (ContactPersonTestBO bo in actualCol)
            {
                Assert.AreSame(orderedPeople[index], bo, "Item in collection does not match expected item.");
                index++;
            }
        }

        /// <summary>
        /// Creates the specified number of saved Contact People with random Surnames and returns an array of the 
        /// created items sorted by their surname.
        /// </summary>
        /// <param name="noOfPeople">The number of saved contact people to create</param>
        /// <returns>Returns an array of the created items sorted by their surname.</returns>
        private static ContactPersonTestBO[] CreateSavedSortedContactPeople(int noOfPeople)
        {
            List<ContactPersonTestBO> createdBos = new List<ContactPersonTestBO>(noOfPeople);
            while (createdBos.Count < noOfPeople)
            {
                createdBos.Add(ContactPersonTestBO.CreateSavedContactPerson(TestUtil.GetRandomString()));
            }
            createdBos.Sort((x, y) => StringComparer.InvariantCultureIgnoreCase.Compare(x.Surname, y.Surname));
            return createdBos.ToArray();
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

        private static BusinessObjectCollection<ContactPersonTestBO> CreateCollectionWithTestPack(out ContactPersonTestBO cp1, out ContactPersonTestBO cp2, out ContactPersonTestBO cp3)
        {
            return CreateCollectionWithTestPack("abab", out cp1, out cp2, out cp3);
        }

        private static BusinessObjectCollection<ContactPersonTestBO> CreateCollectionWithTestPack(string firstName, out ContactPersonTestBO cp1, out ContactPersonTestBO cp2, out ContactPersonTestBO cp3)
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            cp1 = ContactPersonTestBO.CreateSavedContactPerson("zzaaaa", firstName);
            cp2 = ContactPersonTestBO.CreateSavedContactPerson("aaaa", firstName);
            cp3 = ContactPersonTestBO.CreateSavedContactPerson("ZZZZZ", "FirstName");
            BusinessObjectCollection<ContactPersonTestBO> col = new BusinessObjectCollection<ContactPersonTestBO>();
            col.Load("", "Surname");
            col.Sort("Surname", true, true);
            return col;
        }
    }
}
