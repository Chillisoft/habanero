using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
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
					<classes>
						<class name=""TestClass"" assembly=""Habanero.Test.Bo.Loaders"" >
							<propertyDef name=""TestClassID"" />
                            <primaryKeyDef>
                                <prop name=""TestClassID""/>
                            </primaryKeyDef>
						</class>
						<class name=""TestRelatedClass"" assembly=""Habanero.Test.Bo.Loaders"" >
							<propertyDef name=""TestRelatedClassID"" />
                            <primaryKeyDef>
                                <prop name=""TestRelatedClassID""/>
                            </primaryKeyDef>
						</class>
					</classes>",
                    new DtdLoader()));
        }

        [Test]
        public void TestSimpleProperty()
        {
            SuperClassDesc desc =
                itsLoader.LoadSuperClassDesc(
                    @"<superClassDesc className=""Habanero.Test.Bo.Loaders.TestClass"" assemblyName=""Habanero.Test.Bo"" />");
            Assert.AreEqual(ORMapping.ClassTableInheritance, desc.ORMapping);
            Assert.AreSame(ClassDef.GetClassDefCol[typeof (TestClass)], desc.SuperClassDef);
        }

        [Test]
        public void TestORMapping()
        {
            SuperClassDesc desc =
                itsLoader.LoadSuperClassDesc(
                    @"<superClassDesc className=""TestClass"" assemblyName=""Habanero.Test.Bo.Loaders"" orMapping=""SingleTableInheritance"" />");
            Assert.AreEqual(ORMapping.SingleTableInheritance, desc.ORMapping);
        }
    }
}