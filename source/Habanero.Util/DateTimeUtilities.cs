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
    }
}
