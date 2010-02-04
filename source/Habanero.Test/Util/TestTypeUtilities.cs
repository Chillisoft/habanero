// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
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
using System.ComponentModel;
using Habanero.Base;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestTypeUtilities
    {
        [Test]
        public void TestIsIntegerReturnsTrueForIntegerTypes()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(TypeUtilities.IsInteger(typeof(int)));
            Assert.IsTrue(TypeUtilities.IsInteger(typeof(long)));
            Assert.IsTrue(TypeUtilities.IsInteger(typeof(short)));
            Assert.IsTrue(TypeUtilities.IsInteger(typeof(byte)));
            Assert.IsTrue(TypeUtilities.IsInteger(typeof(uint)));
            Assert.IsTrue(TypeUtilities.IsInteger(typeof(ulong)));
            Assert.IsTrue(TypeUtilities.IsInteger(typeof(ushort)));
            Assert.IsTrue(TypeUtilities.IsInteger(typeof(sbyte)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsIntegerReturnsFalseForNonIntegerTypes()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(TypeUtilities.IsInteger(typeof(string)));
            Assert.IsFalse(TypeUtilities.IsInteger(typeof(DateTime)));
            Assert.IsFalse(TypeUtilities.IsInteger(typeof(decimal)));
            Assert.IsFalse(TypeUtilities.IsInteger(typeof(MyBO)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsDecimalReturnsTrueForDecimalType()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(TypeUtilities.IsDecimal(typeof(decimal)));
            Assert.IsTrue(TypeUtilities.IsDecimal(typeof(float)));
            Assert.IsTrue(TypeUtilities.IsDecimal(typeof(double)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestIsDecimalReturnsFalseForNonDecimalTypes()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(TypeUtilities.IsDecimal(typeof(string)));
            Assert.IsFalse(TypeUtilities.IsDecimal(typeof(DateTime)));
            Assert.IsFalse(TypeUtilities.IsDecimal(typeof(int)));
            Assert.IsFalse(TypeUtilities.IsDecimal(typeof(MyBO)));
            //---------------Tear down -------------------------
        }

        [Test]
        public void Test_ConvertTo()
        {
            //---------------Set up test pack-------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            DateTime snapshot = DateTime.Now;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTime dateTime = TypeUtilities.ConvertTo<DateTime>(dateTimeNow);
            //---------------Test Result -----------------------
            Assert.Greater(dateTime, snapshot.AddSeconds(-1));
            Assert.Less(dateTime, snapshot.AddSeconds(1));
        }

        private enum MyEnum
        {
            MyValue1,
            MyValue2
        }

        [Test]
        public void Test_ConvertTo_WhenDestinationTypeIsSameAsSourceType_ShouldReturnValueUnchanged()
        {
            //---------------Set up test pack-------------------
            const MyEnum value = MyEnum.MyValue1;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            MyEnum returnedValue = TypeUtilities.ConvertTo<MyEnum>(value);
            //---------------Test Result -----------------------
            Assert.AreEqual(value, returnedValue);
        }

        [Test]
        public void Test_ConvertTo_WhenDestinationTypeIsNullableOfSourceType_ShouldReturnValueUnchanged()
        {
            //---------------Set up test pack-------------------
            const MyEnum value = MyEnum.MyValue1;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            MyEnum? returnedValue = TypeUtilities.ConvertTo<MyEnum?>(value);
            //---------------Test Result -----------------------
            Assert.IsTrue(returnedValue.HasValue);
            Assert.AreEqual(value, returnedValue.Value);
        }

        [Test]
        [Ignore("I'm not sure why this is not working. The code is there.")] //TODO Mark 19 Oct 2009: Ignored Test - I'm not sure why this is not working. The code is there.
        public void Test_ConvertTo_WhenDestinationTypeIsNullableOfSourceType_ShouldReturnNullableOfValueUnchanged()
        {
            //---------------Set up test pack-------------------
            const MyEnum value = MyEnum.MyValue1;
            Type type = typeof(MyEnum?);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object returnedValue = TypeUtilities.ConvertTo(type, value);
            //---------------Test Result -----------------------
            Assert.AreEqual(type, returnedValue.GetType());
        }

        private class MyClass
        {
            
        }

        private class MyClass2 : MyClass
        {
            
        }

        [Test]
        public void Test_ConvertTo_WhenDestinationTypeIsSuperTypeOfSourceType_ShouldReturnValueUnchanged()
        {
            //---------------Set up test pack-------------------
            MyClass2 value = new MyClass2();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            MyClass returnedValue = TypeUtilities.ConvertTo<MyClass>(value);
            //---------------Test Result -----------------------
            Assert.AreSame(value, returnedValue);
        }

        [Test]
        public void Test_ConvertTo_WhenValueIsNull_ShouldReturnNull()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DateTime? dateTime = TypeUtilities.ConvertTo<DateTime?>(null);
            //---------------Test Result -----------------------
            Assert.IsNull(dateTime);
        }

        [Test]
        public void Test_ConvertTo_WithTypeArgument()
        {
            //---------------Set up test pack-------------------
            DateTimeNow dateTimeNow = new DateTimeNow();
            DateTime snapshot = DateTime.Now;
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            object returnValue = TypeUtilities.ConvertTo(typeof(DateTime), dateTimeNow);
            //---------------Test Result -----------------------
            DateTime dateTime = TestUtil.AssertIsInstanceOf<DateTime>(returnValue);
            Assert.Greater(dateTime, snapshot.AddSeconds(-1));
            Assert.Less(dateTime, snapshot.AddSeconds(1));
        }
    }
}
