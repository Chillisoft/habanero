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

using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test.Structure
{
    using System;
    using Habanero.BO;
    
    
    public partial class Part
    {
        public new static ClassDef LoadDefaultClassDef()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Part"" assembly=""Habanero.Test.Structure"" table=""table_Part"">
			    <property name=""PartID"" type=""Guid"" databaseField=""field_Part_ID"" compulsory=""true"" />
			    <property name=""ModelNo"" databaseField=""field_Model_No"" />
			    <property name=""PartType"" databaseField=""field_Part_Type"" />
			    <primaryKey>
			      <prop name=""PartID"" />
			    </primaryKey>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static ClassDef LoadClassDef_WithClassTableInheritance()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef = itsLoader.LoadClass(@"
			  <class name=""Part"" assembly=""Habanero.Test.Structure"" table=""table_class_Part"">
			    <superClass class=""Entity"" assembly=""Habanero.Test.Structure"" id=""PartID"" />
			    <property name=""PartID"" type=""Guid"" databaseField=""field_Part_ID"" compulsory=""true"" />
			    <property name=""ModelNo"" databaseField=""field_Model_No"" />
			    <property name=""PartType"" databaseField=""field_Part_Type"" />
			    <primaryKey>
			      <prop name=""PartID"" />
			    </primaryKey>
			  </class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }

        public static Part CreateSavedPart()
        {
            Part part = CreateUnsavedPart();
            part.Save();
            return part;
        }

        public static Part CreateSavedPart(string modelNo)
        {
            Part part = CreateUnsavedPart(modelNo);
            part.Save();
            return part;
        }

        private static Part CreateUnsavedPart()
        {
            return CreateUnsavedPart(TestUtil.CreateRandomString());
        }

        private static Part CreateUnsavedPart(string modelNo)
        {
            Part part = new Part();
            part.ModelNo = modelNo;
            return part;
        }
    }
}
