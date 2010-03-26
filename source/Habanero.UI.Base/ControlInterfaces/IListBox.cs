// ---------------------------------------------------------------------------------
//  Copyright (C) 2007-2010 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System;
using System.ComponentModel;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Specifies the selection behavior of a list box
    /// </summary>
    public enum ListBoxSelectionMode
    {
        /// <summary>
        /// Multiple items can be selected, and the user can use the SHIFT, CTRL, and arrow keys to make selections
        /// </summary>
        MultiExtended,
        /// <summary>
        /// Multiple items can be selected
        /// </summary>
        MultiSimple,
        /// <summary>
        /// No items can be selected
        /// </summary>
        None,
        /// <summary>
        /// Only one item can be selected
        /// </summary>
        One
    }

    /// <summary>
    /// Represents a control to display a list of items
    /// </summary>
    public interface IListBox : IListControl
    {
        /// <summary>
        /// Occurs when the SelectedIndex property has changed
        /// </summary>
        event EventHandler SelectedIndexChanged;
        
        /// <summary>
        /// Unselects all items in the ListBox
        /// </summary>
        void ClearSelected();

        /// <summary>
        /// Gets the items of the ListBox
        /// </summary>
        IListBoxObjectCollection Items { get; }

        ///// <summary>
        ///// Gets or sets the zero-based index of the currently selected item in a <see cref="T:Gizmox.WebGUI.Forms.ListBox"></see>.
        ///// </summary>
        /////	<returns>A zero-based index of the currently selected item. A value of negative one (-1) is returned if no item is selected.</returns>
        /////	<exception cref="T:System.ArgumentException">The <see cref="P:Gizmox.WebGUI.Forms.ListBox.SelectionMode"></see> property is set to None.</exception>
        /////	<exception cref="T:System.ArgumentOutOfRangeException">The assigned value is less than -1 or greater than or equal to the item count.</exception>
        ///// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        //[DefaultValue(-1)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), Bindable(true)]
        //int SelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the currently selected item in the ListBox
        /// </summary>
        ///	<returns>An object that represents the current selection in the control.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        [DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden), Browsable(false)
        , Bindable(true)]
        object SelectedItem { get; set; }

        /// <summary>
        /// Gets a collection containing the currently selected items in the ListBox
        /// </summary>
        IListBoxSelectedObjectCollection SelectedItems { get; }

        /// <summary>
        /// Gets or sets the method in which items are selected in the ListBox
        /// </summary>
        ListBoxSelectionMode SelectionMode { get; set; }

        /// <summary>
        /// Selects or clears the selection for the specified item in a ListBox
        /// </summary>
        /// <param name="value">true to select the specified item; otherwise, false. </param>
        /// <param name="index">The zero-based index of the item in a ListBox
        /// to select or clear the selection for.</param>
        /// <filterpriority>1</filterpriority>
        void SetSelected(int index, bool value);

        /// <summary>
        /// Finds the first item in the ListBox that exactly matches the specified string
        /// </summary>
        /// <param name="strValue">The text to search for. </param>
        ///	<returns>The zero-based index of the first item found; returns ListBox.NoMatches if no match is found.</returns>
        int FindString(string strValue);

        /// <summary>
        /// Finds the first item in the System.Windows.Forms.ListBox that exactly
        /// matches the specified string. The search starts at a specific starting index.
        /// </summary>
        ///	<param name="strValue">The text to search for. </param>
        ///	<param name="intStartIndex">The zero-based index of the item before the first
        /// item to be searched. Set to negative one (-1) to search from the beginning of the control.</param>
        /// <returns>The zero-based index of the first item found; returns ListBox.NoMatches if no match is found.</returns>
        int FindString(string strValue, int intStartIndex);

        ///// <summary>
        ///// Swaps the items.
        ///// </summary>
        ///// <param name="intIndexA">The int index A.</param>
        ///// <param name="intIndexB">The int index B.</param>
        //void SwapItems(int intIndexA, int intIndexB);

        /// <summary>
        /// Gets or sets a value indicating whether the items in the ListBox are sorted alphabetically
        /// </summary>
        ///	<returns>true if items in the control are sorted; otherwise, false. The default is false.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" /><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" /></PermissionSet>
        [ DefaultValue(false)]
        bool Sorted { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether [check boxes].
        ///// </summary>
        ///// <value><c>true</c> if [check boxes]; otherwise, <c>false</c>.</value>
        //[DefaultValue(false)]
        //bool CheckBoxes { get; set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether [radio boxes].
        ///// </summary>
        ///// <value><c>true</c> if [radio boxes]; otherwise, <c>false</c>.</value>
        //[DefaultValue(false)]
        //bool RadioBoxes { get; set; }
        


        ///// <summary>
        ///// Gets a collection that contains the zero-based indexes of all currently selected items in the <see cref="T:Gizmox.WebGUI.Forms.ListBox"></see>.
        ///// </summary>
        /////	<returns>A <see cref="T:Gizmox.WebGUI.Forms.ListBox.SelectedIndexCollection"></see> containing the indexes of the currently selected items in the control. If no items are currently selected, an empty <see cref="T:Gizmox.WebGUI.Forms.ListBox.SelectedIndexCollection"></see> is returned.</returns>
        //[Browsable(false),
        // DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //ICollection SelectedIndices { get; }



        ///// <summary>
        ///// Gets a collection containing the currently selected items in the <see cref="T:Gizmox.WebGUI.Forms.ListBox"></see>.
        ///// </summary>
        /////	<returns>A <see cref="T:Gizmox.WebGUI.Forms.ListBox.SelectedObjectCollection"></see> containing the currently selected items in the control.</returns>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        // SRDescription("ListBoxSelectedItemsDescr"), Browsable(false)]
        //ListBox.SelectedObjectCollection SelectedItems { get; }
    }
}
