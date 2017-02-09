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

using Habanero.Base.Logging;
using NUnit.Framework;

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
            Assert.IsInstanceOf<Log4NetLogger>(logger);
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