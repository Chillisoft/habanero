using FluentMigrator;

namespace Habanero.Test.Migrations
{
    [Migration(4)]
    public class Migration_4_CreateOrderItemTable : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("orderitem")
                  .WithColumn("OrderNumber").AsInt64().NotNullable().WithDefaultValue(0).PrimaryKey()
                  .WithColumn("Product").AsAnsiString(100).NotNullable().WithDefaultValue("").PrimaryKey();
        }

    }
}