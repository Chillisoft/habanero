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
using Habanero.BO.ClassDefinition;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestKeyDefCol
    {
        [Test]
        public void TestAddDuplicationException()
        {
            //---------------Set up test pack-------------------
            KeyDef keyDef = new KeyDef();
            KeyDefCol col = new KeyDefCol();
            col.Add(keyDef);
            //---------------Execute Test ----------------------
            try
            {
                col.Add(keyDef);
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("already exists", ex.Message);
            }
        }
/*
        [Test]
        public void Test_Add_ShouldSetPropDefsClassDef()
        {
            //---------------Set up test pack-------------------
            var keyDef = new KeyDef();
            var col = new KeyDefCol();
            var expectedClassDef = MockRepository.GenerateStub<IClassDef>();
            col.ClassDef = expectedClassDef;
            //---------------Assert Preconditions---------------
            Assert.IsNull(keyDef.ClassDef);
            //---------------Execute Test ----------------------
            col.Add(keyDef);
            //---------------Test Result -----------------------
            Assert.AreSame(expectedClassDef, keyDef.ClassDef);
        }*/


        [Test]
        public void TestRemove()
        {
            KeyDef keyDef = new KeyDef();
            KeyDefColInheritor col = new KeyDefColInheritor();
            
            col.CallRemove(keyDef);
            col.Add(keyDef);
            Assert.AreEqual(1, col.Count);
            col.CallRemove(keyDef);
            Assert.AreEqual(0, col.Count);
        }

        [Test]
        public void TestContainsKeyName()
        {
            KeyDef keyDef = new KeyDef("mykey");
            KeyDefCol col = new KeyDefCol();

            Assert.IsFalse(col.Contains("mykey"));
            col.Add(keyDef);
            Assert.IsTrue(col.Contains("mykey"));
        }

        [Test]
        public void TestThisIndexer()
        {
            KeyDef keyDef = new KeyDef("mykey");
            KeyDefColInheritor col = new KeyDefColInheritor();
            col.Add(keyDef);
            Assert.AreEqual(keyDef, col.GetThis("mykey"));
        }

        [Test]
        public void TestThisIndexerException()
        {
            //---------------Set up test pack-------------------
            KeyDefColInheritor col = new KeyDefColInheritor();
            //---------------Execute Test ----------------------
            try
            {
                col.GetThis("mykey");
                Assert.Fail("Expected to throw an ArgumentException");
            }
                //---------------Test Result -----------------------
            catch (ArgumentException ex)
            {
                StringAssert.Contains("does not exist in the collection of key definitions", ex.Message);
            }
        }

        // Grants access to protected methods
        private class KeyDefColInheritor : KeyDefCol
        {
            public void CallRemove(IKeyDef keyDef)
            {
                Remove(keyDef);
            }

            public IKeyDef GetThis(string keyName)
            {
                return this[keyName];
            }
        }
    }
}