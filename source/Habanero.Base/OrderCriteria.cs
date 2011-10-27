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
using System.Collections;
using System.Collections.Generic;

namespace Habanero.Base
{
    /// <summary>
    /// Represents a set of order criteria used when loading collections of BusinessObjects.
    /// </summary>
    public interface IOrderCriteria : IComparer {
        /// <summary>
        /// The fields that will be ordered on. See <see cref="IOrderCriteriaField"/>
        /// </summary>
        List<IOrderCriteriaField> Fields { get; }

        /// <summary>
        /// Add a field to the fields to be sorted on. This is a convenience method that will created a Field and add it
        /// to the Fields list with the Ascending option.
        /// </summary>
        /// <param name="field">The name of the field to be sorted on</param>
        /// <returns>This OrderCriteria, to allow chaining of adds (eg:  myOrderCriteria.Add("Name").Add("Age"))</returns>
        IOrderCriteria Add(string field);

        /// <summary>
        /// Add a field to the fields to be sorted on. This is a convenience method that will created a Field and add it
        /// to the Fields list. </summary>
        /// <param name="field">The name of the field to be sorted on</param>
        /// <param name="sortDirection">The sort direction of this field</param>
        /// <returns>This OrderCriteria, to allow chaining of adds</returns>
        IOrderCriteria Add(string field, SortDirection sortDirection);

        /// <summary>
        /// Adds a field to the fields to be sorted on.
        /// </summary>
        /// <param name="orderCriteriaField">The <see cref="OrderCriteriaField"/> to add to the OrderCriteria's field collection</param>
        /// <returns>This OrderCriteria, to allow chaining of adds</returns>
        IOrderCriteria Add(IOrderCriteriaField orderCriteriaField);

        /// <summary>
        /// Compares two BusinessObjects using the criteria set up in this OrderCriteria object.
        /// </summary>
        /// <typeparam name="T">The Type to be compared</typeparam>
        /// <param name="bo1">The first object to be used in the comparison</param>
        /// <param name="bo2">The second object to be used in the comparison</param>
        /// <returns>a value less than 0 if bo1 is less than bo2, 0 if bo1 = bo2 and greater than 0 if b01 is greater than bo2
        /// The value returned is negated if the SortDirection is Descending
        /// </returns>
        int Compare<T>(T bo1, T bo2) where T:IBusinessObject;


    }

    /// <summary>
    /// Defines different sort direction options - the only ones being Ascending or Descending
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Sort in ascending order (lowest to highest, a to z)
        /// </summary>
        Ascending,
        /// <summary>
        /// Sort in descending order (highest to lowest, z to a)
        /// </summary>
        Descending
    }

    /// <summary>
    /// Represents a set of order criteria used when loading collections of BusinessObjects.
    /// </summary>
    public class OrderCriteria :  IOrderCriteria
    {
        private readonly List<IOrderCriteriaField> _fields = new List<IOrderCriteriaField>();



        /// <summary>
        /// The fields that will be ordered on. See <see cref="OrderCriteriaField"/>
        /// </summary>
        public List<IOrderCriteriaField> Fields
        {
            get
            {
                return _fields;
            }
        }

        /// <summary>
        /// Add a field to the fields to be sorted on. This is a convenience method that will created a Field and add it
        /// to the Fields list with the Ascending option.
        /// </summary>
        /// <param name="field">The name of the field to be sorted on</param>
        /// <returns>This OrderCriteria, to allow chaining of adds (eg:  myOrderCriteria.Add("Name").Add("Age"))</returns>
        public IOrderCriteria Add(string field)
        {
            return Add(field, SortDirection.Ascending);
        }

        /// <summary>
        /// Add a field to the fields to be sorted on. This is a convenience method that will created a Field and add it
        /// to the Fields list. </summary>
        /// <param name="field">The name of the field to be sorted on</param>
        /// <param name="sortDirection">The sort direction of this field</param>
        /// <returns>This OrderCriteria, to allow chaining of adds</returns>
        public IOrderCriteria Add(string field, SortDirection sortDirection)
        {
            _fields.Add(new OrderCriteriaField(field, field, null, sortDirection));
            return this;
        }

        /// <summary>
        /// Adds a field to the fields to be sorted on.
        /// </summary>
        /// <param name="orderCriteriaField">The <see cref="OrderCriteriaField"/> to add to the OrderCriteria's field collection</param>
        /// <returns>This OrderCriteria, to allow chaining of adds</returns>
        public IOrderCriteria Add(IOrderCriteriaField orderCriteriaField)
        {
            _fields.Add(orderCriteriaField);
            return this;
        }

        /// <summary>
        /// Compares two BusinessObjects using the criteria set up in this OrderCriteria object.
        /// </summary>
        /// <typeparam name="T">The Type to be compared</typeparam>
        /// <param name="bo1">The first object to be used in the comparison</param>
        /// <param name="bo2">The second object to be used in the comparison</param>
        /// <returns>a value less than 0 if bo1 is less than bo2, 0 if bo1 = bo2 and greater than 0 if b01 is greater than bo2
        /// The value returned is negated if the SortDirection is Descending
        /// </returns>
        public int Compare<T>(T bo1, T bo2) where T : IBusinessObject
        {
            foreach (OrderCriteriaField field in _fields)
            {
                int compareResult = field.Compare(bo1, bo2);
                if (compareResult != 0) return compareResult;
            }
            return 0;
        }

        ///<summary>
        ///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override string ToString()
        {
            List<string> fieldsAsString = new List<string>();
            _fields.ForEach(obj => fieldsAsString.Add(obj.ToString()));
            return String.Join(", ", fieldsAsString.ToArray());
        }


        /// <summary>
        /// Creates an OrderCriteria object by parsing a string in the correct format.
        /// The format is:
        /// <para>&lt;orderCriteria&gt; => &lt;emptystring&gt; | &lt;field&gt; [, &lt;field&gt;, ... ] <br/>
        /// &lt;field&gt; => &lt;fieldName&gt; [ ASC | DESC ] </para>
        /// For example: <code>Surname, Age DESC</code> or <code>Age ASC, Surname DESC</code>
        /// </summary>
        /// <param name="orderCriteriaString">The string in the correct format (see above)</param>
        /// <returns>An OrderCriteria created from the string</returns>
        public static IOrderCriteria FromString(string orderCriteriaString)
        {
            IOrderCriteria orderCriteria = new OrderCriteria();
            if (string.IsNullOrEmpty(orderCriteriaString)) return orderCriteria;
            orderCriteriaString = orderCriteriaString.Trim();
            if (string.IsNullOrEmpty(orderCriteriaString)) return orderCriteria;
            string[] orderFields = orderCriteriaString.Split(',');
            foreach (string field in orderFields)
            {
                orderCriteria.Add(OrderCriteriaField.FromString(field));

            }
            return orderCriteria;
        }

        ///<summary>
        ///Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        ///</summary>
        ///
        ///<returns>
        ///A hash code for the current <see cref="T:System.Object"></see>.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        ///<summary>
        ///Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        ///</summary>
        ///
        ///<returns>
        ///Value Condition Less than zero x is less than y. Zero x equals y. Greater than zero x is greater than y. 
        ///</returns>
        ///
        ///<param name="y">The second object to compare. </param>
        ///<param name="x">The first object to compare. </param>
        ///<exception cref="T:System.ArgumentException">Neither x nor y implements the <see cref="T:System.IComparable"></see> interface.-or- x and y are of different types and neither one can handle comparisons with the other. </exception><filterpriority>2</filterpriority>
        int IComparer.Compare(object x, object y) { return Compare((IBusinessObject)x, (IBusinessObject)y); }
    }
}