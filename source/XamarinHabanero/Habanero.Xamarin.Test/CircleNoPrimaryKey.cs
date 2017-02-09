#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.Test
{
    public class CircleNoPrimaryKey : Shape
    {

        public new static IClassDef GetClassDef()
        {
            return ClassDef.IsDefined(typeof (CircleNoPrimaryKey)) ? ClassDef.ClassDefs[typeof (CircleNoPrimaryKey)] : CreateClassDef();
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
                new PropDef("Radius", typeof(int), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);
            //propDef = new PropDef("ContactPersonID", typeof(Guid), PropReadWriteRule.WriteOnce, "ContactPersonID", null);
            //lPropDefCol.Add(propDef);

            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //RelKeyDef relKeyDef = new RelKeyDef();
            //IPropDef relPropDef = lPropDefCol["ContactPersonID"];
            //RelPropDef lRelPropDef = new RelPropDef(relPropDef, "ContactPersonID");
            //relKeyDef.Add(lRelPropDef);
            //RelationshipDef relDef = new SingleRelationshipDef("ContactPerson", typeof(ContactPerson), relKeyDef, false, DeleteParentAction.DoNothing);
            //relDefCol.Add(relDef);

            ClassDef lClassDef = new ClassDef(typeof(CircleNoPrimaryKey), null, "circle_table", lPropDefCol, keysCol, relDefCol, null);
            //ClassDef lClassDef = new ClassDef(typeof(CircleNoPrimaryKey), null, lPropDefCol, keysCol, relDefCol);
            
            lClassDef.SuperClassDef = new SuperClassDef(Shape.GetClassDef(), ORMapping.ClassTableInheritance);
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        public int Radius
        {
            get { return (int)GetPropertyValue("Radius"); }
            set { SetPropertyValue("Radius", value); }
        }

        public static IClassDef GetClassDefWithSingleInheritance()
        {
            IClassDef shapeClassDef = Shape.GetClassDef();
            shapeClassDef.PropDefcol.Add(new PropDef("ShapeType_field", typeof(string), PropReadWriteRule.WriteOnce, "ShapeType_field", null));
            IClassDef circleClassDef = GetClassDef();
            circleClassDef.SuperClassDef = new SuperClassDef(shapeClassDef, ORMapping.SingleTableInheritance);
            circleClassDef.SuperClassDef.Discriminator = "ShapeType_field";
            circleClassDef.TableName = "shape_table";
            return circleClassDef;
        }

        public static CircleNoPrimaryKey CreateSavedCircle()
        {
            CircleNoPrimaryKey circle = new CircleNoPrimaryKey();
            circle.Radius = 10;
            circle.ShapeName = Guid.NewGuid().ToString();
            circle.Save();
            return circle;
        }
    }
}