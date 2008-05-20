using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class ComboBoxGiz : ComboBox, IComboBox
    {
        private ComboBoxManager _manager;


        public ComboBoxGiz()
        {
            _manager = new ComboBoxManager(this);
            
        }

        public new IComboBoxObjectCollection Items
        {
            get
            {
                IComboBoxObjectCollection objectCollection = new ComboBoxObjectCollectionGiz(base.Items);
                return objectCollection;
            }
        }

        object IComboBox.SelectedItem
        {
            get
            {
                return _manager.GetSelectedItem(this.SelectedItem);
        
            }
            set
            {
                this.SelectedItem = _manager.GetItemToSelect(value);
            }
        }

        object IComboBox.SelectedValue
        {
            get
            {
                return _manager.GetSelectedValue(this.SelectedItem);
            }
            set
            {
                SelectedItem = _manager.GetValueToSelect(value);
            }
        }

        internal class ComboBoxObjectCollectionGiz : IComboBoxObjectCollection
        {
            private readonly ObjectCollection _items;
            private string _label;

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

            public string Label
            {
                get { return _label; }
                set { _label = value; }
            }

            public void Remove(object item)
            {
                _items.Remove(item);
            }

            public void Clear()
            {
                _items.Clear();
            }

            public void SetCollection(BusinessObjectCollection<BusinessObject> collection)
            {
                throw new System.NotImplementedException();
            }

            public object this[int index]
            {
                get { return _items[index]; }
                set { throw new NotImplementedException("WebGUI doesn't have a setter"); } //_items[index] = value; }
            }

            public bool Contains(object value)
            {
                return _items.Contains(value);
            }

            public int IndexOf(object value)
            {
                return _items.IndexOf(value);
            }

            ///<summary>
            ///Returns an enumerator that iterates through a collection.
            ///</summary>
            ///<returns>
            ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
            ///</returns>
            ///<filterpriority>2</filterpriority>
            public IEnumerator GetEnumerator()
            {
                return _items.GetEnumerator();
            }
        }

        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }
}