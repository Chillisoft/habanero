using System;
using System.Collections.Generic;
using System.Linq;
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
            TransactionalBusinessObjectDB transactionalBODB = new TransactionalBusinessObjectDB(cp);

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
            TransactionalBusinessObjectDB transactionalBODB = new TransactionalBusinessObjectDB(cp);

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
