//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using System.Data;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for Tester.
    /// </summary>
    [TestFixture]
    public class Tester : TestUsingDatabase
    {
        private BOPrimaryKey deleteContactPersonID;
        private BOPrimaryKey updateContactPersonID;
        private int dbInterval = 1;
        private ContactPerson mContactPersonUpdateConcurrency;
        private ContactPerson mContactPersonDeleteConcurrency;
        private ContactPerson mContactPBeginEditsConcurrency;
        private ContactPerson mCotanctPTestRefreshFromObjMan;
        private ContactPerson mContactPDeleted;
        private ContactPerson mContactPTestSave;

        [TestFixtureSetUp]
        public void CreateTestPack()
        {
            this.SetupDBConnection();
            ContactPerson.DeleteAllContactPeople();

            createUpdatedContactPersonTestPack();

            createDeleteContactPersonTestPack();
            mContactPersonUpdateConcurrency = new ContactPerson();
            mContactPersonUpdateConcurrency.Surname = "Update Concurrency";
            mContactPersonUpdateConcurrency.Save();

            mContactPersonDeleteConcurrency = new ContactPerson();
            mContactPersonDeleteConcurrency.Surname = "Delete Concurrency";
            mContactPersonDeleteConcurrency.Save();

            mContactPBeginEditsConcurrency = new ContactPerson();
            mContactPBeginEditsConcurrency.Surname = "BeginEdits Concurrency";
            mContactPBeginEditsConcurrency.Save();

            mCotanctPTestRefreshFromObjMan = new ContactPerson();
            mCotanctPTestRefreshFromObjMan.Surname = "FirstSurname";
            mCotanctPTestRefreshFromObjMan.Save();

            CreateDeletedPersonTestPack();
            CreateSaveContactPersonTestPack();
            //waitForDB();
            //Ensure that a fresh object is loaded from DB
            ContactPerson.ClearContactPersonCol();
        }

        private void CreateSaveContactPersonTestPack()
        {
            mContactPTestSave = new ContactPerson();
            mContactPTestSave.DateOfBirth = new DateTime(1980, 01, 22);
            mContactPTestSave.FirstName = "Brad";
            mContactPTestSave.Surname = "Vincent1";

            mContactPTestSave.Save(); //save the object to the DB
        }

        private void CreateDeletedPersonTestPack()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.DateOfBirth = new DateTime(1980, 01, 22);
            myContact.FirstName = "Brad";
            myContact.Surname = "Vincent2";

            myContact.Save(); //save the object to the DB
            //waitForDB();
            myContact.Delete();
            myContact.Save();

            mContactPDeleted = myContact;
        }

        private void createUpdatedContactPersonTestPack()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.DateOfBirth = new DateTime(1969, 01, 29);
            myContact.FirstName = "FirstName";
            myContact.Surname = "Surname";
            myContact.Save();
            updateContactPersonID = myContact.ID;
        }

        private void createDeleteContactPersonTestPack()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.FirstName = "To Be deleted";
            myContact.Surname = "To Be deleted";
            myContact.Save();
            deleteContactPersonID = myContact.ID;
        }

//		private void waitForDB() {
//			Thread.Sleep(dbInterval);
//		}

        [Test]
        public void TestUpdateExistingContactPerson()
        {
            ContactPerson myContactPerson = ContactPerson.GetContactPerson(updateContactPersonID);
            myContactPerson.FirstName = "NewFirstName";
            myContactPerson.Save();

            //waitForDB();
            ContactPerson.ClearContactPersonCol();
            //Reload the person and make sure that the changes have been made.
            ContactPerson myNewContactPerson = ContactPerson.GetContactPerson(updateContactPersonID);
            Assert.AreEqual("NewFirstName", myNewContactPerson.FirstName,
                            "The firstName was not updated");
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
        public void TestSaveContactPerson()
        {
            Assert.IsFalse(mContactPTestSave.State.IsNew); // this object is saved and thus no longer
            // new

            BOPrimaryKey id = mContactPTestSave.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, mContactPTestSave.ID);

            ContactPerson mySecondContactPerson = ContactPerson.GetContactPerson(id);
            Assert.IsFalse(mContactPTestSave.State.IsNew); // this object is recovered from the DB
            // and is thus not new.
            Assert.AreEqual(mContactPTestSave.ID.ToString(), mySecondContactPerson.ID.ToString());
            Assert.AreEqual(mContactPTestSave.FirstName, mySecondContactPerson.FirstName);
            Assert.AreEqual(mContactPTestSave.DateOfBirth, mySecondContactPerson.DateOfBirth);

            //Add test to make certain that myContact person and contact person are not 
            // pointing at the same physical object

            mContactPTestSave.FirstName = "Change FirstName";
            Assert.IsFalse(mContactPTestSave.FirstName == mySecondContactPerson.FirstName);
        }

        [Test]
        public void TestDeleteFlagsSetContactPerson()
        {
            ContactPerson myContact = new ContactPerson();
            Assert.IsTrue(myContact.State.IsNew); // this object is new
            myContact.DateOfBirth = new DateTime(1980, 01, 22);
            myContact.FirstName = "Brad";
            myContact.Surname = "Vincent4";

            myContact.Save(); //save the object to the DB
            Assert.IsFalse(myContact.State.IsNew); // this object is saved and thus no longer
            // new
            Assert.IsFalse(myContact.State.IsDeleted);

            BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);
            //Put a loop in to take up some time due to MSAccess 
            myContact.Delete();
            Assert.IsTrue(myContact.State.IsDeleted);
            myContact.Save();
            Assert.IsTrue(myContact.State.IsDeleted);
            Assert.IsTrue(myContact.State.IsNew);
        }

        [Test]
        [ExpectedException(typeof (BusinessObjectNotFoundException))]
        public void TestDeleteContactPerson()
        {
            ContactPerson mySecondContactPerson = ContactPerson.GetContactPerson(mContactPDeleted.ID);
        }

        [Test]
        public void TestEditTwoInstancesContactPerson()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.DateOfBirth = new DateTime(1980, 01, 22);
            myContact.FirstName = "Brad";
            myContact.Surname = "Vincent5";

            myContact.Save(); //save the object to the DB

            BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);

            ContactPerson mySecondContactPerson = ContactPerson.GetContactPerson(id);

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

        [Test]
        public void TestObjectBeingRemovedFromCollection()
        {
            ContactPerson myContact = new ContactPerson();
            BOPrimaryKey id = myContact.ID;
            myContact = null; // clear the person s.t. the GC can collect
            GC.Collect(); //Force the GC to collect
            //waitForDB(); // wait to give GC time to collect
            Dictionary<string, WeakReference> boCol = BusinessObject.AllLoaded();
            Assert.IsFalse(boCol.ContainsKey(id.ToString()), "Object has not been removed from the dictionary");
        }

//		[Test]
//		[ExpectedException(typeof (InvalidPropertyValueException))]
//		public void TestObjectSurnameTooLong() {
//			ContactPerson myContact = new ContactPerson();
//			myContact.Surname = "MyPropertyIsTooLongByFarThisWill Cause and Error in Bus object";
//		}

        [Test]
        [ExpectedException(typeof (BusObjectInAnInvalidStateException))]
        public void TestObjectSurnameTooLong()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.Surname = "MyPropertyIsTooLongByFarThisWill Cause and Error in Bus object";
            myContact.Save();
        }

        [Test]
        [ExpectedException(typeof (BusObjectInAnInvalidStateException))]
        public void TestObjectCompulsorySurnameNotSet()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.Save();
        }

        [Test]
        public void TestCancelEdits()
        {
            ContactPerson myContact = new ContactPerson();

            Assert.IsFalse(myContact.IsValid());
            myContact.Surname = "My Surname";
            Assert.IsTrue(myContact.IsValid());
            Assert.AreEqual("My Surname", myContact.Surname);
            myContact.Restore();
            Assert.IsFalse(myContact.IsValid());
            Assert.IsTrue(myContact.Surname.Length == 0);
        }

        [Test]
        public void TestStateAfterApplyEdit()
        {
            ContactPerson myContact = new ContactPerson();
            myContact.Surname = "Test Surname";
            myContact.Save();
            Assert.IsFalse(myContact.State.IsNew, "BO is still IsNew after being saved.");
        }

        //TODO-Peter - check with Brett exactly what this should be doing - it's failing now
        // but I can't see how it could pass.
        //		[Test]
        //		[ExpectedException(typeof (BusObjOptimisticConcurrencyControlException))]
        //		public void TestOptimisticConcurrencyControl() {
        //			ContactPerson myContact = mContactPersonUpdateConcurrency;
        //			//Ensure that we have two physical instances of the same logical contact person
        ////			ContactPerson.ClearBusinessObjectBaseCol();//Ensure that a fresh object is loaded from DB
        //			ContactPerson myContact2 = ContactPerson.GetContactPerson(myContact.ID);
        //
        //			myContact.Surname = "New Surname"; //edit first object
        //			myContact2.Surname = "New Surname2"; //edit second object
        //			Assert.IsFalse(object.ReferenceEquals(myContact, myContact2));
        //			myContact.Save(); //save first
        //			waitForDB();
        //			myContact2.Save(); //save second
        //		}
        //
        [Test]
            public void TestMultipleUpdates()
        {
            mContactPersonUpdateConcurrency.Surname = "New Surname";
            mContactPersonUpdateConcurrency.Save();
            //waitForDB();
            mContactPersonUpdateConcurrency.Surname = "New Surname 2";
            mContactPersonUpdateConcurrency.Save();
            //waitForDB();
            mContactPersonUpdateConcurrency.Surname = "New Surname 3";
        }

        //TODO: concurrency control is disabled - to implement again
        [Test]
        [ExpectedException(typeof (BusObjDeleteConcurrencyControlException)), Ignore]
            public void TestDeleteObjectPriorToUpdatesConcurrencyControl()
        {
            ContactPerson myContact2 = ContactPerson.GetContactPerson(mContactPersonDeleteConcurrency.ID);
            mContactPersonDeleteConcurrency.Delete();
            myContact2.Surname = "New Surname 2";
            mContactPersonDeleteConcurrency.Save();
            //waitForDB();
            myContact2.Save();
        }

        //TODO: concurrency control is disabled - to implement again
        [Test]
        [ExpectedException(typeof (BusObjBeginEditConcurrencyControlException)), Ignore]
            public void TestBeginEditsOnADirtyObject()
        {
            ContactPerson.ClearContactPersonCol();
            //load second person from DB.
            mContactPBeginEditsConcurrency.Surname = "First Update to Surname";

            ContactPerson myContact2 = ContactPerson.GetContactPerson(mContactPBeginEditsConcurrency.ID);
            Assert.IsTrue(myContact2 != mContactPBeginEditsConcurrency, "two objects are the same");
            mContactPBeginEditsConcurrency.Surname = "First Update to Surname";

            mContactPBeginEditsConcurrency.Save(); //save first object
            //Try edit second instance (should raise error)
            //waitForDB();
            myContact2.Surname = "Second Update To Surname";
        }

        /// <summary>
        /// Tests to ensure that if the object has been edited in the object manager by 
        /// another user the one we get back is always the latest.
        /// </summary>
        [Test]
        public void TestAlwaysGetTheFreshestObject()
        {
            ContactPerson originalContactPerson = new ContactPerson();
            originalContactPerson.Surname = "FirstSurname";
            originalContactPerson.Save();

            ContactPerson.ClearContactPersonCol();

            //load second object from DB to ensure that it is now in the object manager
            ContactPerson myContact2 = ContactPerson.GetContactPerson(originalContactPerson.ID);

            //Edit first object and save
            originalContactPerson.Surname = "SecondSurname";
            originalContactPerson.Save(); //

            ContactPerson myContact3 = ContactPerson.GetContactPerson(originalContactPerson.ID);

            //The two surnames should be equal since the myContact3 was refreshed
            // when it was loaded.
            Assert.AreEqual(originalContactPerson.Surname, myContact3.Surname);
            //Just to check the myContact2 should also match since it is physically the 
            // same object as myContact3
            Assert.AreEqual(originalContactPerson.Surname, myContact2.Surname);
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
            //get the second new object from the object manager
            //waitForDB();
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
        public void TestForDuplicateExistingObjects()
        {
            //create the first object
            ContactPerson myContact_1 = new ContactPerson();

            //Edit first object and save
            myContact_1.Surname = "My Surname";
            myContact_1.SetPropertyValue("PK2Prop1", "PK2Prop1Value1");
            myContact_1.SetPropertyValue("PK2Prop2", "PK2Prop1Value2");
            myContact_1.Save(); //
            //get the second new object from the object manager
            //waitForDB();
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
        public void TestForDuplicateNewObjectsSinglePropKey()
        {
            //create the first object
            ContactPerson myContact_1 = new ContactPerson();

            //Edit first object and save
            myContact_1.Surname = "My Surname SinglePropKey 1";
            myContact_1.SetPropertyValue("PK3Prop", "PK3PropValue1");
            myContact_1.Save(); //
            //get the second new object from the object manager
            //waitForDB();
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
            //waitForDB();
            ContactPerson myContact_2 = new ContactPerson();
            //set this new object to have the same 
            // data as the already saved object
            myContact_2.Surname = "My Surname SinglePropKeyNull";
            myContact_2.SetPropertyValue("PK3Prop", myContact_1.GetPropertyValue("PK3Prop"));
            // set the previous value to null
            myContact_2.Save(); //Should raise an errors
        }

        //TODO: Transaction log is not implemented under the new TransactionCommitter design (yet)
        [Test, Ignore]
        public void TestDirtyXml()
        {
            TransactionLog.DeleteAllTransactionLogs();

            ContactPerson myContact_1 = new ContactPerson();
            //Edit first object and save
            myContact_1.Surname = "My Surname 1";
            //myContact_1.SetPropertyValue("PK3Prop", null); // set the previous value to null
            myContact_1.Save(); //

            myContact_1.Surname = "My Surname New";

            Assert.AreEqual(
                "<ContactPerson ID=" + myContact_1.ID +
                "><Properties><Surname><PreviousValue>My Surname 1</PreviousValue><NewValue>My Surname New</NewValue></Surname><ContactPerson>",
                myContact_1.DirtyXML);

            myContact_1.Save();

            myContact_1.Delete();
            myContact_1.Save();

            BusinessObjectCollection<BusinessObject> myCol = TransactionLog.LoadBusinessObjCol("", "TransactionSequenceNo");
            Assert.AreEqual(myCol.Count, 3);

            TransactionLog myTransactionLog;
            int maxTransactionNo = 0;
            //TODO: hack until loading collections works better.

            //Get max transaction number
            for (int i = 0; i <= 2; i++)
            {
                myTransactionLog = (TransactionLog) myCol[i];
                if (maxTransactionNo < (int) myTransactionLog.GetPropertyValue("TransactionSequenceNo"))
                {
                    maxTransactionNo = (int) myTransactionLog.GetPropertyValue("TransactionSequenceNo");
                }
            }
            //Get latest transaction
            //TODO: this does not work at all the same transaction object is returned
            // each time regardless of whether i get item 0, 1 or 2.
            //this is possibly some bug with filling the collection.
            for (int i = 0; i <= 2; i++)
            {
                myTransactionLog = null;
                myTransactionLog = (TransactionLog) myCol[i];
                if ((int) myTransactionLog.GetPropertyValue("TransactionSequenceNo") == (maxTransactionNo - 2))
                {
                    Assert.AreEqual("Created", myTransactionLog.GetPropertyValue("CRUDAction"));
                }
                else if ((int) myTransactionLog.GetPropertyValue("TransactionSequenceNo") == (maxTransactionNo - 1))
                {
                    Assert.AreEqual("Updated", myTransactionLog.GetPropertyValue("CRUDAction"));
                }
                else if ((int) myTransactionLog.GetPropertyValue("TransactionSequenceNo") == (maxTransactionNo))
                {
                    Assert.AreEqual("Deleted", myTransactionLog.GetPropertyValue("CRUDAction"));
                }
                else
                {
                    Assert.AreEqual("", "1"); //force failure
                }
            }
        }

        //[Test]
        //public void TestTransactionSuccess()
        //{
        //    ContactPerson myContact_1 = new ContactPerson();
        //    //Edit first object and save
        //    myContact_1.Surname = "My Surname 1";
        //    //myContact_1.SetPropertyValue("PK3Prop", null); // set the previous value to null
        //    Transaction transact = new Transaction(DatabaseConnection.CurrentConnection);
        //    transact.AddTransactionObject(myContact_1);

        //    ContactPerson myContact_2 = new ContactPerson();
        //    myContact_2.Surname = "My Surname 2";

        //    Assert.IsTrue(myContact_2.IsValid());

        //    transact.AddTransactionObject(myContact_2);

        //    transact.CommitTransaction();

        //    Assert.IsFalse(myContact_2.State.IsDirty);
        //    Assert.IsFalse(myContact_1.State.IsNew);
        //    Assert.IsFalse(myContact_1.State.IsDirty);
        //    Assert.IsFalse(myContact_2.State.IsNew);
        //    Assert.IsTrue(myContact_2.IsValid());

        //    //Ensure object loaded from DB.
        //    ContactPerson.ClearContactPersonCol();

        //    ContactPerson myContact_3 = ContactPerson.GetContactPerson(myContact_1.ID);

        //    Assert.AreEqual(myContact_1.ID, myContact_3.ID);
        //    Assert.AreEqual(myContact_1.Surname, myContact_3.Surname);

        //    ContactPerson myContact_4 = ContactPerson.GetContactPerson(myContact_2.ID);

        //    Assert.AreEqual(myContact_2.ID, myContact_4.ID);
        //    Assert.AreEqual(myContact_2.Surname, myContact_4.Surname);
        //}

        //[Test]
        //public void TestTransactionFail()
        //{
        //    ContactPerson myContact_1 = new ContactPerson();
        //    //Edit first object and save
        //    myContact_1.Surname = "My Surname 1";
        //    //myContact_1.SetPropertyValue("PK3Prop", null); // set the previous value to null
        //    Transaction transact = new Transaction(DatabaseConnection.CurrentConnection);
        //    transact.AddTransactionObject(myContact_1);

        //    ContactPerson myContact_2 = new ContactPerson();
        //    myContact_2.Surname = "My Surname 1"; //Should result in a duplicate error when try to persist
        //    //will result in the commit failing
        //    Assert.IsTrue(myContact_2.IsValid());
        //    transact.AddTransactionObject(myContact_2);
        //    bool errorRaised = false;
        //    try
        //    {
        //        transact.CommitTransaction();
        //    }
        //    catch (Exception ex) //todo:check type of error?
        //    {
        //        errorRaised = true;
        //    }

        //    Assert.IsTrue(errorRaised, "Error should have been raised");

        //    Assert.IsTrue(myContact_2.State.IsDirty);
        //    Assert.IsTrue(myContact_1.State.IsNew);
        //    Assert.IsTrue(myContact_1.State.IsDirty);
        //    Assert.IsTrue(myContact_2.State.IsNew);
        //    Assert.IsTrue(myContact_2.IsValid());
        //    Assert.IsTrue(myContact_2.IsValid());


        //    //Ensure object loaded from DB.
        //    ContactPerson.ClearContactPersonCol();
        //    errorRaised = false;
        //    try
        //    {
        //        ContactPerson myContact_3 = ContactPerson.GetContactPerson(myContact_1.ID);
        //    }
        //        //Expect this error since the object should not have been persisted to the DB.
        //    catch (BusinessObjectNotFoundException ex)
        //    {
        //        errorRaised = true;
        //    }
        //    Assert.IsTrue(errorRaised);


        //    //Test canceledits to transaction
        //    transact.CancelEdits();
        //    Assert.IsTrue(myContact_1.Surname.Length == 0);
        //    Assert.IsTrue(myContact_2.Surname.Length == 0);
        //    Assert.IsFalse(myContact_2.IsValid());
        //    Assert.IsFalse(myContact_2.IsValid());
        //}

        #region tests

        [Test]
        public void TestActivatorCreate()
        {
            object contact = Activator.CreateInstance(typeof (ContactPerson), true);
        }

        #endregion tests

    }

    /// <summary>
    /// This is used only for testing reading transactions
    /// </summary>
    public class TransactionLog : BusinessObject
    {
        #region Constructors

        public TransactionLog() : base()
        {
        }

        internal TransactionLog(BOPrimaryKey id)
            : base(id)
        {
        }

        public TransactionLog(ClassDef def)
            : base(def)
        {
        }

        private static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (TransactionLog)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (TransactionLog)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            _classDef = GetClassDef();
            return _classDef;
        }

        protected override void ConstructFromClassDef(bool newObject)
        {
            base.ConstructFromClassDef(newObject);
            SetTransactionLog(new TransactionLogTable("TransactionLog",
                                                      "DateTimeUpdated",
                                                      "WindowsUser",
                                                      "LogonUser",
                                                      "MachineName",
                                                      "BusinessObjectTypeName",
                                                      "CRUDAction",
                                                      "DirtyXML"));
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["TransactionSequenceNo"]);
            ClassDef lClassDef = new ClassDef(typeof (TransactionLog), primaryKey, lPropDefCol, keysCol, null);
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
        /// returns the TransactionLog identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object primary Key</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Habanero.BO.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static TransactionLog GetTransactionLog(BOPrimaryKey id)
        {
            TransactionLog myTransactionLog = (TransactionLog)BOLoader.Instance.GetLoadedBusinessObject(id);
            if (myTransactionLog == null)
            {
                myTransactionLog = new TransactionLog(id);
            }
            return myTransactionLog;
        }

        #endregion //Constructors

        #region ForTesting

        internal static void ClearTransactionLogCol()
        {
            ClearLoadedBusinessObjectBaseCol();
        }

        internal static void DeleteAllTransactionLogs()
        {
            string sql = "DELETE FROM TransactionLog";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        #endregion

        #region ForCollections //TODO: refactor this so that class construction occurs in its own 

        //class
        protected internal string GetObjectNewID()
        {
            return _primaryKey.GetObjectNewID();
        }

        protected internal static BusinessObjectCollection<BusinessObject> LoadBusinessObjCol()
        {
            return LoadBusinessObjCol("", "");
        }

        protected internal static BusinessObjectCollection<BusinessObject> LoadBusinessObjCol(string searchCriteria,
                                                                                  string orderByClause)
        {
            TransactionLog lTransactionLog = new TransactionLog();
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection);
            statement.Statement.Append(lTransactionLog.SelectSqlWithNoSearchClause());
            if (searchCriteria.Length > 0)
            {
                statement.AppendCriteria("");
                SqlCriteriaCreator creator =
                    new SqlCriteriaCreator(Expression.CreateExpression(searchCriteria), lTransactionLog);
                creator.AppendCriteriaToStatement(statement);
            }
            BusinessObjectCollection<BusinessObject> bOCol = new BusinessObjectCollection<BusinessObject>(lTransactionLog.ClassDef);
            using (IDataReader dr = DatabaseConnection.CurrentConnection.LoadDataReader(statement, orderByClause))
            {
                try
                {
                    while (dr.Read())
                    {
                        BOLoader.Instance.LoadProperties(lTransactionLog, dr);
                        TransactionLog lTempPerson2;
                        lTempPerson2 = (TransactionLog)BOLoader.Instance.GetLoadedBusinessObject(lTransactionLog.GetObjectNewID());
                        if (lTempPerson2 == null)
                        {
                            bOCol.Add(lTransactionLog);
                        }
                        else
                        {
                            bOCol.Add(lTempPerson2);
                        }
                    }
                }
                finally
                {
                    if (dr != null && !(dr.IsClosed))
                    {
                        dr.Close();
                    }
                }
            }
            return bOCol;
        }

        #endregion
    }
}
