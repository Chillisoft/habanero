using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public class TestDatabaseConnectionSpecificDatabase : TestUsingDatabase
    {
//        [SetUp]
//        public  void SetupTest()
//        {
//            //Runs every time that any testmethod is executed
//            base.SetupTest();
//        }
//
//        [TestFixtureSetUp]
//        public void TestFixtureSetup()
//        {
//            //Code that is executed before any test is run in this class. If multiple tests
//            // are executed then it will still only be called once.
//        }
//
//        [TearDown]
//        public override void TearDownTest()
//        {
//            //runs every time any testmethod is complete
//            base.TearDownTest();
//        }

        [Test]
        public void Test_CreateFirebirdConnection()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "");            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter formatter = databaseConnection.SqlFormatter;

            //---------------Test Result -----------------------
            Assert.AreEqual("", formatter.LeftFieldDelimiter);
            Assert.AreEqual("", formatter.RightFieldDelimiter);
            Assert.AreEqual("TOP", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("", formatter.LimitClauseAtEnd);

        }

        [Test]
        public void Test_CreateFirebirdConnection_AltConstructor()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "","");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter formatter = databaseConnection.SqlFormatter;

            //---------------Test Result -----------------------
            Assert.AreEqual("", formatter.LeftFieldDelimiter);
            Assert.AreEqual("", formatter.RightFieldDelimiter);
            Assert.AreEqual("TOP", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("", formatter.LimitClauseAtEnd);

        }

        [Test]
        public void Test_CreateMySqlConnection()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionMySql("", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter formatter = databaseConnection.SqlFormatter;

            //---------------Test Result -----------------------
            Assert.AreEqual("`", formatter.LeftFieldDelimiter);
            Assert.AreEqual("`", formatter.RightFieldDelimiter);
            Assert.AreEqual("", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("LIMIT", formatter.LimitClauseAtEnd);

        }

        [Test]
        public void Test_CreateMySqlConnection_AltConstructor()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionMySql("", "", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter formatter = databaseConnection.SqlFormatter;

            //---------------Test Result -----------------------
            Assert.AreEqual("`", formatter.LeftFieldDelimiter);
            Assert.AreEqual("`", formatter.RightFieldDelimiter);
            Assert.AreEqual("", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("LIMIT", formatter.LimitClauseAtEnd);

        }
    }
}