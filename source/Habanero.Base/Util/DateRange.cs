using System;

namespace Habanero.Base.Util
{
    
    /// <summary>
    /// Date Range Class originally from http://noticeablydifferent.com/CodeSamples/DateRange.aspx
    /// </summary>
    [CoverageExclude(ExcludeReason = "This code was downloaded from a website and did not have any tests.")]
    public class DateRange : IEquatable<DateRange>
    {
        private DateTime _startDate, _endDate;

        public DateRange() : this(DateTime.MinValue, DateTime.MaxValue)
        {
        }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            AssertStartDateFollowsEndDate(startDate, endDate);
            this._startDate = startDate;
            this._endDate = endDate;
        }

        ///<summary>
        /// The Timespan between StartDate and End Date.
        ///</summary>
        public TimeSpan TimeSpan
        {
            get { return _endDate - _startDate; }
        }
        /// <summary>
        /// The Start date of the Date Range.
        /// </summary>
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                AssertStartDateFollowsEndDate(value, this._endDate);
                _startDate = value;
            }
        }
        /// <summary>
        /// The End Date of the Date Range.
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                AssertStartDateFollowsEndDate(this._startDate, value);
                _endDate = value;
            }
        }

        private void AssertStartDateFollowsEndDate(DateTime? startDate,
                                                   DateTime? endDate)
        {
            if ((startDate.HasValue && endDate.HasValue) &&
                (endDate.Value < startDate.Value))
                throw new InvalidOperationException("Start Date must be less than or equal to End Date");
        }
/*        /// <summary>
        /// The intersection between the two date ranges.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public DateRange GetIntersection(DateRange other)
        {
            if (!Intersects(other)) throw new InvalidOperationException("DateRanges do not intersect");
            return new DateRange(GetLaterStartDate(other.StartDate), GetEarlierEndDate(other.EndDate));
        }

        private DateTime GetLaterStartDate(DateTime other)
        {
            return Nullable.Compare<DateTime>(_startDate, other) >= 0 ? _startDate : other;
        }

        private DateTime GetEarlierEndDate(DateTime other)
        {
            //!endDate.HasValue == +infinity, not negative infinity
            //as is the case with !startDate.HasValue
            if (Nullable.Compare<DateTime>(_endDate, other) == 0) return other;
            return (Nullable.Compare<DateTime>(_endDate, other) >= 0) ? other : _endDate;
        }
       /// <summary>
        /// Returns true if any part of the two date ranges intersect.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Intersects(DateRange other)
        {
            return !(
                 (other.EndDate < this.StartDate) ||
                 (other.StartDate > this.EndDate) ||
                 (this.EndDate < other.StartDate) ||
                 (this.StartDate > other.EndDate));

        }*/
        /// <summary>
        /// Returns true if the two date ranges have exactly the same start and end date.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DateRange other)
        {
            if (object.ReferenceEquals(other, null)) return false;
            return ((_startDate == other.StartDate) && (_endDate == other.EndDate));
        }
    }

    /// <summary>
    /// Date Range Class originally from <see cref="http://noticeablydifferent.com/CodeSamples/DateRange.aspx"/>
    /// </summary>
    public class DateRangeComparerByStartDate : System.Collections.IComparer,
                                                System.Collections.Generic.IComparer<DateRange>
    {
        public int Compare(object x, object y)
        {
            if (!(x is DateRange) || !(y is DateRange))
                throw new System.ArgumentException("Value not a DateRange");
            return Compare((DateRange)x, (DateRange)y);
        }

        public int Compare(DateRange x, DateRange y)
        {
            if (x.StartDate < y.StartDate)
            {
                return -1;
            }
            if (x.StartDate > y.StartDate)
            {
                return 1;
            }
            return 0;
        }
    }
}