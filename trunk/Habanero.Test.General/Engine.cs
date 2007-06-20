using System;
using System.Collections;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Db;

namespace Habanero.Test.General
{
    /// <summary>
    /// Summary description for Engine.
    /// </summary>
    public class Engine : BusinessObject
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
			ClassDef.GetClassDefCol.Add(lClassDef);
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
                new PropDef("EngineNo", typeof (String), PropReadWriteRule.ReadManyWriteMany, "ENGINE_NO", null);
            lPropDefCol.Add(propDef);

            propDef =
                lPropDefCol.Add("EngineID", typeof (Guid), PropReadWriteRule.ReadManyWriteOnce, "Engine_ID", null);
            propDef = lPropDefCol.Add("CarID", typeof (Guid), PropReadWriteRule.ReadManyWriteOnce, "CAR_ID", null);

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
        /// <exception cref="Habanero.Bo.BusObjDeleteConcurrencyControlException">
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
            BusinessObject.ClearLoadedBusinessObjectBaseCol();
        }

        internal static Hashtable GetEngineCol()
        {
            return BusinessObject.GetLoadedBusinessObjectBaseCol();
        }

        internal static void DeleteAllEngines()
        {
            string sql = "DELETE FROM " + GetClassDef().TableName;
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        #endregion

        #region ForCollections 

        //class
        protected internal string GetObjectNewID()
        {
            return _primaryKey.GetObjectNewID();
        }

        protected internal static BusinessObjectCollection LoadBusinessObjCol()
        {
            return LoadBusinessObjCol("", "");
        }

        protected internal static BusinessObjectCollection LoadBusinessObjCol(string searchCriteria,
                                                                                  string orderByClause)
        {
            BusinessObjectCollection bOCol = new BusinessObjectCollection(GetClassDef());
            bOCol.Load(searchCriteria, orderByClause);
            return bOCol;
        }

        #endregion
    }
}