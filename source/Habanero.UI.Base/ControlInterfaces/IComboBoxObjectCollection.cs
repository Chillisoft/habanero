using System.Collections;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IComboBoxObjectCollection : IEnumerable
    {
        void Add(object item);

        int Count { get; }

        string Label { get; set; }

        void Remove(object item);

        void Clear();

        void SetCollection(BusinessObjectCollection<BusinessObject> collection);

        object this[int index] { get; set; }

        bool Contains(object value);

        int IndexOf(object value);
    }
}