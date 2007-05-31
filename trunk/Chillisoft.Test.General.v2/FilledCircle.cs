using System;
using Chillisoft.Bo.ClassDefinition.v2;

namespace Chillisoft.Test.General.v2
{
    /// <summary>
    /// Summary description for FilledCircle.
    /// </summary>
    public class FilledCircle : Circle
    {
        public static FilledCircle GetNewObject()
        {
            FilledCircle obj = new FilledCircle();
            AddToLoadedBusinessObjectCol(obj);
            return obj;
        }

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (FilledCircle)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.GetClassDefCol()[typeof (FilledCircle)];
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
                new PropDef("Colour", typeof (int), cbsPropReadWriteRule.ReadManyWriteMany, "Colour", null);
            lPropDefCol.Add(propDef);
            propDef = lPropDefCol.Add("FilledCircleID", typeof (Guid), cbsPropReadWriteRule.ReadManyWriteOnce, null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["FilledCircleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof (FilledCircle), primaryKey, lPropDefCol, keysCol, relDefCol);
            lClassDef.SuperClassDesc = new SuperClassDesc(Circle.GetClassDef(), ORMapping.ConcreteTableInheritance);
            return lClassDef;
        }
    }
}