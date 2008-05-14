using System.Collections;

namespace Habanero.UI.Base
{

    public interface IDataGridViewSelectedRowCollection : IEnumerable
    {
        int Count { get; }

        IDataGridViewRow this[int index] { get; }
    }
}
