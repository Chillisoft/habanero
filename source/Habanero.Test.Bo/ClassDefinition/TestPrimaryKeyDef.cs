//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestPrimaryKeyDef
    {
        [Test, ExpectedException(typeof(InvalidPropertyException))]
        public void TestMultiplePropertiesForIDException()
        {
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);
            new PrimaryKeyDef {propDef1, propDef2};
        }

        [Test]
        public void TestIgnoreIfNullReturnsFalse()
        {
            PrimaryKeyDef pkDef = new PrimaryKeyDef();
            Assert.IsFalse(pkDef.IgnoreIfNull);
            pkDef.IgnoreIfNull = false;
            Assert.IsFalse(pkDef.IgnoreIfNull);
        }

        [Test, ExpectedException(typeof(InvalidKeyException))]
        public void TestSettingIgnoreIfNullTrueException()
        {
            new PrimaryKeyDef {IgnoreIfNull = true};
        }

        [Test]
        public void Test_CreatePrimaryKey_TwoPropDefs()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PropDef propDef2 = new PropDef("prop2", typeof(String), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef keyDef = new PrimaryKeyDef { IsGuidObjectID = false };
            keyDef.Add(propDef2);
            keyDef.Add(propDef1);

            //---------------Assert Precondition----------------
            Assert.AreEqual(2, keyDef.Count);
            //---------------Execute Test ----------------------
            bool isCompositeKey = keyDef.IsCompositeKey;
            //---------------Test Result -----------------------
            Assert.IsTrue(isCompositeKey);
        }
        [Test]
        public void Test_CreatePrimaryKey_OnePropDefs()
        {
            //---------------Set up test pack-------------------
            PropDef propDef1 = new PropDef("prop1", typeof(String), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef keyDef = new PrimaryKeyDef { IsGuidObjectID = false };
            keyDef.Add(propDef1);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, keyDef.Count);
            //---------------Execute Test ----------------------
            bool isCompositeKey = keyDef.IsCompositeKey;
            //---------------Test Result -----------------------
            Assert.IsFalse(isCompositeKey);

        }
        [Test]
        public void Test_ToString_ShouldReturnPropName()
        {
            //---------------Set up test pack-------------------
            const string propertyName = "prop1";
            PropDef propDef1 = new PropDef(propertyName, typeof(String), PropReadWriteRule.ReadWrite, null);
            PrimaryKeyDef keyDef = new PrimaryKeyDef { IsGuidObjectID = false };
            keyDef.Add(propDef1);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, keyDef.Count);
            //---------------Execute Test ----------------------
            var toString = keyDef.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(propertyName, toString);

        }
        [Test]
        public void Test_ToString_WhenComposite_ShouldReturnConcatenatedPropNames()
        {
            //---------------Set up test pack-------------------
            string propertyName = TestUtil.GetRandomString();
            string propName2 = TestUtil.GetRandomString();
            PrimaryKeyDef keyDef = new PrimaryKeyDef { IsGuidObjectID = false };
            keyDef.Add(new PropDef(propertyName, typeof(String), PropReadWriteRule.ReadWrite, null));
            keyDef.Add(new PropDef(propName2, typeof(String), PropReadWriteRule.ReadWrite, null));
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, keyDef.Count);
            //---------------Execute Test ----------------------
            var toString = keyDef.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(propertyName +"_" + propName2, toString);

        }
        
    }
}
