using System.Collections.ObjectModel;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public partial class ListBoxWin : ListBox,IListBox
    {
        private readonly IControlFactory _controlFactory;

        public ListBoxWin(IControlFactory controlFactory)
        {
            this._controlFactory = controlFactory;
            
        }


        public new IListBoxObjectCollection Items
        {
            get
            {
                ListBoxObjectCollectionWin objectCollection = new ListBoxObjectCollectionWin(base.Items);
                return objectCollection;
            }
        }

        internal class ListBoxObjectCollectionWin : IListBoxObjectCollection
        {
            private readonly ObjectCollection _items;

            public ListBoxObjectCollectionWin(ObjectCollection items)
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
