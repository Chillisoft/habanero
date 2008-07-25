//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Collections;
using System.Collections.Generic;


namespace Habanero.Base
{
    /// <summary>
    /// Represents a set of order criteria used when loading collections of BusinessObjects.
    /// </summary>
    public class OrderCriteria : IComparer
    {
        private readonly List<Field> _fields = new List<Field>();

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
        /// The fields that will be ordered on. See <see cref="Field"/>
        /// </summary>
        public List<Field> Fields
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
        public OrderCriteria Add(string field)
        {
            return Add(field, SortDirection.Ascending);
        }

        /// <summary>
        /// Add a field to the fields to be sorted on. This is a convenience method that will created a Field and add it
        /// to the Fields list. </summary>
        /// <param name="field">The name of the field to be sorted on</param>
        /// <param name="sortDirection">The sort direction of this field</param>
        /// <returns>This OrderCriteria, to allow chaining of adds</returns>
        public OrderCriteria Add(string field, SortDirection sortDirection)
        {
            _fields.Add(new Field(field, field, null, sortDirection));
            return this;
        }

        /// <summary>
        /// Adds a field to the fields to be sorted on.
        /// </summary>
        /// <param name="field">The <see cref="OrderCriteria.Field"/> to add to the OrderCriteria's field collection</param>
        /// <returns>This OrderCriteria, to allow chaining of adds</returns>
        public OrderCriteria Add(Field field)
        {
            _fields.Add(field);
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
        public int Compare<T>(T bo1, T bo2) where T:IBusinessObject
        {
            foreach (Field field in _fields)
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
            _fields.ForEach(delegate(Field obj)
            {
                fieldsAsString.Add(obj.ToString());
            });
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
        public static OrderCriteria FromString(string orderCriteriaString)
        {
            OrderCriteria orderCriteria = new OrderCriteria();
            if (string.IsNullOrEmpty(orderCriteriaString)) return orderCriteria;
            orderCriteriaString = orderCriteriaString.Trim();
            if (string.IsNullOrEmpty(orderCriteriaString)) return orderCriteria;
            string[] orderFields = orderCriteriaString.Split(',');
            foreach (string field in orderFields)
            {
                orderCriteria.Add(Field.FromString(field));

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

        /// <summary>
        /// Field represents one field in an OrderCriteria object.  Each Field has a name and SortDirection.
        /// </summary>
        public class Field : QueryField

        {
            private readonly SortDirection _sortDirection;

            /// <summary>
            /// Creates a Field with the given name and SortDirection
            /// </summary>
            /// <param name="propertyName">The name of the property to sort on</param>
            /// <param name="sortDirection">The SortDirection option to use when sorting</param>
            public Field(string propertyName, string fieldName, Source source, SortDirection sortDirection)
                : base(propertyName, fieldName, source)
            {

                _sortDirection = sortDirection;
            }

            /// <summary>
            /// Creates a Field with the given name and SortDirection
            /// </summary>
            /// <param name="propertyName">The name of the property to sort on</param>
            /// <param name="sortDirection">The SortDirection option to use when sorting</param>
            public Field(string propertyName, SortDirection sortDirection)
                : this(propertyName, propertyName, null, sortDirection)
            {


            }

            /// <summary>
            /// The SortDirection option to use when sorting
            /// </summary>
            public SortDirection SortDirection
            {
                get { return _sortDirection; }
            }

            /// <summary>
            /// Returns the full name of the order criteria - ie "Source.Name"
            /// </summary>
            public string FullName
            {
                get { return this.Source == null || String.IsNullOrEmpty(this.Source.ToString()) ? this.PropertyName : this.Source + "." + this.PropertyName; }
            }


            ///<summary>
            ///Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
            ///</summary>
            ///
            ///<returns>
            ///true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
            ///</returns>
            ///
            ///<param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>. </param><filterpriority>2</filterpriority>
            public override bool Equals(object obj)
            {
                if (!(obj is Field)) return false;
                Field objField = obj as Field;
                return (this.PropertyName.Equals(objField.PropertyName) && _sortDirection.Equals(objField.SortDirection));
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
                string fieldAsString = "";
                if (this.Source != null) fieldAsString += Source + ".";
                return fieldAsString + string.Format("{0} {1}", PropertyName, (_sortDirection == SortDirection.Ascending ? "ASC" : "DESC"));
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

            /// <summary>
            /// Creates an Field object by parsing a string in the correct format.
            /// The format is:
            /// <para>&lt;field&gt; => &lt;fieldName&gt; [ ASC | DESC ] </para>
            /// For example: <code>Age DESC</code> or <code>Surname DESC</code>
            /// </summary>
            /// <param name="fieldString">The string in the correct format (see above)</param>
            /// <returns>A Field created from the string</returns>
            public static Field FromString(string fieldString)
            {
                string[] parts = fieldString.Trim().Split(' ');
                if (parts.Length > 1)
                {
                    SortDirection sortDirection;
                    if (parts[1].ToUpper().Equals("ASC")) sortDirection = SortDirection.Ascending;
                    else if (parts[1].ToUpper().Equals("DESC")) sortDirection = OrderCriteria.SortDirection.Descending;
                    else throw new ArgumentException(string.Format("'{0}' is an invalid sort order. Valid options are ASC and DESC", parts[1]));
                    return CreateField(parts[0], sortDirection);
                }
                else
                    return CreateField(parts[0], SortDirection.Ascending);
            }

            /// <summary>
            /// Compares two BusinessObjects using this field.
            /// </summary>
            /// <typeparam name="T">The Type of objects being compared. This must be a class that implements IBusinessObject</typeparam>
            /// <param name="bo1">The first object to compare</param>
            /// <param name="bo2">The object to compare the first with</param>
            /// <returns>a value less than 0 if bo1 is less than bo2, 0 if bo1 = bo2 and greater than 0 if b01 is greater than bo2
            /// The value returned is negated if the SortDirection is Descending</returns>
            public int Compare<T>(T bo1, T bo2) where T : IBusinessObject
            {
                string fullPropName = this.FullName;
                IPropertyComparer<T> comparer = bo1.ClassDef.CreatePropertyComparer<T>(fullPropName);
                comparer.PropertyName = PropertyName;
                comparer.Source = Source;
                int compareResult = comparer.Compare(bo1, bo2);
                if (compareResult != 0)
                    return SortDirection == SortDirection.Ascending ? compareResult : -compareResult;
                return 0;
            }

            private static Field CreateField(string sourceAndFieldName, SortDirection sortDirection)
            {
                //string source = null;
                string[] parts = sourceAndFieldName.Trim().Split('.');
                string fieldName = parts[parts.Length-1];
                Base.Source source = Base.Source.FromString(String.Join(".", parts, 0, parts.Length - 1));

//                if (parts.Length > 2)
//                {
//                    string subSourceAndFieldName = String.Join(".", parts, 0, parts.Length -1 );
//                    subField = CreateField(subSourceAndFieldName, sortDirection);
////                    fieldName = parts[1];
//                } else if (parts.Length == 2)
//                {
//                    source = parts[0];
//                    //fieldName = parts[1];
//                } else
//                {
//                    //fieldName = parts[0];
//                }
//                Source newSource = string.IsNullOrEmpty(source) ? null : new Source(source);
                Field field = new Field(fieldName, fieldName, source, sortDirection);
               // if (subField != null) field.Source.Joins.Add(new Source.Join(field.Source, subField.));
                return field;
            }
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
        int IComparer.Compare(object x, object y)
        {
                return Compare((IBusinessObject) x, (IBusinessObject) y);

        }

    }
}