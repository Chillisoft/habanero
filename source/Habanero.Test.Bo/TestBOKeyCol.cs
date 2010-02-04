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
        [Test, ExpectedException(typeof(InvalidKeyException))]
        public void TestAddDuplicates()
        {
            BOKeyCol col = new BOKeyCol();
            IBOKey boKey = new BOKey(new KeyDef());
            col.Add(boKey);
                col.Add(boKey);
                Assert.Fail("Expected to throw an InvalidKeyException");
        }

        [Test]
        public void TestIndexer()
        {
            BOKeyCol col = new BOKeyCol();
            col.Add(new BOKey(new KeyDef("anotherkey")));
            BOKey boKey = new BOKey(new KeyDef("key"));
            col.Add(boKey);
            Assert.AreEqual(boKey, col["key"]);
        }

#pragma warning disable 168
        [Test, ExpectedException(typeof(InvalidKeyException))]
        public void TestIndexerWithNonExistingKey()
        {
            BOKeyCol col = new BOKeyCol();
                IBOKey key = col["invalidkey"];

                Assert.Fail("Expected to throw an InvalidKeyException");
        }
#pragma warning restore 168
    }
}