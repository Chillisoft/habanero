using System;
using Chillisoft.Bo.ClassDefinition.v2;

namespace Chillisoft.Test.General.v2
{
    public class Circle : Shape
    {
        public static Circle GetNewObject()
        {
            Circle obj = new Circle();
            AddToLoadedBusinessObjectCol(obj);
            return obj;
        }

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Circle)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.GetClassDefCol[typeof (Circle)];
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
                new PropDef("Radius", typeof (int), cbsPropReadWriteRule.ReadManyWriteMany, "Radius", null);
            lPropDefCol.Add(propDef);
            propDef = lPropDefCol.Add("CircleID", typeof (Guid), cbsPropReadWriteRule.ReadManyWriteOnce, null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["CircleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof (Circle), primaryKey, lPropDefCol, keysCol, relDefCol);
            lClassDef.SuperClassDesc = new SuperClassDesc(Shape.GetClassDef(), ORMapping.ConcreteTableInheritance);
            return lClassDef;
        }
    }
}