using System;
using Habanero.Base.Util;
using NUnit.Framework;

namespace Habanero.Test.Base
{
    [TestFixture]
    public class TestDateRange
    {
        [Test]
        public void Test_ConstructWithNoParams_ShouldSetStartAndEndTimeToMinAndMaxRespectively()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var dateRange = new DateRange();
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.MinValue, dateRange.StartDate);
            Assert.AreEqual(DateTime.MaxValue, dateRange.EndDate);
        }
/*        [Test]
        public void Test_GeIntersection_ShouldSetStartAndEndTimeToMinAndMaxRespectively()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var dateRange = new DateRange();
            //---------------Test Result -----------------------
            Assert.AreEqual(DateTime.MinValue, dateRange.StartDate);
            Assert.AreEqual(DateTime.MaxValue, dateRange.EndDate);
        }*/
    }
}