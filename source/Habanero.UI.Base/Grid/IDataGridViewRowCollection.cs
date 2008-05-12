using System.Collections.Generic;

namespace Habanero.UI.Base
{
    public interface IDataGridViewRowCollection
    {
        int Count { get; }

        IDataGridViewRow this[int index] { get; }
    }
}