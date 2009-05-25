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
            PropDef lPropDef = mKeyDef["PropName"];
            Assert.AreEqual("PropName", lPropDef.PropertyName);
            Assert.IsTrue(mKeyDef.IgnoreIfNull);
        }

        [Test, ExpectedException(typeof(InvalidPropertyNameException))]
        public void TestThisInvalidPropertyNameException()
        {
            KeyDef keyDef = new KeyDef();
            PropDef propDef = keyDef["wrongprop"];
        }

        [Test, ExpectedException(typeof(HabaneroArgumentException))]
        public void TestAddNullException()
        {
            KeyDef keyDef = new KeyDef();
            keyDef.Add(null);
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

        // Exposes protected methods for testing
        private class KeyDefInheritor : KeyDef
        {
            public void CallRemove(PropDef propDef)
            {
                Remove(propDef);
            }
        }

    }

}