using System;
using Habanero.Base;
using NUnit.Framework;

namespace Habanero.Test.BO
{
    public static class BOTestUtils
    {
        private static readonly Random rndm = new Random();

        public static int RandomInt
        {
            get { return rndm.Next(0, 100000); }
        }

        public static string RandomString
        {
            get { return "Rnd" + RandomInt; }
        }

        public static void WaitForGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public static void AssertBOStateIsValidAfterDelete(IBusinessObject bo)
        {
            Assert.IsTrue(bo.Status.IsNew);
            Assert.IsTrue(bo.Status.IsDeleted);
        }
    }
}