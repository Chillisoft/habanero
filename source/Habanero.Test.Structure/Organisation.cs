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
			  <class name=""Organisation"" assembly=""Habanero.Test.Structure"" table=""table_Organisation"">
			    <property name=""Name"" databaseField=""field_Name"" />
			    <property name=""DateFormed"" databaseField=""field_Date_Formed"" />
			    <property name=""OrganisationID"" type=""Guid"" databaseField=""field_Organisation_ID"" compulsory=""true"" />
			    <primaryKey>
			      <prop name=""OrganisationID"" />
			    </primaryKey>
			    <relationship name=""OrganisationPerson"" type=""multiple"" relatedClass=""OrganisationPerson"" relatedAssembly=""Habanero.Test.Structure"">
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
			  <class name=""Organisation"" assembly=""Habanero.Test.Structure"" table=""table_class_Organisation"">
			    <superClass class=""LegalEntity"" assembly=""Habanero.Test.Structure"" />
			    <property name=""Name"" databaseField=""field_Name"" />
			    <property name=""DateFormed"" databaseField=""field_Date_Formed"" />
			    <property name=""OrganisationID"" type=""Guid"" databaseField=""field_Organisation_ID"" compulsory=""true"" />
			    <primaryKey>
			      <prop name=""OrganisationID"" />
			    </primaryKey>
			    <relationship name=""OrganisationPerson"" type=""multiple"" relatedClass=""OrganisationPerson"" relatedAssembly=""Habanero.Test.Structure"">
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
