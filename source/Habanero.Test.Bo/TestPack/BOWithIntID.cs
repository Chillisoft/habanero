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
            XmlClassLoader itsLoader = new XmlClassLoader();
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
            XmlClassLoader itsLoader = new XmlClassLoader();
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
            XmlClassLoader itsLoader = new XmlClassLoader();
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
            XmlClassLoader itsLoader = new XmlClassLoader();
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
            DatabaseConnection.CurrentConnection.ExecuteRawSql(sql);
        }
    }
}