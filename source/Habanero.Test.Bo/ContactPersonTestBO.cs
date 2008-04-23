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

using System;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    class ContactPersonTestBO: BusinessObject
    {
        private bool _afterLoadCalled;

        public enum ContactType
        {
            Family,
            Friend,
            Business
        }

        public ContactPersonTestBO() { }

        internal ContactPersonTestBO(BOPrimaryKey id) : base(id) { }

        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
                    <property  name=""FirstName"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
			ClassDef.ClassDefs.Add(itsClassDef);
			return itsClassDef;
        }

        public static ClassDef LoadClassDefWithSurnameAsPrimaryKey()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
					<primaryKey isObjectID=""false"" >
						<prop name=""Surname"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static ClassDef LoadClassDefWithCompositePrimaryKey()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
					<primaryKey isObjectID=""false"" >
						<prop name=""ContactPersonID"" />
                        <prop name=""Surname"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDefWithCompositeAlternateKey()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
                    <property name=""FirstName"" />
                    <key name=""AlternateKey"">
                        <prop name=""Surname"" />
                        <prop name=""FirstName"" />
                    </key>
                    <primaryKey >
						<prop name=""ContactPersonID"" />
					</primaryKey>

			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDefWithCompositePrimaryKeyNameSurname()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
                    <property name=""FirstName"" />
                    <primaryKey isObjectID=""false""  >
                        <prop name=""Surname"" />
                        <prop name=""FirstName"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadFullClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
                    <property  name=""FirstName"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDefWithAddressesRelationship_DeleteRelated()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
                    <property  name=""FirstName"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""Address"" relatedAssembly=""Habanero.Test"" deleteAction=""DeleteRelated"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static ClassDef LoadClassDefWithAddressesRelationship_PreventDelete()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" />
                    <property  name=""FirstName"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""Address"" relatedAssembly=""Habanero.Test"" deleteAction=""Prevent"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        #region Properties

        public Guid ContactPersonID
        {
            get { return (Guid)GetPropertyValue("ContactPersonID"); }
            set { this.SetPropertyValue("ContactPersonID", value); }
        }

        public string Surname
        {
            get { return (string)GetPropertyValue("Surname"); }
            set { SetPropertyValue("Surname", value); }
        }
        public string FirstName
        {
            get { return (string)GetPropertyValue("FirstName"); }
            set { SetPropertyValue("FirstName", value); }
        }

        public DateTime DateOfBirth
        {
            get { return (DateTime)GetPropertyValue("DateOfBirth"); }
            set { SetPropertyValue("DateOfBirth", value); }
        }

        public RelatedBusinessObjectCollection<Address> Addresses
        {
            get { return (RelatedBusinessObjectCollection<Address>)this.Relationships.GetRelatedCollection<Address>("Addresses"); }
        }

        public bool AfterLoadCalled
        {
            get { return _afterLoadCalled; }
        }


        protected internal override void AfterLoad()
        {
            base.AfterLoad();
            _afterLoadCalled = true;
        }

        #endregion //Properties

        public override string ToString()
        {
            return Surname;
        }

        /// <summary>
        /// returns the ContactPersonTestBO identified by id.
        /// </summary>
        /// <remarks>
        /// If the Contact person is already leaded then an identical copy of it will be returned.
        /// </remarks>
        /// <param name="id">The object primary Key</param>
        /// <returns>The loaded business object</returns>
        /// <exception cref="BusObjDeleteConcurrencyControlException">
        ///  if the object has been deleted already</exception>
        public static ContactPersonTestBO GetContactPerson(BOPrimaryKey id)
        {
            ContactPersonTestBO myContactPersonTestBOTestBO = (ContactPersonTestBO)BOLoader.GetLoadedBusinessObject(id);
            if (myContactPersonTestBOTestBO == null)
            {
                myContactPersonTestBOTestBO = new ContactPersonTestBO(id);
            }
            return myContactPersonTestBOTestBO;
        }

        internal static void DeleteAllContactPeople()
        {
            string sql = "DELETE FROM contact_person_address";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
            sql = "DELETE FROM Contact_Person";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }

        public static void CreateSampleData()
        {
            ClassDef.ClassDefs.Clear();
            LoadFullClassDef();

            string[] surnames = {"zzz", "abc", "abcd"};
            string[] firstNames = {"a", "aa", "aa"};

            for (int i = 0; i<surnames.Length; i++)
            {
                if (BOLoader.Instance.GetBusinessObject<ContactPersonTestBO>("surname = " + surnames[i]) == null)
                {
                    ContactPersonTestBO contact = new ContactPersonTestBO();
                    contact.Surname = surnames[i];
                    contact.FirstName = firstNames[i];
                    contact.Save();
                }
            }
            ClassDef.ClassDefs.Clear();
        }

        public static ContactPersonTestBO CreateSavedContactPersonNoAddresses()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = Guid.NewGuid().ToString();
            contactPersonTestBO.FirstName = Guid.NewGuid().ToString();
            contactPersonTestBO.Save();
            return contactPersonTestBO;
        }

        public static ContactPersonTestBO CreateContactPersonWithOneAddress_CascadeDelete(out Address address)
        {
            LoadClassDefWithAddressesRelationship_DeleteRelated();
            ContactPersonTestBO contactPersonTestBO = CreateSavedContactPersonNoAddresses();
            address = contactPersonTestBO.Addresses.CreateBusinessObject();
            address.Save();
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
            return contactPersonTestBO;
        }
        public static ContactPersonTestBO CreateContactPersonWithOneAddress_PreventDelete(out Address address)
        {
            LoadClassDefWithAddressesRelationship_PreventDelete();
            ContactPersonTestBO contactPersonTestBO = CreateSavedContactPersonNoAddresses();
            address = contactPersonTestBO.Addresses.CreateBusinessObject();
            address.Save();
            Assert.AreEqual(1, contactPersonTestBO.Addresses.Count);
            return contactPersonTestBO;
        }
    }
}
