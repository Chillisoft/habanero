using System.Collections.Generic;

namespace Habanero.Base.Data
{
    public class QueryResultRow : IQueryResultRow
    {
        public QueryResultRow()
        {
            RawValues = new List<object>();
            Values = new List<object>();
        }

        public QueryResultRow(object[] rawValues) : this()
        {
            rawValues.ForEach(RawValues.Add);
            rawValues.ForEach(Values.Add);
        }

        public IList<object> RawValues { get; private set; }
        public IList<object> Values { get; private set; }
    }
}