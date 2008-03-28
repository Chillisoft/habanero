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
using System.Collections;
using System.Collections.Generic;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.DB;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for ContactPersonCompositeKey.
    /// This is a test class used for testing Business Object architecture
    /// using composite keys as the primary object identifier.
    /// </summary>
    public class ContactPersonCompositeKey : BusinessObject
    {
        #region Constructors

        public ContactPersonCompositeKey() : base()
        {
        }

        public ContactPersonCompositeKey(BOPrimaryKey id) : base(id)
        {
        }

        public static void LoadClassDefs()
        {
            CreateClassDef();
        }

        protected override ClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        private static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (ContactPersonCompositeKey)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (ContactPersonCompositeKey)];
            }
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();
            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = false;
            primaryKey.Add(lPropDefCol["PK1Prop1"]);
            primaryKey.Add(lPropDefCol["PK1Prop2"]);

            RelationshipDefCol relDefs = CreateRelationshipDefCol(lPropDefCol);
            ClassDef lClassDef =
                new ClassDef(typeof (ContactPersonCompositeKey), primaryKey, lPropDefCol, keysCol, relDefs);
            lClassDef.HasObjectID = false;
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        protected static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();


            //Define Driver Relationships
            RelKeyDef relKeyDef = new RelKeyDef();
            PropDef propDef = lPropDefCol["PK1Prop1"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "DriverFK1");
            relKeyDef.Add(lRelPropDef);

            propDef = lPropDefCol["PK1Prop2"];

            lRelPropDef = new RelPropDef(propDef, "DriverFK2");
            relKeyDef.Add(lRelPropDef);

            RelationshipDef relDef = new MultipleRelationshipDef("Driver",
                                                                 typeof (Car), relKeyDef, true, "", 
                                                                 DeleteParentAction.DereferenceRelated);

            relDefCol.Add(relDef);
            return relDefCol;
        }

        protected static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef = new PropDef("Surname", typeof (String), PropReadWriteRule.ReadWrite, null);
            propDef.PropRule = new PropRuleString("ContactPerson-" + propDef.PropertyName, "", 2, 50, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("FirstName", typeof (String), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("DateOfBirth", typeof (DateTime), PropReadWriteRule.WriteOnce, null);
            lPropDefCol.Add(propDef);

            //Create concurrency control properties
            propDef = new PropDef("DateLastUpdated", typeof(DateTime), PropReadWriteRule.ReadWrite, DateTime.Now);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("UserLastUpdated", typeof(string), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("MachineLastUpdated", typeof(string), PropReadWriteRule.ReadWrite, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("VersionNumber", typeof(int), PropReadWriteRule.ReadWrite, 1);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("PK1Prop1", typeof(string), PropReadWriteRule.ReadWrite, "PK1_Prop1", null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("PK1Prop2", typeof(string), PropReadWriteRule.ReadWrite, "PK1_Prop2", null);
            lPropDefCol.Add(propDef);

            return lPropDefCol;
        }

        /// <summary>
        /// returns the ContactPerson identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object Value</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Habanero.BO.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static ContactPersonCompositeKey GetContactPersonCompositeKey(BOPrimaryKey id)
        {
            ContactPersonCompositeKey myContactPerson =
                (ContactPersonCompositeKey)BOLoader.Instance.GetLoadedBusinessObject(id);
            if (myContactPerson == null)
            {
                myContactPerson = new ContactPersonCompositeKey(id);
            }
            return myContactPerson;
        }

        #endregion //Constructors

        #region RelationShips

        public IBusinessObjectCollection GetCarsDriven()
        {
            return Relationships.GetRelatedCollection("Driver");
            //			return Car.LoadBusinessObjCol("DriverFK1 = " + 
            //					this.GetPropertyValueString("PK1Prop1") +
            //					" AND DriverFK2 = " + this.GetPropertyValueString("PK1Prop2"),"");
        }

        #endregion //Relationships

        #region ForTesting

        public static void ClearContactPersonCol()
        {
            BusinessObject.ClearLoadedBusinessObjectBaseCol();
        }

        public static void DeleteAllContactPeople()
        {
            string sql = "DELETE FROM ContactPersonCompositeKey";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        #endregion //ForTesting
    }
}