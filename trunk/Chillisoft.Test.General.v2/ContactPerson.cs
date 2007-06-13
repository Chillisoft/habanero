// Static Model
using System;
using System.Collections;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.CriteriaManager.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;

namespace Chillisoft.Test.General.v2
{
    // TODO:
    //	internal class ContactPersonCreator: IObjectCreator	
    //	{
    //		public BusinessObjectBase CreateBusinessObject ()
    //		{
    //			return new ContactPerson();
    //		}
    //	}

    public class ContactPerson : BusinessObjectBase
    {
        #region Fields

        protected BOProp mPropDateLastUpdated = null;
        protected BOProp mPropUserLastUpdated = null;
        protected BOProp mPropMachineLastUpdated = null;
        protected BOProp mPropVersionNumber = null;

        #endregion

        #region Constructors

        internal ContactPerson() : base()
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
                return ClassDef.GetClassDefCol[typeof (ContactPerson)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            _classDef = GetClassDef();


            return _classDef;
        }

        protected override void ConstructClass(bool newObject)
        {
            base.ConstructClass(newObject);

            mPropDateLastUpdated = _boPropCol["DateLastUpdated"];
            mPropUserLastUpdated = _boPropCol["UserLastUpdated"];
            mPropMachineLastUpdated = _boPropCol["MachineLastUpdated"];
            mPropVersionNumber = _boPropCol["VersionNumber"];

            SetConcurrencyControl(new OptimisticLockingVersionNumber(mPropDateLastUpdated,
                                                                     mPropUserLastUpdated, mPropMachineLastUpdated,
                                                                     mPropVersionNumber));
            SetTransactionLog(new TransactionLogTable("tbTransactionLog",
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
            lKeyDef.IgnoreNulls = true;
            lKeyDef.Add(lPropDefCol["PK2Prop1"]);
            lKeyDef.Add(lPropDefCol["PK2Prop2"]);
            KeyDefCol keysCol = new KeyDefCol();

            keysCol.Add(lKeyDef);

            lKeyDef = new KeyDef();
            lKeyDef.IgnoreNulls = false;

            lKeyDef.Add(lPropDefCol["PK3Prop"]);
            keysCol.Add(lKeyDef);

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["ContactPersonID"]);


            //Releationships
            RelationshipDefCol relDefs = CreateRelationshipDefCol(lPropDefCol);

            ClassDef lClassDef = new ClassDef(typeof (ContactPerson), primaryKey, lPropDefCol, keysCol, relDefs);
            return lClassDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //Define Owner Relationships
            RelKeyDef relKeyDef = new RelKeyDef();
            PropDef propDef = lPropDefCol["ContactPersonID"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "OwnerId");
            relKeyDef.Add(lRelPropDef);

            RelationshipDef relDef = new MultipleRelationshipDef("Owner", typeof (Car),
                                                                 relKeyDef, false, "", -1, -1,
                                                                 DeleteParentAction.cbsDereferenceRelatedObjects);

            relDefCol.Add(relDef);

            return relDefCol;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("Surname", typeof (String), cbsPropReadWriteRule.ReadManyWriteMany, "Surname", null);
            propDef.assignPropRule(new PropRuleString("ContactPerson-" + propDef.PropertyName, true, 2, 50));
            lPropDefCol.Add(propDef);

            propDef = new PropDef("FirstName", typeof (String), cbsPropReadWriteRule.ReadManyWriteMany, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("DateOfBirth", typeof (DateTime), cbsPropReadWriteRule.ReadManyWriteOnce, null);
            lPropDefCol.Add(propDef);

            //Create concurrency control properties
            propDef = new PropDef("DateLastUpdated", typeof (DateTime), cbsPropReadWriteRule.OnlyRead, DateTime.Now);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("UserLastUpdated", typeof (string), cbsPropReadWriteRule.OnlyRead, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("MachineLastUpdated", typeof (string), cbsPropReadWriteRule.OnlyRead, null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("VersionNumber", typeof (int), cbsPropReadWriteRule.OnlyRead, 1);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("PK2Prop1", typeof (string), cbsPropReadWriteRule.OnlyRead, "PK2_Prop1", null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("PK2Prop2", typeof (string), cbsPropReadWriteRule.OnlyRead, "PK2_Prop2", null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("PK3Prop", typeof (string), cbsPropReadWriteRule.OnlyRead, "PK3_Prop", null);
            lPropDefCol.Add(propDef);

            propDef = lPropDefCol.Add("ContactPersonID", typeof (Guid), cbsPropReadWriteRule.ReadManyWriteOnce, null);
            return lPropDefCol;
        }

        /// <summary>
        /// Creates a new contact person and adds this new contact person to the object manager collection
        /// </summary>
        /// <returns>newly created contact person ContactPerson</returns>
        public static ContactPerson GetNewContactPerson()
        {
            ContactPerson myContactPerson = new ContactPerson();
            AddToLoadedBusinessObjectCol(myContactPerson);
            return myContactPerson;
        }

        /// <summary>
        /// returns the ContactPerson identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object primary Key</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Chillisoft.Bo.v2.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static ContactPerson GetContactPerson(BOPrimaryKey id)
        {
            ContactPerson myContactPerson = (ContactPerson) ContactPerson.GetLoadedBusinessObject(id);
            if (myContactPerson == null)
            {
                myContactPerson = new ContactPerson(id);
                AddToLoadedBusinessObjectCol(myContactPerson);
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
        /// <exception cref="Chillisoft.Bo.v2.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static ContactPerson GetContactPerson(IExpression searchExpression)
        {
            ContactPerson myContactPerson =
                (ContactPerson) ContactPerson.GetLoadedBusinessObject(searchExpression.ExpressionString());

            if (myContactPerson == null)
            {
                myContactPerson = ContactPerson.GetNewContactPerson();
                myContactPerson = (ContactPerson) myContactPerson.GetBusinessObject(searchExpression);
                //				AddToLoadedBusinessObjectCol(myContactPerson);
            }
            return myContactPerson;
        }

        #endregion //Constructors

        #region persistance

        #endregion /persistance

        #region Properties

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

        #region RelationShips

        public BusinessObjectBaseCollection GetCarsOwned()
        {
            return Relationships.GetRelatedBusinessObjectCol("Owner");
        }

        #endregion //Relationships

        #region ForTesting

        internal static void ClearContactPersonCol()
        {
            BusinessObjectBase.ClearLoadedBusinessObjectBaseCol();
        }

        internal static Hashtable GetContactPersonCol()
        {
            return BusinessObjectBase.GetLoadedBusinessObjectBaseCol();
        }

        internal static void DeleteAllContactPeople()
        {
            string sql = "DELETE FROM tbContactPerson";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        #endregion

        #region ForCollections //TODO: refactor this so that class construction occurs in its own 

        //class
        protected internal string GetObjectNewID()
        {
            return _primaryKey.GetObjectNewID();
        }

        protected internal static BusinessObjectBaseCollection LoadBusinessObjCol()
        {
            return LoadBusinessObjCol("", "");
        }

        protected internal static BusinessObjectBaseCollection LoadBusinessObjCol(string searchCriteria,
                                                                                  string orderByClause)
        {
            BusinessObjectBaseCollection bOCol = new BusinessObjectBaseCollection(GetClassDef());
            bOCol.Load(searchCriteria, orderByClause);
            return bOCol;
        }

        #endregion
    }
}