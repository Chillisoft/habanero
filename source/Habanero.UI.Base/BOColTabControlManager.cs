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
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides arguments to attach for an event involving business objects
    /// </summary>
    public class TabPageEventArgs : EventArgs
    {
        private readonly ITabPage _tabPage;
        private readonly IBusinessObjectControl _boControl;

        /// <summary>
        /// Constructor to initialise a new set of arguments
        /// </summary>
        /// <param name="tabPage">The related business object</param>
        /// <param name="boControl"></param>
        public TabPageEventArgs(ITabPage tabPage, IBusinessObjectControl boControl)
        {
            _tabPage = tabPage;
            _boControl = boControl;
        }

        /// <summary>
        /// Returns the Tab Page related to the event
        /// </summary>
        public ITabPage TabPage
        {
            get { return _tabPage; }
        }

        ///<summary>
        /// Returns the <see cref="IBusinessObjectControl"/> associated with this Event.
        ///</summary>
        public IBusinessObjectControl BOControl
        {
            get { return _boControl; }
        }
    }
    /// <summary>
    /// This is a delegate for creating a  Business Object Control.<br/>
    ///  This can be used as an alternate to setting the control
    /// on the <see cref="IBOColTabControl"/> so that a different instance of the control
    ///  is created for each tab instead of them  using the same control with diff data.<br/>
    /// This has been created for performance reasons.
    /// </summary>
    public delegate IBusinessObjectControl BusinessObjectControlCreatorDelegate();

    /// <summary>
    /// This manager groups common logic for <see cref="IBOColTabControl"/>  objects.
    /// Do not use this object in working code - rather call CreateBOColTabControl
    /// in the appropriate control factory.
    /// <remarks>
    /// This Manager is an extract of common functionality required for the <see cref="IBOColTabControl"/> it is used to 
    /// as part of the pattern to isolate the implementation of the actual BOColTabControl from the code using the BOColTabControl.
    /// This allows the developer to swap <see cref="IBOColTabControl"/>s that support this interface without having to redevelop 
    /// any code.
    /// Habanero uses this to isolate the UIframework so that a different framework can be implemented
    /// using these interfaces. This allows swapping in custom controls as well total other control libraries without 
    ///  modifying the app.
    /// This allows the Architecture to swap between Visual Web Gui and Windows or in fact between any UI framework and
    /// any other UI Framework.
    /// </remarks>
    /// </summary>
    public class BOColTabControlManager
    {
        private readonly ITabControl _tabControl;
        private readonly IControlFactory _controlFactory;
        private readonly Dictionary<ITabPage, IBusinessObject> _pageBoTable;
        private readonly Dictionary<IBusinessObject, ITabPage> _boPageTable;
        private IBusinessObjectControl _boControl;
        private IBusinessObjectCollection _businessObjectCollection;
        /// <summary>
        /// Occurs when the collection in the grid is changed
        /// </summary>
        public event EventHandler<TabPageEventArgs> TabPageAdded;
        /// <summary>
        /// Occurs when the collection in the grid is changed
        /// </summary>
        public event EventHandler<TabPageEventArgs> TabPageRemoved;

        ///<summary>
        /// Constructor for the <see cref="BOColTabControlManager"/>
        ///</summary>
        ///<param name="tabControl"></param>
        ///<param name="controlFactory"></param>
        public BOColTabControlManager(ITabControl tabControl, IControlFactory controlFactory)
        {
            if (tabControl == null) throw new ArgumentNullException("tabControl");
            if (controlFactory == null) throw new ArgumentNullException("controlFactory");
            //BorderLayoutManager manager = new BorderLayoutManager(this);
            _tabControl = tabControl;
            _controlFactory = controlFactory;
            //manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _pageBoTable = new Dictionary<ITabPage, IBusinessObject>();
            _boPageTable = new Dictionary<IBusinessObject, ITabPage>();
            TabControl.SelectedIndexChanged += delegate { FireBusinessObjectSelected(); };
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
        /// Gets and sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.
        /// </summary>
        public IBusinessObjectControl BusinessObjectControl
        {
            set
            {
                _boControl = value;
                if (_boControl == null)
                {
                    throw new ArgumentException("boControl must be of type IControlHabanero or one of its subtypes.");
                }
                BorderLayoutManager layoutManager = _controlFactory.CreateBorderLayoutManager(TabControl);
                ITabPage tabPage = _controlFactory.CreateTabPage(BusinessObjectControl.Text);
                BusinessObjectControl.Dock = DockStyle.Fill;
                tabPage.Controls.Add(BusinessObjectControl);
                //_pageBoTable.Add(tabPage,_boControl.BusinessObject);
                //_boPageTable.Add(_boControl.BusinessObject,tabPage);
                layoutManager.AddControl(tabPage, BorderLayoutManager.Position.Centre);
            }
            get
            {
                if (_boControl != null) return _boControl;
                ITabPage selectedTab = this.TabControl.SelectedTab;
                if (selectedTab != null)
                {
                    return GetBusinessObjectControl(selectedTab);
                }
                return null;
            }
        }

        protected virtual IBusinessObjectControl GetBusinessObjectControl(ITabPage selectedTab)
        {
            if (selectedTab.Controls.Count > 0)
            {
                IControlHabanero control = selectedTab.Controls[0];
                if (typeof (IBusinessObjectControl).IsInstanceOfType(control))
                {
                    return (IBusinessObjectControl) control;
                }
            }
            return null;
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business (<see cref="IBusinessObjectCollection"/>)
        /// objects used to Create teh Tab Pages.
        /// </summary>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            set
            {
                CheckBusinessObjectControlSet("BusinessObjectCollection");
                DeregisterForBusinessObjectCollectionEvents();
                _businessObjectCollection = value;
                ReloadCurrentCollection();
            }
            get { return _businessObjectCollection; }
        }

        private void DeregisterForBusinessObjectCollectionEvents()
        {
            if (_businessObjectCollection != null)
            {
                _businessObjectCollection.BusinessObjectAdded -= BusinessObjectAddedHandler;
                _businessObjectCollection.BusinessObjectRemoved -= BusinessObjectRemovedHandler;
            }
        }

        private void ReloadCurrentCollection()
        {
            TabControl.SelectedIndexChanged -= TabChangedHandler;
            ClearTabPages();

            if (_businessObjectCollection == null)
            {
                TabControl.SelectedIndexChanged += TabChangedHandler;
                return;
            }
            foreach (BusinessObject bo in _businessObjectCollection)
            {
                AddTabPage(bo);
            }
            if (_businessObjectCollection.Count > 0)
            {
                TabControl.SelectedIndex = 0;
            }
            else
            {
                TabControl.SelectedIndex = -1;
            }
            TabControl.SelectedIndexChanged += TabChangedHandler;

            _businessObjectCollection.BusinessObjectAdded += BusinessObjectAddedHandler;
            _businessObjectCollection.BusinessObjectRemoved += BusinessObjectRemovedHandler;
            TabChanged();
        }

        private void AddTabPage(IBusinessObject bo)
        {
            ITabPage page = _controlFactory.CreateTabPage(bo.ToString());
            AddTabPage(page, bo);
        }

        /// <summary>
        /// Handles the event that the user chooses a different tab. Calls the
        /// TabChanged() method.
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void TabChangedHandler(object sender, EventArgs e)
        {
            TabChanged();
        }

        /// <summary>
        /// When the SelectedTab Changes manages the Logic of setting up the Business Object
        /// and the <see cref="IBusinessObjectControl"/> if appropriate.
        /// </summary>
        public virtual void TabChanged()
        {
            if (TabControl.SelectedTab == null) return;

            if (_boControl == null) return;
            TabControl.SelectedTab.Controls.Clear();
            TabControl.SelectedTab.Controls.Add(_boControl);
            if (_boControl == null) return;
            _boControl.BusinessObject = GetBo(TabControl.SelectedTab);
        }

        /// <summary>
        /// Returns the TabControl object
        /// </summary>
        internal ITabControl TabControl
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
            if (tabPage == null) return null;

            return _pageBoTable.ContainsKey(tabPage) ? _pageBoTable[tabPage] : null;
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public IBusinessObject CurrentBusinessObject
        {
            get
            {
                CheckBusinessObjectControlSet("CurrentBusinessObject");
                if (_businessObjectCollection == null)
                {
                    return null;
                }
                int tabIndex = TabControl.TabPages.IndexOf(TabControl.SelectedTab);
                return tabIndex == -1 ? null : _businessObjectCollection[tabIndex];
            }
            set
            {
                if (value == null) return; //The control is being swapped out 
                // onto each tab i.e. all the tabs use the Same BusinessObjectControl
                // setting the selected Bo to null is therefore not a particularly 
                // sensible action on a BOTabControl.
                CheckBusinessObjectColNotNull();
                TabControl.SelectedIndex = _businessObjectCollection.IndexOf(value);
                if (_boControl != null)
                {
                    _boControl.BusinessObject = value;
                }
            }
        }

        private void CheckBusinessObjectColNotNull()
        {
            if (_businessObjectCollection != null) return;

            const string expectedMessage =
                "You cannot set the 'CurrentBusinessObject' for BOColTabControlManager since the BusinessObjectCollection has not been set";
            throw new HabaneroDeveloperException(expectedMessage, expectedMessage);
        }

        private void CheckBusinessObjectControlSet(string methodName)
        {
            if (HasBOControlOrCreator()) return;

            string errMessage = "You cannot set the '" + methodName
                                + "' for BOColTabControlManager since the BusinessObjectControl has not been set";
            throw new HabaneroDeveloperException(errMessage, errMessage);
        }

        private bool HasBOControlOrCreator()
        {
            return _boControl != null || this.BusinessObjectControlCreator != null;
        }

        ///<summary>
        /// A dictionalry linking the Tab Page to the particular Business Object.
        ///</summary>
        public Dictionary<ITabPage, IBusinessObject> PageBoTable
        {
            get { return _pageBoTable; }
        }

        ///<summary>
        /// A dictionary linking the Business Object to a particular TabPage
        ///</summary>
        public Dictionary<IBusinessObject, ITabPage> BoPageTable
        {
            get { return _boPageTable; }
        }

        /// <summary>Gets the number of rows displayed in the <see cref="IBOColSelectorControl"></see>.</summary>
        /// <returns>The number of rows in the <see cref="IBOColSelectorControl"></see>.</returns>
        public int NoOfItems
        {
            get { return TabControl.TabPages.Count; }
        }

        ///<summary>
        /// 
        ///</summary>
        public BusinessObjectControlCreatorDelegate BusinessObjectControlCreator { get; set; }

        /// <summary>
        /// Adds a tab page to represent the given business object
        /// </summary>
        /// <param name="page">The TabPage object to add</param>
        /// <param name="bo">The business ojbect to represent</param>
        protected virtual void AddTabPage(ITabPage page, IBusinessObject bo)
        {
            if (_boControl != null)
            {
                _boControl.BusinessObject = bo;
                page.Controls.Add(_boControl);
            }
            else if (BusinessObjectControlCreator != null)
            {
                IBusinessObjectControl businessObjectControl = BusinessObjectControlCreator();
                businessObjectControl.Dock = DockStyle.Fill;
                businessObjectControl.BusinessObject = bo;
                page.Controls.Add(businessObjectControl);
            }
            AddTabPageToEnd(page);
            AddBoPageIndexing(bo, page);
        }

        /// <summary>
        /// Adds the necessagry indexing for a Business Object and TabPage relationship.
        /// </summary>
        /// <param name="bo">The Business Object related to the Tab Page</param>
        /// <param name="page">The Tab Page related to the Business Object</param>
        protected virtual void AddBoPageIndexing(IBusinessObject bo, ITabPage page)
        {
            _pageBoTable.Add(page, bo);
            _boPageTable.Add(bo, page);
        }

        /// <summary>
        /// Adds a tab page to the end of the tab order
        /// </summary>
        /// <param name="page">The Tab Page to be added to the Tab Control</param>
        protected virtual void AddTabPageToEnd(ITabPage page)
        {
            TabControl.TabPages.Add(page);
            IBusinessObjectControl boControl = this.GetBusinessObjectControl(page);
            FireTabPageAdded(page, boControl);
        }

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public virtual ITabPage GetTabPage(IBusinessObject bo)
        {
            return _boPageTable.ContainsKey(bo) ? _boPageTable[bo] : null;
        }

        /// <summary>
        /// Clears the Business Object collection and removes all tab pages.
        /// </summary>
        public virtual void Clear()
        {
            ClearTabPages();
            _businessObjectCollection = null;
        }

        /// <summary>
        /// Clears the tab pages
        /// </summary>
        protected virtual void ClearTabPages()
        {
            TabControl.Controls.Clear();
            _pageBoTable.Clear();
            _boPageTable.Clear();
        }

        /// <summary>
        /// Returns the business object at the specified row number
        /// </summary>
        /// <param name="row">The row number in question</param>
        /// <returns>Returns the busines object at that row, or null
        /// if none is found</returns>
        public IBusinessObject GetBusinessObjectAtRow(int row)
        {
            if (IndexOutOfRange(row)) return null;
            ITabPage page = TabControl.TabPages[row];
            return GetBo(page);
        }

        private bool IndexOutOfRange(int row)
        {
            return row < 0 || row >= NoOfItems;
        }

        #region For IBOColSelectorControl

        /// <summary>
        /// Event Occurs when a business object is selected
        /// </summary>
        public event EventHandler<BOEventArgs> BusinessObjectSelected;

        private void FireBusinessObjectSelected()
        {
            if (this.BusinessObjectSelected != null)
            {
                this.BusinessObjectSelected(this, new BOEventArgs(this.CurrentBusinessObject));
            }
        }

        /// <summary>
        /// This handler is called when a business object has been removed from
        /// the collection - it subsequently removes the item from the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectRemovedHandler(object sender, BOEventArgs e)
        {
            ITabPage page = GetTabPage(e.BusinessObject);
            TabControl.Controls.Remove(page);
            IBusinessObjectControl boControl = this.GetBusinessObjectControl(page);
            FireTabPageRemoved(page, boControl);
        }

        /// <summary>
        /// This handler is called when a business object has been added to
        /// the collection - it subsequently adds the item to the ListBox
        /// list as well.
        /// </summary>
        /// <param name="sender">The object that notified of the change</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void BusinessObjectAddedHandler(object sender, BOEventArgs e)
        {
            IBusinessObject busObject = e.BusinessObject;
            if (HasTabPage(busObject))return;
            AddTabPage(busObject);
        }

        private bool HasTabPage(IBusinessObject busObject)
        {
            return this.GetTabPage(busObject) != null;
        }

        #endregion
    }
}