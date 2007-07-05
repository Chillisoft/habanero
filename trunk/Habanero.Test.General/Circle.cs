using System;
using Habanero.Bo.ClassDefinition;

namespace Habanero.Test.General
{
    public class Circle : Shape
    {

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Circle)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (Circle)];
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
                new PropDef("Radius", typeof (int), PropReadWriteRule.ReadWrite, "Radius", null);
            lPropDefCol.Add(propDef);
            propDef = lPropDefCol.Add("CircleID", typeof (Guid), PropReadWriteRule.WriteOnce, null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["CircleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof (Circle), primaryKey, lPropDefCol, keysCol, relDefCol);
            lClassDef.SuperClassDef = new SuperClassDef(Shape.GetClassDef(), ORMapping.ConcreteTableInheritance);
			ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }
    }
}