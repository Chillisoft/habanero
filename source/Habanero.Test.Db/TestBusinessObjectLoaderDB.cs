using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.BO.BusinessObjectLoader;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestBusinessObjectLoaderDB : TestBusinessObjectLoader
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetupTest()
        {
            base.SetupTest();
            ContactPersonTestBO.DeleteAllContactPeople();
        }

        #endregion

        protected override void DeleteEnginesAndCars()
        {
            Engine.DeleteAllEngines();
            Car.DeleteAllCars();
        }

        public TestBusinessObjectLoaderDB()
        {
            new TestUsingDatabase().SetupDBConnection();
        }

        protected override void SetupDataAccessor()
        {
            BORegistry.DataAccessor = new DataAccessorDB();
            const string sql = "DELETE FROM bowithintid";
            if (DatabaseConnection.CurrentConnection != null) DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        [Test]
        public void TestGetBusinessObjectByIDInt_ByCriteriaObject()
        {
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            BOWithIntID.LoadClassDefWithIntID();
            BOWithIntID bo = new BOWithIntID {IntID = TestUtil.GetRandomInt()};
            bo.Save();
            BusinessObjectManager.Instance.ClearLoadedObjects();
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
            BusinessObjectManager.Instance.ClearLoadedObjects();
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
            BusinessObjectManager.Instance.ClearLoadedObjects();
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

        //[Test]
        //public void Test_BusinessObjectClearsItselfFromObjectManager()
        //{
        //    //---------------Set up test pack-------------------
        //    BusinessObjectManager.Instance.ClearLoadedObjects();
        //    WaitForGC();
        //    ClassDef.ClassDefs.Clear();
        //    ContactPersonTestBO.LoadDefaultClassDef();
        //    const string surname = "abc";
        //    const string firstName = "aa";
        //    ContactPersonTestBO savedContactPerson = CreateSavedContactPerson(surname, firstName);

        //    //---------------Assert Precondition----------------
        //    Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
        //    Assert.IsNotNull(savedContactPerson);

        //    //---------------Execute Test ----------------------
        //    savedContactPerson = null;
        //    WaitForGC();

        //    //---------------Test Result -----------------------
        //    Assert.IsNull(savedContactPerson);
        //    Assert.AreEqual(0, BusinessObjectManager.Instance.Count);
        //}

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

            BusinessObjectManager.Instance.ClearLoadedObjects();

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
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
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
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = ContactPersonTestBO.CreateSavedContactPersonNoAddresses();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
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
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            originalContactPerson.Surname = "FirstSurname";
            originalContactPerson.Save();
            IPrimaryKey origCPID = originalContactPerson.ID;

            BusinessObjectManager.Instance.ClearLoadedObjects();

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
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();

            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson
                (surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

            //---------------Assert Precondition----------------
            //Object loaded in object manager
            Assert.AreEqual(3, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(savedContactPerson.ID));

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
            ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();

            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson
                (surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

            //---------------Assert Precondition----------------
            //Object loaded in object manager
            Assert.AreEqual(3, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(savedContactPerson.ID));

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
            IClassDef classDef = ContactPersonTestBO.LoadDefaultClassDef();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();

            const string surname = "abc";
            const string firstName = "aa";
            ContactPersonTestBO.CreateSavedContactPerson("ZZ", "zzz");
            ContactPersonTestBO savedContactPerson = ContactPersonTestBO.CreateSavedContactPerson
                (surname, firstName);
            ContactPersonTestBO.CreateSavedContactPerson("aaaa", "aaa");

            //---------------Assert Precondition----------------
            //Object loaded in object manager
            Assert.AreEqual(3, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(savedContactPerson.ID));

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
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString("N");
            cp.FirstName = Guid.NewGuid().ToString("N");
            cp.Save();

            BusinessObjectManager.Instance.ClearLoadedObjects();
            WaitForGC();

            SelectQuery query = CreateSelectQuery(cp);
            //                new SelectQuery(new Criteria("Surname", Criteria.ComparisonOp.Equals, cp.Surname)));
            //                query.Fields.Add("Surname", new QueryField("Surname", "Surname_field", null));
            //                query.Fields.Add("ContactPersonID", new QueryField("ContactPersonID", "ContactPersonID", null));
            //                query.Source = new Source(cp.ClassDef.TableName);

            //---------------Assert Precondition ---------------
            //Object not loaded in all loaded business objects
            Assert.AreEqual(0, BusinessObjectManager.Instance.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO loadedCp =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(query);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
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
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();

            //load second object from DB to ensure that it is now in the object manager
            ContactPerson loadedContactPerson1 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(origConactPersonID);

            //---------------Assert Precondition----------------
            Assert.IsNull(originalContactPerson);
            Assert.AreEqual(firstSurname, loadedContactPerson1.Surname);
            Assert.AreNotSame(originalContactPerson, loadedContactPerson1);
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);

            //---------------Execute Test ----------------------

            ContactPerson loadedContactPerson2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(origConactPersonID);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
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
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO originalContactPerson = new ContactPersonTestBO();
            originalContactPerson.Surname = "FirstSurname";
            originalContactPerson.Save();

            BusinessObjectManager.Instance.ClearLoadedObjects();

            //load second object from DB to ensure that it is now in the object manager
            ContactPersonTestBO myContact2 =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                    (originalContactPerson.ID);

            //-----------------------------Assert Precondition -----------------
            Assert.AreNotSame(originalContactPerson, myContact2);
            IPrimaryKey id = myContact2.ID;
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(id));
            IBusinessObject boFromAllLoadedObjects = BusinessObjectManager.Instance[id];
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
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(myContact3));
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
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO contactPerson1 = ContactPersonTestBO.CreateSavedContactPerson
                (Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));
            BusinessObjectManager.Instance.ClearLoadedObjects();

            //---------------Assert Precondition----------------
            Assert.AreEqual(0, BusinessObjectManager.Instance.Count);

            //---------------Execute Test ----------------------
            ContactPersonTestBO contactPerson =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>
                    (contactPerson1.ID);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, BusinessObjectManager.Instance.Count);
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(contactPerson));
        }

        [Test]
        public void Test_RefreshCallsAfterLoadNotCalledIfObjectNotReloaded()
        {
            //---------------Set up test pack---------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();

            //---------------Assert Precondition------------------
            Assert.IsTrue(BusinessObjectManager.Instance.Contains(cp));
            Assert.IsFalse(cp.AfterLoadCalled);

            //---------------Execute Test ------------------------
            BORegistry.DataAccessor.BusinessObjectLoader.Refresh(cp);

            //---------------Test Result -------------------------
            Assert.IsTrue(cp.AfterLoadCalled);
        }

        [Test]
        public void Test_RefreshCallsAfterLoadCalledIfObjectLoaded()
        {
            //---------------Set up test pack---------------------
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();

            ContactPersonTestBO cp = new ContactPersonTestBO();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            BusinessObjectManager.Instance.ClearLoadedObjects();

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
        public void TestBoLoader_RefreshBusinessObjectDeletedByAnotherUser()
        {
            //-------------Setup Test Pack------------------
            ContactPersonTestBO.LoadDefaultClassDef();
            ContactPersonTestBO cpTemp = ContactPersonTestBO.CreateSavedContactPerson();
            BusinessObjectManager.Instance.ClearLoadedObjects();

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
    }
}