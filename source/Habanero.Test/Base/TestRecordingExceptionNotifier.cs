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
            Assert.IsInstanceOfType(typeof(IExceptionNotifier), exceptionNotifier);
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
        public void Test_RethrowRecordedException_WhenRecordedExceptionExists_ShouldRethrowException()
        {
            //---------------Set up test pack-------------------
            RecordingExceptionNotifier exceptionNotifier = new RecordingExceptionNotifier();
            Exception exception = new Exception();
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
                    "{0}Title: {1}{0}Further Message: {2}", Environment.NewLine, title, furtherMessage), ex.Message);
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
            catch (Exception ex)
            {
                exceptionThrown = true;
                throw;
            }
            Assert.IsFalse(exceptionThrown, "Expected not to throw an Exception");
        }
    }
}