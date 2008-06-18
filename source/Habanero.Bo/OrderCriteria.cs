using System.Collections.Generic;

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
    }
}