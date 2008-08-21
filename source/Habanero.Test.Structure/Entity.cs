
// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Entity
    {
        public static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Entity"" assembly=""Habanero.Test.Structure.BO"" table=""table_Entity"">
			    <property name=""EntityID"" type=""Guid"" databaseField=""field_Entity_ID"" compulsory=""true"" />
			    <property name=""EntityType"" databaseField=""field_Entity_Type"" />
			    <primaryKey>
			      <prop name=""EntityID"" />
			    </primaryKey>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static Entity CreateSavedEntity()
        {
            Entity entity = CreateUnsavedEntity();
            entity.Save();
            return entity;
        }

        private static Entity CreateUnsavedEntity()
        {
            Entity entity = new Entity();
            return entity;
        }
    }
}
