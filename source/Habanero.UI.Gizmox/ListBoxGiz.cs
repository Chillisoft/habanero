using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Gizmox
{
    public partial class ListBoxGiz : ListBox, IListBox
    {
        private readonly IControlFactory _controlFactory;

        public ListBoxGiz(IControlFactory controlFactory)
        {
            this._controlFactory = controlFactory;
        }


        public new IListBoxObjectCollection Items
        {
            get
            {
                ListBoxObjectCollectionGiz objectCollection = new ListBoxObjectCollectionGiz(base.Items);
                return objectCollection;
            }
        }

        internal class ListBoxObjectCollectionGiz : IListBoxObjectCollection
        {
            private readonly ObjectCollection _items;

            public ListBoxObjectCollectionGiz(ObjectCollection items)
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