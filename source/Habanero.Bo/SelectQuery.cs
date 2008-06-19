using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.BO
{
    public class SelectQuery : ISelectQuery
    {
        private Criteria _criteria;
        private readonly Dictionary<string, QueryField> _fields = new Dictionary<string, QueryField>(5);
        private string _source;
//        private List<string> _orderFields = new List<string>();
        private OrderCriteria _orderCriteria;

        public SelectQuery()
        {
            
        }

        public SelectQuery(Criteria criteria) : this()
        {
            _criteria = criteria;
        }


        public Criteria Criteria
        {
            get { return _criteria; }
            set { _criteria = value; }
        }

        public Dictionary<string, QueryField> Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// The source of the data. In a database query this would be the first table listed in the FROM clause.
        /// </summary>
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public OrderCriteria OrderCriteria
        {
            get { return _orderCriteria; }
            set { _orderCriteria = value; }
        }
    }
}