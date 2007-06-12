using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.Loaders.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.Loaders.v2
{
    /// <summary>
    /// Summary description for TestXmlSuperClassDescLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlSuperClassDescLoader
    {
        private XmlSuperClassDescLoader itsLoader;

        [SetUp]
        public void SetupTest()
        {
            itsLoader = new XmlSuperClassDescLoader();
            ClassDef.GetClassDefCol.Clear();
            ClassDef.LoadClassDefs(
                new XmlClassDefsLoader(
                    @"
					<classDefs>
						<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" >
							<propertyDef name=""TestClassID"" />
						</classDef>
						<classDef name=""TestRelatedClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" >
							<propertyDef name=""TestRelatedClassID"" />
						</classDef>
					</classDefs>",
                    ""));
        }

        [Test]
        public void TestSimpleProperty()
        {
            SuperClassDesc desc =
                itsLoader.LoadSuperClassDesc(
                    @"<superClassDesc className=""TestClass"" assemblyName=""Chillisoft.Test.Bo.Loaders.v2"" />");
            Assert.AreEqual(ORMapping.ClassTableInheritance, desc.ORMapping);
            Assert.AreSame(ClassDef.GetClassDefCol[typeof (TestClass)], desc.SuperClassDef);
        }

        [Test]
        public void TestORMapping()
        {
            SuperClassDesc desc =
                itsLoader.LoadSuperClassDesc(
                    @"<superClassDesc className=""TestClass"" assemblyName=""Chillisoft.Test.Bo.Loaders.v2"" orMapping=""SingleTableInheritance"" />");
            Assert.AreEqual(ORMapping.SingleTableInheritance, desc.ORMapping);
        }
    }
}