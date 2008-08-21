
// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Engine
    {
        public new static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Engine"" assembly=""Habanero.Test.Structure.BO"" table=""table_Engine"">
			    <property name=""EngineID"" type=""Guid"" databaseField=""field_Engine_ID"" compulsory=""true"" />
			    <property name=""EngineNo"" databaseField=""field_Engine_No"" />
			    <property name=""DateManufactured"" type=""DateTime"" databaseField=""field_Date_Manufactured"" />
			    <property name=""HorsePower"" type=""Int32"" databaseField=""field_Horse_Power"" />
			    <property name=""FuelInjected"" type=""Boolean"" databaseField=""field_Fue_lInjected"" />
			    <property name=""CarID"" type=""Guid"" databaseField=""field_Car_ID"" />
			    <primaryKey>
			      <prop name=""EngineID"" />
			    </primaryKey>
			    <relationship name=""Car"" type=""single"" relatedClass=""Car"" relatedAssembly=""Habanero.Test.Structure.BO"">
			      <relatedProperty property=""CarID"" relatedProperty=""CarID"" />
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
			  <class name=""Engine"" assembly=""Habanero.Test.Structure.BO"" table=""table_Engine"">
			    <superClass class=""Part"" assembly=""Habanero.Test.Structure.BO"" id=""EngineID"" />
			    <property name=""EngineID"" type=""Guid"" databaseField=""field_Engine_ID"" compulsory=""true"" />
			    <property name=""EngineNo"" databaseField=""field_Engine_No"" />
			    <property name=""DateManufactured"" type=""DateTime"" databaseField=""field_Date_Manufactured"" />
			    <property name=""HorsePower"" type=""Int32"" databaseField=""field_Horse_Power"" />
			    <property name=""FuelInjected"" type=""Boolean"" databaseField=""field_Fue_lInjected"" />
			    <property name=""CarID"" type=""Guid"" databaseField=""field_Car_ID"" />
			    <primaryKey>
			      <prop name=""EngineID"" />
			    </primaryKey>
			    <relationship name=""Car"" type=""single"" relatedClass=""Car"" relatedAssembly=""Habanero.Test.Structure.BO"">
			      <relatedProperty property=""CarID"" relatedProperty=""CarID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static Engine CreateSavedEngine()
        {
            Engine engine = CreateUnsavedEngine();
            engine.Save();
            return engine;
        }

        public static Engine CreateSavedEngine(string engineNo)
        {
            Engine engine = CreateUnsavedEngine(engineNo);
            engine.Save();
            return engine;
        }

        private static Engine CreateUnsavedEngine()
        {
            return CreateUnsavedEngine(TestUtil.CreateRandomString());
        }

        private static Engine CreateUnsavedEngine(string engineNo)
        {
            Engine engine = new Engine();
            engine.EngineNo = engineNo;
            return engine;
        }
    }
}
