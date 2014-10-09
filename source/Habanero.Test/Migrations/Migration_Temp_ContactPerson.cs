using FluentMigrator;

namespace Habanero.Test.Migrations
{
    public class Migration_Temp_ContactPerson : AutoReversingMigration
    {
        private readonly string _tableName;

        public Migration_Temp_ContactPerson(string tableName)
        {
            _tableName = tableName;
        }

        public override void Up()
        {
            Migration_1_InitialDataStructure.CreateContactPersonTable(this, _tableName);
        }
    }
}