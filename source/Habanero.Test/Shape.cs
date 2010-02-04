// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.Test
{
    public class Shape : BusinessObject
    {

        public static IClassDef GetClassDef()
        {
            if (ClassDef.IsDefined(typeof (Shape)))
            {
                return ClassDef.ClassDefs[typeof (Shape)];
            }
            return CreateClassDef();
        }

        protected override IClassDef ConstructClassDef()
        {
            _classDef = (ClassDef) GetClassDef();
            return _classDef;
        }

        public static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("ShapeName", typeof (String), PropReadWriteRule.ReadWrite, "ShapeName", null);
            lPropDefCol.Add(propDef);
            propDef = new PropDef("ShapeID", typeof(Guid), PropReadWriteRule.WriteOnce, "ShapeID_field", null);
            lPropDefCol.Add(propDef);

           // propDef = new PropDef("MyID", typeof(Guid), PropReadWriteRule.WriteOnce, null);
           // lPropDefCol.Add(propDef);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["ShapeID"]);
            KeyDefCol keysCol = new KeyDefCol();
            KeyDef lKeyDef = new KeyDef();
            lKeyDef.Add(lPropDefCol["ShapeName"]);
            keysCol.Add(lKeyDef);
//            RelKeyDef relKeyDef = new RelKeyDef();

            //RelPropDef lRelPropDef = new RelPropDef(propDef, "OwnerID");
            //relKeyDef.Add(lRelPropDef);
            //RelationshipDef relDef = new MultipleRelationshipDef("Owner", typeof (Shape),
           //                                                      relKeyDef, false, "", DeleteParentAction.DereferenceRelated);
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            //relDefCol.Add(relDef);

            ClassDef lClassDef = new ClassDef(typeof (Shape), primaryKey, "Shape_table", lPropDefCol, keysCol, relDefCol);
			ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        public static ClassDef CreateTestMapperClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("ShapeName", typeof(String), PropReadWriteRule.ReadWrite, "ShapeName", null);
            lPropDefCol.Add(propDef);
            propDef = new PropDef("ShapeID", typeof(Guid), PropReadWriteRule.WriteOnce, "ShapeID_field", null);
            lPropDefCol.Add(propDef);
            propDef = new PropDef("ShapeValue", typeof(Int32), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);
            // propDef = new PropDef("MyID", typeof(Guid), PropReadWriteRule.WriteOnce, null);
            // lPropDefCol.Add(propDef);
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["ShapeID"]);
            KeyDefCol keysCol = new KeyDefCol();
            KeyDef lKeyDef = new KeyDef();
            lKeyDef.Add(lPropDefCol["ShapeName"]);
            keysCol.Add(lKeyDef);
//            RelKeyDef relKeyDef = new RelKeyDef();

            //RelPropDef lRelPropDef = new RelPropDef(propDef, "OwnerID");
            //relKeyDef.Add(lRelPropDef);
            //RelationshipDef relDef = new MultipleRelationshipDef("Owner", typeof (Shape),
            //                                                      relKeyDef, false, "", DeleteParentAction.DereferenceRelated);
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            //relDefCol.Add(relDef);

            ClassDef lClassDef = new ClassDef(typeof(Shape), primaryKey, "Shape_table", lPropDefCol, keysCol, relDefCol);
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

        public static Shape CreateSavedShape()
        {
            Shape shape = new Shape();
            shape.ShapeName = TestUtil.GetRandomString();
            shape.Save();
            return shape;
        }

    }
}