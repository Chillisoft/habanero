using System;
using FluentMigrator;

namespace Habanero.Test.Migrations
{
    [Migration(2)]
    public class Migration_2_InitialSampleData : FluentMigrator.Migration
    {
        public override void Up()
        {
            const string databaseLookupInt = "database_lookup_int";

            Insert.IntoTable(databaseLookupInt).Row(new { LookupID = 1, LookupValue = "TestInt1" });
            Insert.IntoTable(databaseLookupInt).Row(new { LookupID = 7, LookupValue = "TestInt7" });

            const string databaseLookupGuid = "database_lookup_guid";

            Insert.IntoTable(databaseLookupGuid).Row(new { LookupID = new Guid("6EAE79DD-11A8-4f31-8AF5-A08F22FE556E"), LookupValue = "test2" });
            Insert.IntoTable(databaseLookupGuid).Row(new { LookupID = new Guid("831B3C35-5842-484b-BEC9-CE24CCE05AC2"), LookupValue = "test1" });
        }

        public override void Down()
        {

        }

    }
}