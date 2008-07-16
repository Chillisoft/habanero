//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Collections.Generic;
using System.Text;
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
    }
}
