using System;

namespace Habanero.Test.DB4O
{
    public class TestUtil
    {
        public static string GetRandomString()
        {
            return Guid.NewGuid().ToString("N");
        }

    }
}
