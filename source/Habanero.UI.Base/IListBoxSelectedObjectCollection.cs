using System.Collections;

namespace Habanero.UI.Base
{
    public interface IListBoxSelectedObjectCollection : IEnumerable
    {
        void Add(object item);
    }
}