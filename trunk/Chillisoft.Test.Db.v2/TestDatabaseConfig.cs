using System;
using System.Collections;
using Chillisoft.Db.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Db.v2
{
    /// <summary>
    /// Summary description for TestDatabaseConfigDataReader.
    /// </summary>
    [TestFixture]
    public class TestDatabaseConfig
    {
        private IDictionary settingsMySQL;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            settingsMySQL = new Hashtable();
            settingsMySQL.Add("vendor", DatabaseConfig.MySQL);
            settingsMySQL.Add("server", "b");
            settingsMySQL.Add("database", "c");
            settingsMySQL.Add("username", "d");
            settingsMySQL.Add("password", "e");
            settingsMySQL.Add("port", "f");
        }

        [Test]
        public void TestDatabaseConfigSettings()
        {
            DatabaseConfig d = new DatabaseConfig(settingsMySQL);
            Assert.AreEqual(DatabaseConfig.MySQL, d.Vendor);
            Assert.AreEqual("b", d.Server);
            Assert.AreEqual("c", d.Database);
            Assert.AreEqual("d", d.UserName);
            Assert.AreEqual("e", d.Password);
            Assert.AreEqual("f", d.Port);
        }

        [Test]
        public void TestDatabaseConfigAlternateConstructor()
        {
            DatabaseConfig d = new DatabaseConfig(DatabaseConfig.MySQL, "a", "b", "c", "d", "e");
            Assert.AreEqual(DatabaseConfig.MySQL, d.Vendor);
            Assert.AreEqual("a", d.Server);
            Assert.AreEqual("b", d.Database);
            Assert.AreEqual("c", d.UserName);
            Assert.AreEqual("d", d.Password);
            Assert.AreEqual("e", d.Port);
        }

        // un-ignore this if you want to test reading from a config file.
        // Reading from a config file works fine if you're using NUnit and specify the config file
        // but using nunittestrunner you can't specify a config file.
        //		[Test, Ignore("If not running in NUnit")]
        //		public void TestReadFromConfigFile() {
        //			DatabaseConfig d = DatabaseConfig.ReadFromConfigFile();
        //			Assert.AreEqual("SQLServer", d.Vendor);
        //			Assert.AreEqual("Core", d.Server);
        //			Assert.AreEqual("WorkShopManagement", d.Database);
        //			Assert.AreEqual("sa", d.UserName);
        //			Assert.AreEqual("", d.Password);
        //			Assert.AreEqual("", d.Port);
        //		}

        [Test]
        public void TestUsingDatabaseConfigMySQL()
        {
            DatabaseConfig config = new DatabaseConfig(settingsMySQL);
            String connectString =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySQL).GetConnectionString("b", "c", "d", "e", "f");
            Assert.AreEqual(connectString, config.GetConnectionString(),
                            "ConnectionStringFactory not working for MySQL using ConfigData");
        }
    }
}