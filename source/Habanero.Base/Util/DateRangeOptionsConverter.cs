using System;
using Habanero.Util;

namespace Habanero.Base.Util
{
    ///<summary>
    /// 
    ///</summary>
    public class DateRangeOptionsConverter
    {
        private readonly DateTimeNow _dateTimeNow;

        public DateRangeOptionsConverter():this(new DateTimeNow())
        {
        }
        public DateRangeOptionsConverter(DateTimeNow dateTimeNow)
        {
            if (dateTimeNow == null) throw new ArgumentNullException("dateTimeNow");
            _dateTimeNow = dateTimeNow;
        }

        public DateRange ConvertDateRange(DateRangeOptions dateRange)
        {
            var currentDateTime = _dateTimeNow.ResolveToValue();

            switch (dateRange)
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
                        return new DateRange(DayStart(currentDateTime),DayEnd(currentDateTime));
                    }
                case DateRangeOptions.Yesterday:
                    {
                        return new DateRange(DayStart(currentDateTime).AddDays(-1), DayEnd(currentDateTime).AddDays(-1));
                    }
                case DateRangeOptions.ThisWeek:
                    {
                        return new DateRange(WeekStart(currentDateTime), WeekEnd(currentDateTime));
                    }
            }
            var range = new DateRange();
            
            /*          
                          


                          case DateRangeOptions.Yesterday:
                              {
                                  _startDate = DayStart(Now.AddDays(-1));
                                  _endDate = DayStart(Now);
                                  break;
                              }
         
                          case DateRangeOptions.WeekToDate:
                              {
                                  _startDate = WeekStart(Now);
                                  _endDate = Now;
                                  break;
                              }
                          case DateRangeOptions.PreviousWeek:
                              {
                                  _startDate = WeekStart(Now).AddDays(-7);
                                  _endDate = WeekStart(Now);
                                  break;
                              }
                          case DateRangeOptions.Previous7Days:
                              {
                                  _startDate = DayStart(Now).AddDays(-7);
                                  _endDate = DayStart(Now);
                                  break;
                              }
                          case DateRangeOptions.MonthToDate:
                              {
                                  _startDate = MonthStart(Now);
                                  _endDate = Now;
                                  break;
                              }
                          case DateRangeOptions.PreviousMonth:
                              {
                                  _startDate = MonthStart(Now).AddMonths(-1);
                                  _endDate = MonthStart(Now);
                                  break;
                              }
                          case DateRangeOptions.Previous30Days:
                              {
                                  _startDate = DayStart(Now).AddDays(-30);
                                  _endDate = DayStart(Now);
                                  break;
                              }
                          case DateRangeOptions.Previous31Days:
                              {
                                  _startDate = DayStart(Now).AddDays(-31);
                                  _endDate = DayStart(Now);
                                  break;
                              }
                          case DateRangeOptions.YearToDate:
                              {
                                  _startDate = YearStart(Now);
                                  _endDate = Now;
                                  break;
                              }                
                          case DateRangeOptions.ThisYear:
                              {
                                  _startDate = YearStart(Now);
                                  _endDate = YearStart(Now).AddYears(1);
                                  break;
                              }
                          case DateRangeOptions.PreviousYear:
                              {
                                  _startDate = YearStart(Now).AddYears(-1);
                                  _endDate = YearStart(Now);
                                  break;
                              }
                          case DateRangeOptions.Previous365Days:
                              {
                                  _startDate = DayStart(Now).AddDays(-365);
                                  _endDate = DayStart(Now);
                                  break;
                              }
                          case DateRangeOptions.Current2Years:
                              {
                                  _startDate = Now.AddYears(-2);
                                  _endDate = Now;
                                  break;
                              }
                          case DateRangeOptions.Current3Years:
                              {
                                  _startDate = Now.AddYears(-3);
                                  _endDate = Now;
                                  break;
                              }
                          case DateRangeOptions.Current5Years:
                              {
                                  _startDate = Now.AddYears(-5);
                                  _endDate = Now;
                                  break;
                              }
                          case DateRangeOptions.Previous2Years:
                              {
                                  _startDate = YearStart(Now).AddYears(-2);
                                  _endDate = YearStart(Now);
                                  break;
                              }
                          case DateRangeOptions.Previous3Years:
                              {
                                  _startDate = YearStart(Now).AddYears(-3);
                                  _endDate = YearStart(Now);
                                  break;
                              }
                          case DateRangeOptions.Previous5Years:
                              {
                                  _startDate = YearStart(Now).AddYears(-5);
                                  _endDate = YearStart(Now);
                                  break;
                              }
                          default:
                              {
                                  _startDate = DateTime.MinValue;
                                  _endDate = DateTime.MaxValue;
                                  break;
                              }
                   }*/
                      return range; return range;
        }

        private DateTime WeekEnd(DateTime currentDateTime)
        {
            return DateTimeUtilities.WeekEnd(currentDateTime);
        }

        private DateTime WeekStart(DateTime currentDateTime)
        {
            return DateTimeUtilities.WeekStart(currentDateTime);
        }

        private DateTime DayEnd(DateTime currentDateTime)
        {
            return DateTimeUtilities.DayEnd(currentDateTime);
        }

        private DateTime DayStart(DateTime currentDateTime)
        {
            return DateTimeUtilities.DayStart(currentDateTime);
        }

        private DateTime HourEnd(DateTime currentDateTime)
        {
            return DateTimeUtilities.HourEnd(currentDateTime);
        }

        private DateTime HourStart(DateTime currentDateTime)
        {
            return DateTimeUtilities.HourStart(currentDateTime);
        }
    }
}