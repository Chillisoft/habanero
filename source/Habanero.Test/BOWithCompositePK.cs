using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.Test
{
    public class BOWithCompositePK : BusinessObject
    {
        private const string PK1_PROP1_NAME = "PK1Prop1";
        private const string PK1_PROP2_NAME = "PK1Prop2";



        public string PK1Prop1
        {
            get { return GetPropertyValueString(PK1_PROP1_NAME); }
            set { SetPropertyValue(PK1_PROP1_NAME, value); }
        }

        public string PK1Prop2
        {
            get { return GetPropertyValueString(PK1_PROP2_NAME); }
            set { SetPropertyValue(PK1_PROP2_NAME, value); }
        }

        public static void LoadClassDefs()
        {
            CreateClassDef();
        }

        protected override IClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        private static IClassDef GetClassDef()
        {
            if (ClassDef.IsDefined(typeof(BOWithCompositePK)))
            {
                return ClassDef.Get<BOWithCompositePK>();
            }
            return CreateClassDef();
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            var keysCol = new KeyDefCol();
            var primaryKey = new PrimaryKeyDef { IsGuidObjectID = false };
            primaryKey.Add(lPropDefCol[PK1_PROP1_NAME]);
            primaryKey.Add(lPropDefCol[PK1_PROP2_NAME]);

            var relDefs = new RelationshipDefCol();
            var lClassDef =
                new ClassDef(typeof(BOWithCompositePK), primaryKey, lPropDefCol, keysCol, relDefs);

            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }


        protected static PropDefCol CreateBOPropDef()
        {
            var lPropDefCol = new PropDefCol();

            var propDef = new PropDef(PK1_PROP1_NAME, typeof (string), PropReadWriteRule.ReadWrite, "PK1_Prop1", null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef(PK1_PROP2_NAME, typeof(string), PropReadWriteRule.ReadWrite, "PK1_Prop2", null);
            lPropDefCol.Add(propDef);

            return lPropDefCol;
        }
    }
}