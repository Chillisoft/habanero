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
    /// <summary>
    /// Summary description for FilledCircle.
    /// </summary>
    public class FilledCircle : Circle
    {

        public new static IClassDef GetClassDef()
        {
            return !ClassDef.IsDefined(typeof (FilledCircle)) ? CreateClassDef() : ClassDef.ClassDefs[typeof (FilledCircle)];
        }

        protected override IClassDef ConstructClassDef()
        {
            _classDef = (ClassDef) GetClassDef();
            return _classDef;
        }

        private new static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            IPropDef propDef =
                new PropDef("Colour", typeof(int), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);
            propDef = lPropDefCol.Add("FilledCircleID", typeof(Guid), PropReadWriteRule.WriteOnce, "FilledCircleID_field", null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["FilledCircleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof (FilledCircle), primaryKey, "FilledCircle_table", lPropDefCol, keysCol, relDefCol);
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

        public static IClassDef GetClassDefWithClassInheritanceHierarchy()
        {
            IClassDef shapeClassDef = Shape.GetClassDef();
            IClassDef circleClassDef = Circle.GetClassDef();
            circleClassDef.SuperClassDef = new SuperClassDef(shapeClassDef, ORMapping.ClassTableInheritance);
            IClassDef filledCircleClassDef = GetClassDef();
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

        public static IClassDef GetClassDefWithConcreteInheritanceHierarchy()
        {
            IClassDef circleClassDef = Circle.GetClassDefWithConcreteTableInheritance();
            IClassDef filledCircleClassDef = GetClassDef();
            filledCircleClassDef.TableName = "filledcircle_concrete";
            filledCircleClassDef.SuperClassDef = new SuperClassDef(circleClassDef, ORMapping.ConcreteTableInheritance);
            return filledCircleClassDef;
        }
    }
}