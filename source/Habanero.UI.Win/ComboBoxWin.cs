using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public partial class ComboBoxWin : ComboBox, IComboBox
    {
        public ComboBoxWin()
        {
            InitializeComponent();
        }
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

            public string Label
            {
                get { throw new System.NotImplementedException(); }
                set { throw new System.NotImplementedException(); }
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
        }
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }
    }
}
