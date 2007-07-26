using System;
using Habanero.BO.ClassDefinition;
using Habanero.BO;

namespace Habanero.Test
{
    public class Shape : BusinessObject
    {

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Shape)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (Shape)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            _classDef = GetClassDef();
            return _classDef;
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("ShapeName", typeof (String), PropReadWriteRule.ReadWrite, "ShapeName", null);
            lPropDefCol.Add(propDef);
            propDef = new PropDef("ShapeID", typeof(Guid), PropReadWriteRule.WriteOnce, null);
            lPropDefCol.Add(propDef);
           // propDef = new PropDef("MyID", typeof(Guid), PropReadWriteRule.WriteOnce, null);
           // lPropDefCol.Add(propDef);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["ShapeID"]);
            KeyDefCol keysCol = new KeyDefCol();
            KeyDef lKeyDef = new KeyDef();
            lKeyDef.Add(lPropDefCol["ShapeName"]);
            keysCol.Add(lKeyDef);
            RelKeyDef relKeyDef = new RelKeyDef();

            //RelPropDef lRelPropDef = new RelPropDef(propDef, "OwnerID");
            //relKeyDef.Add(lRelPropDef);
            //RelationshipDef relDef = new MultipleRelationshipDef("Owner", typeof (Shape),
           //                                                      relKeyDef, false, "", DeleteParentAction.DereferenceRelated);
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            //relDefCol.Add(relDef);
            ClassDef lClassDef = new ClassDef(typeof (Shape), primaryKey, lPropDefCol, keysCol, relDefCol);
			ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        public string ShapeName
        {
            get { return (string) this.GetPropertyValue("ShapeName"); }
            set { this.SetPropertyValue("ShapeName", value); }
        }
    }
}