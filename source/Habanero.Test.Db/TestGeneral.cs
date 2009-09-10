//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    /// <summary>
    /// Summary description for TestGeneral.
    /// </summary>
    [TestFixture]
    public class TestGeneral : TestUsingDatabase
    {
        private IPrimaryKey _updateContactPersonId;
        private ContactPerson _contactPersonUpdateConcurrency;
        private ContactPerson _contactPBeginEditsConcurrency;
        private ContactPerson _contactPTestRefreshFromObjMan;
        private ContactPerson _contactPTestSave;

        [TestFixtureSetUp]
        public void CreateTestPack()
        {
            SetupDBConnection();
            new Engine();
            new Address();
            ContactPerson.DeleteAllContactPeople();
            new Car();
            createUpdatedContactPersonTestPack();


            _contactPersonUpdateConcurrency = new ContactPerson();
            _contactPersonUpdateConcurrency.Surname = "Update Concurrency";
            _contactPersonUpdateConcurrency.Save();


            _contactPBeginEditsConcurrency = new ContactPerson();
            _contactPBeginEditsConcurrency.Surname = "BeginEdits Concurrency";
            _contactPBeginEditsConcurrency.Save();

            _contactPTestRefreshFromObjMan = new ContactPerson();
            _contactPTestRefreshFromObjMan.Surname = "FirstSurname";
            _contactPTestRefreshFromObjMan.Save();
            new Engine();
            CreateDeletedPersonTestPack();
            CreateSaveContactPersonTestPack();
  
            //Ensure that a fresh object is loaded from DB
            BusinessObjectManager.Instance.ClearLoadedObjects();
        }

        private void CreateSaveContactPersonTestPack()
        {
            _contactPTestSave = new ContactPerson();
            _contactPTestSave.DateOfBirth = new DateTime(1980, 01, 22);
            _contactPTestSave.FirstName = "Brad";
            _contactPTestSave.Surname = "Vincent1";

            _contactPTestSave.Save(); //save the object to the DB
        }

        private void CreateDeletedPersonTestPack()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.DateOfBirth = new DateTime(1980, 01, 22);
            myContact.FirstName = "Brad";
            myContact.Surname = "Vincent2";

            myContact.Save(); //save the object to the DB
            myContact.MarkForDelete();
            myContact.Save();
        }

        private void createUpdatedContactPersonTestPack()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.DateOfBirth = new DateTime(1969, 01, 29);
            myContact.FirstName = "FirstName";
            myContact.Surname = "Surname";
            myContact.Save();
            _updateContactPersonId = myContact.ID;
        }

        [Test]
        public void TestActivatorCreate()
        {
            object contact = Activator.CreateInstance(typeof (ContactPerson), true);
        }

        [Test]
        public void TestCancelEdits()
        {
            ContactPerson myContact = new ContactPerson();

            Assert.IsFalse(myContact.Status.IsValid());
            myContact.Surname = "My Surname";
            Assert.IsTrue(myContact.Status.IsValid());
            Assert.AreEqual("My Surname", myContact.Surname);
            myContact.CancelEdits();
            Assert.IsFalse(myContact.Status.IsValid());
            Assert.IsTrue(myContact.Surname.Length == 0);
        }

        [Test]
        public void TestCreateContactPerson()
        {
            ContactPerson myContact = new ContactPerson();
            Assert.IsNotNull(myContact);
            myContact.DateOfBirth = new DateTime(1980, 01, 22);
            myContact.FirstName = "Brad";
            myContact.Surname = "Vincent3";

            Assert.AreEqual("Brad", myContact.FirstName);
            Assert.AreEqual(new DateTime(1980, 01, 22), myContact.DateOfBirth);
        }

        [Test]
        public void TestDeleteFlagsSetContactPerson()
        {
            ContactPerson myContact = new ContactPerson();
            Assert.IsTrue(myContact.Status.IsNew); // this object is new
            myContact.DateOfBirth = new DateTime(1980, 01, 22);
            myContact.FirstName = "Brad";
            myContact.Surname = "Vincent4";

            myContact.Save(); //save the object to the DB
            Assert.IsFalse(myContact.Status.IsNew); // this object is saved and thus no longer
            // new
            Assert.IsFalse(myContact.Status.IsDeleted);

            IPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);
            //Put a loop in to take up some time due to MSAccess 
            myContact.MarkForDelete();
            Assert.IsTrue(myContact.Status.IsDeleted);
            myContact.Save();
            Assert.IsTrue(myContact.Status.IsDeleted);
            Assert.IsTrue(myContact.Status.IsNew);
        }


        [Test]
        public void TestEditTwoInstancesContactPerson()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.DateOfBirth = new DateTime(1980, 01, 22);
            myContact.FirstName = "Brad";
            myContact.Surname = "Vincent5";

            myContact.Save(); //save the object to the DB

            IPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);

            ContactPerson mySecondContactPerson =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(id);

            Assert.AreEqual(myContact.ID,
                            mySecondContactPerson.ID);
            Assert.AreEqual(myContact.FirstName, mySecondContactPerson.FirstName);
            Assert.AreEqual(myContact.DateOfBirth, mySecondContactPerson.DateOfBirth);

            //Change the MyContact's Surname see if mySecondContactPerson is changed.
            //this should change since the second contact person was obtained from object manager and 
            // these should thus be the same instance.
            myContact.Surname = "New Surname";
            Assert.AreEqual(myContact.Surname, mySecondContactPerson.Surname);
        }


//		[Test]
//		[ExpectedException(typeof (InvalidPropertyValueException))]
//		public void TestObjectSurnameTooLong() {
//			ContactPerson myContact = new ContactPerson();
//			myContact.Surname = "MyPropertyIsTooLongByFarThisWill Cause and Error in Bus object";
//		}

        /// <summary>
        /// Tests to ensure that if the new object that is being saved to the database is always
        /// unique.
        /// </summary>
        [Test]
        [ExpectedException(typeof (BusObjDuplicateConcurrencyControlException))]
        public void TestForDuplicateExistingObjects()
        {
            //create the first object
            ContactPerson myContact_1 = new ContactPerson();

            //Edit first object and save
            myContact_1.Surname = "My Surname";
            myContact_1.SetPropertyValue("PK2Prop1", "PK2Prop1Value1");
            myContact_1.SetPropertyValue("PK2Prop2", "PK2Prop1Value2");
            myContact_1.Save(); //
            //get the second new object from the object manager);
            ContactPerson myContact_2 = new ContactPerson();
            myContact_2.SetPropertyValue("PK2Prop1", "PK2Prop1Value1  Two");
            myContact_2.Surname = "My Surname two";
            myContact_2.Save();

            //set this new object to have the same 
            // data as the already saved object
            myContact_2.SetPropertyValue("PK2Prop1", myContact_1.GetPropertyValue("PK2Prop1"));
            myContact_2.SetPropertyValue("PK2Prop2", myContact_1.GetPropertyValue("PK2Prop2"));
            myContact_2.Save(); //Should raise an errors
        }

        /// <summary>
        /// Tests to ensure that if the new object that is being saved to the database is always
        /// unique.
        /// </summary>
        [Test]
        [ExpectedException(typeof (BusObjDuplicateConcurrencyControlException))]
        public void TestForDuplicateNewObjects()
        {
            //create the first object
            ContactPerson myContact_1 = new ContactPerson();

            //Edit first object and save
            myContact_1.Surname = "My Surname";
            myContact_1.SetPropertyValue("PK2Prop1", "PK2Prop1Value1");
            myContact_1.SetPropertyValue("PK2Prop2", "PK2Prop1Value2");
            myContact_1.Save(); //
            //get the second new object from the object manager;
            ContactPerson myContact_2 = new ContactPerson();
            //set this new object to have the same 
            // data as the already saved object
            myContact_2.Surname = "My Surname";
            myContact_2.SetPropertyValue("PK2Prop1", myContact_1.GetPropertyValue("PK2Prop1"));
            myContact_2.SetPropertyValue("PK2Prop2", myContact_1.GetPropertyValue("PK2Prop2"));
            myContact_2.Save(); //Should raise an errors
        }

        /// <summary>
        /// Tests to ensure that if the new object that is being saved to the database is always
        /// unique.
        /// </summary>
        [Test]
        [ExpectedException(typeof (BusObjDuplicateConcurrencyControlException))]
        public void TestForDuplicateNewObjectsSinglePropKey()
        {
            //create the first object
            ContactPerson myContact_1 = new ContactPerson();

            //Edit first object and save
            myContact_1.Surname = "My Surname SinglePropKey 1";
            myContact_1.SetPropertyValue("PK3Prop", "PK3PropValue1");
            myContact_1.Save(); //
            //get the second new object from the object manager
            ContactPerson myContact_2 = new ContactPerson();
            //set this new object to have the same 
            // data as the already saved object
            myContact_2.Surname = "My Surname SinglePropKey 22";
            myContact_2.SetPropertyValue("PK3Prop", myContact_1.GetPropertyValue("PK3Prop"));
            myContact_2.Save(); //Should raise an errors
        }

        [Test]
        [ExpectedException(typeof (BusObjDuplicateConcurrencyControlException))]
        public void TestForDuplicateNewObjectsSinglePropKeyNull()
        {
            //create the first object
            ContactPerson myContact_1 = new ContactPerson();

            //Edit first object and save
            myContact_1.Surname = "My Surname SinglePropKeyNull";
            myContact_1.SetPropertyValue("PK3Prop", null); // set the previous value to null
            myContact_1.Save(); //
            //get the second new object from the object manager
            ContactPerson myContact_2 = new ContactPerson();
            //set this new object to have the same 
            // data as the already saved object
            myContact_2.Surname = "My Surname SinglePropKeyNull";
            myContact_2.SetPropertyValue("PK3Prop", myContact_1.GetPropertyValue("PK3Prop"));
            // set the previous value to null
            myContact_2.Save(); //Should raise an errors
        }

        [Test]
        public void TestMultipleUpdates_NoConcurrencyErrors()
        {
            _contactPersonUpdateConcurrency.Surname = "New Surname";
            _contactPersonUpdateConcurrency.Save();
            _contactPersonUpdateConcurrency.Surname = "New Surname 2";
            _contactPersonUpdateConcurrency.Save();
            _contactPersonUpdateConcurrency.Surname = "New Surname 3";
        }

        [Test]
        [ExpectedException(typeof (BusObjectInAnInvalidStateException))]
        public void TestObjectCompulsorySurnameNotSet()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.Save();
        }

        [Test]
        [ExpectedException(typeof (BusObjectInAnInvalidStateException))]
        public void TestObjectSurnameTooLong()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.Surname = "MyPropertyIsTooLongByFarThisWill Cause and Error in Bus object";
            myContact.Save();
        }

        [Test]
        public void TestSaveContactPerson()
        {
            Assert.IsFalse(_contactPTestSave.Status.IsNew); // this object is saved and thus no longer
            // new

            IPrimaryKey id = _contactPTestSave.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, _contactPTestSave.ID);

            ContactPerson mySecondContactPerson =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(id);
            Assert.IsFalse(_contactPTestSave.Status.IsNew); // this object is recovered from the DB
            // and is thus not new.
            Assert.AreEqual(_contactPTestSave.ID.ToString(), mySecondContactPerson.ID.ToString());
            Assert.AreEqual(_contactPTestSave.FirstName, mySecondContactPerson.FirstName);
            Assert.AreEqual(_contactPTestSave.DateOfBirth, mySecondContactPerson.DateOfBirth);

            //Add test to make certain that myContact person and contact person are not 
            // pointing at the same physical object

            _contactPTestSave.FirstName = "Change FirstName";
            Assert.IsFalse(_contactPTestSave.FirstName == mySecondContactPerson.FirstName);
        }

        [Test]
        public void TestStateAfterApplyEdit()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.Surname = "Test Surname";
            myContact.Save();
            Assert.IsFalse(myContact.Status.IsNew, "BO is still IsNew after being saved.");
        }

        [Test]
        public void TestUpdateExistingContactPerson()
        {
            ContactPerson myContactPerson =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(_updateContactPersonId);
            myContactPerson.FirstName = "NewFirstName";
            myContactPerson.Save();

            //waitForDB();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //Reload the person and make sure that the changes have been made.
            ContactPerson myNewContactPerson =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPerson>(_updateContactPersonId);
            Assert.AreEqual("NewFirstName", myNewContactPerson.FirstName,
                            "The firstName was not updated");
        }
    }

    /// <summary>
    /// This is used only for testing reading transactions
    /// </summary>
    public class TransactionLogStub : BusinessObject
    {
        #region Constructors

        public TransactionLogStub()
        {
        }

        public TransactionLogStub(ClassDef def)
            : base(def)
        {
        }

        private static IClassDef GetClassDef()
        {
            return ClassDef.IsDefined(typeof (TransactionLogStub))
                       ? ClassDef.ClassDefs[typeof (TransactionLogStub)]
                       : CreateClassDef();
        }

        protected override IClassDef ConstructClassDef()
        {
            _classDef = (ClassDef) GetClassDef();
            return _classDef;
        }

        //protected override void ConstructFromClassDef(bool newObject)
        //{
        //    base.ConstructFromClassDef(newObject);
        //    //SetTransactionLog(new TransactionLogTable("TransactionLogStub",
        //    //                                          "DateTimeUpdated",
        //    //                                          "WindowsUser",
        //    //                                          "LogonUser",
        //    //                                          "MachineName",
        //    //                                          "BusinessObjectTypeName",
        //    //                                          "CRUDAction",
        //    //                                          "DirtyXML"));
        //}

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["TransactionSequenceNo"]);
            ClassDef lClassDef = new ClassDef(typeof (TransactionLogStub), primaryKey, lPropDefCol, keysCol, null);
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("TransactionSequenceNo", typeof (int), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("DateTimeUpdated", typeof (DateTime), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("WindowsUser", typeof (String), PropReadWriteRule.WriteOnce, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("LogonUser", typeof (String), PropReadWriteRule.ReadOnly, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("BusinessObjectTypeName", typeof (string), PropReadWriteRule.ReadOnly, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("CRUDAction", typeof (string), PropReadWriteRule.ReadOnly, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("DirtyXML", typeof (string), PropReadWriteRule.ReadOnly, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("MachineName", typeof (string), PropReadWriteRule.ReadOnly, null);
            lPropDefCol.Add(propDef);
            return lPropDefCol;
        }

        /// <summary>
        /// returns the TransactionLogStub identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object primary Key</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Habanero.BO.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static TransactionLogStub GetTransactionLog(BOPrimaryKey id)
        {
            //TransactionLogStub myTransactionLogStub = (TransactionLogStub)BOLoader.Instance.GetLoadedBusinessObject(id);
            //if (myTransactionLogStub == null)
            //{
            TransactionLogStub myTransactionLogStub =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<TransactionLogStub>(id);
//            }
            return myTransactionLogStub;
        }

        #endregion //Constructors

        #region ForTesting

        internal static void ClearTransactionLogCol()
        {
            BusinessObjectManager.Instance.ClearLoadedObjects();
        }

        internal static void DeleteAllTransactionLogs()
        {
            const string sql = "DELETE FROM TransactionLogStub";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        #endregion

        #region ForCollections //TODO: refactor this so that class construction occurs in its own 

        //class

        protected internal static BusinessObjectCollection<TransactionLogStub> LoadBusinessObjCol()
        {
            return LoadBusinessObjCol("", "");
        }

        protected internal static BusinessObjectCollection<TransactionLogStub> LoadBusinessObjCol(string searchCriteria,
                                                                                                  string orderByClause)
        {
            //TransactionLogStub lTransactionLogStub = new TransactionLogStub();
            return
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObjectCollection<TransactionLogStub>(
                    searchCriteria,
                    orderByClause);
            //SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection);
            //statement.Statement.Append(lTransactionLogStub.SelectSqlWithNoSearchClause());
            //if (searchCriteria.Length > 0)
            //{
            //    statement.AppendCriteria("");
            //    SqlCriteriaCreator creator =
            //        new SqlCriteriaCreator(Expression.CreateExpression(searchCriteria), lTransactionLogStub);
            //    creator.AppendCriteriaToStatement(statement);
            //}
            //BusinessObjectCollection<BusinessObject> bOCol = new BusinessObjectCollection<BusinessObject>(lTransactionLogStub.ClassDef);
            //using (IDataReader dr = DatabaseConnection.CurrentConnection.LoadDataReader(statement, orderByClause))
            //{
            //    while (dr.Read())
            //    {
            //        BOLoader.Instance.LoadProperties(lTransactionLogStub, dr);
            //        TransactionLogStub lTempPerson2 = (TransactionLogStub)BOLoader.Instance.GetLoadedBusinessObject(lTransactionLogStub.GetObjectNewID());
            //        if (lTempPerson2 == null)
            //        {
            //            bOCol.Add(lTransactionLogStub);
            //        }
            //        else
            //        {
            //            bOCol.Add(lTempPerson2);
            //        }
            //    }
            //}
            //return bOCol;
        }

        #endregion
    }
}