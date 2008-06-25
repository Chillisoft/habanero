//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------


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

        /// <summary>
        /// Returns the first day of the current financial year.
        /// If the financial year starts in march then monthFinancialYearStarts = 3.
        /// If the current date is before 01 March e.g. 12 Feb 2007 then the the current financial year starts on the 01 March 2006.
        /// Then the current date is after 01 March e.g. 21 August 2007 then the current financial year starts on the 01 March 2007. 
        /// </summary>
        /// <param name="monthFinancialYearStarts">the month that the financial year starts usually march in south africa</param>
        /// <returns></returns>
        public static DateTime FirstDayOFinancialYear(int monthFinancialYearStarts)
        {
            return FirstDayOFinancialYear(monthFinancialYearStarts, DateTime.Today);
        }

        /// <summary>
        /// Returns the first day of the current financial year.
        /// If the financial year starts in march then monthFinancialYearStarts = 3.
        /// If the current date is before 01 March e.g. 12 Feb 2007 then the the current financial year starts on the 01 March 2006.
        /// Then the current date is after 01 March e.g. 21 August 2007 then the current financial year starts on the 01 March 2007. 
        /// </summary>
        /// <param name="currentDate">The date for which you wish to calculate the first day of the financial year</param>
        /// <param name="monthFinancialYearStarts">the month that the financial year starts usually march in south africa</param>
        /// <returns></returns>
        public static DateTime FirstDayOFinancialYear(int monthFinancialYearStarts, DateTime currentDate)
        {
            DateTime firstday;
            int currentYear = currentDate.Year;
            int currentMonth = currentDate.Month;
            if (currentMonth >= monthFinancialYearStarts)
            {
                firstday = new DateTime(currentYear, monthFinancialYearStarts, 1);
            }
            else
            {
                firstday = new DateTime(currentYear - 1, monthFinancialYearStarts, 1);
            }
            return firstday;
        }

        /// <summary>
        /// Returns the last day of the current financial year (the current financial year is determined by the current date).
        /// If the financial year starts in march then monthFinancialYearStarts = 3.
        /// If the current date is before 01 March e.g. 12 Feb 2007 then the the current financial year starts on the 01 March 2006
        ///  and ends on the 28 Feb 2007.
        /// If the current date is after 01 March e.g. 21 August 2007 then the current financial year starts on the 01 March 2007
        /// and ends on the 29 Feb 2008. 
        /// </summary>
        /// <param name="currentDate">The date for which you wish to calculate the last day of the financial year</param>
        /// <param name="monthFinancialYearStarts">the month that the financial year starts usually march in south africa</param>
        /// <returns></returns>
        public static DateTime LastDayOfFinancialYear(int monthFinancialYearStarts, DateTime currentDate)
        {
            DateTime firstDayOFinancialYear = FirstDayOFinancialYear(monthFinancialYearStarts, currentDate);
            return firstDayOFinancialYear.AddYears(1).AddDays(-1);
        }
    }
}