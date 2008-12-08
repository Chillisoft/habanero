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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.BO
{
    public class OrganisationTestBO : BusinessObject
    {

        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
					<relationship name=""ContactPeople"" type=""multiple"" relatedClass=""ContactPersonTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadDefaultClassDef_WithRelationShipToAddress()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""Address"" relatedAssembly=""Habanero.Test"" deleteAction=""DeleteRelated"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
                    <relationship name=""ContactPeople"" type=""multiple"" relatedClass=""ContactPersonTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>

			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public Guid OrganisationID
        {
            get { return (Guid)this.GetPropertyValue("OrganisationID"); }
        }

        public static void ClearAllFromDB()
        {
            BusinessObjectCollection<OrganisationTestBO> col = new BusinessObjectCollection<OrganisationTestBO>();
            col.LoadAll();

            while (col.Count > 0)
            {
                OrganisationTestBO obj = col[0];
                obj.MarkForDelete();   
            }
            col.SaveAll();
        }

        public static OrganisationTestBO CreateSavedOrganisation()
        {
            OrganisationTestBO bo = new OrganisationTestBO();
            bo.Save();
            return bo;
        }
    }
}
