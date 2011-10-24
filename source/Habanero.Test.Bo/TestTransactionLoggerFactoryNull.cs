using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestTransactionLoggerFactoryNull
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
            var transactionLoggerFactory = new TransactionLoggerFactoryNull();
            //---------------Test Result -----------------------
            Assert.IsNotNull(transactionLoggerFactory);
            Assert.IsInstanceOf<ITransactionLoggerFactory>(transactionLoggerFactory, "Should be instance of ITransactionLoggerFactory");
        }
        
        [Test]
        public void GetLogger_WithBOAndTableName_ShouldReturnNullTransactionLogger()
        {
            //---------------Set up test pack-------------------
            var tableName = TestUtil.GetRandomString();
            var transactionLoggerFactory = new TransactionLoggerFactoryNull();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(transactionLoggerFactory);
            //---------------Execute Test ----------------------
            var transactionLog = transactionLoggerFactory.GetLogger(new ContactPersonTestBO(), tableName);
            //---------------Test Result -----------------------
            Assert.IsNotNull(transactionLog);
            Assert.IsInstanceOf<NullTransactionLogger>(transactionLog, "Should be NullTransactionLogger");
        }

        [Test]
        public void GetLogger_WithBO_ShouldReturnNullTransactionLogger()
        {
            //---------------Set up test pack-------------------
            var transactionLoggerFactory = new TransactionLoggerFactoryNull();
            //---------------Assert Precondition----------------
            Assert.IsNotNull(transactionLoggerFactory);
            //---------------Execute Test ----------------------
            var transactionLog = transactionLoggerFactory.GetLogger(new ContactPersonTestBO());
            //---------------Test Result -----------------------
            Assert.IsNotNull(transactionLog);
            Assert.IsInstanceOf<NullTransactionLogger>(transactionLog, "Should be NullTransactionLogger");
        }
    }
}
