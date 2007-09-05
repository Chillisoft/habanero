using System.Data;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestParameterNameGenerator : TestUsingDatabase
    {
        //	private ParameterNameGenerator gen;


        [Test]
        public void TestNameGeneration()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.SqlServer, "test", "test", "test", "test", "1000");
            IDbConnection dbConn = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
            ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
            Assert.AreEqual("@Param0", gen.GetNextParameterName());
            Assert.AreEqual("@Param1", gen.GetNextParameterName());
            gen.Reset();
            Assert.AreEqual("@Param0", gen.GetNextParameterName());
        }

        [Test]
        public void TestNameGenerationMySql()
        {
            DatabaseConfig config = new DatabaseConfig(DatabaseConfig.MySql, "test", "test", "test", "test", "1000");
            IDbConnection dbConn = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
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
            IDbConnection dbConn = DatabaseConnectionFactory.CreateConnection(config).TestConnection;
            ParameterNameGenerator gen = new ParameterNameGenerator(dbConn);
            Assert.AreEqual(":Param0", gen.GetNextParameterName());
            Assert.AreEqual(":Param1", gen.GetNextParameterName());
            gen.Reset();
            Assert.AreEqual(":Param0", gen.GetNextParameterName());
        }
    }
}