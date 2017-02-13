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

using System.Collections.Generic;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestSqlStatementBuilder
    {
        private IDatabaseConnection _connection;

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            var config = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");
            _connection = new DatabaseConnectionFactory().CreateConnection(config);
        }

        [Test]
        public void AppendCriteria_WithNoWhere_AddWhere()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string appendSql = "this = that";
            const string expectedSql = startSql + " WHERE " + appendSql;
            //---------------Execute Test ----------------------
            builder.AppendCriteria(appendSql);
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AppendCriteria_WithWhere_AddWhere()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob WHERE that = this";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string appendSql = "this = that";
            const string expectedSql = startSql + " AND " + appendSql;
            //---------------Execute Test ----------------------
            builder.AppendCriteria("this = that");
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AppendCriteria_Complex()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select [FAKE WHERE CLAUSE] from bob WHERE that = 'FAKE WHERE CLAUSE'";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string appendSql = "this = that";
            const string expectedSql = startSql + " AND " + appendSql;
            //---------------Execute Test ----------------------
            builder.AppendCriteria(appendSql);
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AppendOrderBy()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob WHERE that = this";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string appendSql = "this";
            const string expectedSql = startSql + " ORDER BY " + appendSql;
            //---------------Execute Test ----------------------
            builder.AppendOrderBy(appendSql);
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AddJoin_WithEmptyStatement()
        {
            //---------------Set up test pack-------------------
            const string startSql = "";
            var builder = new SqlStatementBuilder(_connection, startSql);

            //---------------Execute Test ----------------------
            try
            {
                builder.AddJoin("left join", "bobby", "bobs = bobbys");
                Assert.Fail("Expected to throw an SqlStatementException");
            }
            //---------------Test Result -----------------------
            catch (SqlStatementException ex)
            {
                StringAssert.Contains("Cannot add a join clause to a SQL statement that does not contain a from clause", ex.Message);
            }
        }

        [Test]
        public void TestAddJoin_WithNoWhere()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string expectedSql = "select DISTINCT * from bob LEFT JOIN [bobby] ON bobs = bobbys";
            //---------------Execute Test ----------------------
            builder.AddJoin("left join", "bobby", "bobs = bobbys");
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AddJoin_WithNoWhere_InnerJoin()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string expectedSql = "select * from bob INNER JOIN [bobby] ON bobs = bobbys";
            //---------------Execute Test ----------------------
            builder.AddJoin("inner join", "bobby", "bobs = bobbys");
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AddJoin_WithNoWhere_AlsoAddWhere()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string expectedSql = "select DISTINCT * from bob LEFT JOIN [bobby] ON bobs = bobbys WHERE this = that";
            //---------------Execute Test ----------------------
            builder.AddJoin("left join", "bobby", "bobs = bobbys");
            builder.AppendCriteria("this = that");
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AddJoin_WithWhere()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob WHERE this = that";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string expectedSql = "select DISTINCT * from bob LEFT JOIN [bobby] ON bobs = bobbys WHERE this = that";
            //---------------Execute Test ----------------------
            builder.AddJoin("left join", "bobby", "bobs = bobbys");
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AddJoin_WithWhere_AlsoAddWhere()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob WHERE that = this";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string expectedSql = "select DISTINCT * from bob LEFT JOIN [bobby] ON bobs = bobbys WHERE that = this AND this = that";
            //---------------Execute Test ----------------------
            builder.AddJoin("left join", "bobby", "bobs = bobbys");
            builder.AppendCriteria("this = that");
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AddJoin_WithWhere_AlsoAddWhere_AddAnotherJoin()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select * from bob WHERE that = this";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string expectedSql = "select DISTINCT * from (bob LEFT JOIN [bobby] ON bobs = bobbys) "+
                                       "LEFT JOIN [bobbin] ON bobbys = bobbins WHERE that = this AND this = that";
            //---------------Execute Test ----------------------
            builder.AddJoin("left join", "bobby", "bobs = bobbys");
            builder.AppendCriteria("this = that");
            builder.AddJoin("left join", "bobbin", "bobbys = bobbins");
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void AddJoin_Complex()
        {
            //---------------Set up test pack-------------------
            const string startSql = "select [FALSE FROM CLAUSE], [FALSE WHERE CLAUSE] from bob WHERE that = this";
            var builder = new SqlStatementBuilder(_connection, startSql);
            const string expectedSql = "select DISTINCT [FALSE FROM CLAUSE], [FALSE WHERE CLAUSE] from bob LEFT JOIN [bobby] ON bobs = bobbys WHERE that = this AND this = that";
            //---------------Execute Test ----------------------
            builder.AddJoin("left join", "bobby", "bobs = bobbys");
            builder.AppendCriteria("this = that");
            var actual = builder.GetStatement().Statement.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedSql, actual);
        }

        [Test]
        public void TestAddSelectFields()
        {
            //-------------Setup Test Pack ------------------
            const string startSql = "select [FALSE FROM CLAUSE], [FALSE WHERE CLAUSE] from bob WHERE that = this";
            var builder = new SqlStatementBuilder(_connection, startSql);
            var fields = new List<string> {"myField1", "myField2"};
            const string expectedSql = "select [FALSE FROM CLAUSE], [FALSE WHERE CLAUSE], [myField1], [myField2] from bob WHERE that = this";
            //-------------Execute test ---------------------
            builder.AddSelectFields(fields);
            var actual = builder.GetStatement().Statement.ToString();
            //-------------Test Result ----------------------
            Assert.AreEqual(expectedSql, actual);
        }           
        
    }
}