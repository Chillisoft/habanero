//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public partial class ComboBoxWin : ComboBox, IComboBox
    {
            private ComboBoxManager _manager;


        public ComboBoxWin()
        {
            InitializeComponent();
            _manager = new ComboBoxManager(this);
        }
        public new IComboBoxObjectCollection Items
        {
            get
            {
                IComboBoxObjectCollection objectCollection = new ComboBoxObjectCollectionWin(base.Items);
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


        internal class ComboBoxObjectCollectionWin : IComboBoxObjectCollection
        {
            private readonly ObjectCollection _items;

            public ComboBoxObjectCollectionWin(ObjectCollection items)
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

            public object this[int index]
            {
                get { return _items[index]; }
                set { _items[index] = value; }
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
            get { return new ControlCollectionWin(base.Controls); }
        }
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (System.Windows.Forms.DockStyle)value; }
        }
    }
}
