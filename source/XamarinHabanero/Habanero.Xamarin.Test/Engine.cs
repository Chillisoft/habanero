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
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.Test
{
    /// <summary>
    /// Summary description for Engine.
    /// </summary>
    public class Engine : BusinessObject
    {
        #region Constructors

        public Engine() : base()
        {
        }

        protected internal static IClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Engine)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (Engine)];
            }
        }

        protected override IClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        private static ClassDef CreateClassDef()
        {
            PropDefCol lPropDefCol = CreateBOPropDef();

            KeyDefCol keysCol = new KeyDefCol();

            PrimaryKeyDef primaryKey = new PrimaryKeyDef();
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["EngineID"]);

            RelationshipDefCol relDefCol = CreateRelationshipDefCol(lPropDefCol);

            ClassDef lClassDef = new ClassDef(typeof (Engine), primaryKey, lPropDefCol, keysCol, relDefCol);
            lClassDef.TableName = "Table_Engine";
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //Define Engine Relationships
            RelKeyDef relKeyDef = new RelKeyDef();
            IPropDef propDef = lPropDefCol["CarID"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "CarID");
            relKeyDef.Add(lRelPropDef);

            RelationshipDef relDef = new SingleRelationshipDef("Car", typeof (Car), relKeyDef, false, DeleteParentAction.Prevent);
            relDefCol.Add(relDef);

            return relDefCol;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            IPropDef propDef =
                new PropDef("EngineNo", typeof (String), PropReadWriteRule.ReadWrite, "ENGINE_NO", null);
            lPropDefCol.Add(propDef);

            propDef =
                lPropDefCol.Add("EngineID", typeof (Guid), PropReadWriteRule.WriteOnce, "Engine_ID", null);
            propDef = lPropDefCol.Add("CarID", typeof(Guid), PropReadWriteRule.ReadWrite, "CAR_ID", null);

            return lPropDefCol;
        }

        #endregion //Constructors

        #region persistance

        #endregion /persistance

        #region Properties

        public Guid CarID
        {
            get { return (Guid)GetPropertyValue("CarID"); }
            set { SetPropertyValue("CarID", value);}
        }

        public string EngineNo
        {
            get { return (string)GetPropertyValue("EngineNo"); }
            set { SetPropertyValue("EngineNo", value); }
        }
        #endregion //Properties

        #region Relationships

        public Car GetCar()
        {
            return Relationships.GetRelatedObject<Car>("Car");
        }

        public Car Car
        {
            get { return Relationships.GetRelatedObject<Car>("Car"); }
        }

        #endregion //Relationships

        #region ForTesting

        internal static void ClearEngineCol()
        {
            FixtureEnvironment.ClearBusinessObjectManager();
        }

        public static void DeleteAllEngines()
        {
            if (DatabaseConnection.CurrentConnection != null)
            {
                string sql = "DELETE FROM " + GetClassDef().TableName;
                DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
            }
        }

        #endregion

        #region ForCollections 

        //class

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

        public static Engine CreateSavedEngine(Car car, string engineNo)
        {
            Engine engine = new Engine();
            engine.CarID = car.CarID;
            engine.EngineNo = engineNo;
            engine.Save();
            return engine;
        }

        public static Engine CreateSavedEngine(string engineNo)
        {
            Engine engine = new Engine();
            engine.EngineNo = engineNo;
            engine.Save();
            return engine;
        }

        public static IClassDef LoadClassDef_IncludingCarAndOwner()
        {

            new Engine();
            new Car();
            new ContactPerson();
            return ClassDef.Get<Engine>();
          
      
        }

    	public static Engine CreateEngineWithCarWithContact(out string surname, out string regno, out string engineNo)
    	{
    		regno = TestUtil.GetRandomString();
    		engineNo = TestUtil.GetRandomString();
    		surname = TestUtil.GetRandomString();
    		ContactPerson owner = ContactPerson.CreateSavedContactPerson(surname);
    		Car car = Car.CreateSavedCar(regno, owner);
    		return CreateSavedEngine(car, engineNo);
    	}

    	public static void CreateEngineWithCarWithContact()
    	{
    		string surname;
    		string regno;
    		string engineNo;
    		CreateEngineWithCarWithContact(out surname, out regno, out engineNo);
    		return;
    	}
    }
}