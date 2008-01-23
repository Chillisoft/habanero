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
using Habanero.BO;

namespace Habanero.Test
{
    public class Shape : BusinessObject
    {

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Shape)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (Shape)];
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
                new PropDef("ShapeName", typeof (String), PropReadWriteRule.ReadWrite, "ShapeName", null);
            lPropDefCol.Add(propDef);
            propDef = new PropDef("ShapeID", typeof(Guid), PropReadWriteRule.WriteOnce, null);
            lPropDefCol.Add(propDef);
           // propDef = new PropDef("MyID", typeof(Guid), PropReadWriteRule.WriteOnce, null);
           // lPropDefCol.Add(propDef);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["ShapeID"]);
            KeyDefCol keysCol = new KeyDefCol();
            KeyDef lKeyDef = new KeyDef();
            lKeyDef.Add(lPropDefCol["ShapeName"]);
            keysCol.Add(lKeyDef);
            RelKeyDef relKeyDef = new RelKeyDef();

            //RelPropDef lRelPropDef = new RelPropDef(propDef, "OwnerID");
            //relKeyDef.Add(lRelPropDef);
            //RelationshipDef relDef = new MultipleRelationshipDef("Owner", typeof (Shape),
           //                                                      relKeyDef, false, "", DeleteParentAction.DereferenceRelated);
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            //relDefCol.Add(relDef);
			
            ClassDef lClassDef = new ClassDef(typeof (Shape), primaryKey, lPropDefCol, keysCol, relDefCol);
			ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        public Guid? ShapeID
        {
            get { return (Guid?)GetPropertyValue("ShapeID"); }
            set { SetPropertyValue("ShapeID", value); }
        }

        public string ShapeName
        {
            get { return (string) GetPropertyValue("ShapeName"); }
            set { SetPropertyValue("ShapeName", value); }
        }

        public string ShapeNameGetOnly
        {
            get { return ShapeName; }
        }
    }
}