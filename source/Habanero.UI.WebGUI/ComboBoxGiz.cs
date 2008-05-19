using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.BO;
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
            }

            public bool Contains(object value)
            {
               return _items.Contains(value);
            }
        }
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
    }
}
