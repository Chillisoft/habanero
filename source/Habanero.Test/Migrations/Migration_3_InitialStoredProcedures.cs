using FluentMigrator;

namespace Habanero.Test.Migrations
{
    [Migration(3)]
    public class Migration_3_InitialStoredProcedures : FluentMigrator.Migration
    {
        public override void Up()
        {
            IfDatabase("mysql").Execute.Sql("CREATE  PROCEDURE `TestStoredProc`(IN str VARCHAR(1024)) " +
                                            "BEGIN " +
                                            "INSERT INTO testtableread (TestTableReadData) values (str); " +
                                            "END;");

            IfDatabase("sqlserver").Execute.Sql("CREATE PROCEDURE [dbo].[TestStoredProc] @str nvarchar(1024) AS  " +
                                                "BEGIN " +
                                                "INSERT dbo.testtableread(dbo.testtableread.TestTableReadData)VALUES (@str)  " +
                                                "END");
        }

        public override void Down()
        {
            Execute.Sql("DROP PROCEDURE TestStoredProc");
        }

    }
}