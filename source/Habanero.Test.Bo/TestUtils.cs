using System;

namespace Habanero.Test.BO
{
    public static class TestUtils
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
    }
}