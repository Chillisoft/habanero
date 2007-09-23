using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.Test.General
{
	public class Address: BusinessObject
	{
		#region Constructors

        public Address() : base()
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


            ClassDef classDef = new ClassDef(typeof (Address), primaryKey, propDefCol, keysCol, relDefCol);
			ClassDef.ClassDefs.Add(classDef);
            return classDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //Define Relationships
            RelKeyDef relKeyDef = new RelKeyDef();
			PropDef propDef = lPropDefCol["ContactPersonID"];

            RelPropDef relPropDef = new RelPropDef(propDef, "ContactPersonID");
            relKeyDef.Add(relPropDef);

            RelationshipDef relDef = new SingleRelationshipDef("ContactPerson", typeof (ContactPerson), relKeyDef, false);

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

		#endregion //Properties

        #region Relationships

		public ContactPerson GetContactPerson()
        {
			return (ContactPerson)Relationships.GetRelatedObject("ContactPerson");
        }

        #endregion //Relationships

        #region ForTesting

        internal static void DeleteAllAddresses()
        {
            string sql = "DELETE FROM Address";
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
