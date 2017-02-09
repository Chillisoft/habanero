//#region Licensing Header
//// ---------------------------------------------------------------------------------
////  Copyright (C) 2007-2011 Chillisoft Solutions
////  
////  This file is part of the Habanero framework.
////  
////      Habanero is a free framework: you can redistribute it and/or modify
////      it under the terms of the GNU Lesser General Public License as published by
////      the Free Software Foundation, either version 3 of the License, or
////      (at your option) any later version.
////  
////      The Habanero framework is distributed in the hope that it will be useful,
////      but WITHOUT ANY WARRANTY; without even the implied warranty of
////      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
////      GNU Lesser General Public License for more details.
////  
////      You should have received a copy of the GNU Lesser General Public License
////      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//// ---------------------------------------------------------------------------------
//#endregion
//using System;
//using Habanero.Base.Exceptions;
//using Habanero.Base.Logging;
//using log4net;
//using NSubstitute;
//using NUnit.Framework;

//// ReSharper disable InconsistentNaming
//namespace Habanero.Test.Base.Logging
//{
//    [TestFixture]
//    public class TestLog4NetLogger
//    {
//        [Test]
//        public void Test_Construct_WithContextName_ShouldSetUpContextName()
//        {
//            //---------------Set up test pack-------------------
//            var contextName = TestUtil.GetRandomString();   
//            //---------------Assert Precondition----------------

//            //---------------Execute Test ----------------------
//            var logger = new Log4NetLogger(contextName);
//            //---------------Test Result -----------------------
//            Assert.IsInstanceOf<IHabaneroLogger>(logger);
//            Assert.AreEqual(contextName, logger.ContextName);
//        }

//        [Test]
//        public void Test_Log_WithMessage_ShouldLogMessage_MediumPriority_Error()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var message = TestUtil.GetRandomString();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Error(message));
//            //---------------Execute Test ----------------------
//            logger.Log(message, LogCategory.Exception);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Error(message));
//        }

//        [Test]
//        public void Test_Log_WithMessageAndNonUserException_ShouldLog_Error()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var exception = new Exception();
//            var message = TestUtil.GetRandomString();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Error(message, exception));
//            Assert.IsNotInstanceOf<UserException>(exception);
//            //---------------Execute Test ----------------------
//            logger.Log(message, exception);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Error(message, exception));
//            log.AssertWasNotCalled(log1 => log1.Info(message, exception));
//        }
//        [Test]
//        public void Test_Log_WithMessageAndNonUserExceptionAndLogCategoryWarn_ShouldLog_Warn()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var exception = new Exception();
//            var message = TestUtil.GetRandomString();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Warn(message, exception));
//            Assert.IsNotInstanceOf<UserException>(exception);
//            //---------------Execute Test ----------------------
//            logger.Log(message, exception, LogCategory.Warn);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Warn(message, exception));
//            log.AssertWasNotCalled(log1 => log1.Error(message, exception));
//            log.AssertWasNotCalled(log1 => log1.Info(message, exception));
//        }
//        [Test]
//        public void Test_Log_WithMessageAndNonUserExceptionAndLogCategoryFatal_ShouldLog_Fatal()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var exception = new Exception();
//            var message = TestUtil.GetRandomString();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Fatal(message, exception));
//            Assert.IsNotInstanceOf<UserException>(exception);
//            //---------------Execute Test ----------------------
//            logger.Log(message, exception, LogCategory.Fatal);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Fatal(message, exception));
//            log.AssertWasNotCalled(log1 => log1.Error(message, exception));
//            log.AssertWasNotCalled(log1 => log1.Info(message, exception));
//            log.AssertWasNotCalled(log1 => log1.Warn(message, exception));
//        }
//        [Test]
//        public void Test_Log_WithNonUserException_ShouldLog_Error()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var exception = new Exception();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Error("", exception));
//            Assert.IsNotInstanceOf<UserException>(exception);
//            //---------------Execute Test ----------------------
//            logger.Log(exception);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Error("", exception));
//            log.AssertWasNotCalled(log1 => log1.Info("", exception));
//        }

//        [Test]
//        public void Test_Log_WithUserException_ShouldLog_Info()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var exception = new UserException();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Info("", exception));
//            Assert.IsInstanceOf<UserException>(exception);
//            //---------------Execute Test ----------------------
//            logger.Log(exception);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Info("", exception));
//            log.AssertWasNotCalled(log1 => log1.Error("", exception));
//        }


//        [Test]
//        public void Test_Log_WithMessageAndLogCategoryIsDebug_ShouldLogMessage_Debug()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var message = TestUtil.GetRandomString();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Debug(message));
//            //---------------Execute Test ----------------------
//            logger.Log(message,LogCategory.Debug);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Debug(message));
//        }

//        [Test]
//        public void Test_Log_WithMessageAndLogCategoryIsException_ShouldLogMessage_Error()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var message = TestUtil.GetRandomString();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Error(message));
//            //---------------Execute Test ----------------------
//            logger.Log(message,LogCategory.Exception);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Error(message));
//        }

//        [Test]
//        public void Test_Log_WithMessageAndLogCategoryIsWarn_ShouldLogMessage_Warning()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var message = TestUtil.GetRandomString();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Warn(message));
//            //---------------Execute Test ----------------------
//            logger.Log(message, LogCategory.Warn);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Warn(message));
//        }

//        [Test]
//        public void Test_Log_WithMessageAndLogCategoryIsInfo_ShouldLogMessage_Info()
//        {
//            //---------------Set up test pack-------------------
//            var log = GetMockLog();
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            var message = TestUtil.GetRandomString();
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.Info(message));
//            //---------------Execute Test ----------------------
//            logger.Log(message, LogCategory.Info);
//            //---------------Test Result -----------------------
//            log.AssertWasCalled(log1 => log1.Info(message));
//		}





//		[Test]
//		public void Test_Log_WithMessageAndLogCategoryIsDebug_AndNotEnabled_ShouldNotLogMessage_Debug()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//            log.IsDebugEnabled.Returns(false);
//            IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//			var message = TestUtil.GetRandomString();
//			//---------------Assert Precondition----------------
//			log.AssertWasNotCalled(log1 => log1.Debug(message));
//			//---------------Execute Test ----------------------
//			logger.Log(message, LogCategory.Debug);
//			//---------------Test Result -----------------------
//			log.AssertWasNotCalled(log1 => log1.Debug(message));
//		}

//		[Test]
//		public void Test_Log_WithMessageAndLogCategoryIsException_AndNotEnabled_ShouldNotLogMessage_Error()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			log.IsErrorEnabled.Returns(false);
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//			var message = TestUtil.GetRandomString();
//			//---------------Assert Precondition----------------
//			log.AssertWasNotCalled(log1 => log1.Error(message));
//			//---------------Execute Test ----------------------
//			logger.Log(message, LogCategory.Exception);
//			//---------------Test Result -----------------------
//			log.AssertWasNotCalled(log1 => log1.Error(message));
//		}

//		[Test]
//		public void Test_Log_WithMessageAndLogCategoryIsWarn_AndNotEnabled_ShouldNotLogMessage_Warning()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			log.IsWarnEnabled.Returns(false);
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//			var message = TestUtil.GetRandomString();
//			//---------------Assert Precondition----------------
//			log.AssertWasNotCalled(log1 => log1.Warn(message));
//			//---------------Execute Test ----------------------
//			logger.Log(message, LogCategory.Warn);
//			//---------------Test Result -----------------------
//			log.AssertWasNotCalled(log1 => log1.Warn(message));
//		}

//		[Test]
//		public void Test_Log_WithMessageAndLogCategoryIsInfo_AndNotEnabled_ShouldNotLogMessage_Info()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			log.Stub(log2 => log2.IsInfoEnabled).Return(false);
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//			var message = TestUtil.GetRandomString();
//			//---------------Assert Precondition----------------
//			log.AssertWasNotCalled(log1 => log1.Info(message));
//			//---------------Execute Test ----------------------
//			logger.Log(message, LogCategory.Info);
//			//---------------Test Result -----------------------
//			log.AssertWasNotCalled(log1 => log1.Info(message));
//		}

//		[Test]
//		public void Test_IsLogging_WhenLogCategoryIsDebug_AndIsTrue_ShouldReturnTrue_Debug()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            log.IsDebugEnabled.Returns(true);
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.IsDebugEnabled);
//			//---------------Execute Test ----------------------
//			var isLogging = logger.IsLogging(LogCategory.Debug);
//			//---------------Test Result -----------------------
//			Assert.IsTrue(isLogging);
//			log.AssertWasCalled(log1 => log1.IsDebugEnabled);
//		}

//		[Test]
//		public void Test_IsLogging_WhenLogCategoryIsException_AndIsTrue_ShouldReturnTrue_Error()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            log.IsErrorEnabled.Returns(true);
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.IsErrorEnabled);
//			//---------------Execute Test ----------------------
//			var isLogging = logger.IsLogging(LogCategory.Exception);
//			//---------------Test Result -----------------------
//			Assert.IsTrue(isLogging);
//			log.AssertWasCalled(log1 => log1.IsErrorEnabled);
//		}

//		[Test]
//		public void Test_IsLogging_WhenLogCategoryIsWarn_AndIsTrue_ShouldReturnTrue_Warning()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            log.IsWarnEnabled.Returns(true);
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.IsWarnEnabled);
//			//---------------Execute Test ----------------------
//			var isLogging = logger.IsLogging(LogCategory.Warn);
//			//---------------Test Result -----------------------
//			Assert.IsTrue(isLogging);
//			log.AssertWasCalled(log1 => log1.IsWarnEnabled);
//		}

//		[Test]
//		public void Test_IsLogging_WhenLogCategoryIsInfo_AndIsTrue_ShouldReturnTrue_Info()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            log.IsInfoEnabled.Returns(true);
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.IsInfoEnabled);
//			//---------------Execute Test ----------------------
//			var isLogging = logger.IsLogging(LogCategory.Info);
//			//---------------Test Result -----------------------
//			Assert.IsTrue(isLogging);
//			log.AssertWasCalled(log1 => log1.IsInfoEnabled);
//		}

//		[Test]
//		public void Test_IsLogging_WhenLogCategoryIsDebug_AndIsFalse_ShouldReturnFalse_Debug()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            log.IsDebugEnabled.Returns(false);
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.IsDebugEnabled);
//			//---------------Execute Test ----------------------
//			var isLogging = logger.IsLogging(LogCategory.Debug);
//			//---------------Test Result -----------------------
//			Assert.IsFalse(isLogging);
//			log.AssertWasCalled(log1 => log1.IsDebugEnabled);
//		}

//		[Test]
//		public void Test_IsLogging_WhenLogCategoryIsException_AndIsFalse_ShouldReturnFalse_Error()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//			//---------------Assert Precondition----------------
//			log.AssertWasNotCalled(log1 => log1.IsErrorEnabled);
//			//---------------Execute Test ----------------------
//			var isLogging = logger.IsLogging(LogCategory.Exception);
//			//---------------Test Result -----------------------
//			Assert.IsFalse(isLogging);
//			log.AssertWasCalled(log1 => log1.IsErrorEnabled);
//		}

//		[Test]
//		public void Test_IsLogging_WhenLogCategoryIsWarn_AndIsFalse_ShouldReturnFalse_Warning()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//		    log.IsWarnEnabled.Returns(false);
//			//---------------Assert Precondition----------------
//			log.AssertWasNotCalled(log1 => log1.IsWarnEnabled);
//			//---------------Execute Test ----------------------
//			var isLogging = logger.IsLogging(LogCategory.Warn);
//			//---------------Test Result -----------------------
//			Assert.IsFalse(isLogging);
//			log.AssertWasCalled(log1 => log1.IsWarnEnabled);
//		}

//		[Test]
//		public void Test_IsLogging_WhenLogCategoryIsInfo_AndIsFalse_ShouldReturnFalse_Info()
//		{
//			//---------------Set up test pack-------------------
//			var log = GetBasicMockLog();
//			IHabaneroLogger logger = new Log4NetLoggerSpy(log);
//            log.IsInfoEnabled.Returns(true);
//            //---------------Assert Precondition----------------
//            log.AssertWasNotCalled(log1 => log1.IsInfoEnabled);
//			//---------------Execute Test ----------------------
//			var isLogging = logger.IsLogging(LogCategory.Info);
//			//---------------Test Result -----------------------
//			Assert.IsFalse(isLogging);
//			log.Received(log1 => log1.IsInfoEnabled);
//		}

//    	private static ILog GetBasicMockLog()
//	    {
//	        return Substitute.For<ILog>();
//	    }
        
//    	private static ILog GetMockLog()
//	    {
//	        var log = Substitute.For<ILog>();
//	        log.IsInfoEnabled.Returns(true);
//            log.IsInfoEnabled.Returns(true);
//            log.IsErrorEnabled.Returns(true);
//            log.IsFatalEnabled.Returns(true);
//            log.IsWarnEnabled.Returns(true);
//            log.IsDebugEnabled.Returns(true);
//            return log;
//        }
//    }

   

   
///*    public enum LogPriority
//    {
//        High, Low, Medium, None
//    }*/

   

//    internal class Log4NetLoggerSpy : Log4NetLogger
//    {
//        public Log4NetLoggerSpy(ILog log) : base(TestUtil.GetRandomString())
//        {
//            _log = log;
//        }
//    }
//}