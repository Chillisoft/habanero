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
    public enum MultiSelectorButton
    {
        Select,
        Deselect,
        SelectAll,
        DeselectAll
    }
    /// <summary>
    /// Provides a multiselector control. The type to be displayed in the 
    /// lists is set by the template type.
    /// </summary>
    public interface IMultiSelector<T> :IControlChilli
    {
        List<T> Options { get; set; }

        IListBox AvailableOptionsListBox { get; }

        MultiSelectorModel<T> Model { get; }
        ///<summary>
        /// Gets or sets the list of selectioned items (i.e. the items in the right hand listbox
        ///</summary>
        List<T> Selections { get; set; }
        IListBox SelectionsListBox { get; }
        IButton GetButton(MultiSelectorButton buttonType);
        ReadOnlyCollection<T> SelectionsView { get;}
    }
}
