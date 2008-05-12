using System.Collections;

namespace Habanero.UI
{
    public interface IListBoxSelectedObjectCollection : IEnumerable
    {
        void Add(object item);
    }
}