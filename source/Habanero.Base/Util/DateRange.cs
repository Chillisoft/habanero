#region Licensing Header
// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2011 Chillisoft Solutions
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
#endregion
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
        /// <summary>
        /// Constructs range with StartDate - DateTime.Min and EndDate DateTime.Max.
        /// </summary>
        public DateRange() : this(DateTime.MinValue, DateTime.MaxValue)
        {
        }
        /// <summary>
        /// Constructs DateRange with specified start and end date.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
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

        private static void AssertStartDateFollowsEndDate(DateTime? startDate,
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
    /// Date Range Class originally from http://noticeablydifferent.com/CodeSamples/DateRange.aspx
    /// </summary>
    public class DateRangeComparerByStartDate : System.Collections.IComparer,
                                                System.Collections.Generic.IComparer<DateRange>
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero <paramref name="x"/> is less than <paramref name="y"/>. Zero <paramref name="x"/> equals <paramref name="y"/>. Greater than zero <paramref name="x"/> is greater than <paramref name="y"/>. 
        /// </returns>
        /// <param name="x">The first object to compare. </param><param name="y">The second object to compare. </param><exception cref="T:System.ArgumentException">Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.-or- <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other. </exception><filterpriority>2</filterpriority>
        public int Compare(object x, object y)
        {
            if (!(x is DateRange) || !(y is DateRange))
                throw new System.ArgumentException("Value not a DateRange");
            return Compare((DateRange)x, (DateRange)y);
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>, as shown in the following table.Value Meaning Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first object to compare.</param><param name="y">The second object to compare.</param>
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