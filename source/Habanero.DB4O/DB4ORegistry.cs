using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db4objects.Db4o;

namespace Habanero.DB4O
{
    public static class DB4ORegistry
    {
        public static IObjectContainer DB { get; set; }

    }
}
