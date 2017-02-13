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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    public class ContactPersonTestBO : BusinessObject<ContactPersonTestBO>
    {
        public ContactPersonTestBO() : base() { }
        public ContactPersonTestBO(ClassDef def) : base(def) { }

        #region ContactType enum

        public enum ContactType
        {
            Family,
            Friend,
            Business
        }

        #endregion

        private bool _afterLoadCalled;

        public static IClassDef LoadDefaultClassDef_WOrganisationID()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
                    <property name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        private static XmlClassLoader CreateXmlClassLoader()
        {
            return new XmlClassLoader(new DtdLoader(), new DefClassFactory());
        }

        public static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDefWithEnum()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
					<property name=""ContactType"" assembly=""Habanero.Test.BO"" type=""ContactPersonTestBO+ContactType"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDefWithPersonTestBOSuperClass()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
                     <superClass class=""PersonTestBO"" assembly=""Habanero.Test.BO"" />
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDefWithKeyOnSurname()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
                    <key name=""Surname"">
                        <prop name=""Surname"" />
                    </key>
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadDefaultClassDef_W_IntegerProperty()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
					<property name=""IntegerProperty"" type=""Int32"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadDefaultClassDefWithUIDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
					<property name=""DateOfBirth"" type=""DateTime"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Surname"" property=""Surname"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Surname"" property=""Surname"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""First Name"" property=""FirstName"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
//            XmlClassLoader _loader = CreateXmlClassLoader();
//            ClassDef _classDef =
//                itsLoader.LoadClass(
//                    @"
//				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
//					<property name=""ContactPersonID"" type=""Guid"" />
//					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
//                    <property name=""FirstName"" databaseField=""FirstName_field"" />
//					<property name=""DateOfBirth"" type=""DateTime"" />
//					<primaryKey>
//						<prop name=""ContactPersonID"" />
//					</primaryKey>
//					<ui>
//						<grid>
//							<column heading=""Surname"" property=""Surname"" type=""DataGridViewTextBoxColumn"" />
//							<column heading=""FirstName"" property=""FirstName"" type=""DataGridViewComboBoxColumn"" />
//						</grid>
//						<form>
//							<tab name=""Tab1"">
//								<columnLayout>
//									<field label=""Surname"" property=""Surname"" type=""TextBox"" mapperType=""TextBoxMapper"" />
//									<field label=""First Name"" property=""FirstName"" type=""TextBox"" mapperType=""TextBoxMapper"" />
//								</columnLayout>
//							</tab>
//						</form>
//					</ui>
//			    </class>
//			");
//            ClassDef.ClassDefs.Add(_classDef);
//            return _classDef;
        }


        public static IClassDef LoadClassDefWithSurnameAsPrimaryKey()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
					<primaryKey isObjectID=""false"" >
						<prop name=""Surname"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithSurnameAsPrimaryKey_WriteNew()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" readWriteRule=""WriteNew"" compulsory=""true"" />
					<primaryKey isObjectID=""false"" >
						<prop name=""Surname"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithCompositePrimaryKey()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
					<primaryKey isObjectID=""false"" >
						<prop name=""ContactPersonID"" />
                        <prop name=""Surname"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithCompositeAlternateKey()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
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

        public static IClassDef LoadClassDefWithCompositePrimaryKeyNameSurname()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property name=""ContactPersonID"" type=""Guid"" />
					<property name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property name=""FirstName"" databaseField=""FirstName_field"" />
                    <primaryKey isObjectID=""false""  >
                        <prop name=""Surname"" />
                        <prop name=""FirstName"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadFullClassDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithAddressesRelationship_DeleteRelated()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"" reverseRelationship=""ContactPersonTestBO"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            if (!ClassDef.ClassDefs.Contains(typeof(AddressTestBO)))
                AddressTestBO.LoadDefaultClassDef();
            return itsClassDef;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableSuffix">used to suffix onto the address and contact person table s.t. we do not have interacting tests</param>
        /// <returns></returns>
        public static IClassDef LoadClassDefWithAddressesRelationship_DeleteRelated(string tableSuffix)
        {
            var itsLoader = CreateXmlClassLoader();
            var personClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"" reverseRelationship=""ContactPersonTestBO"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");

            ClassDef.ClassDefs.Add(personClassDef);
            if (!ClassDef.ClassDefs.Contains(typeof(AddressTestBO)))
            {
                var addressClassDef = AddressTestBO.LoadDefaultClassDef();
                addressClassDef.TableName += "_" + tableSuffix;
            }
            personClassDef.TableName += "_" + tableSuffix;
            return personClassDef;
        }

        public static IClassDef LoadClassDefWithAddresBOsRelationship_AddressReverseRelationshipConfigured()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""AddressTestBOs"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"" reverseRelationship=""ContactPersonTestBO"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");
            XmlClassLoader addressLoader = CreateXmlClassLoader();
            IClassDef addressClassDef = addressLoader.LoadClass
                (@"				
                <class name=""AddressTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person_address"">
					<property  name=""ContactPersonID"" compulsory=""true"" type=""Guid"" />
					<property  name=""AddressID"" compulsory=""true"" type=""Guid""/>
                    <property  name=""AddressLine1"" />
					<primaryKey>
						<prop name=""AddressID"" />
					</primaryKey>
					<relationship name=""ContactPersonTestBO"" type=""single"" relatedClass=""ContactPersonTestBO"" relatedAssembly=""Habanero.Test.BO"" reverseRelationship=""AddressTestBOs"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>");
            ClassDef.ClassDefs.Add(itsClassDef);
            ClassDef.ClassDefs.Add(addressClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithAddressesRelationship_PreventDelete()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""Prevent"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            if (!ClassDef.ClassDefs.Contains(typeof(AddressTestBO)))
                AddressTestBO.LoadDefaultClassDef();
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithAddressTestBOsRelationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""AddressTestBOs"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""Prevent"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithAddressesRelationship_PreventDelete_WithUIDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""Address"" relatedAssembly=""Habanero.Test"" deleteAction=""Prevent"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""Surname"" property=""Surname"" type=""DataGridViewTextBoxColumn"" />
							<column heading=""FirstName"" property=""FirstName"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Surname"" property=""Surname"" type=""TextBox"" mapperType=""TextBoxMapper"" />
									<field label=""First Name"" property=""FirstName"" type=""TextBox"" mapperType=""TextBoxMapper"" />
								</columnLayout>
							</tab>
						</form>
					</ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithAddressesRelationship_DeleteDoNothing()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DoNothing"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);

            if (!ClassDef.ClassDefs.Contains(typeof(AddressTestBO)))
                AddressTestBO.LoadDefaultClassDef();
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithAddressesRelationship_DeleteDoNothing(string tableSuffix)
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DoNothing"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);

            if (!ClassDef.ClassDefs.Contains(typeof(AddressTestBO)))
            {
                var addressClassDef = AddressTestBO.LoadDefaultClassDef();
                addressClassDef.TableName += "_" + tableSuffix;
            }
            itsClassDef.TableName += "_" + tableSuffix;
            return itsClassDef;
        }




        public static IClassDef LoadClassDefWithAddressesRelationship_SortOrder_AddressLine1()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DoNothing"" orderBy=""AddressLine1"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            if (!ClassDef.ClassDefs.Contains(typeof(AddressTestBO)))
                AddressTestBO.LoadDefaultClassDef();
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithImageProperty()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""Image"" type=""Android.Media.Image"" assembly=""Mono.Android"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDefWithUIDef_ReadWriteRule()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""Surname"" property=""Surname"" type=""DataGridViewTextBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Surname"" property=""Surname"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""readWriteRule"" value=""WriteNew"" />
                                     </field>
									<field label=""First Name"" property=""FirstName"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""readWriteRule"" value=""WriteNotNew"" />
                                     </field>
								</columnLayout>
							</tab>
						</form>
					</ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDef_NoOrganisationRelationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" databaseField=""Surname_field"" compulsory=""true"" />
                    <property  name=""FirstName"" databaseField=""FirstName_field"" compulsory=""true"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" >
                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
					<ui>
						<grid>
							<column heading=""OrganisationID"" property=""OrganisationID"" type=""DataGridViewComboBoxColumn"" />
						</grid>
						<form>
							<tab name=""Tab1"">
								<columnLayout>
									<field label=""Surname"" property=""Surname"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""readWriteRule"" value=""WriteNew"" />
                                     </field>
									<field label=""First Name"" property=""FirstName"" type=""TextBox"" mapperType=""TextBoxMapper"" >
                                        <parameter name=""readWriteRule"" value=""WriteNotNew"" />
                                     </field>
								</columnLayout>
							</tab>
						</form>
                    </ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }


//        public static ClassDef LoadClassDef_NoOrganisationRelationshipDefined()
//        {
//            XmlClassLoader itsLoader = CreateXmlClassLoader();
//            ClassDef itsClassDef =
//                itsLoader.LoadClass(
//                    @"
//				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
//					<property  name=""ContactPersonID"" type=""Guid"" />
//					<property  name=""Surname"" compulsory=""true"" databaseField=""Surname_field"" />
//                    <property  name=""FirstName"" compulsory=""true"" databaseField=""FirstName_field"" />
//					<property  name=""DateOfBirth"" type=""DateTime"" />
//                    <property  name=""OrganisationID"" type=""Guid"" >
//                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
//                    </property>
//					<primaryKey>
//						<prop name=""ContactPersonID"" />
//					</primaryKey>
//					<ui>
//						<grid>
//							<column heading=""OrganisationID"" property=""OrganisationID"" type=""DataGridViewComboBoxColumn"" />
//						</grid>
//                    </ui>
//			    </class>
//			");
//            ClassDef.ClassDefs.Add(itsClassDef);
//            return itsClassDef;
//        }
        public static IClassDef LoadClassDefOrganisationTestBORelationship_MultipleReverse()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" databaseField=""Surname_field"" />
                    <property  name=""FirstName"" compulsory=""true"" databaseField=""FirstName_field"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" >
                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
                    <relationship name=""Organisation"" type=""single"" relatedClass=""OrganisationTestBO"" relatedAssembly=""Habanero.Test.BO"" reverseRelationship=""ContactPeople"" deleteAction=""DoNothing"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""OrganisationID"" property=""OrganisationID"" type=""DataGridViewComboBoxColumn"" />
						</grid>
                    </ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefOrganisationTestBORelationship_SingleReverse()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" databaseField=""Surname_field"" />
                    <property  name=""FirstName"" compulsory=""true"" databaseField=""FirstName_field"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" >
                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
                    <relationship name=""Organisation"" type=""single"" relatedClass=""OrganisationTestBO"" relatedAssembly=""Habanero.Test.BO"" reverseRelationship=""ContactPerson"" deleteAction=""DoNothing"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""OrganisationID"" property=""OrganisationID"" type=""DataGridViewComboBoxColumn"" />
						</grid>
                    </ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefOrganisationTestBORelationship_WithAutoIncrementingPK_SingleReverse()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Int32"" autoIncrementing=""true"" />
					<property  name=""Surname"" compulsory=""true"" databaseField=""Surname_field"" />
                    <property  name=""FirstName"" compulsory=""true"" databaseField=""FirstName_field"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Int32""  >
                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey isObjectID=""false"">
						<prop name=""ContactPersonID"" />
					</primaryKey>
                    <relationship name=""Organisation"" type=""single"" relatedClass=""OrganisationTestBO"" relatedAssembly=""Habanero.Test.BO"" reverseRelationship=""ContactPerson"" deleteAction=""DoNothing"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""OrganisationID"" property=""OrganisationID"" type=""DataGridViewComboBoxColumn"" />
						</grid>
                    </ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadClassDefOrganisationTestBORelationship_SingleReverse_NoReverse()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" databaseField=""Surname_field"" />
                    <property  name=""FirstName"" compulsory=""true"" databaseField=""FirstName_field"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" >
                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
                    <relationship name=""Organisation"" type=""single"" relatedClass=""OrganisationTestBO"" relatedAssembly=""Habanero.Test.BO"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""OrganisationID"" property=""OrganisationID"" type=""DataGridViewComboBoxColumn"" />
						</grid>
                    </ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefOrganisationTestBORelationship_SingleCompositeReverse()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""Surname"" compulsory=""true"" databaseField=""Surname_field"" />
                    <property  name=""FirstName"" compulsory=""true"" databaseField=""FirstName_field"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" >
                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey isObjectID = ""false"">
						<prop name=""Surname"" />
						<prop name=""OrganisationID"" />
					</primaryKey>
                    <relationship name=""Organisation"" type=""single"" relatedClass=""OrganisationTestBO"" relatedAssembly=""Habanero.Test.BO""                                           reverseRelationship=""ContactPerson"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""OrganisationID"" property=""OrganisationID"" type=""DataGridViewComboBoxColumn"" />
						</grid>
                    </ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefOrganisationTestBOTwoRelationships()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" databaseField=""Surname_field"" />
                    <property  name=""FirstName"" compulsory=""true"" databaseField=""FirstName_field"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" >
                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
                    <relationship name=""Organisation1"" type=""single"" relatedClass=""OrganisationTestBO"" relatedAssembly=""Habanero.Test.BO"" >
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
                    <relationship name=""Organisation2"" type=""single"" relatedClass=""OrganisationTestBO"" relatedAssembly=""Habanero.Test.BO"" >
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""OrganisationID"" property=""OrganisationID"" type=""DataGridViewComboBoxColumn"" />
						</grid>
                    </ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadClassDefWithOrganisationAndAddressRelationships()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""ContactPersonTestBO"" assembly=""Habanero.Test.BO"" table=""contact_person"">
					<property  name=""ContactPersonID"" type=""Guid"" />
					<property  name=""Surname"" compulsory=""true"" databaseField=""Surname_field"" />
                    <property  name=""FirstName"" compulsory=""true"" databaseField=""FirstName_field"" />
					<property  name=""DateOfBirth"" type=""DateTime"" />
                    <property  name=""OrganisationID"" type=""Guid"" >
                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey>
						<prop name=""ContactPersonID"" />
					</primaryKey>
                    <relationship name=""Organisation"" type=""single"" relatedClass=""OrganisationTestBO"" relatedAssembly=""Habanero.Test.BO"" reverseRelationship=""ContactPeople"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DoNothing"" reverseRelationship=""ContactPersonTestBO"">
						<relatedProperty property=""ContactPersonID"" relatedProperty=""ContactPersonID"" />
					</relationship>
					<ui>
						<grid>
							<column heading=""OrganisationID"" property=""OrganisationID"" type=""DataGridViewComboBoxColumn"" />
						</grid>
                    </ui>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }


        public override string ToString()
        {
            return Surname;
        }

        public static void DeleteAllContactPeople()
        {
            return;
        }


        public static void DeleteAllContactPeople(string contactPersonTableName)
        {
            return;
        }

        public static void DropContactPersonTable(string contactPersonTableName)
        {
            return;
        }

        public static void CreateContactPersonTable(string tableName)
        {
            return;
        }
        
        public static void CreateSampleData()
        {
            ClassDef.ClassDefs.Clear();
            LoadFullClassDef();

            string[] surnames = {"zzz", "abc", "abcd"};
            string[] firstNames = {"a", "aa", "aa"};

            for (int i = 0; i < surnames.Length; i++)
            {
                Criteria criteria = new Criteria("Surname", Criteria.ComparisonOp.Equals, surnames[i]);
                if (BORegistry.DataAccessor.BusinessObjectLoader.GetBusinessObject<ContactPersonTestBO>(criteria) != null) continue;
                ContactPersonTestBO contact = new ContactPersonTestBO {Surname = surnames[i], FirstName = firstNames[i]};
                contact.Save();
            }
            ClassDef.ClassDefs.Clear();
        }

        public static ContactPersonTestBO CreateSavedContactPersonNoAddresses()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = TestUtil.GetRandomString();
            contactPersonTestBO.FirstName = TestUtil.GetRandomString();
            contactPersonTestBO.Save();
            return contactPersonTestBO;
        }

        public static ContactPersonTestBO CreateContactPersonWithOneAddress_CascadeDelete(out AddressTestBO address, string tableNameExtension)
        {
            LoadClassDefWithAddressesRelationship_DeleteRelated(tableNameExtension);
            return CreateContactPerson(out address);
        }

        public static ContactPersonTestBO CreateContactPersonWithOneAddress_PreventDelete(out AddressTestBO address)
        {
            LoadClassDefWithAddressesRelationship_PreventDelete();
            return CreateContactPerson(out address);
        }

        private static ContactPersonTestBO CreateContactPerson(out AddressTestBO address)
        {
            ContactPersonTestBO contactPersonTestBO = CreateSavedContactPersonNoAddresses();
            address = contactPersonTestBO.Addresses.CreateBusinessObject();
            address.Save();
            RelatedBusinessObjectCollection<AddressTestBO> collection = contactPersonTestBO.Addresses;
            Assert.AreEqual(1, collection.Count);
            return contactPersonTestBO;
        }
        private static ContactPersonTestBO CreateContactPersonTestBO(out AddressTestBO address)
        {
            ContactPersonTestBO contactPersonTestBO = CreateSavedContactPersonNoAddresses();
            address = contactPersonTestBO.AddressTestBOs.CreateBusinessObject();
            address.Save();
            Assert.AreEqual(1, contactPersonTestBO.AddressTestBOs.Count);
            return contactPersonTestBO;
        }

        public static ContactPersonTestBO CreateContactPersonWithOneAddress_DeleteDoNothing(out AddressTestBO address)
        {
            LoadClassDefWithAddressesRelationship_DeleteDoNothing();
            return CreateContactPerson(out address);
        }

        public static ContactPersonTestBO CreateContactPersonWithOneAddressTestBO(out AddressTestBO address)
        {
            LoadClassDefWithAddresBOsRelationship_AddressReverseRelationshipConfigured();
            return CreateContactPersonTestBO(out address);
        }

        public static ContactPersonTestBO CreateSavedContactPerson(DateTime? dteBirth, string surname, string firstName)
        {
            ContactPersonTestBO contact = CreateUnsavedContactPerson(surname, firstName);
            if (dteBirth != null)
            {
                contact.DateOfBirth = dteBirth.Value;
            }
            contact.Save();
            return contact;
        }
        public static ContactPersonTestBO CreateSavedContactPerson(string surname, string firstName)
        {
            ContactPersonTestBO contact = CreateUnsavedContactPerson(surname, firstName);
            contact.Save();
            return contact;
        }

        public static ContactPersonTestBO CreateSavedContactPerson()
        {
            var contact = CreateUnsavedContactPerson();
            contact.Save();
            return contact;
        }

        public static ContactPersonTestBO CreateSavedContactPerson(string surname)
        {
            ContactPersonTestBO contact = CreateUnsavedContactPerson(surname);
            contact.Save();
            return contact;
        }

        public static ContactPersonTestBO CreateSavedContactPerson(DateTime dateOfBirth)
        {
            ContactPersonTestBO contact = CreateUnsavedContactPerson(dateOfBirth);
            contact.Save();
            return contact;
        }

        public static ContactPersonTestBO CreateSavedContactPerson(DateTime dateOfBirth, string surname)
        {
            ContactPersonTestBO contact = CreateUnsavedContactPerson(dateOfBirth, surname);
            contact.Save();
            return contact;
        }

        public static List<ContactPersonTestBO> CreateManySavedContactPersons(int numberOfItems)
        {
            var originalCps = new List<ContactPersonTestBO>();
            for (int i = 0; i < numberOfItems; i++)
            {
                originalCps.Add(CreateSavedContactPerson());
            }
            return originalCps;
        }

        public static ContactPersonTestBO CreateUnsavedContactPerson()
        {
            return CreateUnsavedContactPerson(TestUtil.GetRandomString());
        }

        public static ContactPersonTestBO CreateUnsavedContactPerson_NoFirstNameProp()
        {
            return CreateUnsavedContactPerson_NoFirstNameProp(TestUtil.GetRandomString());
        }

        private static ContactPersonTestBO CreateUnsavedContactPerson(DateTime dateOfBirth)
        {
            ContactPersonTestBO contact = CreateUnsavedContactPerson();
            contact.DateOfBirth = dateOfBirth;
            return contact;
        }

        public static ContactPersonTestBO CreateUnsavedContactPerson(string surname)
        {
            return CreateUnsavedContactPerson(surname, TestUtil.GetRandomString());
        }

        public static ContactPersonTestBO CreateUnsavedContactPerson_NoFirstNameProp(string surname)
        {
            ContactPersonTestBO contact = new ContactPersonTestBO();
            contact.ContactPersonID = Guid.NewGuid();
            contact.Surname = surname;
            return contact;
        }

        private static ContactPersonTestBO CreateUnsavedContactPerson(DateTime dateOfBirth, string surname)
        {
            ContactPersonTestBO contact = CreateUnsavedContactPerson(surname);
            contact.DateOfBirth = dateOfBirth;
            return contact;
        }

        public static ContactPersonTestBO CreateUnsavedContactPerson(string surname, string firstName)
        {
            ContactPersonTestBO contact = new ContactPersonTestBO {Surname = surname, FirstName = firstName};
            return contact;
        }

        public string ReflectiveProp { get; set; }
        #region Properties

        public Guid ContactPersonID
        {
            get { return (Guid) GetPropertyValue("ContactPersonID"); }
            set { SetPropertyValue("ContactPersonID", value); }
        }

        public string Surname
        {
            get { return (string) GetPropertyValue("Surname"); }
            set { SetPropertyValue("Surname", value); }
        }

        public string SurnameAsExpression
        {
            get { return GetPropertyValue(bo => bo.Surname); }
            set { SetPropertyValue(bo => bo.Surname, value); }
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

        public DateTime DateOfBirthAsExpression
        {
            get { return GetPropertyValue(bo => bo.DateOfBirth); }
            set { SetPropertyValue(bo => bo.DateOfBirth, value); }
        }

        public RelatedBusinessObjectCollection<AddressTestBO> Addresses
        {
            get
            {
                return
                    (RelatedBusinessObjectCollection<AddressTestBO>)Relationships.GetRelatedCollection<AddressTestBO>("Addresses");
            }
        }

        public Guid? OrganisationID
        {
            get { return (Guid?)GetPropertyValue("OrganisationID"); }
            set { SetPropertyValue("OrganisationID", value); }
        }

        public bool AfterLoadCalled
        {
            get { return _afterLoadCalled; }
            set { _afterLoadCalled = value; }
        }

        public RelatedBusinessObjectCollection<AddressTestBO> AddressTestBOs
        {
            get
            {
                return
                    (RelatedBusinessObjectCollection<AddressTestBO>)
                    Relationships.GetRelatedCollection<AddressTestBO>("AddressTestBOs");
            }
        }
        public OrganisationTestBO Organisation
        {
            get { return Relationships.GetRelatedObject<OrganisationTestBO>("Organisation"); }
            set { Relationships.SetRelatedObject("Organisation", value); }
        }

        protected internal override void AfterLoad()
        {
            base.AfterLoad();
            _afterLoadCalled = true;
        }

        #endregion //Properties

        public static ContactPersonTestBO CreateSavedContactPerson_AsChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO myBO = CreateUnsavedContactPerson_AsChild(cpCol);
            myBO.Save();
            return myBO;
        }

        public static ContactPersonTestBO CreateUnsavedContactPerson_AsChild(BusinessObjectCollection<ContactPersonTestBO> cpCol)
        {
            ContactPersonTestBO myBO = cpCol.CreateBusinessObject();
            myBO.Surname = TestUtil.GetRandomString();
            myBO.FirstName = TestUtil.GetRandomString();
            return myBO;
        }
    }
}
