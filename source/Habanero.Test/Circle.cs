// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.Test
{
    public class Circle : Shape
    {

        public new static IClassDef GetClassDef()
        {
            if (ClassDef.IsDefined(typeof (Circle)))
            {
                return ClassDef.ClassDefs[typeof (Circle)];
            }
            return CreateClassDef();
        }

        protected override IClassDef ConstructClassDef()
        {
            _classDef = (ClassDef) GetClassDef();
            return _classDef;
        }

        private new static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("Radius", typeof (int), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);
            lPropDefCol.Add("CircleID", typeof(Guid), PropReadWriteRule.WriteOnce, "CircleID_field", null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["CircleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof (Circle), primaryKey,  lPropDefCol, keysCol, relDefCol)
                                     {
                                         SuperClassDef =
                                             new SuperClassDef(Shape.GetClassDef(), ORMapping.ClassTableInheritance)
                                     };
            lClassDef.TableName = "circle_table";
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        public Guid? CircleID
        {
            get { return (Guid?)GetPropertyValue("CircleID"); }
            set { SetPropertyValue("CircleID", value); }
        }

        public int Radius
        {
            get { return (int)GetPropertyValue("Radius"); }
            set { SetPropertyValue("Radius", value); }
        }

        public static IClassDef GetClassDefWithConcreteTableInheritance()
        {

            IClassDef shapeClassDef = Shape.GetClassDef();
            IClassDef circleClassDef = GetClassDef();
            circleClassDef.TableName = "circle_concrete";
            circleClassDef.SuperClassDef = new SuperClassDef(shapeClassDef, ORMapping.ConcreteTableInheritance);
            return circleClassDef;
        }

        public static IClassDef GetClassDefWithClassTableInheritance()
        {
            IClassDef shapeClassDef = Shape.GetClassDef();
            IClassDef circleClassDef = GetClassDef();
            SuperClassDef superClassDef = new SuperClassDef(shapeClassDef, ORMapping.ClassTableInheritance);
            circleClassDef.SuperClassDef = superClassDef;
            superClassDef.ID = "ShapeID";
            return circleClassDef;
        }

        public static IClassDef GetClassDefWithSingleTableInheritance()
        {
            IClassDef shapeClassDef = Shape.GetClassDef();
            IClassDef circleClassDef = GetClassDef();
            SuperClassDef superClassDef = new SuperClassDef(shapeClassDef, ORMapping.SingleTableInheritance);
            circleClassDef.SuperClassDef = superClassDef;
            superClassDef.Discriminator = "ShapeType";
            return circleClassDef;
        }

        public static Circle CreateSavedCircle()
        {
            Circle circle = new Circle();
            circle.Radius = 10;
            circle.ShapeName = Guid.NewGuid().ToString();
            circle.Save();
            return circle;
        }
    }
}