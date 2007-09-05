using System.Data;
using Habanero.BO.CriteriaManager;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for TestBusinessObjectISqlParameterInfo.
    /// </summary>
    [TestFixture]
    public class TestBusinessObjectISqlParameterInfo : TestUsingDatabase
    {
        public TestBusinessObjectISqlParameterInfo()
        {
        }

        [TestFixtureSetUp]
        public void SetupTestFixture()
        {
            this.SetupDBConnection();
        }

        [Test]
        public void TestSqlParameterInfo()
        {
            IExpression exp = Expression.CreateExpression("PK3Prop = 'test'");
            ContactPerson cp = new ContactPerson();
            SqlCriteriaCreator creator = new SqlCriteriaCreator(exp, cp);
            SqlStatement statement = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            creator.AppendCriteriaToStatement(statement);
            Assert.AreEqual("ContactPerson.PK3_Prop = ?Param0", statement.Statement.ToString());
            Assert.AreEqual("test", ((IDbDataParameter) statement.Parameters[0]).Value);
        }
    }
}