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

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Lists the available buttons for a multi-selector
    /// </summary>
    public enum MultiSelectorButton
    {
        /// <summary>
        /// Copies the highlighted item(s) to the selections list
        /// </summary>
        Select,
        /// <summary>
        /// Removes the highlighted item(s) from the selections list
        /// </summary>
        Deselect,
        /// <summary>
        /// Copies all available items to the selections list
        /// </summary>
        SelectAll,
        /// <summary>
        /// Removes all available items from the selections list
        /// </summary>
        DeselectAll
    }

    /// <summary>
    /// Provides a multiselector control. The type to be displayed in the 
    /// lists is set by the template type.  The multiselector helps the user to
    /// select from an available list of options.  Unselected options appear on the
    /// left and selected ones appear on the right.  The AllOptions consists
    /// of all options, both selected and unselected - no object may appear in the
    /// selected list if it is not also in the AllOptions list.  All list
    /// control is managed through the Model object.
    /// </summary>
    public interface IMultiSelector<T> : IControlChilli
    {
        /// <summary>
        /// Gets and sets the complete list of options available to go in
        /// either panel.  SelectedOptions must also reside in this list.
        /// AvailableOptions will be calculated as the remaining options when
        /// SelectedOptions are taken from this list.
        /// </summary>
        List<T> AllOptions { get; set; }

        /// <summary>
        /// Gets the ListBox control that contains the available options that
        /// have not been selected
        /// </summary>
        IListBox AvailableOptionsListBox { get; }

        /// <summary>
        /// Gets the model that manages the options
        /// </summary>
        MultiSelectorModel<T> Model { get; }

        ///<summary>
        /// Gets or sets the list of items already selected (which is a subset of
        /// AllOptions).  This list typically appears on the right-hand side.
        ///</summary>
        List<T> SelectedOptions { get; set; }

        /// <summary>
        /// Gets the ListBox control that contains the options that have been
        /// selected from those available
        /// </summary>
        IListBox SelectionsListBox { get; }

        /// <summary>
        /// Gets the button control as indicated by the <see cref="MultiSelectorButton"/> enumeration.
        /// </summary>
        /// <param name="buttonType">The type of button</param>
        /// <returns>Returns a button</returns>
        IButton GetButton(MultiSelectorButton buttonType);

        /// <summary>
        /// Gets a view of the SelectedOptions collection
        /// </summary>
        ReadOnlyCollection<T> SelectionsView { get;}
    }
}
