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
    }

}
