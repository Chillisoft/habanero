using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.BO
{
    public class BORegistry
    {
        private static IBusinessObjectLoader _businessObjectLoader;


        public static IBusinessObjectLoader BusinessObjectLoader
        {
            get { return _businessObjectLoader; }
            set { _businessObjectLoader = value; }
        }
    }
}
