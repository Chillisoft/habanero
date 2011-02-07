using System;
using System.Xml;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.Base.Logging;
using log4net;
using log4net.Config;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.Base.Logging
{
    [TestFixture]
    public class TestLog4NetLogger
    {
        [Test]
        public void Test_Construct_WithContextName_ShouldSetUpContextName()
        {
            //---------------Set up test pack-------------------
            var contextName = TestUtil.GetRandomString();   
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var logger = new Log4NetLogger(contextName);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<IHabaneroLogger>(logger);
            Assert.AreEqual(contextName, logger.ContextName);
        }

        [Test]
        public void Test_Log_WithMessage_ShouldLogMessage_MediumPriority_Error()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Error(message));
            //---------------Execute Test ----------------------
            logger.Log(message);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Error(message));
        }

        [Test]
        public void Test_Log_WithMessageAndNonUserException_ShouldLog_Error()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var exception = new Exception();
            var message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Error(message, exception));
            Assert.IsNotInstanceOf<UserException>(exception);
            //---------------Execute Test ----------------------
            logger.Log(message, exception);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Error(message, exception));
            log.AssertWasNotCalled(log1 => log1.Info(message, exception));
        }
        [Test]
        public void Test_Log_WithMessageAndNonUserExceptionAndLogCategoryWarn_ShouldLog_Warn()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var exception = new Exception();
            var message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Warn(message, exception));
            Assert.IsNotInstanceOf<UserException>(exception);
            //---------------Execute Test ----------------------
            logger.Log(message, exception, LogCategory.Warn);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Warn(message, exception));
            log.AssertWasNotCalled(log1 => log1.Error(message, exception));
            log.AssertWasNotCalled(log1 => log1.Info(message, exception));
        }
        [Test]
        public void Test_Log_WithMessageAndNonUserExceptionAndLogCategoryFatal_ShouldLog_Fatal()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var exception = new Exception();
            var message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Fatal(message, exception));
            Assert.IsNotInstanceOf<UserException>(exception);
            //---------------Execute Test ----------------------
            logger.Log(message, exception, LogCategory.Fatal);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Fatal(message, exception));
            log.AssertWasNotCalled(log1 => log1.Error(message, exception));
            log.AssertWasNotCalled(log1 => log1.Info(message, exception));
            log.AssertWasNotCalled(log1 => log1.Warn(message, exception));
        }
        [Test]
        public void Test_Log_WithNonUserException_ShouldLog_Error()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var exception = new Exception();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Error("", exception));
            Assert.IsNotInstanceOf<UserException>(exception);
            //---------------Execute Test ----------------------
            logger.Log(exception);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Error("", exception));
            log.AssertWasNotCalled(log1 => log1.Info("", exception));
        }

        [Test]
        public void Test_Log_WithUserException_ShouldLog_Info()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var exception = new UserException();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Info("", exception));
            Assert.IsInstanceOf<UserException>(exception);
            //---------------Execute Test ----------------------
            logger.Log(exception);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Info("", exception));
            log.AssertWasNotCalled(log1 => log1.Error("", exception));
        }


        [Test]
        public void Test_Log_WithMessageAndLogCategoryIsDebug_ShouldLogMessage_Debug()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Debug(message));
            //---------------Execute Test ----------------------
            logger.Log(message,LogCategory.Debug);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Debug(message));
        }

        [Test]
        public void Test_Log_WithMessageAndLogCategoryIsException_ShouldLogMessage_Error()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Error(message));
            //---------------Execute Test ----------------------
            logger.Log(message,LogCategory.Exception);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Error(message));
        }

        [Test]
        public void Test_Log_WithMessageAndLogCategoryIsWarn_ShouldLogMessage_Warning()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Warn(message));
            //---------------Execute Test ----------------------
            logger.Log(message, LogCategory.Warn);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Warn(message));
        }

        [Test]
        public void Test_Log_WithMessageAndLogCategoryIsInfo_ShouldLogMessage_Info()
        {
            //---------------Set up test pack-------------------
            var log = GetMockLog();
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            var message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Info(message));
            //---------------Execute Test ----------------------
            logger.Log(message, LogCategory.Info);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Info(message));
        }

        private static ILog GetMockLog()
        {
            var log = MockRepository.GenerateStub<ILog>();
            log.Stub(log2 => log2.IsInfoEnabled).Return(true);
            log.Stub(log2 => log2.IsErrorEnabled).Return(true);
            log.Stub(log2 => log2.IsFatalEnabled).Return(true);
            log.Stub(log2 => log2.IsWarnEnabled).Return(true);
            log.Stub(log2 => log2.IsDebugEnabled).Return(true);
            return log;
        }
    }

   

   
/*    public enum LogPriority
    {
        High, Low, Medium, None
    }*/

   

    internal class Log4NetLoggerSpy : Log4NetLogger
    {
        public Log4NetLoggerSpy(ILog log) : base(TestUtil.GetRandomString())
        {
            _log = log;
        }
    }
}