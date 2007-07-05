using Habanero.Bo;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.General
{
    public abstract class TestInheritanceBase : TestUsingDatabase
    {
        protected BusinessObject objCircle;
        protected SqlStatementCollection itsInsertSql;
        protected SqlStatementCollection itsUpdateSql;
        protected SqlStatementCollection itsDeleteSql;
        protected SqlStatement selectSql;
        protected string strID;

        public void SetupTest()
        {
            this.SetupDBConnection();
            SetupInheritanceSpecifics();
            objCircle = new Circle();
            SetStrID();
            objCircle.SetPropertyValue("ShapeName", "MyShape");
            objCircle.SetPropertyValue("Radius", 10);
            itsInsertSql = objCircle.GetInsertSql();
            itsUpdateSql = objCircle.GetUpdateSql();
            itsDeleteSql = objCircle.GetDeleteSql();
            selectSql = new SqlStatement(DatabaseConnection.CurrentConnection.GetConnection());
            selectSql.Statement.Append(objCircle.SelectSqlStatement(selectSql));
        }

        protected abstract void SetupInheritanceSpecifics();
        protected abstract void SetStrID();
    }
}