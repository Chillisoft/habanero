//  ---------------------------------------------------------------------------------
//   Copyright (C) 2007-2010 Chillisoft Solutions
//   
//   This file is part of the Habanero framework.
//   
//       Habanero is a free framework: you can redistribute it and/or modify
//       it under the terms of the GNU Lesser General Public License as published by
//       the Free Software Foundation, either version 3 of the License, or
//       (at your option) any later version.
//   
//       The Habanero framework is distributed in the hope that it will be useful,
//       but WITHOUT ANY WARRANTY; without even the implied warranty of
//       MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//       GNU Lesser General Public License for more details.
//   
//       You should have received a copy of the GNU Lesser General Public License
//       along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//  ---------------------------------------------------------------------------------
using System;
using Habanero.Util;

namespace Habanero.Base.Util
{
    ///<summary>
    /// 
    ///</summary>
    public class DateRangeOptionsConverter
    {
        private DateTimeNow _dateTimeNow;

        ///<summary>
        /// Constructs A DateRangeOptions Converter with a default DateTimeNow.
        ///</summary>
        public DateRangeOptionsConverter():this(new DateTimeNow())
        {
        }
        /// <summary>
        /// Sets a fixed date time to use as now.
        /// </summary>
        /// <param name="now">the Fixed DateTime to use as now in all calculations</param>
        public void SetNow(DateTime now)
        {
            _dateTimeNow = new DateTimeNowFixed(now);
        }
        ///<summary>
        /// Constructs A DateRangeOptions Converter with a specified DateTimeNow.
        /// This is primarily used for testing so that a Fake DateTimeNow can be used.
        ///</summary>
        public DateRangeOptionsConverter(DateTimeNow dateTimeNow)
        {
            if (dateTimeNow == null) throw new ArgumentNullException("dateTimeNow");
            _dateTimeNow = dateTimeNow;
        }

        private TimeSpan _midnightOffset;
        /// <summary>
        /// The offset used to determine the start of the day. E.g. if the day start is assumed to be
        /// 08h00 then this will be set to a timespan of 8 hrs. NNB this means that an event occuring at 7:59 Today will be seen as Yesterday
        /// </summary>
        public TimeSpan MidnightOffset
        {
            get { return _midnightOffset; }
            set
            {
                if (Math.Abs(value.TotalHours) >= 24)
                {
                    throw new ArgumentException("A midnight offset must be " +
                        "less than 24 hours.");
                }
                _midnightOffset = value;
            }
        }

        /// <summary>
        /// An off set for the week start. By Defaut the week start is Sunday Morning. But for certain applications
        /// you may require a week start on Monday. You Should then set the WeekStartOffSet to 1.
        /// </summary>
        public int WeekStartOffset { get; set; }

        private int _monthStartOffset;
        /// <summary>
        /// Gets and sets the number of days to add or subtract from
        /// the first day of the month in order to adjust which day
        /// is typically the first of the month.  If the 5th is the typical start
        /// of a new month for the given application,
        /// then this property can be set to 4 (1+4=5).
        /// </summary>
        public int MonthStartOffset
        {
            get { return _monthStartOffset; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Month start offset cannot be negative.");
                }
                _monthStartOffset = value;
            }
        }

        private int _yearStartOffset;

        /// <summary>
        /// Gets and sets the number of months to add or subtract from
        /// January to redefine the first month of the year.  For example,
        /// if March is the first month of the new year for the given application,
        /// then this property can be set with 2 (1+2=3).
        /// </summary>
        public int YearStartOffset
        {
            get { return _yearStartOffset; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Year start offset cannot be negative.");
                }
                _yearStartOffset = value;
            }
        }

        ///<summary>
        /// Using the Current Date Time <see cref="DateTimeNow"/> and the <see cref="DateRangeOptions"/>
        /// returns an appropriate <see cref="DateRange"/>.
        ///</summary>
        ///<param name="dateRangeOptions">A date range option e.g. Yesterday</param>
        ///<returns></returns>
        public DateRange ConvertDateRange(DateRangeOptions dateRangeOptions)
        {
            var currentDateTime = _dateTimeNow.ResolveToValue();

            switch (dateRangeOptions)
            {
                case DateRangeOptions.ThisHour:
                    {
                        return new DateRange(HourStart(currentDateTime), HourEnd(currentDateTime));
                    }
                case DateRangeOptions.Current24Hours:
                    {
                        return new DateRange(currentDateTime.AddDays(-1), currentDateTime);
                    }
                case DateRangeOptions.PreviousHour:
                    {
                        var previousHour = currentDateTime.AddHours(-1);
                        return new DateRange(HourStart(previousHour), HourEnd(previousHour));
                    }
                case DateRangeOptions.Current60Minutes:
                    {
                        return new DateRange(currentDateTime.AddHours(-1), currentDateTime);
                    }
                case DateRangeOptions.Today:
                    {
                        return new DateRange(DayStart(currentDateTime), DayEnd(currentDateTime));
                    }
                case DateRangeOptions.Yesterday:
                    {
                        return new DateRange(DayStart(currentDateTime).AddDays(-1), DayEnd(currentDateTime).AddDays(-1));
                    }
                case DateRangeOptions.ThisWeek:
                    {
                        return new DateRange(WeekStart(currentDateTime), WeekEnd(currentDateTime));
                    }
                case DateRangeOptions.PreviousWeek:
                    {
                        var lastWeekDate = currentDateTime.AddDays(-7);
                        return new DateRange(WeekStart(lastWeekDate), WeekEnd(lastWeekDate));
                    }
                case DateRangeOptions.Previous7Days:
                    {
                        return new DateRange(DayStart(currentDateTime.AddDays(-7)), currentDateTime);
                    }
                case DateRangeOptions.MonthToDate:
                    {
                        return new DateRange(MonthStart(currentDateTime), currentDateTime);
                    }
                case DateRangeOptions.WeekToDate:
                    {
                        return new DateRange(WeekStart(currentDateTime), currentDateTime);
                    }

                case DateRangeOptions.ThisMonth:
                    {
                        return new DateRange(MonthStart(currentDateTime), MonthEnd(currentDateTime));
                    }
                case DateRangeOptions.PreviousMonth:
                    {
                        var previousMonthDate = currentDateTime.AddMonths(-1);
                        return new DateRange(MonthStart(previousMonthDate), MonthEnd(previousMonthDate));
                    }
                case DateRangeOptions.Previous30Days:
                    {
                        var startTime = currentDateTime.AddDays(-30);
                        var endTime = currentDateTime;
                        return new DateRange(DayStart(startTime), endTime);
                    }
                case DateRangeOptions.Previous31Days:
                    {
                        var startTime = currentDateTime.AddDays(-31);
                        var endTime = currentDateTime;
                        return new DateRange(DayStart(startTime), endTime);
                    }
                case DateRangeOptions.YearToDate:
                    {
                        return new DateRange(YearStart(currentDateTime), currentDateTime);
                    }
                case DateRangeOptions.Previous365Days:
                    {
                        return new DateRange(DayStart(currentDateTime.AddDays(-365)), currentDateTime);
                    }

                case DateRangeOptions.Current2Years:
                    {
                        return new DateRange(currentDateTime.AddYears(-2), currentDateTime);
                    }
                case DateRangeOptions.Current3Years:
                    {
                        return new DateRange(currentDateTime.AddYears(-3), currentDateTime);
                    }
                case DateRangeOptions.Current5Years:
                    {
                        return new DateRange(currentDateTime.AddYears(-5), currentDateTime);
                    }

                case DateRangeOptions.ThisYear:
                    {
                        return new DateRange(YearStart(currentDateTime), YearEnd(currentDateTime));
                    }
                case DateRangeOptions.PreviousYear:
                    {
                        DateTime previousYear = currentDateTime.AddYears(-1);
                        return new DateRange(YearStart(previousYear), YearEnd(previousYear));
                    }

                case DateRangeOptions.Previous2Years:
                    {
                        DateTime twoYearsAgo = currentDateTime.AddYears(-2);
                        return new DateRange(YearStart(twoYearsAgo), YearEnd(twoYearsAgo.AddYears(1)));
                    }

                case DateRangeOptions.Previous3Years:
                    {
                        DateTime threeYearsAgo = currentDateTime.AddYears(-3);
                        return new DateRange(YearStart(threeYearsAgo), YearEnd(threeYearsAgo).AddYears(2));
                    }
                case DateRangeOptions.Previous5Years:
                    {
                        DateTime fiveYearsAgo = currentDateTime.AddYears(-5);
                        return new DateRange(YearStart(fiveYearsAgo), YearEnd(fiveYearsAgo).AddYears(4));
                    }
                case DateRangeOptions.Tommorrow:
                    {
                        return new DateRange(DayStart(currentDateTime.AddDays(1)), DayEnd(currentDateTime.AddDays(1)));
                    }
                case DateRangeOptions.Next24Hours:
                    {
                        return new DateRange(currentDateTime, currentDateTime.AddHours(24).AddMilliseconds(-1));
                    }
                case DateRangeOptions.Next7Days:
                    {
                        return new DateRange(currentDateTime, DayEnd(currentDateTime.AddDays(7)));
                    }
                case DateRangeOptions.Next30Days:
                    {
                        return new DateRange(currentDateTime, DayEnd(currentDateTime.AddDays(30)));
                    }
                default:
                    {
                        return new DateRange(DateTime.MinValue, DateTime.MaxValue);
                    }
            } 
        }

        private DateTime YearEnd(DateTime currentDateTime)
        {
            return DateTimeUtilities.YearEnd(currentDateTime, this.YearStartOffset, TotalMonthStartOffSet());
        }

        private DateTime YearStart(DateTime currentDateTime)
        {
            return DateTimeUtilities.YearStart(currentDateTime, this.YearStartOffset, TotalMonthStartOffSet());
        }

        private DateTime MonthEnd(DateTime previousMonthDate)
        {
            return DateTimeUtilities.MonthEnd(previousMonthDate, this.TotalMonthStartOffSet());
        }

        private DateTime WeekEnd(DateTime currentDateTime)
        {
            return DateTimeUtilities.WeekEnd(currentDateTime, TotalWeekStartOffSet());
        }

        private DateTime WeekStart(DateTime currentDateTime)
        {
            return DateTimeUtilities.WeekStart(currentDateTime, TotalWeekStartOffSet());
        }
        /// <summary>
        /// Sum of Midnight offset and WeekStartOffSet
        /// </summary>
        /// <returns></returns>
        private TimeSpan TotalWeekStartOffSet()
        {
            return this.MidnightOffset.Add(new TimeSpan(this.WeekStartOffset,0,0,0));
        }
        /// <summary>
        /// Sum of Midnight offset and MonthStartOffSet
        /// </summary>
        /// <returns></returns>
        private TimeSpan TotalMonthStartOffSet()
        {
            return this.MidnightOffset.Add(new TimeSpan(this.MonthStartOffset,0,0,0));
        }

        private DateTime DayEnd(DateTime currentDateTime)
        {
            return DateTimeUtilities.DayEnd(currentDateTime, this.MidnightOffset);
        }

        private DateTime DayStart(DateTime currentDateTime)
        {
            return DateTimeUtilities.DayStart(currentDateTime, this.MidnightOffset);
        }

        private DateTime HourEnd(DateTime currentDateTime)
        {
            return DateTimeUtilities.HourEnd(currentDateTime);
        }

        private DateTime HourStart(DateTime currentDateTime)
        {
            return DateTimeUtilities.HourStart(currentDateTime);
        }
        private DateTime MonthStart(DateTime currentDateTime)
        {
            return DateTimeUtilities.MonthStart(currentDateTime, this.TotalMonthStartOffSet());
        }
    }
}