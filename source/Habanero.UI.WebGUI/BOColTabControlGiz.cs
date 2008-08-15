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
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    /// <summary>
    /// Displays a business object collection in a tab control, with one
    /// business object per tab.  Each tab holds a business control, provided
    /// by the developer, that refreshes to display the business object for
    /// the current tab.
    /// <br/>
    /// This control is suitable for a business object collection with a limited
    /// number of objects.
    /// </summary>
    public class BOColTabControlGiz : UserControlGiz, IBOColTabControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly ITabControl _tabControl;
        private readonly BOColTabControlManager _boColTabControlManager;

        /// <summary>
        /// Constructor to initialise a new tab control
        /// </summary>
        public BOColTabControlGiz(IControlFactory controlFactory)
        {
            _controlFactory = controlFactory;
            BorderLayoutManager manager = _controlFactory.CreateBorderLayoutManager(this);
            _tabControl = _controlFactory.CreateTabControl();
            manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _boColTabControlManager = new BOColTabControlManager(_tabControl, _controlFactory);
        }

        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.
        /// </summary>
        /// <param name="value">The business object control that is
        /// displaying the business object information in the tab page</param>
        public IBusinessObjectControl BusinessObjectControl
        {
            get { return _boColTabControlManager.BusinessObjectControl; }
            set { BOColTabControlManager.BusinessObjectControl = value; }
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="value">The business object collection to create tab pages
        /// for</param>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return BOColTabControlManager.BusinessObjectCollection; }
            set { BOColTabControlManager.BusinessObjectCollection = value; }
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
            return BOColTabControlManager.GetBo(tabPage);
        }

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public ITabPage GetTabPage(IBusinessObject bo)
        {
            return BOColTabControlManager.GetTabPage(bo);
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public IBusinessObject CurrentBusinessObject
        {
            get { return BOColTabControlManager.CurrentBusinessObject; }
            set { BOColTabControlManager.CurrentBusinessObject = value; }
        }

        /// <summary>
        /// Returns the manager that provides logic common to all
        /// UI environments
        /// </summary>
        private BOColTabControlManager BOColTabControlManager
        {
            get { return _boColTabControlManager; }
        }

    }
}