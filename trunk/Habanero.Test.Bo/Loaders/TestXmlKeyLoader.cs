using System;
using System.IO;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo.Loaders;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Bo.Loaders
{
    /// <summary>
    /// Summary description for TestXmlKeyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlKeyLoader
    {
        private XmlKeyLoader itsLoader;
        private PropDefCol itsPropDefs;

        [SetUp]
        public void SetupTest()
        {
            itsLoader = new XmlKeyLoader();
            itsPropDefs = new PropDefCol();
            itsPropDefs.Add(new PropDef("TestProp", typeof (string), PropReadWriteRule.ReadWrite, null));
        }

        [Test]
        public void TestLoadSimpleKey()
        {
            KeyDef def = itsLoader.LoadKey(@"<key><prop name=""TestProp"" /></key>", itsPropDefs);
            Assert.AreEqual(1, def.Count);
            Assert.IsFalse(def.IgnoreIfNull);
        }

        [Test]
        public void TestLoadKeyWithName()
        {
            KeyDef def = itsLoader.LoadKey(@"<key name=""Key1""><prop name=""TestProp"" /></key>", itsPropDefs);
            Assert.AreEqual(1, def.Count);
            Assert.AreEqual("Key1_TestProp", def.KeyName);
        }

        [Test]
        public void TestLoadKeyWithMultipleProps()
        {
            itsPropDefs.Add(new PropDef("TestProp2", typeof (string), PropReadWriteRule.ReadWrite, null));
            KeyDef def =
                itsLoader.LoadKey(
                    @"<key name=""Key1""><prop name=""TestProp"" /><prop name=""TestProp2"" /></key>", itsPropDefs);
            Assert.AreEqual(2, def.Count);
        }

        [Test]
        public void TestLoadKeyWithIgnoreNulls()
        {
            KeyDef def =
                itsLoader.LoadKey(@"<key name=""Key1"" ignoreIfNull=""true""><prop name=""TestProp"" /></key>",
                                  itsPropDefs);
            Assert.IsTrue(def.IgnoreIfNull);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException), "An invalid node 'keyDef' was encountered when loading the class definitions.")]
        public void TestLoadKeyWithWrongElementName()
        {
            itsLoader.LoadKey(@"<keyDef name=""Key1""><prop name=""TestProp"" /></keyDef>", itsPropDefs);
        }

        [
            Test,
                ExpectedException(typeof(ArgumentException),
                    "The property name 'TestProp2' does not exist in the " +
                    "collection of property definitions.")]
        public void TestLoadKeyNonExistentProp()
        {
            itsLoader.LoadKey(@"<key name=""Key1""><prop name=""TestProp"" /><prop name=""TestProp2"" /></key>",
                              itsPropDefs);
        }
    }
}