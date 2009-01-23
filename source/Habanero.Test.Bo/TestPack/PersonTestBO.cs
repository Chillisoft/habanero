using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    public class PersonTestBO : BusinessObject
    {
        

        private bool _afterLoadCalled;


        public static ClassDef LoadDefaultClassDefWithTestOrganisationBOLookup()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""PersonTestBO"" assembly=""Habanero.Test.BO"" table=""person_test"">
					<property  name=""PersonTestID"" type=""Guid"" />
                    <property  name=""OrganisationID"" type=""Guid"" >
                      <businessObjectLookupList class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey>
						<prop name=""PersonTestID"" />
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

        public static ClassDef LoadDefaultClassDefWithTestOrganisationBOLookup_DatabaseLookupList()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""PersonTestBO"" assembly=""Habanero.Test.BO"" table=""person_test"">
					<property  name=""PersonTestID"" type=""Guid"" />
                    <property  name=""OrganisationID"" type=""Guid"" >
                      <databaseLookupList sql=""select OrganisationID, Name from organisation"" class=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" />
                    </property>
					<primaryKey>
						<prop name=""PersonTestID"" />
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
     

        #region Properties

        public Guid PersonTestID
        {
            get { return (Guid)GetPropertyValue("PersonTestID"); }
            set { SetPropertyValue("PersonTestID", value); }
        }

      
        public Guid OrganisationID
        {
            get { return (Guid)GetPropertyValue("OrganisationID"); }
            set { SetPropertyValue("OrganisationID", value); }
        }

        public bool AfterLoadCalled
        {
            get { return _afterLoadCalled; }
            set { _afterLoadCalled = value; }
        }

        protected internal override void AfterLoad()
        {
            base.AfterLoad();
            _afterLoadCalled = true;
        }

        #endregion //Properties

       
    }
}