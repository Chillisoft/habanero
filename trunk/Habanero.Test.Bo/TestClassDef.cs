using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Bo;
using Habanero.Test;
using NUnit.Framework;

namespace Habanero.Test.Bo
{
    /// <summary>
    /// Summary description for TestClassDef.
    /// </summary>
    [TestFixture]
    public class TestClassDef
    {
        private ClassDef itsClassDef;

        [Test]
        public void TestCreateBusinessObject()
        {
            ClassDef.GetClassDefCol.Clear();
            XmlClassLoader loader = new XmlClassLoader();
            itsClassDef =
                loader.LoadClass(
                    @"
				<classDef name=""MyBo"" assembly=""Habanero.Test"">
					<propertyDef name=""MyBoID"" type=""Guid"" />
					<propertyDef name=""TestProp"" />
					<primaryKeyDef>
						<prop name=""MyBoID"" />
					</primaryKeyDef>
				</classDef>
			");
            BusinessObject bo = itsClassDef.CreateNewBusinessObject();
            Assert.AreSame(typeof (MyBo), bo.GetType());
            bo.SetPropertyValue("TestProp", "TestValue");
            Assert.AreEqual("TestValue", bo.GetPropertyValue("TestProp"));
        }

        [Test]
        public void TestLoadClassDefs()
        {
            XmlClassDefsLoader loader =
                new XmlClassDefsLoader(
                    @"
					<classDefs>
						<classDef name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" >
							<propertyDef name=""TestClassID"" />
                            <primaryKeyDef>
                                <prop name=""TestClassID""/>
                            </primaryKeyDef>
						</classDef>
						<classDef name=""TestRelatedClass"" assembly=""Habanero.Test.Bo.Loaders"" >
							<propertyDef name=""TestRelatedClassID"" />
                            <primaryKeyDef>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKeyDef>
						</classDef>
					</classDefs>
			",
                    "");
            ClassDef.GetClassDefCol.Clear();
            ClassDef.LoadClassDefs(loader);
            Assert.AreEqual(2, ClassDef.GetClassDefCol.Count);
        }
    }
}