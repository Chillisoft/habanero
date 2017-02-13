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

namespace Habanero.Test.Base.Encryption
{
    [TestFixture]
    public class TestBase64Sha1Hasher
    {
        protected virtual IHasher CreateHasher() { return new Base64Sha1Hasher(); }

        [Test]
        public void HashString_ShouldReturnDifferentString()
        {
            //---------------Set up test pack-------------------
            var hasher = CreateHasher();
            const string originalString = "123456";
            //---------------Execute Test ----------------------
            var hash = hasher.HashString(originalString);
            //---------------Test Result -----------------------
            Assert.AreNotEqual(originalString, hash);
        }

        [Test]
        public void HashString_ShouldReturnSameStringEachTime()
        {
            //---------------Set up test pack-------------------
            var hasher = CreateHasher();
            const string originalString = "123456";
            var expectedHash = hasher.HashString(originalString);
            //---------------Execute Test ----------------------
            var hash = hasher.HashString(originalString);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedHash, hash);
        }

        [Test]
        public void HashString_ShouldReturnSameLengthStringsForEachValue()
        {
            //---------------Set up test pack-------------------
            var hasher = CreateHasher();
            //---------------Execute Test ----------------------
            var hash1 = hasher.HashString("abcdef"); 
            var hash2 = hasher.HashString("123");
            //---------------Test Result -----------------------
            Assert.AreEqual(hash1.Length, hash2.Length);
        }

        [Test]
        public void HashString_ShouldWorkWithEmptyString()
        {
            //---------------Set up test pack-------------------
            var hasher = CreateHasher();
            //---------------Execute Test ----------------------
            hasher.HashString(""); 
        }

        [Test]
        public void HashString_ShouldWorkWithNull()
        {
            //---------------Set up test pack-------------------
            var hasher = CreateHasher();
            //---------------Execute Test ----------------------
            hasher.HashString(null); 
        }

    }
}
