// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    public partial class OrganisationPerson
    {
        public static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""OrganisationPerson"" assembly=""Habanero.Test.Structure"" table=""table_OrganisationPerson"">
			    <property name=""OrganisatiionID"" type=""Guid"" databaseField=""field_Organisatiion_ID"" />
			    <property name=""PersonID"" type=""Guid"" databaseField=""field_Person_ID"" />
			    <property name=""Relationship"" databaseField=""field_Relationship"" />
			    <primaryKey isObjectID=""false"">
			      <prop name=""OrganisatiionID"" />
			      <prop name=""PersonID"" />
			    </primaryKey>
			    <relationship name=""Organisation"" type=""single"" relatedClass=""Organisation"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""OrganisatiionID"" relatedProperty=""OrganisationID"" />
			    </relationship>
			    <relationship name=""Person"" type=""single"" relatedClass=""Person"" relatedAssembly=""Habanero.Test.Structure"">
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
