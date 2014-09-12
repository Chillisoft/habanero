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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.BO.BusinessObjectLoader;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestBusinessObjectLoaderDB : TestBusinessObjectLoader
    {
        private string _contactPersonTableName;

        #region Setup/Teardown

        [SetUp]
        public override void SetupTest()
        {
            FixtureEnvironment.SetupNewIsolatedBusinessObjectManager();
            base.SetupTest();
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        protected override void DeleteEnginesAndCars()
        {
            Engine.DeleteAllEngines();
            Car.DeleteAllCars();
        }

        public TestBusinessObjectLoaderDB()
        {
            new TestUsingDatabase().SetupDBConnection();
        }

        [TestFixtureSetUp]
        public virtual void SetupDatabaseConnection()
        {
            new TestUsingDatabase().SetupDBConnection();
        }

        protected override void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorDB();
            const string sql = "DELETE FROM bowithintid";
            if (DatabaseConnection.CurrentConnection != null) DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        protected virtual void CreateContactPersonTable()
        {
            _contactPersonTableName = BOTestUtils.CreateContactPersonTable(TestUtil.GetRandomString());
        }

        public string GetContactPersonTableName()
        {
            return _contactPersonTableName;
        }

        private IClassDef SetupDefaultContactPersonBO()
        {
            CreateContactPersonTable();
            var cpClassDef = ContactPersonTestBO.LoadDefaultClassDef();
            //cpClassDef.TableName = "ContactPersonTable with a randomlygenerated guid";
            cpClassDef.TableName = GetContactPersonTableName();
            return cpClassDef;
        }

        [TearDown]
        public override void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
            BOTestUtils.DropNewContactPersonAndAddressTables();
        }

        #endregion

        [Test]
        public void TestGetBusinessObjectByIDInt_ByCriteriaObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID {IntID = TestUtil.GetRandomInt()};
            bo.Save();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        
            Criteria criteria = new Criteria("IntID", Criteria.ComparisonOp.Equals, bo.IntID.ToString());
            //---------------Execute Test ----------------------
            BOWithIntID bo1 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<BOWithIntID>(criteria);
            //---------------Test Result -----------------------
            Assert.IsNotNull(bo1); 
            BOWithIntID bo2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<BOWithIntID>(bo1.ID);
            Assert.AreSame(bo1, bo2);
        }

        [Test]
        public void TestGetBusinessObjectByIDInt_ByCriteriaString()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID { IntID = TestUtil.GetRandomInt() };
            bo.Save();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        
            //---------------Execute Test ----------------------
            BOWithIntID bo1 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<BOWithIntID>
                (string.Format("IntID = {0}", bo.IntID));
            //---------------Test Result -----------------------
            Assert.IsNotNull(bo1);
            BOWithIntID bo2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<BOWithIntID>(bo1.ID);
            Assert.AreSame(bo1, bo2);
        }

        [Test]
        public void TestGetBusinessObjectByIDInt_CriteriaString_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID { IntID = TestUtil.GetRandomInt() };
            bo.Save();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        
            //---------------Execute Test ----------------------
            BOWithIntID bo1 = (BOWithIntID) BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject
                (classDef,  string.Format("IntID = {0}", bo.IntID));
            //---------------Test Result -----------------------
            Assert.IsNotNull(bo1);
            BOWithIntID bo2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<BOWithIntID>(bo1.ID);
            Assert.AreSame(bo1, bo2);
        }

        [Test]
        public void TestGetBusinessObjectByIDIntSavenewAutoIncNumber()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            TestAutoInc.LoadClassDefWithAutoIncrementingID();
            TestAutoInc autoInc = new TestAutoInc();
            autoInc.TestAutoIncID = int.MaxValue;
            autoInc.Save();

            //---------------Execute Test ----------------------
            Criteria criteria = new Criteria("testautoincid", Criteria.ComparisonOp.Equals, autoInc.TestAutoIncID);
            TestAutoInc tai1 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TestAutoInc>(criteria);
            TestAutoInc tai2 = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TestAutoInc>(tai1.ID);

            ////---------------Test Result -----------------------
            Assert.AreSame(tai1, tai2);
            Assert.AreEqual("testing", tai2.TestField);
        }

        [Test]
        public void Test_ReturnSameObjectFromBusinessObjectLoader()
        {
            //---------------Set up test pack-------------------
            //------------------------------Setup Test
            new Engine();
            new Car();
            ContactPerson originalContactPerson = new ContactPerson();
            originalContactPerson.Surname = "FirstSurname";
            originalContactPerson.Save();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        

            //load second object from DB to ensure that it is now in the object manager
            ContactPerson myContact2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                    (originalContactPerson.ID);

            //---------------Assert Precondition----------------
            Assert.AreNotSame(originalContactPerson, myContact2);

            //---------------Execute Test ----------------------
            ContactPerson myContact3 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>
                    (originalContactPerson.ID);

            //---------------Test Result -----------------------
            Assert.AreNotSame(originalContactPerson, myContact3);
            Assert.AreSame(myContact2, myContact3);
        }

        [Test]
        public void TestAfterLoadCalled_GetBusinessObject()
        {
            //---------------Set up test pack-------------------
            SetupDefaultContactPersonBO();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.ID);
            //---------------Test Result -----------------------
            Assert.AreNotSame(cp, loadedCP);
            Assert.IsTrue(loadedCP.AfterLoadCalled);
        }

        [Test]
        public void TestAfterLoadCalled_GetBusinessObject_Untyped()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = SetupDefaultContactPersonBO();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();

            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCP =
                (ContactPersonTestBO)
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject(classDef, cp.ID);
            //---------------Test Result -----------------------
            Assert.AreNotSame(cp, loadedCP);
            Assert.IsTrue(loadedCP.AfterLoadCalled);
        }

        /// <summary>
        /// Tests to ensure that if the object has been edited by another user
        ///  and the default strategy to reload has been replaced then one we do not get back is always the latest.
        /// Note_: This behaviour must be made configurable using a strategy TestGetTheFreshestObject_Strategy test 
        /// </summary>
        [Test, Ignore("Need to implement via a strategy")]
        public void TestDontGetTheFreshestObject_Strategy()
        {
            //------------------------------Setup Test
            ClassDef.ClassDefs.Clear();
            SetupDefaultContactPersonBO();
            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            originalContactPerson.Surname = "FirstSurname";
            originalContactPerson.Save();
            IPrimaryKey origCPID = originalContactPerson.ID;
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        

            //load second object from DB to ensure that it is now in the object manager
            ContactPersonTestBO myContact2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(origCPID);

            //-----------------------------Execute Test-------------------------
            //Edit first object and save
            originalContactPerson.Surname = "SecondSurname";
            originalContactPerson.Save();

            ContactPersonTestBO myContact3 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(origCPID);

            //-----------------------------Assert Result-----------------------
            Assert.AreSame(myContact3, myContact2);
            //The two surnames should be equal since the myContact3 was refreshed
            // when it was loaded.
            Assert.AreNotEqual(originalContactPerson.Surname, myContact3.Surname);
            //Just to check the myContact2 should also match since it is physically the 
            // same object as myContact3
            Assert.AreNotEqual(originalContactPerson.Surname, myContact2.Surname);
        }

        [Test]
        public void TestGetBusinessObject_MultipleCriteria()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            SetupDefaultContactPersonBO();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        

            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson
                (surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");
            //---------------Execute Test ----------------------
            Criteria criteria1 = new Criteria("Surname", Criteria.ComparisonOp.Equals, surname);
            Criteria criteria2 = new Criteria("FirstName", Criteria.ComparisonOp.Equals, firstName);
            Criteria criteria = new Criteria(criteria1, Criteria.LogicalOp.And, criteria2);
            ContactPersonTestBO cp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria);

            //---------------Test Result -----------------------
            Assert.AreSame(savedContactPerson, cp);

            Assert.AreEqual(surname, cp.Surname);
            Assert.AreEqual(firstName, cp.FirstName);
            Assert.IsFalse(cp.Status.IsNew);
            Assert.IsFalse(cp.Status.IsDirty);
            Assert.IsFalse(cp.Status.IsDeleted);
        }

        [Test]
        public void TestGetBusinessObject_MultipleCriteria_CriteriaString()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            SetupDefaultContactPersonBO();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test

            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson
                (surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ContactPersonTestBO cp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                    ("Surname = " + surname + " AND FirstName = " + firstName);

            //---------------Test Result -----------------------
            Assert.AreSame(savedContactPerson, cp);

            Assert.AreEqual(surname, cp.Surname);
            Assert.AreEqual(firstName, cp.FirstName);
            Assert.IsFalse(cp.Status.IsNew);
            Assert.IsFalse(cp.Status.IsDirty);
            Assert.IsFalse(cp.Status.IsDeleted);
        }

        [Test]
        public void TestGetBusinessObject_MultipleCriteria_CriteriaString_Untyped()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            IClassDef classDef = SetupDefaultContactPersonBO();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test

            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson
                (surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ContactPersonTestBO cp =
                (ContactPersonTestBO)
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject
                    (classDef, "Surname = " + surname + " AND FirstName = " + firstName);

            //---------------Test Result -----------------------
            Assert.AreSame(savedContactPerson, cp);

            Assert.AreEqual(surname, cp.Surname);
            Assert.AreEqual(firstName, cp.FirstName);
            Assert.IsFalse(cp.Status.IsNew);
            Assert.IsFalse(cp.Status.IsDirty);
            Assert.IsFalse(cp.Status.IsDeleted);
        }

        [Test]
        public void TestGetBusinessObject_SelectQuery_Fresh()
        {
            //---------------Set up test pack-------------------
            SetupDataAccessor();
            SetupDefaultContactPersonBO();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.FirstName = Guid.NewGuid().ToString("N");
            cp.Save();
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test

            SelectQuery query = CreateSelectQuery(cp);
            //                new SelectQuery(new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname)));
            //                query.Fields.Add("Surname", new QueryField("Surname", "Surname_field", null));
            //                query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
            //                query.Source = new Source(cp.ClassDef.TableName);

            //---------------Assert Precondition ---------------
            //Object not loaded in all loaded business objects
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();//Ensures a new BOMan is created and used for each test
        
            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(query);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            Assert.AreNotSame(loadedCp, cp);
            Assert.AreEqual(cp.ContactPersonID, loadedCp.ContactPersonID);
            Assert.AreEqual(cp.Surname, loadedCp.Surname);
            Assert.IsTrue(String.IsNullOrEmpty(loadedCp.FirstName), "Firstname is not being loaded");
            // not being loaded
            Assert.IsFalse(loadedCp.Status.IsNew);
            Assert.IsFalse(loadedCp.Status.IsDeleted);
            Assert.IsFalse(loadedCp.Status.IsDirty);
            Assert.IsTrue(loadedCp.Status.IsValid());
        }

        [Test]
        public void TestLoadFromDatabaseAlwaysLoadsSameObject()
        {
            //---------------Set up test pack-------------------
            new Engine();
            new Car();
            ContactPerson originalContactPerson = new ContactPerson();
            const string firstSurname = "FirstSurname";
            originalContactPerson.Surname = firstSurname;
            originalContactPerson.Save();
            IPrimaryKey origConactPersonID = originalContactPerson.ID;
            originalContactPerson = null;
            FixtureEnvironment.ClearBusinessObjectManager();
            TestUtil.WaitForGC();

            //load second object from DB to ensure that it is now in the object manager
            ContactPerson loadedContactPerson1 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(origConactPersonID);

            //---------------Assert Precondition----------------
            Assert.IsNull(originalContactPerson);
            Assert.AreEqual(firstSurname, loadedContactPerson1.Surname);
            Assert.AreNotSame(originalContactPerson, loadedContactPerson1);
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);

            //---------------Execute Test ----------------------

            ContactPerson loadedContactPerson2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(origConactPersonID);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            Assert.AreEqual(firstSurname, loadedContactPerson2.Surname);
            Assert.AreNotSame(originalContactPerson, loadedContactPerson2);

            Assert.AreSame(loadedContactPerson1, loadedContactPerson2);
        }

        /// <summary>
        /// Tests to ensure that if the object has been edited by
        /// another user and is not currently being edited by this user
        ///  then one we get back is always the latest.
        /// Note_: This behaviour is configurable using a strategy TestDontGetTheFreshestObject_Strategy test 
        /// </summary>
        [Test]
        public void TestGetTheFreshestObject_Strategy()
        {
            //------------------------------Setup Test----------------------------
            SetupDefaultContactPersonBO();
            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            originalContactPerson.Surname = "FirstSurname";
            originalContactPerson.Save();

            FixtureEnvironment.ClearBusinessObjectManager();

            //load second object from DB to ensure that it is now in the object manager
            ContactPersonTestBO myContact2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                    (originalContactPerson.ID);

            //-----------------------------Assert Precondition -----------------
            Assert.AreNotSame(originalContactPerson, myContact2);
            IPrimaryKey id = myContact2.ID;
            Assert.IsTrue(BORegistry.BusinessObjectManager.Contains(id));
            IBusinessObject boFromAllLoadedObjects = BORegistry.BusinessObjectManager[id];
            Assert.AreSame(boFromAllLoadedObjects, myContact2);
            Assert.IsFalse(myContact2.Status.IsEditing);

            //-----------------------------Execute Test-------------------------
            //Edit first object and save
            originalContactPerson.Surname = "SecondSurname";
            originalContactPerson.Save();

            ContactPersonTestBO myContact3 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                    (originalContactPerson.ID);

            //-----------------------------Assert Result-----------------------
            Assert.IsFalse(myContact3.Status.IsEditing);
            Assert.AreNotSame(originalContactPerson, myContact3);
            Assert.IsTrue(BORegistry.BusinessObjectManager.Contains(myContact3));
            Assert.AreSame(myContact3, myContact2);
            //The two surnames should be equal since the myContact3 was refreshed
            // when it was loaded.
            Assert.AreEqual(originalContactPerson.Surname, myContact3.Surname);
            //Just to check the myContact2 should also match since it is physically the 
            // same object as myContact3
            Assert.AreEqual(originalContactPerson.Surname, myContact2.Surname);
        }

        [Test]
        public void TestLoadedObjectIsAddedToObjectManager()
        {
            //---------------Set up test pack-------------------
            ContactPerson.DeleteAllContactPeople();
            SetupDefaultContactPersonBO();
            ContactPersonTestBO contactPerson1 = ContactPersonTestBO.CreateSavedContactPerson
                (Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
            FixtureEnvironment.ClearBusinessObjectManager();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, BORegistry.BusinessObjectManager.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO contactPerson =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                    (contactPerson1.ID);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, BORegistry.BusinessObjectManager.Count);
            Assert.IsTrue(BORegistry.BusinessObjectManager.Contains(contactPerson));
        }

        [Test]
        public void Test_Refresh_AfterLoadNotCalledIfObjectIsNotUpdatedInReloading()
        {
            //---------------Set up test pack---------------------
            ClassDef.ClassDefs.Clear();
            SetupDefaultContactPersonBO();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();

            //---------------Assert Precondition------------------
            Assert.IsTrue(BORegistry.BusinessObjectManager.Contains(cp));
            Assert.IsFalse(cp.AfterLoadCalled);

            //---------------Execute Test ------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cp);

            //---------------Test Result -------------------------
            Assert.IsFalse(cp.AfterLoadCalled);
        }
        
        [Test]
        public void Test_Refresh_UpdatedEventIsNotFired_IfObjectIsNotUpdatedInReloading()
        {
            //---------------Set up test pack---------------------
            ClassDef.ClassDefs.Clear();
            SetupDefaultContactPersonBO();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            bool updatedEventFired = false;
            cp.Updated += (sender, args) => updatedEventFired = true; 

            //---------------Assert Precondition------------------
            Assert.IsFalse(updatedEventFired);

            //---------------Execute Test ------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cp);

            //---------------Test Result -------------------------
            Assert.IsFalse(updatedEventFired);
        }

        [Test]
        public void Test_Refresh_AfterLoadIsCalled_IfObjectUpdatedInLoading()
        {
            //---------------Set up test pack---------------------
            ClassDef.ClassDefs.Clear();
            SetupDefaultContactPersonBO();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            FixtureEnvironment.ClearBusinessObjectManager();

            ContactPersonTestBO cpLoaded =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.ID);
            string newFirstname = cp.FirstName = TestUtil.GetRandomString();
            cp.Save();
            cpLoaded.AfterLoadCalled = false;

            //---------------Assert Precondition------------------
            Assert.IsFalse(cpLoaded.AfterLoadCalled);

            //---------------Execute Test ------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpLoaded);

            //---------------Test Result -------------------------
            Assert.IsTrue(cpLoaded.AfterLoadCalled);
            Assert.AreNotSame(cp, cpLoaded);

            Assert.AreEqual(cp.FirstName, cpLoaded.FirstName);
            Assert.AreEqual(newFirstname, cpLoaded.FirstName);
        }

        [Test]
        public void Test_Refresh_UpdatedEventIsFired_IfObjectUpdatedInLoading()
        {
            //---------------Set up test pack---------------------
            ClassDef.ClassDefs.Clear();
            SetupDefaultContactPersonBO();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            FixtureEnvironment.ClearBusinessObjectManager();

            ContactPersonTestBO cpLoaded =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cp.ID);
            string newFirstname = cp.FirstName = TestUtil.GetRandomString();
            cp.Save();
            bool updatedEventFired = false;
            cpLoaded.Updated += (sender, args) => updatedEventFired = true; 

            //---------------Assert Precondition------------------
            Assert.IsFalse(updatedEventFired);

            //---------------Execute Test ------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpLoaded);

            //---------------Test Result -------------------------
            Assert.IsTrue(updatedEventFired);
        }


        
        
        [Test]
        public void TestBoLoader_RefreshBusinessObjectDeletedByAnotherUser()
        {
            //-------------Setup Test Pack------------------
            SetupDefaultContactPersonBO();
            ContactPersonTestBO cpTemp = ContactPersonTestBO.CreateSavedContactPerson();
            FixtureEnvironment.ClearBusinessObjectManager();

            ContactPersonTestBO cpLoaded =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(cpTemp.ID);
            cpTemp.MarkForDelete();
            cpTemp.Save();

            //-------------Execute Test ---------------------
            try
            {
                BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cpLoaded);
                Assert.Fail();
            }
                //-------------Test Result ---------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                StringAssert.Contains
                    ("A Error has occured since the object you are trying to refresh has been deleted by another user.",
                     ex.Message);
                StringAssert.Contains("There are no records in the database for the Class", ex.Message);
                StringAssert.Contains("ContactPersonTestBO", ex.Message);
                StringAssert.Contains(cpLoaded.ID.ToString(), ex.Message);
            }
        }

        [Test]
        public void Test_Refresh_WithDuplicateObjectsInPersistedCollection_ShouldThrowHabaneroDeveloperException()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            bo.Save();
            BusinessObjectCollection<MyBO> collection = new BusinessObjectCollection<MyBO>();
            collection.Load("MyBoID = '" + bo.MyBoID + "'", "");
            collection.PersistedBusinessObjects.Add(bo);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, collection.PersistedBusinessObjects.Count);
            Assert.AreSame(collection.PersistedBusinessObjects[0], collection.PersistedBusinessObjects[1]);
            //---------------Execute Test ----------------------
            try
            {
                collection.Refresh();
                //---------------Test Result -----------------------
            } catch (Exception ex) 
            {
                Assert.IsInstanceOf<HabaneroDeveloperException>(ex, "Should have thrown a HabaneroDeveloperException because of the duplicate item in the PersistedBusinessObjects collection");
                StringAssert.Contains("A duplicate Business Object was found in the persisted objects collection of the BusinessObjectCollection during a reload", ex.Message);
                StringAssert.Contains("MyBO", ex.Message);
                StringAssert.Contains(bo.MyBoID.Value.ToString("B").ToUpper(), ex.Message);
            }

        }

    }
}