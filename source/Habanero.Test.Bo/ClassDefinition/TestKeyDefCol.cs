//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO.ClassDefinition
{
    [TestFixture]
    public class TestKeyDefCol
    {
        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestAddDuplicationException()
        {
            KeyDef keyDef = new KeyDef();
            KeyDefCol col = new KeyDefCol();
            col.Add(keyDef);
            col.Add(keyDef);
        }

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

        [Test, ExpectedException(typeof(ArgumentException))]
        public void TestThisIndexerException()
        {
            KeyDefColInheritor col = new KeyDefColInheritor();
            col.GetThis("mykey");
        }

        // Grants access to protected methods
        private class KeyDefColInheritor : KeyDefCol
        {
            public void CallRemove(KeyDef keyDef)
            {
                Remove(keyDef);
            }

            public KeyDef GetThis(string keyName)
            {
                return this[keyName];
            }
        }
    }
}