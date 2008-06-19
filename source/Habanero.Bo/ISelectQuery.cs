using System.Collections.Generic;

namespace Habanero.BO
{
    public interface ISelectQuery
    {
        Criteria Criteria { get; set; }

        Dictionary<string, QueryField> Fields { get; }

        /// <summary>
        /// The source of the data. In a database query this would be the first table listed in the FROM clause.
        /// </summary>
        string Source { get; set; }

        OrderCriteria OrderCriteria { get; set; }
    }
}