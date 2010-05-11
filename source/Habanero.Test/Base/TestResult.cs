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
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestResult
    {
        [Test]
        public void Test_CanCreateResultSuccess_WithNoMessage()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Result result = new Result(true);
            //---------------Test Result -----------------------
            Assert.IsTrue(result.Successful);
            Assert.IsNull(result.Message);
        }

        [Test]
        public void Test_CanCreateResultSuccess_WithMessage()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string message = TestUtil.GetRandomString();
            Result result = new Result(true, message);
            //---------------Test Result -----------------------
            Assert.IsTrue(result.Successful);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public void Test_CanCreateResultNoSuccess_WithNoMessage()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            Result result = new Result(false);
            //---------------Test Result -----------------------
            Assert.IsFalse(result.Successful);
            Assert.IsNull(result.Message);
        }

        [Test]
        public void Test_CanCreateResultNoSuccess_WithMessage()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            string message = TestUtil.GetRandomString();
            Result result = new Result(false, message);
            //---------------Test Result -----------------------
            Assert.IsFalse(result.Successful);
            Assert.AreEqual(message, result.Message);
        }
    }
}
