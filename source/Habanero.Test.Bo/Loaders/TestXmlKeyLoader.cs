//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System.Threading;
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
        private IPropDefCol _propDefCol;

        [SetUp]
        public virtual void SetupTest()
        {
            Initialise();
            GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }
        
        protected void Initialise() {
            _xmlKeyLoader = new XmlKeyLoader(new DtdLoader(), GetDefClassFactory());
            _propDefCol = GetDefClassFactory().CreatePropDefCol();
            _propDefCol.Add(GetDefClassFactory().CreatePropDef("TestProp", "System", "String", PropReadWriteRule.ReadWrite, "TestProp", null, false, false, 255, null, null, false));
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }


        [Test]
        public void TestLoadSimpleKey()
        {
            IKeyDef def = _xmlKeyLoader.LoadKey(@"<key><prop name=""TestProp"" /></key>", _propDefCol);
            Assert.AreEqual(1, def.Count);
            Assert.IsFalse(def.IgnoreIfNull);
        }

        [Test]
        public void TestLoadKeyWithMultipleProps()
        {
            _propDefCol.Add(GetDefClassFactory().CreatePropDef("TestProp2", "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false));
            IKeyDef def =
                _xmlKeyLoader.LoadKey(
                    @"<key name=""Key1""><prop name=""TestProp"" /><prop name=""TestProp2"" /></key>", _propDefCol);
            Assert.AreEqual(2, def.Count);

        }

        [Test]
        public virtual void TestLoadKeyWithMultipleProps_UsesKeyName()
        {
            _propDefCol.Add(GetDefClassFactory().CreatePropDef("TestProp2", "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false));
            IKeyDef def =
                _xmlKeyLoader.LoadKey(
                    @"<key name=""Key1""><prop name=""TestProp"" /><prop name=""TestProp2"" /></key>", _propDefCol);
            Assert.AreEqual("Key1", def.KeyName);
        }

        [Test]
        public virtual void TestLoadKeyWithMultipleProps_WithNoKeyName_UsesPropertyNames()
        {
            _propDefCol.Add(GetDefClassFactory().CreatePropDef("TestProp2", "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false));
            IKeyDef def =
                _xmlKeyLoader.LoadKey(
                    @"<key><prop name=""TestProp"" /><prop name=""TestProp2"" /></key>", _propDefCol);
            Assert.AreEqual("TestProp_TestProp2", def.KeyName);
        }

        [Test]
        public void TestLoadKeyWithIgnoreNulls()
        {
            IKeyDef def =
                _xmlKeyLoader.LoadKey(@"<key name=""Key1"" ignoreIfNull=""true""><prop name=""TestProp"" /></key>",
                                  _propDefCol);
            Assert.IsTrue(def.IgnoreIfNull);
        }

        [Test]
        public void TestLoadKeyWithWrongElementName()
        {
            try
            {
                _xmlKeyLoader.LoadKey(@"<keyDef name=""Key1""><prop name=""TestProp"" /></keyDef>", _propDefCol);
                Assert.Fail("Expected to throw an RecordedExceptionsException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An invalid node 'keyDef' was encountered when loading the class", ex.Message);
            }
        }

        [Test]
        public void TestInvalidIgnoreIfNullValue()
        {
            try
            {
                _xmlKeyLoader.LoadKey(@"
                <key name=""Key1"" ignoreIfNull=""invalidvalue"">
                    <prop name=""TestProp"" />
                </key>", _propDefCol);
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("In a 'key' element, the 'ignoreIfNull' attribute provided an invalid", ex.Message);
            }
        }

        [Test]
        public void TestPropElementInvalid()
        {
            try
            {
                _xmlKeyLoader.LoadKey(@"
                <key name=""Key1"">
                    <props name=""TestProp"" />
                </key>", _propDefCol);
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("A 'key' node is missing 'prop' nodes", ex.Message);
            }
        }

        [Test]
        public void TestPropElementMissingName()
        {
            try
            {
                _xmlKeyLoader.LoadKey(@"
                <key name=""Key1"">
                    <prop />
                </key>", _propDefCol);
                Assert.Fail("Expected to throw an RecordedExceptionsException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The 'prop' node under a key definition is missing a valid 'name'", ex.Message);
            }
        }

        [Test]
        public void TestPropElementForNonExistingProp()
        {
            //This test was changed to not expect an exception anymore because the 
            // prop could come from an inherited class.
            IKeyDef keyDef = _xmlKeyLoader.LoadKey(@"
                <key name=""Key1"">
                    <prop name=""doesntexist"" />
                </key>", _propDefCol);
            Assert.AreEqual(1, keyDef.Count);
            Assert.IsFalse(_propDefCol.Contains(keyDef["doesntexist"]), 
                "A temporary propDef should have been created for this prop. This will be clarified later.");
        }
    }
}