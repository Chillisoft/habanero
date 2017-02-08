#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;
using Habanero.Base;
using Habanero.DB;
using Habanero.Util;
using NUnit.Framework;

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
            var test = new TestSqlStatement();
            test.SetupTestFixture();
        }

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            var config = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");
            _connection = new DatabaseConnectionFactory().CreateConnection(config);
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
        public void Statement()
        {
            //---------------Set up test pack-------------------
            var sql = new SqlStatement(_connection);
            var sb = new StringBuilder("test");
            //---------------Execute Test ----------------------
            sql.Statement = sb;
            //---------------Test Result -----------------------
            Assert.AreSame(sb, sql.Statement);
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
            var addingParamStatement = new SqlStatement(_connection);
            addingParamStatement.AddParameterToStatement(1);
            Assert.AreEqual("@Param0", addingParamStatement.Statement.ToString(),
                            "AddParameterToStatement is not building statement correctly.");
        }

        [Test]
        public void TestSqlStatementConstructor()
        {
            var newTest = new SqlStatement(_connection, "select * from bob");
            Assert.AreEqual("select * from bob", newTest.Statement.ToString(),
                            "SqlStatement constructor does not set the statement correctly.");
        }

		[Test]
		public void AddParameter_WithNullValueAndType()
		{
			//---------------Set up test pack-------------------
			const string startSql = "select * from bob WHERE name = ";
			var builder = new SqlStatement(_connection, startSql);
			const string paramName = "@Param0";
			var paramType = typeof(string);
			const DbType expectedParamType = DbType.String;
			//---------------Execute Test ----------------------
			var param = builder.AddParameter(paramName, null, paramType);
			var actual = builder.Statement.ToString();
			//---------------Test Result -----------------------
			Assert.AreEqual(expectedParamType, param.DbType);
			Assert.AreEqual(1, builder.Parameters.Count);
			Assert.AreEqual(startSql, actual);
		}

		[Test]
		public void AddParameter_WithNullValueAndTimeSpanType_ShouldBeDateTimeDbType()
		{
			//---------------Set up test pack-------------------
			const string startSql = "select * from bob WHERE name = ";
			var builder = new SqlStatement(_connection, startSql);
			const string paramName = "@Param0";
			var paramType = typeof(TimeSpan);
			const DbType expectedParamType = DbType.DateTime;
			//---------------Execute Test ----------------------
			var param = builder.AddParameter(paramName, null, paramType);
			var actual = builder.Statement.ToString();
			//---------------Test Result -----------------------
			Assert.AreEqual(expectedParamType, param.DbType);
			Assert.AreEqual(1, builder.Parameters.Count);
			Assert.AreEqual(startSql, actual);
		}

        [Test]
        public void AddParameter_WithOnlyValue()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob WHERE name = ";
            var builder = new SqlStatement(_connection, startSql);
            const string paramName = "@Param0";
            const DbType expectedParamType = DbType.String;
            //---------------Execute Test ----------------------
            var param = builder.AddParameter(paramName, "bob");
            var actual = builder.Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedParamType, param.DbType);
            Assert.AreEqual(1, builder.Parameters.Count);
            Assert.AreEqual(startSql, actual);
        }

        [Test]
        public void Equals_MatchingSql()
        {
            //---------------Set up test pack-------------------
            var test1 = new SqlStatement(_connection, "test");
            var test2 = new SqlStatement(_connection, "test");
            //---------------Test Result -----------------------
            Assert.AreEqual(test1, test2);
        }
       
        [Test]
        public void Equals_NonMatchingSql()
        {
            //---------------Set up test pack-------------------
            var test1 = new SqlStatement(_connection, "test1");
            var test2 = new SqlStatement(_connection, "test2");
            //---------------Test Result -----------------------
            Assert.AreNotEqual(test1, test2);
        }
       
        [Test]
        public void Equals_MatchingSqlAndParams()
        {
            //---------------Set up test pack-------------------
            var test1 = new SqlStatement(_connection, "test");
            var test2 = new SqlStatement(_connection, "test");
            test1.AddParameter("param0", 1);
            test2.AddParameter("param0", 1);
            //---------------Test Result -----------------------
            Assert.AreEqual(test1, test2);
        }
       
        [Test]
        public void Equals_MatchingSqlAndNonMatchingParamValues()
        {
            //---------------Set up test pack-------------------
            var test1 = new SqlStatement(_connection, "test");
            var test2 = new SqlStatement(_connection, "test");
            test1.AddParameter("param0", 1);
            test2.AddParameter("param0", 1);
            test1.AddParameter("param1", 2);
            test2.AddParameter("param1", 1);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(test1, test2);
        }
       
        [Test]
        public void Equals_MatchingSqlAndNonMatchingParamNames()
        {
            //---------------Set up test pack-------------------
            var test1 = new SqlStatement(_connection, "test");
            var test2 = new SqlStatement(_connection, "test");
            test1.AddParameter("param0", 1);
            test2.AddParameter("sdf", 1);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(test1, test2);
        }

        [Test]
        public void Equals_MatchingSqlAndNonMatchingNumbers()
        {
            //---------------Set up test pack-------------------
            var test1 = new SqlStatement(_connection, "test");
            var test2 = new SqlStatement(_connection, "test");
            test1.AddParameter("param0", 1);
            //---------------Test Result -----------------------
            Assert.IsFalse(test1.Equals(test2));
        }


        private static IDatabaseConnection GetSQLServerCeConnection()
        {
            var uri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) ?? "");
            var databaseFile = Path.Combine(uri.LocalPath, "sqlserverce-testdb.sdf");
            var databaseConfig = new DatabaseConfig(DatabaseConfig.SqlServerCe, "", databaseFile, "", "", null);
            return databaseConfig.GetDatabaseConnection();
        }

        [Test]
        public void AddParameter_WhenSqlServerCE_ByteArray_ShouldReturnSqlServerImageType()
        {
            //---------------Set up test pack-------------------
            var sqlStatement = new SqlStatement(GetSQLServerCeConnection(), "some statement");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var parameter = sqlStatement.AddParameter("imageParam", new byte[8001]);
            //---------------Test Result -----------------------
            Assert.AreEqual(SqlDbType.Image,ReflectionUtilities.GetPropertyValue(parameter,"SqlDbType"));
        }

        [Test]
        public void AddParameter_WhenSqlServerCE_StringGreaterThan4000_ShouldReturnSqlServerNTextType()
        {
            //---------------Set up test pack-------------------
            var sqlStatement = new SqlStatement(GetSQLServerCeConnection(), "some statement");
            var bigTextValue = TestUtil.GetRandomString(4001);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var parameter = sqlStatement.AddParameter("ntextParam", bigTextValue);
            //---------------Test Result -----------------------
            Assert.AreEqual(SqlDbType.NText,ReflectionUtilities.GetPropertyValue(parameter,"SqlDbType"));
        }
    }

}