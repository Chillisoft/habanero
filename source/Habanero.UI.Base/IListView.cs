using System;
using System.Collections;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IListView : IControlChilli
    {
        /// <summary>
        /// Occurs when the SelectedIndex property has changed.
        /// </summary>
        event EventHandler SelectedIndexChanged;

        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.DefaultValue(false)]
        /// <summary>
            ///
            /// </summary>
            bool GridLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether full row select is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if full row select is enabled; otherwise, <c>false</c>.
        /// </value>
        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.DefaultValue(true)]
        bool FullRowSelect { set; get; }

        /// <summary>
        ///  Gets the collection of items contained within the listview.
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Content)]
        IListViewItemCollection Items { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a check box appears next to each item in the control.
        /// </summary>
        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.DefaultValue(false)]
        bool CheckBoxes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multiple items can be selected.
        /// </summary>
        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.DefaultValue(true)]
        bool MultiSelect { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance can focus.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can focus; otherwise, <c>false</c>.
        /// </value>
        bool CanFocus { get; }

        /// <summary>
        /// Gets the currently selected item index.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        int SelectedIndex { get; set; }

        /// <summary>
        /// Gets the currently selected item index.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        IListViewItem SelectedItem { get; set; }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.Browsable(false)]
        ISelectedListViewItemCollection SelectedItems { get; }

        /// <summary>
        /// Gets the checked items.
        /// </summary>
        /// <value></value>
        [System.ComponentModel.Browsable(false)]
        ArrayList CheckedItems { get; }

        /// <summary>
        /// Removes all items and columns from the control.
        /// </summary>
        void Clear();

        /// <summary>
        ///  Gets the collection of columns contained within the listview.
        /// </summary>
        [System.ComponentModel.Browsable(true)] [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Content)] 
        IColumnHeaderCollection Columns{get;}


        void SetCollection(IBusinessObjectCollection collection);

        IListViewItem CreateListViewItem(string displayName );
    }
}