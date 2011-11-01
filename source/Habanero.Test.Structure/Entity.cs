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
    public partial class Entity
    {
        public Entity()
        {
        }

        protected Entity(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
        public static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Entity"" assembly=""Habanero.Test.Structure"" table=""table_Entity"">
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

        public static IClassDef LoadDefaultClassDef_WithCircularDeleteRelatedToSelf()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Entity"" assembly=""Habanero.Test.Structure"" table=""table_Entity"">
			    <property name=""EntityID"" type=""Guid"" databaseField=""field_Entity_ID"" compulsory=""true"" />
			    <property name=""RelatedEntityID"" type=""Guid"" databaseField=""field_Entity_Type"" />
                <primaryKey>
			      <prop name=""EntityID"" />
			    </primaryKey>
				<relationship name=""RelatedEntity"" type=""single"" relatedClass=""Entity"" relatedAssembly=""Habanero.Test.Structure"" 
                        owningBOHasForeignKey=""true"" reverseRelationship=""RelatedEntityReverse""
                        deleteAction=""DeleteRelated"">
					<relatedProperty property=""RelatedEntityID"" relatedProperty=""EntityID"" />
				</relationship>
				<relationship name=""RelatedEntityReverse"" type=""single"" relatedClass=""Entity"" relatedAssembly=""Habanero.Test.Structure"" 
                        owningBOHasForeignKey=""false"" reverseRelationship=""RelatedEntity""
                        deleteAction=""DoNothing"">
					<relatedProperty property=""EntityID"" relatedProperty=""RelatedEntityID"" />
				</relationship>
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
