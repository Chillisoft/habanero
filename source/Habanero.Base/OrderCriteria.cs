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
            _fields.Add(new Field(field, SortDirection.Ascending));
            return this;
        }

        /// <summary>
        /// Add a field to the fields to be sorted on. This is a convenience method that will created a Field and add it
        /// to the Fields list. </summary>
        /// <param name="field">The name of the field to be sorted on</param>
        /// <param name="sortDirection">The sort direction of this field</param>
        /// <returns>This OrderCriteria, to allow chaining of adds</returns>
        public OrderCriteria Add(string field, SortDirection sortDirection)
        {
            _fields.Add(new Field(field, sortDirection));
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
        public class Field

        {
            private readonly string _name;
            private readonly SortDirection _sortDirection;
            private string _source;

            /// <summary>
            /// Creates a Field with the given name and SortDirection
            /// </summary>
            /// <param name="name">The name of the field to sort on</param>
            /// <param name="sortDirection">The SortDirection option to use when sorting</param>
            public Field(string name, SortDirection sortDirection)
            {
                _name = name;
                _sortDirection = sortDirection;
            }

            /// <summary>
            /// The name of the field to sort on
            /// </summary>
            public string Name
            {
                get { return _name; }
            }

            /// <summary>
            /// The SortDirection option to use when sorting
            /// </summary>
            public SortDirection SortDirection
            {
                get { return _sortDirection; }
            }

            /// <summary>
            /// The source of the field (such as the relationship this field is on)
            /// </summary>
            public string Source
            {
                get { return _source; }
                set { _source = value; }
            }

            /// <summary>
            /// Returns the full name of the order criteria - ie "Source.Name"
            /// </summary>
            public string FullName
            {
                get { return String.IsNullOrEmpty(this.Source ) ? this.Name : this.Source + "." + this.Name; }
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
                return (_name.Equals(objField.Name) && _sortDirection.Equals(objField.SortDirection));
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
                if (!String.IsNullOrEmpty(_source)) fieldAsString += _source + ".";
                return fieldAsString + string.Format("{0} {1}", _name, (_sortDirection == SortDirection.Ascending ? "ASC" : "DESC"));
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
                string fullPropName = String.IsNullOrEmpty(Source) ? Name : Source + "." + Name;
                IPropertyComparer<T> comparer = bo1.ClassDef.CreatePropertyComparer<T>(fullPropName);
                comparer.PropertyName = Name;
                comparer.Source = Source;
                int compareResult = comparer.Compare(bo1, bo2);
                if (compareResult != 0)
                    return SortDirection == SortDirection.Ascending ? compareResult : -compareResult;
                return 0;
            }

            private static Field CreateField(string sourceAndFieldName, SortDirection sortDirection)
            {
                string fieldName;
                string source = null;
                string[] parts = sourceAndFieldName.Trim().Split('.');
                if (parts.Length > 1)
                {
                    source = parts[0];
                    fieldName = parts[1];
                } else
                {
                    fieldName = parts[0];
                }
                Field field = new Field(fieldName, sortDirection);
                if (!String.IsNullOrEmpty(source)) field.Source = source;
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