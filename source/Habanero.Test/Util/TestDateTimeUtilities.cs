//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using Habanero.Util;
using NUnit.Framework; 

namespace Habanero.Test.Util
{
    [TestFixture]
    public class TestTestDateTimeUtilities
        //:TestBase
    {
        [SetUp]
        public
            void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

        [TestFixtureSetUp]
        public
            void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public
            void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }

        [Test]
        public void Test_FirstDayOfFinYear_MonthGTStartMonth()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.FirstDayOFinancialYear(4, new DateTime(2007, 4, 12));
            ;
            //---------------Test Result -----------------------
            Assert.AreEqual(4, firstDay.Month);
            Assert.AreEqual(1, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }

        [Test]
        public void Test_FirstDayOfFinYear_MonthLTStartMonth()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.FirstDayOFinancialYear(4, new DateTime(2007, 3, 12));
            ;
            //---------------Test Result -----------------------
            Assert.AreEqual(4, firstDay.Month);
            Assert.AreEqual(1, firstDay.Day);
            Assert.AreEqual(2006, firstDay.Year);
        }
        [Test]
        public void Test_FirstDayOfFinYear_MonthGTStartDecember()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.FirstDayOFinancialYear(12, new DateTime(2007, 12, 12));
            ;
            //---------------Test Result -----------------------
            Assert.AreEqual(12, firstDay.Month);
            Assert.AreEqual(1, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }

        [Test]
        public void Test_FirstDayOfFinYear_MonthGTStartJanuary()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.FirstDayOFinancialYear(01, new DateTime(2007, 12, 12));
            ;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, firstDay.Month);
            Assert.AreEqual(1, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }

        [Test]
        public void Test_FirstDayOfFinYear_InvalidStartMonth()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                DateTimeUtilities.FirstDayOFinancialYear(13, new DateTime(2007, 12, 12));
                Assert.Fail("expected Err");
            }
                //---------------Test Result -----------------------
            catch (ArgumentOutOfRangeException ex)
            {
                StringAssert.Contains("Year, Month, and Day parameters describe an un-representable DateTime", ex.Message);
            }
      
        }

        [Test]
        public void Test_LastDayOfFinYear()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.LastDayOfFinancialYear(1, new DateTime(2007, 12, 11));
            
            //---------------Test Result -----------------------
            Assert.AreEqual(12, firstDay.Month);
            Assert.AreEqual(31, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }
        [Test]
        public void Test_LastDayOfFinYear_MonthGTStartDecember()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.LastDayOfFinancialYear(12, new DateTime(2007, 12, 12));
            ;
            //---------------Test Result -----------------------
            Assert.AreEqual(11, firstDay.Month);
            Assert.AreEqual(30, firstDay.Day);
            Assert.AreEqual(2008, firstDay.Year);
        }

        [Test]
        public void Test_LastDayOfFinYear_currentDateOnStartDate()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.LastDayOfFinancialYear(1, new DateTime(2007, 01, 01));
            ;
            //---------------Test Result -----------------------
            Assert.AreEqual(12, firstDay.Month);
            Assert.AreEqual(31, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }
        [Test]
        public void Test_LastDayOfFinYear_currentDateOnLastDay()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            DateTime firstDay = DateTimeUtilities.LastDayOfFinancialYear(1, new DateTime(2007, 12, 31));
            ;
            //---------------Test Result -----------------------
            Assert.AreEqual(12, firstDay.Month);
            Assert.AreEqual(31, firstDay.Day);
            Assert.AreEqual(2007, firstDay.Year);
        }

        [Test]
        public void Test_CloseToDateTimeNow_BeforeNow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            DateTime testValue = DateTime.Now.AddSeconds(-5);
            Assert.IsTrue(DateTimeUtilities.CloseToDateTimeNow(testValue, 10));
            Assert.IsFalse(DateTimeUtilities.CloseToDateTimeNow(testValue, 4)); 
        }

        [Test]
        public void Test_CloseToDateTimeNow_AfterNow()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            //---------------Test Result -----------------------
            DateTime testValue = DateTime.Now.AddSeconds(5);
            Assert.IsTrue(DateTimeUtilities.CloseToDateTimeNow(testValue, 20));
            Assert.IsFalse(DateTimeUtilities.CloseToDateTimeNow(testValue, 3));
        }
    }
}