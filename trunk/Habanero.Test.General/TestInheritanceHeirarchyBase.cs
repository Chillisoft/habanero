using Habanero.Bo;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    public abstract class TestInheritanceHeirarchyBase : TestUsingDatabase
    {
        protected BusinessObject itsFilledCircle;
        protected SqlStatementCollection itsInsertSql;
        protected SqlStatementCollection itsUpdateSql;
        protected SqlStatementCollection itsDeleteSql;
        protected SqlStatement itsSelectSql;
        protected string itsFilledCircleId;
        protected SqlStatement itsLoadSql;

        [TestFixtureSetUp]
        public void SetupTest()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            itsFilledCircle = FilledCircle.GetNewObject();
            SetStrID();
            itsFilledCircle.SetPropertyValue("Colour", 3);
            itsFilledCircle.SetPropertyValue("Radius", 10);
            itsFilledCircle.SetPropertyValue("ShapeName", "MyFilledCircle");

            itsInsertSql = itsFilledCircle.GetInsertSql();
            itsUpdateSql = itsFilledCircle.GetUpdateSql();
            itsDeleteSql = itsFilledCircle.GetDeleteSql();
            itsSelectSql = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            itsSelectSql.Statement.Append(itsFilledCircle.SelectSqlStatement(itsSelectSql));
        }

        protected abstract void SetupInheritanceSpecifics();
        protected abstract void SetStrID();
    }
}