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

using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.CriteriaManager;
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
            new PropDef("prop", typeof(int), PropReadWriteRule.ReadWrite, 1);
        }

        [Test]
        public void Test_SetPropDefUnitOfMeasure()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            //---------------Assert Precondition----------------
            Assert.AreEqual("", propDef.UnitOfMeasure);
            //---------------Execute Test ----------------------
            string newUOM = "New UOM";
            propDef.UnitOfMeasure = newUOM;
            //---------------Test Result -----------------------
            Assert.AreEqual(newUOM, propDef.UnitOfMeasure);

        }
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreatePropDefInvalidDefault()
        {
            new PropDef("prop", typeof(int), PropReadWriteRule.ReadWrite, "");
        }

        [Test]
        public void TestCreateLatePropDefInvalidTypeNotAccessed()
        {
            new PropDef("prop", "NonExistentAssembly", "NonExistentType", PropReadWriteRule.ReadWrite, null, "", false, false);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCreatePropDefInvalidDefault2()
        {
            new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, 1);
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
            new PropDef("prop", "System", "Int32", PropReadWriteRule.ReadWrite, null, "", false, false);
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

        [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage= "A property name cannot contain any of the following characters: [.-|]  Invalid property name This.That")]
        public void TestDotIsNotAllowedInName()
        {
            new PropDef("This.That", typeof(string), PropReadWriteRule.ReadWrite, "");
        }

        [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "A property name cannot contain any of the following characters: [.-|]  Invalid property name This-That")]
        public void TestDashIsNotAllowedInName()
        {
            new PropDef("This-That", typeof(string), PropReadWriteRule.ReadWrite, "");
        }

        [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "A property name cannot contain any of the following characters: [.-|]  Invalid property name This|That")]
        public void TestPipeIsNotAllowedInName()
        {
            new PropDef("This|That", typeof(string), PropReadWriteRule.ReadWrite, "");
        }

        [Test]
        public void TestCreatePropDefWithEnumType()
        {
            PropDef propDef = new PropDef("EnumProp", typeof(ContactPersonTestBO.ContactType), PropReadWriteRule.ReadWrite, ContactPersonTestBO.ContactType.Family);
            Assert.AreEqual("Habanero.Test.BO", propDef.PropertyTypeAssemblyName);
            Assert.AreEqual("Habanero.Test.BO.ContactPersonTestBO+ContactType", propDef.PropertyTypeName);
            Assert.AreEqual("Family", propDef.DefaultValueString);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Family, propDef.DefaultValue);
        }

        [Test]
        public void TestCreatePropDefWithEnumTypeString()
        {
            PropDef propDef = new PropDef("EnumProp", "Habanero.Test.BO", "ContactPersonTestBO+ContactType", 
                PropReadWriteRule.ReadWrite, "EnumProp" , "Family", false, false);
            Assert.AreEqual(typeof(ContactPersonTestBO.ContactType), propDef.PropertyType);
            Assert.AreEqual("Habanero.Test.BO", propDef.PropertyTypeAssemblyName);
            Assert.AreEqual("ContactPersonTestBO+ContactType", propDef.PropertyTypeName);
            Assert.AreEqual("Family", propDef.DefaultValueString);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Family, propDef.DefaultValue);
        }

        [Test]
        public void TestCreateBOProp()
        {
            IBOProp prop = _propDef.CreateBOProp(false);
            Assert.AreEqual("PropName", prop.PropertyName);
            Assert.AreEqual("PropName", prop.DatabaseFieldName);
        }

        [Test]
        public void TestGetComparer()
        {
            PropDef propDef = new PropDef("prop", typeof(string), PropReadWriteRule.ReadWrite, null);
            Assert.AreEqual("PropertyComparer`2", propDef.GetPropertyComparer<MyBO>().GetType().Name);
        }

        [Test]
        public void TestProtectedSets()
        {
            PropDefInheritor propDef = new PropDefInheritor();

            Assert.AreEqual("prop", propDef.PropertyName);
            propDef.SetPropertyName("myprop");
            Assert.AreEqual("myprop", propDef.PropertyName);

            Assert.AreEqual(typeof(MyBO), propDef.PropertyType);
            propDef.SetPropertyType(typeof(MyRelatedBo));
            Assert.AreEqual(typeof(MyRelatedBo), propDef.PropertyType);

            Assert.AreEqual("Habanero.Test.MyRelatedBo", propDef.PropertyTypeName);
            propDef.SetPropertyTypeName("myproptype");
            Assert.AreEqual("myproptype", propDef.PropertyTypeName);

            Assert.AreEqual("Habanero.Test", propDef.PropertyTypeAssemblyName);
            propDef.SetPropertyTypeAssemblyName("myassembly");
            Assert.AreEqual("myassembly", propDef.PropertyTypeAssemblyName);
            Assert.IsNull(propDef.PropertyTypeName);
            Assert.IsNull(propDef.PropertyType);

            Assert.AreEqual("prop", propDef.DatabaseFieldName);
            propDef.SetDatabaseFieldName("propfield");
            Assert.AreEqual("propfield", propDef.DatabaseFieldName);

            Assert.IsNull(propDef.DefaultValue);
            propDef.SetPropertyType(typeof(String));
            propDef.SetDefaultValue("default");
            Assert.AreEqual("default", propDef.DefaultValue);

            Assert.AreEqual("default", propDef.DefaultValueString);
            propDef.SetDefaultValueString("none");
            Assert.AreEqual("none", propDef.DefaultValueString);

            Assert.IsFalse(propDef.Compulsory);
            propDef.SetCompulsory(true);
            Assert.IsTrue(propDef.Compulsory);

            Assert.AreEqual(PropReadWriteRule.ReadWrite, propDef.ReadWriteRule);
            propDef.SetReadWriteRule(PropReadWriteRule.ReadOnly);
            Assert.AreEqual(PropReadWriteRule.ReadOnly, propDef.ReadWriteRule);

            Assert.IsTrue(propDef.IsPropValueValid("somestring"));

            Assert.AreEqual(typeof(String), propDef.PropType);
            propDef.SetPropType(typeof(DateTime));
            Assert.AreEqual(typeof(DateTime), propDef.PropType);

            PropDefParameterSQLInfo propDefParameterSQLInfo = new PropDefParameterSQLInfo(propDef);
            Assert.AreEqual(ParameterType.Date, propDefParameterSQLInfo.ParameterType);
        }

        // Used to access protected properties
        private class PropDefInheritor : PropDef
        {
            public PropDefInheritor() : base("prop", typeof(MyBO), PropReadWriteRule.ReadWrite, null)
            {}

            public void SetPropertyName(string name)
            {
                PropertyName = name;
            }

            public void SetPropertyTypeAssemblyName(string name)
            {
                PropertyTypeAssemblyName = name;
            }

            public void SetPropertyTypeName(string name)
            {
                PropertyTypeName = name;
            }

            public void SetPropertyType(Type type)
            {
                PropertyType = type;
            }

            public void SetDatabaseFieldName(string name)
            {
                DatabaseFieldName = name;
            }

            public void SetDefaultValue(object value)
            {
                DefaultValue = value;
            }

            public void SetDefaultValueString(string value)
            {
                DefaultValueString = value;
            }

            public void SetCompulsory(bool value)
            {
                Compulsory = value;
            }

            public void SetReadWriteRule(PropReadWriteRule rule)
            {
                ReadWriteRule = rule;
            }

            public bool IsPropValueValid(object value)
            {
                string errors = "";
                return IsValueValid(null, "test", ref errors);
            }

            public void SetPropType(Type type)
            {
                PropType = type;
            }
        }


    }

}
