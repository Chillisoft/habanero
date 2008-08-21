
// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class OrganisationPerson
    {
        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""OrganisationPerson"" assembly=""Habanero.Test.Structure.BO"" table=""table_OrganisationPerson"">
			    <property name=""OrganisatiionID"" type=""Guid"" databaseField=""field_Organisatiion_ID"" />
			    <property name=""PersonID"" type=""Guid"" databaseField=""field_Person_ID"" />
			    <property name=""Relationship"" databaseField=""field_Relationship"" />
			    <primaryKey isObjectID=""false"">
			      <prop name=""OrganisatiionID"" />
			      <prop name=""PersonID"" />
			    </primaryKey>
			    <relationship name=""Organisation"" type=""single"" relatedClass=""Organisation"" relatedAssembly=""Habanero.Test.Structure.BO"">
			      <relatedProperty property=""OrganisatiionID"" relatedProperty=""OrganisationID"" />
			    </relationship>
			    <relationship name=""Person"" type=""single"" relatedClass=""Person"" relatedAssembly=""Habanero.Test.Structure.BO"">
			      <relatedProperty property=""PersonID"" relatedProperty=""PersonID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static OrganisationPerson CreateSavedOrganisationPerson()
        {
            OrganisationPerson organisationPerson = CreateUnsavedOrganisationPerson();
            organisationPerson.Save();
            return organisationPerson;
        }

        public static OrganisationPerson CreateSavedOrganisationPerson(Person person, Organisation organisation)
        {
            OrganisationPerson organisationPerson = CreateUnsavedOrganisationPerson(person, organisation);
            organisationPerson.Save();
            return organisationPerson;
        }

        private static OrganisationPerson CreateUnsavedOrganisationPerson()
        {
            return CreateUnsavedOrganisationPerson(null, null);
        }

        private static OrganisationPerson CreateUnsavedOrganisationPerson(Person person, Organisation organisation)
        {
            OrganisationPerson organisationPerson = new OrganisationPerson();
            organisationPerson.Person = person;
            organisationPerson.Organisation = organisation;
            return organisationPerson;
        }
    }
}
