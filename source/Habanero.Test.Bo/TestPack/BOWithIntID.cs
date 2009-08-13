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

using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.Test.BO
{
    public class BOWithIntID : BusinessObject
    {
        public int? IntID
        {
            get
            {
                return (int?)GetPropertyValue("IntID");
            }
            set { SetPropertyValue("IntID", value); }
        }

        public string TestField
        {
            get
            {
                return GetPropertyValueString("TestField");
            }
            set { SetPropertyValue("TestField", value); }
        }

        public override string ToString()
        {
            return TestField;
        }

        public static ClassDef LoadClassDefWithIntID()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""BOWithIntID"" assembly=""Habanero.Test.BO"" table=""bowithintid"" >
					<property  name=""IntID"" type=""Int32"" />
					<property  name=""TestField"" default=""testing"" />
					<primaryKey isObjectID=""false"">
						<prop name=""IntID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static ClassDef LoadClassDefWithIntID_RelationshipToSelf()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""BOWithIntID"" assembly=""Habanero.Test.BO"" table=""bowithintid"" >
					<property  name=""IntID"" type=""Int32"" />
					<property  name=""ChildIntID"" type=""Int32"" />
					<property  name=""TestField"" default=""testing"" />
					<primaryKey isObjectID=""false"">
						<prop name=""IntID"" />
					</primaryKey>
					<relationship name=""MyChildBoWithInt"" type=""single"" relatedClass=""BOWithIntID"" 
                        relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"" owningBOHasForeignKey=""true""
                        reverseRelationship=""MyParentBOWithInt"">
						<relatedProperty property=""ChildIntID"" relatedProperty=""IntID"" />
					</relationship>
                    <relationship name=""MyParentBOWithInt"" type=""single"" relatedClass=""BOWithIntID"" 
                        relatedAssembly=""Habanero.Test.BO"" deleteAction=""DeleteRelated"" owningBOHasForeignKey=""false""
                        reverseRelationship=""MyChildBoWithInt"">
						<relatedProperty property=""IntID"" relatedProperty=""ChildIntID"" />
					</relationship>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static ClassDef LoadClassDefWithIntID_DiscriminatorField()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""BOWithIntID"" assembly=""Habanero.Test.BO"" table=""bowithintid"" >
					<property  name=""IntID"" type=""Int32"" />
					<property  name=""TestField"" default=""testing"" />
                    <property  name=""Type_field"" />
					<primaryKey isObjectID=""false"">
						<prop name=""IntID"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        public static ClassDef LoadClassDefWithIntID_WithCompositeKey()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""BOWithIntID"" assembly=""Habanero.Test.BO"" table=""bowithintid"" >
					<property  name=""IntID"" type=""Int32"" />
					<property  name=""TestField"" default=""testing"" />
					<primaryKey isObjectID=""false"">
						<prop name=""IntID"" />
						<prop name=""TestField"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
        internal static void DeleteAllBOWithIntID()
        {
            const string sql = "DELETE FROM bowithintid";
            if (DatabaseConnection.CurrentConnection != null) DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }
    }
}