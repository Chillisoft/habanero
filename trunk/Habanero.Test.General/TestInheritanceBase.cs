using Habanero.Bo;
using Habanero.Db;
using NUnit.Framework;

namespace Habanero.Test.General
{
    [TestFixture]
    public abstract class TestInheritanceBase : TestUsingDatabase
    {
        protected BusinessObject objCircle;
        protected SqlStatementCollection itsInsertSql;
        protected SqlStatementCollection itsUpdateSql;
        protected SqlStatementCollection itsDeleteSql;
        protected SqlStatement selectSql;
        protected string strID;

        [TestFixtureSetUp]
        public void SetupTest()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            objCircle = Circle.GetNewObject();
            SetStrID();
            objCircle.SetPropertyValue("ShapeName", "MyShape");
            objCircle.SetPropertyValue("Radius", 10);
            itsInsertSql = objCircle.GetInsertSQL();
            itsUpdateSql = objCircle.GetUpdateSQL();
            itsDeleteSql = objCircle.GetDeleteSQL();
            selectSql = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            selectSql.Statement.Append(objCircle.SelectSqlStatement(selectSql));
        }

        protected abstract void SetupInheritanceSpecifics();
        protected abstract void SetStrID();
    }
}