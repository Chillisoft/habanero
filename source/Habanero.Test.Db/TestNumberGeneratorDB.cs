using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestNumberGeneratorDB : TestUsingDatabase
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
        public void TestAcceptance_LockNumberGenerator()
        {
            //---------------Set up test pack-------------------
            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            const string numberType = "tmp";
            BOSequenceNumberLocking.LoadNumberGenClassDef();
            DatabaseConnection.CurrentConnection.ExecuteRawSql("Delete From numbergenerator");
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
            catch (BusObjPessimisticConcurrencyControlException ex)
            {
                Assert.IsTrue(ex.Message.Contains("You cannot begin edits on the 'BOSequenceNumberLocking', as another user has started edits and therefore locked to this record"));
            }
        }

        [Test]
        public void TestAcceptance_LockNumberGenerator_ClearsAfter15Minutes()
        {
            //---------------Set up test pack-------------------
            //Create an entry in the number generator table for entry type to seed with seed = 0 and lockduration = 15 minutes.
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
            const string numberType = "tmp";
            BOSequenceNumberLocking.LoadNumberGenClassDef();
            DatabaseConnection.CurrentConnection.ExecuteRawSql("Delete From numbergenerator");
            NumberGeneratorPessimisticLocking numGen = new NumberGeneratorPessimisticLocking(numberType);
            numGen.SetSequenceNumber(0);

            //get the next number for invoice number
            int num = numGen.NextNumber();
            BOSequenceNumberLocking boSequenceNumber1 = numGen.BoSequenceNumber;
            Assert.AreEqual(1, num, "The first generated number should be 1");
            // set the datetime locked to > 15 minutes ago.
            UpdateDatabaseLockAsExpired(20);
            BusinessObjectManager.Instance.ClearLoadedObjects();
            TestUtil.WaitForGC();
            //---------------Execute Test ----------------------
            //Create a seperate instance of the number generator.
            //try Get  number
            NumberGeneratorPessimisticLocking numGen2 = new NumberGeneratorPessimisticLocking(numberType);
            //try Get second number
            num = numGen2.NextNumber();
            BOSequenceNumberLocking boSequenceNumber2 = numGen2.BoSequenceNumber;
            //---------------Test Result -----------------------
            Assert.AreNotSame(numGen, numGen2);
            Assert.AreNotSame(boSequenceNumber2, boSequenceNumber1);
            //should not get locking error
            //assert nextnumber = 1
            Assert.AreEqual(1, num, "The second generated number should be 1. Time: " + DateTime.Now.ToLongTimeString());
        }


        [Test]
        public void TestAcceptance_NumberGeneratorCreatedInBeforeUpdateForAnObject_2ObjectsInTransaction()
        {
            //---------------Set up test pack-------------------
            //Create an object that sets the number generator for it.
            //            CleanUpContactPersonNumberGenerator();
            BORegistry.DataAccessor = new DataAccessorInMemory();
            TestNumberGenerator.CleanUpContactPersonNumberGenerator_ForInMemory();
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
}
