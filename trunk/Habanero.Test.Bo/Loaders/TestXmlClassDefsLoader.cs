using System.Collections;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlClassDefsLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlClassDefsLoader
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.GetClassDefCol.Clear();
        }

        [Test]
        public void TestLoadClassDefs()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            IList classDefList =
                loader.LoadClassDefs(
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
			");
            Assert.AreEqual(2, classDefList.Count);
        }
    }
}