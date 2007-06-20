using System;
using Habanero.Db;
using NUnit.Framework;

namespace Chillisoft.Test.Db.v2
{
    /// <summary>
    /// Summary description for TestConnectionStringFactory.
    /// </summary>
    [TestFixture]
    public class TestConnectionStringFactory
    {
        [Test]
        public void TestSQLServer()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SQLServer).GetConnectionString("testserver", "testdb",
                                                                                                 "testusername",
                                                                                                 "testpassword",
                                                                                                 "testport");
            Assert.AreEqual("Server=testserver;Initial Catalog=testdb;User ID=testusername;password=testpassword;", conn,
                            "ConnectionStringFactory not working for SQL Server");
        }

        [Test]
        public void TestSQLServerNoPassword()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.SQLServer).GetConnectionString("testserver", "testdb",
                                                                                                 "testusername", "",
                                                                                                 "testport");
            Assert.AreEqual("Server=testserver;Initial Catalog=testdb;User ID=testusername;", conn,
                            "ConnectionStringFactory not working for SQL Server");
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
        public void TestMySQL()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySQL).GetConnectionString("testserver", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "testport");
            Assert.AreEqual(
                "Username=testusername; Host=testserver; Port=testport; Database=testdb; Password=testpassword;", conn,
                "ConnectionStringFactory not working for MySQL");
        }

        [Test]
        public void TestMySQLNoPassword()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySQL).GetConnectionString("testserver", "testdb",
                                                                                             "testusername", "",
                                                                                             "testport");
            Assert.AreEqual("Username=testusername; Host=testserver; Port=testport; Database=testdb;", conn,
                            "ConnectionStringFactory not working for MySQL");
        }

        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestMySqlNoServerName()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySQL).GetConnectionString("", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "testport");
        }

        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestMySqlNoUserName()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySQL).GetConnectionString("sdf", "testdb", "",
                                                                                             "testpassword", "testport");
        }

        [Test, ExpectedException(typeof (ArgumentException))]
        public void TestMySqlNoDatabaseName()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySQL).GetConnectionString("sdf", "", "sasdf",
                                                                                             "testpassword", "testport");
        }

        [Test]
        public void TestMySqlNoPort()
        {
            String conn =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySQL).GetConnectionString("testserver", "testdb",
                                                                                             "testusername",
                                                                                             "testpassword", "");
            Assert.AreEqual(
                "Username=testusername; Host=testserver; Port=3306; Database=testdb; Password=testpassword;", conn,
                "ConnectionStringFactory not working for MySQL");
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
            Assert.AreSame(typeof (ConnectionStringMySQLFactory),
                           ConnectionStringFactory.GetFactory(DatabaseConfig.MySQL).GetType(),
                           "GetFactory not creating correct type : MySQL.");
            Assert.AreSame(typeof (ConnectionStringSQLServerFactory),
                           ConnectionStringFactory.GetFactory(DatabaseConfig.SQLServer).GetType(),
                           "GetFactory not creating correct type : SQLServer.");
        }
    }
}