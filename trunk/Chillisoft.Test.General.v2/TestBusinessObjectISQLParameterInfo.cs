using System.Data;
using Chillisoft.Bo.CriteriaManager.v2;
using Chillisoft.Db.v2;
using NUnit.Framework;

namespace Chillisoft.Test.General.v2
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
            Assert.AreEqual("tbContactPerson.PK3_Prop = ?Param0", statement.Statement.ToString());
            Assert.AreEqual("test", ((IDbDataParameter) statement.Parameters[0]).Value);
        }
    }
}