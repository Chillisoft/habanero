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
using Habanero.Base;
using Habanero.BO;
using Habanero.UI.Base;
using AutoCompleteMode=Habanero.UI.Base.AutoCompleteMode;
using AutoCompleteSource=Habanero.UI.Base.AutoCompleteSource;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Represents a ComboBox control
    /// </summary>
    public class ComboBoxVWG : ComboBox, IComboBox
    {
        private ComboBoxManager _manager;

        public ComboBoxVWG()
        {
            _manager = new ComboBoxManager(this);
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
        /// Gets an object representing the collection of the items
        /// contained in this ComboBox
        /// </summary>
        public new IComboBoxObjectCollection Items
        {
            get
            {
                IComboBoxObjectCollection objectCollection = new ComboBoxObjectCollectionVWG(base.Items);
                return objectCollection;
            }
        }

        /// <summary>
        /// Gets or sets currently selected item in the ComboBox
        /// </summary>
        object IComboBox.SelectedItem
        {
            get { return _manager.GetSelectedItem(this.SelectedItem); }
            set { this.SelectedItem = _manager.GetItemToSelect(value); }
        }

        /// <summary>
        /// Gets or sets the value of the member property specified by
        /// the ValueMember property
        /// </summary>
        object IComboBox.SelectedValue
        {
            get { return _manager.GetSelectedValue(this.SelectedItem); }
            set { SelectedItem = _manager.GetValueToSelect(value); }
        }

        Base.AutoCompleteMode IComboBox.AutoCompleteMode
        {
            get { return ComboBoxAutoCompleteModeVWG.GetAutoCompleteMode(base.AutoCompleteMode); }
            set { base.AutoCompleteMode = ComboBoxAutoCompleteModeVWG.GetAutoCompleteMode(value); }
        }

        Base.AutoCompleteSource IComboBox.AutoCompleteSource
        {
            get { return ComboBoxAutoCompleteSourceVWG.GetAutoCompleteSource(base.AutoCompleteSource); }
            set { base.AutoCompleteSource = ComboBoxAutoCompleteSourceVWG.GetAutoCompleteSource(value); }
        }


        /// <summary>
        /// Represents the collection of items in a ComboBox
        /// </summary>
        internal class ComboBoxObjectCollectionVWG : IComboBoxObjectCollection
        {
            private readonly ObjectCollection _items;
            private string _label;

            public ComboBoxObjectCollectionVWG(ObjectCollection items)
            {
                this._items = items;
            }

            /// <summary>
            /// Adds an item to the list of items for a ComboBox
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
            /// Gets or sets the label to display
            /// </summary>
            public string Label
            {
                get { return _label; }
                set { _label = value; }
            }

            /// <summary>
            /// Removes the specified item from the ComboBox
            /// </summary>
            /// <param name="item">The System.Object to remove from the list</param>
            public void Remove(object item)
            {
                _items.Remove(item);
            }

            /// <summary>
            /// Removes all items from the ComboBox
            /// </summary>
            public void Clear()
            {
                _items.Clear();
            }

            /// <summary>
            /// Populates the collection using the given BusinessObjectCollection
            /// </summary>
            /// <param name="collection">A BusinessObjectCollection</param>
            public void SetCollection(IBusinessObjectCollection collection)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Retrieves the item at the specified index within the collection
            /// </summary>
            /// <param name="index">The index of the item in the collection to retrieve</param>
            /// <returns>An object representing the item located at the
            /// specified index within the collection</returns>
            public object this[int index]
            {
                get { return _items[index]; }
                set
                {//Hack: due to a bug in webgui.
                    _items.RemoveAt(index);
                    _items.Insert(index, value);
                } 
            }

            /// <summary>
            /// Determines if the specified item is located within the collection
            /// </summary>
            /// <param name="value">An object representing the item to locate in the collection</param>
            /// <returns>true if the item is located within the collection; otherwise, false</returns>
            public bool Contains(object value)
            {
                return _items.Contains(value);
            }

            /// <summary>
            /// Retrieves the index within the collection of the specified item
            /// </summary>
            /// <param name="value">An object representing the item to locate in the collection</param>
            /// <returns>The zero-based index where the item is
            /// located within the collection; otherwise, -1</returns>
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
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (Gizmox.WebGUI.Forms.DockStyle)value; }
        }
    }
}