//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
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
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Manages the panel object for a user interface. This is a class that stores the information
    /// regarding a panel that is being created by the panel factory e.g panel height. 
    /// i.e. the panel factory creates this
    ///    class which describes and contains the panel created by the factory.
    /// </summary>
    public class PanelFactoryInfo : IPanelFactoryInfo
    {
        private readonly IPanel _panel;
        private readonly IControlMapperCollection _mappers;
        private readonly IControlChilli _firstControlToFocus;
        private int _preferredHeight;
        private int _preferredWidth;
        //private IDictionary<string, EditableGrid> _formGrids;
        private IToolTip _toolTip;
        private int _minimumPanelHeight;
        private int _minumumPanelWidth;
        private string _panelTabTest;
        private UIForm _uiForm;
        private UIFormTab _uiFormTab;

        /// <summary>
        /// Constructor to initialise a new instance of the class
        /// </summary>
        /// <param name="panel">The panel object being managed</param>
        /// <param name="mappers">The control mappers collection</param>
        /// <param name="firstControlToFocus">The first control to focus on</param>
        public PanelFactoryInfo(IPanel panel, IControlMapperCollection mappers, IControlChilli firstControlToFocus)
        {
            _panel = panel;
            _mappers = mappers;
            _firstControlToFocus = firstControlToFocus;
        }

        /// <summary>
        /// A constructor as before, but with only the panel specified
        /// </summary>
        public PanelFactoryInfo(IPanel panel)
            : this(panel, new ControlMapperCollection(), null)
        {
        }

        /// <summary>
        /// Returns the panel object
        /// </summary>
        public IPanel Panel
        {
            get { return _panel; }
        }

        /// <summary>
        /// Returns the collection of control mappers
        /// </summary>
        public IControlMapperCollection ControlMappers
        {
            get { return _mappers; }
        }

        /// <summary>
        /// Gets and sets the preferred height setting
        /// </summary>
        public int PreferredHeight
        {
            get { return _preferredHeight; }
            set { _preferredHeight = value; }
        }

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
        public IControlChilli FirstControlToFocus
        {
            get { return _firstControlToFocus; }
        }

        public IToolTip ToolTip
        {
            get { return _toolTip; }
            set { _toolTip = value; }
        }

        public int MinimumPanelHeight
        {
            get { return _minimumPanelHeight; }
            set { _minimumPanelHeight = value; }
        }

        public int MinumumPanelWidth
        {
            get { return _minumumPanelWidth; }
            set { _minumumPanelWidth = value; }
        }

        public string PanelTabText
        {
            get { return _panelTabTest; }
            set { _panelTabTest = value; }
        }

        public UIForm UIForm
        {
            get { return _uiForm; }
            set { _uiForm = value; }
        }

        public UIFormTab UiFormTab
        {
            get { return _uiFormTab; }
            set { _uiFormTab = value; }
        }
    }
}