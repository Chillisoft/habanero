using System.Data;
using Habanero.Base;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.DB
{
    [TestFixture]
    public class TestDatabaseConnectionSqlServer
    {
        [Test]
        public void TestCreateParameterNameGenerator()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection databaseConnection = new DatabaseConnectionSqlServer("", "");
            //---------------Assert PreConditions---------------            
            //---------------Execute Test ----------------------
            IParameterNameGenerator generator = databaseConnection.CreateParameterNameGenerator();
            //---------------Test Result -----------------------
            Assert.AreEqual("@", generator.PrefixCharacter);
            //---------------Tear Down -------------------------          
        }
        
        [Test]
        public void Test_NoColumnName_DoesntError_SqlServer()
        {
            //---------------Set up test pack-------------------
            IDatabaseConnection originalConnection = DatabaseConnection.CurrentConnection;
            DatabaseConfig databaseConfig = new DatabaseConfig("SqlServer", "localhost", "habanero_test_trunk", "sa", "sa", null);
            DatabaseConnection.CurrentConnection = databaseConfig.GetDatabaseConnection();
            //DatabaseConnection.CurrentConnection = new DatabaseConnectionSqlServer("System.Data", "System.Data.SqlClient.SqlConnection","server=localhost;database=habanero_test_trunk;user=sa;password=sa");
            const string sql = "Select FirstName + ', ' + Surname from tbPersonTable";
            SqlStatement sqlStatement = new SqlStatement(DatabaseConnection.CurrentConnection, sql);

            //---------------Execute Test ----------------------
            DataTable dataTable = DatabaseConnection.CurrentConnection.LoadDataTable(sqlStatement, "", "");

            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataTable.Columns.Count);

            //---------------Tear Down -------------------------     
            DatabaseConnection.CurrentConnection = originalConnection;
        }
    }
}