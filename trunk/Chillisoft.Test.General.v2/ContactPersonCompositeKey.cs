using System;
using System.Collections;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;

namespace Chillisoft.Test.General.v2
{
    /// <summary>
    /// Summary description for ContactPersonCompositeKey.
    /// This is a test class used for testing Business Object architecture
    /// using composite keys as the primary object identifier.
    /// </summary>
    public class ContactPersonCompositeKey : BusinessObject
    {
        #region Constructors

        internal ContactPersonCompositeKey() : base()
        {
        }

        internal ContactPersonCompositeKey(BOPrimaryKey id) : base(id)
        {
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
                return ClassDef.GetClassDefCol[typeof (ContactPersonCompositeKey)];
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
            return lClassDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
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
                                                                 typeof (Car), relKeyDef, true, "", -1, -1,
                                                                 DeleteParentAction.cbsDereferenceRelatedObjects);

            relDefCol.Add(relDef);
            return relDefCol;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef = new PropDef("Surname", typeof (String), cbsPropReadWriteRule.ReadManyWriteMany, null);
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

            propDef = new PropDef("PK1Prop1", typeof (string), cbsPropReadWriteRule.OnlyRead, "PK1_Prop1", null);
            lPropDefCol.Add(propDef);

            propDef = new PropDef("PK1Prop2", typeof (string), cbsPropReadWriteRule.OnlyRead, "PK1_Prop2", null);
            lPropDefCol.Add(propDef);

            return lPropDefCol;
        }

        public static ContactPersonCompositeKey GetNewContactPersonCompositeKey()
        {
            ContactPersonCompositeKey myContactPerson = new ContactPersonCompositeKey();
            AddToLoadedBusinessObjectCol(myContactPerson);
            return myContactPerson;
        }

        /// <summary>
        /// returns the ContactPerson identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object Id</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Chillisoft.Bo.v2.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static ContactPersonCompositeKey GetContactPersonCompositeKey(BOPrimaryKey id)
        {
            ContactPersonCompositeKey myContactPerson =
                (ContactPersonCompositeKey) ContactPersonCompositeKey.GetLoadedBusinessObject(id);
            if (myContactPerson == null)
            {
                myContactPerson = new ContactPersonCompositeKey(id);
                AddToLoadedBusinessObjectCol(myContactPerson);
            }
            return myContactPerson;
        }

        #endregion //Constructors

        #region RelationShips

        public BusinessObjectCollection GetCarsDriven()
        {
            return Relationships.GetRelatedBusinessObjectCol("Driver");
            //			return Car.LoadBusinessObjCol("DriverFK1 = " + 
            //					this.GetPropertyValueString("PK1Prop1") +
            //					" AND DriverFK2 = " + this.GetPropertyValueString("PK1Prop2"),"");
        }

        #endregion //Relationships

        #region ForTesting

        internal static void ClearContactPersonCol()
        {
            BusinessObject.ClearLoadedBusinessObjectBaseCol();
        }

        internal static Hashtable GetContactPersonCol()
        {
            return BusinessObject.GetLoadedBusinessObjectBaseCol();
        }

        internal static void DeleteAllContactPeople()
        {
            string sql = "DELETE FROM tbContactPersonCompositeKey";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        #endregion //ForTesting
    }
}