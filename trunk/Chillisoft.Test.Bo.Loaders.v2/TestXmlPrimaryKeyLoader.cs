using System.IO;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.Loaders.v2;
using Chillisoft.Generic.v2;
using NUnit.Framework;

namespace Chillisoft.Test.Bo.Loaders.v2
{
    /// <summary>
    /// Summary description for TestXmlPrimaryKeyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlPrimaryKeyLoader
    {
        private XmlPrimaryKeyLoader itsLoader;
        private PropDefCol itsPropDefs;

        [SetUp]
        public void SetupTest()
        {
            itsLoader = new XmlPrimaryKeyLoader();
            itsPropDefs = new PropDefCol();
            itsPropDefs.Add(new PropDef("TestProp", typeof (string), cbsPropReadWriteRule.ReadManyWriteMany, null));
        }

        [Test]
        public void TestSimplePrimaryKey()
        {
            PrimaryKeyDef def =
                itsLoader.LoadPrimaryKey(@"<primaryKeyDef><prop name=""TestProp"" /></primaryKeyDef>", itsPropDefs);
            Assert.AreEqual(1, def.Count, "Def should have one property in it.");
            Assert.AreEqual(true, def.IsObjectID, "Def should by default be an objectID");
        }

        [Test, ExpectedException(typeof (FileNotFoundException))]
        public void TestWrongElementName()
        {
            itsLoader.LoadPrimaryKey(@"<primaryKey><prop name=""TestProp"" /></primaryKey>", itsPropDefs);
        }

        [
            Test,
                ExpectedException(typeof (InvalidXmlDefinitionException),
                    "A primaryKeyDef node must have one or more prop nodes")]
        public void TestWithNoProps()
        {
            itsLoader.LoadPrimaryKey(@"<primaryKeyDef></primaryKeyDef>", itsPropDefs);
        }

        [
            Test,
                ExpectedException(typeof (InvalidXmlDefinitionException),
                    "The PropDef named TestProp does not exist in the PropDefCol given")]
        public void TestWithPropThatDoesNotExist()
        {
            PropDefCol propDefs = new PropDefCol();
            itsLoader.LoadPrimaryKey(@"<primaryKeyDef><prop name=""TestProp"" /></primaryKeyDef>", propDefs);
        }

        [Test]
        public void TestCompositeKey()
        {
            itsPropDefs.Add(new PropDef("TestProp2", typeof (string), cbsPropReadWriteRule.ReadManyWriteMany, null));
            PrimaryKeyDef def =
                itsLoader.LoadPrimaryKey(
                    @"<primaryKeyDef isObjectID=""false""><prop name=""TestProp"" /><prop name=""TestProp2"" /></primaryKeyDef>",
                    itsPropDefs);
            Assert.AreEqual(2, def.Count, "Def should have one property in it.");
            Assert.AreEqual(false, def.IsObjectID, "Def should not be an objectID");
        }
    }
}