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
using System.Threading;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestINumberGenerator_AgainstDB : TestUsingDatabase
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            ClassDef.ClassDefs.Clear();
            FixtureEnvironment.SetupNewIsolatedBusinessObjectManager();
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
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();
            string numberType = RandomValueGen.GetRandomString();
            BOSequenceNumberLocking.LoadNumberGenClassDef();
            //DatabaseConnection.CurrentConnection.ExecuteRawSql("Delete From numbergenerator");
            INumberGenerator numGen = new NumberGeneratorPessimisticLocking(numberType);
            numGen.SetSequenceNumber(0);
            Thread.Sleep(1); // ensure that the new time is higher. just here to check if this resolves a sporadically failing test.
            //get the next number for invoice number
            numGen.NextNumber();
            //Clear all loaded objects from object manager
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();
            Thread.Sleep(1); // ensure that the new time is higher. just here to check if this resolves a sporadically failing test.

            //---------------Execute Test ----------------------
            //Create a seperate instance of the number generator (simulating a simultaneous user).
            INumberGenerator numGen2 = new NumberGeneratorPessimisticLocking(numberType);
            Assert.AreNotSame(numGen, numGen2);
            //try Get second number
            try
            {
                numGen2.NextNumber();
                Assert.Fail("Should not be able to get second number since locked");
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
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();
            TestUtil.WaitForGC();
            string numberType = RandomValueGen.GetRandomString();
            BOSequenceNumberLocking.LoadNumberGenClassDef();
            //DatabaseConnection.CurrentConnection.ExecuteRawSql("Delete From numbergenerator");
            var numGen = new NumberGeneratorPessimisticLocking(numberType);
            numGen.SetSequenceNumber(0);

            //get the next number for invoice number
            long num = numGen.NextNumber();
            BOSequenceNumberLocking boSequenceNumber1 = numGen.BoSequenceNumber;
            Assert.AreEqual(1, num, "The first generated number should be 1");
            // set the datetime locked to > 15 minutes ago.
            UpdateDatabaseLockAsExpired(numberType, 20);
            BORegistry.BusinessObjectManager = new BusinessObjectManagerSpy();
            //---------------Execute Test ----------------------
            //Create a seperate instance of the number generator.
            //try Get  number
            var numGen2 = new NumberGeneratorPessimisticLocking(numberType);
            Assert.AreNotSame(numGen, numGen2);
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
            var cp = new ContactPersonWithNumberGenerator();
            cp.Surname = Guid.NewGuid().ToString();
            var cp2 = new ContactPersonWithNumberGenerator();
            cp2.Surname = Guid.NewGuid().ToString();
            //---------------Execute Test ----------------------
            //Add the objects 
            TransactionCommitterStubDB trnCommit = new TransactionCommitterStubDB(DatabaseConnection.CurrentConnection);
            trnCommit.AddBusinessObject(cp);
            trnCommit.AddBusinessObject(cp2);
            trnCommit.CommitTransaction();

            //---------------Test Result -----------------------
            //check that the objects have its number set to the appropriate value.
            Assert.AreEqual("1", cp.GeneratedNumber);
            Assert.AreEqual("2", cp2.GeneratedNumber);
            //---------------Tear Down -------------------------        
        }


        private static void UpdateDatabaseLockAsExpired(string numberType, int lockDuration)
        {
            var sqlStatement = new SqlStatement(DatabaseConnection.CurrentConnection);
            sqlStatement.Statement.Append("UPDATE `numbergenerator` SET ");
            sqlStatement.Statement.Append(DatabaseConnection.CurrentConnection.SqlFormatter.DelimitField("DateTimeLocked"));
            sqlStatement.Statement.Append(" = ");
            sqlStatement.AddParameterToStatement(DateTime.Now.AddMinutes(-1 * lockDuration - 1));
            sqlStatement.Statement.Append(" WHERE ");
            sqlStatement.Statement.Append(DatabaseConnection.CurrentConnection.SqlFormatter.DelimitField("NumberType"));
            sqlStatement.Statement.Append(" = ");
            sqlStatement.AddParameterToStatement(numberType);
            DatabaseConnection.CurrentConnection.ExecuteSql(sqlStatement);
        }
    }
}
