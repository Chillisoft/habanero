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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;
using Habanero.BO.Loaders;
using Habanero.DB;
using Habanero.DB.ConcurrencyControl;
using NUnit.Framework;

namespace Habanero.Test.DB.ConcurrencyControl
{
    [TestFixture]
    public class TestConcurrencyControl_OptimisticLockingVersionNumberDB : TestUsingDatabase
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
            //Runs every time that any testmethod is executed
            //base.SetupTest();
            FixtureEnvironment.ResetBORegistryBusinessObjectManager();
            BORegistry.DataAccessor = new DataAccessorDB();
        }
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.SetupDBConnection();
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }
        [TearDown]
        public  void TearDownTest()
        {
            //runs every time any testmethod is complete
            //DeleteObjects();
        }
        [Test]
        public void TestLockingVersionNumber_DoesNotCauseProblemsWithMultipleSaves()
        {
            //---------------Set up test pack-------------------
            //Create object in DB

            ContactPersonOptimisticLockingVersionNumberDB.LoadDefaultClassDef();
            ContactPersonOptimisticLockingVersionNumberDB contactPerson = new ContactPersonOptimisticLockingVersionNumberDB();
            contactPerson.Surname = Guid.NewGuid().ToString();
            AddObjectToDelete(contactPerson);
            contactPerson.Save();
            //---------------Execute Test ----------------------
            //Edit first object and persist to the database.
            contactPerson.Surname = Guid.NewGuid().ToString();
            contactPerson.Save();

        }
        [Test]
        public void TestLockingVersionNumber_OnBeginEdit()
        {
            //---------------Set up test pack-------------------
            //Create object in DB

            ContactPersonOptimisticLockingVersionNumberDB.LoadDefaultClassDef();
            ContactPersonOptimisticLockingVersionNumberDB contactPerson = new ContactPersonOptimisticLockingVersionNumberDB
                                                                              {Surname = Guid.NewGuid().ToString()};
            AddObjectToDelete(contactPerson);
            contactPerson.Save();
            //Clear object manager
            FixtureEnvironment.ClearBusinessObjectManager();
            //Load second object from DB
            ContactPersonOptimisticLockingVersionNumberDB duplicateContactPerson =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonOptimisticLockingVersionNumberDB>(contactPerson.ID);
            AddObjectToDelete(duplicateContactPerson);

            //---------------Execute Test ----------------------
            //Edit first object and persist to the database.
            contactPerson.Surname = Guid.NewGuid().ToString();
            contactPerson.Save();
            //Begin Edit on second object
            try
            {
                duplicateContactPerson.FirstName = Guid.NewGuid().ToString();
                Assert.Fail("Should throw error");
            }    
                //---------------Test Result -----------------------
                //Raise Exception that the object has been edited since 
                // the user last edited.
            catch(BusObjBeginEditConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("You cannot Edit 'ContactPersonOptimisticLockingVersionNumberDB', as another user has edited this record"));
            }
        }
        [Test]
        public void TestLockingVersionNumber_OnSave()
        {
            //---------------Set up test pack-------------------
            //Create object in DB

            ContactPersonOptimisticLockingVersionNumberDB contactPerson = CreateSavedCntactPersonOptimisticLockingVersionNumberDB();

            //Clear object manager
            FixtureEnvironment.ClearBusinessObjectManager();
            //Load second object from DB
            ContactPersonOptimisticLockingVersionNumberDB duplicateContactPerson =
                BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonOptimisticLockingVersionNumberDB>(contactPerson.ID);
            AddObjectToDelete(duplicateContactPerson);

            //---------------Execute Test ----------------------
            //Edit first object and persist to the database.
            contactPerson.Surname = Guid.NewGuid().ToString();
            //Begin Edit on second object
            duplicateContactPerson.FirstName = Guid.NewGuid().ToString();

            //Save first object
            contactPerson.Save();
            try
            {
                duplicateContactPerson.Save();
                Assert.Fail();
            }
                //---------------Test Result -----------------------
                //Raise Exception that the object has been edited since 
                // the user last edited.
            catch (BusObjOptimisticConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("You cannot save the changes to 'ContactPersonOptimisticLockingVersionNumberDB', as another user has edited this record"));
            }
        }
        [Test]
        public void TestDeleteObjectPriorToUpdatesConcurrencyControl()
        {
            //----------SETUP TEST PACK--------------------------
            ContactPersonOptimisticLockingVersionNumberDB contactPersonDeleteConcurrency 
                = CreateSavedCntactPersonOptimisticLockingVersionNumberDB();
            //Clear object manager
            FixtureEnvironment.ClearBusinessObjectManager();
            //Load second object from DB            

            ContactPersonOptimisticLockingVersionNumberDB contactPerson2 
                = BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonOptimisticLockingVersionNumberDB>(contactPersonDeleteConcurrency.ID);
            //---------Run TEST ---------------------------------
            contactPersonDeleteConcurrency.MarkForDelete();
            contactPerson2.Surname = "New Surname 2";
            contactPersonDeleteConcurrency.Save();
            try
            {
                contactPerson2.Save();
                Assert.Fail();
            }
                //--------Check Result --------------------------------
            catch (BusObjDeleteConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("You cannot save the changes to 'ContactPersonOptimisticLockingVersionNumberDB', as another user has deleted the record"));
            }
        }
        //Rollback failure must reset concurrency version number.
        [Test]
        public void TestRollBackVersionNumberOnError()
        {
            //---------------Set up test pack-------------------
            //Create object in DB
            ContactPersonOptimisticLockingVersionNumberDB contactPerson = CreateSavedCntactPersonOptimisticLockingVersionNumberDB();

            int versionNumber = contactPerson.VersionNumber;

            //---------------Execute Test ----------------------
            contactPerson.Surname = Guid.NewGuid().ToString();
            Assert.AreEqual(versionNumber, contactPerson.VersionNumber);
            try
            {
                TransactionCommitterStubDB trnCommitter = new TransactionCommitterStubDB(DatabaseConnection.CurrentConnection);
                trnCommitter.AddBusinessObject(contactPerson);
                trnCommitter.AddTransaction(new StubDatabaseFailureTransaction());
                trnCommitter.CommitTransaction();
                Assert.Fail();
            }
                //---------------Test Result -----------------------
            catch (NotImplementedException)
            {
                Assert.AreEqual(versionNumber, contactPerson.VersionNumber);
            }
        }
        private static ContactPersonOptimisticLockingVersionNumberDB CreateSavedCntactPersonOptimisticLockingVersionNumberDB()
        {
            ContactPersonOptimisticLockingVersionNumberDB.LoadDefaultClassDef();
            ContactPersonOptimisticLockingVersionNumberDB contactPersonOptimisticLockingVersionNumberDB;
            contactPersonOptimisticLockingVersionNumberDB = new ContactPersonOptimisticLockingVersionNumberDB();
            contactPersonOptimisticLockingVersionNumberDB.Surname = Guid.NewGuid().ToString();
            contactPersonOptimisticLockingVersionNumberDB.Save();
            AddObjectToDelete(contactPersonOptimisticLockingVersionNumberDB);
            return contactPersonOptimisticLockingVersionNumberDB;
        }
    }

    public class ContactPersonOptimisticLockingVersionNumberDB : BusinessObject
    {
        public ContactPersonOptimisticLockingVersionNumberDB()
        {
            //BOProp propDateLocked = _boPropCol["DateTimeLocked"];
            //BOProp propUserLocked = _boPropCol["UserLocked"];
            //BOProp propMachineLocked = _boPropCol["MachineLocked"];
            //BOProp propOperatingSystemUserLocked = _boPropCol["OperatingSystemUserLocked"];
            //BOProp propLocked = _boPropCol["Locked"];
            //SetConcurrencyControl(new PessimisticLockingDB(this,propDateLocked,
            //                                                           propUserLocked, propMachineLocked,
            //                                                           propOperatingSystemUserLocked,propLocked));
            IBOProp propDateLastUpdated = _boPropCol["DateLastUpdated"];
            IBOProp propUserLastUpdated = _boPropCol["UserLastUpdated"];
            IBOProp propMachineLastUpdated = _boPropCol["MachineLastUpdated"];
            IBOProp propVersionNumber = _boPropCol["VersionNumber"];
            SetConcurrencyControl(new OptimisticLockingVersionNumberDB(this, propDateLastUpdated,
                                                                       propUserLastUpdated, propMachineLastUpdated,
                                                                       propVersionNumber));
        }
        public static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""Habanero.Test.DB.ConcurrencyControl.ContactPersonOptimisticLockingVersionNumberDB"" assembly=""Habanero.Test.DB"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" />
					<property  name=""DateLastUpdated"" type=""DateTime"" />
					<property  name=""UserLastUpdated"" />
					<property  name=""VersionNumber"" type=""Int32""/>
					<property  name=""MachineLastUpdated"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public Guid ContactPersonID
        {
            get { return (Guid)GetPropertyValue("ContactPersonID"); }
            set { this.SetPropertyValue("ContactPersonID", value); }
        }

        public string Surname
        {
            get { return (string)GetPropertyValue("Surname"); }
            set { SetPropertyValue("Surname", value); }
        }
        public string FirstName
        {
            get { return (string)GetPropertyValue("FirstName"); }
            set { SetPropertyValue("FirstName", value); }
        }

        public int VersionNumber
        {
            get { return (int)GetPropertyValue("VersionNumber"); }
            set { SetPropertyValue("VersionNumber", value); }
        }
        public override string ToString()
        {
            return Surname;
        }
    }
}