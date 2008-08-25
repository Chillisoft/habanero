
// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class LegalEntity
    {
        public new static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""LegalEntity"" assembly=""Habanero.Test.Structure"" table=""table_LegalEntity"">
			    <property name=""LegalEntityID"" type=""Guid"" databaseField=""field_Legal_Entity_ID"" />
			    <property name=""LegalEntityType"" databaseField=""field_Legal_Entity_Type"" />
			    <primaryKey>
			      <prop name=""LegalEntityID"" />
			    </primaryKey>
			    <relationship name=""VehiclesOwned"" type=""multiple"" relatedClass=""Vehicle"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""LegalEntityID"" relatedProperty=""OwnerID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDef_WithClassTableInheritance()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""LegalEntity"" assembly=""Habanero.Test.Structure"" table=""table_class_LegalEntity"">
			    <superClass class=""Entity"" assembly=""Habanero.Test.Structure"" />
			    <property name=""LegalEntityID"" type=""Guid"" databaseField=""field_Legal_Entity_ID"" />
			    <property name=""LegalEntityType"" databaseField=""field_Legal_Entity_Type"" />
			    <primaryKey>
			      <prop name=""LegalEntityID"" />
			    </primaryKey>
			    <relationship name=""VehiclesOwned"" type=""multiple"" relatedClass=""Vehicle"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""LegalEntityID"" relatedProperty=""OwnerID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static LegalEntity CreateSavedLegalEntity()
        {
            LegalEntity legalEntity = CreateUnsavedLegalEntity();
            legalEntity.Save();
            return legalEntity;
        }

        private static LegalEntity CreateUnsavedLegalEntity()
        {
            LegalEntity legalEntity = new LegalEntity();
            return legalEntity;
        }
    }
}
