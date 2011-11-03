// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.Globalization;
using Habanero.Base;
using Habanero.Base.Exceptions;

namespace Habanero.Util
{
    ///<summary>
    ///General utilities used for datetime manipulation e.g. LastDayOfMonth
    ///</summary>
    public static class DateTimeUtilities
    {
        private const  int _DECEMBER = 12;
        private const int _NOVEMBER = 11;
        private const int _JANUARY = 11;

        /// <summary>
        /// The standard date Time Format to use.
        /// </summary>
        public const string StandardDateTimeFormat = "dd MMM yyyy HH:mm:ss:fff";
        private static IFormatProvider _dateTimeFormatProvider = new DateTimeFormatInfo { FullDateTimePattern = StandardDateTimeFormat};
                    
        /// <summary>
        /// returns the last day of the current month (i.e. LastDayOfTheMonth(Today)
        /// </summary>
        /// <returns></returns>
        public static DateTime LastDayOfTheMonth()
        {
            return LastDayOfTheMonth(DateTime.Today);
        }

        /// <summary>
        /// returns the last day of the month defined by dte
        /// </summary>
        /// <param name="dte"></param>
        /// <returns></returns>
        public static DateTime LastDayOfTheMonth(DateTime dte)
        {
            int lastDayOfMonth = DateTime.DaysInMonth(dte.Year, dte.Month);
            return new DateTime(dte.Year, dte.Month, lastDayOfMonth, 23, 59, 59, 999);
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

        /// <summary>
        /// Indicates whether the given DateTime value is close to DateTime.Now,
        /// within the given tolerance range
        /// </summary>
        /// <param name="valueToCheck">The DateTime value to check</param>
        /// <param name="toleranceInSeconds">The maximum number of distance in seconds that
        /// the value can be from Now and still be accepted</param>
        /// <returns>Returns true if within the tolerance range</returns>
        public static bool CloseToDateTimeNow(DateTime valueToCheck, int toleranceInSeconds)
        {
            TimeSpan difference = DateTime.Now - valueToCheck;
            return Math.Abs(difference.TotalSeconds) < toleranceInSeconds;
        }

        ///<summary>
        /// Parses the <paramref name="valueToParse"/> to a valid date time
        ///</summary>
        ///<param name="valueToParse"></param>
        ///<returns>The parsed valid date time</returns>
        /// <exception cref="HabaneroIncorrectTypeException">Exception raised if value cannot be parsed</exception>
        /// <seealso cref="TryParseValue"/>
        public static DateTime ParseToDate(object valueToParse)
        {
            object returnValue = ParseValue(valueToParse);

            if (returnValue is IResolvableToValue<DateTime>)
            {
                returnValue = ((IResolvableToValue<DateTime>)returnValue).ResolveToValue();
            }
            return (DateTime)returnValue;
        }

        private static object ParseValue(object valueToParse)
        {
            object returnValue;
            bool isParsed = TryParseValue(valueToParse, out returnValue);

            if (!isParsed)
            {
                RaiseIncorrectTypeException(valueToParse);
            }
            return returnValue;
        }

        /// <summary>
        /// Raises an Erorr if the Incorrect type of property is being set to this BOProp.
        /// </summary>
        /// <param name="value"></param>
        private static void RaiseIncorrectTypeException(object value)
        {
            string message = string.Format("PropRuleDate cannot be set to {0}. It is cannot be converted to a type of a DateTime", value);
            throw new HabaneroIncorrectTypeException(message, message);
        }

        ///<summary>
        /// Tries to parse a value as an object to a valid DateTime.
        /// The valid value may not be a date will always be a date i.e. "Now" will resolve to now.
        /// If you need lazy resolution then use <see cref="TryParseValue"/>.
        ///</summary>
        ///<param name="valueToParse"></param>
        ///<param name="returnValue">valid date if try Parse true else null</param>
        ///<returns>If the value cannot be parsed to a valid date time then returns false else true</returns>
        /// <seealso cref="TryParseValue"/>
        public static bool TryParseDate(object valueToParse, out DateTime? returnValue)
        {
            object tempReturnvalue;
            bool isParsed = TryParseValue(valueToParse, out tempReturnvalue);
            returnValue = null;
            if (isParsed)
            {
                if (tempReturnvalue is IResolvableToValue<DateTime>)
                {
                    returnValue = ((IResolvableToValue<DateTime>)tempReturnvalue).ResolveToValue();
                }
                else if(tempReturnvalue != null)
                {
                    returnValue = (DateTime) tempReturnvalue;
                }
            }

            return isParsed;
        }

        ///<summary>
        /// Tries to parse a value as an object to a valid DateTimeValue or a resolvable DateTimeValue.
        /// The valid value may not be a date but could instead return a <see cref="DateTimeToday"/> or <see cref="DateTimeNow"/> etc
        /// These objects are convertable to DateTime via the <see cref="DateTimeToday.ResolveToValue"/>
        ///</summary>
        ///<param name="valueToParse"></param>
        ///<param name="returnValue"></param>
        ///<returns>If the value cannot be parsed to a valid date time then returns false else true</returns>
        public static bool TryParseValue(object valueToParse, out object returnValue)
        {
            returnValue = null;
            if (valueToParse != null && valueToParse is string && ((string)valueToParse).Length == 0) return true;
            if(valueToParse == null || valueToParse == DBNull.Value) return true;

            if (!(valueToParse is DateTime))
            {
                if (valueToParse is DateTimeToday || valueToParse is DateTimeNow)
                {
                    returnValue = valueToParse;
                    return true;
                }
                if (valueToParse is String)
                {
                    string stringValueToConvert = (string)valueToParse;
                    var stringValueToConvertUpperCase = stringValueToConvert.ToUpper();
                    if (stringValueToConvertUpperCase == "TODAY")
                    {
                        returnValue = new DateTimeToday();
                        return true;
                    }
                    if (stringValueToConvertUpperCase == "YESTERDAY")
                    {
                        returnValue = new DateTimeToday();
                        ((DateTimeToday) returnValue).OffSet = -1;
                        return true;
                    }
                    if (stringValueToConvertUpperCase == "TOMORROW")
                    {
                        returnValue = new DateTimeToday();
                        ((DateTimeToday) returnValue).OffSet = 1;
                        return true;
                    }
                    if (stringValueToConvertUpperCase == "NOW")
                    {
                        returnValue = new DateTimeNow();
                        return true;
                    }
                  
                    DateTime dtOut;
                    if (DateTime.TryParseExact(stringValueToConvert, StandardDateTimeFormat, _dateTimeFormatProvider, DateTimeStyles.AllowWhiteSpaces, out dtOut))
                    {
                        returnValue = dtOut;
                        return true;
                    }
                }
                DateTime dateTimeOut;
                string valueToParseAsString = valueToParse.ToString();
                if (DateTime.TryParse(valueToParseAsString, out dateTimeOut))
                {
                    returnValue = dateTimeOut;
                    return true;
                }
                if (DateTime.TryParse(valueToParseAsString, new DateTimeFormatInfo { FullDateTimePattern = "dd/MM/yyyy HH:mm:ss:fff" }, DateTimeStyles.AllowWhiteSpaces, out dateTimeOut))
                {
                     returnValue = dateTimeOut;
                     return true;
                }
                if (DateTime.TryParse(valueToParseAsString, new DateTimeFormatInfo { FullDateTimePattern = "MM/dd/yyyy HH:mm:ss:fff" }, DateTimeStyles.AllowWhiteSpaces, out dateTimeOut))
                {
                    returnValue = dateTimeOut;
                    return true;
                }
                return false;
            }
            returnValue = valueToParse;
            return true;
        }

        /// <summary>
        /// Determines whether is week day.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns>
        /// 	<c>true</c> if [is week day] ; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekDay(DateTime date)
        {
            return ((date.DayOfWeek != DayOfWeek.Saturday) && (date.DayOfWeek != DayOfWeek.Sunday));
        }
        /// <summary>
        /// Return the <paramref name="date"/> if it is on the <paramref name="day"/> of the week.
        /// Else returns the following day. This i used to iteratively walk through days untill you find the '
        /// day the next day that is the day of the week.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime OnOrNextDayOfWeek(DateTime date, DayOfWeek day)
        {
            while (date.DayOfWeek != day)
                date = date.AddDays(1);
            return date;
        }
        /// <summary>
        /// Returns unchanged if is business day
        /// else subsequent Monday.
        /// Does not take into account public holidays.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns><see cref="T:System.DateTime"/></returns>
        public static DateTime OnOrNextBusinessDay(DateTime date)
        {
            DateTime d = date;
            switch (date.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                    d = date.AddDays(1);
                    break;
                case System.DayOfWeek.Saturday:
                    d = date.AddDays(2);
                    break;
            }
            return d;
        }
        /// <summary>
        /// Return the <paramref name="date"/> if it is on the <paramref name="day"/> of the week.
        /// Else returns the previous day. This i used to iteratively walk through days untill you find
        /// the previous day that is the day of the week.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime OnOrPreviousDayOfWeek(DateTime date, DayOfWeek day)
        {
            while (date.DayOfWeek != day)
                date = date.AddDays(-1);
            return date;
        }
        /// <summary>
        /// Returns unchanged if is business day
        /// else previous  Friday
        /// Does not take into account public holidays.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static DateTime OnOrPreviousBusinessDay(DateTime date)
        {
            DateTime d = date;
            switch (date.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                    d = date.AddDays(-2);
                    break;
                case System.DayOfWeek.Saturday:
                    d = date.AddDays(-1);
                    break;
            }
            return d;
        }
        /// <summary>
        /// Next Business Day (not Sat or Sun)
        /// Does not take into account public holidays.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns><see cref="T:System.DateTime"/></returns>
        public static DateTime NextBusinessDay(DateTime date)
        {
            DateTime d = date;
            switch (date.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                case System.DayOfWeek.Monday:
                case System.DayOfWeek.Tuesday:
                case System.DayOfWeek.Wednesday:
                case System.DayOfWeek.Thursday:
                    d = date.AddDays(1);
                    break;
                case System.DayOfWeek.Friday:
                    d = date.AddDays(3);
                    break;
                case System.DayOfWeek.Saturday:
                    d = date.AddDays(2);
                    break;
            }
            return d;
        }
        /// <summary>
        /// Previous Trade Day (not Sat or Sun)
        /// Does not take into account public holidays.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns><see cref="T:System.DateTime"/></returns>
        public static DateTime PreviousBusinessDay(DateTime date)
        {
            DateTime d = date;
            switch (date.DayOfWeek)
            {
                case System.DayOfWeek.Sunday:
                    d = date.AddDays(-2);
                    break;
                case System.DayOfWeek.Monday:
                    d = date.AddDays(-3);
                    break;
                case System.DayOfWeek.Tuesday:
                case System.DayOfWeek.Wednesday:
                case System.DayOfWeek.Thursday:
                case System.DayOfWeek.Friday:
                case System.DayOfWeek.Saturday:
                    d = date.AddDays(-1);
                    break;
            }
            return d;
        }


        /// <summary>
        /// Converts the given date to the start of the hour
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        public static DateTime HourStart(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0, 0);
        }
        /// <summary>
        /// Converts the given date to the start of the hour
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        public static DateTime HourEnd(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, 59, 59, 999);
        }
        /// <summary>
        /// Converts the given date to the previous midnight,
        /// factoring in the midnight offset
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        public static DateTime DayStart(DateTime date)
        {
            return DayStart(date, new TimeSpan(0));
        }

        /// <summary>
        /// Converts the given date to the previous midnight,
        /// factoring in the midnight offset
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <param name="dayStartOffSet">You can offset the Date Start (typically used when you wan to report on date starting at 8:00 am)</param>
        /// <returns>Returns the converted date</returns>
        public static DateTime DayStart(DateTime date, TimeSpan dayStartOffSet)
        {
            DateTime midnight = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
            DateTime newMidnight = midnight.Add(dayStartOffSet);
            if (newMidnight > date) return newMidnight.AddDays(-1);
            double dif = (date - newMidnight).TotalHours;
            return dif > 24 ? newMidnight.AddDays(1) : newMidnight;
        }
        /// <summary>
        /// Converts the given date to the previous midnight,
        /// factoring in the midnight offset
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        public static DateTime DayEnd(DateTime date)
        {
            return DayEnd(date, new TimeSpan(0));
        }

        /// <summary>
        /// Converts the given date to the previous midnight,
        /// factoring in the midnight offset
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <param name="dayStartOffSet">You can offset the Date Start (typically used when you wan to report on date starting at 8:00 am)</param>
        /// <returns>Returns the converted date</returns>
        public static DateTime DayEnd(DateTime date, TimeSpan dayStartOffSet)
        {
            DateTime dayEnd = DayStart(date, dayStartOffSet).AddDays(1).AddMilliseconds(-1);
            return dayEnd;
        }

        /// <summary>
        /// Converts the given date to the previous Monday midnight,
        /// factoring in the midnight offset and week start
        /// </summary>
        /// <param name="currentDateTime">The date to convert</param>
        /// <param name="weekStartOffSet">The off set period</param>
        /// <returns>Returns the converted date</returns>
        public static DateTime WeekStart(DateTime currentDateTime, TimeSpan weekStartOffSet)
        {
            DateTime sunday = DayStart( OnOrPreviousDayOfWeek(currentDateTime, DayOfWeek.Sunday));
            var weekStart = sunday.Add(weekStartOffSet);
            if (weekStart > currentDateTime)
            {
                return weekStart.AddDays(-7);
            }
            return weekStart;
        }

        ///<summary>
        /// The FirstDay of the currentDateTime
        ///</summary>
        ///<param name="currentDateTime"></param>
        ///<returns></returns>
        public static DateTime MonthStart(DateTime currentDateTime)
        {
            return MonthStart(currentDateTime, new TimeSpan(0));
        }

        ///<summary>
        /// The FirstDay of the currentDateTime taking into a
        ///</summary>
        ///<param name="currentDateTime"></param>
        ///<param name="monthStartOffSet"></param>
        ///<returns></returns>
        public static DateTime MonthStart(DateTime currentDateTime, TimeSpan monthStartOffSet)
        {
            var firstDayOfCurrentMonth = FirstDayOfMonth(currentDateTime).Add(monthStartOffSet);
            var lastDayOfCurrentMonth = LastDayOfTheMonth(currentDateTime).Add(monthStartOffSet);
            if (currentDateTime < firstDayOfCurrentMonth)
            {
                return firstDayOfCurrentMonth.AddMonths(-1);
            }
            if (currentDateTime > lastDayOfCurrentMonth)
            {
                DateTime newMonthStart = currentDateTime.Month == _DECEMBER 
                         ? new DateTime(currentDateTime.Year + 1, 1, 1) 
                         : new DateTime(currentDateTime.Year, currentDateTime.Month + 1, 1);
                return newMonthStart.Add(monthStartOffSet);
            }
            return firstDayOfCurrentMonth;
        }

        ///<summary>
        /// The WeekStart for the currentDateTime where the WeekStart is assumed to be Sunday
        ///</summary>
        ///<param name="currentDateTime"></param>
        ///<returns></returns>
        public static DateTime WeekStart(DateTime currentDateTime)
        {
            return WeekStart(currentDateTime, new TimeSpan(0));
        }
        /// <summary>
        /// The WeekEnd For the currentDateTime where the WeekEnd is assumed to be Saturday
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public static DateTime WeekEnd(DateTime currentDateTime)
        {
            return WeekEnd(currentDateTime, new TimeSpan(0));
        }

        /// <summary>
        /// The WeekEnd For the currentDateTime
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <param name="startOfWeekOffSet">The OffSet</param>
        /// <returns></returns>
        public static DateTime WeekEnd(DateTime currentDateTime, TimeSpan startOfWeekOffSet)
        {
            var weekStart = WeekStart(currentDateTime, startOfWeekOffSet);
            return weekStart.AddDays(7).AddMilliseconds(-1);
        }
        /// <summary>
        /// The MonthEnd For the currentDateTime where the MonthEnd is assumed to be Saturday
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public static DateTime MonthEnd(DateTime currentDateTime)
        {
            return MonthEnd(currentDateTime, new TimeSpan(0));
        }

        /// <summary>
        /// The MonthEnd For the currentDateTime
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <param name="startOfMonthOffSet">The OffSet</param>
        /// <returns></returns>
        public static DateTime MonthEnd(DateTime currentDateTime, TimeSpan startOfMonthOffSet)
        {
            var firstDayOfCurrentMonth = FirstDayOfMonth(currentDateTime).Add(startOfMonthOffSet);
            var lastDayOfCurrentMonth = LastDayOfTheMonth(currentDateTime).Add(startOfMonthOffSet);
            if (currentDateTime < firstDayOfCurrentMonth)
            {
                var currentMonth = currentDateTime.Month;
                DateTime newMonthEnd;
                switch (currentMonth)
                {
                    default:
                        newMonthEnd = new DateTime(currentDateTime.Year, currentMonth, 1);
                        break;
                }
                return newMonthEnd.AddMilliseconds(-1).Add(startOfMonthOffSet);
            }
            if (currentDateTime > lastDayOfCurrentMonth)
            {
                var currentMonth = currentDateTime.Month;
                DateTime newMonthEnd;
                switch (currentMonth)
                {
                    case _DECEMBER:
                        newMonthEnd = new DateTime(currentDateTime.Year + 1, 2, 1);
                        break;
                    case _NOVEMBER:
                        newMonthEnd = new DateTime(currentDateTime.Year + 1, 1, 1);
                        break;
                    default:
                        newMonthEnd = new DateTime(currentDateTime.Year, currentMonth + 2, 1);
                        break;
                }
                return newMonthEnd.AddMilliseconds(-1).Add(startOfMonthOffSet);
            }
            return lastDayOfCurrentMonth;
        }
/*


        

        /// <summary>
        /// Converts the given date to the first day of the month,
        /// factoring in the month start and midnight offset
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        private DateTime MonthStart(DateTime date)
        {
            DateTime first = new DateTime(date.Year, date.Month, 1);
            first = first.AddDays(MonthStartOffset).Add(MidnightOffset);
            if (first > date)
            {
                first = first.AddMonths(-1);
            }
            if (MidnightOffset < new TimeSpan(0, 0, 0, 0, 0))
            {
                DateTime closer = new DateTime(date.Year, date.Month + 1, 1);
                closer = closer.AddDays(MonthStartOffset).Add(MidnightOffset);
                if (closer < date) return closer;
            }
            return first;
        }

        /// <summary>
        /// Converts the given date to the first day of the year,
        /// factoring in the year start, month start and midnight offset
        /// </summary>
        /// <param name="date">The date to convert</param>
        /// <returns>Returns the converted date</returns>
        private DateTime YearStart(DateTime date)
        {
            DateTime first = new DateTime(date.Year, 1, 1);
            first = first.AddMonths(YearStartOffset).AddDays(MonthStartOffset).Add(MidnightOffset);
            if (first > date)
            {
                first = first.AddYears(-1);
            }
            if (MidnightOffset < new TimeSpan(0, 0, 0, 0, 0))
            {
                DateTime closer = new DateTime(date.Year + 1, 1, 1);
                closer = closer.AddMonths(YearStartOffset).AddDays(MonthStartOffset).Add(MidnightOffset);
                if (closer < date) return closer;
            }
            return first;
        }*/

        ///<summary>
        /// The Very first second of this year.
        ///</summary>
        ///<param name="currentDateTime"></param>
        ///<returns></returns>
        public static DateTime YearStart(DateTime currentDateTime)
        {
            return YearStart(currentDateTime, 0);
        }

        ///<summary>
        /// The very first second of this year plus the offset. This is typically used 
        /// for Financial Year type calculations e.g. when you are 
        /// looking for a year starting on 01 March use a noOfMonthsOffSet = 2.
        ///</summary>
        ///<param name="currentDateTime">the currentDate from which the YearStart should be calculated</param>
        ///<param name="noOfMonthsOffSet">The noOfMonthsOffSet used to </param>
        ///<returns></returns>
        public static DateTime YearStart(DateTime currentDateTime, int noOfMonthsOffSet)
        {
            return YearStart(currentDateTime, noOfMonthsOffSet, new TimeSpan(0));
        }

        ///<summary>
        /// The very first second of this year plus the month offset plus the Time offset. This is typically used 
        /// for Financial Year type calculations e.g. when you are 
        /// looking for a year starting on 01 March use a noOfMonthsOffSet = 2.
        ///</summary>
        ///<param name="currentDateTime">the currentDate from which the YearStart should be calculated</param>
        ///<param name="noOfMonthsOffSet">The noOfMonthsOffSet used to </param>
        ///<param name="monthStartOffSet"></param>
        ///<returns></returns>
        public static DateTime YearStart(DateTime currentDateTime, int noOfMonthsOffSet, TimeSpan monthStartOffSet)
        {
            var yearStartWithoutOffSet = new DateTime(currentDateTime.Year, 1, 1);
            var yearStartWithOffSet = yearStartWithoutOffSet.AddMonths(noOfMonthsOffSet).Add(monthStartOffSet);
            var yearEndWithoutOFfset = new DateTime(currentDateTime.Year, 12, 31);
            var yearEndWithOffSet = yearEndWithoutOFfset.AddMonths(noOfMonthsOffSet).Add(monthStartOffSet);
            if (currentDateTime < yearStartWithOffSet)
            {
                return yearStartWithOffSet.AddYears(-1);
            }
            if (currentDateTime > yearEndWithOffSet)
            {
                return yearStartWithOffSet.AddYears(1);
            }
            return yearStartWithOffSet;
        }

        /// <summary>
        /// The YearEnd For the currentDateTime 
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public static DateTime YearEnd(DateTime currentDateTime)
        {
            return YearEnd(currentDateTime, 0);
        }

        /// <summary>
        /// The YearEnd For the currentDateTime
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <param name="noOfMonthsOffSet">The OffSet</param>
        /// <returns></returns>
        public static DateTime YearEnd(DateTime currentDateTime, int noOfMonthsOffSet)
        {
            return YearEnd(currentDateTime, noOfMonthsOffSet, new TimeSpan(0));
        }

        /// <summary>
        /// The YearEnd For the currentDateTime
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <param name="noOfMonthsOffSet">The number of moths that are OffSet i.e. offset = 3 will result in year start 1 Apr and Year End 31 Mar</param>
        /// <param name="monthStartOffSet"></param>
        /// <returns></returns>
        public static DateTime YearEnd(DateTime currentDateTime, int noOfMonthsOffSet, TimeSpan monthStartOffSet)
        {
            DateTime lastDayOfYear = YearStart(currentDateTime, noOfMonthsOffSet, monthStartOffSet).AddYears(1).AddMilliseconds(-1);
            return lastDayOfYear;
        }
    }
}