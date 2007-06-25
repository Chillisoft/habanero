using System;
using System.Data;
using Habanero.Db;
using NUnit.Framework;

namespace Habanero.Test.Db
{
    [TestFixture]
    public class TestSQLStatementList : TestUsingDatabase
    {
        private SqlStatementCollection testCollection;
        private SqlStatement testStatement1;
        private SqlStatement testStatement2;
        private SqlStatementCollection testCollectionWithOneStatement;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            try
            {
                this.SetupDBConnection();
                IDbConnection connection = DatabaseConnection.CurrentConnection.GetConnection();
                testCollection = new SqlStatementCollection();
                testStatement1 = new SqlStatement(connection);
                testStatement2 = new SqlStatement(connection);
                testCollectionWithOneStatement = new SqlStatementCollection(testStatement1);
                testCollection.Add(testStatement1);
                testCollection.Add(testStatement2);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        [Test]
        public void TestCount()
        {
            Assert.AreEqual(2, testCollection.Count, "Count property on SQLStatementList is returning incorrect value.");
        }

        [Test]
        public void TestAddList()
        {
            SqlStatementCollection newCollection = new SqlStatementCollection();
            newCollection.Add(testCollection);
            Assert.AreEqual(2, newCollection.Count, "Adding a list to a SQLStatementList not working properly.");
        }

        [Test]
        public void TestToString()
        {
            Assert.AreEqual(
                testStatement1.ToString() + Environment.NewLine + testStatement2.ToString() + Environment.NewLine,
                testCollection.ToString(), "Tostring of SQLStatementList not working correctly.");
        }

        [Test]
        public void TestConstructorWithOneStatement()
        {
            Assert.AreEqual(1, testCollectionWithOneStatement.Count,
                            "A SQLStatementList created with a SQLStatement should only contain one statement");
            Assert.AreSame(testStatement1, testCollectionWithOneStatement[0]);
        }
    }
}