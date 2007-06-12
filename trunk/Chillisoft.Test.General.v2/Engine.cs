using System;
using System.Collections;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Db.v2;

namespace Chillisoft.Test.General.v2
{
    /// <summary>
    /// Summary description for Engine.
    /// </summary>
    public class Engine : BusinessObjectBase
    {
        #region Constructors

        internal Engine() : base()
        {
        }

        internal Engine(BOPrimaryKey id) : base(id)
        {
        }

        protected static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Engine)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.GetClassDefCol[typeof (Engine)];
            }
        }

        protected override ClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsObjectID = true;
            primaryKey.Add(lPropDefCol["EngineID"]);

            RelationshipDefCol relDefCol = CreateRelationshipDefCol(lPropDefCol);

            ClassDef lClassDef = new ClassDef(typeof (Engine), primaryKey, lPropDefCol, keysCol, relDefCol);
            lClassDef.TableName = "Table_Engine";
            return lClassDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //Define Engine Relationships
            RelKeyDef relKeyDef = new RelKeyDef();
            PropDef propDef = lPropDefCol["CarID"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "CarID");
            relKeyDef.Add(lRelPropDef);

            RelationshipDef relDef = new SingleRelationshipDef("Car", typeof (Car), relKeyDef, false);

            relDefCol.Add(relDef);

            return relDefCol;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("EngineNo", typeof (String), cbsPropReadWriteRule.ReadManyWriteMany, "ENGINE_NO", null);
            lPropDefCol.Add(propDef);

            propDef =
                lPropDefCol.Add("EngineID", typeof (Guid), cbsPropReadWriteRule.ReadManyWriteOnce, "Engine_ID", null);
            propDef = lPropDefCol.Add("CarID", typeof (Guid), cbsPropReadWriteRule.ReadManyWriteOnce, "CAR_ID", null);

            return lPropDefCol;
        }

        /// <summary>
        /// Creates a new contact person and adds this new contact person to the object manager collection
        /// </summary>
        /// <returns>newly created contact person Engine</returns>
        public static Engine GetNewEngine()
        {
            Engine myEngine = new Engine();
            AddToLoadedBusinessObjectCol(myEngine);
            return myEngine;
        }

        /// <summary>
        /// returns the Engine identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object primary Key</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="Chillisoft.Bo.v2.BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static Engine GetEngine(BOPrimaryKey id)
        {
            Engine myEngine = (Engine) Engine.GetLoadedBusinessObject(id);
            if (myEngine == null)
            {
                myEngine = new Engine(id);
                AddToLoadedBusinessObjectCol(myEngine);
            }
            return myEngine;
        }

        #endregion //Constructors

        #region persistance

        #endregion /persistance

        #region Properties

        #endregion //Properties

        #region Relationships

        public Car GetCar()
        {
            return (Car) Relationships.GetRelatedBusinessObject("Car");
        }

        #endregion //Relationships

        #region ForTesting

        internal static void ClearEngineCol()
        {
            BusinessObjectBase.ClearLoadedBusinessObjectBaseCol();
        }

        internal static Hashtable GetEngineCol()
        {
            return BusinessObjectBase.GetLoadedBusinessObjectBaseCol();
        }

        internal static void DeleteAllEngines()
        {
            string sql = "DELETE FROM " + GetClassDef().TableName;
            DatabaseConnection.CurrentConnection.ExecutePlainSql(sql);
        }

        #endregion

        #region ForCollections 

        //class
        protected internal string GetObjectNewID()
        {
            return mPrimaryKey.GetObjectNewID();
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