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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.ObjectManager;
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

        [Test]
        public void TestAddedEvent_FiringWhenSavingACreatedBusinessObject()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            _addedEventFired = false;
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            cpCol.LoadAll();
            cpCol.BusinessObjectAdded += delegate { _addedEventFired = true; };

            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();

            //---------------Execute Test ----------------------
            newCP.Save();
            //---------------Test Result -----------------------
            Assert.IsTrue(_addedEventFired);
        }

        [Test]
        public void TestAddedEvent_NotFiringWhenRefreshing()
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

            //---------------Execute Test ----------------------
            cpCol.Refresh();
            //---------------Test Result -----------------------
            Assert.IsFalse(_addedEventFired);
        }

        [Test]
        public void TestAddMethod()
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
        public void TestAddMethod_WithEnumerable_Collection()
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
        public void TestAddMethod_WithEnumerable_List()
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
        public void TestAddMethod_WithParamArray()
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
        public void TestAddMethod_WithCollection()
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
        public void TestCreateBusinessObject()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            Assert.IsTrue(newCP.Status.IsNew);
            Assert.AreEqual(1, cpCol.CreatedBusinessObjects.Count);
        }

        [Test]
        public void TestFindByGuid()
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

        [Test]
        public void TestPersistOfCreatedBusinessObject()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();

            newCP.Save();
            Assert.AreEqual(1, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        [Test]
        public void TestRefreshCollectionDoesNotRefreshDirtyOject()
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
        public void TestRefreshCollectionRefreshesNonDirtyObjects()
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
        public void TestRestoreAll()
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
        public void TestRestoreOfACreatedBusinessObject()
        {
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectCollection<ContactPersonTestBO> cpCol = new BusinessObjectCollection<ContactPersonTestBO>();
            ContactPersonTestBO newCP = cpCol.CreateBusinessObject();
            newCP.Surname = Guid.NewGuid().ToString();

            newCP.Restore();
            Assert.AreEqual(0, cpCol.Count);
            Assert.AreEqual(0, cpCol.CreatedBusinessObjects.Count);
        }

        [Test]
        public void TestFindIncludesCreatedBusinessObjects()
        {
            //---------------Set up test pack-------------------
            BORegistry.DataAccessor = new DataAccessorInMemory();
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectCollection<ContactPersonTestBO> cpCol =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<ContactPersonTestBO>("");
            ContactPersonTestBO cp2 = cpCol.CreateBusinessObject();
            //---------------Execute Test ----------------------
            ContactPersonTestBO foundCp = cpCol.Find(cp2.ID.ToString());

            //---------------Test Result -----------------------
            Assert.IsNotNull(foundCp);
            Assert.AreSame(cp2, foundCp);
            //---------------Tear Down -------------------------

        }
    }
}