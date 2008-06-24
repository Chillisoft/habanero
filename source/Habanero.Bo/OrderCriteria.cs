using System.Collections.Generic;

namespace Habanero.BO
{

    /// <summary>
    /// Represents a set of order criteria used when loading collections of BusinessObjects.
    /// </summary>
    public class OrderCriteria
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
        /// Compares two BusinessObjects using the criteria set up in this OrderCriteria object.
        /// </summary>
        /// <typeparam name="T">The Type to be compared</typeparam>
        /// <param name="bo1">The first object to be used in the comparison</param>
        /// <param name="bo2">The second object to be used in the comparison</param>
        /// <returns>a value less than 0 if bo1 is less than bo2, 0 if bo1 = bo2 and greater than 0 if b01 is greater than bo2
        /// The value returned is negated if the SortDirection is Descending
        /// </returns>
        public int Compare<T>(T bo1, T bo2) where T: BusinessObject, new()
        {
            IComparer<T> comparer;
            foreach (Field field in _fields)
            {
                comparer = bo1.Props[field.Name].PropDef.GetPropertyComparer<T>();
                int compareResult = comparer.Compare(bo1, bo2);
                if (compareResult != 0) return field.SortDirection == SortDirection.Ascending ? compareResult : -compareResult;
            }
            return 0;
        }

        /// <summary>
        /// Field represents one field in an OrderCriteria object.  Each Field has a name and SortDirection.
        /// </summary>
        public class Field

        {
            private readonly string _name;
            private readonly SortDirection _sortDirection;

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
        }

    }
}