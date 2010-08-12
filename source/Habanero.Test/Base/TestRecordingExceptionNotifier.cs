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
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestRecordingExceptionNotifier
    {
        [Test]
        public void Test_Construct()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, exceptionNotifier.Exceptions.Count);
            Assert.IsInstanceOf(typeof(IExceptionNotifier), exceptionNotifier);
        }

        [Test]
        public void Test_Notify_ShouldIncludeDetailsInExceptions()
        {
            //---------------Set up test pack-------------------
            Exception exception = new Exception();
            string furtherMessage = TestUtil.GetRandomString();
            string title = TestUtil.GetRandomString();
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, exceptionNotifier.Exceptions.Count);
            //---------------Execute Test ----------------------
            exceptionNotifier.Notify(exception, furtherMessage, title);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, exceptionNotifier.Exceptions.Count);
            RecordingExceptionNotifier.ExceptionDetail exceptionDetail = exceptionNotifier.Exceptions[0];
            Assert.AreSame(exception, exceptionDetail.Exception);
            Assert.AreSame(furtherMessage, exceptionDetail.FurtherMessage);
            Assert.AreSame(title, exceptionDetail.Title);
        }

        [Test]
        public void Test_Notify_Again_ShouldIncludeDetailsInExceptions()
        {
            //---------------Set up test pack-------------------
            Exception exception = new Exception();
            string furtherMessage = TestUtil.GetRandomString();
            string title = TestUtil.GetRandomString();
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            exceptionNotifier.Notify(new Exception(), TestUtil.GetRandomString(), TestUtil.GetRandomString());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, exceptionNotifier.Exceptions.Count);
            //---------------Execute Test ----------------------
            exceptionNotifier.Notify(exception, furtherMessage, title);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, exceptionNotifier.Exceptions.Count);
            RecordingExceptionNotifier.ExceptionDetail exceptionDetail = exceptionNotifier.Exceptions[1];
            Assert.AreSame(exception, exceptionDetail.Exception);
            Assert.AreSame(furtherMessage, exceptionDetail.FurtherMessage);
            Assert.AreSame(title, exceptionDetail.Title);
        }
        [Test]
        public void Test_HasException_WhenHas_ShouldRetTrue()
        {
            //---------------Set up test pack-------------------
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            exceptionNotifier.Notify(new Exception(), TestUtil.GetRandomString(), TestUtil.GetRandomString());
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, exceptionNotifier.Exceptions.Count);
            //---------------Execute Test ----------------------
            var hasExceptions = exceptionNotifier.HasExceptions;
            //---------------Test Result -----------------------
            Assert.IsTrue(hasExceptions);
        }
        [Test]
        public void Test_HasException_WhenNotHas_ShouldRetFalse()
        {
            //---------------Set up test pack-------------------
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, exceptionNotifier.Exceptions.Count);
            //---------------Execute Test ----------------------
            var hasExceptions = exceptionNotifier.HasExceptions;
            //---------------Test Result -----------------------
            Assert.IsFalse(hasExceptions);
        }

        [Test]
        public void Test_Message_When_NoItems_ShouldBeEmptyString()
        {
            //---------------Set up test pack-------------------
//            Exception exception = new Exception();
//            string furtherMessage = TestUtil.GetRandomString();
//            string title = TestUtil.GetRandomString();
            IExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var exceptionMessage = exceptionNotifier.ExceptionMessage;
            //---------------Test Result -----------------------
            Assert.IsNullOrEmpty(exceptionMessage);
        }

        [Test]
        public void Test_Message_WhenOneItem_ShouldReturnMessage()
        {
            //---------------Set up test pack-------------------
            Exception exception = new Exception(GetRandomString());
            string furtherMessage = TestUtil.GetRandomString();
            string title = TestUtil.GetRandomString();
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            exceptionNotifier.Notify(exception, furtherMessage, title);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, exceptionNotifier.Exceptions.Count);
            //---------------Execute Test ----------------------
            var exceptionMessage = exceptionNotifier.ExceptionMessage;
            //---------------Test Result -----------------------
            Assert.AreEqual(exception.Message + " - " + furtherMessage, exceptionMessage);
        }

        [Test]
        public void Test_Message_WhenTwoItem_ShouldReturnMessage()
        {
            //---------------Set up test pack-------------------
            string title = TestUtil.GetRandomString();
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();

            Exception exception = new Exception(GetRandomString());
            string furtherMessage = TestUtil.GetRandomString();
            exceptionNotifier.Notify(exception, furtherMessage, title);

            Exception exception2 = new Exception(GetRandomString());
            string furtherMessage2 = TestUtil.GetRandomString();
            exceptionNotifier.Notify(exception2, furtherMessage2, title);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, exceptionNotifier.Exceptions.Count);
            //---------------Execute Test ----------------------
            var exceptionMessage = exceptionNotifier.ExceptionMessage;
            //---------------Test Result -----------------------
            var expectedErrorMessage = exception.Message + " - " + furtherMessage + Environment.NewLine 
                           + exception2.Message + " - " + furtherMessage2;
            Assert.AreEqual(expectedErrorMessage, exceptionMessage);
        }

        [Test]
        public void Test_ExceptionDetailToString_ShouldConcatExceptionMessageAndFurtherMessage()
        {
            //---------------Set up test pack-------------------
            Exception exception = new Exception(GetRandomString());
            string furtherMessage = TestUtil.GetRandomString();
            RecordingExceptionNotifier.ExceptionDetail exceptionDetail
                    = new RecordingExceptionNotifier.ExceptionDetail(exception, furtherMessage, GetRandomString());
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var toString = exceptionDetail.ToString();
            //---------------Test Result -----------------------
            Assert.AreEqual(exception.Message + " - " + furtherMessage, toString);
        }

        private static string GetRandomString()
        {
            return TestUtil.GetRandomString();
        }
        [Test]
        public void Test_RethrowRecordedException_WhenRecordedExceptionExists_ShouldRethrowException()
        {
            //---------------Set up test pack-------------------
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            Exception exception = new Exception(GetRandomString());
            string furtherMessage = TestUtil.GetRandomString();
            string title = TestUtil.GetRandomString();
            exceptionNotifier.Notify(exception, furtherMessage, title);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool exceptionThrown = false;
            try
            {
                exceptionNotifier.RethrowRecordedException();
            }
            //---------------Test Result -----------------------
            catch (Exception ex)
            {
                exceptionThrown = true;
                StringAssert.Contains(string.Format(
                    "An Exception that was recorded by the RecordingExceptionNotifier and has been rethrown." +
                    "{0}Title: {1}{0}Further Message: {2}", Environment.NewLine, title, exceptionNotifier.ExceptionMessage), ex.Message);
                Assert.AreSame(exception, ex.InnerException);
            }
            Assert.IsTrue(exceptionThrown, "Expected to throw an Exception");
        }

        [Test]
        public void Test_RethrowRecordedException_WhenNoRecordedExceptionsExist_ShouldDoNothing()
        {
            //---------------Set up test pack-------------------
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            bool exceptionThrown = false;
            try
            {
                exceptionNotifier.RethrowRecordedException();
            }
            //---------------Test Result -----------------------
            catch (Exception)
            {
                exceptionThrown = true;
            }
            Assert.IsFalse(exceptionThrown, "Expected not to throw an Exception");
        }
    }
}