using System;
using System.Data;
using System.Text;
using Habanero.Db;
using NUnit.Framework;
//using ByteFX.Data.MySqlClient;

namespace Chillisoft.Test.Db.v2
{
    [TestFixture]
    public class TestSQLStatement
    {
        private IDbConnection connection;
        private String rawStatement;
        private SqlStatement testStatement;
        private IDbDataParameter addedParameter;
        private IDbCommand command;

        public static void RunTest()
        {
            TestSQLStatement test = new TestSQLStatement();
            test.SetupTestFixture();
        }

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SQLServer, "test", "test", "test", "test", "1000");
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            connection = factory.CreateConnection(config).GetTestConnection();
            testStatement = new SqlStatement(connection);
            rawStatement = "insert into tb1 (field1, field2, field3) values (@Param1, @Param2, @Param3)";
            testStatement.Statement.Append(rawStatement);
            addedParameter = testStatement.AddParameter("Param1", "12345");
            testStatement.AddParameter("Param2", "67890");
            testStatement.AddParameter("Param3", "13579");
            command = connection.CreateCommand();
            testStatement.SetupCommand(command);
        }

        [Test]
        public void TestStatement()
        {
            Assert.AreEqual(rawStatement, testStatement.Statement.ToString(),
                            "Statement property not functioning correctly.");
        }

        [Test]
        public void TestAddParameterType()
        {
            Assert.AreEqual("SqlParameter", addedParameter.GetType().Name,
                            "Type of parameter should be SqlParameter but isn't.");
        }

        [Test]
        public void TestGetParameters()
        {
            Assert.AreEqual(3, testStatement.Parameters.Count, "Number of parameters is incorrect.");
        }

        [Test]
        public void TestSetupCommandObject()
        {
            Assert.AreEqual(rawStatement, command.CommandText, "Command text is not the same as the raw statement.");
            Assert.AreEqual(3, command.Parameters.Count, "Number of parameters of Command object is incorrect.");
        }

        [Test]
        public void TestToString()
        {
            Assert.AreEqual("Raw statement: " + rawStatement + "   , Parameter values: 12345, 67890, 13579, ",
                            testStatement.ToString(), "ToString of SQLStatement not correct.");
        }

        [Test]
        public void TestParamNameGenerator()
        {
            Assert.IsNotNull(testStatement.GetParameterNameGenerator(), "GetParameterNameGenerator returns null.");
            Assert.AreEqual("@Param0", testStatement.GetParameterNameGenerator().GetNextParameterName());
            Assert.AreEqual("@Param1", testStatement.GetParameterNameGenerator().GetNextParameterName());
        }

        [Test]
        public void TestAddingParameterToStatement()
        {
            SqlStatement addingParamStatement = new SqlStatement(connection);
            addingParamStatement.AddParameterToStatement(1);
            Assert.AreEqual("@Param0", addingParamStatement.Statement.ToString(),
                            "AddParameterToStatement is not building statement correctly.");
        }

        [Test]
        public void TestSQLStatementConstructor()
        {
            SqlStatement newTest = new SqlStatement(connection, "select * from bob");
            Assert.AreEqual("select * from bob", newTest.Statement.ToString(),
                            "SQLStatement constructor does not set the statement correctly.");
        }

        [Test]
        public void TestEquals()
        {
            SqlStatement test1 = new SqlStatement(connection, "test1");
            SqlStatement test2 = new SqlStatement(connection, "test2");
            Assert.IsFalse(test1.Equals(test2));
            test2.Statement = new StringBuilder("test1");
            Assert.IsTrue(test1.Equals(test2));
            test1.AddParameter("param0", 1);
            test2.AddParameter("param0", 1);
            Assert.IsTrue(test1.Equals(test2));
            test1.AddParameter("param1", 2);
            test2.AddParameter("param1", 1);
            Assert.IsFalse(test1.Equals(test2));
            test1.Parameters.Clear();
            test2.Parameters.Clear();
            test1.AddParameter("param0", 1);
            test2.AddParameter("sdf", 1);
            Assert.IsFalse(test1.Equals(test2));
            test1.Parameters.Clear();
            test2.Parameters.Clear();
            test1.AddParameter("param0", 1);
            Assert.IsFalse(test1.Equals(test2));
        }

        [Test]
        public void TestAppendCriteriaWithNoWhere()
        {
            SqlStatement sql = new SqlStatement(connection, "select * from bob");
            sql.AppendCriteria("this = that");
            Assert.AreEqual("select * from bob WHERE this = that", sql.Statement.ToString());
        }

        [Test]
        public void TestAppendCriteriaWhere()
        {
            SqlStatement sql = new SqlStatement(connection, "select * from bob WHERE that = this");
            sql.AppendCriteria("this = that");
            Assert.AreEqual("select * from bob WHERE that = this AND this = that", sql.Statement.ToString());
        }

        [Test]
        public void TestAppendOrderBy()
        {
            SqlStatement sql = new SqlStatement(connection, "select * from bob WHERE that = this");
            sql.AppendOrderBy("this");
            Assert.AreEqual("select * from bob WHERE that = this ORDER BY this", sql.Statement.ToString());
        }

        //[Test]
        //public void TestOracleClobField()
        //{
        //    DatabaseConfig oraConfig = new DatabaseConfig(DatabaseConfig.SQLServer, "test", "test", "test", "test", "1000");
        //    IDbConnection oraConnection;
        //    oraConnection = factory.CreateConnection(config).GetTestConnection();
        //    oraTestStatement = new SqlStatement(oraConnection);
        //    rawStatement = "insert into tb1 (field1, field2, field3) values (@Param1, @Param2, @Param3)";
        //    ....
        //    testStatement.Statement.Append(rawStatement);
        //    addedParameter = testStatement.AddParameter("Param1", "12345");
        //    testStatement.AddParameter("Param2", "67890");
        //    testStatement.AddParameter("Param3", "13579");
        //    command = connection.CreateCommand();
        //    testStatement.SetupCommand(command);
        //}
    }
}