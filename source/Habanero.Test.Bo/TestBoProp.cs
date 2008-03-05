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
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBoProp
    {
        private PropDef _propDef;
        private BOProp _prop;

        [SetUp]
        public void init()
        {
            _propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            _prop = _propDef.CreateBOProp(false);
        }

        [Test]
        public void TestSetBOPropValue()
        {
            _prop.Value = "Prop Value";
            Assert.AreEqual("Prop Value", _prop.Value);
        }

        [Test]
        public void TestRestorePropValue()
        {
            _prop.InitialiseProp("OrigionalValue");
            _prop.Value = "Prop New Value";
            Assert.AreEqual("Prop New Value", _prop.Value);
            Assert.IsTrue(_prop.IsDirty);
            Assert.IsTrue(_prop.isValid);
            _prop.RestorePropValue();
            Assert.AreEqual("OrigionalValue", _prop.Value);
            Assert.IsFalse(_prop.IsDirty);
            Assert.IsTrue(_prop.isValid);
        }

        #region Tests for Compulsory Attribute

        [Test]
        public void TestPropCompulsoryRestore()
        {
            //Test compulsory with no default set
            PropDef lPropDefWithRules = new PropDef("PropNameWithRules", "System", "String",
                                                    PropReadWriteRule.ReadWrite, null, null, true, false);
            lPropDefWithRules.PropRule = new PropRuleString(lPropDefWithRules.PropertyName, "", -1, -1, null);
            BOProp lBOProp = lPropDefWithRules.CreateBOProp(true);
            Assert.IsFalse(lBOProp.isValid);
            Assert.IsTrue(lBOProp.InvalidReason.Length > 0);
            lBOProp.Value = "New Value";
            Assert.IsTrue(lBOProp.isValid);
            Assert.IsFalse(lBOProp.InvalidReason.Length > 0);
            lBOProp.RestorePropValue();
            Assert.IsFalse(lBOProp.isValid);
            Assert.IsTrue(lBOProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForStrings()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);
            
            boProp.Value = "";
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "New Value";
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForGuids()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Guid",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = Guid.Empty;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = Guid.NewGuid();
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForIntegers()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Int32",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = 0;
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForDecimals()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Decimal",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = 0.0m;
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForDoubles()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Double",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = 0.0d;
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForDateTime()
        {
            PropDef propDef = new PropDef("TestProp", "System", "DateTime",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DateTime.Now;
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForBooleans()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Boolean",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = true;
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        #endregion //Tests for Compulsory Attribute
        
        [Test]
        public void TestPropBrokenRuleRestore()
        {
            //Test compulsory with no default set
            PropDef lPropDefWithRules = new PropDef("PropNameWithRules", typeof(string),
                                                    PropReadWriteRule.ReadWrite, null);
            lPropDefWithRules.PropRule = new PropRuleString(lPropDefWithRules.PropertyName, "", 50, 51, null);
            BOProp lBOProp = lPropDefWithRules.CreateBOProp(true);
            Assert.IsTrue(lBOProp.isValid);
            try
            {
                lBOProp.Value = "New Value";
            }
            catch (InvalidPropertyValueException)
            {
                //do nothing
            }
            Assert.IsFalse(lBOProp.isValid);
            lBOProp.RestorePropValue();
            Assert.IsTrue(lBOProp.isValid);
        }

        [Test]
        public void TestBackupProp()
        {
            _prop.InitialiseProp("OrigionalValue");
            _prop.Value = "Prop New Value";
            Assert.AreEqual("Prop New Value", _prop.Value);
            _prop.BackupPropValue();
            Assert.AreEqual("Prop New Value", _prop.Value);
            Assert.IsFalse(_prop.IsDirty);
            Assert.IsTrue(_prop.isValid);
        }

        [Test]
        //Test that the proprety is not being set to dirty when the 
        // value is set but has not changed.
        public void TestDirtyProp()
        {
            _prop.InitialiseProp("OrigionalValue");
            _prop.Value = "OrigionalValue";
            Assert.IsFalse(_prop.IsDirty);
            Assert.IsTrue(_prop.isValid);
        }

        [Test]
        //Test persisted property value is returned correctly.
        public void TestPersistedPropValue()
        {
            _prop.InitialiseProp("OrigionalValue");
            _prop.Value = "New Value";
            Assert.IsTrue(_prop.IsDirty);
            Assert.AreEqual("OrigionalValue", _prop.PersistedPropertyValue);
            Assert.AreEqual("PropName = 'New Value'", _prop.DatabaseNameFieldNameValuePair(null));
            Assert.AreEqual("PropName = 'OrigionalValue'", _prop.PersistedDatabaseNameFieldNameValuePair(null));
        }

        [Test]
        //Test DirtyXML.
        public void TestDirtyXml()
        {
            _prop.InitialiseProp("OrigionalValue");
            _prop.Value = "New Value";
            Assert.IsTrue(_prop.IsDirty);
            string dirtyXml = "<" + _prop.PropertyName + "><PreviousValue>OrigionalValue" +
                              "</PreviousValue><NewValue>New Value</NewValue></" +
                              _prop.PropertyName + ">";
            Assert.AreEqual(dirtyXml, _prop.DirtyXml);
        }

        [Test]
        public void TestPropLengthForStrings()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                PropReadWriteRule.ReadWrite, null, null, false, false, 5);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = "abcdef";
            Assert.IsFalse(boProp.isValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = null;
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);

            boProp.Value = "abc";
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);

            boProp.Value = "abcde";
            Assert.IsTrue(boProp.isValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestDisplayNameAssignment()
        {
            Assert.AreEqual("PropName", _prop.DisplayName);
            _prop.DisplayName = "Prop Name";
            Assert.AreEqual("Prop Name", _prop.DisplayName);
            Assert.IsFalse(_prop.InvalidReason.Length > 0);
        }

        [Test]
        public void TestDisplayNameSetAfterInvalid()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                          PropReadWriteRule.ReadWrite, null, null, false, false, 5);
            BOProp boProp = new BOProp(propDef);

            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));

            boProp.Value = "abcdef";
            Assert.IsTrue(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));

            boProp.DisplayName = "Test Prop";
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Prop'"));
        }

        [Test]
        public void TestDisplayNameSetBeforeInvalid()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                          PropReadWriteRule.ReadWrite, null, null, false, false, 5);
            BOProp boProp = new BOProp(propDef);

            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));

            boProp.DisplayName = "Test Prop";
            boProp.Value = "abcdef";
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Prop'"));
        }

        [Test]
        public void TestDisplayNameSetAfterCompulsoryInitialisation()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                  PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = propDef.CreateBOProp(true);

            Assert.IsTrue(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));

            boProp.DisplayName = "Test Prop";
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Prop'"));
        }

        #region Tests for Enum type Bo Props

        [Test]
        public void TestBoPropWithEnumCreate()
        {
            PropDef propDef = new PropDef("EnumProp", typeof(ContactPerson.ContactType), PropReadWriteRule.ReadWrite, ContactPerson.ContactType.Family);
            //Create the property for a new object (default will be set)
            BOProp boProp;
            boProp = propDef.CreateBOProp(true);
            Assert.AreEqual(ContactPerson.ContactType.Family, boProp.Value);
            Assert.AreEqual("Family", boProp.PropertyValueString);
            //Create the property for anexisting object (default will not be set)
            boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value);
            Assert.AreEqual("", boProp.PropertyValueString);
        }

        public void TestBoPropWithEnumValueChange()
        {
            PropDef propDef = new PropDef("EnumProp", typeof(ContactPerson.ContactType), PropReadWriteRule.ReadWrite, ContactPerson.ContactType.Family);
            //Create the property for anexisting object (default will not be set)
            BOProp boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value);
            Assert.AreEqual("", boProp.PropertyValueString);
            boProp.InitialiseProp(ContactPerson.ContactType.Business);
            Assert.IsFalse(boProp.IsDirty);
            Assert.AreEqual(ContactPerson.ContactType.Business, boProp.Value);
            Assert.AreEqual("Business", boProp.PropertyValueString);
            boProp.Value = ContactPerson.ContactType.Friend;
            Assert.IsTrue(boProp.IsDirty);
            Assert.AreEqual(ContactPerson.ContactType.Friend, boProp.Value);
            Assert.AreEqual("Friend", boProp.PropertyValueString);
            boProp.RestorePropValue();
            Assert.IsFalse(boProp.IsDirty);
            Assert.AreEqual(ContactPerson.ContactType.Business, boProp.Value);
            Assert.AreEqual("Business", boProp.PropertyValueString);
        }

        public void TestBoPropWithEnumPersistValue()
        {
            PropDef propDef = new PropDef("EnumProp", typeof(ContactPerson.ContactType), PropReadWriteRule.ReadWrite, ContactPerson.ContactType.Family);
            //Create the property for anexisting object (default will not be set)
            BOProp boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value);
            Assert.AreEqual(null, boProp.PersistedPropertyValue);
            Assert.AreEqual("", boProp.PropertyValueString);
            Assert.AreEqual("", boProp.PersistedPropertyValueString);
            boProp.InitialiseProp(ContactPerson.ContactType.Business);
            Assert.IsFalse(boProp.IsDirty);
            Assert.AreEqual(ContactPerson.ContactType.Business, boProp.Value);
            Assert.AreEqual(ContactPerson.ContactType.Business, boProp.PersistedPropertyValue);
            Assert.AreEqual("Business", boProp.PersistedPropertyValueString);
            Assert.AreEqual("Business", boProp.PropertyValueString);
        }

        public void TestBoPropWithEnumPersistValueFromString()
        {
            PropDef propDef = new PropDef("EnumProp", typeof(ContactPerson.ContactType), PropReadWriteRule.ReadWrite, ContactPerson.ContactType.Family);
            //Create the property for anexisting object (default will not be set)
            BOProp boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value);
            Assert.AreEqual(null, boProp.PersistedPropertyValue);
            Assert.AreEqual("", boProp.PropertyValueString);
            Assert.AreEqual("", boProp.PersistedPropertyValueString);
            boProp.InitialiseProp("Business");
            Assert.IsFalse(boProp.IsDirty);
            Assert.AreEqual(ContactPerson.ContactType.Business, boProp.Value);
            Assert.AreEqual(ContactPerson.ContactType.Business, boProp.PersistedPropertyValue);
            Assert.AreEqual("Business", boProp.PersistedPropertyValueString);
            Assert.AreEqual("Business", boProp.PropertyValueString);
        }

        #endregion //Tests for Enum type Bo Props
    }

}
