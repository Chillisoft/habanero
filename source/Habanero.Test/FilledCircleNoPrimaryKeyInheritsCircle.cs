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
    /// FilledCircle with no primary key, used for special forms of inheritance, eg SingleTableInheritance
    /// </summary>
    public class FilledCircleNoPrimaryKeyInheritsCircle : Circle
    {

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof(FilledCircleNoPrimaryKeyInheritsCircle)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (FilledCircleNoPrimaryKeyInheritsCircle)];
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
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            //ClassDef lClassDef = new ClassDef(typeof (FilledCircleNoPrimaryKeyInheritsCircle), null, lPropDefCol, keysCol, relDefCol);
            ClassDef lClassDef = new ClassDef(typeof(FilledCircleNoPrimaryKeyInheritsCircle), null, "FilledCircle", lPropDefCol, keysCol, relDefCol, null);
            lClassDef.SuperClassDef = new SuperClassDef(Circle.GetClassDef(), ORMapping.ConcreteTableInheritance);
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        public Int32? Colour
        {
            get { return (Int32?)GetPropertyValue("Colour"); }
            set { SetPropertyValue("Colour", value); }
        }
    }
}