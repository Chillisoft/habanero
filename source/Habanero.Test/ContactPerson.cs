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

// Static Model
using System;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
using Habanero.DB;

namespace Habanero.Test
{
    public class ContactPerson : BusinessObject
    {
        #region Fields

        protected BOProp mPropDateLastUpdated = null;
        protected BOProp mPropUserLastUpdated = null;
        protected BOProp mPropMachineLastUpdated = null;
        protected BOProp mPropVersionNumber = null;

        #endregion

        #region Constructors

        public ContactPerson() : base()
        {
            SetPropertyValue("PK3Prop", this.ID.GetObjectId());
        }

        internal ContactPerson(BOPrimaryKey id) : base(id)
        {
        }

        internal ContactPerson(IExpression searchExpression) : base(searchExpression)
        {
        }

        public ContactPerson(ClassDef classDef) : base(classDef)
        {
        }

        protected static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (ContactPerson)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (ContactPerson)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            _classDef = GetClassDef();


            return _classDef;
        }

        protected override void ConstructFromClassDef(bool newObject)
        {
            base.ConstructFromClassDef(newObject);

            mPropDateLastUpdated = _boPropCol["DateLastUpdated"];
            mPropUserLastUpdated = _boPropCol["UserLastUpdated"];
            mPropMachineLastUpdated = _boPropCol["MachineLastUpdated"];
            mPropVersionNumber = _boPropCol["VersionNumber"];

            SetConcurrencyControl(new OptimisticLockingVersionNumber(mPropDateLastUpdated,
                                                                     mPropUserLastUpdated, mPropMachineLastUpdated,
                                                                     mPropVersionNumber));
            SetTransactionLog(new TransactionLogTable("TransactionLog",
                                                      "DateTimeUpdated",
                                                      "WindowsUser",
                                                      "LogonUser",
                                                      "MachineName",
                                                      "BusinessObjectTypeName",
                                                      "CRUDAction",
                                                      "DirtyXML"));
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDef lKeyDef = new KeyDef();
            lKeyDef.IgnoreIfNull = true;
            lKeyDef.Add(lPropDefCol["PK2Prop1"]);
            lKeyDef.Add(lPropDefCol["PK2Prop2"]);
            KeyDefCol keysCol = new KeyDefCol();

            keysCol.Add(lKeyDef);

            lKeyDef = new KeyDef();
            lKeyDef.IgnoreIfNull = false;

            lKeyDef.Add(lPropDefCol["PK3Prop"]);
            keysCol.Add(lKeyDef);

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["ContactPersonID"]);


            //Releationships
            RelationshipDefCol relDefs = CreateRelationshipDefCol(lPropDefCol);

            ClassDef lClassDef = new ClassDef(typeof (ContactPerson), primaryKey, lPropDefCol, keysCol, relDefs);
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //Define Owner Relationships
            RelKeyDef relKeyDef;
            PropDef propDef;
            RelPropDef lRelPropDef;
            RelationshipDef relDef;
            relKeyDef = new RelKeyDef();
            propDef = lPropDefCol["ContactPersonID"];
            lRelPropDef = new RelPropDef(propDef, "OwnerId");
            relKeyDef.Add(lRelPropDef);
            relDef = new MultipleRelationshipDef("Owner", typeof(Car),
                                                 relKeyDef, false, "",
                                                 DeleteParentAction.DereferenceRelated);
            relDefCol.Add(relDef);

            relKeyDef = new RelKeyDef();
            propDef = lPropDefCol["ContactPersonID"];
            lRelPropDef = new RelPropDef(propDef, "ContactPersonID");
            relKeyDef.Add(lRelPropDef);
            relDef = new MultipleRelationshipDef("Addresses", typeof(Address),
                                                 relKeyDef, false, "",
                                                 DeleteParentAction.DeleteRelated);
            relDefCol.Add(relDef);
			
            return relDefCol;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("Surname", "System", "String", PropReadWriteRule.ReadWrite, "Surname", null, true, false);
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

            propDef = new PropDef("PK2Prop1", typeof(string), PropReadWriteRule.ReadWrite, "PK2_Prop1", null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("PK2Prop2", typeof(string), PropReadWriteRule.ReadWrite, "PK2_Prop2", null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("PK3Prop", typeof (string), PropReadWriteRule.WriteNew, "PK3_Prop", null);
            lPropDefCol.Add(propDef);

            propDef = lPropDefCol.Add("ContactPersonID", typeof (Guid), PropReadWriteRule.WriteOnce, null);

            return lPropDefCol;
        }


        /// <summary>
        /// returns the ContactPerson identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object primary Key</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Habanero.BO.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static ContactPerson GetContactPerson(BOPrimaryKey id)
        {
            ContactPerson myContactPerson = (ContactPerson) BOLoader.Instance.GetLoadedBusinessObject(id);
            if (myContactPerson == null)
            {
                myContactPerson = new ContactPerson(id);
            }
            return myContactPerson;
        }

        /// <summary>
        /// returns the ContactPerson identified by the search criteria else returns null.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="searchExpression">The object primary Key as a searchExpression or any of the objects
        /// alternate keys propertyname value pairs</param>
        /// <returns>The loaded business object or null</returns>
        /// <exception cref="Habanero.BO.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static ContactPerson GetContactPerson(IExpression searchExpression)
        {
            ContactPerson myContactPerson =
                (ContactPerson)BOLoader.Instance.GetLoadedBusinessObject(searchExpression.ExpressionString());

            if (myContactPerson == null)
            {
                myContactPerson = new ContactPerson();
                myContactPerson = (ContactPerson) BOLoader.Instance.GetBusinessObject(myContactPerson, searchExpression);
                
            }
            return myContactPerson;
        }

        #endregion //Constructors

        #region persistance

        #endregion /persistance

        #region Properties

        public Guid? ContactPersonID
        {
            get { return (Guid?)GetPropertyValue("ContactPersonID"); }
            set { SetPropertyValue("ContactPersonID", value); }
        }

        public string Surname
        {
            get { return GetPropertyValueString("Surname"); }
            set { SetPropertyValue("Surname", value); }
        }

        public string FirstName
        {
            get { return (string) GetPropertyValue("FirstName"); }
            set { SetPropertyValue("FirstName", value); }
        }

        public DateTime DateOfBirth
        {
            get { return (DateTime) GetPropertyValue("DateOfBirth"); }
            set { SetPropertyValue("DateOfBirth", value); }
        }

        /// <summary>
        /// Age in years
        /// </summary>
        public double Age
        {
            get { return DateTime.Now.Year - DateOfBirth.Year; }
        }

        #endregion //Properties

        #region Relationships

        public IBusinessObjectCollection GetCarsOwned()
        {
            return Relationships.GetRelatedCollection("Owner");
        }

        public BusinessObjectCollection<Address> Addresses
        {
            get { return Relationships.GetRelatedCollection<Address>("Addresses"); }
        }

        #endregion //Relationships

        #region ForTesting

        public static void ClearContactPersonCol()
        {
            ClearLoadedBusinessObjectBaseCol();
        }

        public static void DeleteAllContactPeople()
        {
            string sql = "DELETE FROM ContactPerson";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        #endregion

        #region ForCollections //TODO: refactor this so that class construction occurs in its own 

        //class
        protected internal string GetObjectNewID()
        {
            return _primaryKey.GetObjectNewID();
        }

        public static BusinessObjectCollection<ContactPerson> LoadBusinessObjCol()
        {
            return LoadBusinessObjCol("", "");
        }

        public static BusinessObjectCollection<ContactPerson> LoadBusinessObjCol(string searchCriteria,
                                                                                              string orderByClause)
        {
            BusinessObjectCollection<ContactPerson> bOCol = new BusinessObjectCollection<ContactPerson>(GetClassDef());
            bOCol.Load(searchCriteria, orderByClause);
            return bOCol;
        }

        #endregion

        public void AddPreventDeleteRelationship()
        {
            RelKeyDef relKeyDef = new RelKeyDef();
            RelPropDef lRelPropDef = new RelPropDef(ClassDef.PropDefcol["ContactPersonID"], "ContactPersonID");
            relKeyDef.Add(lRelPropDef);
            RelationshipDef relDef = new MultipleRelationshipDef("AddressesNoDelete", typeof(Address),
                                                                  relKeyDef, false, "", DeleteParentAction.Prevent);
            ClassDef.RelationshipDefCol = new RelationshipDefCol();
            ClassDef.RelationshipDefCol.Add(relDef);

            ((RelationshipCol)Relationships).Add(new MultipleRelationship(this, relDef, Props));
        }
    }
}