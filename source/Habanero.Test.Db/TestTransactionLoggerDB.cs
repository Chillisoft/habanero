// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;
using Habanero.Test.BO;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestTransactionLoggerDB : TestUsingDatabase
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
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            base.SetupDBConnection();
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
        public void TestTransactionLogPersistsSQL_AddedToBusinessObjectPersistsSql()
        {
            //---------------Set up test pack-------------------
            //Create Mock Business object that implements a stub transaction log.
            ContactPersonTransactionLogging cp = CreateUnsavedContactPersonTransactionLogging();
            TransactionalBusinessObjectDB transactionalBODB = new TransactionalBusinessObjectDB(cp, DatabaseConnection.CurrentConnection);

            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            ISqlStatementCollection sqlStatementCollection = transactionalBODB.GetPersistSql();
            //---------------Test Result -----------------------
            //check if the transaction committer has 2 object
            // check that the one object is the transaction log object.
            Assert.AreEqual(2, sqlStatementCollection.Count);
            ISqlStatement sqlStatement = sqlStatementCollection[1];
            TransactionLogTable transactionLogTable = new TransactionLogTable(cp);
            Assert.AreEqual(transactionLogTable.GetPersistSql()[0].Statement.ToString(), sqlStatement.Statement.ToString());
        }


        [Test]
        public void TestTransactionLogPersistsSQL_NotAddedToBusinessObjectPersistsSql_WhenObjectUnchanged()
        {
            //---------------Set up test pack-------------------
            //Create Mock Business object that implements a stub transaction log.
            ContactPersonTransactionLogging cp = CreateUnsavedContactPersonTransactionLogging();
            TransactionCommitterStub tc = new TransactionCommitterStub();
            tc.AddBusinessObject(cp);
            tc.CommitTransaction();
            TransactionalBusinessObjectDB transactionalBODB = new TransactionalBusinessObjectDB(cp, DatabaseConnection.CurrentConnection);

            //---------------Assert Preconditions --------------

            //---------------Execute Test ----------------------
            ISqlStatementCollection sqlStatementCollection = transactionalBODB.GetPersistSql();
            //---------------Test Result -----------------------
            //check if the transaction committer has 2 object
            // check that the one object is the transaction log object.
            Assert.AreEqual(0, sqlStatementCollection.Count);
            //ISqlStatement sqlStatement = sqlStatementCollection[1];
            //TransactionLogTable transactionLogTable = new TransactionLogTable(cp);
            //Assert.AreEqual(transactionLogTable.GetPersistSql()[0].Statement.ToString(), sqlStatement.Statement.ToString());
        }



        private static ContactPersonTransactionLogging CreateUnsavedContactPersonTransactionLogging()
        {
            ContactPersonTransactionLogging cp = new ContactPersonTransactionLogging();
            cp.Surname = Guid.NewGuid().ToString();
            return cp;
        }
    }
}
