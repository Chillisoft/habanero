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
using System.Collections.Generic;
using System.Text;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{

    [TestFixture]
    public class TestPropDef
    {
        private PropDef _propDef;

        [SetUp]
        public void Init()
        {
            _propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
        }

        [Test]
        public void TestCreatePropDef()
        {
            Assert.AreEqual("PropName", _propDef.PropertyName);
            Assert.AreEqual("PropName", _propDef.DatabaseFieldName);
            Assert.AreEqual(typeof(string), _propDef.PropType);
            PropDef lPropDef = new PropDef("prop", typeof(int), PropReadWriteRule.ReadWrite, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreatePropDefInvalidDefault()
        {
            PropDef lPropDef = new PropDef("prop", typeof(int), PropReadWriteRule.ReadWrite, "");
        }

        [Test]
        public void TestCreateLatePropDefInvalidTypeNotAccessed()
        {
            PropDef lPropDef = new PropDef("prop", "NonExistentAssembly", "NonExistentType", PropReadWriteRule.ReadWrite, null, "", false, false);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreatePropDefInvalidDefault2()
        {
            PropDef lPropDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, 1);
        }

        [Test]
        [ExpectedException(typeof(UnknownTypeNameException))]
        public void TestCreateLatePropDefInvalidType()
        {
            PropDef propDef = new PropDef("prop", "NonExistentAssembly", "NonExistentType", PropReadWriteRule.ReadWrite, null, "", false, false);
            Type propType = propDef.PropertyType;
            Assert.Fail("This line should not be reached because the previous line should have failed.");
        }

        [Test]
        public void TestCreateLatePropDefInvalidDefaultNotAccessed()
        {
            PropDef lPropDef = new PropDef("prop", "System", "Int32", PropReadWriteRule.ReadWrite, null, "", false, false);
            //No error
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void TestCreateLatePropDefInvalidDefault()
        {
            PropDef lPropDef = new PropDef("prop", "System", "Int32", PropReadWriteRule.ReadWrite, null, "", false, false);
            object defaultValue = lPropDef.DefaultValue;
            Assert.Fail("This line should not be reached because the previous line should have failed.");
        }

        public void TestCreateBOProp()
        {
            BOProp prop = _propDef.CreateBOProp(false);
            Assert.AreEqual("PropName", prop.PropertyName);
            Assert.AreEqual("PropName", prop.DatabaseFieldName);
        }
    }

}
