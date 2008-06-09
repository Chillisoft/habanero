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
