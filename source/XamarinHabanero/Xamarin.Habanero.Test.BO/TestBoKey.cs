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
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBoKey
    {
        private BOPropCol _boPropCol1;
        private KeyDef _keyDef1;

        private BOPropCol _boPropCol2;
        private KeyDef _keyDef2;

        private bool _updatedEventHandled;

        [SetUp]
        public void Setup()
        {
            ClassDef.ClassDefs.Clear();
        	_boPropCol1 = new BOPropCol();
        	_keyDef1 = new KeyDef();
        	_boPropCol2 = new BOPropCol();
        	_keyDef2 = new KeyDef();

            //Props for KeyDef 1
            PropDef lPropDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadWrite, null);
            _boPropCol1.Add(lPropDef.CreateBOProp(false));
            _keyDef1.Add(lPropDef);

            lPropDef = new PropDef("PropName1", typeof(string), PropReadWriteRule.ReadWrite, null);
            _boPropCol1.Add(lPropDef.CreateBOProp(false));
            _keyDef1.Add(lPropDef);

            //Props for KeyDef 2
            lPropDef = new PropDef("PropName1", typeof(string), PropReadWriteRule.ReadWrite, null);
            _boPropCol2.Add(lPropDef.CreateBOProp(false));
            _keyDef2.Add(lPropDef);

            lPropDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadWrite, null);
            _boPropCol2.Add(lPropDef.CreateBOProp(false));
            _keyDef2.Add(lPropDef);
        }

        [Test]
        public void TestBOKeyEqual()
        {
            //Set values for Key1
            BOKey lBOKey1 = (BOKey) _keyDef1.CreateBOKey(_boPropCol1);
            IBOProp lProp = _boPropCol1["PropName"];
            lProp.Value = "Prop Value";

            lProp = _boPropCol1["PropName1"];
            lProp.Value = "Value 2";

            //Set values for Key2
            BOKey lBOKey2 = (BOKey) _keyDef2.CreateBOKey(_boPropCol2);
            lProp = _boPropCol2["PropName"];
            lProp.Value = "Prop Value";

            lProp = _boPropCol2["PropName1"];
            lProp.Value = "Value 2";

            //Assert.AreEqual(lBOKey1, lBOKey2);
            Assert.IsTrue(lBOKey1 == lBOKey2);

            Assert.AreEqual(lBOKey1.GetHashCode(), lBOKey2.GetHashCode());
        }

        [Test]
        public void TestSortedValues()
        {
            BOKey lBOKey1 = (BOKey) _keyDef1.CreateBOKey(_boPropCol2);
            IBOProp lProp = _boPropCol2["PropName"];
            Assert.AreSame(lProp, lBOKey1.SortedValues[0]);
        }
        [Test]
        public void TestIndexerPropertyNotFound()
        {
            try
            {
                IBOKey boKey = _keyDef1.CreateBOKey(_boPropCol1);
                IBOProp prop = boKey["invalidpropname"];
                Assert.Fail("Expected to throw an InvalidPropertyNameException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                StringAssert.Contains("invalidpropname", ex.Message);
            }
        }

        [Test]
        public void TestIndexerIntegerOutOfRange()
        {
            try
            {
                IBOKey boKey = _keyDef1.CreateBOKey(_boPropCol1);
                IBOProp prop = boKey[2];

                Assert.Fail("Expected to throw an IndexOutOfRangeException");
            }
                //---------------Test Result -----------------------
            catch (IndexOutOfRangeException ex)
            {
                StringAssert.Contains("the collection does not contain that many items", ex.Message);
            }
        }

        [Test]
        public void TestAddNullBOProp()
        {
            try
            {
                IBOKey boKey = _keyDef1.CreateBOKey(_boPropCol1);
                boKey.Add(null);

                Assert.Fail("Expected to throw an HabaneroArgumentException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains("The argument 'boProp' is not valid. boProp cannot be null", ex.Message);
            }
        }

        [Test]
        public void TestAddDuplicateBOProp()
        {
            try
            {
                IBOKey boKey = _keyDef1.CreateBOKey(_boPropCol1);
                boKey.Add(boKey["PropName"]);

                Assert.Fail("Expected to throw an InvalidPropertyException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyException ex)
            {
                StringAssert.Contains("already exists in the key collection", ex.Message);
            }
        }

        [Test]
        public void TestEquality()
        {
            BOKey boKey = (BOKey) _keyDef1.CreateBOKey(_boPropCol1);

            // Test when property count is different
            KeyDef keyDef = new KeyDef();
            BOKey otherKey = new BOKey(keyDef);
            Assert.IsFalse(boKey == otherKey);

            // Same property count, but different prop names
            PropDef propDef1 = new PropDef("PropName5", typeof(string), PropReadWriteRule.ReadOnly, null);
            PropDef propDef2 = new PropDef("PropName6", typeof(string), PropReadWriteRule.ReadOnly, null);
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef1.CreateBOProp(false));
            propCol.Add(propDef2.CreateBOProp(false));
            keyDef.Add(propDef1);
            keyDef.Add(propDef2);
            otherKey = (BOKey) keyDef.CreateBOKey(propCol);
            Assert.IsFalse(boKey == otherKey);

            // Same props but different values (with one null)
            otherKey = (BOKey) _keyDef1.CreateBOKey(_boPropCol2);
            otherKey["PropName"].Value = "blah";
            Assert.IsFalse(boKey == otherKey);

            // Same props but different values (neither are null)
            otherKey = (BOKey) _keyDef1.CreateBOKey(_boPropCol2);
            boKey["PropName"].Value = "diblah";
            Assert.IsFalse(boKey == otherKey);
            Assert.IsFalse(boKey.Equals(otherKey));

            // False when different type of object
            Assert.IsFalse(boKey.Equals(keyDef));

            // Finally, when they are equal
            boKey["PropName"].Value = "blah";
            Assert.IsTrue(boKey == otherKey);
            Assert.IsTrue(boKey.Equals(otherKey));
        }

        [Test]
        public void TestIntegerIndexer()
        {
            PropDef propDef1 = new PropDef("PropName1", typeof(string), PropReadWriteRule.ReadOnly, null);
            PropDef propDef2 = new PropDef("PropName2", typeof(string), PropReadWriteRule.ReadOnly, null);
            
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef1.CreateBOProp(false));
            propCol.Add(propDef2.CreateBOProp(false));
            
            KeyDef keyDef = new KeyDef();
            keyDef.Add(propDef1);
            keyDef.Add(propDef2);
            IBOKey boKey = keyDef.CreateBOKey(propCol);

            Assert.AreEqual(propCol["PropName1"], boKey[0]);
            Assert.AreEqual(propCol["PropName2"], boKey[1]);
        }

        [Test]
        public void Test_AsString_CurrentValue_New()
        {
            //--------------- Set up test pack ------------------
            IBOKey boKey = CreateBOKeyGuid();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            string keyAsString = boKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("ContactPersonTestBO.PropName1=", keyAsString);
        }

        [Test]
        public void Test_AsString_CurrentValue()
        {
            //--------------- Set up test pack ------------------
            IBOKey boKey = CreateBOKeyGuid();
            Guid guid = Guid.NewGuid();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            boKey[0].Value = guid;
            string keyAsString = boKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("ContactPersonTestBO.PropName1=" + guid, keyAsString);
        }

        [Test]
        public void Test_AsString_CurrentValue_TwoProps()
        {
            //--------------- Set up test pack ------------------
            IBOKey boKey = CreateBOKeyGuidAndString();
            Guid guid = Guid.NewGuid();
            string str = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            boKey[0].Value = guid;
            boKey[1].Value = str;
            string keyAsString = boKey.AsString_CurrentValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase("ContactPersonTestBO.PropName1=" + guid + ";ContactPersonTestBO.PropName2=" + str, keyAsString);
        }

        [Test]
        public void Test_AsString_PreviousValue_New()
        {
            //--------------- Set up test pack ------------------
            IBOKey boKey = CreateBOKeyGuid();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            string keyAsString = boKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(boKey.AsString_CurrentValue(), keyAsString);
        }

        [Test]
        public void Test_AsString_PreviousValue_FirstChange()
        {
            //--------------- Set up test pack ------------------
            IBOKey boKey = CreateBOKeyGuid();
            string expectedPreviousValue = boKey.AsString_CurrentValue();
            Guid guid = Guid.NewGuid();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            boKey[0].Value = guid;
            string keyAsString = boKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(expectedPreviousValue, keyAsString);
        }

        [Test]
        public void Test_AsString_PreviousValue_SecondChange()
        {
            //--------------- Set up test pack ------------------
            IBOKey boKey = CreateBOKeyGuid();
            boKey[0].Value = Guid.NewGuid();
            string expectedPreviousValue = boKey.AsString_CurrentValue();
            Guid guid = Guid.NewGuid();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            boKey[0].Value = guid;
            string keyAsString = boKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(expectedPreviousValue, keyAsString);
        }


        [Test]
        public void Test_AsString_PreviousValue_TwoPropKey()
        {
            //--------------- Set up test pack ------------------
            IBOKey boKey = CreateBOKeyGuidAndString();
            string expectedPreviousValue = boKey.AsString_CurrentValue();
            Guid guid = Guid.NewGuid();
            string str = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            boKey[0].Value = guid;
            boKey[1].Value = str;
            string keyAsString = boKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(expectedPreviousValue, keyAsString);
        }


        [Test]
        public void Test_AsString_PreviousValue_SecondChange_TwoPropKey()
        {
            //--------------- Set up test pack ------------------
            IBOKey boKey = CreateBOKeyGuidAndString();
            boKey[0].Value = Guid.NewGuid();
            boKey[1].Value = TestUtil.GetRandomString();
            string expectedPreviousValue = boKey.AsString_CurrentValue();
            Guid guid = Guid.NewGuid();
            string str = TestUtil.GetRandomString();
            //--------------- Test Preconditions ----------------

            //--------------- Execute Test ----------------------
            boKey[0].Value = guid;
            boKey[1].Value = str;
            string keyAsString = boKey.AsString_PreviousValue();
            //--------------- Test Result -----------------------
            StringAssert.AreEqualIgnoringCase(expectedPreviousValue, keyAsString);
        }



        private static IBOKey CreateBOKeyGuid()
        {
            PropDef propDef1 = new PropDef("PropName1", typeof(Guid), PropReadWriteRule.ReadWrite, null)
                                   {ClassDef = ContactPersonTestBO.LoadDefaultClassDef()};
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef1.CreateBOProp(false));
            KeyDef keyDef = new KeyDef {propDef1};
            return keyDef.CreateBOKey(propCol);
        }

        private static IBOKey CreateBOKeyGuidAndString()
        {
            PropDef propDef1 = new PropDef("PropName1", typeof(Guid), PropReadWriteRule.ReadWrite, null)
                                   {ClassDef = ContactPersonTestBO.LoadDefaultClassDef()};
            PropDef propDef2 = new PropDef("PropName2", typeof(string), PropReadWriteRule.ReadWrite, null)
                                   {ClassDef = propDef1.ClassDef};
            BOPropCol propCol = new BOPropCol();
            propCol.Add( propDef1.CreateBOProp(false));
            propCol.Add(propDef2.CreateBOProp(false));
            KeyDef keyDef = new KeyDef {propDef1, propDef2};
            return keyDef.CreateBOKey(propCol);
        }

        [Test]
        public void TestUpdatedEvent()
        {
            PropDef propDef1 = new PropDef("PropName1", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("PropName2", typeof(string), PropReadWriteRule.ReadWrite, null);

            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef1.CreateBOProp(false));
            propCol.Add(propDef2.CreateBOProp(false));

            KeyDef keyDef = new KeyDef {propDef1, propDef2};
            IBOKey boKey = keyDef.CreateBOKey(propCol);

            boKey.Updated += UpdatedEventHandler;
            propCol["PropName1"].Value = "new value";
            Assert.IsTrue(_updatedEventHandled);
        }

        void UpdatedEventHandler(object sender, BOKeyEventArgs e)
        {
            _updatedEventHandled = true;
        }

        [Test]
        public void TestHasAutoIncrementingProperty_False()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("PropName1", typeof(string), PropReadWriteRule.ReadWrite, null);
            propDef1.AutoIncrementing = false;
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef1.CreateBOProp(false));
            KeyDef keyDef = new KeyDef {propDef1};
            IBOKey boKey = keyDef.CreateBOKey(propCol);

            //---------------Assert PreConditions---------------            
            
            //---------------Execute Test ----------------------
            bool hasAutoIncrementingProperty = boKey.HasAutoIncrementingProperty;
            //---------------Test Result -----------------------

            Assert.IsFalse(hasAutoIncrementingProperty);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestHasAutoIncrementingProperty_True()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("PropName1", typeof(string), PropReadWriteRule.ReadWrite, null);
            propDef1.AutoIncrementing = true;
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef1.CreateBOProp(false));
            KeyDef keyDef = new KeyDef {propDef1};
            IBOKey boKey = keyDef.CreateBOKey(propCol);

            //---------------Assert PreConditions---------------            

            //---------------Execute Test ----------------------
            bool hasAutoIncrementingProperty = boKey.HasAutoIncrementingProperty;
            //---------------Test Result -----------------------

            Assert.IsTrue(hasAutoIncrementingProperty);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestHasAutoIncrementingProperty_TwoProps_True()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("PropName1", typeof(string), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("PropName2", typeof(string), PropReadWriteRule.ReadWrite, null);
            propDef2.AutoIncrementing = true;
            BOPropCol propCol = new BOPropCol();
            propCol.Add(propDef1.CreateBOProp(false));
            propCol.Add(propDef2.CreateBOProp(false));
            KeyDef keyDef = new KeyDef();
            keyDef.Add(propDef1);
            keyDef.Add(propDef2);
            IBOKey boKey = keyDef.CreateBOKey(propCol);

            //---------------Assert PreConditions---------------            

            //---------------Execute Test ----------------------
            bool hasAutoIncrementingProperty = boKey.HasAutoIncrementingProperty;
            //---------------Test Result -----------------------

            Assert.IsTrue(hasAutoIncrementingProperty);
            //---------------Tear Down -------------------------          
        }


   
    }
}