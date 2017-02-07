using System.Collections.Generic;

namespace Habanero.Base.Data
{
    /// <summary>
    /// A bare bones query result set.
    /// </summary>
    public interface IQueryResult
    {
        /// <summary>
        /// The rows in the result
        /// </summary>
        List<IQueryResultRow> Rows { get; }

        /// <summary>
        /// The fields/columns in the result
        /// </summary>
        List<IQueryResultField> Fields { get; }
    }
}