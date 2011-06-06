using System;
using System.Reflection;
using Habanero.Base;
using Habanero.Base.Logging;
using NUnit.Framework;
using Rhino.Mocks;

// ReSharper disable InconsistentNaming
namespace Habanero.Test.Base.Logging
{
    [TestFixture]
    public class TestLog4NetLoggerFactory
    {
        [Test]
        public void Test_Construct_ShouldHaveLoggerFactoryInterface()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var loggerFactory = new Log4NetLoggerFactory();
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<IHabaneroLoggerFactory>(loggerFactory );
        }
        [Test]
        public void Test_GetLogger_ShouldReturnNewLogger()
        {
            //---------------Set up test pack-------------------
            IHabaneroLoggerFactory loggerFactory = new Log4NetLoggerFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var logger = loggerFactory.GetLogger(TestUtil.GetRandomString());
            //---------------Test Result -----------------------
            Assert.IsNotNull(logger);
            Assert.IsInstanceOf<IHabaneroLogger>(logger );
            Assert.IsInstanceOf<HabaneroLoggerLog4Net>(logger);
        } 
        [Test]
        public void Test_GetLogger_WithContextname_ShouldReturnNewLoggerWithContextName()
        {
            //---------------Set up test pack-------------------
            IHabaneroLoggerFactory loggerFactory = new Log4NetLoggerFactory();
            string expectedContextName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var logger = loggerFactory.GetLogger(expectedContextName);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedContextName, logger.ContextName);
        }

        [Test]
        public void Test_GetLogger_WithType_ShouldCreateLog4NetLogger()
        {
            //---------------Set up test pack-------------------
            IHabaneroLoggerFactory loggerFactory = new Log4NetLoggerFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var logger = loggerFactory.GetLogger(typeof(FakeObject));
            //---------------Test Result -----------------------
            Assert.IsNotNull(logger);
            Assert.IsInstanceOf<HabaneroLoggerLog4Net>(logger);
        }
        [Test]
        public void Test_GetLogger_WithType_ShouldSetContext()
        {
            //---------------Set up test pack-------------------
            IHabaneroLoggerFactory loggerFactory = new Log4NetLoggerFactory();
            const string expectedContextName = "Habanero.Test.Base.Logging.FakeObject";
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var logger = loggerFactory.GetLogger(typeof(FakeObject));
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedContextName, logger.ContextName);
        }
    }

    public class FakeObject
    {
    }
}