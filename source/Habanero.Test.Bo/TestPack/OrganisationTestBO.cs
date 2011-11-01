#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.Test.BO
{
    public class OrganisationTestBO : BusinessObject
    {
        public static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
					<relationship name=""ContactPeople"" type=""multiple"" relatedClass=""ContactPersonTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"" reverseRelationship=""Organisation"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
                    <ui>
                        <grid>
                            <column heading=""OrganisationID"" property=""OrganisationID"" />
                        </grid>
                        <form>
                            <field label=""OrganisationID: *"" property=""OrganisationID"" />
                        </form>
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

        public static IClassDef LoadDefaultClassDef_NoRelationships()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadDefaultClassDef_WithTwoRelationshipsToContactPerson()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
					<relationship name=""ContactPeople"" type=""multiple"" relatedClass=""ContactPersonTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
					<relationship name=""OtherContactPeople"" type=""multiple"" relatedClass=""ContactPersonTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        
        public static IClassDef LoadDefaultClassDef_WithSingleRelationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
					<relationship name=""ContactPerson"" type=""single"" relatedClass=""ContactPersonTestBO"" 
                        relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"" owningBOHasForeignKey=""false""
                        reverseRelationship=""Organisation"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDef_WithContactPersonRelationship_NoReverseRelationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
					<relationship name=""ContactPerson"" type=""single"" relatedClass=""ContactPersonTestBO"" 
                        relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"" owningBOHasForeignKey=""false"">
						<relatedProperty property=""OrganisationID"" relatedProperty=""OrganisationID"" />
					</relationship>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadDefaultClassDef_WithNoContactPersonRelationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
			    </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static IClassDef LoadDefaultClassDef_PreventAddChild()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
			    </class>
			");
            RelPropDef relPropDef = new RelPropDef(itsClassDef.PropDefcol["OrganisationID"], "OrganisationID");
            RelKeyDef relKeyDef = new RelKeyDef();
            relKeyDef.Add(relPropDef);
            MultipleRelationshipDef relationshipDef = new MultipleRelationshipDef("ContactPeople", "Habanero.Test.BO",
                    "ContactPersonTestBO", relKeyDef,true, "", DeleteParentAction.DeleteRelated, InsertParentAction.InsertRelationship, RelationshipType.Composition, 0);
            relationshipDef.ReverseRelationshipName = "Organisation";
            itsClassDef.RelationshipDefCol.Add(relationshipDef);
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadDefaultClassDef_NoReverseRelationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
			    </class>
			");
            RelPropDef relPropDef = new RelPropDef(itsClassDef.PropDefcol["OrganisationID"], "OrganisationID");
            RelKeyDef relKeyDef = new RelKeyDef();
            relKeyDef.Add(relPropDef);
            MultipleRelationshipDef relationshipDef = new MultipleRelationshipDef("ContactPeople", "Habanero.Test.BO",
                    "ContactPersonTestBO", relKeyDef, true, "", DeleteParentAction.DeleteRelated, InsertParentAction.InsertRelationship, RelationshipType.Composition, 0);
        
            itsClassDef.RelationshipDefCol.Add(relationshipDef);
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadDefaultClassDef_SingleRel_NoReverseRelationship()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
			    </class>
			");
            RelPropDef relPropDef = new RelPropDef(itsClassDef.PropDefcol["OrganisationID"], "OrganisationID");
            RelKeyDef relKeyDef = new RelKeyDef();
            relKeyDef.Add(relPropDef);
            IRelationshipDef relationshipDef = new SingleRelationshipDef("ContactPerson", "Habanero.Test.BO",
                    "ContactPersonTestBO", relKeyDef, true, DeleteParentAction.DeleteRelated, InsertParentAction.InsertRelationship, RelationshipType.Aggregation);
            relationshipDef.OwningBOHasForeignKey = false;
            itsClassDef.RelationshipDefCol.Add(relationshipDef);
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static IClassDef LoadDefaultClassDef_WithMultipleRelationshipToAddress()
        {
            XmlClassLoader itsLoader = CreateXmlClassLoader();
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""OrganisationTestBO"" assembly=""Habanero.Test.BO"" table=""organisation"">
					<property  name=""OrganisationID"" type=""Guid"" />
                    <property name=""Name"" />
					<primaryKey>
						<prop name=""OrganisationID"" />
					</primaryKey>
					<relationship name=""Addresses"" type=""multiple"" relatedClass=""AddressTestBO"" relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"">
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
        public Guid? OrganisationID
        {
            get { return (Guid?)this.GetPropertyValue("OrganisationID"); }
            set { this.SetPropertyValue("OrganisationID", value); }
        }

        public string Name
        {
            get { return (String) this.GetPropertyValue("Name"); }
            set { this.SetPropertyValue("Name", value);}
        }

        public ContactPersonTestBO ContactPerson
        {
            get { return Relationships.GetRelatedObject<ContactPersonTestBO>("ContactPerson"); }
            set { Relationships.SetRelatedObject("ContactPerson", value); }
        }

        public BusinessObjectCollection<ContactPersonTestBO> ContactPeople
        {
            get
            {
                return Relationships.GetMultiple<ContactPersonTestBO>("ContactPeople").BusinessObjectCollection;
            }
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
            OrganisationTestBO bo = CreateUnsavedOrganisation();
            bo.Save();
            return bo;
        }

        internal static OrganisationTestBO CreateUnsavedOrganisation() 
        {
            return new OrganisationTestBO();
        }

        public static void DeleteAllOrganisations()
        {
            string sql = "DELETE FROM contact_person_address";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
            sql = "DELETE FROM Contact_Person";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
            sql = "DELETE FROM organisation";
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }
    }
}
