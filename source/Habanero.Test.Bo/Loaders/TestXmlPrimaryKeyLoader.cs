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
            itsLoader = new XmlPrimaryKeyLoader(new DtdLoader(), GetDefClassFactory());
            itsPropDefs = new PropDefCol();
            itsPropDefs.Add(new PropDef("TestProp", typeof (string), PropReadWriteRule.ReadWrite, null));
        }

        protected virtual IDefClassFactory GetDefClassFactory()
        {
            return new DefClassFactory();
        }

        [Test]
        public void TestSimplePrimaryKey()
        {
            PrimaryKeyDef def =
                itsLoader.LoadPrimaryKey(@"<primaryKey><prop name=""TestProp"" /></primaryKey>", itsPropDefs);
            Assert.AreEqual(1, def.Count, "Def should have one property in it.");
            Assert.AreEqual(true, def.IsGuidObjectID, "Def should by default be an objectID");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException), ExpectedMessage = "An invalid node 'primaryKeyDef' was encountered when loading the class definitions.")]

        public void TestWrongElementName()
        {
            itsLoader.LoadPrimaryKey(@"<primaryKeyDef><prop name=""TestProp"" /></primaryKeyDef>", itsPropDefs);
        }

        [Test, ExpectedException(typeof (InvalidXmlDefinitionException), ExpectedMessage = "A primaryKey node must have one or more prop nodes")]
        public void TestWithNoProps()
        {
            itsLoader.LoadPrimaryKey(@"<primaryKey></primaryKey>", itsPropDefs);
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
            Assert.AreEqual(false, def.IsGuidObjectID, "Def should not be an objectID");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropElementMissingName()
        {
            itsLoader.LoadPrimaryKey(@"
                <primaryKey>
                    <prop />
                </primaryKey>", itsPropDefs);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestPropElementForNonExistingProp()
        {
            itsLoader.LoadPrimaryKey(@"
                <primaryKey>
                    <prop name=""doesntexist"" />
                </primaryKey>", itsPropDefs);
        }
    }
}