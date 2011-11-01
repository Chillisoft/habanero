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
using System.Runtime.Serialization;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    public partial class LegalEntity
    {
        public LegalEntity()
        {
        }

        protected LegalEntity(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            
        }
        public new static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef = itsLoader.LoadClass(@"
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

        public static IClassDef LoadClassDef_WithClassTableInheritance()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef = itsLoader.LoadClass(@"
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
        
        public static IClassDef LoadClassDef_WithSingleTableInheritance()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""LegalEntity"" assembly=""Habanero.Test.Structure"" table=""table_class_LegalEntity"">
			    <superClass class=""Entity"" assembly=""Habanero.Test.Structure"" orMapping=""SingleTableInheritance"" discriminator=""EntityType"" />
			    <property name=""LegalEntityType"" databaseField=""field_Legal_Entity_Type"" />
			    <relationship name=""VehiclesOwned"" type=""multiple"" relatedClass=""Vehicle"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""EntityID"" relatedProperty=""OwnerID"" />
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
