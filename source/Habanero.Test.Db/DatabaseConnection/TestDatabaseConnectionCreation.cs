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
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class TestDatabaseConnectionCreation
    {
        #region MySql

        [Test]
        public void Test_CreateDatabaseConnection_MySQL()
        {
            DatabaseConnection conn = new DatabaseConnectionMySql
                ("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.MySql, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual
                ("MySql.Data.MySqlClient", conn.TestConnection.GetType().Namespace,
                 "Namespace of mysqlconnection is wrong.");
        }

        [Test]
        public void Test_IsolationLevel_MySQL()
        {
            //---------------Execute Test ----------------------
            DatabaseConnection conn =
                new DatabaseConnectionMySql("MySql.Data", "MySql.Data.MySqlClient.MySqlConnection");
            //---------------Test Result -----------------------
            Assert.AreEqual(IsolationLevel.RepeatableRead, conn.IsolationLevel);
        }

		#endregion

		#region SqlServer

		[Test]
		public void Test_CreateDatabaseConnection_SqlServer()
		{
			DatabaseConnection conn = new DatabaseConnectionSqlServer
				("System.Data", "System.Data.SqlClient.SqlConnection");
			conn.ConnectionString =
				new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000").GetConnectionString
					();
			Assert.AreEqual
				("System.Data.SqlClient", conn.TestConnection.GetType().Namespace,
				 "Namespace of Sql connection is wrong.");
		}

		[Test]
		public void Test_IsolationLevel_SqlServer()
		{
			//---------------Execute Test ----------------------
			DatabaseConnection conn =
				new DatabaseConnectionSqlServer("System.Data", "System.Data.SqlClient.SqlConnection");
			//---------------Test Result -----------------------
			Assert.AreEqual(IsolationLevel.ReadUncommitted, conn.IsolationLevel);
		}

		[Test]
		public void Test_CreateSqlFormatter_SQLServer()
		{
			//---------------Set up test pack-------------------
			IDatabaseConnection dbConn = new DatabaseConnectionSqlServer
				("System.Data", "System.Data.SqlClient.SqlConnection");
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
			SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
			Assert.IsNotNull(sqlFormatter);
			Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
			Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
			Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
			Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
			Assert.AreEqual("[", dbConn.LeftFieldDelimiter);
			Assert.AreEqual("]", dbConn.RightFieldDelimiter);
			//            StringAssert.Contains("TOP", dbConn.GetLimitClauseForBeginning(1));
			//            Assert.AreEqual("", dbConn.GetLimitClauseForEnd(1));
		}

		[Test]
		public void Test_CreateSqlFormatter_AlternateConstructor_SQLServer()
		{
			//---------------Set up test pack-------------------
			string connectionString =
				new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000").GetConnectionString
					();
			IDatabaseConnection dbConn = new DatabaseConnectionSqlServer
				("System.Data", "System.Data.SqlClient.SqlConnection", connectionString);
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
			SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
			Assert.IsNotNull(sqlFormatter);
			Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
			Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
			Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
			Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
			Assert.AreEqual("[", dbConn.LeftFieldDelimiter);
			Assert.AreEqual("]", dbConn.RightFieldDelimiter);
			//            StringAssert.Contains("TOP", dbConn.GetLimitClauseForBeginning(1));
			//            Assert.AreEqual("", dbConn.GetLimitClauseForEnd(1));
		}

		#endregion

		#region SqlServerCe

		[Test]
		public void Test_CreateDatabaseConnection_SqlServerCe()
		{
			DatabaseConnection conn = new DatabaseConnectionSqlServerCe
				("System.Data.SqlServerCe", "System.Data.SqlServerCe.SqlCeConnection");
			conn.ConnectionString =
				new DatabaseConfig(DatabaseConfig.SqlServerCe, "test", "test", "test", "test", "1000").GetConnectionString
					();
			Assert.AreEqual
				("System.Data.SqlServerCe", conn.TestConnection.GetType().Namespace,
				 "Namespace of Sql connection is wrong.");
		}

		[Test]
		public void Test_IsolationLevel_SqlServerCe()
		{
			//---------------Execute Test ----------------------
			DatabaseConnection conn =
				new DatabaseConnectionSqlServerCe("System.Data.SqlServerCe", "System.Data.SqlServerCe.SqlCeConnection");
			//---------------Test Result -----------------------
			Assert.AreEqual(IsolationLevel.ReadCommitted, conn.IsolationLevel);
		}

		[Test]
		public void Test_CreateSqlFormatter_SQLServerCe()
		{
			//---------------Set up test pack-------------------
			IDatabaseConnection dbConn = new DatabaseConnectionSqlServerCe
				("System.Data.SqlServerCe", "System.Data.SqlServerCe.SqlCeConnection");
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof(SqlFormatterForSqlServerCe), defaultSqlFormatter);
			SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
			Assert.IsNotNull(sqlFormatter);
			Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
			Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
			Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
			Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
			Assert.AreEqual("[", dbConn.LeftFieldDelimiter);
			Assert.AreEqual("]", dbConn.RightFieldDelimiter);
			//            StringAssert.Contains("TOP", dbConn.GetLimitClauseForBeginning(1));
			//            Assert.AreEqual("", dbConn.GetLimitClauseForEnd(1));
		}

		[Test]
		public void Test_CreateSqlFormatter_AlternateConstructor_SQLServerCe()
		{
			//---------------Set up test pack-------------------
			string connectionString =
				new DatabaseConfig(DatabaseConfig.SqlServerCe, "test", "test", "test", "test", "1000").GetConnectionString
					();
			IDatabaseConnection dbConn = new DatabaseConnectionSqlServerCe
				("System.Data.SqlServerCe", "System.Data.SqlServerCe.SqlCeConnection", connectionString);
			//---------------Assert Precondition----------------
			//---------------Execute Test ----------------------
			ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
			//---------------Test Result -----------------------
			Assert.IsInstanceOf(typeof(SqlFormatterForSqlServerCe), defaultSqlFormatter);
			SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
			Assert.IsNotNull(sqlFormatter);
			Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
			Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
			Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
			Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
			Assert.AreEqual("[", dbConn.LeftFieldDelimiter);
			Assert.AreEqual("]", dbConn.RightFieldDelimiter);
			//            StringAssert.Contains("TOP", dbConn.GetLimitClauseForBeginning(1));
			//            Assert.AreEqual("", dbConn.GetLimitClauseForEnd(1));
		}

		#endregion

        #region Oracle

        //[Test]
        //public void Test_Oracle_CreateDatabaseConnection()
        //{

        //    DatabaseConnection conn =
        //        new DatabaseConnectionOracle("Oracle.DataAccess", "Oracle.DataAccess.Client.OracleConnection");
        //    conn.ConnectionString =
        //        new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000").GetConnectionString();
        //    Assert.AreEqual("Oracle.DataAccess.Client", conn.GetTestConnection().GetType().Namespace,
        //                    "Namespace of Oracle connection is wrong.");
        //}

        [Test]
        public void Test_CreateDatabaseConnection_OracleMicrosoft()
        {
            DatabaseConnection conn = new DatabaseConnectionOracle
                ("System.Data.OracleClient, Version=2.0.0.0, Culture=neutral,PublicKeyToken=b77a5c561934e089", "System.Data.OracleClient.OracleConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual
                ("System.Data.OracleClient", conn.TestConnection.GetType().Namespace,
                 "Namespace of Oracle connection is wrong.");
        }

        [Test]
        public void Test_IsolationLevel_Oracle()
        {
            //---------------Execute Test ----------------------
            DatabaseConnection conn =
                new DatabaseConnectionOracle("System.Data.OracleClient", "System.Data.OracleClient.OracleConnection");
            //---------------Test Result -----------------------
            Assert.AreEqual(IsolationLevel.ReadCommitted, conn.IsolationLevel);
        }

        [Test]
        public void Test_CreateSqlFormatter_Oracle()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection dbConn = new DatabaseConnectionOracle("System.Data", "System.Data.SqlClient.SqlConnection");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("ROWNUM <=", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual("", dbConn.LeftFieldDelimiter);
            Assert.AreEqual("", dbConn.RightFieldDelimiter);
//            StringAssert.Contains(sqlFormatter.LimitClauseAtBeginning, dbConn.GetLimitClauseForBeginning(1));
//            StringAssert.Contains(sqlFormatter.LimitClauseAtEnd, dbConn.GetLimitClauseForEnd(1));
        }

        [Test]
        public void Test_CreateSqlFormatter_AlternateConstructor_Oracle()
        {
            //---------------Set up test pack-------------------
            string connectionString =
                new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000").GetConnectionString
                    ();
            IDatabaseConnection dbConn = new DatabaseConnectionOracle
                ("System.Data", "System.Data.SqlClient.SqlConnection", connectionString);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("ROWNUM <=", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual("", dbConn.LeftFieldDelimiter);
            Assert.AreEqual("", dbConn.RightFieldDelimiter);
//            StringAssert.Contains(sqlFormatter.LimitClauseAtBeginning, dbConn.GetLimitClauseForBeginning(1));
//            StringAssert.Contains(sqlFormatter.LimitClauseAtEnd, dbConn.GetLimitClauseForEnd(1));
        }

        #endregion

        #region Access

        [Test]
        public void Test_CreateDatabaseConnection_Access()
        {
            DatabaseConnection conn =
                new DatabaseConnectionAccess("System.Data", "System.Data.OleDb.OleDbConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.Access, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual
                ("System.Data.OleDb", conn.TestConnection.GetType().Namespace,
                 "Namespace of Access connection is wrong.");
        }

        [Test]
        public void Test_IsolationLevel_Access()
        {
            //---------------Execute Test ----------------------
            DatabaseConnection conn =
                new DatabaseConnectionAccess("System.Data", "System.Data.OleDb.OleDbConnection");
            //---------------Test Result -----------------------
            Assert.AreEqual(IsolationLevel.ReadUncommitted, conn.IsolationLevel);
        }

        [Test]
        public void Test_CreateSqlFormatter_Access()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection dbConn = new DatabaseConnectionAccess("System.Data", "System.Data.SqlClient.SqlConnection");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatterForAccess), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual(sqlFormatter.LeftFieldDelimiter, dbConn.LeftFieldDelimiter);
            Assert.AreEqual(sqlFormatter.RightFieldDelimiter, dbConn.RightFieldDelimiter);
            //            StringAssert.Contains("TOP", dbConn.GetLimitClauseForBeginning(1));
            //            Assert.AreEqual("", dbConn.GetLimitClauseForEnd(1));
        }

        [Test]
        public void Test_CreateSqlFormatter_AlternateConstructor_Access()
        {
            //---------------Set up test pack-------------------
            string connectionString =
                new DatabaseConfig(DatabaseConfig.Access, "test", "test", "test", "test", "1000").GetConnectionString
                    ();
            IDatabaseConnection dbConn = new DatabaseConnectionAccess
                ("System.Data", "System.Data.SqlClient.SqlConnection", connectionString);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatterForAccess), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual(sqlFormatter.LeftFieldDelimiter, dbConn.LeftFieldDelimiter);
            Assert.AreEqual(sqlFormatter.RightFieldDelimiter, dbConn.RightFieldDelimiter);
            //            StringAssert.Contains("TOP", dbConn.GetLimitClauseForBeginning(1));
            //            Assert.AreEqual("", dbConn.GetLimitClauseForEnd(1));
        }
        #endregion

        #region Access 2007

        [Test]
        public void Test_CreateDatabaseConnection_Access2007()
        {
            DatabaseConnection conn =
                new DatabaseConnectionAccess2007("System.Data", "System.Data.OleDb.OleDbConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.Access2007, "test", "test", "test", "test", "1000").GetConnectionString();
            Assert.AreEqual
                ("System.Data.OleDb", conn.TestConnection.GetType().Namespace,
                 "Namespace of Access connection is wrong.");
        }

        [Test]
        public void Test_IsolationLevel_Access2007()
        {
            //---------------Execute Test ----------------------
            DatabaseConnection conn =
                new DatabaseConnectionAccess2007("System.Data", "System.Data.OleDb.OleDbConnection");
            //---------------Test Result -----------------------
            Assert.AreEqual(IsolationLevel.ReadUncommitted, conn.IsolationLevel);
        }

        [Test]
        public void Test_CreateSqlFormatter_Access2007()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection dbConn = new DatabaseConnectionAccess2007("System.Data", "System.Data.SqlClient.SqlConnection");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatterForAccess), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual(sqlFormatter.LeftFieldDelimiter, dbConn.LeftFieldDelimiter);
            Assert.AreEqual(sqlFormatter.RightFieldDelimiter, dbConn.RightFieldDelimiter);
            //            StringAssert.Contains("TOP", dbConn.GetLimitClauseForBeginning(1));
            //            Assert.AreEqual("", dbConn.GetLimitClauseForEnd(1));
        }

        [Test]
        public void Test_CreateSqlFormatter_AlternateConstructor_Access2007()
        {
            //---------------Set up test pack-------------------
            string connectionString =
                new DatabaseConfig(DatabaseConfig.Access2007, "test", "test", "test", "test", "1000").GetConnectionString
                    ();
            IDatabaseConnection dbConn = new DatabaseConnectionAccess
                ("System.Data", "System.Data.SqlClient.SqlConnection", connectionString);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatterForAccess), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("[", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("]", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("TOP", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual(sqlFormatter.LeftFieldDelimiter, dbConn.LeftFieldDelimiter);
            Assert.AreEqual(sqlFormatter.RightFieldDelimiter, dbConn.RightFieldDelimiter);
            //            StringAssert.Contains("TOP", dbConn.GetLimitClauseForBeginning(1));
            //            Assert.AreEqual("", dbConn.GetLimitClauseForEnd(1));
        }
        #endregion

        #region PostgreSql

        [Test]
        public void Test_CreateDatabaseConnection_PostgreSql()
        {
            DatabaseConnection conn =
                new DatabaseConnectionPostgreSql("Npgsql", "Npgsql.NpgsqlConnection");
            conn.ConnectionString =
                new DatabaseConfig(DatabaseConfig.PostgreSql, "test", "test", "test", "test", "1000").
                    GetConnectionString();
            Assert.AreEqual
                ("Npgsql", conn.TestConnection.GetType().Namespace, "Namespace of PostgreSql connection is wrong.");
        }

        [Test]
        public void Test_IsolationLevel_PostgreSql()
        {
            //---------------Execute Test ----------------------
            DatabaseConnection conn =
                new DatabaseConnectionPostgreSql("Npgsql", "Npgsql.NpgsqlConnection");
            //---------------Test Result -----------------------
            Assert.AreEqual(IsolationLevel.RepeatableRead, conn.IsolationLevel);
        }

        [Test]
        public void Test_CreateSqlFormatter_PostgreSql()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection dbConn = new DatabaseConnectionPostgreSql("System.Data", "System.Data.SqlClient.SqlConnection");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
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
        public void Test_CreateSqlFormatter_AlternateConstructor_PostgreSql()
        {
            //---------------Set up test pack-------------------
            string connectionString =
                new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000").GetConnectionString();
            IDatabaseConnection dbConn = new DatabaseConnectionPostgreSql
                ("System.Data", "System.Data.SqlClient.SqlConnection", connectionString);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
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

        #endregion

        #region SQLite


        #endregion

        #region Firebird

        [Test]
        public void Test_CreateDatabaseConnection_Firebird()
        {
            
            DatabaseConnection conn = new DatabaseConnectionFirebird
                ("FirebirdSql.Data.FirebirdClient", "FirebirdSql.Data.FirebirdClient.FbConnection");
            conn.ConnectionString =
                new DatabaseConfig
                    (DatabaseConfig.Firebird, "testserver", "testdatabase", "testusername", "testpassword", "3050").
                    GetConnectionString();
            Assert.AreEqual
                ("FirebirdSql.Data.FirebirdClient", conn.TestConnection.GetType().Namespace,
                 "Namespace of firebird connection is wrong.");
        }

        [Test]
        public void Test_IsolationLevel_Firebird()
        {
            //---------------Execute Test ----------------------
            DatabaseConnection conn =
                new DatabaseConnectionFirebird("FirebirdSql.Data.FirebirdClient", "FirebirdSql.Data.FirebirdClient.FbConnection");
            //---------------Test Result -----------------------
            Assert.AreEqual(IsolationLevel.RepeatableRead, conn.IsolationLevel);
        }

        [Test]
        public void Test_CreateSqlFormatter_Firebird()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection dbConn = new DatabaseConnectionFirebird("System.Data", "System.Data.SqlClient.SqlConnection");
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("FIRST", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual(sqlFormatter.LeftFieldDelimiter, dbConn.LeftFieldDelimiter);
            Assert.AreEqual(sqlFormatter.RightFieldDelimiter, dbConn.RightFieldDelimiter);
//            StringAssert.Contains(sqlFormatter.LimitClauseAtBeginning, dbConn.GetLimitClauseForBeginning(1));
//            StringAssert.Contains(sqlFormatter.LimitClauseAtEnd, dbConn.GetLimitClauseForEnd(1));
        }

        [Test]
        public void Test_CreateSqlFormatter_AlternateConstructor_Firebird()
        {
            //---------------Set up test pack-------------------
            string connectionString =
                new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000").GetConnectionString
                    ();
            IDatabaseConnection dbConn = new DatabaseConnectionFirebird
                ("System.Data", "System.Data.SqlClient.SqlConnection", connectionString);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            ISqlFormatter defaultSqlFormatter = dbConn.SqlFormatter;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf(typeof(SqlFormatter), defaultSqlFormatter);
            SqlFormatter sqlFormatter = (SqlFormatter)defaultSqlFormatter;
            Assert.IsNotNull(sqlFormatter);
            Assert.AreEqual("", sqlFormatter.LeftFieldDelimiter);
            Assert.AreEqual("", sqlFormatter.RightFieldDelimiter);
            Assert.AreEqual("FIRST", sqlFormatter.LimitClauseAtBeginning);
            Assert.AreEqual("", sqlFormatter.LimitClauseAtEnd);
            Assert.AreEqual(sqlFormatter.LeftFieldDelimiter, dbConn.LeftFieldDelimiter);
            Assert.AreEqual(sqlFormatter.RightFieldDelimiter, dbConn.RightFieldDelimiter);
//            StringAssert.Contains(sqlFormatter.LimitClauseAtBeginning, dbConn.GetLimitClauseForBeginning(1));
//            StringAssert.Contains(sqlFormatter.LimitClauseAtEnd, dbConn.GetLimitClauseForEnd(1));
        }

        #endregion
    }

   
}