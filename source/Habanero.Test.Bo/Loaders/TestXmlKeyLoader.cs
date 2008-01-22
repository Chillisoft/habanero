//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of Habanero Standard.
// 
//     Habanero Standard is free software: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     Habanero Standard is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with Habanero Standard.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.IO;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
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
            KeyDef def = itsLoader.LoadKey(@"<key name=""Key 1""><prop name=""TestProp"" /></key>", itsPropDefs);
            Assert.AreEqual(1, def.Count);
            Assert.AreEqual("Key 1_TestProp", def.KeyName);
            Assert.AreEqual("Key 1", def.KeyNameForDisplay);
        }

        [Test]
        public void TestLoadKeyWithMultipleProps()
        {
            itsPropDefs.Add(new PropDef("TestProp2", typeof (string), PropReadWriteRule.ReadWrite, null));
            KeyDef def =
                itsLoader.LoadKey(
                    @"<key name=""Key1""><prop name=""TestProp"" /><prop name=""TestProp2"" /></key>", itsPropDefs);
            Assert.AreEqual(2, def.Count);
            Assert.AreEqual("Key1_TestProp_TestProp2", def.KeyName);
            Assert.AreEqual("Key1", def.KeyNameForDisplay);
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

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidIgnoreIfNullValue()
        {
            itsLoader.LoadKey(@"
                <key name=""Key1"" ignoreIfNull=""invalidvalue"">
                    <prop name=""TestProp"" />
                </key>", itsPropDefs);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropElementInvalid()
        {
            itsLoader.LoadKey(@"
                <key name=""Key1"">
                    <props name=""TestProp"" />
                </key>", itsPropDefs);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropElementMissingName()
        {
            itsLoader.LoadKey(@"
                <key name=""Key1"">
                    <prop />
                </key>", itsPropDefs);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropElementForNonExistingProp()
        {
            itsLoader.LoadKey(@"
                <key name=""Key1"">
                    <prop name=""doesntexist"" />
                </key>", itsPropDefs);
        }
    }
}