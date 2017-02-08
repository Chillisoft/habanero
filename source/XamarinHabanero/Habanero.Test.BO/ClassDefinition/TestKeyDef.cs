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

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestKeyDef
    {
        private KeyDef mKeyDef;

        [SetUp]
        public void init()
        {
            mKeyDef = new KeyDef();
            mKeyDef.IgnoreIfNull = true;
            PropDef lPropDef = new PropDef("PropName", typeof(string), PropReadWriteRule.ReadOnly, null);
            mKeyDef.Add(lPropDef);
        }

        [Test]
        public void TestAddPropDef()
        {
            Assert.AreEqual(1, mKeyDef.Count);
        }

        [Test]
        public void TestContainsPropDef()
        {
            Assert.IsTrue(mKeyDef.Contains("PropName"));
            IPropDef lPropDef = mKeyDef["PropName"];
            Assert.AreEqual("PropName", lPropDef.PropertyName);
            Assert.IsTrue(mKeyDef.IgnoreIfNull);
        }

        [Test]
        public void TestThisInvalidPropertyNameException()
        {
            //---------------Set up test pack-------------------
            KeyDef keyDef = new KeyDef();
            //---------------Execute Test ----------------------
            try
            {
                IPropDef propDef = keyDef["wrongprop"];
                Assert.Fail("Expected to throw an InvalidPropertyNameException");
            }
                //---------------Test Result -----------------------
            catch (InvalidPropertyNameException ex)
            {
                StringAssert.Contains("no property with the name 'wrongprop' exists in the collection of properties", ex.Message);
            }
        }

        [Test]
        public void TestAddNullException()
        {
            //---------------Set up test pack-------------------
            KeyDef keyDef = new KeyDef();
            //---------------Execute Test ----------------------
            try
            {
                keyDef.Add(null);
                Assert.Fail("Expected to throw an HabaneroArgumentException");
            }
                //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains("You cannot add a null prop def to a classdef", ex.Message);
            }
        }

        [Test]
        public void TestRemove()
        {
            PropDef propDef = new PropDef("prop", typeof(String), PropReadWriteRule.ReadWrite, null);
            KeyDefInheritor keyDef = new KeyDefInheritor();

            keyDef.CallRemove(propDef);
            Assert.AreEqual(0, keyDef.Count);
            keyDef.Add(propDef);
            Assert.AreEqual(1, keyDef.Count);
            keyDef.CallRemove(propDef);
            Assert.AreEqual(0, keyDef.Count);
        }

        [Test]
        public void TestIsValid()
        {
            PropDef propDef = new PropDef("prop", typeof(String), PropReadWriteRule.ReadWrite, null);
            KeyDef keyDef = new KeyDef();
            Assert.IsFalse(keyDef.IsValid());

            keyDef.Add(propDef);
            Assert.IsTrue(keyDef.IsValid());
        }

        [Test]
        public void TestKeyName_DontBuildFromProps()
        {
            //-------------Setup Test Pack ------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);
            KeyDef keyDef = new KeyDef("bob");
            //-------------Test Pre-conditions --------------
            Assert.AreEqual("bob", keyDef.KeyName);
            //-------------Execute test ---------------------
            keyDef.Add(propDef1);
            keyDef.Add(propDef2);
            //-------------Test Result ----------------------
            Assert.AreEqual("bob", keyDef.KeyName);
        }

        [Test]
        public void TestKeyName_BuildFromProps_WhenBlank()
        {
            //-------------Setup Test Pack ------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);
            KeyDef keyDef = new KeyDef("");
            //-------------Test Pre-conditions --------------
            Assert.AreEqual("", keyDef.KeyName);
            //-------------Execute test ---------------------
            keyDef.Add(propDef1);
            keyDef.Add(propDef2);
            //-------------Test Result ----------------------
            Assert.AreEqual("prop1_prop2", keyDef.KeyName);
        }

        // Exposes protected methods for testing
        private class KeyDefInheritor : KeyDef
        {
            public void CallRemove(PropDef propDef)
            {
                Remove(propDef);
            }
        }

        [Test]
        public void TestIndexer_Int()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);
            KeyDef keyDef = new KeyDef("bob");
            keyDef.Add(propDef1);
            keyDef.Add(propDef2);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            IPropDef keyDef0 = keyDef[0];
            IPropDef keyDef1 = keyDef[1];
            //------------Test Result ----------------------
            Assert.AreSame(propDef1, keyDef0);
            Assert.AreSame(propDef2, keyDef1);
        }


        [Test]
        public void TestIndexer_Int_OutOfRange()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);
            KeyDef keyDef = new KeyDef("bob");
            keyDef.Add(propDef1);
            keyDef.Add(propDef2);
            //-------------Test Pre-conditions --------------
            //-------------Execute test ---------------------
            try
            {
                IPropDef keyDef0 = keyDef[2];
                Assert.Fail("Expected to throw an ArgumentOutOfRangeException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentOutOfRangeException ex)
            {
                StringAssert.Contains("index out of range", ex.Message);
            }
        }
    }

}
