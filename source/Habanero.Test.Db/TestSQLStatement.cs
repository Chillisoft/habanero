//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Data;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;
//using ByteFX.Data.MySqlClient;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestSqlStatement
    {
        private IDatabaseConnection _connection;
        private String _rawStatement;
        private SqlStatement _testStatement;
        private IDbDataParameter _addedParameter;
        private IDbCommand _command;

        public static void RunTest()
        {
            TestSqlStatement test = new TestSqlStatement();
            test.SetupTestFixture();
        }

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");
            _connection = DatabaseConnectionFactory.CreateConnection(config);
            _testStatement = new SqlStatement(_connection);
            _rawStatement = "insert into tb1 (field1, field2, field3) values (@Param1, @Param2, @Param3)";
            _testStatement.Statement.Append(_rawStatement);
            _addedParameter = _testStatement.AddParameter("Param1", "12345");
            _testStatement.AddParameter("Param2", "67890");
            _testStatement.AddParameter("Param3", "13579");
            _command = _connection.GetConnection().CreateCommand();
            _testStatement.SetupCommand(_command);
        }

        [Test]
        public void TestStatement()
        {
            Assert.AreEqual(_rawStatement, _testStatement.Statement.ToString(),
                            "Statement property not functioning correctly.");
        }

        [Test]
        public void TestAddParameterType()
        {
            Assert.AreEqual("SqlParameter", _addedParameter.GetType().Name,
                            "Type of parameter should be SqlParameter but isn't.");
        }

        [Test]
        public void TestGetParameters()
        {
            Assert.AreEqual(3, _testStatement.Parameters.Count, "Number of parameters is incorrect.");
        }

        [Test]
        public void TestSetupCommandObject()
        {
            Assert.AreEqual(_rawStatement, _command.CommandText, "Command text is not the same as the raw statement.");
            Assert.AreEqual(3, _command.Parameters.Count, "Number of parameters of Command object is incorrect.");
        }

        [Test]
        public void TestToString()
        {
            Assert.AreEqual("Raw statement: " + _rawStatement + "   , Parameter values: 12345, 67890, 13579, ",
                            _testStatement.ToString(), "ToString of SqlStatement not correct.");
        }

        [Test]
        public void TestParamNameGenerator()
        {
            Assert.IsNotNull(_testStatement.ParameterNameGenerator, "GetParameterNameGenerator returns null.");
            Assert.AreEqual("@Param0", _testStatement.ParameterNameGenerator.GetNextParameterName());
            Assert.AreEqual("@Param1", _testStatement.ParameterNameGenerator.GetNextParameterName());
        }

        [Test]
        public void TestAddingParameterToStatement()
        {
            SqlStatement addingParamStatement = new SqlStatement(_connection);
            addingParamStatement.AddParameterToStatement(1);
            Assert.AreEqual("@Param0", addingParamStatement.Statement.ToString(),
                            "AddParameterToStatement is not building statement correctly.");
        }

        [Test]
        public void TestSqlStatementConstructor()
        {
            SqlStatement newTest = new SqlStatement(_connection, "select * from bob");
            Assert.AreEqual("select * from bob", newTest.Statement.ToString(),
                            "SqlStatement constructor does not set the statement correctly.");
        }

        [Test]
        public void TestEquals()
        {
            SqlStatement test1 = new SqlStatement(_connection, "test1");
            SqlStatement test2 = new SqlStatement(_connection, "test2");
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

        #region Test AppendCriteria

        [Test]
        public void TestAppendCriteria_WithNoWhere_AddWhere()
        {
            SqlStatement sql = new SqlStatement(_connection, "select * from bob");
            sql.AppendCriteria("this = that");
            Assert.AreEqual("select * from bob WHERE this = that", sql.Statement.ToString());
        }

        [Test]
        public void TestAppendCriteria_WithWhere_AddWhere()
        {
            SqlStatement sql = new SqlStatement(_connection, "select * from bob WHERE that = this");
            sql.AppendCriteria("this = that");
            Assert.AreEqual("select * from bob WHERE that = this AND this = that", sql.Statement.ToString());
        }

        [Test]
        public void TestAppendCriteria_Complex()
        {
            SqlStatement sql;
            sql = new SqlStatement(_connection, "select [FAKE WHERE CLAUSE] from bob");
            sql.AppendCriteria("this = that");
            Assert.AreEqual("select [FAKE WHERE CLAUSE] from bob WHERE this = that", sql.Statement.ToString());
            sql = new SqlStatement(_connection, "select [FAKE WHERE CLAUSE] from bob WHERE that = 'FAKE WHERE CLAUSE'");
            sql.AppendCriteria("this = that");
            Assert.AreEqual("select [FAKE WHERE CLAUSE] from bob WHERE that = 'FAKE WHERE CLAUSE' AND this = that", sql.Statement.ToString());
        }

        #endregion //Test AppendCriteria

        [Test]
        public void TestAppendOrderBy()
        {
            SqlStatement sql = new SqlStatement(_connection, "select * from bob WHERE that = this");
            sql.AppendOrderBy("this");
            Assert.AreEqual("select * from bob WHERE that = this ORDER BY this", sql.Statement.ToString());
        }

        #region Test AddJoin

        [Test, ExpectedException(typeof(SqlStatementException))]
        public void TestAddJoin_WithEmptyStatement()
        {
            SqlStatement sql = new SqlStatement(_connection, "");
            sql.AddJoin("left join", "bobby", "bobs = bobbys");
        }

        [Test]
        public void TestAddJoin_WithNoWhere()
        {
            SqlStatement sql = new SqlStatement(_connection, "select * from bob");
            sql.AddJoin("left join", "bobby", "bobs = bobbys");
            Assert.AreEqual("select * from (bob) LEFT JOIN [bobby] ON bobs = bobbys", sql.Statement.ToString());
        }

        [Test]
        public void TestAddJoin_WithNoWhere_AlsoAddWhere()
        {
            SqlStatement sql = new SqlStatement(_connection, "select * from bob");
            sql.AddJoin("left join", "bobby", "bobs = bobbys");
            sql.AppendCriteria("this = that");
            Assert.AreEqual("select * from (bob) LEFT JOIN [bobby] ON bobs = bobbys WHERE this = that", sql.Statement.ToString());
        }

        [Test]
        public void TestAddJoin_WithNoWhere_AddWhereThenAddJoin()
        {
            SqlStatement sql = new SqlStatement(_connection, "select * from bob");
            sql.AppendCriteria("this = that");
            sql.AddJoin("left join", "bobby", "bobs = bobbys");
            Assert.AreEqual("select * from (bob) LEFT JOIN [bobby] ON bobs = bobbys WHERE this = that", sql.Statement.ToString());
        }

        [Test]
        public void AddJoin_WithWhere()
        {
            SqlStatement sql = new SqlStatement(_connection, "select * from bob WHERE that = this");
            sql.AddJoin("left join", "bobby", "bobs = bobbys");
            Assert.AreEqual("select * from (bob) LEFT JOIN [bobby] ON bobs = bobbys WHERE that = this", sql.Statement.ToString());
        }

        [Test]
        public void AddJoin_WithWhere_AlsoAddWhere()
        {
            SqlStatement sql = new SqlStatement(_connection, "select * from bob WHERE that = this");
            sql.AddJoin("left join", "bobby", "bobs = bobbys");
            sql.AppendCriteria("this = that");
            Assert.AreEqual("select * from (bob) LEFT JOIN [bobby] ON bobs = bobbys WHERE that = this AND this = that", sql.Statement.ToString());
        }

        [Test]
        public void AddJoin_Complex()
        {
            SqlStatement sql = new SqlStatement(_connection, "select [FALSE FROM CLAUSE], [FALSE WHERE CLAUSE] from bob WHERE that = this");
            sql.AddJoin("left join", "bobby", "bobs = bobbys");
            sql.AppendCriteria("this = that");
            Assert.AreEqual("select [FALSE FROM CLAUSE], [FALSE WHERE CLAUSE] from (bob) LEFT JOIN [bobby] ON bobs = bobbys WHERE that = this AND this = that", sql.Statement.ToString());
        }

        #endregion //Test AddJoin

        //[Test]
        //public void TestOracleClobField()
        //{
        //    DatabaseConfig oraConfig = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");
        //    IDbConnection oraConnection;
        //    oraConnection = factory.CreateConnection(config).GetTestConnection();
        //    oraTestStatement = new SqlStatement(oraConnection);
        //    _rawStatement = "insert into tb1 (field1, field2, field3) values (@Param1, @Param2, @Param3)";
        //    ....
        //    _testStatement.Statement.Append(_rawStatement);
        //    _addedParameter = _testStatement.AddParameter("Param1", "12345");
        //    _testStatement.AddParameter("Param2", "67890");
        //    _testStatement.AddParameter("Param3", "13579");
        //    _command = _connection.CreateCommand();
        //    _testStatement.SetupCommand(_command);
        //}
    }
}