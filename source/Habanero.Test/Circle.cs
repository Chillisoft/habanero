//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.BO.ClassDefinition;

namespace Habanero.Test
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
            
            lClassDef.SuperClassDef = new SuperClassDef(Shape.GetClassDef(), ORMapping.ClassTableInheritance);

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
    }
}