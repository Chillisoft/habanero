//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;
using Rhino.Mocks;
//using ByteFX.Data.MySqlClient;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestSqlStatement
    {
        private IDbConnection connection;
        private String rawStatement;
        private SqlStatement testStatement;
        private IDbDataParameter addedParameter;
        private IDbCommand command;

        public static void RunTest()
        {
            TestSqlStatement test = new TestSqlStatement();
            test.SetupTestFixture();
        }

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");
            connection = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
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
                            testStatement.ToString(), "ToString of SqlStatement not correct.");
        }

        [Test]
        public void TestParamNameGenerator()
        {
            Assert.IsNotNull(testStatement.ParameterNameGenerator, "GetParameterNameGenerator returns null.");
            Assert.AreEqual("@Param0", testStatement.ParameterNameGenerator.GetNextParameterName());
            Assert.AreEqual("@Param1", testStatement.ParameterNameGenerator.GetNextParameterName());
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
        public void TestSqlStatementConstructor()
        {
            SqlStatement newTest = new SqlStatement(connection, "select * from bob");
            Assert.AreEqual("select * from bob", newTest.Statement.ToString(),
                            "SqlStatement constructor does not set the statement correctly.");
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
        //    DatabaseConfig oraConfig = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");
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