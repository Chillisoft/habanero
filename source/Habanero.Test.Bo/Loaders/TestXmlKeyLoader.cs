//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using NUnit.Framework;

namespace Habanero.Test.BO.Loaders
{
    /// <summary>
    /// Summary description for TestXmlKeyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlKeyLoader
    {
        private XmlKeyLoader _xmlKeyLoader;
        private PropDefCol _propDefCol;

        [SetUp]
        public void SetupTest()
        {
            _xmlKeyLoader = new XmlKeyLoader(new DtdLoader(), GetDefClassFactory());
            _propDefCol = new PropDefCol();
            _propDefCol.Add(new PropDef("TestProp", typeof (string), PropReadWriteRule.ReadWrite, null));
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }


        [Test]
        public void TestLoadSimpleKey()
        {
            KeyDef def = _xmlKeyLoader.LoadKey(@"<key><prop name=""TestProp"" /></key>", _propDefCol);
            Assert.AreEqual(1, def.Count);
            Assert.IsFalse(def.IgnoreIfNull);
        }

        [Test]
        public void TestLoadKeyWithMultipleProps()
        {
            _propDefCol.Add(new PropDef("TestProp2", typeof (string), PropReadWriteRule.ReadWrite, null));
            KeyDef def =
                _xmlKeyLoader.LoadKey(
                    @"<key name=""Key1""><prop name=""TestProp"" /><prop name=""TestProp2"" /></key>", _propDefCol);
            Assert.AreEqual(2, def.Count);
            Assert.AreEqual("Key1_TestProp_TestProp2", def.KeyName);
        }

        [Test]
        public void TestLoadKeyWithIgnoreNulls()
        {
            KeyDef def =
                _xmlKeyLoader.LoadKey(@"<key name=""Key1"" ignoreIfNull=""true""><prop name=""TestProp"" /></key>",
                                  _propDefCol);
            Assert.IsTrue(def.IgnoreIfNull);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException), ExpectedMessage = "An invalid node 'keyDef' was encountered when loading the class definitions.")]
        public void TestLoadKeyWithWrongElementName()
        {
            _xmlKeyLoader.LoadKey(@"<keyDef name=""Key1""><prop name=""TestProp"" /></keyDef>", _propDefCol);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestInvalidIgnoreIfNullValue()
        {
            _xmlKeyLoader.LoadKey(@"
                <key name=""Key1"" ignoreIfNull=""invalidvalue"">
                    <prop name=""TestProp"" />
                </key>", _propDefCol);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropElementInvalid()
        {
            _xmlKeyLoader.LoadKey(@"
                <key name=""Key1"">
                    <props name=""TestProp"" />
                </key>", _propDefCol);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropElementMissingName()
        {
            _xmlKeyLoader.LoadKey(@"
                <key name=""Key1"">
                    <prop />
                </key>", _propDefCol);
        }

        [Test]
        public void TestPropElementForNonExistingProp()
        {
            //This test was changed to not expect an exception anymore because the 
            // prop could come from an inherited class.
            KeyDef keyDef = _xmlKeyLoader.LoadKey(@"
                <key name=""Key1"">
                    <prop name=""doesntexist"" />
                </key>", _propDefCol);
            Assert.AreEqual(1, keyDef.Count);
            Assert.IsFalse(_propDefCol.Contains(keyDef["doesntexist"]), 
                "A temporary propDef should have been created for this prop. This will be clarified later.");
        }
    }
}