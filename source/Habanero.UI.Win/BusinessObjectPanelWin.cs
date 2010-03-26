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
using Habanero.Base;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Represents a panel containing a PanelInfo used to edit a single business object.
    /// </summary>
    public class BusinessObjectPanelWin<T> : UserControlWin, IBusinessObjectPanel where T : class, IBusinessObject
    {
        private IPanelInfo _panelInfo;

        ///<summary>
        /// Constructor for <see cref="BusinessObjectPanelWin{T}"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        ///<param name="uiDefName"></param>
        public BusinessObjectPanelWin(IControlFactory controlFactory, string uiDefName)
        {
            CreateBOPanel(controlFactory, uiDefName);
        }

        private void CreateBOPanel(IControlFactory controlFactory, string uiDefName)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            if (string.IsNullOrEmpty(uiDefName)) throw new ArgumentNullException("uiDefName");
            PanelBuilder panelBuilder = new PanelBuilder(controlFactory);
            _panelInfo = panelBuilder.BuildPanelForForm(ClassDef.Get<T>().UIDefCol[uiDefName].UIForm);
            BorderLayoutManager layoutManager = controlFactory.CreateBorderLayoutManager(this);
            layoutManager.AddControl(_panelInfo.Panel, BorderLayoutManager.Position.Centre );
            this.Size = _panelInfo.Panel.Size;
            this.MinimumSize = _panelInfo.Panel.Size;
        }

        /// <summary>
        /// Gets or sets the business object being represented
        /// </summary>
        IBusinessObject IBusinessObjectControl.BusinessObject
        {
            get { return _panelInfo.BusinessObject; }
            set { _panelInfo.BusinessObject = value; }
        }

        /// <summary>
        /// Gets or sets the business object being represented
        /// </summary>
        public  T BusinessObject
        {
            get { return (T) _panelInfo.BusinessObject; }
            set { _panelInfo.BusinessObject = value; }
        }

        /// <summary>
        /// Gets and sets the PanelInfo object created by the control
        /// </summary>
        public IPanelInfo PanelInfo
        {
            get { return _panelInfo; }
            set { _panelInfo = value; }
        }
    }
}