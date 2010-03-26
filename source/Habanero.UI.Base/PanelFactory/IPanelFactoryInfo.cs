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
using System.Collections.Generic;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a panel that has been created to view or edit business object
    /// details on a form.  This object is created when you call
    /// <see cref="IPanelFactory.CreatePanel"/>, and contains references to the
    /// controls, mappers, the panel control itself and the business object.
    /// </summary>
    [Obsolete("Panelfactory and PanelFactoryInfo is soon to be replaced by PanelBuilder and PanelInfo.")]
    public interface IPanelFactoryInfo
    {
        /// <summary>
        /// Returns the panel control
        /// </summary>
        IPanel Panel { get; }

        /// <summary>
        /// Returns the collection of control mappers, which map individual
        /// controls to the properties on the business object
        /// </summary>
        IControlMapperCollection ControlMappers { get; }

        /// <summary>
        /// Gets and sets the preferred height setting
        /// </summary>
        int PreferredHeight { get; set; }

        /// <summary>
        /// Gets and sets the preferred width setting
        /// </summary>
        int PreferredWidth { get; set; }

        /// <summary>
        /// Returns the first control to focus on in the user interface
        /// </summary>
        IControlHabanero FirstControlToFocus { get; }

        /// <summary>
        /// Gets and sets the tooltip for the panel
        /// </summary>
        IToolTip ToolTip { get; set; }

        /// <summary>
        /// Gets and sets the minimum height for the panel
        /// </summary>
        int MinimumPanelHeight { get; set; }

        /// <summary>
        /// Gets and sets the minimum width for the panel
        /// </summary>
        int MinumumPanelWidth { get; set; }

        /// <summary>
        /// Gets and sets the text for the panel tab
        /// </summary>
        string PanelTabText { get; set; }

        /// <summary>
        /// Gets and sets the UIForm definition used to construct the
        /// panel - this is taken from the class definitions for the
        /// business object
        /// </summary>
        IUIForm UIForm { get; set; }

        /// <summary>
        /// Gets the UIFormTab definition used to construct the panel
        /// for a single tab in the form.  By default, there is one
        /// tab for a form, even if it has not been explicitly defined.
        /// </summary>
        IUIFormTab UiFormTab { get; set; }

        /// <summary>
        /// Gets the UI definition name as set in the "name" attribute.
        /// Multiple definitions are permitted for a business object.  If none
        /// is specified, the "default" definition will be used, which is a
        /// UI definition without a "name" attribute.
        /// </summary>
        string UIDefName { get; }

        /// <summary>
        /// Gets the form grids
        /// </summary>
        IDictionary<string, IEditableGridControl> FormGrids { get; set; }

        ///<summary>
        /// Constructs and returns an <see cref="IEditableGridControl"/> for a specified relationship.
        ///</summary>
        ///<param name="relationShipName"></param>
        ///<returns></returns>
        IEditableGridControl GetFormGrid(string relationShipName);
    }
}