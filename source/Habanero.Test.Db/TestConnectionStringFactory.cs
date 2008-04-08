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
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    /// <summary>
    /// Summary description for TestConnectionStringFactory.
    /// </summary>
    [TestFixture]
    public class TestConnectionStringFactory
    {
		[Test]
		public void TestGetFactory()
		{
			Assert.AreSame(typeof(ConnectionStringSqlServerFactory),
						   ConnectionStringFactory.GetFactory(DatabaseConfig.SqlServer).GetType(),
						   "GetFactory not creating correct type : SqlServer.");
			Assert.AreSame(typeof(ConnectionStringOracleFactory),
						   ConnectionStringFactory.GetFactory(DatabaseConfig.Oracle).GetType(),
						   "GetFactory not creating correct type : Oracle.");
			Assert.AreSame(typeof(ConnectionStringMySqlFactory),
						   ConnectionStringFactory.GetFactory(DatabaseConfig.MySql).GetType(),
						   "GetFactory not creating correct type : MySql.");
			Assert.AreSame(typeof(ConnectionStringAccessFactory),
						   ConnectionStringFactory.GetFactory(DatabaseConfig.Access).GetType(),
						   "GetFactory not creating correct type : Access.");
            Assert.AreSame(typeof(ConnectionStringPostgreSqlFactory),
                           ConnectionStringFactory.GetFactory(DatabaseConfig.PostgreSql).GetType(),
                           "GetFactory not creating correct type : PostgreSql.");
            Assert.AreSame(typeof(ConnectionStringSQLiteFactory),
                           ConnectionStringFactory.GetFactory(DatabaseConfig.SQLite).GetType(),
                           "GetFactory not creating correct type : SQLite.");
        }

    	#region SqlServer

        [Test]
        public void TestSqlServer()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SqlServer).GetConnectionString("testserver", "testdb",
                                                                                                 "testusername",
                                                                                                 "testpassword",
                                                                                                 "testport");
            Assert.AreEqual("Server=testserver;Initial Catalog=testdb;User ID=testusername;password=testpassword;", conn,
                            "ConnectionStringFactory not working for Sql Server");
        }

        [Test]
        public void TestSqlServerNoPassword()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SqlServer).GetConnectionString("testserver", "testdb",
                                                                                                 "testusername", "",
                                                                                                 "testport");
            Assert.AreEqual("Server=testserver;Initial Catalog=testdb;User ID=testusername;", conn,
                            "ConnectionStringFactory not working for Sql Server");
        }

    	#endregion //SqlServer

    	#region Oracle

        [Test]
        public void TestOracle()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.Oracle).GetConnectionString("", "testdatasource",
                                                                                              "testuser", "testpassword",
                                                                                              "");
            Assert.AreEqual("Data Source=testdatasource;user ID=testuser;Password=testpassword;", conn,
                            "ConnectionStringFactory not working for Oracle");
        }

        [Test]
        public void TestOracleNoPassword()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.Oracle).GetConnectionString("", "testdatasource",
                                                                                              "testuser", "", "");
            Assert.AreEqual("Data Source=testdatasource;user ID=testuser;", conn,
                            "ConnectionStringFactory not working for Oracle");
        }

    	#endregion //Oracle

    	#region MySql

        [Test]
        public void TestMySql()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySql).GetConnectionString("testserver", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "testport");
            Assert.AreEqual(
                "Username=testusername; Host=testserver; Port=testport; Database=testdb; Password=testpassword;", conn,
                "ConnectionStringFactory not working for MySql");
        }

        [Test]
        public void TestMySqlNoPassword()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySql).GetConnectionString("testserver", "testdb",
                                                                                             "testusername", "",
                                                                                             "testport");
            Assert.AreEqual("Username=testusername; Host=testserver; Port=testport; Database=testdb;", conn,
                            "ConnectionStringFactory not working for MySql");
        }

        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestMySqlNoServerName()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySql).GetConnectionString("", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "testport");
        }

        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestMySqlNoUserName()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySql).GetConnectionString("sdf", "testdb", "",
                                                                                             "testpassword", "testport");
        }

        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestMySqlNoDatabaseName()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySql).GetConnectionString("sdf", "", "sasdf",
                                                                                             "testpassword", "testport");
        }

        [Test]
        public void TestMySqlNoPort()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySql).GetConnectionString("testserver", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "");
            Assert.AreEqual(
                "Username=testusername; Host=testserver; Port=3306; Database=testdb; Password=testpassword;", conn,
                "ConnectionStringFactory not working for MySql");
        }

    	#endregion //MySql

    	#region Access

        [Test]
        public void TestAccess()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.Access).GetConnectionString("testserver", "testdb",
                                                                                              "testusername",
                                                                                              "testpassword", "");
            Assert.AreEqual(
                @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=testdb;User ID=testusername;password=testpassword", conn,
                "ConnectionStringFactory not working for Access");
        }

    	#endregion //Access

    	#region PostgreSql

		[Test]
		public void TestPostgreSql()
		{
			String conn =
				ConnectionStringFactory.GetFactory(DatabaseConfig.PostgreSql).GetConnectionString("testserver", "testdb",
																							 "testusername",
																							 "testpassword", "testport");
			Assert.AreEqual(
				"Server=testserver;Port=testport;Database=testdb;Userid=testusername;Password=testpassword;", conn,
				"ConnectionStringFactory not working for PostgreSql");
		}

		[Test]
		public void TestPostgreSqlNoPassword()
		{
			String conn =
				ConnectionStringFactory.GetFactory(DatabaseConfig.PostgreSql).GetConnectionString("testserver", "testdb",
																							 "testusername", "",
																							 "testport");
			Assert.AreEqual("Server=testserver;Port=testport;Database=testdb;Userid=testusername;", conn,
							"ConnectionStringFactory not working for PostgreSql");
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void TestPostgreSqlNoServerName()
		{
			String conn =
				ConnectionStringFactory.GetFactory(DatabaseConfig.PostgreSql).GetConnectionString("", "testdb",
																							 "testusername",
																							 "testpassword", "testport");
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void TestPostgreSqlNoUserName()
		{
			String conn =
				ConnectionStringFactory.GetFactory(DatabaseConfig.PostgreSql).GetConnectionString("sdf", "testdb", "",
																							 "testpassword", "testport");
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void TestPostgreSqlNoDatabaseName()
		{
			String conn =
				ConnectionStringFactory.GetFactory(DatabaseConfig.PostgreSql).GetConnectionString("sdf", "", "sasdf",
																							 "testpassword", "testport");
		}

		[Test]
		public void TestPostgreSqlNoPort()
		{
			String conn =
				ConnectionStringFactory.GetFactory(DatabaseConfig.PostgreSql).GetConnectionString("testserver", "testdb",
																							 "testusername",
																							 "testpassword", "");
			Assert.AreEqual(
				"Server=testserver;Port=5432;Database=testdb;Userid=testusername;Password=testpassword;", conn,
				"ConnectionStringFactory not working for PostgreSql");
		}

    	#endregion //PostgreSql

        #region SQLite

        [Test]
        public void TestSQLite()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SQLite).GetConnectionString("testserver", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "testport");
            Assert.AreEqual(
                "Data Source=testdb;Password=testpassword;BinaryGUID=False", conn,
                "ConnectionStringFactory not working for SQLite");
        }

        [Test]
        public void TestSQLiteNoPassword()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SQLite).GetConnectionString("testserver", "testdb",
                                                                                             "testusername", "",
                                                                                             "testport");
            Assert.AreEqual("Data Source=testdb;BinaryGUID=False", conn,
                            "ConnectionStringFactory not working for SQLite");
        }

        public void TestSQLiteNoServerName()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SQLite).GetConnectionString("", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "testport");
            Assert.AreEqual(
                "Data Source=testdb;Password=testpassword;BinaryGUID=False", conn,
                "ConnectionStringFactory not working for SQLite");
        }

        public void TestSQLiteNoUserName()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SQLite).GetConnectionString("testserver", "testdb", "",
                                                                                             "testpassword", "testport");
            Assert.AreEqual(
                "Data Source=testdb;Password=testpassword;BinaryGUID=False", conn,
                "ConnectionStringFactory not working for SQLite");
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestSQLiteNoDatabaseName()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SQLite).GetConnectionString("testserver", "", "testusername",
                                                                                             "testpassword", "testport");
        }

        [Test]
        public void TestSQLiteNoPort()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SQLite).GetConnectionString("testserver", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "");
            Assert.AreEqual(
                "Data Source=testdb;Password=testpassword;BinaryGUID=False", conn,
                "ConnectionStringFactory not working for SQLite");
        }

        #endregion //SQLite
    }
}