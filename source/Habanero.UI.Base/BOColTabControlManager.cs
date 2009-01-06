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
using Habanero.BO;

namespace Habanero.UI.Base
{
    /// <summary>
    /// This manager groups common logic for IBOColTabControl objects.
    /// Do not use this object in working code - rather call CreateBOColTabControl
    /// in the appropriate control factory.
    /// </summary>
    public class BOColTabControlManager
    {
        private ITabControl _tabControl;
        private readonly IControlFactory _controlFactory;
        private Dictionary<ITabPage, IBusinessObject> _pageBoTable;
        private Dictionary<IBusinessObject, ITabPage> _boPageTable;
        private IBusinessObjectControl _boControl;
        private IBusinessObjectCollection _businessObjectCollection;

        public BOColTabControlManager(ITabControl tabControl, IControlFactory controlFactory)
        {
            //BorderLayoutManager manager = new BorderLayoutManager(this);
            _tabControl = tabControl;
            _controlFactory = controlFactory;
            //manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _pageBoTable = new Dictionary<ITabPage, IBusinessObject>();
            _boPageTable = new Dictionary<IBusinessObject, ITabPage>();
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
                if (_boControl != null)
                {
                    BorderLayoutManager manager = _controlFactory.CreateBorderLayoutManager(TabControl);
                    ITabPage tabPage = _controlFactory.CreateTabPage(_boControl.Text);
                    _boControl.Dock = DockStyle.Fill;
                    tabPage.Controls.Add(_boControl);
                    //_pageBoTable.Add(tabPage,_boControl.BusinessObject);
                    //_boPageTable.Add(_boControl.BusinessObject,tabPage);
                    manager.AddControl(tabPage, BorderLayoutManager.Position.Centre);
                    
                }
                else
                {
                    throw new ArgumentException("boControl must be of type IControlHabanero or one of its subtypes.");
                }
            }
            get { return _boControl; }
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="value">The business object collection to create tab pages
        /// for</param>
        public IBusinessObjectCollection BusinessObjectCollection
        {
            set
            {
                _businessObjectCollection = value;
                ReloadCurrentCollection();
            }
            get { return _businessObjectCollection; }
        }

        private void ReloadCurrentCollection()
        {
            _tabControl.SelectedIndexChanged -= TabChangedHandler;
            ClearTabPages();

            if (_businessObjectCollection == null)
            {
                _tabControl.SelectedIndexChanged += TabChangedHandler;
                return;
            }
            foreach (BusinessObject bo in _businessObjectCollection)
            {
                ITabPage page = _controlFactory.CreateTabPage(bo.ToString());
                //page.Text =  ;
                AddTabPage(page, bo);
            }
            if (_businessObjectCollection.Count > 0)
            {
                _tabControl.SelectedIndex = 0;
            }
            _tabControl.SelectedIndexChanged += TabChangedHandler;
            TabChanged();
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
        /// Carries out additional steps when the user selects a different tab
        /// </summary>
        public virtual void TabChanged()
        {
            if (_tabControl.SelectedTab != null)
            {
                _tabControl.SelectedTab.Controls.Clear();
                _tabControl.SelectedTab.Controls.Add(_boControl);
                if (_boControl != null)
                {
                    _boControl.BusinessObject = GetBo(_tabControl.SelectedTab);
                   // _boControl.Dock = DockStyle.Fill;
                    _tabControl.SelectedTab.Controls.Add(_boControl);
                    
                }
            }
        }

//        protected virtual Dictionary<string, object> GetBusinessObjectDisplayValueDictionary()
//        {
//            return BusinessObjectLookupList.CreateDisplayValueDictionary(_businessObjectCollection, false);
//        }

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
            if (tabPage == null) return null;

            if (_pageBoTable.ContainsKey(tabPage))
            {
                return _pageBoTable[tabPage];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public IBusinessObject CurrentBusinessObject
        {
            get
            {
                if (_businessObjectCollection == null)
                {
                    return null;
                }
                int tabIndex = TabControl.TabPages.IndexOf(TabControl.SelectedTab);
                if (tabIndex == -1) return null;
                else
                {
                    return _businessObjectCollection[tabIndex];
                }
            }
            set
            {
                if (value == null) return;
                TabControl.SelectedIndex = _businessObjectCollection.IndexOf(value);
                _boControl.BusinessObject = value;
            }
        }

        public Dictionary<ITabPage, IBusinessObject> PageBoTable
        {
            get { return _pageBoTable; }
        }

        public Dictionary<IBusinessObject, ITabPage> BoPageTable
        {
            get { return _boPageTable; }
        }

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
            _tabControl.TabPages.Add(page);
        }

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public virtual ITabPage GetTabPage(IBusinessObject bo)
        {
            if (_boPageTable.ContainsKey(bo))
            {
                return _boPageTable[bo];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Clears the tab pages
        /// </summary>
        protected virtual void ClearTabPages()
        {
            _tabControl.Controls.Clear();
            //_tabControl.TabPages.Clear();
            _pageBoTable.Clear();
            _boPageTable.Clear();
        }
    }
}