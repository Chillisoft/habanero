using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.BO
{

    /// <summary>
    /// A model of a Select Query that can be used to load data from a data store.  This includes the Fields to load, the source to load from
    /// (such as the database table name), the OrderCriteria to use (what fields must be sorted on), the Criteria to use (only objects that
    /// match the given criteria will be loaded), and the number of objects to load (defined by the Limit).
    /// </summary>
    public class SelectQuery : ISelectQuery
    {
        private Criteria _criteria;
        private readonly Dictionary<string, QueryField> _fields = new Dictionary<string, QueryField>(5);
        private string _source;
        private OrderCriteria _orderCriteria = new OrderCriteria();
        private int _limit;
        private IClassDef _classDef;

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


        /// <summary>
        /// The Criteria to use when loading. Only objects that match these criteria will be loaded.
        /// </summary>
        public Criteria Criteria
        {
            get { return _criteria; }
            set { _criteria = value; }
        }

        /// <summary>
        /// The fields to load from the data store.
        /// </summary>
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

        /// <summary>
        /// The fields to use to order a collection of objects when loading them.
        /// </summary>
        public OrderCriteria OrderCriteria
        {
            get { return _orderCriteria; }
            set { _orderCriteria = value; }
        }

        /// <summary>
        /// The number of objects to load
        /// </summary>
        public int Limit
        {
            get { return _limit; }
            set { _limit = value; }
        }

        /// <summary>
        /// The classdef this select query corresponds to. This can be null if the select query is being used
        /// without classdefs, but if it is built using the QueryBuilder 
        /// </summary>
        public IClassDef ClassDef
        {
            get { return _classDef; }
            set { _classDef = value; }
        }
    }
}