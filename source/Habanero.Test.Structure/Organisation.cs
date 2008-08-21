
// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Organisation
    {
        public new static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Organisation"" assembly=""Habanero.Test.Structure.BO"" table=""table_Organisation"">
			    <property name=""Name"" databaseField=""field_Name"" />
			    <property name=""DateFormed"" databaseField=""field_Date_Formed"" />
			    <property name=""OrganisationID"" type=""Guid"" databaseField=""field_Organisation_ID"" compulsory=""true"" />
			    <primaryKey>
			      <prop name=""OrganisationID"" />
			    </primaryKey>
			    <relationship name=""OrganisationPerson"" type=""multiple"" relatedClass=""OrganisationPerson"" relatedAssembly=""Habanero.Test.Structure.BO"">
			      <relatedProperty property=""OrganisationID"" relatedProperty=""OrganisatiionID"" />
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
			  <class name=""Organisation"" assembly=""Habanero.Test.Structure.BO"" table=""table_Organisation"">
			    <superClass class=""LegalEntity"" assembly=""Habanero.Test.Structure.BO"" />
			    <property name=""Name"" databaseField=""field_Name"" />
			    <property name=""DateFormed"" databaseField=""field_Date_Formed"" />
			    <property name=""OrganisationID"" type=""Guid"" databaseField=""field_Organisation_ID"" compulsory=""true"" />
			    <primaryKey>
			      <prop name=""OrganisationID"" />
			    </primaryKey>
			    <relationship name=""OrganisationPerson"" type=""multiple"" relatedClass=""OrganisationPerson"" relatedAssembly=""Habanero.Test.Structure.BO"">
			      <relatedProperty property=""OrganisationID"" relatedProperty=""OrganisatiionID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static Organisation CreateSavedOrganisation()
        {
            Organisation organisation = CreateUnsavedOrganisation();
            organisation.Save();
            return organisation;
        }

        public static Organisation CreateSavedOrganisation(string name)
        {
            Organisation organisation = CreateUnsavedOrganisation(name);
            organisation.Save();
            return organisation;
        }

        private static Organisation CreateUnsavedOrganisation()
        {
            return CreateUnsavedOrganisation(TestUtil.CreateRandomString());
        }

        private static Organisation CreateUnsavedOrganisation(string name)
        {
            Organisation organisation = new Organisation();
            organisation.Name = name;
            return organisation;
        }
    }
}
