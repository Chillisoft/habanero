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
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;

namespace Habanero.Test.General
{
    public class Shape : BusinessObject
    {
        public static Shape GetNewObject()
        {
            Shape obj = new Shape();
            AddToLoadedBusinessObjectCol(obj);
            return obj;
        }

        public static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Shape)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.GetClassDefCol[typeof (Shape)];
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
                new PropDef("ShapeName", typeof (String), PropReadWriteRule.ReadManyWriteMany, "ShapeName", null);
            lPropDefCol.Add(propDef);
            propDef = lPropDefCol.Add("ShapeID", typeof (Guid), PropReadWriteRule.ReadManyWriteOnce, null);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["ShapeID"]);
            KeyDefCol keysCol = new KeyDefCol();
            KeyDef lKeyDef = new KeyDef();
            lKeyDef.Add(lPropDefCol["ShapeName"]);
            keysCol.Add(lKeyDef);
            RelKeyDef relKeyDef = new RelKeyDef();
            RelPropDef lRelPropDef = new RelPropDef(propDef, "OwnerID");
            relKeyDef.Add(lRelPropDef);
            RelationshipDef relDef = new MultipleRelationshipDef("Owner", typeof (Shape),
                                                                 relKeyDef, false, "", -1, -1,
                                                                 DeleteParentAction.DereferenceRelatedObjects);
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            relDefCol.Add(relDef);
            ClassDef lClassDef = new ClassDef(typeof (Shape), primaryKey, lPropDefCol, keysCol, relDefCol);
			ClassDef.GetClassDefCol.Add(lClassDef);
            return lClassDef;
        }

        public string ShapeName
        {
            get { return (string) this.GetPropertyValue("ShapeName"); }
            set { this.SetPropertyValue("ShapeName", value); }
        }
    }
}