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
using System.Collections.Generic;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages a panel that has been created to view or edit business object
    /// details on a form.  This object is created when you call
    /// <see cref="IPanelFactory.CreatePanel"/>, and contains references to the
    /// controls, mappers, the panel control itself and the business object.
    /// </summary>
    [Obsolete("Panelfactory and PanelFactoryInfo is soon to be replaced by PanelBuilder and PanelInfo.")]
    public class PanelFactoryInfo : IPanelFactoryInfo
    {
        private readonly IPanel _panel;
        private readonly IControlMapperCollection _mappers;
        private readonly IControlHabanero _firstControlToFocus;
        private int _preferredWidth;
        private IDictionary<string, IEditableGridControl> _formGrids;
        private IToolTip _toolTip;
        private int _minimumPanelHeight;
        private int _minumumPanelWidth;
        private string _panelTabTest;
        private IUIForm _uiForm;
        private IUIFormTab _uiFormTab;
        private readonly string _uiDefName;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="panel">The panel control being managed</param>
        /// <param name="mappers">The control mappers collection</param>
        /// <param name="uiDefName">The UI definition name to use</param>
        /// <param name="firstControlToFocus">The first control to focus on</param>
        public PanelFactoryInfo(IPanel panel, IControlMapperCollection mappers, string uiDefName, IControlHabanero firstControlToFocus)
        {
            _panel = panel;
            _mappers = mappers;
            _firstControlToFocus = firstControlToFocus;
            _uiDefName = uiDefName;
            _formGrids = new Dictionary<string, IEditableGridControl>();
        }

        /// <summary>
        /// A constructor as before, but with only the panel specified
        /// </summary>
        public PanelFactoryInfo(IPanel panel)
            : this(panel, new ControlMapperCollection(), null, null)
        {
        }

        /// <summary>
        /// Returns the panel control
        /// </summary>
        public IPanel Panel
        {
            get { return _panel; }
        }

        /// <summary>
        /// Returns the collection of control mappers, which map individual
        /// controls to the properties on the business object
        /// </summary>
        public IControlMapperCollection ControlMappers
        {
            get { return _mappers; }
        }


        /// <summary>
        /// Gets and sets the form grids
        /// </summary>
        // TODO : This should not have a set method, this should be populated some other way
        public IDictionary<string, IEditableGridControl> FormGrids
        {
            get
            {
                return _formGrids;
            }
            set
            {
                _formGrids = value;
            }
        }

        ///<summary>
        /// Constructs and returns an <see cref="IEditableGridControl"/> for a specified relationship.
        ///</summary>
        ///<param name="relationShipName"></param>
        ///<returns></returns>
        public IEditableGridControl GetFormGrid(string relationShipName)
        {
            return _formGrids[relationShipName];
        }

        /// <summary>
        /// Gets and sets the preferred height setting
        /// </summary>
        public int PreferredHeight { get; set; }

        /// <summary>
        /// Gets and sets the preferred width setting
        /// </summary>
        public int PreferredWidth
        {
            get { return _preferredWidth; }
            set { _preferredWidth = value; }
        }

        /// <summary>
        /// Returns the first control to focus on in the user interface
        /// </summary>
        public IControlHabanero FirstControlToFocus
        {
            get { return _firstControlToFocus; }
        }

        /// <summary>
        /// Gets and sets the tooltip for the panel
        /// </summary>
        public IToolTip ToolTip
        {
            get { return _toolTip; }
            set { _toolTip = value; }
        }

        /// <summary>
        /// Gets and sets the minimum height for the panel
        /// </summary>
        public int MinimumPanelHeight
        {
            get { return _minimumPanelHeight; }
            set { _minimumPanelHeight = value; }
        }

        /// <summary>
        /// Gets and sets the minimum width for the panel
        /// </summary>
        public int MinumumPanelWidth
        {
            get { return _minumumPanelWidth; }
            set { _minumumPanelWidth = value; }
        }

        /// <summary>
        /// Gets and sets the text for the panel tab
        /// </summary>
        public string PanelTabText
        {
            get { return _panelTabTest; }
            set { _panelTabTest = value; }
        }

        /// <summary>
        /// Gets and sets the UIForm definition used to construct the
        /// panel - this is taken from the class definitions for the
        /// business object
        /// </summary>
        public IUIForm UIForm
        {
            get { return _uiForm; }
            set { _uiForm = value; }
        }

        /// <summary>
        /// Gets the UIFormTab definition used to construct the panel
        /// for a single tab in the form.  By default, there is one
        /// tab for a form, even if it has not been explicitly defined.
        /// </summary>
        public IUIFormTab UiFormTab
        {
            get { return _uiFormTab; }
            set { _uiFormTab = value; }
        }

        /// <summary>
        /// Gets the UI definition name as set in the "name" attribute.
        /// Multiple definitions are permitted for a business object.  If none
        /// is specified, the "default" definition will be used, which is a
        /// UI definition without a "name" attribute.
        /// </summary>
        public string UIDefName
        {
            get { return _uiDefName; }
        }
    }
}