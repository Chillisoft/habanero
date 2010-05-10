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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.Test.BO
{
    public class BOWithIntID_Child : BusinessObject
    {
        public int? IntID
        {
            get
            {
                return (int?)GetPropertyValue("IntID");
            }
            set { SetPropertyValue("IntID", value); }
        }

        public string TestField
        {
            get
            {
                return GetPropertyValueString("TestField");
            }
            set { SetPropertyValue("TestField", value); }
        }

        public override string ToString()
        {
            return TestField;
        }
        private IClassDef _classDefCircleNoPrimaryKey;
        private IClassDef _classDefShape;
        private IClassDef _classDefFilledCircleNoPrimaryKey;

        protected void SetupInheritanceSpecifics()
        {
            ClassDef.ClassDefs.Clear();
            _classDefShape = Shape.GetClassDef();
            _classDefCircleNoPrimaryKey = CircleNoPrimaryKey.GetClassDef();
            _classDefCircleNoPrimaryKey.SuperClassDef = new SuperClassDef(_classDefShape,
                                                                          ORMapping.SingleTableInheritance);
            _classDefCircleNoPrimaryKey.SuperClassDef.Discriminator = "ShapeType_field";
            _classDefFilledCircleNoPrimaryKey = FilledCircleNoPrimaryKey.GetClassDef();
            _classDefFilledCircleNoPrimaryKey.SuperClassDef = new SuperClassDef(_classDefCircleNoPrimaryKey,
                                                                                ORMapping.SingleTableInheritance);
            _classDefFilledCircleNoPrimaryKey.SuperClassDef.Discriminator = "ShapeType_field";
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("SomeNewProp", typeof(int), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);
            KeyDefCol keysCol = new KeyDefCol();
            RelationshipDefCol relDefCol = new RelationshipDefCol();
            ClassDef lClassDef = new ClassDef(typeof(BOWithIntID_Child), null, "bowithintid", lPropDefCol, keysCol, relDefCol, null);
            return lClassDef;
        }

        public static ClassDef LoadClassDefWith_SingleTableInherit()
        {
            ClassDef itsClassDef = CreateClassDef();
            IClassDef classDef_BOWithIntID = BOWithIntID.LoadClassDefWithIntID_DiscriminatorField();
            itsClassDef.SuperClassDef = new SuperClassDef(classDef_BOWithIntID, ORMapping.SingleTableInheritance)
                                            {Discriminator = "Type_field"};
            itsClassDef.TableName = "bowithintid";
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        internal static void DeleteAllBOWithIntID()
        {
            const string sql = "DELETE FROM bowithintid";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }
    }
}