using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDatabaseConnectionFirebird
    {
        [Test]
        public void TestCreateParameterNameGenerator()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IParameterNameGenerator generator = databaseConnection.CreateParameterNameGenerator();
            //---------------Test Result -----------------------
            Assert.AreEqual("@", generator.PrefixCharacter);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSqlFormatter()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            SqlFormatter formatter = databaseConnection.SqlFormatter;

            //---------------Test Result -----------------------
            Assert.AreEqual("", formatter.LeftFieldDelimiter);
            Assert.AreEqual("", formatter.RightFieldDelimiter);
            Assert.AreEqual("FIRST", formatter.LimitClauseAtBeginning);
            Assert.AreEqual("", formatter.LimitClauseAtEnd);

        }


        [Test]
        public void Test_CreateCustomConnectionString()
        {
            //---------------Set up test pack-------------------
            string connectString = "";
            DatabaseConnection databaseConnection = new DatabaseConnectionFirebird("", "", connectString);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string setConnectionString = databaseConnection.ConnectionString;

            //---------------Test Result -----------------------
            Assert.AreEqual(setConnectionString, connectString);
        }

    }
}