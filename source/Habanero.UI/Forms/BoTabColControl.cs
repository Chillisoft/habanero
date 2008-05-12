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
using Habanero.UI;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// Manages a collection of tab pages that hold business object controls
    /// </summary>
    public class BoTabColControl : UserControl
    {
        private TabControl _tabControl;
        private CollectionTabControlMapper _collectionTabControlMapper;

        /// <summary>
        /// Constructor to initialise a new tab control
        /// </summary>
        public BoTabColControl()
        {
            BorderLayoutManager manager = new BorderLayoutManager(this);
            _tabControl = new TabControl();
            manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _collectionTabControlMapper = new CollectionTabControlMapper(_tabControl);
        }

        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.
        /// </summary>
        /// <param name="boControl">The business object control that is
        /// displaying the business object information in the tab page</param>
        public void SetBusinessObjectControl(IBusinessObjectControl boControl)
        {
            _collectionTabControlMapper.SetBusinessObjectControl(boControl);
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="businessObjectCollection">The business object collection to create tab pages
        /// for</param>
        public void SetCollection(IBusinessObjectCollection businessObjectCollection)
        {
            _collectionTabControlMapper.SetCollection(businessObjectCollection);
        }

        /// <summary>
        /// Carries out additional steps when the user selects a different tab
        /// </summary>
        public void TabChanged()
        {
            _collectionTabControlMapper.TabChanged();
        }

        /// <summary>
        /// Returns the TabControl object
        /// </summary>
        public TabControl TabControl
        {
            get { return _tabControl; }
        }

        /// <summary>
        /// Returns the business object represented in the specified tab page
        /// </summary>
        /// <param name="tabPage">The tab page</param>
        /// <returns>Returns the business object, or null if not available
        /// </returns>
        public BusinessObject GetBo(TabPage tabPage)
        {
            return _collectionTabControlMapper.GetBo(tabPage);
        }

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public TabPage GetTabPage(BusinessObject bo)
        {
            return _collectionTabControlMapper.GetTabPage(bo);
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public BusinessObject CurrentBusinessObject
        {
            get { return _collectionTabControlMapper.CurrentBusinessObject; }
            set { _collectionTabControlMapper.CurrentBusinessObject = value; }
        }
    }
}