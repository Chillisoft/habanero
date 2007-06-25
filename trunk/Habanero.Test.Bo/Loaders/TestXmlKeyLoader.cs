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
            itsPropDefs.Add(new PropDef("TestProp", typeof (string), PropReadWriteRule.ReadManyWriteMany, null));
        }

        [Test]
        public void TestLoadSimpleKey()
        {
            KeyDef def = itsLoader.LoadKey(@"<keyDef><prop name=""TestProp"" /></keyDef>", itsPropDefs);
            Assert.AreEqual(1, def.Count);
            Assert.IsFalse(def.IgnoreNulls);
        }

        [Test]
        public void TestLoadKeyWithName()
        {
            KeyDef def = itsLoader.LoadKey(@"<keyDef name=""Key1""><prop name=""TestProp"" /></keyDef>", itsPropDefs);
            Assert.AreEqual(1, def.Count);
            Assert.AreEqual("Key1_TestProp", def.KeyName);
        }

        [Test]
        public void TestLoadKeyWithMultipleProps()
        {
            itsPropDefs.Add(new PropDef("TestProp2", typeof (string), PropReadWriteRule.ReadManyWriteMany, null));
            KeyDef def =
                itsLoader.LoadKey(
                    @"<keyDef name=""Key1""><prop name=""TestProp"" /><prop name=""TestProp2"" /></keyDef>", itsPropDefs);
            Assert.AreEqual(2, def.Count);
        }

        [Test]
        public void TestLoadKeyWithIgnoreNulls()
        {
            KeyDef def =
                itsLoader.LoadKey(@"<keyDef name=""Key1"" ignoreNulls=""true""><prop name=""TestProp"" /></keyDef>",
                                  itsPropDefs);
            Assert.IsTrue(def.IgnoreNulls);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException), "An invalid node 'key' was encountered when loading the class definitions.")]
        public void TestLoadKeyWithWrongElementName()
        {
            itsLoader.LoadKey(@"<key name=""Key1""><prop name=""TestProp"" /></key>", itsPropDefs);
        }

        [
            Test,
                ExpectedException(typeof(ArgumentException),
                    "The property name 'TestProp2' does not exist in the " +
                    "collection of property definitions.")]
        public void TestLoadKeyNonExistentProp()
        {
            itsLoader.LoadKey(@"<keyDef name=""Key1""><prop name=""TestProp"" /><prop name=""TestProp2"" /></keyDef>",
                              itsPropDefs);
        }
    }
}