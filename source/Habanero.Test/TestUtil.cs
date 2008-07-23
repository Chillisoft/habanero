using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.Test
{
    public class TestUtil
    {
        public static string CreateRandomString()
        {
            return Guid.NewGuid().ToString("N");

        }
    }
}
