using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public partial class ListBoxWin : ListBox,IListBox
    {
        private readonly ListBoxSelectedObjectCollectionWin _selectedObjectCollection;
        private readonly ListBoxObjectCollectionWin _objectCollection;

        public ListBoxWin()
        {
            _objectCollection = new ListBoxObjectCollectionWin(base.Items);
            _selectedObjectCollection = new ListBoxSelectedObjectCollectionWin(base.SelectedItems);
           
        }

        public new IListBoxObjectCollection Items
        {
            get
            {
                return _objectCollection;
            }
        }

        public new IListBoxSelectedObjectCollection SelectedItems
        {
            get
            {
                return _selectedObjectCollection;
            }
        }

        public new ListBoxSelectionMode SelectionMode
        {
            get { return (ListBoxSelectionMode)Enum.Parse(typeof(ListBoxSelectionMode), base.SelectionMode.ToString()); }
            set { base.SelectionMode = (SelectionMode)Enum.Parse(typeof(SelectionMode), value.ToString()); }
        }
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
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

        private class ListBoxSelectedObjectCollectionWin : IListBoxSelectedObjectCollection
        {
            private readonly SelectedObjectCollection _items;
            public ListBoxSelectedObjectCollectionWin(SelectedObjectCollection items)
            {
                this._items = items;
            }

            public void Add(object item)
            {
                _items.Add(item);
            }

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            public IEnumerator GetEnumerator()
            {
                return _items.GetEnumerator();
            }
        }
    }

}
