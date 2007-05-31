using System.Collections;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.Loaders.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.Loaders.v2
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
            ClassDef.GetClassDefCol().Clear();
        }

        [Test]
        public void TestLoadClassDefs()
        {
            XmlClassDefsLoader loader = new XmlClassDefsLoader();
            IList classDefList =
                loader.LoadClassDefs(
                    @"
					<classDefs>
						<classDef name=""TestClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" >
							<propertyDef name=""TestClassID"" />
						</classDef>
						<classDef name=""TestRelatedClass"" assembly=""Chillisoft.Test.Bo.Loaders.v2"" >
							<propertyDef name=""TestRelatedClassID"" />
						</classDef>
					</classDefs>
			");
            Assert.AreEqual(2, classDefList.Count);
        }
    }
}