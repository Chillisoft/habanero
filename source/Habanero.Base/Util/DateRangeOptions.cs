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
using System.Collections.Generic;
using System.Text;

namespace Habanero.Base.Util
{

    /// <summary>
    /// Provides options that can be added to or removed from the
    /// DateRangeComboBox
    /// </summary>
    public enum DateRangeOptions
    {
        /// <summary>
        /// The period from the start of this hour until End of this hour
        /// </summary>
        ThisHour,
        /// <summary>
        /// The period covering the previous hour before the start of the
        /// current one
        /// </summary>
        PreviousHour,
        /// <summary>
        /// The period covering the previous 60 minutes before this one
        /// </summary>
        Current60Minutes,
        /// <summary>
        /// The period from the start of the day until end of Today
        /// </summary>
        Today,
        /// <summary>
        /// The period from the start of the previous day till
        /// the end of the previous day
        /// </summary>
        Yesterday,
        /// <summary>
        /// The period covering the last 24 hours up till now
        /// </summary>
        Current24Hours,
        /// <summary>
        /// The period from the first day of the week until now
        /// </summary>
        WeekToDate,
        /// <summary>
        /// The period from the first day of the previous week
        /// until the last day of the previous week
        /// </summary>
        PreviousWeek,
        /// <summary>
        /// The previous seven days, excluding today
        /// </summary>
        Previous7Days,
        /// <summary>
        /// The period from the start of the month until now
        /// </summary>
        MonthToDate,
        /// <summary>
        /// The period covering the previous month
        /// </summary>
        PreviousMonth,
        /// <summary>
        /// The period covering the previous 30 days, excluding today
        /// </summary>
        Previous30Days,
        /// <summary>
        /// The period covering the previous 31 days, excluding today
        /// </summary>
        Previous31Days,
        /// <summary>
        /// The period covering the start of the year until now
        /// </summary>
        YearToDate,
        /// <summary>
        /// The period covering the current year (Jan 1 to Dec 31)
        /// </summary>
        ThisYear,
        /// <summary>
        /// The period covering the previous year
        /// </summary>
        PreviousYear,
        /// <summary>
        /// The period covering the last 365 days, excluding today
        /// </summary>
        Previous365Days,
        /// <summary>
        /// The period covering the 2 years up till now
        /// </summary>
        Current2Years,
        /// <summary>
        /// The period covering the 3 years up till now
        /// </summary>
        Current3Years,
        /// <summary>
        /// The period covering the 5 years up till now
        /// </summary>
        Current5Years,
        /// <summary>
        /// The period covering the 2 years before the current one
        /// </summary>
        Previous2Years,
        /// <summary>
        /// The period covering the 3 years before the current one
        /// </summary>
        Previous3Years,
        /// <summary>
        /// The period covering the 5 years before the current one
        /// </summary>
        Previous5Years,

        /// <summary>
        /// The period from the start of the next day till
        /// the end of the next day
        /// </summary>
        Tommorrow,
        /// <summary>
        /// The period covering the next 24 hours from now
        /// </summary>
        Next24Hours,
        /// <summary>
        /// The period from the first day of the week until the last day of the week. (Week Starts on Sunday 00:00:00)
        /// </summary>
        ThisWeek,
        /// <summary>
        /// The period from the first day of the next week
        /// until the last day of the next week. (Week Starts on Sunday 00:00:00)
        /// </summary>
        Next7Days,
        /// <summary>
        /// The period from the start of this month until end of this month.
        /// </summary>
        ThisMonth,
        /// <summary>
        /// The period covering the next 30 days, including today
        /// </summary>
        Next30Days,
    }

}
