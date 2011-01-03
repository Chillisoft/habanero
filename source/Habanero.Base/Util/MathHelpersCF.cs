using System;

using System.Collections.Generic;
using System.Text;

namespace Habanero.Base.Util
{
    public static class MathHelpersCF
    {
        public static double Truncate(double range)
        {
            if (range >= 0) return Math.Floor(range);
            return Math.Ceiling(range);
        }

        public static decimal Truncate(decimal range)
        {
            double rangeAsDouble = Convert.ToDouble(range);
            double resultAsDouble = 0;
            decimal resultAsDecimal = 0;

            if (rangeAsDouble >= 0)
            {
                resultAsDouble = Math.Floor(rangeAsDouble);
                resultAsDecimal = Convert.ToDecimal(resultAsDouble);
            }
            else
            {
                resultAsDouble = Math.Ceiling(rangeAsDouble);
                resultAsDecimal = Convert.ToDecimal(resultAsDouble);
                
            }
            return resultAsDecimal;
        }

    }
}
