using System;
using System.Reflection;
using Habanero.Base;
using NUnit.Framework;
using Rhino.Mocks;

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
            IHabaneroLogger logger = loggerFactory.GetLogger(TestUtil.GetRandomString());
            //---------------Test Result -----------------------
            Assert.IsNotNull(logger);
            Assert.IsInstanceOf<IHabaneroLogger>(logger );
            Assert.IsInstanceOf<Log4NetLogger>(logger);
        } 
        [Test]
        public void Test_GetLogger_WithContextname_ShouldReturnNewLoggerWithContextName()
        {
            //---------------Set up test pack-------------------
            IHabaneroLoggerFactory loggerFactory = new Log4NetLoggerFactory();
            string expectedContextName = TestUtil.GetRandomString();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IHabaneroLogger logger = loggerFactory.GetLogger(expectedContextName);
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedContextName, logger.ContextName);

        } 

    }

   



   

/*

    */
}