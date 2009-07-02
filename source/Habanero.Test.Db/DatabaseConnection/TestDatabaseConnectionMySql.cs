using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDatabaseConnectionMySql
    {
        [Test]
        public void TestCreateParameterNameGenerator()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection databaseConnection = new DatabaseConnectionMySql("", "");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IParameterNameGenerator generator = databaseConnection.CreateParameterNameGenerator();
            //---------------Test Result -----------------------
            Assert.AreEqual("?", generator.PrefixCharacter);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestSqlFormatter()
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
        public void Test_CreateCustomConnectionString()
        {
            //---------------Set up test pack-------------------
            string connectString = "";
            DatabaseConnection databaseConnection = new DatabaseConnectionMySql("", "", connectString);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string setConnectionString = databaseConnection.ConnectionString;

            //---------------Test Result -----------------------
            Assert.AreEqual(setConnectionString, connectString);
        }
    }
}
