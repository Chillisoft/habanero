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

using Habanero.Base;

namespace Habanero.UI.Base
{
    ///<summary>
    /// Interface for a control that displays a collection of a Business Object along side an editor/creator panel.
    /// The collection of business objects can be shown using any selector control e.g. an <see cref="IEditableGridControl"/>,
    ///   <see cref="IGridControl"/> etc.
    ///</summary>
    public interface IGridAndBOEditorControl : IControlHabanero
    {
        /// <summary>
        /// Sets the business object collection to populate the grid.  If the grid
        /// needs to be cleared, set an empty collection rather than setting to null.
        /// Until you set a collection, the controls are disabled, since any given
        /// collection needs to be provided by a suitable context.
        /// </summary>
        void SetBusinessObjectCollection(IBusinessObjectCollection col);
        /// <summary>
        /// Returns the <see cref="IGridControl"/> that is being used along side of the <see cref="IBusinessObjectControlWithErrorDisplay"/>
        ///  to provide bo editing behaviour.
        /// </summary>
        IGridControl GridControl { get; }
        /// <summary>
        /// The <see cref="IBusinessObjectControlWithErrorDisplay"/> that is being used to 
        /// edit the <see cref="IBusinessObject"/>.
        /// </summary>
        IBusinessObjectControlWithErrorDisplay BusinessObjectControl { get; }
        /// <summary>
        /// The <see cref="IButtonGroupControl"/> that is has the individual buttons that
        ///   are shown at the bottom of this control.
        /// </summary>
        IButtonGroupControl ButtonGroupControl { get; }
        /// <summary>
        /// Method to create a new Business Object that is part of the collection.
        /// </summary>
        IBusinessObject CurrentBusinessObject { get; }
    }
}