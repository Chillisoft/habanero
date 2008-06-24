using System.Collections.Generic;

namespace Habanero.BO
{

    /// <summary>
    /// A model of a Select Query that can be used to load data from a data store.  This includes the Fields to load, the source to load from
    /// (such as the database table name), the OrderCriteria to use (what fields must be sorted on), the Criteria to use (only objects that
    /// match the given criteria will be loaded), and the number of objects to load (defined by the Limit).
    /// </summary>
    public interface ISelectQuery
    {
        /// <summary>
        /// The Criteria to use when loading. Only objects that match these criteria will be loaded.
        /// </summary>
        Criteria Criteria { get; set; }

        /// <summary>
        /// The fields to load from the data store.
        /// </summary>
        Dictionary<string, QueryField> Fields { get; }

        /// <summary>
        /// The source of the data. In a database query this would be the first table listed in the FROM clause.
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// The fields to use to order a collection of objects when loading them.
        /// </summary>
        OrderCriteria OrderCriteria { get; set; }

        /// <summary>
        /// The number of objects to load
        /// </summary>
        int Limit
        {
            get;
            set;
        }
    }
}