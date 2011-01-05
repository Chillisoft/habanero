

using System;
using OpenNETCF;

namespace Habanero.Base.Util
{
    public static class HabaneroEnvironmentCF
    {
        public static string MachineName()
        {
            try
            {
                return Environment2.MachineName;
            }
            catch (Exception)
            {
                return "testMachine";
            }
        }

        public static string GetCurrentUser()
        {
            return "NA";
        }
    }
}
