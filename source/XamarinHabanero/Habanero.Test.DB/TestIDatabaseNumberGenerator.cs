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

using System.Linq;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Base;
using Habanero.DB;
using Habanero.Test.Structure;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestIDatabaseNumberGenerator : TestUsingDatabase
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
        public void GetNextNumber_WhenWasOne_ShouldReturnTwo()
        {
            //---------------Set up test pack-------------------
            var numberType = RandomValueGen.GetRandomString();
            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            IDBNumberGenerator numGen = new DatabaseNumberGenerator(numberType, "numbergenerator", 0, "NumberType", "SequenceNumber");
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, numGen.GetNextNumberInt());
            //---------------Execute Test ----------------------
            var nextNumberInt = numGen.GetNextNumberInt();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, nextNumberInt);
        }
        [Test]
        public void TestIntegration_UpdateAsRolledBack_ThenGetNextNumber_ShouldResetNumberToOriginalNumber()
        {
            //See GetNextNumber_WhenWasOne_ShouldReturnTwo Test for what would happen if the UpdateAsRolledBack Did not happen 
            //---------------Set up test pack-------------------
            var numberType = RandomValueGen.GetRandomString();

            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            IDBNumberGenerator numGen = new DatabaseNumberGenerator(numberType, "numbergenerator", 0, "NumberType", "SequenceNumber");
            var updateTransaction = numGen.GetUpdateTransaction();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, numGen.GetNextNumberInt());
            //---------------Execute Test ----------------------
            updateTransaction.UpdateAsRolledBack();
            var nextNumberInt = numGen.GetNextNumberInt();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, nextNumberInt);
        }
        [Test]
        public void TestIntegration_Commit_ThenRollBack_ThenGetNextNumber_ShouldGenerateNextNumber()
        {
            //See GetNextNumber_WhenWasOne_ShouldReturnTwo Test for what would happen if the UpdateAsRolledBack Did not happen 
            //See TestAcceptance_UpdateAsRolledBack_ThenGetNextNumber_ShouldResetNumberToOriginalNumber to see what would happen if RollBackWasCalled
            //---------------Set up test pack-------------------
            var numberType = RandomValueGen.GetRandomString();

            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            IDBNumberGenerator numGen = new DatabaseNumberGenerator(numberType, "numbergenerator", 0, "NumberType", "SequenceNumber");
            var updateTransaction = numGen.GetUpdateTransaction();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, numGen.GetNextNumberInt());
            //---------------Execute Test ----------------------
            updateTransaction.UpdateStateAsCommitted();
            updateTransaction.UpdateAsRolledBack();
            var nextNumberInt = numGen.GetNextNumberInt();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, nextNumberInt);
        }

        [Test]
        public void TestIntegration_GetPersistSql_WhenNumberEQ2_ShouldReturnCorrectSQL()
        {
            //---------------Set up test pack-------------------
            var numberType = RandomValueGen.GetRandomString();

            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            IDBNumberGenerator numGen = new DatabaseNumberGenerator(numberType, "numbergenerator", 0, "NumberType", "SequenceNumber");
            var updateTransaction = numGen.GetUpdateTransaction() as ITransactionalDB;
            numGen.GetNextNumberInt();
            var currentNumber = numGen.GetNextNumberInt();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(updateTransaction);
            Assert.AreEqual(2, currentNumber);
            //---------------Execute Test ----------------------
            var sqlStatements = updateTransaction.GetPersistSql();
            //---------------Test Result -----------------------
            var sqlStatement = sqlStatements.FirstOrDefault();
            Assert.IsNotNull(sqlStatement);
            var stringStatement = sqlStatement.Statement.ToString();

            StringAssert.StartsWith(" update numbergenerator", stringStatement);
        }

        [Test]
        public void TestIntegration_UpdateDB_WhenCurrentNumberIs2_ShouldUpdateDB()
        {
            //---------------Set up test pack-------------------
            var numberType = RandomValueGen.GetRandomString();

            //Create an instance of the number for a specific type of number (e.g. Invoice number)
            var dbTransactionCommitter = BORegistry.DataAccessor.CreateTransactionCommitter();
            IDBNumberGenerator numGen = new DatabaseNumberGenerator(numberType, "numbergenerator", 1, "NumberType", "SequenceNumber");
            var updateTransaction = numGen.GetUpdateTransaction() as ITransactionalDB;
            var currentNumber = numGen.GetNextNumberInt();
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<TransactionCommitterDB>(dbTransactionCommitter);
            Assert.IsNotNull(updateTransaction);
            Assert.AreEqual(2, currentNumber);
            //---------------Execute Test ----------------------
            dbTransactionCommitter.AddTransaction(updateTransaction);
            dbTransactionCommitter.CommitTransaction();
            //---------------Test Result -----------------------

            IDBNumberGenerator numGen2 = new DatabaseNumberGenerator(numberType, "numbergenerator", 1, "NumberType", "SequenceNumber");
            var nextNumberInt = numGen2.GetNextNumberInt();
            Assert.AreEqual(3, nextNumberInt);
        }

    }
}