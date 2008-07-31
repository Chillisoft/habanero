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
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public partial class ListBoxWin : ListBox, IListBox
    {
        private readonly ListBoxSelectedObjectCollectionWin _selectedObjectCollection;
        private readonly ListBoxObjectCollectionWin _objectCollection;
        private string _errorMessage;

        public ListBoxWin()
        {
            _objectCollection = new ListBoxObjectCollectionWin(base.Items);
            _selectedObjectCollection = new ListBoxSelectedObjectCollectionWin(base.SelectedItems);
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
            get { return (ListBoxSelectionMode)Enum.Parse(typeof(ListBoxSelectionMode), base.SelectionMode.ToString()); }
            set { base.SelectionMode = (SelectionMode)Enum.Parse(typeof(SelectionMode), value.ToString()); }
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
