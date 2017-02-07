using System.Collections.Generic;

namespace Habanero.Base.Data
{
    /// <summary>
    /// A row of a <see cref="QueryResult"/>
    /// </summary>
    public class QueryResultRow : IQueryResultRow
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public QueryResultRow()
        {
            RawValues = new List<object>();
            Values = new List<object>();
        }

        /// <summary>
        /// Constructor that sets up the values given
        /// </summary>
        /// <param name="rawValues"></param>
        public QueryResultRow(object[] rawValues) : this()
        {
            rawValues.ForEach(RawValues.Add);
            rawValues.ForEach(Values.Add);
        }

        /// <summary>
        /// The raw values of the result set.
        /// </summary>
        public IList<object> RawValues { get; private set; }

        /// <summary>
        /// The values of the result - with each value coerced to the type defined by the <see cref="IQueryResult"/>
        /// </summary>
        public IList<object> Values { get; private set; }
    }
}