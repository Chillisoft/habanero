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
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestNumberGenerator : TestUsingDatabase
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            BusinessObjectManager.Instance.ClearLoadedObjects();
            BORegistry.DataAccessor = new DataAccessorDB();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        [Test]
        public void TestAcceptance_GenerateFirstNumber_SeedZero()
        {
            //---------------Clean Up --------------------------
            //Create an entry in the number generator table for entry type to seed.
            BOSequenceNumber.LoadNumberGenClassDef();
            string numberType = "tmp";
            SetNumberGeneratorSeedZero(numberType);
            //---------------Set up test pack-------------------
            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            INumberGenerator numGen = new NumberGenerator("tmp");

            //---------------Execute Test ----------------------
            //get the next number for invoice number
            int nextNum = numGen.NextNumber();

            //---------------Test Result -----------------------
            //test nextnumber should be one.
            Assert.AreEqual(1, nextNum);
            //---------------Tear Down -------------------------  
        }

        [Test]
        public void TestAcceptance_GenerateFirstNumber_SeedZero_WithNoLoadedClassDefs()
        {
            //---------------Clean Up --------------------------
            //Create an entry in the number generator table for entry type to seed.
            string numberType = "tmp";
            SetNumberGeneratorSeedZero(numberType);
            //---------------Set up test pack-------------------
            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            INumberGenerator numGen = new NumberGenerator("tmp");

            //---------------Execute Test ----------------------
            //get the next number for invoice number
            int nextNum = numGen.NextNumber();

            //---------------Test Result -----------------------
            //test nextnumber should be one.
            Assert.AreEqual(1, nextNum);
            //---------------Tear Down -------------------------  
        }
        [Test]
        public void TestAcceptance_GenerateFirstNumber_NoSeed()
        {
            //Delete entry from database for the number type.
            BOSequenceNumber.LoadNumberGenClassDef();
            BOSequenceNumber.DeleteAllNumbers();
            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            //---------------Set up test pack-------------------
            INumberGenerator numGen = new NumberGenerator("tmp");
            //---------------Execute Test ----------------------
            //get the next number for invoice number
            int sequenceNumber = numGen.NextNumber();
            //---------------Test Result -----------------------
            //test number should be one.
            Assert.AreEqual(1, sequenceNumber);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAcceptance_GenerateFirstNumber_SeedNonZero()
        {
            //---------------Set up test pack-------------------
            //Create an entry in the number generator table for entry type to seed with seed = 100.
            BOSequenceNumber.LoadNumberGenClassDef();
            INumberGenerator numGen = new NumberGenerator("tmp");
            int seedNumber = 100;
            numGen.SetSequenceNumber(seedNumber);
            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            numGen = new NumberGenerator("tmp");
            //---------------Execute Test ----------------------
            //get the next number for invoice number
            int nextNumber = numGen.NextNumber();
            //---------------Test Result -----------------------
            //test number should be 101.
            Assert.AreEqual(seedNumber + 1, nextNumber);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAcceptance_GenerateSecondNumber_IncrementFirstNumber()
        {
            //---------------Set up test pack-------------------
            //Create an entry in the number generator table for entry type to seed with seed = 0.
            string numberType = "tmp";
            BOSequenceNumber.LoadNumberGenClassDef();
            SetNumberGeneratorSeedZero(numberType);
            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            INumberGenerator numGen = new NumberGenerator(numberType);
            //get the next number for invoice number
            numGen.NextNumber();
            //update to datasource

            //---------------Execute Test ----------------------
            //Get second number
            int nextNum = numGen.NextNumber();

            //---------------Test Result -----------------------
            //test number should be 2.
            Assert.AreEqual(2, nextNum);
        }

        [Test]
        public void TestAcceptance_LockNumberGenerator()
        {
            //---------------Set up test pack-------------------
            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            const string numberType = "tmp";
            BOSequenceNumberLocking.LoadNumberGenClassDef();
            INumberGenerator numGen = new NumberGeneratorPessimisticLocking(numberType);
            numGen.SetSequenceNumber(0);
            //get the next number for invoice number
            numGen.NextNumber();
            //Clear all loaded objects from object manager
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Execute Test ----------------------
            //Create a seperate instance of the number generator (simulating a simultaneous user).
            INumberGenerator numGen2 = new NumberGeneratorPessimisticLocking(numberType);
            //try Get second number
            try
            {
                numGen2.NextNumber();
                Assert.Fail("Should not b able to get second number since locked");
            }
            //---------------Test Result -----------------------
            //should get locking error
            catch(BusObjPessimisticConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("You cannot begin edits on the 'BOSequenceNumberLocking', as another user has started edits and therefore locked to this record"));
            }
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAcceptance_LockNumberGenerator_ClearsAfter15Minutes()
        {
            //---------------Set up test pack-------------------
            //Create an entry in the number generator table for entry type to seed with seed = 0 and lockduration = 15 minutes.

            const string numberType = "tmp";
            BOSequenceNumberLocking.LoadNumberGenClassDef();
            BOSequenceNumberLocking.DeleteAllNumbers();
            INumberGenerator numGen = new NumberGeneratorPessimisticLocking(numberType);
            numGen.SetSequenceNumber(0);
            //get the next number for invoice number
            int num = numGen.NextNumber();
            Assert.AreEqual(1, num,"The first generated number should be 1");
            // set the datetime locked to > 15 minutes ago.
            UpdateDatabaseLockAsExpired(15);
            BusinessObjectManager.Instance.ClearLoadedObjects();
            //---------------Execute Test ----------------------
            //Create a seperate instance of the number generator.
            //try Get  number
            INumberGenerator numGen2 = new NumberGeneratorPessimisticLocking(numberType);
            //try Get second number
            num = numGen2.NextNumber();
            //---------------Test Result -----------------------
            Assert.AreNotSame(numGen, numGen2);
            //should not get locking error
            //assert nextnumber = 1
            Assert.AreEqual(1, num, "The second generated number should be 1");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAcceptance_NumberGeneratorCreatedInBeforeUpdateForAnObject()
        {
            //---------------Clean Up --------------------------
            CleanUpContactPersonNumberGenerator();
            //---------------Set up test pack-------------------
            //Create an object that sets the number generator for it.
            //Edit the object.
            ContactPersonNumberGenerator cp = new ContactPersonNumberGenerator();
            cp.Surname = Guid.NewGuid().ToString();
            //---------------Execute Test ----------------------
            //save the object
            cp.Save();
            //---------------Test Result -----------------------
            //check that the object has its number set to the appropriate value.
            Assert.AreEqual("1", cp.GeneratedNumber);
            //---------------Tear Down -------------------------        
        }

        [Test]
        public void TestAcceptance_NumberGeneratorCreatedInBeforeUpdateForAnObject_2ObjectsSavedOneAfterTheOther()
        {
            //---------------Clean Up --------------------------
            CleanUpContactPersonNumberGenerator();
            //---------------Set up test pack-------------------
            //Create an object that sets the number generator for it.
            //Edit the object.
            ContactPersonNumberGenerator cp = new ContactPersonNumberGenerator();
            cp.Surname = Guid.NewGuid().ToString();

            //---------------Execute Test ----------------------
            cp.Save();            
            ContactPersonNumberGenerator cp2 = new ContactPersonNumberGenerator();
            cp2.Surname = Guid.NewGuid().ToString();
            cp2.Save();
            //---------------Test Result -----------------------
            //check that the object has its number set to the appropriate value.
            Assert.AreEqual("2", cp2.GeneratedNumber);
            //---------------Tear Down -------------------------        
        }

        private static void CleanUpContactPersonNumberGenerator()
        {
            ContactPersonNumberGenerator.LoadDefaultClassDef();
            BOSequenceNumber.LoadNumberGenClassDef();
            ContactPersonNumberGenerator.DeleteAllContactPeople();
            INumberGenerator numGen = new NumberGenerator("GeneratedNumber");
            numGen.SetSequenceNumber(0);
        }


        [Test]
        public void TestAcceptance_NumberGeneratorCreatedInBeforeUpdateForAnObject_2ObjectsInTransaction()
        {
            //---------------Set up test pack-------------------
            //Create an object that sets the number generator for it.
            CleanUpContactPersonNumberGenerator();
            //---------------Set up test pack-------------------
            //Create an objects that sets the number generator for it.
            //Edit the objects.
            ContactPersonNumberGenerator cp = new ContactPersonNumberGenerator();
            cp.Surname = Guid.NewGuid().ToString();
            ContactPersonNumberGenerator cp2 = new ContactPersonNumberGenerator();
            cp2.Surname = Guid.NewGuid().ToString();
            //---------------Execute Test ----------------------
            //Add the objects 
            TransactionCommitterStubDB trnCommit = new TransactionCommitterStubDB();
            trnCommit.AddBusinessObject(cp);
            trnCommit.AddBusinessObject(cp2);
            trnCommit.CommitTransaction();

            //---------------Test Result -----------------------
            //check that the objects have its number set to the appropriate value.
            Assert.AreEqual("1", cp.GeneratedNumber); 
            Assert.AreEqual("2", cp2.GeneratedNumber);
            //---------------Tear Down -------------------------        
        }

        [Test]
        public void TestGetSecondNumber_FromSeperateNumberGeneratorInstance()
        {
            //---------------Clean Up --------------------------
            CleanupNumberGenerator();
            INumberGenerator numGen;
            //---------------Set up test pack-------------------
            numGen = new NumberGenerator("tmp");
            //---------------Execute Test ----------------------
            numGen.NextNumber();
            numGen = new NumberGenerator("tmp");
            int nextNum = numGen.NextNumber();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, nextNum);
            //---------------Tear Down   -----------------------
        }

        private static void CleanupNumberGenerator()
        {
            BOSequenceNumber.LoadNumberGenClassDef();
            INumberGenerator numGen = new NumberGenerator("tmp");
            numGen.SetSequenceNumber(0);
        }

        [Test]
        public void TestGetSecondNumber_FromSeperateNumberGeneratorInstance_AfterUpdate()
        {
            //---------------Set up test pack-------------------
            CleanupNumberGenerator();
            NumberGenerator numGen = new NumberGenerator("tmp");
            //---------------Execute Test ----------------------
            numGen.NextNumber();
            numGen.Save();
            numGen = new NumberGenerator("tmp");
            int nextNum = numGen.NextNumber();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, nextNum);
            //---------------Tear Down   -----------------------
        }

        [Test]
        public void TestSetSequenceNumber()
        {
            //---------------Set up test pack-------------------
            BOSequenceNumber.LoadNumberGenClassDef();
            NumberGenerator numGen = new NumberGenerator("tmp");
            numGen.NextNumber();
            numGen.Save();
            Assert.GreaterOrEqual( numGen.NextNumber(),1);
            //---------------Execute Test ----------------------
            numGen.SetSequenceNumber(0);
            //---------------Test Result -----------------------
            numGen = new NumberGenerator("tmp");
            Assert.AreEqual(1, numGen.NextNumber());
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDifferentTableName()
        {
            //---------------Set up test pack-------------------
            //Delete entry from database for the number type.
            BOSequenceNumber.LoadNumberGenClassDef("another_number_generator");
            BOSequenceNumber.DeleteAllNumbers();
            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            //---------------Set up test pack-------------------
            INumberGenerator numGen = new NumberGenerator("tmp", "another_number_generator");
            //---------------Execute Test ----------------------
            //get the next number for invoice number
            int sequenceNumber = numGen.NextNumber();
            //---------------Test Result -----------------------
            //test number should be one.
            Assert.AreEqual(1, sequenceNumber);
            //---------------Tear Down -------------------------
        }


        [Test]
        public void TestDifferentTableNameGetSecondNumber_FromSeperateNumberGeneratorInstance_AfterUpdate()
        {
            //---------------Clean Up --------------------------
            BOSequenceNumber.LoadNumberGenClassDef("another_number_generator");
            NumberGenerator numGen = new NumberGenerator("tmp", "another_number_generator");
            numGen.SetSequenceNumber(0);
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            numGen.NextNumber();
            numGen.Save();
            numGen = new NumberGenerator("tmp", "another_number_generator");
            int nextNum = numGen.NextNumber();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, nextNum);

        }

        private static void SetNumberGeneratorSeedZero(string numberType)
        {
            INumberGenerator numGen = new NumberGenerator(numberType);
            numGen.SetSequenceNumber(0);
        }
        private static void UpdateDatabaseLockAsExpired(int lockDuration)
        {
            SqlStatement sqlStatement = new SqlStatement(DatabaseConnection.CurrentConnection);
            sqlStatement.Statement.Append("UPDATE `numbergenerator` SET ");
            sqlStatement.Statement.Append(SqlFormattingHelper.FormatFieldName("DateTimeLocked", DatabaseConnection.CurrentConnection));
            sqlStatement.Statement.Append(" = ");
            sqlStatement.AddParameterToStatement(DateTime.Now.AddMinutes(-1 * lockDuration - 1));
            DatabaseConnection.CurrentConnection.ExecuteSql(sqlStatement);
        }
    }

    internal class ContactPersonNumberGenerator : BusinessObject
    {
        //public ContactPersonNumberGenerator()
        //{
            
        //}


        internal static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonNumberGenerator"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
					<property  name=""GeneratedNumber"" compulsory=""true"" databaseField=""PK3_Prop""/>
                    <key>
                        <prop name=""GeneratedNumber"" /> 
                    </key>
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

        public string GeneratedNumber
        {
            get { return (string)GetPropertyValue("GeneratedNumber"); }
            set { SetPropertyValue("GeneratedNumber", value); }
        }

        public override string ToString()
        {
            return Surname;
        }

        ///<summary>
        /// Executes any custom code required by the business object before it is persisted to the database.
        /// This has the additionl capability of creating or updating other business objects and adding these
        /// to the transaction committer.
        /// <remarks> Recursive call to UpdateObjectBeforePersisting will not be done i.e. it is the bo developers responsibility to implement</remarks>
        ///</summary>
        ///<param name="transactionCommitter">the transaction committer that is executing the transaction</param>
        protected internal override void UpdateObjectBeforePersisting(ITransactionCommitter transactionCommitter)
        {
            base.UpdateObjectBeforePersisting(transactionCommitter);
            INumberGenerator numGen = new NumberGenerator("GeneratedNumber");
            this.GeneratedNumber = numGen.NextNumber().ToString();
            numGen.AddToTransaction(transactionCommitter);
        }

        public static void DeleteAllContactPeople()
        {
            DatabaseConnection.CurrentConnection.ExecuteRawSql("Delete From contact_person");
        }
    }
}