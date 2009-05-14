using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
