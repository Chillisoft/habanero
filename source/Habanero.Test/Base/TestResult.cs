using System;
using System.Collections.Generic;
using System.Text;
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
