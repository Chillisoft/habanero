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
        private PropDef mPropDef;
        private BOProp mProp;

        [SetUp]
        public void init()
        {
            mPropDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            mProp = mPropDef.CreateBOProp(false);
        }

        [Test]
        public void TestSetBOPropValue()
        {
            mProp.Value = "Prop Value";
            Assert.AreEqual("Prop Value", mProp.Value);
        }

        [Test]
        public void TestRestorePropValue()
        {
            mProp.InitialiseProp("OrigionalValue");
            mProp.Value = "Prop New Value";
            Assert.AreEqual("Prop New Value", mProp.Value);
            Assert.IsTrue(mProp.IsDirty);
            Assert.IsTrue(mProp.isValid);
            mProp.RestorePropValue();
            Assert.AreEqual("OrigionalValue", mProp.Value);
            Assert.IsFalse(mProp.IsDirty);
            Assert.IsTrue(mProp.isValid);
        }

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
            mProp.InitialiseProp("OrigionalValue");
            mProp.Value = "Prop New Value";
            Assert.AreEqual("Prop New Value", mProp.Value);
            mProp.BackupPropValue();
            Assert.AreEqual("Prop New Value", mProp.Value);
            Assert.IsFalse(mProp.IsDirty);
            Assert.IsTrue(mProp.isValid);
        }

        [Test]
        //Test that the proprety is not being set to dirty when the 
        // value is set but has not changed.
        public void TestDirtyProp()
        {
            mProp.InitialiseProp("OrigionalValue");
            mProp.Value = "OrigionalValue";
            Assert.IsFalse(mProp.IsDirty);
            Assert.IsTrue(mProp.isValid);
        }

        [Test]
        //Test persisted property value is returned correctly.
        public void TestPersistedPropValue()
        {
            mProp.InitialiseProp("OrigionalValue");
            mProp.Value = "New Value";
            Assert.IsTrue(mProp.IsDirty);
            Assert.AreEqual("OrigionalValue", mProp.PersistedPropertyValue);
            Assert.AreEqual("PropName = 'New Value'", mProp.DatabaseNameFieldNameValuePair(null));
            Assert.AreEqual("PropName = 'OrigionalValue'", mProp.PersistedDatabaseNameFieldNameValuePair(null));
        }

        [Test]
        //Test DirtyXML.
        public void TestDirtyXml()
        {
            mProp.InitialiseProp("OrigionalValue");
            mProp.Value = "New Value";
            Assert.IsTrue(mProp.IsDirty);
            string dirtyXml = "<" + mProp.PropertyName + "><PreviousValue>OrigionalValue" +
                              "</PreviousValue><NewValue>New Value</NewValue></" +
                              mProp.PropertyName + ">";
            Assert.AreEqual(dirtyXml, mProp.DirtyXml);
        }
    }

}
