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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestUtilities
    {
        [Test]
        public void Test_ToArray_WhenEmptyList_ReturnsEmptyArray()
        {
            //---------------Set up test pack-------------------
            IList list = new ArrayList();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, list.Count);
            //---------------Execute Test ----------------------
            object[] returnedArray = Utilities.ToArray<object>(list);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, returnedArray.Length);
        }

        [Test]
        public void Test_ToArray_WhenSingleItem_ReturnsArrayWithSameItem()
        {
            //---------------Set up test pack-------------------
            IList list = new ArrayList();
            object item = new object();
            list.Add(item);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, list.Count);
            Assert.AreSame(item, list[0]);
            //---------------Execute Test ----------------------
            object[] returnedArray = Utilities.ToArray<object>(list);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, returnedArray.Length);
            Assert.AreSame(item, returnedArray[0]);
        }

        [Test]
        public void Test_ToArray_WhenMultipleItems_ReturnsArrayWithSameItemsInOrder()
        {
            //---------------Set up test pack-------------------
            IList list = new ArrayList();
            list.Add(new object());
            list.Add(new object());
            list.Add(new object());
            list.Add(new object());
            list.Add(new object());
            //---------------Assert Precondition----------------
            Assert.AreEqual(5, list.Count);
            //---------------Execute Test ----------------------
            object[] returnedArray = Utilities.ToArray<object>(list);
            //---------------Test Result -----------------------
            Assert.AreEqual(5, returnedArray.Length);
            Assert.AreSame(list[0], returnedArray[0]);
            Assert.AreSame(list[1], returnedArray[1]);
            Assert.AreSame(list[2], returnedArray[2]);
            Assert.AreSame(list[3], returnedArray[3]);
            Assert.AreSame(list[4], returnedArray[4]);
        }

        [Test]
        public void Test_ToArray_WhenMultipleItems_WithSpecifiedType_ReturnsArrayWithSameItemsInOrder()
        {
            //---------------Set up test pack-------------------
            IList list = new ArrayList();
            list.Add(new Hashtable());
            list.Add(new Hashtable());
            list.Add(new Hashtable());
            list.Add(new Hashtable());
            list.Add(new Hashtable());
            //---------------Assert Precondition----------------
            Assert.AreEqual(5, list.Count);
            //---------------Execute Test ----------------------
            Hashtable[] returnedArray = Utilities.ToArray<Hashtable>(list);
            //---------------Test Result -----------------------
            Assert.AreEqual(5, returnedArray.Length);
            Assert.AreSame(list[0], returnedArray[0]);
            Assert.AreSame(list[1], returnedArray[1]);
            Assert.AreSame(list[2], returnedArray[2]);
            Assert.AreSame(list[3], returnedArray[3]);
            Assert.AreSame(list[4], returnedArray[4]);
        }

        [Test]
        public void Test_ToArray_WhenSingleItem_WithSpecifiedTypeMismatch_ShouldThrowInvalidCastException()
        {
            //---------------Set up test pack-------------------
            IList list = new ArrayList();
            list.Add(new object());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, list.Count);
            //---------------Execute Test ----------------------

            try
            {
                Utilities.ToArray<Hashtable>(list);
                Assert.Fail("Expected to throw an InvalidCastException");
            }
            //---------------Test Result -----------------------
            catch (InvalidCastException ex)
            {
                StringAssert.Contains("Unable to cast object of type 'System.Object' to type 'System.Collections.Hashtable'.", ex.Message);
            }
        }

        [Test]
        public void Test_ToArray_WhenMultipleItems_WithSpecifiedTypeMismatch_ShouldThrowInvalidCastException()
        {
            //---------------Set up test pack-------------------
            IList list = new ArrayList();
            list.Add(new Hashtable());
            list.Add(new object());
            list.Add(new Hashtable());
            //---------------Assert Precondition----------------
            Assert.AreEqual(3, list.Count);
            //---------------Execute Test ----------------------

            try
            {
                Utilities.ToArray<Hashtable>(list);
                Assert.Fail("Expected to throw an InvalidCastException");
            }
            //---------------Test Result -----------------------
            catch (InvalidCastException ex)
            {
                StringAssert.Contains("Unable to cast object of type 'System.Object' to type 'System.Collections.Hashtable'.", ex.Message);
            }
        }
    }
}
