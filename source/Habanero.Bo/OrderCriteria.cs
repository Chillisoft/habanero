using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.Comparer;

namespace Habanero.BO
{
    public class OrderCriteria
    {
        private List<string> _fields = new List<string>( );

        public OrderCriteria(string orderField)
        {
            _fields.Add(orderField);
        }

        public int Count
        {
            get { return _fields.Count; }
        }

        public List<string> Fields
        {
            get
            {
                return _fields;
            }
        }

        public void Add(string field)
        {
            _fields.Add(field);
        }

        public int Compare<T>(T bo1, T bo2) where T: BusinessObject, new()
        {
            int compareResult = 0;
            IComparer<T> comparer;
            foreach (string field in _fields)
            {
                comparer = bo1.Props[field].PropDef.GetPropertyComparer<T>();
                compareResult = comparer.Compare(bo1, bo2);
                if (compareResult != 0) return compareResult;
            }
            return compareResult;
        }
    }
}