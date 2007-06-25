using System.Data;
using Habanero.Db;
using NUnit.Framework;

namespace Habanero.Test.Db
{
    [TestFixture]
    public class TestParameterNameGenerator : TestUsingDatabase
    {
        //	private ParameterNameGenerator gen;


        [Test]
        public void TestNameGeneration()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SQLServer, "test", "test", "test", "test", "1000");
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();
            IDbConnection dbConn = factory.CreateConnection(config).GetTestConnection();
            ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
            Assert.AreEqual("@Param0", gen.GetNextParameterName());
            Assert.AreEqual("@Param1", gen.GetNextParameterName());
            gen.Reset();
            Assert.AreEqual("@Param0", gen.GetNextParameterName());
        }

        [Test]
        public void TestNameGenerationMySql()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.MySQL, "test", "test", "test", "test", "1000");
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();

            IDbConnection dbConn = factory.CreateConnection(config).GetTestConnection();
            ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
            Assert.AreEqual("?Param0", gen.GetNextParameterName());
            Assert.AreEqual("?Param1", gen.GetNextParameterName());
            gen.Reset();
            Assert.AreEqual("?Param0", gen.GetNextParameterName());
        }


        [Test]
        public void TestNameGenerationOracle()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.Oracle, "test", "test", "test", "test", "1000");
            DatabaseConnectionFactory factory = new DatabaseConnectionFactory();

            IDbConnection dbConn = factory.CreateConnection(config).GetTestConnection();
            ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
            Assert.AreEqual(":Param0", gen.GetNextParameterName());
            Assert.AreEqual(":Param1", gen.GetNextParameterName());
            gen.Reset();
            Assert.AreEqual(":Param0", gen.GetNextParameterName());
        }
    }
}