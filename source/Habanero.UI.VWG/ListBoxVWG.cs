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
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Represents a control to display a list of items
    /// </summary>
    public class ListBoxVWG : ListBox, IListBox
    {
        private readonly ListBoxSelectedObjectCollectionGiz _selectedObjectCollection;
        private readonly ListBoxObjectCollectionGiz _objectCollection;
        private string _errorMessage;

        public ListBoxVWG()
        {
            _objectCollection = new ListBoxObjectCollectionGiz(base.Items);
            _selectedObjectCollection = new ListBoxSelectedObjectCollectionGiz(base.SelectedItems);
            _errorMessage = "";
           
        }

        /// <summary>
        /// Gets the items of the ListBox
        /// </summary>
        public new IListBoxObjectCollection Items
        {
            get { return _objectCollection; }
        }

        /// <summary>
        /// Gets a collection containing the currently selected items in the ListBox
        /// </summary>
        public new IListBoxSelectedObjectCollection SelectedItems
        {
            get { return _selectedObjectCollection; }
        }

        /// <summary>
        /// Gets or sets the method in which items are selected in the ListBox
        /// </summary>
        public new ListBoxSelectionMode SelectionMode
        {
            get { return (ListBoxSelectionMode) Enum.Parse(typeof(ListBoxSelectionMode), base.SelectionMode.ToString()); }
            set { base.SelectionMode = (SelectionMode) Enum.Parse(typeof (SelectionMode), value.ToString()); }
        }

        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (Gizmox.WebGUI.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleVWG.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleVWG.GetDockStyle(value); }
        }

        /// <summary>
        /// Represents the collection of items in a ListBox
        /// </summary>
        private class ListBoxObjectCollectionGiz : IListBoxObjectCollection
        {
            private readonly ObjectCollection _items;

            public ListBoxObjectCollectionGiz(ObjectCollection items)
            {
                this._items = items;
            }

            /// <summary>
            /// Adds an item to the list of items for a ListBox
            /// </summary>
            /// <param name="item">An object representing the item to add to the collection</param>
            public void Add(object item)
            {
                _items.Add(item);
            }

            /// <summary>
            /// Gets the number of items in the collection
            /// </summary>
            public int Count
            {
                get { return _items.Count; }
            }

            /// <summary>
            /// Removes the specified object from the collection
            /// </summary>
            /// <param name="item">An object representing the item to remove from the collection</param>
            public void Remove(object item)
            {
                _items.Remove(item);
            }

            /// <summary>
            /// Removes all items from the collection
            /// </summary>
            public void Clear()
            {
                _items.Clear();
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

        /// <summary>
        /// Represents the collection of selected items in the ListBox
        /// </summary>
        private class ListBoxSelectedObjectCollectionGiz : IListBoxSelectedObjectCollection
        {
            private readonly SelectedObjectCollection _items;
            public ListBoxSelectedObjectCollectionGiz(SelectedObjectCollection items)
            {
                this._items = items;
            }

            /// <summary>
            /// Adds an item to the list of selected items for a ListBox
            /// </summary>
            /// <param name="item">An object representing the item to add
            /// to the collection of selected items</param>
            public void Add(object item)
            {
                _items.Add(item);
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
    }

    
}