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

// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Engine
    {
        public new static IClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Engine"" assembly=""Habanero.Test.Structure"" table=""table_Engine"">
			    <property name=""EngineID"" type=""Guid"" databaseField=""field_Engine_ID"" compulsory=""true"" />
			    <property name=""EngineNo"" databaseField=""field_Engine_No"" />
			    <property name=""DateManufactured"" type=""DateTime"" databaseField=""field_Date_Manufactured"" />
			    <property name=""HorsePower"" type=""Int32"" databaseField=""field_Horse_Power"" />
			    <property name=""FuelInjected"" type=""Boolean"" databaseField=""field_Fuel_Injected"" />
			    <property name=""CarID"" type=""Guid"" databaseField=""field_Car_ID"" />
			    <primaryKey>
			      <prop name=""EngineID"" />
			    </primaryKey>
			    <relationship name=""Car"" type=""single"" relatedClass=""Car"" relatedAssembly=""Habanero.Test.Structure"">
			      <relatedProperty property=""CarID"" relatedProperty=""CarID"" />
			    </relationship>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public new static IClassDef LoadClassDef_WithClassTableInheritance()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Engine"" assembly=""Habanero.Test.Structure"" table=""table_class_Engine"">
			    <superClass class=""Part"" assembly=""Habanero.Test.Structure"" id=""EngineID"" />
			    <property name=""EngineID"" type=""Guid"" databaseField=""field_Engine_ID"" compulsory=""true"" />
			    <property name=""EngineNo"" databaseField=""field_Engine_No"" />
			    <property name=""DateManufactured"" type=""DateTime"" databaseField=""field_Date_Manufactured"" />
			    <property name=""HorsePower"" type=""Int32"" databaseField=""field_Horse_Power"" />
			    <property name=""FuelInjected"" type=""Boolean"" databaseField=""field_Fuel_Injected"" />
			    <property name=""CarID"" type=""Guid"" databaseField=""field_Car_ID"" />
			    <primaryKey>
			      <prop name=""EngineID"" />
			    </primaryKey>
			    <relationship name=""Car"" type=""single"" relatedClass=""Car"" relatedAssembly=""Habanero.Test.Structure"">
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
            return CreateUnsavedEngine(TestUtil.GetRandomString());
        }

        private static Engine CreateUnsavedEngine(string engineNo)
        {
            Engine engine = new Engine();
            engine.EngineNo = engineNo;
            return engine;
        }
    }
}
