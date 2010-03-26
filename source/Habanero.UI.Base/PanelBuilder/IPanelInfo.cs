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
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    ///<summary>
    /// A panel info is a class that wraps the panel and provides functionality 
    ///  for linking a business object a layout manager and a panel.
    ///</summary>
    public interface IPanelInfo
    {
        ///<summary>
        /// The panel that this panel info is controlling
        ///</summary>
        IPanel Panel { get; set; }
        ///<summary>
        /// Gets and sets the layout manager used for this Panel
        ///</summary>
        GridLayoutManager LayoutManager { get; set; }
        ///<summary>
        /// Returns a list of Field infos (info on the fields controlled by this panel.
        ///</summary>
        PanelInfo.FieldInfoCollection FieldInfos { get; }
        ///<summary>
        /// Sets the business object for this panel.
        ///</summary>
        IBusinessObject BusinessObject { get; set; }
        ///<summary>
        /// Sets whether the controls on this panel are enabled or not
        ///</summary>
        bool ControlsEnabled { set; }
        ///<summary>
        /// A list of all panel infos containd in this panel info.
        ///</summary>
        IList<IPanelInfo> PanelInfos { get; }
        ///<summary>
        /// Applies any changes in any control on this panel to the business object
        ///</summary>
        void ApplyChangesToBusinessObject();
        ///<summary>
        /// Clears any error providers for all controls visible on this panel
        ///</summary>
        void ClearErrorProviders();

        /// <summary>
        /// Gets the UIFormTab definition used to construct the panel
        /// for a single tab in the form.  By default, there is one
        /// tab for a form, even if it has not been explicitly defined.
        /// </summary>
        IUIFormTab UIFormTab { get; }

        /// <summary>
        /// Gets  the minimum height for the panel
        /// </summary>
        int MinimumPanelHeight { get; }

        /// <summary>
        /// Gets the UIForm definition used to construct the
        /// panel - this is taken from the class definitions for the
        /// business object
        /// </summary>
        IUIForm UIForm { get;  }

        /// <summary>
        /// Gets the text for the panel tab (UIFormTab.Name)
        /// </summary>
        string PanelTabText { get; }

        /// <summary>
        /// Sets the Error providers Error message with the appropriate message from the businessObject for each
        /// Control mapped on this panel.
        /// </summary>
        void UpdateErrorProvidersErrorMessages();
    }
}