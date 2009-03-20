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
using Habanero.UI.Base;

namespace Habanero.UI.VWG
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
    public class BOColTabControlVWG : UserControlVWG, IBOColTabControl
    {
        private readonly IControlFactory _controlFactory;
        private readonly ITabControl _tabControl;
        private readonly BOColTabControlManager _boColTabControlManager;

        /// <summary>
        /// Constructor to initialise a new tab control
        /// </summary>
        public BOColTabControlVWG(IControlFactory controlFactory)
        {
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            _controlFactory = controlFactory;
            BorderLayoutManager manager = _controlFactory.CreateBorderLayoutManager(this);
            _tabControl = _controlFactory.CreateTabControl();
            manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _boColTabControlManager = new BOColTabControlManager(_tabControl, _controlFactory);
            _boColTabControlManager.BusinessObjectSelected += delegate { FireBusinessObjectSelected(); };
            _boColTabControlManager.TabPageAdded += (sender, e) => FireTabPageAdded(e.TabPage, e.BOControl);
            _boColTabControlManager.TabPageRemoved += (sender, e) => FireTabPageRemoved(e.TabPage, e.BOControl);
        }
        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.SelectedBusinessObject));
            }
        }

        private void FireTabPageAdded(ITabPage tabPage, IBusinessObjectControl boControl)
        {
            if (this.TabPageAdded == null) return;

            TabPageEventArgs eventArgs = new TabPageEventArgs(tabPage, boControl);
            this.TabPageAdded(this, eventArgs);
        }
        private void FireTabPageRemoved(ITabPage tabPage, IBusinessObjectControl boControl)
        {
            if (this.TabPageRemoved == null) return;

            TabPageEventArgs eventArgs = new TabPageEventArgs(tabPage, boControl);
            this.TabPageRemoved(this, eventArgs);
        }
        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.<br/>
        /// The business object control that is
        /// displaying the business object information in the tab page
        /// </summary>
        public IBusinessObjectControl BusinessObjectControl
        {
            get { return _boColTabControlManager.BusinessObjectControl; }
            set { BOColTabControlManager.BusinessObjectControl = value; }
        }

        /// <summary>
        /// Occurs when the collection in the grid is changed
        /// </summary>
        public event EventHandler<TabPageEventArgs> TabPageAdded;

        /// <summary>
        /// Occurs when the collection in the grid is changed
        /// </summary>
        public event EventHandler<TabPageEventArgs> TabPageRemoved;

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided.<br/>
        /// The business object collection to create tab pages for
        /// </summary>
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
        /// Gets and Sets the Business Object Control Creator. This is a delegate for creating a
        ///  Business Object Control. This can be used as an alternate to setting the control
        /// on the <see cref="IBOColTabControl"/> so that a different instance of the control
        ///  is created for each tab instead of them  using the same control with diff data.
        /// This has been created for performance reasons.
        /// </summary>
        public BusinessObjectControlCreatorDelegate BusinessObjectControlCreator
        {
            get { return this.BOColTabControlManager.BusinessObjectControlCreator; }
            set { this.BOColTabControlManager.BusinessObjectControlCreator = value; }
        }

        /// <summary>
        /// Returns the manager that provides logic common to all
        /// UI environments
        /// </summary>
        private BOColTabControlManager BOColTabControlManager
        {
            get { return _boColTabControlManager; }
        }

        #region IBOColSelectorControl

        /// <summary>
        /// Gets and sets the currently selected business object in the grid
        /// </summary>
        public IBusinessObject SelectedBusinessObject
        {
            get { return CurrentBusinessObject; }
            set { CurrentBusinessObject = value; }
        }

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        /// <summary>
        /// Clears the business object collection and the rows in the data table
        /// </summary>
        public void Clear()
        {
            BOColTabControlManager.Clear();
        }

        /// <summary>Gets the number of rows displayed in the <see cref="IBOColSelectorControl"></see>.</summary>
        /// <returns>The number of rows in the <see cref="IBOColSelectorControl"></see>.</returns>
        public int NoOfItems
        {
            get { return this.BOColTabControlManager.NoOfItems; }
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            return this.BOColTabControlManager.GetBusinessObjectAtRow(row);
        }

        #endregion

    }
}