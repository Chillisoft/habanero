using System.Collections;

namespace Habanero.UI.Base
{
    public interface IListBoxObjectCollection : IEnumerable
    {
        void Add(object item);

        int Count { get; }
        void Remove(object item);
        void Clear();
    }
}