#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
#endregion
using System;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Exceptions;
using Habanero.Test.Structure;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOProp
    {
        private PropDef _propDef;
        private IBOProp _prop;

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
        public void Test_ConstructBOPropWithValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("Name", typeof(string), PropReadWriteRule.WriteNotNew, "DD", "", false, false);
            const string value = "value";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            BOProp prop1 = new BOProp(propDef, value);

            //---------------Test Result -----------------------
            Assert.IsTrue(prop1.IsObjectNew);
            Assert.AreEqual(value, prop1.Value);
            Assert.AreEqual(value, prop1.PropertyValueToDisplay);
        }

        [Test]
        public void Test_ValidateProp_WhenPropNotValid_IsValidFalse()
        {
            //---------------Set up test pack-------------------
            //Test compulsory with no default set
            PropDef lPropDefWithRules = new PropDef("PropNameWithRules", "System", "String",
                                                    PropReadWriteRule.ReadWrite, null, null, true, false);
            lPropDefWithRules.AddPropRule(new PropRuleString(lPropDefWithRules.PropertyName, "", -1, -1, null));
            IBOProp lBOProp = lPropDefWithRules.CreateBOProp(true);
            //---------------Assert Precondition----------------
            Assert.IsTrue(lBOProp.IsValid);
            Assert.AreEqual("", lBOProp.InvalidReason);
            //---------------Execute Test ----------------------
            lBOProp.Validate();
            //---------------Test Result -----------------------
            Assert.IsFalse(lBOProp.IsValid);
            StringAssert.Contains("'Prop Name With Rules' is a compulsory field and has no value.", lBOProp.InvalidReason);
        }
        [Test]
        public void Test_ValidateProp_WhenPropValid_IsValidTrue()
        {
            //---------------Set up test pack-------------------
            //Test compulsory with no default set
            PropDef lPropDefWithRules = new PropDef("PropNameWithRules", "System", "String",
                                                    PropReadWriteRule.ReadWrite, null, null, true, false);
            lPropDefWithRules.AddPropRule(new PropRuleString(lPropDefWithRules.PropertyName, "", -1, -1, null));
            IBOProp lBOProp = lPropDefWithRules.CreateBOProp(true);
            //---------------Assert Precondition----------------
            Assert.IsTrue(lBOProp.IsValid);
            Assert.AreEqual("", lBOProp.InvalidReason);
            //---------------Execute Test ----------------------
            lBOProp.Value = "Some value";
            lBOProp.Validate();
            //---------------Test Result -----------------------
            Assert.IsTrue(lBOProp.IsValid);
        }

        [Test]
        public void Test_ValidateProp_WhenInvalidPropReloaded_ShouldRevalidate()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropPropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            const object invalidValue = null;
            boProp.Value = invalidValue;
            boProp.Validate();
            boProp.InitialiseProp(invalidValue);
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.Compulsory, "Prop should be compulsory");
            Assert.IsTrue(boProp.IsValid, "Prop should be valid until revalidated");
            Assert.IsTrue(String.IsNullOrEmpty(boProp.InvalidReason), "Prop should not have an invalid message until revalidated");
            //---------------Execute Test ----------------------
            boProp.Validate();
            //---------------Test Result -----------------------
            Assert.IsFalse(boProp.IsValid, "Prop should not be valid");
            Assert.IsFalse(String.IsNullOrEmpty(boProp.InvalidReason), "Prop should have an invalid reason");
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
        public void Test_SetInvalidPropToValidValue()
        {
            //---------------Set up test pack-------------------
            //Test compulsory with no default set
            PropDef lPropDefWithRules = new PropDef("PropNameWithRules", "System", "String",
                                                    PropReadWriteRule.ReadWrite, null, null, true, false);
            lPropDefWithRules.AddPropRule(new PropRuleString(lPropDefWithRules.PropertyName, "", -1, -1, null));
            IBOProp lBOProp = lPropDefWithRules.CreateBOProp(true);
            //Do validate
            lBOProp.Validate();
            //---------------Assert Precondition----------------
            Assert.IsFalse(lBOProp.IsValid);
            Assert.IsTrue(lBOProp.InvalidReason.Length > 0);
            //---------------Execute Test ----------------------
            lBOProp.Value = "New Value";
            //---------------Test Result -----------------------
            Assert.IsTrue(lBOProp.IsValid);
            Assert.IsFalse(lBOProp.InvalidReason.Length > 0);
        }

        [Test]
        public void Test_RestoreValidPropToInvalidState()
        {
            //---------------Set up test pack-------------------
                        //Test compulsory with no default set
            PropDef lPropDefWithRules = new PropDef("PropNameWithRules", "System", "String",
                                                    PropReadWriteRule.ReadWrite, null, null, true, false);
            lPropDefWithRules.AddPropRule(new PropRuleString(lPropDefWithRules.PropertyName, "", -1, -1, null));
            IBOProp lBOProp = lPropDefWithRules.CreateBOProp(true);
            //Do validate
            lBOProp.Validate(); 
            lBOProp.Value = "New Value";
            //---------------Assert Precondition----------------
            Assert.IsTrue(lBOProp.IsValid);
            Assert.IsFalse(lBOProp.InvalidReason.Length > 0);
            //---------------Execute Test ----------------------
            lBOProp.RestorePropValue();
            //---------------Test Result -----------------------
            Assert.IsFalse(lBOProp.IsValid);
            Assert.IsTrue(lBOProp.InvalidReason.Length > 0);
        }

        [Test]
        public void Test_RestorePropValue_WhenInvalidPropRestored_ShouldRevalidate()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropPropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            const object invalidValue = null;
            //boProp.Value = invalidValue;
            //boProp.Validate();
            boProp.InitialiseProp(invalidValue);
            boProp.Validate();
            boProp.Value = "Valid Value";
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.Compulsory, "Prop should be compulsory");
            Assert.IsTrue(boProp.IsValid, "Prop should be valid");
            Assert.IsTrue(String.IsNullOrEmpty(boProp.InvalidReason), "Prop should not have an invalid message");
            //---------------Execute Test ----------------------
            boProp.RestorePropValue();
            //---------------Test Result -----------------------
            Assert.IsFalse(boProp.IsValid, "Prop should not be valid");
            Assert.IsFalse(String.IsNullOrEmpty(boProp.InvalidReason), "Prop should have an invalid reason");
        }

        [Test]
        public void TestPropCompulsoryForStrings()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
            boProp.Value = "x";
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
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
            boProp.Value = Guid.NewGuid();

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
            Assert.IsNull(boProp.Value);
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
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
            boProp.Value = 44;


            boProp.Value = null;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);
            StringAssert.Contains("compulsory field", boProp.InvalidReason);

            boProp.Value = DBNull.Value;
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);
            StringAssert.Contains("compulsory field", boProp.InvalidReason);

            boProp.Value = "";
            Assert.IsFalse(boProp.IsValid);
            Assert.IsTrue(boProp.InvalidReason.Length > 0);
            StringAssert.Contains("compulsory field", boProp.InvalidReason);

            boProp.Value = 1;
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(boProp.IsValid);
            
            boProp.Value = 0;
            Assert.AreEqual("", boProp.InvalidReason);
            Assert.IsTrue(boProp.IsValid);
        }

        [Test]
        public void TestPropCompulsoryForDecimals()
        {
            PropDef propDef = new PropDef("TestProp", "System", "Decimal",
                PropReadWriteRule.ReadWrite, null, null, true, false);
            BOProp boProp = new BOProp(propDef);
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
            boProp.Value = 44.44;

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
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
            boProp.Value = 44.44;
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
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
            boProp.Value = DateTime.Now;
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
            Assert.IsNull(boProp.Value);
            Assert.IsTrue(boProp.IsValid);
            boProp.Value = true;
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
            lPropDefWithRules.AddPropRule( new PropRuleString(lPropDefWithRules.PropertyName, "", 50, 51, null));
            IBOProp lBOProp = lPropDefWithRules.CreateBOProp(true);
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
            BOProp prop = (BOProp) _prop;
            Assert.IsTrue(_prop.IsDirty);
            Assert.AreEqual("OriginalValue", _prop.PersistedPropertyValue);
            Assert.AreEqual("New Value", prop.PropertyValueString);
            Assert.AreEqual("OriginalValue", prop.PersistedPropertyValueString);
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
        //Test DirtyXML.
        public void TestDirtyXml_XmlReservedCharacters()
        {
            _prop.InitialiseProp("OriginalValue");
            _prop.Value = "New <Special> Value \n\r With ' & \" ";
            Assert.IsTrue(_prop.IsDirty);
            string dirtyXml = "<" + _prop.PropertyName + "><PreviousValue>OriginalValue" +
                              "</PreviousValue><NewValue>New &lt;Special&gt; Value \n\r With &apos; &amp; &quot; </NewValue></" +
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
//            _prop.DisplayName = "Property Name";
//            Assert.AreEqual("Property Name", _prop.DisplayName);
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

//            boProp.DisplayName = "Test Property";
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
//            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));
//            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Property'"));
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

//            boProp.DisplayName = "Test Property";
            boProp.Value = "abcdef";
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
//            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Prop'"));
//            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Property'"));
        }

        [Test]
        public void TestDisplayNameSetAfterCompulsoryInitialisation()
        {
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                  PropReadWriteRule.ReadWrite, null, null, true, false);
            IBOProp boProp = propDef.CreateBOProp(true);
            boProp.Validate();
            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
            Assert.IsTrue(boProp.InvalidReason.Contains("'Test Prop'"));
            Assert.IsFalse(boProp.InvalidReason.Contains("'Test Property'"));

            Assert.IsFalse(boProp.InvalidReason.Contains("'TestProp'"));
        }

        #region Tests for Read Write Rules

        #region Shared Methods

        private static void WriteTestValues(IBOProp boProp)
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
            IBOProp boProp = propDef.CreateBOProp(true);
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
            IBOProp boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            boProp.Value = "TestValue";
            Assert.AreEqual("TestValue", boProp.Value, "BOProp value should now have the given value");
            boProp.BackupPropValue();
            boProp.Value = "TestValue2";
            Assert.AreEqual("TestValue2", boProp.Value, "BOProp value should now have the given value");
        }

        #endregion //Test ReadWrite

        #region Test ReadOnly

        [Test]
        public void TestUpdateProp_ReadOnly_New()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                  PropReadWriteRule.ReadOnly, null, null, true, false);
            IBOProp boProp = propDef.CreateBOProp(true);
            //---------------Assert preconditions---------------
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            //---------------Execute Test ----------------------
            try
            {
                boProp.Value = "TestValue";
                Assert.Fail("Expected to throw an BOPropWriteException");
            }
                //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("is not editable since it is set up as ReadOnly", ex.Message);
            }
        }

        [Test]
        public void TestUpdateProp_ReadOnly_Existing()
        { 
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                  PropReadWriteRule.ReadOnly, null, null, true, false);
            IBOProp boProp = propDef.CreateBOProp(false);
            //---------------Assert preconditions---------------
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            //---------------Execute Test ----------------------
            try
            {
                boProp.Value = "TestValue";
                Assert.Fail("Expected to throw an BOPropWriteException");
            }
                //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("is not editable since it is set up as ReadOnly", ex.Message);
            }
        }

        #endregion //Test ReadOnly

        #region Test WriteOnce

        private static IBOProp CreateWriteOnceBoProp(bool isNew)
        {
            return CreateWriteOnceBoProp(isNew, null);
        }

        private static IBOProp CreateWriteOnceBoProp(bool isNew, object defaultValue)
        {
            IBOProp boProp;
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

        [Test]
        public void TestUpdateProp_WriteOnce_NewPersisted_WriteAgain()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = (BOProp) CreateWriteOnceBoProp(true);
            boProp.BackupPropValue();
            boProp.IsObjectNew = false;
            //---------------Execute Test ----------------------
            try
            {
                boProp.Value = "NewValue";
                Assert.Fail("Expected to throw an BOPropWriteException");
            }
                //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("is not editable since it is set up as WriteOnce and the value has already been set", ex.Message);
            }
        }

        [Test]
        public void TestUpdateProp_WriteOnce_NewPersisted_WriteAgain_SameValue()
        {
            BOProp boProp = (BOProp) CreateWriteOnceBoProp(true);
            boProp.BackupPropValue();
            boProp.IsObjectNew = false;
            boProp.Value = "TestValue4";
        }

        [Test]
        public void TestUpdateProp_WriteOnce_Existing()
        {
            CreateWriteOnceBoProp(false);
        }

        [Test]
        public void Test_BackUpProp_SetsIsObjectNewFalse()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = (BOProp)CreateWriteOnceBoProp(false);
            boProp.IsObjectNew = true;
            //---------------Assert Precondition----------------
            Assert.IsTrue(boProp.IsObjectNew);
            //---------------Execute Test ----------------------
            boProp.BackupPropValue();
            //---------------Test Result -----------------------
            Assert.IsFalse(boProp.IsObjectNew);
        }

        [Test]
        public void TestUpdateProp_WriteOnce_Existing_WriteAgain()
        {
            BOProp boProp = (BOProp) CreateWriteOnceBoProp(false);
            boProp.BackupPropValue();
            //-------------------Assert Precondition ----------------
            string message;
            Assert.IsFalse(boProp.IsEditable(out message));
            StringAssert.Contains("The property ", message);
            StringAssert.Contains("Test Prop' is not editable since it is set up as WriteOnce", message);
            //-------------------Execute Test -----------------------
            try
            {
                boProp.Value = "NewValue";
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("The property ", ex.Message);
                StringAssert.Contains("Test Prop' is not editable since it is set up as WriteOnce", ex.Message);
            }
        }

        [Test]
        public void TestUpdateProp_WriteOnce_Existing_WriteAgain_SameValue()
        {
            IBOProp boProp = CreateWriteOnceBoProp(false);
            boProp.BackupPropValue();
            boProp.Value = "TestValue4";
        }

        #endregion //Test WriteOnce

        #region Test WriteNew

        private static IBOProp CreateWriteNewBoProp(bool isNew)
        {
            IBOProp boProp;
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

        [Test]
        public void TestUpdateProp_WriteNew_NewPersisted_WriteAgain()
        {
            //---------------Set up test pack-------------------
            BOProp boProp = (BOProp) CreateWriteNewBoProp(true);
            boProp.BackupPropValue();
            boProp.IsObjectNew = false;
            //---------------Execute Test ----------------------
            try
            {
                boProp.Value = "NewValue";
                Assert.Fail("Expected to throw an BOPropWriteException");
            }
                //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("is not editable since it is set up as WriteNew and the object is not new", ex.Message);
            }
        }

        [Test]
        public void TestUpdateProp_WriteNew_Existing()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                          PropReadWriteRule.WriteNew, null, null, true, false);
            IBOProp boProp = propDef.CreateBOProp(false);
            //---------------Assert preconditions---------------
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            //---------------Execute Test ----------------------
            try
            {
                boProp.Value = "TestValue";
                Assert.Fail("Expected to throw an BOPropWriteException");
            }
                //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("is not editable since it is set up as WriteNew and the object is not new", ex.Message);
            }
        }

        #endregion //Test WriteNew

        #region Test WriteNotNew

        private static IBOProp CreateWriteNotNewBoProp(bool isNew)
        {
            IBOProp boProp;
            PropDef propDef = new PropDef("TestProp", "System", "String",
                                          PropReadWriteRule.WriteNotNew, null, null, true, false);
            boProp = propDef.CreateBOProp(isNew);
            Assert.AreEqual(null, boProp.Value, "BOProp value should start being null");
            return boProp;
        }

        private static IBOProp CreateWriteNotNewBoPropWithValues(bool isNew)
        {
            IBOProp boProp = CreateWriteNotNewBoProp(isNew);
            WriteTestValues(boProp);
            return boProp;
        }

        [Test]
        public void TestUpdateProp_WriteNotNew_New()
        {
            //---------------Set up test pack-------------------
            IBOProp boProp = CreateWriteNotNewBoProp(true);
            //---------------Execute Test ----------------------
            try
            {
                boProp.Value = "TestValue";
                Assert.Fail("Expected to throw an BOPropWriteException");
            }
                //---------------Test Result -----------------------
            catch (BOPropWriteException ex)
            {
                StringAssert.Contains("is not editable since it is set up as WriteNotNew and the object is new", ex.Message);
            }
        }

        [Test]
        public void TestUpdateProp_WriteNotNew_NewPersisted_WriteAgain()
        {
            BOProp boProp = (BOProp) CreateWriteNotNewBoProp(true);
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
            IBOProp boProp = CreateWriteNotNewBoPropWithValues(false);
            boProp.BackupPropValue();
            WriteTestValues(boProp);
        }

        #endregion //Test WriteNotNew

        #endregion //Tests for Read Write Rules

        #region Tests for Enum type Bo Props

        [Test]
        public void TestBoPropWithEnumCreate()
        {
            PropDef propDef = new PropDef("EnumProp", typeof(ContactPersonTestBO.ContactType), PropReadWriteRule.ReadWrite, ContactPersonTestBO.ContactType.Family);
            //Create the property for a new object (default will be set)
            IBOProp boProp;
            boProp = propDef.CreateBOProp(true);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Family, boProp.Value);
            Assert.AreEqual("Family", boProp.PropertyValueString);
            //Create the property for anexisting object (default will not be set)
            boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value);
            Assert.AreEqual("", boProp.PropertyValueString);
        }

        [Test]
        public void TestBoPropWithEnumValueChange()
        {
            PropDef propDef = new PropDef("EnumProp", typeof(ContactPersonTestBO.ContactType), PropReadWriteRule.ReadWrite, ContactPersonTestBO.ContactType.Family);
            //Create the property for anexisting object (default will not be set)
            IBOProp boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value);
            Assert.AreEqual("", boProp.PropertyValueString);
            boProp.InitialiseProp(ContactPersonTestBO.ContactType.Business);
            Assert.IsFalse(boProp.IsDirty);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.Value);
            Assert.AreEqual("Business", boProp.PropertyValueString);
            boProp.Value = ContactPersonTestBO.ContactType.Friend;
            Assert.IsTrue(boProp.IsDirty);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Friend, boProp.Value);
            Assert.AreEqual("Friend", boProp.PropertyValueString);
            boProp.RestorePropValue();
            Assert.IsFalse(boProp.IsDirty);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.Value);
            Assert.AreEqual("Business", boProp.PropertyValueString);
        }

        [Test]
        public void TestBoPropWithEnumPersistValue()
        {
            PropDef propDef = new PropDef("EnumProp", typeof(ContactPersonTestBO.ContactType), PropReadWriteRule.ReadWrite, ContactPersonTestBO.ContactType.Family);
            //Create the property for anexisting object (default will not be set)
            IBOProp boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value);
            Assert.AreEqual(null, boProp.PersistedPropertyValue);
            Assert.AreEqual("", boProp.PropertyValueString);
            Assert.AreEqual("", boProp.PersistedPropertyValueString);
            boProp.InitialiseProp(ContactPersonTestBO.ContactType.Business);
            Assert.IsFalse(boProp.IsDirty);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.Value);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.PersistedPropertyValue);
            Assert.AreEqual("Business", boProp.PersistedPropertyValueString);
            Assert.AreEqual("Business", boProp.PropertyValueString);
        }

        [Test]
        public void TestBoPropWithEnumPersistValueFromString()
        {
            PropDef propDef = new PropDef("EnumProp", typeof(ContactPersonTestBO.ContactType), PropReadWriteRule.ReadWrite, ContactPersonTestBO.ContactType.Family);
            //Create the property for anexisting object (default will not be set)
            IBOProp boProp = propDef.CreateBOProp(false);
            Assert.AreEqual(null, boProp.Value);
            Assert.AreEqual(null, boProp.PersistedPropertyValue);
            Assert.AreEqual("", boProp.PropertyValueString);
            Assert.AreEqual("", boProp.PersistedPropertyValueString);
            boProp.InitialiseProp("Business");
            Assert.IsFalse(boProp.IsDirty);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.Value);
            Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.PersistedPropertyValue);
            Assert.AreEqual("Business", boProp.PersistedPropertyValueString);
            Assert.AreEqual("Business", boProp.PropertyValueString);
        }

        #endregion //Tests for Enum type Bo Props

        #region Tests for CustomProperty type Bo Props

        //private static PropDef CreateCustomPropertyPropDef(object intialValue)
        //{
        //    return new PropDef("CustomPropertyProp", typeof(MyCustomProperty),
        //                       PropReadWriteRule.ReadWrite, intialValue);
        //}

        //private class MyCustomProperty : CustomProperty
        //{
        //    private object _value;

        //    public MyCustomProperty(object value, bool isLoading)
        //        : base(value, isLoading)
        //    {
        //        _value = value;
        //    }

        //    public override object GetPersistValue()
        //    {
        //        return _value;
        //    }
        //}

        //[Test]
        //public void TestCustomPropertyBoProp_Create_New_WithNullDefault()
        //{
        //    //-------------Setup Test Pack ------------------
        //    object intialValue = null;
        //    PropDef propDef = CreateCustomPropertyPropDef(intialValue);

        //    //-------------Test Pre-conditions --------------

        //    //-------------Execute test ---------------------
        //    //Create the property for a new object (default will be set)
        //    BOProp boProp = propDef.CreateBOProp(true);

        //    //-------------Test Result ----------------------
        //    Assert.IsInstanceOf(typeof(MyCustomProperty), boProp.Value);
        //    MyCustomProperty myCustomProperty = (MyCustomProperty)boProp.Value;
        //    Assert.AreEqual(intialValue, myCustomProperty.GetPersistValue());
        //}

        //[Test]
        //public void TestCustomPropertyBoProp_Create_New_WithDefault()
        //{
        //    //-------------Setup Test Pack ------------------
        //    object intialValue = "TestValue";
        //    PropDef propDef = CreateCustomPropertyPropDef(intialValue);

        //    //-------------Test Pre-conditions --------------

        //    //-------------Execute test ---------------------
        //    //Create the property for a new object (default will be set)
        //    BOProp boProp = propDef.CreateBOProp(true);

        //    //-------------Test Result ----------------------
        //    Assert.IsInstanceOf(typeof(MyCustomProperty), boProp.Value);
        //    MyCustomProperty myCustomProperty = (MyCustomProperty)boProp.Value;
        //    Assert.AreEqual(intialValue, myCustomProperty.GetPersistValue());
        //}

        //[Test]
        //public void TestCustomPropertyBoProp_Create_Existing()
        //{
        //    //-------------Setup Test Pack ------------------
        //    object intialValue = "TestValue";
        //    PropDef propDef = CreateCustomPropertyPropDef(intialValue);

        //    //-------------Test Pre-conditions --------------

        //    //-------------Execute test ---------------------
        //    //Create the property for an existing object (default will not be set)
        //    BOProp boProp = propDef.CreateBOProp(false);

        //    //-------------Test Result ----------------------
        //    Assert.IsNull(boProp.Value);
        //    Assert.AreEqual("", boProp.PropertyValueString);
        //}

        //[Test]
        //public void TestCustomProperty_ValueChange()
        //{
        //    //-------------Setup Test Pack ------------------
        //    PropDef propDef = CreateCustomPropertyPropDef(null);
        //    //Create the property for an existing object (default will not be set)
        //    BOProp boProp = propDef.CreateBOProp(false);
        //    string testvalue = "TestValue";
        //    boProp.InitialiseProp(testvalue);
        //    MyCustomProperty myNewCustomProperty = new MyCustomProperty(null, false);

        //    //-------------Test Pre-conditions --------------
        //    Assert.IsFalse(boProp.IsDirty);
        //    Assert.IsInstanceOf(typeof(MyCustomProperty), boProp.Value);
        //    MyCustomProperty myCustomProperty = (MyCustomProperty)boProp.Value;
        //    Assert.AreEqual(testvalue, myCustomProperty.GetPersistValue());

        //    //-------------Execute test ---------------------


        //    //-------------Test Result ----------------------
        //}

        //[Test]
        //public void TestBoPropWithCustomProperty_ValueChange()
        //{
        //    Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.Value);
        //    Assert.AreEqual("Business", boProp.PropertyValueString);
        //    boProp.Value = ContactPersonTestBO.ContactType.Friend;
        //    Assert.IsTrue(boProp.IsDirty);
        //    Assert.AreEqual(ContactPersonTestBO.ContactType.Friend, boProp.Value);
        //    Assert.AreEqual("Friend", boProp.PropertyValueString);
        //    boProp.RestorePropValue();
        //    Assert.IsFalse(boProp.IsDirty);
        //    Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.Value);
        //    Assert.AreEqual("Business", boProp.PropertyValueString);
        //}

        //[Test]
        //public void TestBoPropWithCustomProperty_PersistValue()
        //{
        //    PropDef propDef = new PropDef("EnumProp", typeof(ContactPersonTestBO.ContactType), PropReadWriteRule.ReadWrite, ContactPersonTestBO.ContactType.Family);
        //    //Create the property for anexisting object (default will not be set)
        //    BOProp boProp = propDef.CreateBOProp(false);
        //    Assert.AreEqual(null, boProp.Value);
        //    Assert.AreEqual(null, boProp.PersistedPropertyValue);
        //    Assert.AreEqual("", boProp.PropertyValueString);
        //    Assert.AreEqual("", boProp.PersistedPropertyValueString);
        //    boProp.InitialiseProp(ContactPersonTestBO.ContactType.Business);
        //    Assert.IsFalse(boProp.IsDirty);
        //    Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.Value);
        //    Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.PersistedPropertyValue);
        //    Assert.AreEqual("Business", boProp.PersistedPropertyValueString);
        //    Assert.AreEqual("Business", boProp.PropertyValueString);
        //}

        //[Test]
        //public void TestBoPropWithCustomProperty_PersistValueFromString()
        //{
        //    PropDef propDef = new PropDef("EnumProp", typeof(ContactPersonTestBO.ContactType), PropReadWriteRule.ReadWrite, ContactPersonTestBO.ContactType.Family);
        //    //Create the property for anexisting object (default will not be set)
        //    BOProp boProp = propDef.CreateBOProp(false);
        //    Assert.AreEqual(null, boProp.Value);
        //    Assert.AreEqual(null, boProp.PersistedPropertyValue);
        //    Assert.AreEqual("", boProp.PropertyValueString);
        //    Assert.AreEqual("", boProp.PersistedPropertyValueString);
        //    boProp.InitialiseProp("Business");
        //    Assert.IsFalse(boProp.IsDirty);
        //    Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.Value);
        //    Assert.AreEqual(ContactPersonTestBO.ContactType.Business, boProp.PersistedPropertyValue);
        //    Assert.AreEqual("Business", boProp.PersistedPropertyValueString);
        //    Assert.AreEqual("Business", boProp.PropertyValueString);
        //}

        #endregion //Tests for CustomProperty type Bo Props

        [Test]
        public void Test_SetPropertyToNewValueAndThenToOrigValue_PropNotDirty()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropPropDef();
            IBOProp boProp = propDef.CreateBOProp(true);
            const string origValue = "OrigValue";
            boProp.InitialiseProp(origValue);
            boProp.Value = "newValue";
            //---------------Assert Precondition----------------
            Assert.AreEqual(origValue, boProp.PersistedPropertyValue );
            Assert.IsTrue(boProp.IsDirty);
            //---------------Execute Test ----------------------
            boProp.Value = origValue;
            //---------------Test Result -----------------------
            Assert.AreEqual(origValue, boProp.PersistedPropertyValue);
            Assert.AreEqual(origValue, boProp.Value);
            Assert.IsFalse(boProp.IsDirty);
        }

        private static PropDef CreateTestPropPropDef()
        {
            return new PropDef("TestProp", "System", "String",
                               PropReadWriteRule.ReadWrite, null, null, true, false);
        }

        private static PropDef CreateTestPropDateTimePropDef()
        {
            return new PropDef("TestProp", typeof(DateTime), PropReadWriteRule.ReadWrite, null);
        }

        [Test]
        public void Test_BusinessObject_SetAndGet()
        {
            //---------------Set up test pack-------------------
            IPropDef propDef = CreateTestPropPropDef();
            BOProp boProp = new BOProp(propDef);
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            //---------------Assert Precondition----------------
            Assert.IsNull(boProp.BusinessObject);
            //---------------Execute Test ----------------------
            boProp.BusinessObject = bo;
            //---------------Test Result -----------------------
            Assert.AreSame(bo, boProp.BusinessObject);
        }

        [Test]
        public void Test_BusinessObject_WhenSetAgain_ShouldThrowError()
        {
            //---------------Set up test pack-------------------
            IPropDef propDef = CreateTestPropPropDef();
            BOProp boProp = new BOProp(propDef);
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            boProp.BusinessObject = bo;
            //---------------Assert Precondition----------------
            Assert.AreSame(bo, boProp.BusinessObject);
            //---------------Execute Test ----------------------
            try
            {
                boProp.BusinessObject = new MyBO();
                Assert.Fail("Expected to throw a HabaneroDeveloperException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroDeveloperException ex)
            {
                StringAssert.Contains("Once a BOProp has been assigned to a BusinessObject it cannot be assigned to another BusinessObject.", ex.DeveloperMessage);
            }
        }

        [Test]
        public void Test_BusinessObject_WhenSetAgainToSameBo_ShouldNotThrowError()
        {
            //---------------Set up test pack-------------------
            IPropDef propDef = CreateTestPropPropDef();
            BOProp boProp = new BOProp(propDef);
            ClassDef.ClassDefs.Clear();
            MyBO.LoadDefaultClassDef();
            MyBO bo = new MyBO();
            boProp.BusinessObject = bo;
            //---------------Assert Precondition----------------
            Assert.AreSame(bo, boProp.BusinessObject);
            //---------------Execute Test ----------------------
            Exception exception = null;
            try
            {
                boProp.BusinessObject = bo;
            } catch(Exception ex)
            {
                exception = ex;
            }
            //---------------Test Result -----------------------
            Assert.IsNull(exception);
            Assert.AreSame(bo, boProp.BusinessObject);
        }

        [Test]
        public void Test_PropertyValueToDisplay_WhenDateTimeToday_ShouldReturnResolvedValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropDateTimePropDef();
            IBOProp boProp = propDef.CreateBOProp(true);
            DateTimeToday dateTimeToday = new DateTimeToday();
            boProp.Value = dateTimeToday;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object displayValue = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.IsNotNull(displayValue);
            Assert.AreNotSame(dateTimeToday, displayValue);
            Assert.AreEqual(dateTimeToday.ResolveToValue(), displayValue);
        }

        [Test]
        public void Test_Value_WhenDateTimeToday_ShouldReturnResolvedValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropDateTimePropDef();
            IBOProp boProp = propDef.CreateBOProp(true);
            DateTimeToday dateTimeToday = new DateTimeToday();
            boProp.Value = dateTimeToday;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object value = boProp.Value;
            //---------------Test Result -----------------------
            Assert.IsNotNull(value);
            Assert.AreNotSame(dateTimeToday, value);
        }

        [Test]
        public void Test_PropertyValueToDisplay_WhenDateTimeNow_ShouldReturnResolvedValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropDateTimePropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            DateTimeNow dateTimeNow = new DateTimeNow();
            boProp.Value = dateTimeNow;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object displayValue = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.IsNotNull(displayValue);
            Assert.AreNotSame(dateTimeNow, displayValue);
        }

        [Test]
        public void Test_Value_WhenDateTimeNow_ShouldReturnResolvedValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropDateTimePropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            DateTimeNow dateTimeNow = new DateTimeNow();
            boProp.Value = dateTimeNow;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object value = boProp.Value;
            //---------------Test Result -----------------------
            Assert.IsNotNull(value);
            Assert.AreNotSame(dateTimeNow, value);
        }

        [Test]
        public void Test_PropertyValueToDisplay_WhenDateTimeUtcNow_ShouldReturnResolvedValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropDateTimePropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            boProp.Value = dateTimeUtcNow;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object displayValue = boProp.PropertyValueToDisplay;
            //---------------Test Result -----------------------
            Assert.IsNotNull(displayValue);
            Assert.AreNotSame(dateTimeUtcNow, displayValue);
        }

        [Test]
        public void Test_Value_WhenDateTimeUtcNow_ShouldReturnResolvedValue()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropDateTimePropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            DateTimeUtcNow dateTimeUtcNow = new DateTimeUtcNow();
            boProp.Value = dateTimeUtcNow;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object value = boProp.Value;
            //---------------Test Result -----------------------
            Assert.IsNotNull(value);
            Assert.AreNotSame(dateTimeUtcNow, value);
        }

        [Test]
        public void Test_InitialiseProp_ReturnsFalseIfPropValueNotChanged()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropPropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            //---------------Execute Test ----------------------
            bool propValueChanged = boProp.InitialiseProp(null);
            //---------------Test Result -----------------------
            Assert.IsFalse(propValueChanged);
        }

        [Test]
        public void Test_InitialiseProp_ReturnsTrueIfPropValueHasChanged()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropPropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            //---------------Execute Test ----------------------
            bool propValueChanged = boProp.InitialiseProp("NewValue");
            //---------------Test Result -----------------------
            Assert.IsTrue(propValueChanged);
        }

        [Test]
        public void Test_InitialiseProp_Again_WhenAlreadyInvalid_AndNewInvalidValue_ShouldResetInvalidState()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropPropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            const object invalidValue = null;
            boProp.Value = invalidValue;
            boProp.Validate();
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.Compulsory, "Prop should be compulsory");
            Assert.IsFalse(boProp.IsValid, "Prop should not be valid");
            Assert.IsFalse(String.IsNullOrEmpty(boProp.InvalidReason), "Prop should have an invalid reason");
            //---------------Execute Test ----------------------
            bool propValueChanged = boProp.InitialiseProp(invalidValue);
            //---------------Test Result -----------------------
            Assert.IsFalse(propValueChanged);
            Assert.IsTrue(boProp.IsValid, "Prop should be valid until revalidated");
            Assert.IsTrue(String.IsNullOrEmpty(boProp.InvalidReason), "Prop should not have an invalid message until revalidated");
        }

        [Test]
        public void Test_InitialiseProp_Again_WhenAlreadyInvalid_AndNewValidValue_ShouldResetInvalidState()
        {
            //---------------Set up test pack-------------------
            PropDef propDef = CreateTestPropPropDef(); ;
            IBOProp boProp = propDef.CreateBOProp(true);
            const object invalidValue = null;
            boProp.Value = invalidValue;
            boProp.Validate();
            //---------------Assert Precondition----------------
            Assert.IsTrue(propDef.Compulsory, "Prop should be compulsory");
            Assert.IsFalse(boProp.IsValid, "Prop should not be valid");
            Assert.IsFalse(String.IsNullOrEmpty(boProp.InvalidReason), "Prop should have an invalid reason");
            //---------------Execute Test ----------------------
            const string validValue = "valid Value";
            bool propValueChanged = boProp.InitialiseProp(validValue);
            //---------------Test Result -----------------------
            Assert.IsTrue(propValueChanged);
            Assert.IsTrue(boProp.IsValid, "Prop should be valid until revalidated");
            Assert.IsTrue(String.IsNullOrEmpty(boProp.InvalidReason), "Prop should not have an invalid message until revalidated");
        }

        [Test]
        public void Test_PropValueEquals_WhenSame_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            object x = 1;
            object y = x;
            BOProp prop = new BOPropSpy {Value = x};
            //---------------Assert Precondition----------------
            Assert.AreSame(x, y);
            Assert.AreSame(prop.Value, y);
            //---------------Execute Test ----------------------
            bool currentValueEquals = prop.CurrentValueEquals(y);
            //---------------Test Result -----------------------
            Assert.IsTrue(currentValueEquals);
        }


        [Test]
        public void Test_PropValueEquals_WhenEqualNumber_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            int? x = 1;
            int? y = 1;
            BOPropSpy prop = new BOPropSpy { Value = x };
            //---------------Assert Precondition----------------
            Assert.AreEqual(x, y);
            Assert.AreEqual(prop.Value, y);
            //---------------Execute Test ----------------------
            bool currentValueEquals = prop.CurrentValueEquals(y);
            //---------------Test Result -----------------------
            Assert.IsTrue(currentValueEquals);
        }
        [Test]
        public void Test_PropValueEquals_WhenEqualNumberAndNullable_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            int? x = 1;
            int y = 1;
            BOPropSpy prop = new BOPropSpy { Value = x };
            //---------------Assert Precondition----------------
            Assert.AreEqual(x, y);
            Assert.AreEqual(prop.Value, y);
            //---------------Execute Test ----------------------
            bool currentValueEquals = prop.CurrentValueEquals(y);
            //---------------Test Result -----------------------
            Assert.IsTrue(currentValueEquals);
        }

        [Test]
        public void Test_PropValueEquals_WhenEqualObject_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            int? x = 1;
            object y = 1;
            BOPropSpy prop = new BOPropSpy { Value = x };
            //---------------Assert Precondition----------------
            Assert.AreEqual(x, y);
            Assert.AreEqual(prop.Value, y);
            //---------------Execute Test ----------------------
            bool currentValueEquals = prop.CurrentValueEquals(y);
            //---------------Test Result -----------------------
            Assert.IsTrue(currentValueEquals);
        }

        [Test]
        public void Test_PropValueEquals_WhenBothNull_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            int? x = null;
            int? y = null;
            BOPropSpy prop = new BOPropSpy { Value = x };
            //---------------Assert Precondition----------------
            Assert.AreEqual(x, y);
            Assert.AreEqual(prop.Value, y);
            //---------------Execute Test ----------------------
            bool currentValueEquals = prop.CurrentValueEquals(y);
            //---------------Test Result -----------------------
            Assert.IsTrue(currentValueEquals);
        }

        [TestCase(1, 1, true, "")]
        [TestCase(1, 1, true, "")]
        [TestCase(null, 1, false, "")]
        [TestCase(3, null, false, "")]
        [TestCase(null, null, true, "")]
        [TestCase(1, "1", false, "This code does not do any parsing i.e. the '1' is not parsed to be an int and hence this will return false")]
        public void Test_CurrentValueEquals(object x, object y, bool areEqual, string message)
        {
            //---------------Set up test pack-------------------
            IBOProp prop = new BOPropSpy { Value = x };
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool currentValueEquals = prop.CurrentValueEquals(y);
            //---------------Test Result -----------------------
            string expectedMessage 
                = string.Format("x : '{0}' - y : '{1}' CurrentValueEquals Should be : {2}"
                    , x, y, areEqual) + Environment.NewLine + message;
            Assert.AreEqual(areEqual, currentValueEquals, expectedMessage);
        }

        [Test]
        public void Test_Construct_WhenCustomType_WithDefault_ShouldSetPropValue()
        {
            //---------------Set up test pack-------------------
            var propDef = new PropDef("Name", typeof(EmailAddressAsCustomProperty), PropReadWriteRule.ReadWrite, "DD", null, false, false);
            const string expectedValue = "xxxx.yyyy@ccc.aa.zz";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var prop = new BOProp(propDef, expectedValue);
            var value = prop.Value;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<EmailAddressAsCustomProperty>(prop.Value);
            Assert.AreEqual(expectedValue, value.ToString());
        }
        [Test]
        public void Test_SetValue_WhenCustomType_ShouldSetValue()
        {
            //---------------Set up test pack-------------------
            var propDef = new PropDef("Name", typeof(EmailAddressAsCustomProperty), PropReadWriteRule.ReadWrite, "DD", null, false, false);
            const string expectedValue = "xxxx.yyyy@ccc.aa.zz";
            var prop = new BOProp(propDef, null);
            //---------------Assert Precondition----------------
            Assert.IsNull(prop.Value);
            //---------------Execute Test ----------------------
            prop.Value = expectedValue;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<EmailAddressAsCustomProperty>(prop.Value);
            Assert.AreEqual(expectedValue, prop.Value.ToString());
        }

        [Test]
        public void Test_ConstructProp_WithDefaultValueForCustomType_ShouldConstructWithCorrectValue()
        {
            //---------------Set up test pack-------------------
            const string expectedValue = "xxxx.yyyy@ccc.aa.zz";
            var propDef = new PropDef("Name", typeof(EmailAddressAsCustomProperty), PropReadWriteRule.ReadWrite, "DD", expectedValue, false, false);
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedValue, propDef.DefaultValue.ToString());
            Assert.AreEqual(expectedValue, propDef.DefaultValueString);
            //---------------Execute Test ----------------------
            var prop = propDef.CreateBOProp(true);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<EmailAddressAsCustomProperty>(prop.Value);
            Assert.AreEqual(expectedValue, prop.Value.ToString());
        }


        [Test]
        public void Test_Cunstruct_WhenCustomTypeWithATypeConverter_WithDefault_ShouldSetPropValue()
        {
            //---------------Set up test pack-------------------
            var propDef = new PropDef("Name", typeof(EmailAddressWithTypeConverter), PropReadWriteRule.ReadWrite, "DD", null, false, false);
            const string expectedValue = "xxxx.yyyy@ccc.aa.zz";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var prop = new BOProp(propDef, expectedValue);
            var value = prop.Value;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<EmailAddressWithTypeConverter>(prop.Value);
            Assert.AreEqual(expectedValue, value.ToString());
        }
        [Test]
        public void Test_SetValue_WhenCustomTypeWithATypeConverter()
        {
            //---------------Set up test pack-------------------
            var propDef = new PropDef("Name", typeof(EmailAddressWithTypeConverter), PropReadWriteRule.ReadWrite, "DD", null, false, false);
            const string expectedValue = "xxxx.yyyy@ccc.aa.zz";
            var prop = new BOProp(propDef, null);
            //---------------Assert Precondition----------------
            Assert.IsNull(prop.Value);
            //---------------Execute Test ----------------------
            prop.Value = expectedValue;
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<EmailAddressWithTypeConverter>(prop.Value);
            Assert.AreEqual(expectedValue, prop.Value.ToString());
        }

        [Test]
        public void Test_ConstructProp_ForCustomTypeWithATypeConverter_WithDefaultValue_ShouldConstructWithCorrectValue()
        {
            //---------------Set up test pack-------------------
            const string expectedValue = "xxxx.yyyy@ccc.aa.zz";
            var propDef = new PropDef("Name", typeof(EmailAddressWithTypeConverter), PropReadWriteRule.ReadWrite, "DD", expectedValue, false, false);

            //---------------Assert Precondition----------------
            Assert.AreEqual(expectedValue, propDef.DefaultValue.ToString());
            Assert.AreEqual(expectedValue, propDef.DefaultValueString);
            //---------------Execute Test ----------------------
            var prop = propDef.CreateBOProp(true);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<EmailAddressWithTypeConverter>(prop.Value);
            Assert.AreEqual(expectedValue, prop.Value.ToString());
        }
        [Test]
        public void Test_ConstructProp_ForCustomTypeWithATypeConverter_WithDefaultValueString_ShouldConstructWithCorrectValue()
        {
            //---------------Set up test pack-------------------
            const string expectedValue = "xxxx.yyyy@ccc.aa.zz";
            var type = typeof(EmailAddressWithTypeConverter);
            var propDef = new PropDef("Name", type.Assembly.FullName, type.Name, PropReadWriteRule.ReadWrite, "DD", expectedValue, false, false);
            //---------------Assert Precondition----------------
            Assert.IsInstanceOf<EmailAddressWithTypeConverter>(propDef.DefaultValue);
            Assert.AreEqual(expectedValue, propDef.DefaultValue.ToString());
            Assert.AreEqual(expectedValue, propDef.DefaultValueString);
            //---------------Execute Test ----------------------
            var prop = propDef.CreateBOProp(true);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<EmailAddressWithTypeConverter>(prop.Value);
            Assert.AreEqual(expectedValue, prop.Value.ToString());
        }
        // ReSharper restore InconsistentNaming
    }



    

}
