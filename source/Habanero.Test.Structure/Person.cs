
// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Person
    {
        public new static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Person"" assembly=""Habanero.Test.Structure.BO"" table=""table_Person"">
			    <property name=""IDNumber"" databaseField=""field_ID_Number"" />
			    <property name=""FirstName"" databaseField=""field_First_Name"" />
			    <property name=""LastName"" databaseField=""field_Last_Name"" />
			    <property name=""PersonID"" databaseField=""field_Person_ID"" compulsory=""true"" />
			    <primaryKey>
			      <prop name=""PersonID"" />
			    </primaryKey>
			    <relationship name=""CarsDriven"" type=""multiple"" relatedClass=""Car"" relatedAssembly=""Habanero.Test.Structure.BO"">
			      <relatedProperty property=""PersonID"" relatedProperty=""DriverID"" />
			    </relationship>
			    <relationship name=""OrganisationPerson"" type=""multiple"" relatedClass=""OrganisationPerson"" relatedAssembly=""Habanero.Test.Structure.BO"">
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
			  <class name=""Person"" assembly=""Habanero.Test.Structure.BO"" table=""table_Person"">
			    <superClass class=""LegalEntity"" assembly=""Habanero.Test.Structure.BO"" />
			    <property name=""IDNumber"" databaseField=""field_ID_Number"" />
			    <property name=""FirstName"" databaseField=""field_First_Name"" />
			    <property name=""LastName"" databaseField=""field_Last_Name"" />
			    <property name=""PersonID"" databaseField=""field_Person_ID"" compulsory=""true"" />
			    <primaryKey>
			      <prop name=""PersonID"" />
			    </primaryKey>
			    <relationship name=""CarsDriven"" type=""multiple"" relatedClass=""Car"" relatedAssembly=""Habanero.Test.Structure.BO"">
			      <relatedProperty property=""PersonID"" relatedProperty=""DriverID"" />
			    </relationship>
			    <relationship name=""OrganisationPerson"" type=""multiple"" relatedClass=""OrganisationPerson"" relatedAssembly=""Habanero.Test.Structure.BO"">
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
            return CreateUnsavedPerson(TestUtil.CreateRandomString());
        }

        private static Person CreateUnsavedPerson(string firstName)
        {
            Person person = new Person();
            person.FirstName = firstName;
            return person;
        }
    }
}
