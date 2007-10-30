using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;

namespace Habanero.Test
{
    public class TestAutoInc : BusinessObject
    {
        public int? TestAutoIncID
        {
            get
            {
                return (int?) GetPropertyValue("testautoincid");
            }
        }
        public string TestField
        {
            get
            {
                return GetPropertyValueString("testfield");
            }
        }

        public static ClassDef LoadClassDefWithAutoIncrementingID()
        {
            XmlClassLoader itsLoader = new XmlClassLoader();
            ClassDef itsClassDef =
                itsLoader.LoadClass(
                    @"
				<class name=""TestAutoInc"" assembly=""Habanero.Test"" table=""testautoinc"" >
					<property  name=""testautoincid"" type=""Int32"" auto-incrementing=""true"" />
					<property  name=""testfield"" />
					<primaryKey isObjectID=""false"">
						<prop name=""testautoincid"" />
					</primaryKey>
				</class>
			");
            ClassDef.ClassDefs.Add(itsClassDef);
            return itsClassDef;
        }
    }
}
