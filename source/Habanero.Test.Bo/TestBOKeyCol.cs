// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    [TestFixture]
    public class TestBOKeyCol
    {
        [Test]
        public void TestAddDuplicates()
        {
            //---------------Set up test pack-------------------
            BOKeyCol col = new BOKeyCol();
            IBOKey boKey = new BOKey(new KeyDef());
            col.Add(boKey);
            //---------------Execute Test ----------------------
            try
            {
                col.Add(boKey);
                Assert.Fail("Expected to throw an InvalidKeyException");
            }
            //---------------Test Result -----------------------
            catch (InvalidKeyException ex)
            {
                StringAssert.Contains("already exists in the collection", ex.Message);
            }
        }

        [Test]
        public void TestIndexer()
        {
            //---------------Set up test pack-------------------
            BOKeyCol col = new BOKeyCol();
            col.Add(new BOKey(new KeyDef("anotherkey")));
            BOKey boKey = new BOKey(new KeyDef("key"));
            col.Add(boKey);
            //---------------Execute Test ----------------------
            IBOKey indexedKey = col["key"];
            //---------------Test Result -----------------------
            Assert.AreSame(boKey, indexedKey);
        }

        [Test]
        public void TestIndexerWithNonExistingKey()
        {
            //---------------Set up test pack-------------------
            BOKeyCol col = new BOKeyCol();
            //---------------Execute Test ----------------------
            try
            {
                IBOKey key = col["invalidkey"];
                Assert.Fail("Expected to throw an InvalidKeyException");
            }
                //---------------Test Result -----------------------
            catch (InvalidKeyException ex)
            {
                StringAssert.Contains("does not exist in the collection of keys", ex.Message);
            }
        }
    }
}