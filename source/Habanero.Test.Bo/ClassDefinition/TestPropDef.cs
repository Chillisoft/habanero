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
