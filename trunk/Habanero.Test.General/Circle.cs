using System;
using Habanero.Bo.ClassDefinition;

namespace Habanero.Test.General
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
                new PropDef("Radius", typeof (int), PropReadWriteRule.ReadManyWriteMany, "Radius", null);
            lPropDefCol.Add(propDef);
            propDef = lPropDefCol.Add("CircleID", typeof (Guid), PropReadWriteRule.ReadManyWriteOnce, null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["CircleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof (Circle), primaryKey, lPropDefCol, keysCol, relDefCol);
            lClassDef.SuperClassDef = new SuperClassDef(Shape.GetClassDef(), ORMapping.ConcreteTableInheritance);
			ClassDef.GetClassDefCol.Add(lClassDef);
            return lClassDef;
        }
    }
}