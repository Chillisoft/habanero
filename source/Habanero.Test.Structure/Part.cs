
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
			  <class name=""Part"" assembly=""Habanero.Test.Structure"" table=""table_Part"">
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
