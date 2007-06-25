using System.Data;
using Habanero.Bo.CriteriaManager;
using Habanero.Db;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestBusinessObjectISQLParameterInfo.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectISQLParameterInfo : TestUsingDatabase
    {
        public TestBusinessObjectISQLParameterInfo()
        {
        }

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void TestSQLParameterInfo()
        {
            IExpression exp = Expression.CreateExpression("PK3Prop = 'test'");
            ContactPerson cp = ContactPerson.GetNewContactPerson();
            SQLCriteriaCreator creator = new SQLCriteriaCreator(exp, cp);
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            creator.AppendCriteriaToStatement(statement);
            Assert.AreEqual("ContactPerson.PK3_Prop = ?Param0", statement.Statement.ToString());
            Assert.AreEqual("test", ((IDbDataParameter) statement.Parameters[0]).Value);
        }
    }
}