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
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.DB;

namespace Habanero.Test.BO
{
    public class BOWithIntID_DifferentType : BusinessObject
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

        public static IClassDef LoadClassDefWithIntID()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""BOWithIntID_DifferentType"" assembly=""Habanero.Test.BO"" table=""bowithintid"" >
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
        public static IClassDef LoadClassDefWithIntID_CompositeKey()
        {
            XmlClassLoader itsLoader = new XmlClassLoader(new DtdLoader(), new DefClassFactory());
            IClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""BOWithIntID_DifferentType"" assembly=""Habanero.Test.BO"" table=""bowithintid"" >
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
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }
    }
}