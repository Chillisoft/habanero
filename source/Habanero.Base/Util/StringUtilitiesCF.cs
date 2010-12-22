using System;

using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Habanero.Base.Util
{
    public static class StringUtilitiesCF
    {
        public static bool Contains(string lookingFor, string inString)
        {
            return inString.IndexOf(lookingFor) >= 0;
        }

        public static bool StartsWith(string inString, string value, bool ignoreCase, CultureInfo culture)
        {
            CultureInfo currentCulture;
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (inString == value)
            {
                return true;
            }
            currentCulture = culture ?? CultureInfo.CurrentCulture;
            return currentCulture.CompareInfo.IsPrefix(inString, value, ignoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);

        }


    }
}
