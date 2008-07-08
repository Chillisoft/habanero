//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for FilledCircle.
    /// </summary>
    public class FilledCircle : Circle
    {

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (FilledCircle)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (FilledCircle)];
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
                new PropDef("Colour", typeof (int), PropReadWriteRule.ReadWrite, "Colour", null);
            lPropDefCol.Add(propDef);
            propDef = lPropDefCol.Add("FilledCircleID", typeof (Guid), PropReadWriteRule.WriteOnce, null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["FilledCircleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof (FilledCircle), primaryKey, lPropDefCol, keysCol, relDefCol);
            lClassDef.SuperClassDef = new SuperClassDef(Circle.GetClassDef(), ORMapping.ConcreteTableInheritance);
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        public Guid? FilledCircleID
        {
            get { return (Guid?)GetPropertyValue("FilledCircleID"); }
            set { SetPropertyValue("FilledCircleID", value); }
        }

        public Int32? Colour
        {
            get { return (Int32?)GetPropertyValue("Colour"); }
            set { SetPropertyValue("Colour", value); }
        }

        public static ClassDef GetClassDefWithClassInheritanceHierarchy()
        {
            ClassDef shapeClassDef = Shape.GetClassDef();
            ClassDef circleClassDef = Circle.GetClassDef();
            circleClassDef.SuperClassDef = new SuperClassDef(shapeClassDef, ORMapping.ClassTableInheritance);
            ClassDef filledCircleClassDef = GetClassDef();
            filledCircleClassDef.SuperClassDef = new SuperClassDef(circleClassDef, ORMapping.ClassTableInheritance);
            return filledCircleClassDef;
        }

        public static FilledCircle CreateSavedFilledCircle()
        {

            FilledCircle filledCircle = new FilledCircle();
            filledCircle.ShapeName = Guid.NewGuid().ToString();
            filledCircle.Colour = 1;
            filledCircle.Radius = 10;
            filledCircle.Save();
            return filledCircle;

        }

        public static ClassDef GetClassDefWithConcreteInheritanceHierarchy()
        {
            ClassDef circleClassDef = Circle.GetClassDefWithConcreteTableInheritance();
            ClassDef filledCircleClassDef = GetClassDef();
            filledCircleClassDef.TableName = "filledcircle_concrete";
            filledCircleClassDef.SuperClassDef = new SuperClassDef(circleClassDef, ORMapping.ConcreteTableInheritance);
            return filledCircleClassDef;
        }
    }
}