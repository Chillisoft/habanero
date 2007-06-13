using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.Loaders.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Test.Setup.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.v2
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
				<classDef name=""MyBo"" assembly=""Chillisoft.Test.Setup.v2"">
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
						<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" >
							<propertyDef name=""TestClassID"" />
                            <primaryKeyDef>
                                <prop name=""TestClassID""/>
                            </primaryKeyDef>
						</classDef>
						<classDef name=""TestRelatedClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" >
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