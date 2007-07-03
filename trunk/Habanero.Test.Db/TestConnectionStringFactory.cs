using System;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.Db
{
    /// <summary>
    /// Summary description for TestConnectionStringFactory.
    /// </summary>
    [TestFixture]
    public class TestConnectionStringFactory
    {
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

        [Test]
        public void TestGetFactory()
        {
            Assert.AreSame(typeof (ConnectionStringMySqlFactory),
                           ConnectionStringFactory.GetFactory(DatabaseConfig.MySql).GetType(),
                           "GetFactory not creating correct type : MySql.");
            Assert.AreSame(typeof (ConnectionStringSqlServerFactory),
                           ConnectionStringFactory.GetFactory(DatabaseConfig.SqlServer).GetType(),
                           "GetFactory not creating correct type : SqlServer.");
        }
    }
}