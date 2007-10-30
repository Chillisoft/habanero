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