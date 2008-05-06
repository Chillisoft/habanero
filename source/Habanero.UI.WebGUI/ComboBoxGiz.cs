using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ComboBoxGiz : ComboBox, IComboBox
    {
        public new IComboBoxObjectCollection Items
        {
            get
            {
                IComboBoxObjectCollection objectCollection = new ComboBoxObjectCollectionGiz(base.Items);
                return objectCollection;
            }
        }

        internal class ComboBoxObjectCollectionGiz : IComboBoxObjectCollection
        {
            private readonly ObjectCollection _items;

            public ComboBoxObjectCollectionGiz(ObjectCollection items)
            {
                this._items = items;
            }

            public void Add(object item)
            {
                _items.Add(item);
            }

            public int Count
            {
                get { return _items.Count; }
            }

            public void Remove(object item)
            {
                _items.Remove(item);
            }

            public void Clear()
            {
                _items.Clear();
            }
        }
    }
}
