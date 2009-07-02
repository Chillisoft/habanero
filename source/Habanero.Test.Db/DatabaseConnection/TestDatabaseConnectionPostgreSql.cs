using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDatabaseConnectionPostgreSql
    {
        [Test]
        public void TestCreateParameterNameGenerator()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection databaseConnection = new DatabaseConnectionPostgreSql("", "");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IParameterNameGenerator generator = databaseConnection.CreateParameterNameGenerator();
            //---------------Test Result -----------------------
            Assert.AreEqual(":", generator.PrefixCharacter);
            //---------------Tear Down -------------------------          
        }
    }
}