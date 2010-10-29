using System;
using System.Xml;
using Habanero.Base;
using log4net;
using log4net.Config;
using NUnit.Framework;
using Rhino.Mocks;

namespace Habanero.Test.Base.Logging
{
    [TestFixture]
    public class TestLog4NetLogger
    {
        [Test]
        public void Test_Construct_WithContextName_ShouldSetUpContextName()
        {
            //---------------Set up test pack-------------------
            string contextName = TestUtil.GetRandomString();   
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
            var log = MockRepository.GenerateStub<ILog>();
            log.Stub(log2 => log2.IsErrorEnabled).Return(true);
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            string message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Error(message));
            //---------------Execute Test ----------------------
            logger.Log(message);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Error(message));
        }

        [Test]
        public void Test_Log_WithMessageAndLogCategoryIsDebug_ShouldLogMessage_Debug()
        {
            //---------------Set up test pack-------------------
            var log = MockRepository.GenerateStub<ILog>();
            log.Stub(log2 => log2.IsDebugEnabled).Return(true);
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            string message = TestUtil.GetRandomString();
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
            var log = MockRepository.GenerateStub<ILog>();
            log.Stub(log2 => log2.IsErrorEnabled).Return(true);
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            string message = TestUtil.GetRandomString();
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
            var log = MockRepository.GenerateStub<ILog>();
            log.Stub(log2 => log2.IsWarnEnabled).Return(true);
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            string message = TestUtil.GetRandomString();
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
            var log = MockRepository.GenerateStub<ILog>();
            log.Stub(log2 => log2.IsInfoEnabled).Return(true);
            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
            string message = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------
            log.AssertWasNotCalled(log1 => log1.Info(message));
            //---------------Execute Test ----------------------
            logger.Log(message, LogCategory.Info);
            //---------------Test Result -----------------------
            log.AssertWasCalled(log1 => log1.Info(message));
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