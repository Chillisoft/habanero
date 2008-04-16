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


using System.Windows.Forms;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Provides control for panels that represent a business object
    /// in a user interface.
    /// </summary>
    public class BoPanelControl : UserControl
    {
        private BusinessObject _bo;
        private string _uiDefName;
        private PanelFactoryInfo _panelFactoryInfo;
        private Panel _boPanel;

        /// <summary>
        /// Constructor to create a new control object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <param name="uiDefName">A string name for the control</param>
        public BoPanelControl(BusinessObject bo, string uiDefName) 
        {
            _bo = bo;
            _uiDefName = uiDefName;

            BOMapper mapper = new BOMapper(bo);
			
            UIForm def = (_uiDefName.Length > 0) 
                                ? mapper.GetUIDef(_uiDefName).GetUIFormProperties() 
                                : mapper.GetUIDef().GetUIFormProperties();

            PanelFactory factory = new PanelFactory(_bo, def );
            _panelFactoryInfo = factory.CreatePanel() ;
            _boPanel = _panelFactoryInfo.Panel ;

            BorderLayoutManager manager = new BorderLayoutManager(this) ;
            manager.AddControl(_boPanel, BorderLayoutManager.Position.Centre ) ;
        }

        /// <summary>
        /// Returns the panel object being controlled
        /// </summary>
        public Panel Panel {
            get { return _boPanel; }
        }

        /// <summary>
        /// Returns the panel factory object
        /// </summary>
        public PanelFactoryInfo PanelFactoryInfo
        {
            get { return _panelFactoryInfo; }
        }

        /// <summary>
        /// Sets the business object to be represented
        /// </summary>
        /// <param name="bo">The business object</param>
        public void SetBusinessObject(BusinessObject bo)
        {
            _panelFactoryInfo.ControlMappers.BusinessObject = bo;
        }
    }
}