using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ListBoxGiz : ListBox, IListBox
    {
        private readonly ListBoxSelectedObjectCollectionGiz _selectedObjectCollection;
        private readonly ListBoxObjectCollectionGiz _objectCollection;
        private string _errorMessage;
        public ListBoxGiz()
        {
            _objectCollection = new ListBoxObjectCollectionGiz(base.Items);
            _selectedObjectCollection = new ListBoxSelectedObjectCollectionGiz(base.SelectedItems);
            _errorMessage = "";
           
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
            get { return (ListBoxSelectionMode) Enum.Parse(typeof(ListBoxSelectionMode), base.SelectionMode.ToString()); }
            set { base.SelectionMode = (SelectionMode) Enum.Parse(typeof (SelectionMode), value.ToString()); }
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (Gizmox.WebGUI.Forms.DockStyle)value; }
        }

        private class ListBoxObjectCollectionGiz : IListBoxObjectCollection
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
        private class ListBoxSelectedObjectCollectionGiz : IListBoxSelectedObjectCollection
        {
            private readonly SelectedObjectCollection _items;
            public ListBoxSelectedObjectCollectionGiz(SelectedObjectCollection items)
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