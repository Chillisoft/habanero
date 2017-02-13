using System;
using FluentMigrator;

namespace Habanero.Test.Migrations
{
    [Migration(1)]
    public class Migration_1_InitialDataStructure : FluentMigrator.Migration
    {
        public override void Up()
        {
            #region Create Tables
            Create.Table("another_number_generator").InSchema("dbo")
                  .WithColumn("SequenceNumber").AsInt64().NotNullable()
                  .WithColumn("NumberType").AsAnsiString(45).NotNullable().PrimaryKey()
                  .WithColumn("UserLocked").AsAnsiString(45).Nullable()
                  .WithColumn("Locked").AsInt16().Nullable()
                  .WithColumn("MachineLocked").AsAnsiString(45).Nullable()
                  .WithColumn("OperatingSystemUserLocked").AsAnsiString(45).Nullable()
                  .WithColumn("DateTimeLocked").AsDateTime().Nullable();

            Create.Table("asset").InSchema("dbo")
                  .WithColumn("AssetID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("ParentAssetID").AsFixedLengthAnsiString(38).Nullable();

            Create.Table("bowithintid").InSchema("dbo")
                  .WithColumn("IntID").AsInt64().NotNullable().PrimaryKey()//.Identity()
                  .WithColumn("TestField").AsAnsiString(45).NotNullable();

            Create.Table("car_table").InSchema("dbo")
                  .WithColumn("Car_ID").AsAnsiString(38).Nullable()
                  .WithColumn("Owner_Id").AsAnsiString(38).Nullable()
                  .WithColumn("Car_Reg_No").AsAnsiString(50).Nullable()
                  .WithColumn("Driver_FK1").AsAnsiString(50).Nullable()
                  .WithColumn("Driver_FK2").AsAnsiString(50).Nullable();

            Create.Table("circle_concrete").InSchema("dbo")
                  .WithColumn("CircleID_field").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("Radius").AsInt64().Nullable()
                  .WithColumn("ShapeID_field").AsFixedLengthAnsiString(38).Nullable()
                  .WithColumn("ShapeName").AsAnsiString(255).Nullable();

            Create.Table("circle_table").InSchema("dbo")
                  .WithColumn("CircleID_field").AsFixedLengthAnsiString(38).Nullable()
                  .WithColumn("Radius").AsInt64().NotNullable().WithDefaultValue(0)
                  .WithColumn("ShapeID_field").AsFixedLengthAnsiString(38).Nullable()
                  .WithColumn("Colour").AsInt64().NotNullable().WithDefaultValue(0);

            Create.Table("organisation").InSchema("dbo")
                  .WithColumn("OrganisationID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("Name").AsAnsiString(255).Nullable();

            CreateContactPersonTable(this);

            CreateContactPersonAddressTable(this);

            Create.Table("contactpersoncompositekey").InSchema("dbo")
                  .WithColumn("PK1_Prop1").AsAnsiString(50).Nullable()
                  .WithColumn("PK1_Prop2").AsAnsiString(50).Nullable()
                  .WithColumn("Surname").AsAnsiString(50).Nullable()
                  .WithColumn("FirstName").AsAnsiString(50).Nullable()
                  .WithColumn("DateOfBirth").AsDateTime().Nullable()
                  .WithColumn("DateLastUpdated").AsDateTime().Nullable()
                  .WithColumn("UserLastUpdated").AsAnsiString(50).Nullable()
                  .WithColumn("MachineLastUpdated").AsAnsiString(50).Nullable()
                  .WithColumn("VersionNumber").AsInt32().Nullable();

            Create.Table("database_lookup_guid").InSchema("dbo")
                  .WithColumn("LookupID").AsAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("LookupValue").AsAnsiString(45).NotNullable();


            Create.Table("database_lookup_int").InSchema("dbo")
                  .WithColumn("LookupID").AsInt64().NotNullable().PrimaryKey()
                  .WithColumn("LookupValue").AsAnsiString(45).NotNullable();

            Create.Table("filledcircle_concrete").InSchema("dbo")
                  .WithColumn("FilledCircleID_field").AsFixedLengthString(38).NotNullable().PrimaryKey()
                  .WithColumn("Colour").AsInt64().Nullable()
                  .WithColumn("ShapeID_field").AsFixedLengthString(38).Nullable()
                  .WithColumn("CircleID_field").AsFixedLengthString(38).Nullable()
                  .WithColumn("Radius").AsInt64().Nullable()
                  .WithColumn("ShapeName").AsString(255).Nullable();

            Create.Table("filledcircle_table").InSchema("dbo")
                  .WithColumn("FilledCircleID_field").AsFixedLengthAnsiString(38).Nullable()
                  .WithColumn("Colour").AsInt64().NotNullable()
                  .WithColumn("ShapeID_field").AsFixedLengthAnsiString(38).Nullable()
                  .WithColumn("CircleID_field").AsFixedLengthAnsiString(38).Nullable();

            Create.Table("mockbo").InSchema("dbo")
                  .WithColumn("MockBOID").AsAnsiString(38).Nullable()
                  .WithColumn("MockBOProp1").AsAnsiString(38).Nullable()
                  .WithColumn("MockBOProp2").AsAnsiString(50).Nullable();

            Create.Table("mybo").InSchema("dbo")
                  .WithColumn("MyBoID").AsAnsiString(255).NotNullable().PrimaryKey()
                  .WithColumn("TestProp").AsAnsiString(45).Nullable()
                  .WithColumn("TestProp2").AsAnsiString(45).Nullable()
                  .WithColumn("ShapeID").AsFixedLengthAnsiString(38).Nullable()
                  .WithColumn("ByteArrayProp").AsBinary(-1).Nullable();

            Create.Table("numbergenerator").InSchema("dbo")
                  .WithColumn("SequenceNumber").AsInt64().NotNullable()
                  .WithColumn("NumberType").AsAnsiString(45).NotNullable().PrimaryKey()
                  .WithColumn("UserLocked").AsAnsiString(45).Nullable()
                  .WithColumn("Locked").AsInt16().Nullable()
                  .WithColumn("MachineLocked").AsAnsiString(45).Nullable()
                  .WithColumn("OperatingSystemUserLocked").AsAnsiString(45).Nullable()
                  .WithColumn("DateTimeLocked").AsDateTime().Nullable();


            Create.Table("shape_table").InSchema("dbo")
                  .WithColumn("ShapeID_field").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey().WithDefaultValue("")
                  .WithColumn("ShapeName").AsAnsiString(45).Nullable()
                  .WithColumn("ShapeType_field").AsAnsiString(45).Nullable()
                  .WithColumn("Radius").AsInt64().NotNullable().WithDefaultValue(0)
                  .WithColumn("Colour").AsInt64().NotNullable().WithDefaultValue(0)
                  .WithColumn("CircleType_field").AsAnsiString(45).Nullable();

            Create.Table("stubdatabasetransaction").InSchema("dbo")
                  .WithColumn("id").AsAnsiString(255).Nullable()
                  .WithColumn("name").AsAnsiString(255).Nullable();

            Create.Table("table_class_car").InSchema("dbo")
                  .WithColumn("field_Car_ID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("field_Registration_No").AsAnsiString(50).Nullable()
                  .WithColumn("field_Length").AsInt16().Nullable()
                  .WithColumn("field_Is_Convertible").AsInt16().Nullable()
                  .WithColumn("field_Driver_ID").AsFixedLengthAnsiString(38).Nullable();

            Create.Table("table_class_engine").InSchema("dbo")
                  .WithColumn("field_Engine_ID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("field_Engine_No").AsAnsiString(50).Nullable()
                  .WithColumn("field_Date_Manufactured").AsDateTime().Nullable()
                  .WithColumn("field_Horse_Power").AsAnsiString(50).Nullable()
                  .WithColumn("field_Fue_lInjected").AsInt16().Nullable()
                  .WithColumn("field_Car_ID").AsFixedLengthAnsiString(38).Nullable();

            Create.Table("table_class_legalentity").InSchema("dbo")
                  .WithColumn("field_Legal_Entity_ID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("field_Legal_Entity_Type").AsAnsiString(50).Nullable();

            Create.Table("table_class_organisation").InSchema("dbo")
                  .WithColumn("field_Name").AsAnsiString(50).Nullable()
                  .WithColumn("field_Date_Formed").AsAnsiString(50).Nullable()
                  .WithColumn("field_Organisation_ID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey();

            Create.Table("table_class_part").InSchema("dbo")
                  .WithColumn("field_Part_ID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("field_Model_No").AsAnsiString(50).Nullable()
                  .WithColumn("field_Part_Type").AsAnsiString(50).Nullable();

            Create.Table("table_class_person").InSchema("dbo")
                  .WithColumn("field_ID_Number").AsAnsiString(50).Nullable()
                  .WithColumn("field_First_Name").AsAnsiString(50).Nullable()
                  .WithColumn("field_Last_Name").AsAnsiString(50).Nullable()
                  .WithColumn("field_Person_ID").AsAnsiString(50).NotNullable().PrimaryKey();

            Create.Table("table_class_vehicle").InSchema("dbo")
                  .WithColumn("field_Vehicle_ID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("field_Vehicle_Type").AsAnsiString(50).Nullable()
                  .WithColumn("field_Date_Assembled").AsDateTime().Nullable()
                  .WithColumn("field_Owner_ID").AsFixedLengthAnsiString(38).Nullable();

            Create.Table("table_engine").InSchema("dbo")
                  .WithColumn("ENGINE_ID").AsAnsiString(38).Nullable()
                  .WithColumn("CAR_ID").AsAnsiString(38).Nullable()
                  .WithColumn("ENGINE_NO").AsAnsiString(50).Nullable();

            Create.Table("table_entity").InSchema("dbo")
                  .WithColumn("field_Entity_ID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("field_Entity_Type").AsAnsiString(50).Nullable();

            Create.Table("table_organisationperson").InSchema("dbo")
                  .WithColumn("field_Organisatiion_ID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("field_Person_ID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                  .WithColumn("field_Relationship").AsAnsiString(50).Nullable();

            Create.Table("testautoinc").InSchema("dbo")
                  .WithColumn("testautoincid").AsInt64().Identity().NotNullable().PrimaryKey()
                  .WithColumn("testfield").AsAnsiString(45).NotNullable();

            Create.Table("testtableread").InSchema("dbo")
                  .WithColumn("TestTableReadData").AsAnsiString(50).Nullable()
                  .WithColumn("TestTableReadID").AsAnsiString(38).Nullable();

            Create.Table("transactionlog").InSchema("dbo")
                  .WithColumn("TransactionSequenceNo").AsInt32().Identity().NotNullable().PrimaryKey()
                  .WithColumn("DateTimeUpdated").AsDateTime().Nullable()
                  .WithColumn("WindowsUser").AsAnsiString(50).Nullable()
                  .WithColumn("LogonUser").AsAnsiString(50).Nullable()
                  .WithColumn("MachineName").AsAnsiString(50).Nullable()
                  .WithColumn("BusinessObjectTypeName").AsAnsiString(50).Nullable()
                  .WithColumn("CRUDAction").AsAnsiString(50).Nullable()
                  .WithColumn("DirtyXML").AsAnsiString(Int32.MaxValue).Nullable()
                  .WithColumn("BusinessObjectToString").AsAnsiString(255).Nullable();
            #endregion

            #region Create Foreign Keys
            Create.ForeignKey("FK_asset_1")
                  .FromTable("asset").InSchema("dbo").ForeignColumns("ParentAssetID")
                  .ToTable("asset").InSchema("dbo").PrimaryColumns("AssetID");

            Create.ForeignKey("FK_circle_concrete_1")
                  .FromTable("circle_concrete").InSchema("dbo").ForeignColumns("ShapeID_field")
                  .ToTable("shape_table").InSchema("dbo").PrimaryColumns("ShapeID_field");
            
            Create.ForeignKey("table_class_Engine_Car_FK")
                  .FromTable("table_class_engine").InSchema("dbo").ForeignColumns("field_Car_ID")
                  .ToTable("table_class_car").InSchema("dbo").PrimaryColumns("field_Car_ID");

            Create.ForeignKey("table_class_Vehicle_Owner_FK")
                  .FromTable("table_class_vehicle").InSchema("dbo").ForeignColumns("field_Owner_ID")
                  .ToTable("table_class_legalentity").InSchema("dbo").PrimaryColumns("field_Legal_Entity_ID");

            Create.ForeignKey("table_OrganisationPerson_Organisation_FK")
                  .FromTable("table_organisationperson").InSchema("dbo").ForeignColumns("field_Organisatiion_ID")
                  .ToTable("table_class_organisation").InSchema("dbo").PrimaryColumns("field_Organisation_ID");
            #endregion

            #region Create Unique Constraints
            Create.UniqueConstraint("contact_person$Index_2")
                  .OnTable("contact_person").WithSchema("dbo")
                  .Columns("Surname_field,FirstName_field".Split(','));

            Create.UniqueConstraint("testtableread$Index_1")
                  .OnTable("testtableread").WithSchema("dbo")
                  .Column("TestTableReadData");
            #endregion

            #region Create Indexes
            //Create.Index("FK_asset_1")
            //      .OnTable("asset").InSchema("dbo")
            //      .OnColumn("ParentAssetID").Ascending()
            //      .WithOptions()
            //      .NonClustered();

            Create.Index("Index_2")
                  .OnTable("circle_concrete").InSchema("dbo")
                  .OnColumn("ShapeID_field").Ascending()
                  .WithOptions()
                  .NonClustered();

            //Create.Index("FK_contact_person_1")
            //      .OnTable("contact_person").InSchema("dbo")
            //      .OnColumn("OrganisationID").Ascending()
            //      .WithOptions()
            //      .NonClustered();

            Create.Index("table_class_Car_Driver_FK")
                  .OnTable("table_class_car").InSchema("dbo")
                  .OnColumn("field_Driver_ID").Ascending()
                  .WithOptions()
                  .NonClustered();
            
            //Create.Index("table_class_Engine_Car_FK")
            //      .OnTable("table_class_engine").InSchema("dbo")
            //      .OnColumn("field_Car_ID").Ascending()
            //      .WithOptions()
            //      .NonClustered();

            //Create.Index("table_class_Vehicle_Owner_FK")
            //      .OnTable("table_class_vehicle").InSchema("dbo")
            //      .OnColumn("field_Owner_ID").Ascending()
            //      .WithOptions()
            //      .NonClustered();

            //Create.Index("table_OrganisationPerson_Person_FK")
            //      .OnTable("table_organisationperson").InSchema("dbo")
            //      .OnColumn("field_Person_ID").Ascending()
            //      .WithOptions()
            //      .NonClustered();
            #endregion

        }

        public static void CreateContactPersonTable(MigrationBase migrationBase, string tableName = "contact_person")
        {
            migrationBase.Create.Table(tableName).InSchema("dbo")
                                .WithColumn("ContactPersonID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                                .WithColumn("Surname_field").AsAnsiString(255).Nullable()
                                .WithColumn("FirstName_field").AsAnsiString(255).Nullable()
                                .WithColumn("EmailAddress").AsAnsiString(255).Nullable()
                                .WithColumn("PhoneNumber").AsAnsiString(255).Nullable()
                                .WithColumn("CellNumber").AsAnsiString(255).Nullable()
                                .WithColumn("DateOfBirth").AsDateTime().Nullable()
                                .WithColumn("DateLastUpdated").AsDateTime().Nullable()
                                .WithColumn("UserLastUpdated").AsAnsiString(255).Nullable()
                                .WithColumn("MachineLastUpdated").AsAnsiString(255).Nullable()
                                .WithColumn("VersionNumber").AsInt32().Nullable()
                                .WithColumn("PK2_Prop1").AsAnsiString(255).Nullable()
                                .WithColumn("PK2_Prop2").AsAnsiString(255).Nullable()
                                .WithColumn("PK3_Prop").AsAnsiString(255).Nullable()
                                .WithColumn("OrganisationID").AsFixedLengthAnsiString(38).Nullable()
                                .WithColumn("UserLocked").AsAnsiString(45).Nullable()
                                .WithColumn("DateTimeLocked").AsDateTime().Nullable()
                                .WithColumn("MachineLocked").AsAnsiString(45).Nullable()
                                .WithColumn("OperatingSystemUserLocked").AsAnsiString(45).Nullable()
                                .WithColumn("Locked").AsInt16().Nullable()
                                .WithColumn("IntegerProperty").AsInt32().Nullable();

            migrationBase.Create.ForeignKey("FK_" + tableName + "_1")
                  .FromTable(tableName).InSchema("dbo").ForeignColumns("OrganisationID")
                  .ToTable("organisation").InSchema("dbo").PrimaryColumns("OrganisationID");
        }

        public static void CreateContactPersonAddressTable(MigrationBase migrationBase, string tableName = "contact_person_address", string referencedContactPersonTableName = "contact_person")
        {
            migrationBase.Create.Table(tableName).InSchema("dbo")
                         .WithColumn("AddressID").AsFixedLengthAnsiString(38).NotNullable().PrimaryKey()
                         .WithColumn("ContactPersonID").AsFixedLengthAnsiString(38).NotNullable()
                         .WithColumn("AddressLine1").AsAnsiString(255).Nullable()
                         .WithColumn("AddressLine2").AsAnsiString(255).Nullable()
                         .WithColumn("AddressLine3").AsAnsiString(255).Nullable()
                         .WithColumn("AddressLine4").AsAnsiString(255).Nullable()
                         .WithColumn("OrganisationID").AsFixedLengthAnsiString(38).Nullable();

            migrationBase.Create.ForeignKey("FK_" + tableName + "_1")
                  .FromTable(tableName).InSchema("dbo").ForeignColumns("ContactPersonID")
                  .ToTable(referencedContactPersonTableName).InSchema("dbo").PrimaryColumns("ContactPersonID");

            //Create.Index("FK_" + tableName + "_1")
            //      .OnTable(tableName).InSchema("dbo")
            //      .OnColumn("ContactPersonID").Ascending()
            //      .WithOptions()
            //      .NonClustered();
            
        }

        public override void Down()
        {
            #region Delete Unique Constraints
            Delete.UniqueConstraint("contact_person$Index_2").FromTable("contact_person").InSchema("dbo");
            Delete.UniqueConstraint("testtableread$Index_1").FromTable("testtableread").InSchema("dbo");
            #endregion

            #region Delete Foreign Keys
            Delete.ForeignKey("FK_asset_1").OnTable("asset").InSchema("dbo");
            Delete.ForeignKey("FK_circle_concrete_1").OnTable("circle_concrete").InSchema("dbo");
            Delete.ForeignKey("FK_contact_person_1").OnTable("contact_person").InSchema("dbo");
            Delete.ForeignKey("FK_contact_person_address_1").OnTable("contact_person_address").InSchema("dbo");
            Delete.ForeignKey("table_class_Engine_Car_FK").OnTable("table_class_engine").InSchema("dbo");
            Delete.ForeignKey("table_class_Vehicle_Owner_FK").OnTable("table_class_vehicle").InSchema("dbo");
            Delete.ForeignKey("table_OrganisationPerson_Organisation_FK").OnTable("table_organisationperson").InSchema("dbo");
            #endregion

            #region Delete Tables
            Delete.Table("another_number_generator").InSchema("dbo");
            Delete.Table("asset").InSchema("dbo");
            Delete.Table("bowithintid").InSchema("dbo");
            Delete.Table("car_table").InSchema("dbo");
            Delete.Table("circle_concrete").InSchema("dbo");
            Delete.Table("circle_table").InSchema("dbo");
            Delete.Table("contact_person").InSchema("dbo");
            Delete.Table("contact_person_address").InSchema("dbo");
            Delete.Table("contactpersoncompositekey").InSchema("dbo");
            Delete.Table("database_lookup_guid").InSchema("dbo");
            Delete.Table("database_lookup_int").InSchema("dbo");
            Delete.Table("filledcircle_concrete").InSchema("dbo");
            Delete.Table("filledcircle_table").InSchema("dbo");
            Delete.Table("mockbo").InSchema("dbo");
            Delete.Table("mybo").InSchema("dbo");
            Delete.Table("numbergenerator").InSchema("dbo");
            Delete.Table("organisation").InSchema("dbo");
            Delete.Table("shape_table").InSchema("dbo");
            Delete.Table("stubdatabasetransaction").InSchema("dbo");
            Delete.Table("table_class_car").InSchema("dbo");
            Delete.Table("table_class_engine").InSchema("dbo");
            Delete.Table("table_class_legalentity").InSchema("dbo");
            Delete.Table("table_class_organisation").InSchema("dbo");
            Delete.Table("table_class_part").InSchema("dbo");
            Delete.Table("table_class_person").InSchema("dbo");
            Delete.Table("table_class_vehicle").InSchema("dbo");
            Delete.Table("table_engine").InSchema("dbo");
            Delete.Table("table_entity").InSchema("dbo");
            Delete.Table("table_organisationperson").InSchema("dbo");
            Delete.Table("testautoinc").InSchema("dbo");
            Delete.Table("testtableread").InSchema("dbo");
            Delete.Table("transactionlog").InSchema("dbo");
            #endregion

            #region Delete Schemas
            #endregion
        }

    }
}