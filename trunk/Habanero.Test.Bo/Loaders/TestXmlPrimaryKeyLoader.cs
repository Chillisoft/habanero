using System;
using System.IO;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
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
            itsPropDefs.Add(new PropDef("TestProp", typeof (string), PropReadWriteRule.ReadWrite, null));
        }

        [Test]
        public void TestSimplePrimaryKey()
        {
            PrimaryKeyDef def =
                itsLoader.LoadPrimaryKey(@"<primaryKey><prop name=""TestProp"" /></primaryKey>", itsPropDefs);
            Assert.AreEqual(1, def.Count, "Def should have one property in it.");
            Assert.AreEqual(true, def.IsObjectID, "Def should by default be an objectID");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException), "An invalid node 'primaryKeyDef' was encountered when loading the class definitions.")]

        public void TestWrongElementName()
        {
            itsLoader.LoadPrimaryKey(@"<primaryKeyDef><prop name=""TestProp"" /></primaryKeyDef>", itsPropDefs);
        }

        [
            Test,
                ExpectedException(typeof (InvalidXmlDefinitionException),
                    "A primaryKey node must have one or more prop nodes")]
        public void TestWithNoProps()
        {
            itsLoader.LoadPrimaryKey(@"<primaryKey></primaryKey>", itsPropDefs);
        }

        [
            Test,
                ExpectedException(typeof(ArgumentException),
                    "The property name 'TestProp' does not exist in the " +
                    "collection of property definitions.")]
        public void TestWithPropThatDoesNotExist()
        {
            PropDefCol propDefs = new PropDefCol();
            itsLoader.LoadPrimaryKey(@"<primaryKey><prop name=""TestProp"" /></primaryKey>", propDefs);
        }

        [Test]
        public void TestCompositeKey()
        {
            itsPropDefs.Add(new PropDef("TestProp2", typeof (string), PropReadWriteRule.ReadWrite, null));
            PrimaryKeyDef def =
                itsLoader.LoadPrimaryKey(
                    @"<primaryKey isObjectID=""false""><prop name=""TestProp"" /><prop name=""TestProp2"" /></primaryKey>",
                    itsPropDefs);
            Assert.AreEqual(2, def.Count, "Def should have one property in it.");
            Assert.AreEqual(false, def.IsObjectID, "Def should not be an objectID");
        }
    }
}