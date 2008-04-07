using System;

namespace Habanero.Util
{
    ///<summary>
    ///General utilities used for datetime manipulation e.g. LastDayOfMonth
    ///</summary>
    public class DateTimeUtilities
    {
        /// <summary>
        /// returns the last day of the month today
        /// </summary>
        /// <returns></returns>
        public static DateTime LastDayOfTheMonth()
        {
            return LastDayOfTheMonth(new DateTime());
        }

        /// <summary>
        /// returns the last day of the month defined by dte
        /// </summary>
        /// <param name="dte"></param>
        /// <returns></returns>
        public static DateTime LastDayOfTheMonth(DateTime dte)
        {
            int lastDayOfMonth = DateTime.DaysInMonth(dte.Year, dte.Month);
            return new DateTime(dte.Year, dte.Month, lastDayOfMonth);
        }
        /// <summary>
        /// returns the first day of the current month
        /// </summary>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth()
        {
            return FirstDayOfMonth(DateTime.Today);
        }
        /// <summary>
        /// returns the first day of the month specified by dte
        /// </summary>
        /// <param name="dte"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime dte)
        {
            return new DateTime(dte.Year, dte.Month, 1);
        }
    }
}
