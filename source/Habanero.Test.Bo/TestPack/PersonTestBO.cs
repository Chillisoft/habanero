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
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.BO
{
    public class PersonTestBO : BusinessObject
    {
        

        private bool _afterLoadCalled;


        public static IClassDef LoadDefaultClassDefWithTestOrganisationBOLookup()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
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

        private static XmlClassLoader CreateXmlClassLoader()
        {
            return new XmlClassLoader(new DtdLoader(), new DefClassFactory());
        }

        public static IClassDef LoadDefaultClassDefWithTestOrganisationBOLookup_DatabaseLookupList()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
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