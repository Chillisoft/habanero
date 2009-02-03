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

// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using System.Runtime.Serialization;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;

    [Serializable]
    public partial class Person
    {
        public Person()
        {
        }

        protected Person(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
        public new static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Person"" assembly=""Habanero.Test.Structure"" table=""table_Person"">
			    <property name=""IDNumber"" databaseField=""field_ID_Number"" />
			    <property name=""FirstName"" databaseField=""field_First_Name"" />
			    <property name=""LastName"" databaseField=""field_Last_Name"" />
			    <property name=""PersonID"" type=""System.Guid"" databaseField=""field_Person_ID"" compulsory=""true"" />
                <property name=""DriverID"" type=""System.Guid"" />
			    <primaryKey>
			      <prop name=""PersonID"" />
			    </primaryKey>
			    <relationship name=""CarsDriven"" type=""multiple"" relatedClass=""Car"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""PersonID"" relatedProperty=""DriverID"" />
			    </relationship>
			    <relationship name=""OrganisationPerson"" type=""multiple"" relatedClass=""OrganisationPerson"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""PersonID"" relatedProperty=""PersonID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public new static ClassDef LoadClassDef_WithClassTableInheritance()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Person"" assembly=""Habanero.Test.Structure"" table=""table_class_Person"">
			    <superClass class=""LegalEntity"" assembly=""Habanero.Test.Structure"" />
			    <property name=""IDNumber"" databaseField=""field_ID_Number"" />
			    <property name=""FirstName"" databaseField=""field_First_Name"" />
			    <property name=""LastName"" databaseField=""field_Last_Name"" />
			    <property name=""PersonID"" databaseField=""field_Person_ID"" compulsory=""true"" />
			    <primaryKey>
			      <prop name=""PersonID"" />
			    </primaryKey>
			    <relationship name=""CarsDriven"" type=""multiple"" relatedClass=""Car"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""PersonID"" relatedProperty=""DriverID"" />
			    </relationship>
			    <relationship name=""OrganisationPerson"" type=""multiple"" relatedClass=""OrganisationPerson"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""PersonID"" relatedProperty=""PersonID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static Person CreateSavedPerson()
        {
            Person person = CreateUnsavedPerson();
            person.Save();
            return person;
        }

        public static Person CreateSavedPerson(string firstName)
        {
            Person person = CreateUnsavedPerson(firstName);
            person.Save();
            return person;
        }

        private static Person CreateUnsavedPerson()
        {
            return CreateUnsavedPerson(TestUtil.GetRandomString());
        }

        private static Person CreateUnsavedPerson(string firstName)
        {
            Person person = new Person();
            person.FirstName = firstName;
            return person;
        }
    }
}
