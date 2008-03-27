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
            _propDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadWrite, null);
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
            _prop.InitialiseProp("OriginalValue");
            
            _prop.Value = "Prop New Value";
            Assert.AreEqual("Prop New Value", _prop.Value);
            Assert.IsTrue(_prop.IsDirty);
            Assert.IsTrue(_prop.IsValid);

            _prop.Value = "Second New Value";
            Assert.AreEqual("Second New Value", _prop.Value);
            Assert.AreEqual("Prop New Value", _prop.ValueBeforeLastEdit);
            Assert.IsTrue(_prop.IsDirty);
            Assert.IsTrue(_prop.IsValid);

            _prop.RestorePropValue();
            Assert.AreEqual("OriginalValue", _prop.Value);
            Assert.AreEqual("OriginalValue", _prop.ValueBeforeLastEdit);
            Assert.IsFalse(_prop.IsDirty);
            Assert.IsTrue(_prop.IsValid);
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
            Assert.IsFalse(lBOProp.IsValid);
            Assert.IsTrue(lBOProp.InvalidReason.Length > 0);
            lBOProp.Value = "New Value";
            Assert.IsTrue(lBOProp.IsValid);
            Assert.IsFalse(lBOProp.InvalidReason.Length > 0);
            lBOProp.RestorePropValue();
            Assert.IsFalse(lBOProp.IsValid);
            Assert.IsTrue(lBOProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForStrings()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);
            
            boProp.Value = "";
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "New Value";
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForGuids()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Guid",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = Guid.Empty;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = Guid.NewGuid();
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForIntegers()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Int32",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = 0;
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForDecimals()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Decimal",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = 0.0m;
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForDoubles()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Double",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = 0.0d;
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForDateTime()
        {
            PropDef propDef = new PropDef("TestProp", "System", "DateTime",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DateTime.Now;
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestPropCompulsoryForBooleans()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Boolean",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);

            boProp.Value = null;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = true;
            Assert.IsTrue(boProp.IsValid);
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
            Assert.IsTrue(lBOProp.IsValid);
            try
            {
                lBOProp.Value = "New Value";
            }
            catch (InvalidPropertyValueException)
            {
                //do nothing
            }
            Assert.IsFalse(lBOProp.IsValid);
            lBOProp.RestorePropValue();
            Assert.IsTrue(lBOProp.IsValid);
        }

        [Test]
        public void TestBackupProp()
        {
            _prop.InitialiseProp("OriginalValue");
            _prop.Value = "Prop New Value";
            Assert.AreEqual("Prop New Value", _prop.Value);

            _prop.Value = "Second New Value";
            Assert.AreEqual("Second New Value", _prop.Value);
            Assert.AreEqual("OriginalValue", _prop.PersistedPropertyValue);
            Assert.AreEqual("Prop New Value", _prop.ValueBeforeLastEdit);
            
            _prop.BackupPropValue();
            Assert.AreEqual("Second New Value", _prop.Value);
            Assert.AreEqual("Second New Value", _prop.PersistedPropertyValue);
            Assert.AreEqual("Second New Value", _prop.ValueBeforeLastEdit);
            Assert.IsFalse(_prop.IsDirty);
            Assert.IsTrue(_prop.IsValid);
        }

        [Test]
        //Test that the proprety is not being set to dirty when the 
        // value is set but has not changed.
        public void TestDirtyProp()
        {
            _prop.InitialiseProp("OriginalValue");
            _prop.Value = "OriginalValue";
            Assert.IsFalse(_prop.IsDirty);
            Assert.IsTrue(_prop.IsValid);
        }

        [Test]
        //Test persisted property value is returned correctly.
        public void TestPersistedPropValue()
        {
            _prop.InitialiseProp("OriginalValue");
            _prop.Value = "New Value";
            Assert.IsTrue(_prop.IsDirty);
            Assert.AreEqual("OriginalValue", _prop.PersistedPropertyValue);
            Assert.AreEqual("PropName = 'New Value'", _prop.DatabaseNameFieldNameValuePair(null));
            Assert.AreEqual("PropName = 'OriginalValue'", _prop.PersistedDatabaseNameFieldNameValuePair(null));
        }

        [Test]
        public void TestValueBeforeLastEdit()
        {
            _prop.InitialiseProp("OriginalValue");
            Assert.AreEqual("OriginalValue", _prop.ValueBeforeLastEdit);
            _prop.Value = "New Value";
            Assert.AreEqual("OriginalValue", _prop.ValueBeforeLastEdit);
            _prop.Value = "Second New Value";
            Assert.AreEqual("New Value", _prop.ValueBeforeLastEdit);
            _prop.Value = "Third New Value";
            Assert.AreEqual("Second New Value", _prop.ValueBeforeLastEdit);
        }

        [Test]
        //Test DirtyXML.
        public void TestDirtyXml()
        {
            _prop.InitialiseProp("OriginalValue");
            _prop.Value = "New Value";
            Assert.IsTrue(_prop.IsDirty);
            string dirtyXml = "<" + _prop.PropertyName + "><PreviousValue>OriginalValue" +
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
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);

            boProp.Value = null;
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);

            boProp.Value = "";
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);

            boProp.Value = "abc";
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);

            boProp.Value = "abcde";
            Assert.IsTrue(boProp.IsValid);
            Assert.IsFalse(boProp.InvalidReason.Length > 0);
        }

        [Test]
        public void TestDisplayNameAssignment()
        {
            Assert.AreEqual("Prop Name", _prop.DisplayName);
            _prop.DisplayName = "Property Name";
            Assert.AreEqual("Property Name", _prop.DisplayName);
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
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Property'"));

            boProp.Value = "abcdef";
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Prop'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Property'"));

            boProp.DisplayName = "Test Property";
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));
            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Property'"));
        }

        [Test]
        public void TestDisplayNameSetBeforeInvalid()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                          PropReadWriteRule.ReadWrite, null, null, false, false, 5);
            BOProp boProp = new BOProp(propDef);

            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Property'"));

            boProp.DisplayName = "Test Property";
            boProp.Value = "abcdef";
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));
            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Property'"));
        }

        [Test]
        public void TestDisplayNameSetAfterCompulsoryInitialisation()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                  PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = propDef.CreateBOProp(true);

            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Prop'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Property'"));

            boProp.DisplayName = "Test Property";
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));
            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Property'"));
        }

        #region Tests for Read Write Rules

        #region Shared Methods

        private static void WriteTestValues(BOProp boProp)
        {
            boProp.Value = "TestValue";
            Assert.AreEqual("TestValue", boProp.Value, "BOProp value should now have the given value.");
            boProp.Value = "TestValue2";
            Assert.AreEqual("TestValue2", boProp.Value, "BOProp value should now have the given value.");
            boProp.Value = "TestValue3";
            Assert.AreEqual("TestValue3", boProp.Value, "BOProp value should now have the given value.");
            boProp.Value = "TestValue4";
            Assert.AreEqual("TestValue4", boProp.Value, "BOProp value should now have the given value.");
        }

        #endregion //Shared Methods

        #region Test ReadWrite

        [Test]
        public void TestUpdateProp_ReadWrite_New()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                  PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = propDef.CreateBOProp(true);
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            boProp.Value = "TestValue";
            Assert.AreEqual("TestValue", boProp.Value, "BOProp value should now have the given value");
            boProp.BackupPropValue();
            boProp.Value = "TestValue2";
            Assert.AreEqual("TestValue2", boProp.Value, "BOProp value should now have the given value");
        }

        [Test]
        public void TestUpdateProp_ReadWrite_Existing()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                  PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            boProp.Value = "TestValue";
            Assert.AreEqual("TestValue", boProp.Value, "BOProp value should now have the given value");
            boProp.BackupPropValue();
            boProp.Value = "TestValue2";
            Assert.AreEqual("TestValue2", boProp.Value, "BOProp value should now have the given value");
        }

        #endregion //Test ReadWrite

        #region Test ReadOnly

        [Test, ExpectedException(typeof(BusinessObjectReadWriteRuleException))]
        public void TestUpdateProp_ReadOnly_New()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                  PropReadWriteRule.ReadOnly, null, null, true, false);
            BOProp boProp = propDef.CreateBOProp(true);
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            boProp.Value = "TestValue";
            //Assert.AreEqual("TestValue", boProp.Value, "BOProp value should now have the given value");
        }

        [Test, ExpectedException(typeof(BusinessObjectReadWriteRuleException))]
        public void TestUpdateProp_ReadOnly_Existing()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                  PropReadWriteRule.ReadOnly, null, null, true, false);
            BOProp boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            boProp.Value = "TestValue";
            //Assert.AreEqual("TestValue", boProp.Value, "BOProp value should now have the given value");
        }

        #endregion //Test ReadOnly

        #region Test WriteOnce

        private static BOProp CreateWriteOnceBoProp(bool isNew)
        {
            return CreateWriteOnceBoProp(isNew, null);
        }

        private static BOProp CreateWriteOnceBoProp(bool isNew, object defaultValue)
        {
            BOProp boProp;
            PropDef propDef = new PropDef("TestProp", typeof(String),
                                          PropReadWriteRule.WriteOnce, null, defaultValue, true, false);
            boProp = propDef.CreateBOProp(isNew);
            Assert.AreEqual(defaultValue, boProp.Value, "BOProp value should start being the default value");
            boProp.Value = "TestValue";
            Assert.AreEqual("TestValue", boProp.Value, "BOProp value should now have the given value.");
            boProp.Value = "TestValue2";
            Assert.AreEqual("TestValue2", boProp.Value, "BOProp value should now have the given value.");
            boProp.Value = "TestValue3";
            Assert.AreEqual("TestValue3", boProp.Value, "BOProp value should now have the given value.");
            boProp.Value = "TestValue4";
            Assert.AreEqual("TestValue4", boProp.Value, "BOProp value should now have the given value.");
            return boProp;
        }

        [Test]
        public void TestUpdateProp_WriteOnce_New()
        {
            CreateWriteOnceBoProp(true);
        }
 
        [Test]
        public void TestUpdateProp_WriteOnce_New_With_Default()
        {
            CreateWriteOnceBoProp(true, "My Default");
        }

        [Test, ExpectedException(typeof(BusinessObjectReadWriteRuleException))]
        public void TestUpdateProp_WriteOnce_NewPersisted_WriteAgain()
        {
            BOProp boProp = CreateWriteOnceBoProp(true);
            boProp.BackupPropValue();
            boProp.IsObjectNew = false;
            boProp.Value = "NewValue";
        }

        [Test]
        public void TestUpdateProp_WriteOnce_Existing()
        {
            CreateWriteOnceBoProp(false);
        }

        [Test, ExpectedException(typeof(BusinessObjectReadWriteRuleException))]
        public void TestUpdateProp_WriteOnce_Existing_WriteAgain()
        {
            BOProp boProp = CreateWriteOnceBoProp(false);
            boProp.BackupPropValue();
            boProp.Value = "NewValue";
        }

        #endregion //Test WriteOnce

        #region Test WriteNew

        private static BOProp CreateWriteNewBoProp(bool isNew)
        {
            BOProp boProp;
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                          PropReadWriteRule.WriteNew, null, null, true, false);
            boProp = propDef.CreateBOProp(isNew);
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            boProp.Value = "TestValue";
            Assert.AreEqual("TestValue", boProp.Value, "BOProp value should now have the given value.");
            boProp.Value = "TestValue2";
            Assert.AreEqual("TestValue2", boProp.Value, "BOProp value should now have the given value.");
            boProp.Value = "TestValue3";
            Assert.AreEqual("TestValue3", boProp.Value, "BOProp value should now have the given value.");
            boProp.Value = "TestValue4";
            Assert.AreEqual("TestValue4", boProp.Value, "BOProp value should now have the given value.");
            return boProp;
        }

        [Test]
        public void TestUpdateProp_WriteNew_New()
        {
            CreateWriteNewBoProp(true);
        }

        [Test, ExpectedException(typeof(BusinessObjectReadWriteRuleException))]
        public void TestUpdateProp_WriteNew_NewPersisted_WriteAgain()
        {
            BOProp boProp = CreateWriteNewBoProp(true);
            boProp.BackupPropValue();
            boProp.IsObjectNew = false;
            boProp.Value = "NewValue";
        }

        [Test, ExpectedException(typeof(BusinessObjectReadWriteRuleException))]
        public void TestUpdateProp_WriteNew_Existing()
        {
            CreateWriteNewBoProp(false);
        }

        #endregion //Test WriteNew

        #region Test WriteNotNew

        private static BOProp CreateWriteNotNewBoProp(bool isNew)
        {
            BOProp boProp;
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                          PropReadWriteRule.WriteNotNew, null, null, true, false);
            boProp = propDef.CreateBOProp(isNew);
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            return boProp;
        }

        private static BOProp CreateWriteNotNewBoPropWithValues(bool isNew)
        {
            BOProp boProp = CreateWriteNotNewBoProp(isNew);
            WriteTestValues(boProp);
            return boProp;
        }

        [Test, ExpectedException(typeof(BusinessObjectReadWriteRuleException))]
        public void TestUpdateProp_WriteNotNew_New()
        {
            CreateWriteNotNewBoPropWithValues(true);
        }

        [Test]
        public void TestUpdateProp_WriteNotNew_NewPersisted_WriteAgain()
        {
            BOProp boProp = CreateWriteNotNewBoProp(true);
            boProp.BackupPropValue();
            boProp.IsObjectNew = false;
            WriteTestValues(boProp);
        }

        [Test]
        public void TestUpdateProp_WriteNotNew_Existing()
        {
            CreateWriteNotNewBoPropWithValues(false);
        }

        [Test]
        public void TestUpdateProp_WriteNotNew_Existing_WriteAgain()
        {
            BOProp boProp = CreateWriteNotNewBoPropWithValues(false);
            boProp.BackupPropValue();
            WriteTestValues(boProp);
        }

        #endregion //Test WriteNotNew

        #endregion //Tests for Read Write Rules

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
