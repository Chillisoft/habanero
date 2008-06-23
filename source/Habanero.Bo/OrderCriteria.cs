using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.Comparer;

namespace Habanero.BO
{
    public class OrderCriteria
    {
        private List<Field> _fields = new List<Field>();

        public enum SortDirection
        {
            Ascending,
            Descending
        }

        public int Count
        {
            get { return _fields.Count; }
        }

        public List<Field> Fields
        {
            get
            {
                return _fields;
            }
        }

        public OrderCriteria Add(string field)
        {
            _fields.Add(new Field(field, SortDirection.Ascending));
            return this;
        }

        public OrderCriteria Add(string field, SortDirection sortDirection)
        {
            _fields.Add(new Field(field, sortDirection));
            return this;
        }

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

        public class Field

        {
            private readonly string _name;
            private readonly SortDirection _sortDirection;

            public Field(string name, SortDirection sortDirection)
            {
                _name = name;
                _sortDirection = sortDirection;
            }


            public string Name
            {
                get { return _name; }
            }

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