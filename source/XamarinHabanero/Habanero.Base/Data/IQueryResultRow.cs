using System.Collections.Generic;

namespace Habanero.Base.Data
{
    /// <summary>
    /// A row of a <see cref="IQueryResult"/>
    /// </summary>
    public interface IQueryResultRow
    {
        /// <summary>
        /// The raw values of the result set.
        /// </summary>
        IList<object> RawValues { get; }

        /// <summary>
        /// The values of the result - with each value coerced to the type defined by the <see cref="IQueryResult"/>
        /// </summary>
        IList<object> Values { get; }
    }
}