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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.Test
{
    public class Address: BusinessObject
    {
        #region Constructors

        public Address()
        {
        }

        internal Address(BOPrimaryKey id) : base(id)
        {
        }

        public Address(ClassDef classDef)
            : base(classDef)
        {
        }

        protected static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof(Address)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof(Address)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol propDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(propDefCol["AddressID"]);

            RelationshipDefCol relDefCol = CreateRelationshipDefCol(propDefCol);


            ClassDef classDef = new ClassDef(typeof (Address),  primaryKey, "contact_person_address", propDefCol, keysCol, relDefCol);
            ClassDef.ClassDefs.Add(classDef);
            return classDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //Define Relationships
            RelKeyDef relKeyDef = new RelKeyDef();
            IPropDef propDef = lPropDefCol["ContactPersonID"];

            RelPropDef relPropDef = new RelPropDef(propDef, "ContactPersonID");
            relKeyDef.Add(relPropDef);

            RelationshipDef relDef = new SingleRelationshipDef("ContactPerson", typeof(ContactPerson), relKeyDef, false, DeleteParentAction.Prevent);

            relDefCol.Add(relDef);
            
            return relDefCol;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol propDefCol = new PropDefCol();
            propDefCol.Add("AddressID", typeof(Guid), PropReadWriteRule.WriteOnce, null);
            propDefCol.Add("ContactPersonID", typeof(Guid), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("AddressLine1", typeof(String), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("AddressLine2", typeof(String), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("AddressLine3", typeof(String), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("AddressLine4", typeof(String), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("OrganisationID", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            return propDefCol;
        }

        /// <summary>
        /// returns the Address identified by id.
        /// </summary>
        /// <remarks>
        /// If the Address is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object primary Key</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Habanero.BO.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static Address GetCar(BOPrimaryKey id)
        {
            Address myAddress = (Address)BOLoader.Instance.GetLoadedBusinessObject(id);
            if (myAddress == null)
            {
                myAddress = new Address(id);
                // AddToLoadedBusinessObjectCol(myCar);
            }
            return myAddress;
        }

        #endregion //Constructors

        #region persistance

        #endregion /persistance

        #region Properties

        public Guid AddressID
        {
            get { return (Guid)GetPropertyValue("AddressID"); }
        }
		
        public Guid? ContactPersonID
        {
            get { return (Guid?)GetPropertyValue("ContactPersonID"); }
            set { SetPropertyValue("ContactPersonID", value); }
        }

        public string AddressLine1
        {
            get { return (string)GetPropertyValue("AddressLine1"); }
            set { SetPropertyValue("AddressLine1", value); }
        }

        public string AddressLine2
        {
            get { return (string)GetPropertyValue("AddressLine2"); }
            set { SetPropertyValue("AddressLine2", value); }
        }

        public string AddressLine3
        {
            get { return (string)GetPropertyValue("AddressLine3"); }
            set { SetPropertyValue("AddressLine3", value); }
        }

        public string AddressLine4
        {
            get { return (string)GetPropertyValue("AddressLine4"); }
            set { SetPropertyValue("AddressLine4", value); }
        }

        public ContactPerson ContactPerson
        {
            get { return this.Relationships.GetRelatedObject<ContactPerson>("ContactPerson"); }
        }

        public Guid? OrganisationID
        {
            get { return (Guid?) this.GetPropertyValue("OrganisationID"); }
            set { this.SetPropertyValue("OrganisationID", value); }
        }

        #endregion //Properties

        #region Relationships

        public ContactPerson GetContactPerson()
        {
            return Relationships.GetRelatedObject<ContactPerson>("ContactPerson");
        }

        #endregion //Relationships

        #region ForTesting

        public static void DeleteAllAddresses()
        {
            string sql = "DELETE FROM contact_person_address";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        #endregion

        #region ForCollections 

        //class
        protected internal string GetObjectNewID()
        {
            return _primaryKey.GetObjectNewID();
        }

        protected internal static BusinessObjectCollection<BusinessObject> LoadBusinessObjCol()
        {
            return LoadBusinessObjCol("", "");
        }

        protected internal static BusinessObjectCollection<BusinessObject> LoadBusinessObjCol(string searchCriteria,
                                                                                              string orderByClause)
        {
            BusinessObjectCollection<BusinessObject> bOCol = new BusinessObjectCollection<BusinessObject>(GetClassDef());
            bOCol.Load(searchCriteria, orderByClause);
            return bOCol;
        }

        #endregion
    }
}