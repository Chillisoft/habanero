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
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    //TODO: Port
//    /// <summary>
//    /// Represents a list view control, which displays a collection of
//    /// items that can be displayed using one of four different views
//    /// </summary>
//    public interface IListView : IControlHabanero
//    {
//        /// <summary>
//        /// Occurs when the SelectedIndex property has changed.
//        /// </summary>
//        event EventHandler SelectedIndexChanged;

//        [System.ComponentModel.Browsable(true)]
//        [System.ComponentModel.DefaultValue(false)]
//        /// <summary>
//        /// Gets or sets a value indicating whether grid lines appear between 
//        /// the rows and columns containing the items and subitems in the control
//        /// </summary>
//        bool GridLines { get; set; }

//        /// <summary>
//        /// Gets or sets a value indicating whether full row select is enabled.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if full row select is enabled; otherwise, <c>false</c>.
//        /// </value>
//        [System.ComponentModel.Browsable(true)]
//        [System.ComponentModel.DefaultValue(true)]
//        bool FullRowSelect { set; get; }

//        /// <summary>
//        /// Gets the collection of items contained within the listview.
//        /// </summary>
//        [System.ComponentModel.DesignerSerializationVisibility(
//            System.ComponentModel.DesignerSerializationVisibility.Content)]
//        IListViewItemCollection Items { get; }

//        /// <summary>
//        /// Gets or sets a value indicating whether a check box appears next to each item in the control.
//        /// </summary>
//        [System.ComponentModel.Browsable(true)]
//        [System.ComponentModel.DefaultValue(false)]
//        bool CheckBoxes { get; set; }

//        /// <summary>
//        /// Gets or sets a value indicating whether multiple items can be selected.
//        /// </summary>
//        [System.ComponentModel.Browsable(true)]
//        [System.ComponentModel.DefaultValue(true)]
//        bool MultiSelect { get; set; }

//        /// <summary>
//        /// Gets a value indicating whether this instance can focus.
//        /// </summary>
//        /// <value>
//        /// 	<c>true</c> if this instance can focus; otherwise, <c>false</c>.
//        /// </value>
//        bool CanFocus { get; }

//        /// <summary>
//        /// Gets the currently selected item index.
//        /// </summary>
//        /// <value></value>
//        [System.ComponentModel.Browsable(false)]
//        [System.ComponentModel.DesignerSerializationVisibility(
//            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
//        int SelectedIndex { get; set; }

//        /// <summary>
//        /// Gets the currently selected item index.
//        /// </summary>
//        /// <value></value>
//        [System.ComponentModel.Browsable(false)]
//        [System.ComponentModel.DesignerSerializationVisibility(
//            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
//        IListViewItem SelectedItem { get; set; }

//        /// <summary>
//        /// Gets the selected items.
//        /// </summary>
//        /// <value></value>
//        [System.ComponentModel.Browsable(false)]
//        ISelectedListViewItemCollection SelectedItems { get; }

//        /// <summary>
//        /// Gets the checked items.
//        /// </summary>
//        /// <value></value>
//        [System.ComponentModel.Browsable(false)]
//        ArrayList CheckedItems { get; }

//        /// <summary>
//        /// Removes all items and columns from the control.
//        /// </summary>
//        void Clear();

//        /// <summary>
//        ///  Gets the collection of columns contained within the listview.
//        /// </summary>
//        [System.ComponentModel.Browsable(true)]
//        [System.ComponentModel.DesignerSerializationVisibility(
//System.ComponentModel.DesignerSerializationVisibility.Content)]
//        IColumnHeaderCollection Columns { get;}


//        void SetCollection(IBusinessObjectCollection collection);

//        IListViewItem CreateListViewItem(string displayName);
//    }
}