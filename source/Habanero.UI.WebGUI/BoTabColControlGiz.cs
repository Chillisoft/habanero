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


using System;
using System.Drawing;
using Habanero.Base;
using Habanero.BO;
using Habanero.UI;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Manages a collection of tab pages that hold business object controls
    /// </summary>
    public class BoTabColControlGiz : UserControlGiz, IBoTabColControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly ITabControl _tabControl;
        private readonly CollectionTabControlMapper _collectionTabControlMapper;

        /// <summary>
        /// Constructor to initialise a new tab control
        /// </summary>
        public BoTabColControlGiz(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            BorderLayoutManager manager = _controlFactory.CreateBorderLayoutManager(this);
            _tabControl = _controlFactory.CreateTabControl();
            manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _collectionTabControlMapper = new CollectionTabControlMapper(_tabControl, _controlFactory);
        }

        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.
        /// </summary>
        /// <param name="boControl">The business object control that is
        /// displaying the business object information in the tab page</param>
        public void SetBusinessObjectControl(IBusinessObjectControl boControl)
        {
            CollectionTabControlMapper.SetBusinessObjectControl(boControl);
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="businessObjectCollection">The business object collection to create tab pages
        /// for</param>
        public void SetCollection(IBusinessObjectCollection businessObjectCollection)
        {
            CollectionTabControlMapper.SetCollection(businessObjectCollection);
        }

        /// <summary>
        /// Returns the TabControl object
        /// </summary>
        public ITabControl TabControl
        {
            get { return _tabControl; }
        }

        /// <summary>
        /// Returns the business object represented in the specified tab page
        /// </summary>
        /// <param name="tabPage">The tab page</param>
        /// <returns>Returns the business object, or null if not available
        /// </returns>
        public IBusinessObject GetBo(ITabPage tabPage)
        {
            return CollectionTabControlMapper.GetBo(tabPage);
        }

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public ITabPage GetTabPage(IBusinessObject bo)
        {
            return CollectionTabControlMapper.GetTabPage(bo);
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public IBusinessObject CurrentBusinessObject
        {
            get { return CollectionTabControlMapper.CurrentBusinessObject; }
            set { CollectionTabControlMapper.CurrentBusinessObject = value; }
        }

        public CollectionTabControlMapper CollectionTabControlMapper
        {
            get { return _collectionTabControlMapper; }
        }

    }
}