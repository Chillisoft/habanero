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
using Habanero.Base;
using Habanero.BO;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for ContactPersonCompositeKeyTester.
    /// </summary>
    [TestFixture]
    public class TesterContactPersonCompositeKey : TestUsingDatabase
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            //Ensure that a fresh object is loaded from DB
            BusinessObjectManager.Instance.ClearLoadedObjects();
        }

        #endregion

        private ContactPersonCompositeKey mContactPTestSave;
        private ContactPersonCompositeKey mContactPDeleted;
        private IPrimaryKey updateContactPersonID;

        //public static void RunTest()
        //{
        //    TesterContactPersonCompositeKey test = new TesterContactPersonCompositeKey();
        //    test.TestFixtureSetup();
        //    //			test.TestSaveContactPerson();
        //    //			test.ModifyObjectsPrimaryKey();
        //    test.TestUpdateExistingContactPerson();
        //    //			test.RecoverNewObjectFromObjectManagerBeforeAndAfterPersist();
        //    test.SaveNewObjectWithDuplicatePrimaryKey();
        //}

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            SetupDBConnection();
            DeleteAllContactPersons();
            CreateUpdatedContactPersonTestPack();
            CreateSaveContactPersonTestPack();
            CreateDeletedPersonTestPack();
        }

        private void DeleteAllContactPersons()
        {
            //string connectstring = @"data source=Core;database=WorkShopManagement;uid=sa;pwd=;";

            //IDbConnection con = new SqlConnection(connectstring);
            //con.Open();
            //IDbCommand cmd = con.CreateCommand();
            //cmd.CommandText = "DELETE FROM ContactPersonCompositeKey";
            //cmd.ExecuteNonQuery();
            Database.ExecuteRawSql("DELETE FROM ContactPersonCompositeKey",
                                   DatabaseConnection.CurrentConnection.GetConnection());
        }

        private void CreateSaveContactPersonTestPack()
        {
            mContactPTestSave = new ContactPersonCompositeKey();
            mContactPTestSave.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            mContactPTestSave.SetPropertyValue("FirstName", "Brad");
            mContactPTestSave.SetPropertyValue("Surname", "Vincent");
            mContactPTestSave.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            mContactPTestSave.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            mContactPTestSave.Save(); //save the object to the DB
        }

        private void CreateDeletedPersonTestPack()
        {
            ContactPersonCompositeKey myContact = new ContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            myContact.Save(); //save the object to the DB
            myContact.Delete();
            myContact.Save();

            mContactPDeleted = myContact;
        }

        private void CreateUpdatedContactPersonTestPack()
        {
            ContactPersonCompositeKey myContact = new ContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1969, 01, 29));
            myContact.SetPropertyValue("FirstName", "FirstName");
            myContact.SetPropertyValue("Surname", "Surname");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            myContact.Save();
            updateContactPersonID = myContact.ID;
        }

        [Test]
        public void ChangeObjectPrimaryKeyAndThenCreateNewObjectWithPreviousPrimaryKey()
        {
            ContactPersonCompositeKey myContact = new ContactPersonCompositeKey();
            myContact.SetPropertyValue("Surname", "Vincent");
            Guid prop_1_ID = Guid.NewGuid();
            Guid prop_2_ID = Guid.NewGuid();
            myContact.SetPropertyValue("PK1Prop1", prop_1_ID.ToString());
            myContact.SetPropertyValue("PK1Prop2", prop_2_ID.ToString());
            myContact.Save();
            //modify the primary key
            myContact.SetPropertyValue("PK1Prop1", prop_1_ID + "1");
            myContact.SetPropertyValue("PK1Prop1", prop_1_ID + "1");
            myContact.Save();

            ContactPersonCompositeKey myContactTwo = new ContactPersonCompositeKey();
            myContactTwo.SetPropertyValue("Surname", "Vincent 2");
            myContactTwo.SetPropertyValue("PK1Prop1", prop_1_ID.ToString());
            myContactTwo.SetPropertyValue("PK1Prop2", prop_2_ID.ToString());
            myContactTwo.Save();
        }

        [Test]
        public void CreateTwoConsecutiveObjects()
        {
            ContactPersonCompositeKey myContact = new ContactPersonCompositeKey();
            ContactPersonCompositeKey mySecondContactPerson =
                new ContactPersonCompositeKey();
        }

        [Test]
        public void ModifyObjectsPrimaryKey()
        {
            ContactPersonCompositeKey myContact = new ContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            IPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            myContact.Save(); //save the object to the DB

            //			BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);
            //			System.Console.WriteLine("ID:" + id.GetObjectId());
            //			System.Console.WriteLine("Contact.ID:" + myContact.ID.GetObjectId());
            ContactPersonCompositeKey mySecondContactPerson = ContactPersonCompositeKey.GetContactPersonCompositeKey(id);
            Assert.AreSame(myContact, mySecondContactPerson);

//            Assert.IsTrue(ReferenceEquals(myContact, mySecondContactPerson));
            Assert.AreEqual(myContact.ID,
                            mySecondContactPerson.ID);
            Assert.AreEqual(myContact.GetPropertyValue("FirstName"), mySecondContactPerson.GetPropertyValue("FirstName"));
            Assert.AreEqual(myContact.GetPropertyValue("DateOfBirth"),
                            mySecondContactPerson.GetPropertyValue("DateOfBirth"));

            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            myContact.Save(); //save the object to the DB

            ContactPersonCompositeKey myContactPerson3 = ContactPersonCompositeKey.GetContactPersonCompositeKey(id);
            Assert.IsTrue(ReferenceEquals(myContact, myContactPerson3));
            Assert.AreEqual(myContact.ID,
                            myContactPerson3.ID);
            Assert.AreEqual(myContact.GetPropertyValue("FirstName"), myContactPerson3.GetPropertyValue("FirstName"));

            //Change the MyContact's Surname see if mySecondContactPerson is changed.
            //this should change since the second contact person was obtained from object manager and 
            // these should thus be the same instance.
            myContact.SetPropertyValue("Surname", "New Surname");
            Assert.AreEqual(myContact.GetPropertyValue("Surname"), mySecondContactPerson.GetPropertyValue("Surname"));
        }

        [Test]
        public void RecoverNewObjectFromObjectManagerBeforeAndAfterPersist()
        {
            ContactPersonCompositeKey myContact = new ContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            IPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            myContact.Save(); //save the object to the DB

            //			BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);

            ContactPersonCompositeKey mySecondContactPerson = ContactPersonCompositeKey.GetContactPersonCompositeKey(id);

            Assert.IsTrue(ReferenceEquals(myContact, mySecondContactPerson));
            Assert.AreEqual(myContact.ID,
                            mySecondContactPerson.ID);
            Assert.AreEqual(myContact.GetPropertyValue("FirstName"), mySecondContactPerson.GetPropertyValue("FirstName"));
            Assert.AreEqual(myContact.GetPropertyValue("DateOfBirth"),
                            mySecondContactPerson.GetPropertyValue("DateOfBirth"));

            //Change the MyContact's Surname see if mySecondContactPerson is changed.
            //this should change since the second contact person was obtained from object manager and 
            // these should thus be the same instance.
            myContact.SetPropertyValue("Surname", "New Surname");
            Assert.AreEqual(myContact.GetPropertyValue("Surname"), mySecondContactPerson.GetPropertyValue("Surname"));
        }

        [Test]
        [ExpectedException(typeof (BusObjDuplicateConcurrencyControlException))]
        public void SaveNewObjectWithDuplicatePrimaryKey()
        {
            ContactPersonCompositeKey myContact = new ContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            IPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            myContact.Save(); //save the object to the DB

            //			BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);

            ContactPersonCompositeKey mySecondContactPerson =
                new ContactPersonCompositeKey();
            mySecondContactPerson.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            mySecondContactPerson.SetPropertyValue("FirstName", "Brad");
            mySecondContactPerson.SetPropertyValue("Surname", "Vincent");
            mySecondContactPerson.SetPropertyValue("PK1Prop1", myContact.GetPropertyValue("PK1Prop1"));
            mySecondContactPerson.SetPropertyValue("PK1Prop2", myContact.GetPropertyValue("PK1Prop2"));
            mySecondContactPerson.Save(); //save the object to the DB
        }

        [Test]
        [ExpectedException(typeof (BusObjDeleteConcurrencyControlException))]
        public void TestDeleteContactPerson()
        {
            ContactPersonCompositeKey mySecondContactPerson =
                ContactPersonCompositeKey.GetContactPersonCompositeKey(mContactPDeleted.ID);
        }

        [Test]
        public void TestDeleteFlagsSetContactPerson()
        {
            ContactPersonCompositeKey myContact = new ContactPersonCompositeKey();
            Assert.IsTrue(myContact.Status.IsNew); // this object is new
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());

            myContact.Save(); //save the object to the DB
            Assert.IsFalse(myContact.Status.IsNew); // this object is saved and thus no longer
            // new
            Assert.IsFalse(myContact.Status.IsDeleted);

            IPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);
            //Put a loop in to take up some time due to MSAccess 
            myContact.Delete();
            Assert.IsTrue(myContact.Status.IsDeleted);
            myContact.Save();
            Assert.IsTrue(myContact.Status.IsDeleted);
            Assert.IsTrue(myContact.Status.IsNew);
        }

        [Test]
        public void TestSaveContactPerson()
        {
            //-----------------------------Setup Test Pack-----------------------------
            ContactPersonCompositeKey contactPTestSave = new ContactPersonCompositeKey();
            contactPTestSave.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            contactPTestSave.SetPropertyValue("FirstName", "Brad");
            contactPTestSave.SetPropertyValue("Surname", "Vincent");
            contactPTestSave.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            contactPTestSave.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            contactPTestSave.Save(); //save the object to the DB


            BusinessObjectManager.Instance.ClearLoadedObjects();
            WaitForGC();

            //---------------------------Assert Precondition --------------------------
            Assert.IsFalse(contactPTestSave.Status.IsNew); // this object is saved and thus no longer
            // new

            IPrimaryKey id = contactPTestSave.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, contactPTestSave.ID);

            //--------------------------Execute ---------------------------------------
            ContactPersonCompositeKey secondContactPerson = ContactPersonCompositeKey.GetContactPersonCompositeKey(id);

            //-------------------------Assert Result ----------------------------------
            Assert.AreNotSame(contactPTestSave, secondContactPerson);

            Assert.IsFalse(secondContactPerson.Status.IsNew); // this object is recovered from the DB
            // and is thus not new.
//            Assert.AreEqual(contactPTestSave.ID.ToString().ToUpper(), secondContactPerson.ID.ToString());
            Assert.AreEqual(contactPTestSave.GetPropertyValue("FirstName"),
                            secondContactPerson.GetPropertyValue("FirstName"));
            Assert.AreEqual(contactPTestSave.GetPropertyValue("DateOfBirth"),
                            secondContactPerson.GetPropertyValue("DateOfBirth"));

            //For some reason doing this here old tests
            contactPTestSave.SetPropertyValue("FirstName", "Change FirstName");
            Assert.IsFalse(contactPTestSave.GetPropertyValue("FirstName") ==
                           secondContactPerson.GetPropertyValue("FirstName"));
        }


        [Test]
        public void TestUpdateExistingContactPerson()
        {
            ContactPersonCompositeKey myContactPerson =
                ContactPersonCompositeKey.GetContactPersonCompositeKey(updateContactPersonID);
            myContactPerson.SetPropertyValue("FirstName", "NewFirstName");
            myContactPerson.Save();

            ContactPersonCompositeKey.ClearContactPersonCol();
            //Reload the person and make sure that the changes have been made.
            ContactPersonCompositeKey myNewContactPerson =
                ContactPersonCompositeKey.GetContactPersonCompositeKey(updateContactPersonID);
            Assert.AreEqual("NewFirstName", myNewContactPerson.GetPropertyValue("FirstName"),
                            "The firstName was not updated");
        }
    }
}