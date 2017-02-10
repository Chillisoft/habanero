#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    /// <summary>
    /// Summary description for TestEditableDataSetProvider.
    /// </summary>
    [TestFixture]
    public class TestEnumerableExtensions 
    {
        [Test]
        public void IsEqualTo()
        {
            //---------------Set up test pack-------------------
            var myInts1 = new[] { 1, 3, 5, 10 };
            var myInts2 = new[] { 1, 3, 5, 10 };
            //---------------Execute Test ----------------------
            var areEqual = myInts1.IsEqualTo(myInts2);
            //---------------Test Result -----------------------
            Assert.IsTrue(areEqual);
        }        
        [Test]
        public void IsEqualTo_WhenDifferentLengths1_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var myInts1 = new[] { 1, 3, 5, 10 };
            var myInts2 = new[] { 1, 3, 5 };
            //---------------Execute Test ----------------------
            var areEqual = myInts1.IsEqualTo(myInts2);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEqual);
        }       
        [Test]
        public void IsEqualTo_WhenDifferentLengths2_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var myInts1 = new[] { 1, 3, 5 };
            var myInts2 = new[] { 1, 3, 5, 10 };
            //---------------Execute Test ----------------------
            var areEqual = myInts1.IsEqualTo(myInts2);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEqual);
        }       
        [Test]
        public void IsEqualTo_WhenDifferentValues_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var myInts1 = new[] { 1, 3, 5 };
            var myInts2 = new[] { 1, 3, 6 };
            //---------------Execute Test ----------------------
            var areEqual = myInts1.IsEqualTo(myInts2);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEqual);
        }      
        [Test]
        public void IsEqualTo_WhenDifferentOrder_ShouldReturnFalse()
        {
            //---------------Set up test pack-------------------
            var myInts1 = new[] { 1, 3, 5 };
            var myInts2 = new[] { 1, 5, 3 };
            //---------------Execute Test ----------------------
            var areEqual = myInts1.IsEqualTo(myInts2);
            //---------------Test Result -----------------------
            Assert.IsFalse(areEqual);
        }
    }
}