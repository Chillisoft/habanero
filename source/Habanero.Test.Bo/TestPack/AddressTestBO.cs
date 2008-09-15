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
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.Base;
using Habanero.BO.ClassDefinition;
using Habanero.DB;

namespace Habanero.Test.BO
{
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


    public class AddressTestBO : BusinessObject
    {
        private bool _isDeletable = true;

        #region Constructors

        public AddressTestBO()
        {
        }

        public AddressTestBO(ClassDef classDef)
            : base(classDef)
        {
        }

        protected static ClassDef GetClassDef()
        {
            return ClassDef.IsDefined(typeof (AddressTestBO))
                       ? ClassDef.ClassDefs[typeof (AddressTestBO)]
                       : CreateClassDef();
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
            primaryKey.IsGuidObjectID = true;
            primaryKey.Add(propDefCol["AddressID"]);

            RelationshipDefCol relDefCol = CreateRelationshipDefCol(propDefCol);

            ClassDef classDef = new ClassDef(typeof (AddressTestBO), primaryKey, "contact_person_address", propDefCol, keysCol,
                                             relDefCol);
            ClassDef.ClassDefs.Add(classDef);
            return classDef;
        }

        private static RelationshipDefCol CreateRelationshipDefCol(IPropDefCol lPropDefCol)
        {
            RelationshipDefCol relDefCol = new RelationshipDefCol();

            //Define Relationships
            RelKeyDef relKeyDef = new RelKeyDef();
            IPropDef propDef = lPropDefCol["ContactPersonID"];

            RelPropDef relPropDef = new RelPropDef(propDef, "ContactPersonID");
            relKeyDef.Add(relPropDef);

            RelationshipDef relDef = new SingleRelationshipDef("ContactPersonTestBO", typeof(ContactPersonTestBO), relKeyDef, false,
                                                               DeleteParentAction.Prevent);

            relDefCol.Add(relDef);

            return relDefCol;
        }

        private static PropDefCol CreateBOPropDef()
        {
            PropDefCol propDefCol = new PropDefCol();
            propDefCol.Add("AddressID", typeof (Guid), PropReadWriteRule.WriteOnce, null);
            propDefCol.Add("ContactPersonID", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("AddressLine1", typeof (String), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("AddressLine2", typeof (String), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("AddressLine3", typeof (String), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("AddressLine4", typeof (String), PropReadWriteRule.ReadWrite, null);
            propDefCol.Add("OrganisationID", typeof (Guid), PropReadWriteRule.ReadWrite, null);
            return propDefCol;
        }

        #endregion //Constructors

        #region Properties

        public Guid AddressID
        {
            get { return (Guid) GetPropertyValue("AddressID"); }
        }

        public Guid? ContactPersonID
        {
            get { return (Guid?) GetPropertyValue("ContactPersonID"); }
            set { SetPropertyValue("ContactPersonID", value); }
        }

        public string AddressLine1
        {
            get { return (string) GetPropertyValue("AddressLine1"); }
            set { SetPropertyValue("AddressLine1", value); }
        }

        public string AddressLine2
        {
            get { return (string) GetPropertyValue("AddressLine2"); }
            set { SetPropertyValue("AddressLine2", value); }
        }

        public string AddressLine3
        {
            get { return (string) GetPropertyValue("AddressLine3"); }
            set { SetPropertyValue("AddressLine3", value); }
        }

        public string AddressLine4
        {
            get { return (string) GetPropertyValue("AddressLine4"); }
            set { SetPropertyValue("AddressLine4", value); }
        }

        public ContactPersonTestBO ContactPersonTestBO
        {
            get { return Relationships.GetRelatedObject<ContactPersonTestBO>("ContactPersonTestBO"); }
        }

        public Guid? OrganisationID
        {
            get { return (Guid?) GetPropertyValue("OrganisationID"); }
            set { SetPropertyValue("OrganisationID", value); }
        }

        #endregion //Properties

        #region Relationships

        public ContactPersonTestBO GetContactPerson()
        {
            return Relationships.GetRelatedObject<ContactPersonTestBO>("ContactPersonTestBO");
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

        public void SetDeletable(bool isDeletable)
        {
            _isDeletable = isDeletable;
        }

        public override bool IsDeletable(out string message)
        {
            message = "";
            return _isDeletable;
        }

        public override string ToString()
        {
            return ID.GetObjectId();
        }
    }
}