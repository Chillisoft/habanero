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
using Habanero.BO.ClassDefinition;

namespace Habanero.Test
{
    /// <summary>
    /// FilledCircle with no primary key, used for special forms of inheritance, eg SingleTableInheritance
    /// </summary>
    public class FilledCircleNoPrimaryKey : CircleNoPrimaryKey
    {

        public new static IClassDef GetClassDef()
        {
            if (ClassDef.IsDefined(typeof (FilledCircleNoPrimaryKey)))
            {
                return ClassDef.ClassDefs[typeof (FilledCircleNoPrimaryKey)];
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
                new PropDef("Colour", typeof(int), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            //ClassDef lClassDef = new ClassDef(typeof (FilledCircleNoPrimaryKey), null, lPropDefCol, keysCol, relDefCol);
            ClassDef lClassDef = new ClassDef(typeof(FilledCircleNoPrimaryKey), null, "FilledCircle_table", lPropDefCol, keysCol, relDefCol, null);
            lClassDef.SuperClassDef = new SuperClassDef(Circle.GetClassDef(), ORMapping.ConcreteTableInheritance);
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        public Int32? Colour
        {
            get { return (Int32?)GetPropertyValue("Colour"); }
            set { SetPropertyValue("Colour", value); }
        }

        public static IClassDef GetClassDefWithSingleInheritanceHierarchy()
        {
            IClassDef shapeClassDef = Shape.GetClassDef();
            shapeClassDef.PropDefcol.Add(new PropDef("ShapeType_field", typeof(string), PropReadWriteRule.WriteOnce, "ShapeType_field", null));
            IClassDef circleClassDef = CircleNoPrimaryKey.GetClassDef();
            circleClassDef.SuperClassDef = new SuperClassDef(shapeClassDef, ORMapping.SingleTableInheritance);
            circleClassDef.SuperClassDef.Discriminator = "ShapeType_field";
            IClassDef filledCircleClassDef = GetClassDef();
            filledCircleClassDef.SuperClassDef = new SuperClassDef(circleClassDef, ORMapping.SingleTableInheritance);
            filledCircleClassDef.SuperClassDef.Discriminator = "ShapeType_field";
            return filledCircleClassDef;
        }
        public static IClassDef GetClassDefWithSingleInheritanceHierarchy_NonPersistableProp(string nonPersistablePropertyName)
        {
            var filledCircleClassDef = GetClassDefWithSingleInheritanceHierarchy();
            var nonPersistablePropDef = new PropDef(nonPersistablePropertyName, typeof(string), PropReadWriteRule.ReadWrite,
                                                   "");
            nonPersistablePropDef.Persistable = false;
            filledCircleClassDef.SuperClassClassDef.PropDefcol.Add(nonPersistablePropDef);
            return filledCircleClassDef;
        }

        public static IClassDef GetClassDefWithSingleInheritanceHierarchyDifferentDiscriminators()
        {
            return GetClassDefWithSingleInheritanceHierarchy("CircleType_field");
        }

        private static IClassDef GetClassDefWithSingleInheritanceHierarchy(string filledCircleDiscriminator)
        {
            IClassDef shapeClassDef = Shape.GetClassDef();
            shapeClassDef.PropDefcol.Add(new PropDef("ShapeType_field", typeof(string), PropReadWriteRule.WriteOnce, "ShapeType_field", null));
            IClassDef circleClassDef = CircleNoPrimaryKey.GetClassDef();
            circleClassDef.SuperClassDef = new SuperClassDef(shapeClassDef, ORMapping.SingleTableInheritance);
            circleClassDef.SuperClassDef.Discriminator = "ShapeType_field";
            circleClassDef.PropDefcol.Add(new PropDef(filledCircleDiscriminator, typeof(string), PropReadWriteRule.WriteOnce, filledCircleDiscriminator, null));
            IClassDef filledCircleClassDef = GetClassDef();
            filledCircleClassDef.SuperClassDef = new SuperClassDef(circleClassDef, ORMapping.SingleTableInheritance);
            filledCircleClassDef.SuperClassDef.Discriminator = filledCircleDiscriminator;
            return filledCircleClassDef;
        }

        public static FilledCircleNoPrimaryKey CreateSavedFilledCircle()
        {

            FilledCircleNoPrimaryKey filledCircle = new FilledCircleNoPrimaryKey();
            filledCircle.ShapeName = Guid.NewGuid().ToString();
            filledCircle.Colour = 1;
            filledCircle.Radius = 10;
            filledCircle.Save();
            return filledCircle;

        }


    }
}