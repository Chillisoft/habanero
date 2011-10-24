using System.Collections.Generic;
using System.Linq;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestNullTransactionLogger
    {

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
            ClassDef.ClassDefs.Clear();
            ContactPersonTestBO.LoadDefaultClassDef();
        }
        
        [Test]
        public void Constructor()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var transactionLogger = new NullTransactionLogger();
            //---------------Test Result -----------------------
            Assert.IsNotNull(transactionLogger);
            Assert.IsInstanceOf<ITransactionLog>(transactionLogger, "Should be instance of ITransactionLog");
        }

        [Test]
        public void TransactionID_ShouldReturnTransactionID()
        {
            //---------------Set up test pack-------------------
            var transactionLogger = new NullTransactionLogger();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(transactionLogger);
            //---------------Execute Test ----------------------
            var transactionID = transactionLogger.TransactionID();
            //---------------Test Result -----------------------
            Assert.IsNotNull(transactionID);
            StringAssert.Contains("NullTransactionLoggerID", transactionID);
        }

        [Test]
        public void UpdateAsRolledBack_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var transactionLogger = new NullTransactionLogger();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(transactionLogger);
            //---------------Execute Test ----------------------
            transactionLogger.UpdateAsRolledBack();
            //---------------Test Result -----------------------
            Assert.IsNotNull(transactionLogger);
        }

        [Test]
        public void UpdateStateAsCommitted_ShouldNotThrowException()
        {
            //---------------Set up test pack-------------------
            var transactionLogger = new NullTransactionLogger();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(transactionLogger);
            //---------------Execute Test ----------------------
            transactionLogger.UpdateStateAsCommitted();
            //---------------Test Result -----------------------
            Assert.IsNotNull(transactionLogger);
        }

        [Test]
        public void GetPersistSql_ShouldReturnSqlStatementList()
        {
            //---------------Set up test pack-------------------
            var transactionLogger = new NullTransactionLogger();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(transactionLogger);
            //---------------Execute Test ----------------------
            IEnumerable<ISqlStatement> sqlStatements = transactionLogger.GetPersistSql();
            //---------------Test Result -----------------------
            Assert.IsNotNull(sqlStatements);
            Assert.AreEqual(0, sqlStatements.Count());
        }

    }
}