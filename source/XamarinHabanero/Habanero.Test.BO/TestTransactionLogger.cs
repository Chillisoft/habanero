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
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Util;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestTransactionLogger : TestUsingDatabase
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var asm = Assembly.GetExecutingAssembly();
            ConfigurationManager.Initialise(asm);

            BORegistry.DataAccessor = new DataAccessorInMemory();
            ClassDef.ClassDefs.Clear();
            ContactPersonTransactionLogging.LoadDefaultClassDef();
            TransactionLogBusObj.LoadClassDef();
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        [Test]
        public void TestTransactionLogTransactionNotAddedToTransactionCommitter()
        {
            //---------------Set up test pack-------------------
            //Create Mock Business object that implements a stub transaction log.
            ContactPersonTransactionLogging cp = CreateUnsavedContactPersonTransactionLogging();
            TransactionCommitterStub tc = new TransactionCommitterStub();
            tc.AddBusinessObject(cp);
            Assert.AreEqual(1, tc.OriginalTransactions.Count);
            //---------------Execute Test ----------------------
            //call persist on the object
            tc.CommitTransaction();
            //---------------Test Result -----------------------
            //check that the transaction committer has 1 object
            // i.e. the transaction log object was not added to the transaction.
            Assert.AreEqual(1, tc.ExecutedTransactions.Count);
        }

      


        private static ContactPersonTransactionLogging CreateUnsavedContactPersonTransactionLogging()
        {
            ContactPersonTransactionLogging cp = new ContactPersonTransactionLogging();
            cp.Surname = Guid.NewGuid().ToString();
            return cp;
        }


        [Test]
        public void TestAcceptanceTransactionLog_DB_NewContactPerson()
        {
            //Test that the transaction log 
            //---------------Set up test pack-------------------
            ContactPersonTransactionLogging cp = CreateUnsavedContactPersonTransactionLogging();
            TransactionCommitterDB tc = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            tc.AddBusinessObject(cp);
            string dirtyXML = cp.DirtyXML;
            //---------------Execute Test ----------------------
            tc.CommitTransaction();
            //---------------Test Result -----------------------
            //Test that a transaction Log was created with
            BusinessObjectCollection<TransactionLogBusObj> colTransactions =
                new BusinessObjectCollection<TransactionLogBusObj>();
            colTransactions.LoadAll("TransactionSequenceNo");

            //CRUD = Insert and Dirty XML all properties in DirtyXML.
            Assert.IsTrue(colTransactions.Count > 0);
            TransactionLogBusObj trLog = colTransactions[colTransactions.Count - 1];
            Assert.AreEqual("Created", trLog.CrudAction);
            Assert.AreEqual(dirtyXML, trLog.DirtyXMLLog);
            Assert.AreEqual("ContactPersonTransactionLogging", trLog.BusinessObjectTypeName);
            //Assert.AreEqual(WindowsIdentity.GetCurrent().Name, trLog.WindowsUser);
            Assert.AreEqual(Environment.MachineName, trLog.MachineUpdatedName);
            //Assert.GreaterOrEqual(trLog.DateTimeUpdated, DateTime.Now.AddMinutes(-1));
            Assert.LessOrEqual(trLog.DateTimeUpdated, DateTime.Now.AddSeconds(1));
            Assert.AreEqual(cp.ToString(),trLog.BusinessObjectToString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAcceptanceTransactionLog_DB_EditContactPerson()
        {
            //Test that the transaction log 
            //---------------Set up test pack-------------------
            ContactPersonTransactionLogging cp = CreateUnsavedContactPersonTransactionLogging();
            TransactionCommitterDB tc = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            tc.AddBusinessObject(cp);
            tc.CommitTransaction();
            tc = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            cp.Surname = Guid.NewGuid().ToString();
            tc.AddBusinessObject(cp);
            //---------------Execute Test ----------------------
            tc.CommitTransaction();
            //---------------Test Result -----------------------
            //Test that a transaction Log was created with
            BusinessObjectCollection<TransactionLogBusObj> colTransactions =
                new BusinessObjectCollection<TransactionLogBusObj>();
            colTransactions.LoadAll("TransactionSequenceNo");

            //CRUD = Insert and Dirty XML all properties in DirtyXML.
            Assert.IsTrue(colTransactions.Count > 0);
            TransactionLogBusObj trLog = colTransactions[colTransactions.Count - 1];
            //CRUD = Edited
            Assert.AreEqual("Updated", trLog.CrudAction);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAcceptanceTransactionLog_DB_DeleteContactPerson()
        {
            //---------------Set up test pack-------------------
            ContactPersonTransactionLogging cp = CreateUnsavedContactPersonTransactionLogging();
            TransactionCommitterDB tc = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            tc.AddBusinessObject(cp);
            tc.CommitTransaction();
            cp.MarkForDelete();
            tc = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            tc.AddBusinessObject(cp);
            //---------------Execute Test ----------------------
            tc.CommitTransaction();
            //---------------Test Result -----------------------
            //Test that a transaction Log was created with
            var colTransactions =
                new BusinessObjectCollection<TransactionLogBusObj>();
            colTransactions.LoadAll("TransactionSequenceNo");

            //CRUD = Insert and Dirty XML all properties in DirtyXML.
            Assert.IsTrue(colTransactions.Count > 0);
            var trLog = colTransactions[colTransactions.Count - 1];

            //CRUD = Deleted
            Assert.AreEqual("Deleted", trLog.CrudAction);

            //---------------Tear Down -------------------------          
        }

//        private static ContactPersonTransactionLogging CreateUnsavedContactPersonTransactionLoggingAltKey()
//        {
//            ContactPersonTransactionLogging cp = new ContactPersonTransactionLogging();
//            cp.Surname = Guid.NewGuid().ToString();
//            return cp;
//        }

        [Test,
         Ignore(
             "TransactionCommitter currently does not check for duplicate alternative keys for new business objects in the transaction."
             )]
        public void TestAcceptanceTransactionLog_DuplicateAlternativeKeyEntries()
        {
            //---------------Cleanup databse ------------------
            CleanDB();
            ClassDef.ClassDefs.Clear();
            ContactPersonTransactionLogging.LoadClassDef_SurnameAlternateKey();
            //---------------Set up test pack-------------------
            ContactPersonTransactionLogging cp1 = CreateUnsavedContactPersonTransactionLogging();
            const string AltSurname = "TestAltKey";
            cp1.Surname = AltSurname;
            ContactPersonTransactionLogging cp2 = CreateUnsavedContactPersonTransactionLogging();
            cp2.Surname = AltSurname;

            //---------------Execute Test ----------------------
            TransactionCommitterDB tc = new TransactionCommitterDB(DatabaseConnection.CurrentConnection);
            tc.AddBusinessObject(cp1);
            tc.AddBusinessObject(cp2);
            try
            {
                tc.CommitTransaction();
                Assert.Fail(
                    "The transaction should not be committed as there are 2 objects in the transaction with the same alternate key");
            }
               
                //---------------Test Result -----------------------
            catch (BusObjDuplicateConcurrencyControlException)
            {
            }

                //---------------Tear Down -------------------------   
            finally
            {
                const string sql = "DELETE FROM Contact_Person where Surname_field = '" + AltSurname + "'";
                DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
            }
        }


        //LoadClassDef_SurnameAlternateKey
        [Test]
        public void TestAcceptanceTransactionLog_SaveMultipleTimes()
        {
            //---------------Cleanup databse ------------------
            CleanDB();
            //---------------Set up test pack-------------------
            ContactPersonTransactionLogging cp = CreateUnsavedContactPersonTransactionLogging();
            cp.Save();
            //---------------Execute Test ----------------------
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            cp.Surname = Guid.NewGuid().ToString();
            cp.Save();
            cp.MarkForDelete();
            cp.Save();
            BusinessObjectCollection<TransactionLogBusObj> colTransactions =
                new BusinessObjectCollection<TransactionLogBusObj>();
            colTransactions.LoadAll();
            //cp = CreateUnsavedContactPersonTransactionLogging();
            //cp.Save();
            //---------------Test Result -----------------------
            //Test that a transaction Log was created with
            Assert.AreEqual(6, colTransactions.Count);


            //---------------Tear Down -------------------------          
        }

        private static void CleanDB()
        {
            string sql = "DELETE FROM transactionlog";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        //Moved from tester class
        [Test]
        public void TestDirtyXml()
        {
            ContactPersonTransactionLogging myContact_1 = new ContactPersonTransactionLogging();
            //Edit first object and save
            myContact_1.Surname = "My Surname 1";
            myContact_1.Save(); //

            myContact_1.Surname = "My Surname New";

            Assert.AreEqual(
                "<ContactPersonTransactionLogging ID='" + myContact_1.ID +
                "'><Properties><Surname><PreviousValue>My Surname 1</PreviousValue><NewValue>My Surname New</NewValue></Surname></Properties></ContactPersonTransactionLogging>",
                myContact_1.DirtyXML);
        }
    }

    public class ContactPersonTransactionLogging : BusinessObject
    {
        public ContactPersonTransactionLogging()
        {
            SetTransactionLog(new TransactionLogTable(this));
        }

        public static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTransactionLogging"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            LoadTransactionLogClassDef();
            return itsClassDef;
        }

        private static void LoadTransactionLogClassDef()
        {
//            XmlClassLoader itsLoader = new XmlClassLoader();
//            ClassDef itsClassDef =
//                itsLoader.LoadClass(
//                    @"
//               <class name=""TransactionLogBusObj"" assembly=""Habanero.Test.BO"" table=""transactionlog"">
//					<property  name=""TransactionSequenceNo"" type=""Int32"" autoIncrementing=""true"" />
//					<property  name=""DateTimeUpdated"" type=""DateTime"" />
//					<property  name=""WindowsUser""/>
//					<property  name=""LogonUser"" />
//					<property  name=""MachineUpdatedName"" databaseField=""MachineName""/>
//					<property  name=""BusinessObjectTypeName"" />
//                    <property  name=""BusinessObjectToString""/>
//					<property  name=""CRUDAction"" />
//					<property  name=""DirtyXMLLog"" databaseField=""DirtyXML""/>
//					<primaryKey isObjectID=""false"">
//						<prop name=""TransactionSequenceNo"" />
//					</primaryKey>
//			    </class>
//			");
//            ClassDef.ClassDefs.Add(itsClassDef);
            return;
        }

        public static IClassDef LoadClassDef_SurnameAlternateKey()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTransactionLogging"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <key name=""SurnameKey"">
                      <prop name=""Surname"" />
                    </key>
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            LoadTransactionLogClassDef();
            return itsClassDef;
        }

        public Guid ContactPersonID
        {
            get { return (Guid) GetPropertyValue("ContactPersonID"); }
            set { this.SetPropertyValue("ContactPersonID", value); }
        }

        public string Surname
        {
            get { return (string) GetPropertyValue("Surname"); }
            set { SetPropertyValue("Surname", value); }
        }

        public override string ToString()
        {
            return Surname;
        }
    }
}