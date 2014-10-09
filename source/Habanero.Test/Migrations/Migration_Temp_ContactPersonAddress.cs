using FluentMigrator;

namespace Habanero.Test.Migrations
{
    public class Migration_Temp_ContactPersonAddress : AutoReversingMigration
    {
        private readonly string _tableName;
        private readonly string _referencedContactPersonTableName;

        public Migration_Temp_ContactPersonAddress(string tableName, string referencedContactPersonTableName)
        {
            _tableName = tableName;
            _referencedContactPersonTableName = referencedContactPersonTableName;
        }

        public override void Up()
        {
            Migration_1_InitialDataStructure.CreateContactPersonAddressTable(this, _tableName, _referencedContactPersonTableName);
        }
    }
}