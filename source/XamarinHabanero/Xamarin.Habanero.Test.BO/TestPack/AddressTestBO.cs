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
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.Test.BO
{
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

        protected static IClassDef GetClassDef()
        {
            return ClassDef.IsDefined(typeof (AddressTestBO))
                       ? ClassDef.ClassDefs[typeof (AddressTestBO)]
                       : LoadDefaultClassDef();
        }

        protected override IClassDef ConstructClassDef()
        {
            return GetClassDef();
        }

        public static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""AddressTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person_address"">
					<property name=""AddressID"" type=""Guid"" />
                    <property name=""ContactPersonID"" type=""Guid"" />
					<property name=""OrganisationID"" type=""Guid""  />
                    <property name=""AddressLine1""  />
					<property name=""AddressLine2""  />
					<property name=""AddressLine3""  />
					<property name=""AddressLine4""  />
					<primaryKey>
						<prop name=""AddressID"" />
					</primaryKey>
                    <relationship name=""ContactPersonTestBO"" type=""single"" relatedClass=""ContactPersonTestBO"" relatedAssembly=""Habanero.Test.BO"" reverseRelationship=""Addresses"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
                    <ui>
						<grid>
							<column heading=""AddressLine1"" property=""AddressLine1"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""AddressLine2"" property=""AddressLine2"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""AddressLine3"" property=""AddressLine3"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""AddressLine4"" property=""AddressLine4"" type=""DataGridViewTextBoxColumn"" />
						</grid>
					</ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;

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
            set { Relationships.SetRelatedObject("ContactPersonTestBO", value); }
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

        public static void CreateContactPersonAddressTable(string cpAddressTableName, string cpTableName)
        {
             if (BORegistry.DataAccessor is DataAccessorInMemory)
            {
                return;
            }
            if (BORegistry.DataAccessor is DataAccessorMultiSource)
            {
                return;
            }

        }

        public static void DropCpAddressTable(string cpAddressTableName)
        {
            if (BORegistry.DataAccessor is DataAccessorInMemory)
            {
                return;
            }
            if (BORegistry.DataAccessor is DataAccessorMultiSource)
            {
                return;
            }
            var sql = "DROP TABLE " + cpAddressTableName;
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
            return ID.AsString_CurrentValue();
        }


        public static AddressTestBO CreateSavedAddress(ContactPerson contactPerson, string firstLine)
        {
            AddressTestBO address = CreateUnsavedAddress(contactPerson, firstLine);
            address.Save();
            return address;
        }

        public static AddressTestBO CreateSavedAddress(Guid contactPersonID, string firstLine)
        {
            AddressTestBO address = CreateUnsavedAddress(contactPersonID, firstLine);
            address.Save();
            return address;
        }

        public static AddressTestBO CreateSavedAddress(ContactPerson contactPerson)
        {
            AddressTestBO address = CreateUnsavedAddress(contactPerson);
            address.Save();
            return address;
        }

        public static AddressTestBO CreateSavedAddress(Guid contactPersonID)
        {
            AddressTestBO address = CreateUnsavedAddress(contactPersonID);
            address.Save();
            return address;
        }

        public static AddressTestBO CreateUnsavedAddress(Guid contactPersonID)
        {
            return CreateUnsavedAddress(contactPersonID, TestUtil.GetRandomString());
        }

        public static AddressTestBO CreateUnsavedAddress(ContactPerson contactPerson, string firstLine)
        {
            return CreateUnsavedAddress(contactPerson.ContactPersonID, firstLine);

        }

        public static AddressTestBO CreateUnsavedAddress(Guid contactPersonID, string firstLine)
        {
            AddressTestBO address = new AddressTestBO();
            address.ContactPersonID = contactPersonID;
            address.AddressLine1 = firstLine;
            return address;
        }

        public static AddressTestBO CreateUnsavedAddress(ContactPerson contactPerson)
        {
            return CreateUnsavedAddress(contactPerson.ContactPersonID);
        }

        public static AddressTestBO CreateUnsavedAddress(ContactPersonTestBO contactPerson, string firstLine)
        {
            AddressTestBO address = new AddressTestBO();
            address.ContactPersonTestBO = contactPerson;
            address.AddressLine1 = firstLine;
            return address;
        }
        public static AddressTestBO CreateUnsavedAddress(ContactPersonTestBO contactPerson)
        {
            return CreateUnsavedAddress(contactPerson, TestUtil.GetRandomString());
        }
    }
}