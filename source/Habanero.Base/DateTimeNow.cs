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
using System.ComponentModel;

namespace Habanero.Base
{
    ///<summary>
    /// This is a wrapper class for DateTime.Now. This is used by search and filter criteria to build up a search Criteria object.
    /// For loading the appropriate objects from the collection.
    ///</summary>
    [TypeConverter(typeof(DateTimeNowConverter))]
    public class DateTimeNow : IComparable<DateTime>, IComparable, IResolvableToValue, IResolvableToValue<DateTime>
    {
        ///<summary>
        /// Returns the current Today value from the DateTime object.
        ///</summary>
        public static DateTime Value
        {
            get { return DateTime.Now; }
        }

        ///<summary>
        ///Compares the current instance with another object of the same type.
        ///</summary>
        ///<returns>
        ///A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than obj. Zero This instance is equal to obj. Greater than zero This instance is greater than obj. 
        ///</returns>
        ///<param name="obj">An object to compare with this instance. </param>
        ///<exception cref="T:System.ArgumentException">obj is not the same type as this instance. </exception><filterpriority>2</filterpriority>
        public int CompareTo(object obj)
        {
            if (obj is DateTimeNow) return 0;
            return Value.CompareTo(obj);
        }

        ///<summary>
        ///Compares the current object with another object of the same type.
        ///</summary>
        ///<returns>
        ///A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other. 
        ///</returns>
        ///<param name="other">An object to compare with this object.</param>
        public int CompareTo(DateTime other)
        {
            return Value.CompareTo(other);
        }

        ///<summary>
        /// Resolved the instance class to a value of type DateTime.
        ///</summary>
        ///<returns>The value that the instance class is resolved to.</returns>
        public virtual DateTime ResolveToValue()
        {
            return Value;
        }

        ///<summary>
        /// Returns a ToString of the Value (Today).
        ///</summary>
        ///<returns></returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            return obj is DateTimeNow;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return 0;
        }

        ///<summary>
        /// Resolved the instance class to a value.
        ///</summary>
        ///<returns>The value that the instance class is resolved to.</returns>
        object IResolvableToValue<object>.ResolveToValue()
        {
            return Value;
        }


        //public static bool operator ==(DateTimeToday left, DateTimeToday right)
        //{
        //    return Equals(left, right);
        //}

        //public static bool operator !=(DateTimeToday left, DateTimeToday right)
        //{
        //    return !Equals(left, right);
        //}

    }
    
    ///<summary>
    /// This is a wrapper class for DateTime.Now when you have a fixed DateTimeNow.
    ///</summary>
    public class DateTimeNowFixed : DateTimeNow
    {
        private readonly DateTime _dateTimeNow;

        ///<summary>
        /// Constructs DateTimeNow with a fixed DateTime.
        ///</summary>
        ///<param name="dateTimeNow"></param>
        public DateTimeNowFixed(DateTime dateTimeNow)
        {
            _dateTimeNow = dateTimeNow;
        }

        public override DateTime ResolveToValue()
        {
            return _dateTimeNow;
        }
    }
}