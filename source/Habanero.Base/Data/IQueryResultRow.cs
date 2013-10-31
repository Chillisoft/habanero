using System.Collections.Generic;

namespace Habanero.Base.Data
{
    public interface IQueryResultRow
    {
        IList<object> RawValues { get; }
        IList<object> Values { get; }
    }
}