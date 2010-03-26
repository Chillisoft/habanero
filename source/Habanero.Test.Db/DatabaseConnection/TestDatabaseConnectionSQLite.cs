// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using System.Data;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDatabaseConnectionSQLite
    {

        [Test]
        [Ignore("Ignore - this test doesn't work on the build server (although it works on all desktop pcs)")]
        public void Test_CreateDatabaseConnection_SQLite()
        {
            DatabaseConnection conn =
                new DatabaseConnectionSQLite("System.Data.SQLite", "System.Data.SQLite.SQLiteConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.SQLite, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual("System.Data.SQLite", conn.TestConnection.GetType().Namespace,
                            "Namespace of SQLite connection is wrong.");
        }

        [Test]
        public void Test_IsolationLevel_SQLite()
        {
            //---------------Execute Test ----------------------
            DatabaseConnection conn =
                new DatabaseConnectionSQLite("System.Data.SQLite", "System.Data.SQLite.SQLiteConnection");
            //---------------Test Result -----------------------
            Assert.AreEqual(IsolationLevel.Serializable, conn.IsolationLevel);
        }

        [Test]
        [Ignore("Ignore - this test doesn't work on the build server (although it works on all desktop pcs)")]
        public void Test_OpenConnection()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection conn = new DatabaseConnectionSQLite("System.Data.SQLite", "System.Data.SQLite.SQLiteConnection");
            conn.ConnectionString = new DatabaseConfig(DatabaseConfig.SQLite, "", "sqlite-testdb.db", "", "", "").GetConnectionString();
            //---------------Execute Test ----------------------
            IDbConnection openConnection = null;
            try
            {
                openConnection = conn.GetOpenConnectionForReading();
             
                //---------------Test Result -----------------------
                Assert.IsNotNull(openConnection);
                Assert.AreEqual(ConnectionState.Open, openConnection.State );
                Assert.AreEqual("System.Data.SQLite", openConnection.GetType().Namespace);
            } 
            //---------------Tear down -------------------------
            finally
            {
                if (openConnection != null && openConnection.State != ConnectionState.Closed) { openConnection.Close();}
            }
        }

        [Test]
        [Ignore("Ignore - this test doesn't work on the build server (although it works on all desktop pcs)")]
        public void Test_BeginTransaction()
        {
            //---------------Set up test pack-------------------
            DatabaseConnection conn = new DatabaseConnectionSQLite("System.Data.SQLite", "System.Data.SQLite.SQLiteConnection");
            conn.ConnectionString = new DatabaseConfig(DatabaseConfig.SQLite, "", "sqlite-testdb.db", "", "", "").GetConnectionString();
            //---------------Execute Test ----------------------
            IDbConnection openConnection = null;
            try
            {
                openConnection = conn.GetOpenConnectionForReading();
                IDbTransaction transaction = conn.BeginTransaction(openConnection);

                //---------------Test Result -----------------------
                Assert.IsNotNull(transaction);
                Assert.AreSame(openConnection, transaction.Connection);
                Assert.AreEqual(IsolationLevel.Serializable, transaction.IsolationLevel);
            }
            //---------------Tear down -------------------------
            finally
            {
                if (openConnection != null && openConnection.State != ConnectionState.Closed) { openConnection.Close(); }
            }

        }

        [Test]
        public void Test_CreateSqlFormatter_SQLite()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection dbConn = new DatabaseConnectionSQLite("System.Data", "System.Data.SqlClient.SqlConnection");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(SqlFormatter), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("\"", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("\"", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("limit", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual(sqlFormatter.LeftFieldDelimiter, dbConn.LeftFieldDelimiter);
            Assert.AreEqual(sqlFormatter.RightFieldDelimiter, dbConn.RightFieldDelimiter);
            //            StringAssert.Contains(sqlFormatter.LimitClauseAtBeginning, dbConn.GetLimitClauseForBeginning(1));
            //            StringAssert.Contains(sqlFormatter.LimitClauseAtEnd, dbConn.GetLimitClauseForEnd(1));
        }

        [Test]
        public void Test_CreateSqlFormatter_AlternateConstructor_SQLite()
        {
            //---------------Set up test pack-------------------
            string connectionString =
                new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000").GetConnectionString
                    ();
            IDatabaseConnection dbConn = new DatabaseConnectionSQLite
                ("System.Data", "System.Data.SqlClient.SqlConnection", connectionString);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(SqlFormatter), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("\"", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("\"", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("limit", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual(sqlFormatter.LeftFieldDelimiter, dbConn.LeftFieldDelimiter);
            Assert.AreEqual(sqlFormatter.RightFieldDelimiter, dbConn.RightFieldDelimiter);
            //            StringAssert.Contains(sqlFormatter.LimitClauseAtBeginning, dbConn.GetLimitClauseForBeginning(1));
            //            StringAssert.Contains(sqlFormatter.LimitClauseAtEnd, dbConn.GetLimitClauseForEnd(1));
        }

        [Test]
        public void TestCreateParameterNameGenerator()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection databaseConnection = new DatabaseConnectionSQLite("", "");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IParameterNameGenerator generator = databaseConnection.CreateParameterNameGenerator();
            //---------------Test Result -----------------------
            Assert.AreEqual(":", generator.PrefixCharacter);
            //---------------Tear Down -------------------------          
        }
    }
}