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
using Habanero.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Checks whether the user wants to save
    /// </summary>
    public delegate DialogResult ConfirmSave();

    /// <summary>
    /// Represents a control to edit a collection of business objects.  A grid
    /// lists the objects as specified by SetBusinessObjectCollection and a control
    /// below the grid allows the selected business object to be edited.  Default
    /// buttons are provided: Save, New, Delete and Cancel.
    /// <br/>
    /// The editing control is
    /// specified here as a IBusinessObjectControl, allowing the developer to pass
    /// in a custom control, but the default instantiation uses a IBusinessObjectPanel,
    /// which is more suited to displaying errors.  If the developer provides a custom
    /// control, they are responsible for updating the business object status
    /// and displaying useful feedback to the user (by
    /// catching appropriate events on the business object or the controls).
    /// <br/>
    /// Some customisation is provided through the GridWithPanelControlStrategy,
    /// including how controls should be enabled for the appropriate environment.
    /// </summary>
    [Obsolete("This has been replaced by IBOSelectorAndEditor : Brett 03 Mar 2009")]
    public interface IGridWithPanelControl<TBusinessObject> : IControlHabanero
    {
        /// <summary>
        /// Sets the business object collection to populate the grid.  If the grid
        /// needs to be cleared, set an empty collection rather than setting to null.
        /// Until you set a collection, the controls are disabled, since any given
        /// collection needs to be provided by a suitable context.
        /// </summary>
        void SetBusinessObjectCollection(IBusinessObjectCollection col);

        /// <summary>
        /// Gets the grid control
        /// </summary>
        IReadOnlyGridControl ReadOnlyGridControl { get; }

        /// <summary>
        /// Gets the control used to edit the selected business object
        /// </summary>
        IBusinessObjectControl BusinessObjectControl { get; }

        /// <summary>
        /// Gets the control holding the buttons
        /// </summary>
        IButtonGroupControl Buttons { get; }

        /// <summary>
        /// Gets the business object currently selected in the grid
        /// </summary>
        TBusinessObject CurrentBusinessObject { get; }

        /// <summary>
        /// Gets the strategy used to provide custom behaviour in the control
        /// </summary>
        IGridWithPanelControlStrategy<TBusinessObject> GridWithPanelControlStrategy { get; set; }

        /// <summary>
        /// Called when the user attempts to move away from a dirty business object
        /// and needs to indicate Yes/No/Cancel to the option of saving.  This delegate
        /// facility is provided primarily to facilitate testing.
        /// </summary>
        ConfirmSave ConfirmSaveDelegate { get; set;}
    }

    /// <summary>
    /// Provides a strategy to add custom behaviour to a GridWithPanelControl
    /// </summary>
    [Obsolete("This has been replaced by IBOEditorControl : Brett 03 Mar 2009")]
    public interface IGridWithPanelControlStrategy<TBusinessObject>
    {
        /// <summary>
        /// Provides custom control state.  Since this is called after the default
        /// implementation, it overrides it.
        /// </summary>
        /// <param name="lastSelectedBusinessObject">The previous selected business
        /// object in the grid - used to revert when a user tries to change a grid
        /// row while an object is dirty or invalid</param>
        void UpdateControlEnabledState(TBusinessObject lastSelectedBusinessObject);

        /// <summary>
        /// Whether to show the save confirmation dialog when moving away from
        /// a dirty object
        /// </summary>
        bool ShowConfirmSaveDialog { get; }

        /// <summary>
        /// Indicates whether PanelInfo.ApplyChangesToBusinessObject needs to be
        /// called to copy control values to the business object.  This will be
        /// the case if the application uses minimal events and does not update
        /// the BO every time a control value changes.
        /// </summary>
        bool CallApplyChangesToEditBusinessObject { get; }

        /// <summary>
        /// Indicates whether the grid should be refreshed.  For instance, a VWG
        /// implementation needs regular refreshes due to the lack of synchronisation,
        /// but this behaviour has some adverse affects in the WinForms implementation
        /// </summary>
        bool RefreshGrid { get; }
    }
}