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
    /// Summary description for TestXmlPrimaryKeyLoader.
    /// </summary>
    [TestFixture]
    public class TestXmlPrimaryKeyLoader
    {
        private XmlPrimaryKeyLoader itsLoader;
        private IPropDefCol itsPropDefs;

        [SetUp]
        public virtual void SetupTest()
        {
            Initialise();
                        GlobalRegistry.UIExceptionNotifier = new RethrowingExceptionNotifier();
        }

        protected void Initialise() {
            itsLoader = new XmlPrimaryKeyLoader(new DtdLoader(), GetDefClassFactory());
            itsPropDefs = GetDefClassFactory().CreatePropDefCol();
            itsPropDefs.Add(GetDefClassFactory().CreatePropDef("TestProp", "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false));
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestSimplePrimaryKey()
        {
            IPrimaryKeyDef def =
                itsLoader.LoadPrimaryKey(@"<primaryKey><prop name=""TestProp"" /></primaryKey>", itsPropDefs);
            Assert.AreEqual(1, def.Count, "Def should have one property in it.");
            Assert.AreEqual(true, def.IsGuidObjectID, "Def should by default be an objectID");
        }

        //TODO andrew 03 Jan 2011: CF: Test fails because there is no DTD validation 
        [Test]
        public void TestWrongElementName()
        {
            try
            {
                itsLoader.LoadPrimaryKey(@"<primaryKeyDef><prop name=""TestProp"" /></primaryKeyDef>", itsPropDefs);
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("An invalid node 'primaryKeyDef' was encountered when loading the class definitions.", ex.Message);
            }
        }

        [Test]
        public void TestWithNoProps()
        {
            try
            {
                itsLoader.LoadPrimaryKey(@"<primaryKey></primaryKey>", itsPropDefs);
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("A primaryKey node must have one or more prop nodes", ex.Message);
            }
        }

        [Test]
        public void TestCompositeKey()
        {
            itsPropDefs.Add(GetDefClassFactory().CreatePropDef("TestProp2", "System", "String", PropReadWriteRule.ReadWrite, null, null, false, false, 255, null, null, false));
            IPrimaryKeyDef def =
                itsLoader.LoadPrimaryKey(
                    @"<primaryKey isObjectID=""false""><prop name=""TestProp"" /><prop name=""TestProp2"" /></primaryKey>",
                    itsPropDefs);
            Assert.AreEqual(2, def.Count, "Def should have one property in it.");
            Assert.AreEqual(false, def.IsGuidObjectID, "Def should not be an objectID");
        }

        [Test]
        public void TestPropElementMissingName()
        {
            try
            {
                itsLoader.LoadPrimaryKey(@"
                <primaryKey>
                    <prop />
                </primaryKey>", itsPropDefs);
                Assert.Fail("Expected to throw an InvalidXmlDefinitionException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("The 'prop' element must have a 'name' attribute that provides the name of the property definition that serves as the primary key.", ex.Message);
            }
        }

        [Test]
        public void TestPropElementForNonExistingProp()
        {
            try
            {
                itsLoader.LoadPrimaryKey(@"
                <primaryKey>
                    <prop name=""doesntexist"" />
                </primaryKey>", itsPropDefs);
                Assert.Fail("Expected to throw an AbandonedMutexException");
            }
                //---------------Test Result -----------------------
            catch (InvalidXmlDefinitionException ex)
            {
                StringAssert.Contains("A primary key definition has listed a 'prop' definition for ", ex.Message);
            }
        }
    }
}