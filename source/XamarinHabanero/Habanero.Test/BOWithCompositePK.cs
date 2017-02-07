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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace Habanero.Test
{
    public class BOWithCompositePK : BusinessObject
    {
        private const string PK1_PROP1_NAME = "PK1Prop1";
        private const string PK1_PROP2_NAME = "PK1Prop2";



        public string PK1Prop1
        {
            get { return GetPropertyValueString(PK1_PROP1_NAME); }
            set { SetPropertyValue(PK1_PROP1_NAME, value); }
        }

        public string PK1Prop2
        {
            get { return GetPropertyValueString(PK1_PROP2_NAME); }
            set { SetPropertyValue(PK1_PROP2_NAME, value); }
        }

        public static void LoadClassDefs()
        {
            CreateClassDef();
        }

        protected override IClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        private static IClassDef GetClassDef()
        {
            if (ClassDef.IsDefined(typeof(BOWithCompositePK)))
            {
                return ClassDef.Get<BOWithCompositePK>();
            }
            return CreateClassDef();
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            var keysCol = new KeyDefCol();
            var primaryKey = new PrimaryKeyDef { IsGuidObjectID = false };
            primaryKey.Add(lPropDefCol[PK1_PROP1_NAME]);
            primaryKey.Add(lPropDefCol[PK1_PROP2_NAME]);

            var relDefs = new RelationshipDefCol();
            var lClassDef =
                new ClassDef(typeof(BOWithCompositePK), primaryKey, lPropDefCol, keysCol, relDefs);

            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }


        protected static PropDefCol CreateBOPropDef()
        {
            var lPropDefCol = new PropDefCol();

            var propDef = new PropDef(PK1_PROP1_NAME, typeof (string), PropReadWriteRule.ReadWrite, "PK1_Prop1", null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef(PK1_PROP2_NAME, typeof(string), PropReadWriteRule.ReadWrite, "PK1_Prop2", null);
            lPropDefCol.Add(propDef);

            return lPropDefCol;
        }
    }
}