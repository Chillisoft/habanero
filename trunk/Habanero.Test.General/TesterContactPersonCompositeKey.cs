using System;
using Habanero.Bo;
using Habanero.Db;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for ContactPersonCompositeKeyTester.
    /// </summary>
    [TestFixture]
    public class TesterContactPersonCompositeKey : TestUsingDatabase
    {
        private ContactPersonCompositeKey mContactPTestSave;
        private ContactPersonCompositeKey mContactPDeleted;
        private BOPrimaryKey updateContactPersonID;

        public static void RunTest()
        {
            TesterContactPersonCompositeKey test = new TesterContactPersonCompositeKey();
            test.TestFixtureSetup();
            //			test.TestSaveContactPerson();
            //			test.ModifyObjectsPrimaryKey();
            test.TestUpdateExistingContactPerson();
            //			test.RecoverNewObjectFromObjectManagerBeforeAndAfterPersist();
            test.SaveNewObjectWithDuplicatePrimaryKey();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            this.SetupDBConnection();
            DeleteAllContactPersons();
            CreateUpdatedContactPersonTestPack();
            CreateSaveContactPersonTestPack();
            CreateDeletedPersonTestPack();
        }

        [SetUp]
        public void Setup()
        {
            //Ensure that a fresh object is loaded from DB
            ContactPerson.ClearContactPersonCol();
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
            mContactPTestSave = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            mContactPTestSave.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            mContactPTestSave.SetPropertyValue("FirstName", "Brad");
            mContactPTestSave.SetPropertyValue("Surname", "Vincent");
            mContactPTestSave.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            mContactPTestSave.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            mContactPTestSave.ApplyEdit(); //save the object to the DB
        }

        private void CreateDeletedPersonTestPack()
        {
            ContactPersonCompositeKey myContact = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            myContact.ApplyEdit(); //save the object to the DB
            myContact.Delete();
            myContact.ApplyEdit();

            mContactPDeleted = myContact;
        }

        private void CreateUpdatedContactPersonTestPack()
        {
            ContactPersonCompositeKey myContact = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1969, 01, 29));
            myContact.SetPropertyValue("FirstName", "FirstName");
            myContact.SetPropertyValue("Surname", "Surname");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            myContact.ApplyEdit();
            updateContactPersonID = myContact.ID;
        }

        [Test]
        public void TestSaveContactPerson()
        {
            Assert.IsFalse(mContactPTestSave.IsNew); // this object is saved and thus no longer
            // new

            BOPrimaryKey id = mContactPTestSave.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, mContactPTestSave.ID);

            ContactPersonCompositeKey mySecondContactPerson = ContactPersonCompositeKey.GetContactPersonCompositeKey(id);
            Assert.IsFalse(mContactPTestSave.IsNew); // this object is recovered from the DB
            // and is thus not new.
            Assert.AreEqual(mContactPTestSave.ID.ToString(), mySecondContactPerson.ID.ToString());
            Assert.AreEqual(mContactPTestSave.GetPropertyValue("FirstName"),
                            mySecondContactPerson.GetPropertyValue("FirstName"));
            Assert.AreEqual(mContactPTestSave.GetPropertyValue("DateOfBirth"),
                            mySecondContactPerson.GetPropertyValue("DateOfBirth"));

            //Add test to make certain that myContact person and contact person are not 
            // pointing at the same physical object

            mContactPTestSave.SetPropertyValue("FirstName", "Change FirstName");
            Assert.IsFalse(mContactPTestSave.GetPropertyValue("FirstName") ==
                           mySecondContactPerson.GetPropertyValue("FirstName"));
        }

        [Test]
        public void TestUpdateExistingContactPerson()
        {
            ContactPersonCompositeKey myContactPerson =
                ContactPersonCompositeKey.GetContactPersonCompositeKey(updateContactPersonID);
            myContactPerson.SetPropertyValue("FirstName", "NewFirstName");
            myContactPerson.ApplyEdit();

            ContactPersonCompositeKey.ClearContactPersonCol();
            //Reload the person and make sure that the changes have been made.
            ContactPersonCompositeKey myNewContactPerson =
                ContactPersonCompositeKey.GetContactPersonCompositeKey(updateContactPersonID);
            Assert.AreEqual("NewFirstName", myNewContactPerson.GetPropertyValue("FirstName"),
                            "The firstName was not updated");
        }

        [Test]
        public void TestDeleteFlagsSetContactPerson()
        {
            ContactPersonCompositeKey myContact = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            Assert.IsTrue(myContact.IsNew); // this object is new
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());

            myContact.ApplyEdit(); //save the object to the DB
            Assert.IsFalse(myContact.IsNew); // this object is saved and thus no longer
            // new
            Assert.IsFalse(myContact.IsDeleted);

            BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);
            //Put a loop in to take up some time due to MSAccess 
            myContact.Delete();
            Assert.IsTrue(myContact.IsDeleted);
            myContact.ApplyEdit();
            Assert.IsTrue(myContact.IsDeleted);
            Assert.IsTrue(myContact.IsNew);
        }

        [Test]
        [ExpectedException(typeof (BusinessObjectNotFoundException))]
        public void TestDeleteContactPerson()
        {
            ContactPersonCompositeKey mySecondContactPerson =
                ContactPersonCompositeKey.GetContactPersonCompositeKey(mContactPDeleted.ID);
        }

        [Test]
        public void RecoverNewObjectFromObjectManagerBeforeAndAfterPersist()
        {
            ContactPersonCompositeKey myContact = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            myContact.ApplyEdit(); //save the object to the DB

            //			BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);

            ContactPersonCompositeKey mySecondContactPerson = ContactPersonCompositeKey.GetContactPersonCompositeKey(id);

            Assert.IsTrue(Object.ReferenceEquals(myContact, mySecondContactPerson));
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
        public void ModifyObjectsPrimaryKey()
        {
            ContactPersonCompositeKey myContact = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            myContact.ApplyEdit(); //save the object to the DB

            //			BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);
            //			System.Console.WriteLine("ID:" + id.GetObjectId());
            //			System.Console.WriteLine("Contact.ID:" + myContact.ID.GetObjectId());
            ContactPersonCompositeKey mySecondContactPerson = ContactPersonCompositeKey.GetContactPersonCompositeKey(id);
            Assert.IsTrue(Object.ReferenceEquals(myContact, mySecondContactPerson));
            Assert.AreEqual(myContact.ID,
                            mySecondContactPerson.ID);
            Assert.AreEqual(myContact.GetPropertyValue("FirstName"), mySecondContactPerson.GetPropertyValue("FirstName"));
            Assert.AreEqual(myContact.GetPropertyValue("DateOfBirth"),
                            mySecondContactPerson.GetPropertyValue("DateOfBirth"));

            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            myContact.ApplyEdit(); //save the object to the DB

            ContactPersonCompositeKey myContactPerson3 = ContactPersonCompositeKey.GetContactPersonCompositeKey(id);
            Assert.IsTrue(Object.ReferenceEquals(myContact, myContactPerson3));
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
        [ExpectedException(typeof (BusObjDuplicateConcurrencyControlException))]
        public void SaveNewObjectWithDuplicatePrimaryKey()
        {
            ContactPersonCompositeKey myContact = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            myContact.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            myContact.SetPropertyValue("FirstName", "Brad");
            myContact.SetPropertyValue("Surname", "Vincent");
            myContact.SetPropertyValue("PK1Prop1", Guid.NewGuid());
            myContact.SetPropertyValue("PK1Prop2", Guid.NewGuid());
            BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            myContact.ApplyEdit(); //save the object to the DB

            //			BOPrimaryKey id = myContact.ID; //Save the objectsID so that it can be loaded from the Database
            Assert.AreEqual(id, myContact.ID);

            ContactPersonCompositeKey mySecondContactPerson =
                ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            mySecondContactPerson.SetPropertyValue("DateOfBirth", new DateTime(1980, 01, 22));
            mySecondContactPerson.SetPropertyValue("FirstName", "Brad");
            mySecondContactPerson.SetPropertyValue("Surname", "Vincent");
            mySecondContactPerson.SetPropertyValue("PK1Prop1", myContact.GetPropertyValue("PK1Prop1"));
            mySecondContactPerson.SetPropertyValue("PK1Prop2", myContact.GetPropertyValue("PK1Prop2"));
            mySecondContactPerson.ApplyEdit(); //save the object to the DB
        }

        [Test]
        public void CreateTwoConsecutiveObjects()
        {
            ContactPersonCompositeKey myContact = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            ContactPersonCompositeKey mySecondContactPerson =
                ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
        }

        [Test]
        public void ChangeObjectPrimaryKeyAndThenCreateNewObjectWithPreviousPrimaryKey()
        {
            ContactPersonCompositeKey myContact = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            myContact.SetPropertyValue("Surname", "Vincent");
            Guid prop_1_ID = Guid.NewGuid();
            Guid prop_2_ID = Guid.NewGuid();
            myContact.SetPropertyValue("PK1Prop1", prop_1_ID.ToString());
            myContact.SetPropertyValue("PK1Prop2", prop_2_ID.ToString());
            myContact.ApplyEdit();
            //modify the primary key
            myContact.SetPropertyValue("PK1Prop1", prop_1_ID.ToString() + "1");
            myContact.SetPropertyValue("PK1Prop1", prop_1_ID.ToString() + "1");
            myContact.ApplyEdit();

            ContactPersonCompositeKey myContactTwo = ContactPersonCompositeKey.GetNewContactPersonCompositeKey();
            myContactTwo.SetPropertyValue("Surname", "Vincent 2");
            myContactTwo.SetPropertyValue("PK1Prop1", prop_1_ID.ToString());
            myContactTwo.SetPropertyValue("PK1Prop2", prop_2_ID.ToString());
            myContactTwo.ApplyEdit();
        }
    }
}