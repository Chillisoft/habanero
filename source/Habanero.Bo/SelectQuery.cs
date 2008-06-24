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
        private OrderCriteria _orderCriteria;
        private int _limit;

        ///<summary>
        /// Creates a SelectQuery with no Criteria and no fields.  In order to use the SelectQuery at least on field must be added
        /// to the <see cref="Fields"/>, and a <see cref="Source"/> must be specified.
        ///</summary>
        public SelectQuery()
        {
            
        }


        ///<summary>
        /// Creates a SelectQuery with a Critiria and no fields.  In order to use the SelectQuery at least on field must be added
        /// to the <see cref="Fields"/>, and a <see cref="Source"/> must be specified.
        ///</summary>
        /// <param name="criteria">The Criteria to initialise this SelectQuery with.</param>
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

        public int Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }
    }
}