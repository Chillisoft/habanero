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
		
    	#region SqlServer

        [Test]
        public void TestSqlServer()
        {
            String conn =
                new ConnectionStringSqlServerFactory().GetConnectionString("testserver", "testdb",
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
                new ConnectionStringSqlServerFactory().GetConnectionString("testserver", "testdb",
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
                new ConnectionStringOracleFactory().GetConnectionString("", "testdatasource",
                                                                                              "testuser", "testpassword",
                                                                                              "");
            Assert.AreEqual("Data Source=testdatasource;user ID=testuser;Password=testpassword;", conn,
                            "ConnectionStringFactory not working for Oracle");
        }

        [Test]
        public void TestOracleNoPassword()
        {
            String conn =
                new ConnectionStringOracleFactory().GetConnectionString("", "testdatasource",
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
                new ConnectionStringMySqlFactory().GetConnectionString("testserver", "testdb",
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
                new ConnectionStringMySqlFactory().GetConnectionString("testserver", "testdb",
                                                                                             "testusername", "",
                                                                                             "testport");
            Assert.AreEqual("Username=testusername; Host=testserver; Port=testport; Database=testdb;", conn,
                            "ConnectionStringFactory not working for MySql");
        }

        [Test]
        public void TestMySqlNoServerName()
        {
            //---------------Execute Test ----------------------
            try
            {
                String conn =
                    new ConnectionStringMySqlFactory().GetConnectionString("", "testdb",
                                                                           "testusername",
                                                                           "testpassword", "testport");

                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("The server, database and userName of a connect string can never be empty", ex.Message);
            }
        }

        [Test]
        public void TestMySqlNoUserName()
        {
            try
            {
                String conn =
                    new ConnectionStringMySqlFactory().GetConnectionString("sdf", "testdb", "",
                                                                           "testpassword", "testport");

                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("The server, database and userName of a connect string can never be empty", ex.Message);
            }
        }

        [Test]
        public void TestMySqlNoDatabaseName()
        {
            try
            {
                String conn =
                    new ConnectionStringMySqlFactory().GetConnectionString("sdf", "", "sasdf",
                                                                           "testpassword", "testport");

                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("The server, database and userName of a connect string can never be empty", ex.Message);
            }
        }

        [Test]
        public void TestMySqlNoPort()
        {
            String conn =
                new ConnectionStringMySqlFactory().GetConnectionString("testserver", "testdb",
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
                new ConnectionStringAccessFactory().GetConnectionString("testserver", "testdb",
                                                                                              "testusername",
                                                                                              "testpassword", "");
            Assert.AreEqual(
                @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=testdb;User ID=testusername;password=testpassword", conn,
                "ConnectionStringFactory not working for Access");
        }
        [Test]
        public void TestAccess2007()
        {
            String conn =
                new ConnectionStringAccess2007Factory().GetConnectionString("testserver", "testdb",
                                                                                              "testusername",
                                                                                              "testpassword", "");
            Assert.AreEqual(
                @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=testdb;User ID=testusername;password=testpassword", conn,
                "ConnectionStringFactory not working for Access2007");
        }

    	#endregion //Access

    	#region PostgreSql

		[Test]
		public void TestPostgreSql()
		{
			String conn =
                new ConnectionStringPostgreSqlFactory().GetConnectionString("testserver", "testdb",
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
                new ConnectionStringPostgreSqlFactory().GetConnectionString("testserver", "testdb",
																							 "testusername", "",
																							 "testport");
			Assert.AreEqual("Server=testserver;Port=testport;Database=testdb;Userid=testusername;", conn,
							"ConnectionStringFactory not working for PostgreSql");
		}

		[Test]
		public void TestPostgreSqlNoServerName()
		{
		    try
		    {
		        String conn =
		            new ConnectionStringPostgreSqlFactory().GetConnectionString("", "testdb",
		                                                                        "testusername",
		                                                                        "testpassword", "testport");

		        Assert.Fail("Expected to throw an ArgumentExc");
		    }
		        //---------------Test Result -----------------------
		    catch (ArgumentException ex)
		    {
                StringAssert.Contains("The server, database and userName of a connect string can never be empty", ex.Message);
		    }
		}

		[Test]
		public void TestPostgreSqlNoUserName()
		{
		    try
		    {
		        String conn =
		            new ConnectionStringPostgreSqlFactory().GetConnectionString("sdf", "testdb", "",
		                                                                        "testpassword", "testport");

		        Assert.Fail("Expected to throw an ArgumentException");
		    }
		        //---------------Test Result -----------------------
		    catch (ArgumentException ex)
		    {
                StringAssert.Contains("The server, database and userName of a connect string can never be empty", ex.Message);
		    }
		}

		[Test]
		public void TestPostgreSqlNoDatabaseName()
		{
		    try
		    {
		        String conn =
		            new ConnectionStringPostgreSqlFactory().GetConnectionString("sdf", "", "sasdf",
		                                                                        "testpassword", "testport");

		        Assert.Fail("Expected to throw an ArgumentException");
		    }
		        //---------------Test Result -----------------------
		    catch (ArgumentException ex)
		    {
                StringAssert.Contains("The server, database and userName of a connect string can never be empty", ex.Message);
		    }
		}

		[Test]
		public void TestPostgreSqlNoPort()
		{
			String conn =
                new ConnectionStringPostgreSqlFactory().GetConnectionString("testserver", "testdb",
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
                new ConnectionStringSQLiteFactory().GetConnectionString("testserver", "testdb",
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
                new ConnectionStringSQLiteFactory().GetConnectionString("testserver", "testdb",
                                                                                             "testusername", "",
                                                                                             "testport");
            Assert.AreEqual("Data Source=testdb;BinaryGUID=False", conn,
                            "ConnectionStringFactory not working for SQLite");
        }

        [Test]
        public void TestSQLiteNoServerName()
        {
            String conn =
                new ConnectionStringSQLiteFactory().GetConnectionString("", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "testport");
            Assert.AreEqual(
                "Data Source=testdb;Password=testpassword;BinaryGUID=False", conn,
                "ConnectionStringFactory not working for SQLite");
        }

        [Test]
        public void TestSQLiteNoUserName()
        {
            String conn =
                new ConnectionStringSQLiteFactory().GetConnectionString("testserver", "testdb", "",
                                                                                             "testpassword", "testport");
            Assert.AreEqual(
                "Data Source=testdb;Password=testpassword;BinaryGUID=False", conn,
                "ConnectionStringFactory not working for SQLite");
        }

        [Test]
        public void TestSQLiteNoDatabaseName()
        {
            try
            {
                String conn =
                    new ConnectionStringSQLiteFactory().GetConnectionString("testserver", "", "testusername",
                                                                            "testpassword", "testport");

                Assert.Fail("Expected to throw an Argument");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("The database parameter of a connect string can never be empty", ex.Message);
            }
        }

        [Test]
        public void TestSQLiteNoPort()
        {
            String conn =
                new ConnectionStringSQLiteFactory().GetConnectionString("testserver", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "");
            Assert.AreEqual(
                "Data Source=testdb;Password=testpassword;BinaryGUID=False", conn,
                "ConnectionStringFactory not working for SQLite");
        }

        #endregion //SQLite

        #region Firebird

        [Test]
        public void TestFirebird()
        {
            String conn =
                new ConnectionStringFirebirdFactory().GetConnectionString("testserver", "testdatasource",
                                                                                                "testuser", "testpassword",
                                                                                                "");
            Assert.AreEqual("Server=testserver;User=testuser;Password=testpassword;Database=testdatasource;ServerType=0", conn,
                            "ConnectionStringFactory not working for Firebird");
        }

        [Test]
        public void TestFirebirdEmbedded()
        {
            String conn =
                new ConnectionStringFirebirdEmbeddedFactory().GetConnectionString("testserver", "testdatasource",
                                                                                                "testuser", "testpassword",
                                                                                                "");
            Assert.AreEqual("User=testuser;Password=testpassword;Database=testdatasource;ServerType=1", conn,
                            "ConnectionStringFactory not working for Firebird");
        }

        [Test]
        public void TestFirebirdNoServerName()
        {
            try
            {
                String conn =
                    new ConnectionStringFirebirdFactory().GetConnectionString("", "testdb",
                                                                              "testusername",
                                                                              "testpassword", "testport");

                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("The server, database, password and userName of a connect string can never be empty", ex.Message);
            }
        }

        [Test]
        public void TestFirebirdEmbeddedNoServerName()
        {
            String conn =
                new ConnectionStringFirebirdEmbeddedFactory().GetConnectionString("", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "testport");
        }

        [Test]
        public void TestFirebirdNoUserName()
        {
            try
            {
                String conn =
                    new ConnectionStringFirebirdFactory().GetConnectionString("sdf", "testdb", "",
                                                                              "testpassword", "testport");

                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("The server, database, password and userName of a connect string can never be empty", ex.Message);
            }
        }

        [Test]
        public void TestFirebirdNoDatabaseName()
        {
            try
            {
                String conn =
                    new ConnectionStringFirebirdFactory().GetConnectionString("sdf", "", "sasdf",
                                                                              "testpassword", "testport");

                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("The server, database, password and userName of a connect string can never be empty", ex.Message);
            }
        }

        #endregion //Firebird
    }
}