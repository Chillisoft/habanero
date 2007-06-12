using System;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;

namespace Chillisoft.Test.General.v2
{
    public class Shape : BusinessObjectBase
    {
        public static Shape GetNewObject()
        {
            Shape obj = new Shape();
            AddToLoadedBusinessObjectCol(obj);
            return obj;
        }

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Shape)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.GetClassDefCol[typeof (Shape)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            mClassDef = GetClassDef();
            return mClassDef;
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("ShapeName", typeof (String), cbsPropReadWriteRule.ReadManyWriteMany, "ShapeName", null);
            lPropDefCol.Add(propDef);
            propDef = lPropDefCol.Add("ShapeID", typeof (Guid), cbsPropReadWriteRule.ReadManyWriteOnce, null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["ShapeID"]);
            KeyDefCol keysCol = new KeyDefCol();
            KeyDef lKeyDef = new KeyDef();
            lKeyDef.Add(lPropDefCol["ShapeName"]);
            keysCol.Add(lKeyDef);
            RelKeyDef relKeyDef = new RelKeyDef();
            RelPropDef lRelPropDef = new RelPropDef(propDef, "OwnerID");
            relKeyDef.Add(lRelPropDef);
            RelationshipDef relDef = new MultipleRelationshipDef("Owner", typeof (Shape),
                                                                 relKeyDef, false, "", -1, -1,
                                                                 DeleteParentAction.cbsDereferenceRelatedObjects);
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            relDefCol.Add(relDef);
            ClassDef lClassDef = new ClassDef(typeof (Shape), primaryKey, lPropDefCol, keysCol, relDefCol);
            return lClassDef;
        }

        public string ShapeName
        {
            get { return (string) this.GetPropertyValue("ShapeName"); }
            set { this.SetPropertyValue("ShapeName", value); }
        }
    }
}