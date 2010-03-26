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
    public class FilledCircleInheritsCircleNoPK : CircleNoPrimaryKey
    {

        public new static IClassDef GetClassDef()
        {
            if (ClassDef.IsDefined(typeof (FilledCircleInheritsCircleNoPK)))
            {
                return ClassDef.ClassDefs[typeof (FilledCircleInheritsCircleNoPK)];
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
            IPropDef propDef =
                new PropDef("Colour", typeof (int), PropReadWriteRule.ReadWrite, "Colour", null);
            lPropDefCol.Add(propDef);
            propDef = lPropDefCol.Add("FilledCircleID", typeof(Guid), PropReadWriteRule.WriteOnce, "FilledCircleID_field", null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["FilledCircleID"]);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            //ClassDef lClassDef = new ClassDef(typeof(FilledCircleInheritsCircleNoPK), primaryKey, lPropDefCol, keysCol, relDefCol);
            ClassDef lClassDef = new ClassDef(typeof(FilledCircleInheritsCircleNoPK), primaryKey, "FilledCircle_table", lPropDefCol, keysCol, relDefCol, null);
            lClassDef.SuperClassDef = new SuperClassDef(CircleNoPrimaryKey.GetClassDef(), ORMapping.ConcreteTableInheritance);
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
    }
}