using System.Collections.Generic;

namespace Habanero.Base.Data
{
    public interface IQueryResult
    {
        List<IQueryResultRow> Rows { get; }
        List<IQueryResultField> Fields { get; }
    }
}