using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.Test.BO
{
    public class BOWithIntID_Child : BusinessObject
    {
        public int? IntID
        {
            get
            {
                return (int?)GetPropertyValue("IntID");
            }
            set { SetPropertyValue("IntID", value); }
        }

        public string TestField
        {
            get
            {
                return GetPropertyValueString("TestField");
            }
            set { SetPropertyValue("TestField", value); }
        }

        public override string ToString()
        {
            return TestField;
        }
        private ClassDef _classDefCircleNoPrimaryKey;
        private ClassDef _classDefShape;
        private ClassDef _classDefFilledCircleNoPrimaryKey;

        protected void SetupInheritanceSpecifics()
        {
            ClassDef.ClassDefs.Clear();
            _classDefShape = Shape.GetClassDef();
            _classDefCircleNoPrimaryKey = CircleNoPrimaryKey.GetClassDef();
            _classDefCircleNoPrimaryKey.SuperClassDef = new SuperClassDef(_classDefShape,
                                                                          ORMapping.SingleTableInheritance);
            _classDefCircleNoPrimaryKey.SuperClassDef.Discriminator = "ShapeType_field";
            _classDefFilledCircleNoPrimaryKey = FilledCircleNoPrimaryKey.GetClassDef();
            _classDefFilledCircleNoPrimaryKey.SuperClassDef = new SuperClassDef(_classDefCircleNoPrimaryKey,
                                                                                ORMapping.SingleTableInheritance);
            _classDefFilledCircleNoPrimaryKey.SuperClassDef.Discriminator = "ShapeType_field";
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("SomeNewProp", typeof(int), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof(BOWithIntID_Child), null, "bowithintid", lPropDefCol, keysCol, relDefCol, null);
            return lClassDef;
        }

        public static ClassDef LoadClassDefWith_SingleTableInherit()
        {
            ClassDef itsClassDef = CreateClassDef();
            ClassDef classDef_BOWithIntID = BOWithIntID.LoadClassDefWithIntID_DiscriminatorField();
            itsClassDef.SuperClassDef = new SuperClassDef(classDef_BOWithIntID, ORMapping.SingleTableInheritance)
                                            {Discriminator = "Type_field"};
            itsClassDef.TableName = "bowithintid";
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        internal static void DeleteAllBOWithIntID()
        {
            const string sql = "DELETE FROM bowithintid";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }
    }
}