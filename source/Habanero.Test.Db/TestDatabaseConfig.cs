using System;
using System.Collections;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    /// <summary>
    /// Summary description for TestDatabaseConfigDataReader.
    /// </summary>
    [TestFixture]
    public class TestDatabaseConfig
    {
        private IDictionary settingsMySql;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            settingsMySql = new Hashtable();
            settingsMySql.Add("vendor", DatabaseConfig.MySql);
            settingsMySql.Add("server", "b");
            settingsMySql.Add("database", "c");
            settingsMySql.Add("username", "d");
            settingsMySql.Add("password", "e");
            settingsMySql.Add("port", "f");
        }

        [Test]
        public void TestDatabaseConfigSettings()
        {
            DatabaseConfig d = new DatabaseConfig(settingsMySql);
            Assert.AreEqual(DatabaseConfig.MySql, d.Vendor);
            Assert.AreEqual("b", d.Server);
            Assert.AreEqual("c", d.Database);
            Assert.AreEqual("d", d.UserName);
            Assert.AreEqual("e", d.Password);
            Assert.AreEqual("f", d.Port);
        }

        [Test]
        public void TestDatabaseConfigAlternateConstructor()
        {
            DatabaseConfig d = new DatabaseConfig(DatabaseConfig.MySql, "a", "b", "c", "d", "e");
            Assert.AreEqual(DatabaseConfig.MySql, d.Vendor);
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
        //			Assert.AreEqual("SqlServer", d.Vendor);
        //			Assert.AreEqual("Core", d.Server);
        //			Assert.AreEqual("WorkShopManagement", d.Database);
        //			Assert.AreEqual("sa", d.UserName);
        //			Assert.AreEqual("", d.Password);
        //			Assert.AreEqual("", d.Port);
        //		}

        [Test]
        public void TestUsingDatabaseConfigMySql()
        {
            DatabaseConfig config = new DatabaseConfig(settingsMySql);
            String connectString =
                ConnectionStringFactory.GetFactory(DatabaseConfig.MySql).GetConnectionString("b", "c", "d", "e", "f");
            Assert.AreEqual(connectString, config.GetConnectionString(),
                            "ConnectionStringFactory not working for MySql using ConfigData");
        }
    }
}