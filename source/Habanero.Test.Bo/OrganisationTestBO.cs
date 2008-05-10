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
    }
}
