//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.Test
{
    public class Car : BusinessObject
    {
        #region Constructors

        public Car()
        {
        }


        public Car(ClassDef classDef) : base(classDef)
        {
        }

        protected internal static ClassDef GetClassDef()
        {
            if (!ClassDef.IsDefined(typeof (Car)))
            {
                return CreateClassDef();
            }
            else
            {
                return ClassDef.ClassDefs[typeof (Car)];
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
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(lPropDefCol["CarID"]);

            RelationshipDefCol relDefCol = CreateRelationshipDefCol(lPropDefCol);


            ClassDef lClassDef = new ClassDef(typeof (Car), primaryKey, "car_table", lPropDefCol, keysCol, relDefCol);
            ClassDef.ClassDefs.Add(lClassDef);
            return lClassDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(PropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //Define Owner Relationships
            RelKeyDef relKeyDef = new RelKeyDef();
            IPropDef propDef = lPropDefCol["OwnerId"];

            RelPropDef lRelPropDef = new RelPropDef(propDef, "ContactPersonID");
            relKeyDef.Add(lRelPropDef);

            RelationshipDef relDef = new SingleRelationshipDef("Owner", typeof(ContactPerson), relKeyDef, false, DeleteParentAction.Prevent);

            relDefCol.Add(relDef);

            //Define Driver Relationships
            relKeyDef = new RelKeyDef();
            propDef = lPropDefCol["DriverFK1"];

            lRelPropDef = new RelPropDef(propDef, "PK1Prop1");
            relKeyDef.Add(lRelPropDef);

            propDef = lPropDefCol["DriverFK2"];

            lRelPropDef = new RelPropDef(propDef, "PK1Prop2");
            relKeyDef.Add(lRelPropDef);

            relDef = new SingleRelationshipDef("Driver", typeof(ContactPersonCompositeKey), relKeyDef, true, DeleteParentAction.Prevent);


            relDefCol.Add(relDef);

            //Define Engine Relationships
            relKeyDef = new RelKeyDef();
            propDef = lPropDefCol["CarID"];

            lRelPropDef = new RelPropDef(propDef, "CarID");
            relKeyDef.Add(lRelPropDef);

            relDef = new SingleRelationshipDef("Engine", typeof(Engine), relKeyDef, false, DeleteParentAction.DereferenceRelated);

            relDefCol.Add(relDef);
            return relDefCol;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol lPropDefCol = new PropDefCol();
            PropDef propDef =
                new PropDef("CarRegNo", typeof (String), PropReadWriteRule.ReadWrite, "CAR_REG_NO", null);
            lPropDefCol.Add(propDef);

            lPropDefCol.Add("OwnerId", typeof (Guid), PropReadWriteRule.ReadWrite, "OWNER_ID", null);

            lPropDefCol.Add("CarID", typeof (Guid), PropReadWriteRule.WriteOnce, "CAR_ID", null);
            lPropDefCol.Add("DriverFK1", typeof (String), PropReadWriteRule.WriteOnce, "Driver_FK1", null);
            lPropDefCol.Add("DriverFK2", typeof (String), PropReadWriteRule.WriteOnce, "Driver_FK2", null);

            return lPropDefCol;
        }

        #endregion //Constructors

        #region persistance

        #endregion /persistance

        #region Properties

        public string CarRegNo
        {
            get { return (string)GetPropertyValue("CarRegNo"); }
            set { SetPropertyValue("CarRegNo", value); }
        }

        public Guid CarID
        {
            get { return (Guid)GetPropertyValue("CarID"); }
           
        }

        public Guid OwnerID
        {
            get { return (Guid)GetPropertyValue("OwnerId"); }
            set { SetPropertyValue("OwnerId", value); }
        }

        #endregion //Properties

        #region Relationships

        public ContactPerson GetOwner()
        {
            return Relationships.GetRelatedObject<ContactPerson>("Owner");
        }

        public ContactPersonCompositeKey GetDriver()
        {
            return (ContactPersonCompositeKey) Relationships.GetRelatedObject("Driver");
        }

        public Engine GetEngine()
        {
            return Relationships.GetRelatedObject<Engine>("Engine");
        }

        #endregion //Relationships

        #region ForTesting

        public static void DeleteAllCars()
        {
            string sql = "DELETE FROM car_table";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
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

        public static Car CreateSavedCar(string regno)
        {
            Car car = CreateUnsavedCar(regno);
            car.Save();
            return car;
        }

        public static Car CreateSavedCar(string regno, ContactPerson owner)
        {
            Car car = CreateUnsavedCar(regno, owner);
            car.Save();
            return car;
        }

        private static Car CreateUnsavedCar(string regno)
        {
            return CreateUnsavedCar(regno, null);
        }

        private static Car CreateUnsavedCar(string regno, ContactPerson owner)
        {
            Car car = new Car();
            if (owner != null) car.OwnerID = owner.ContactPersonID;
            car.CarRegNo = regno;
            return car;
        }
    }
}