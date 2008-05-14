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
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.BO;
using Habanero.UI;
using Habanero.UI.Util;

namespace Habanero.UI.Forms
{
    /// <summary>
    /// This class provides mapping from a business object collection to a
    /// user interface TabControl.  This mapper is used at code level when
    /// you are explicitly providing a business object collection.
    /// </summary>
    public class CollectionTabControlMapper
    {

        //private delegate void VoidDelegate();
        //private delegate void SetCollectionDelegate(IList col);

        /// <summary>
        /// The internal variable representing the control that will be used to display a business object.
        /// </summary>
        protected IBusinessObjectControl _boControl;

        /// <summary>
        /// The internal variable that represents the Tab Control that the Business Object Collection is being mapped to.
        /// </summary>
        protected TabControl _tabControl;

        /// <summary>
        /// The internale variable representing the Business Object Collection that the Tab Control is representing.
        /// </summary>
        protected IBusinessObjectCollection _businessObjectCollection;

        /// <summary>
        /// The internal variable that represents the indexing of Business Objects by their Tab Pages.
        /// </summary>
        protected Dictionary<TabPage, BusinessObject> _pageBoTable;

        /// <summary>
        /// The internal variable that represents the indexing of Tab Pages by their Business Objects.
        /// </summary>
        protected Dictionary<BusinessObject, TabPage> _boPageTable;

        /// <summary>
        /// Constructor to initialise a new tab control
        /// </summary>
        public CollectionTabControlMapper(TabControl tabControl)
        {
           
            //BorderLayoutManager manager = new BorderLayoutManager(this);
            _tabControl = tabControl;
            //manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _pageBoTable = new Dictionary<TabPage, BusinessObject>();
            _boPageTable = new Dictionary<BusinessObject, TabPage>();
        }

        /// <summary>
        /// Sets the boControl that will be displayed on each tab page.  This must be called
        /// before the BoTabColControl can be used.
        /// </summary>
        /// <param name="boControl">The business object control that is
        /// displaying the business object information in the tab page</param>
        public void SetBusinessObjectControl(IBusinessObjectControl boControl)
        {
            _boControl = boControl;
            if (boControl is Control)
            {
                ((Control)_boControl).Dock = DockStyle.Fill;
            }
            else
            {
                throw new ArgumentException("boControl must be of type Control or one of its subtypes.");
            }
            
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="businessObjectCollection">The business object collection to create tab pages
        /// for</param>
        public void SetCollection(IBusinessObjectCollection businessObjectCollection)
        {
            _businessObjectCollection = businessObjectCollection;
            ReloadCurrentCollection();
        }

        /// <summary>
        /// Reloads the current Business Object collection.
        /// </summary>
        protected void ReloadCurrentCollection()
        {
            ControlsHelper.SafeGui(_tabControl, delegate()
            {
                ReloadCurrentCollectionInSTAThread(); 
            });
        }

        private void ReloadCurrentCollectionInSTAThread()
        {
            _tabControl.SelectedIndexChanged -= TabChangedHandler;
            ClearTabPages();
            Dictionary<string, object> list = GetBusinessObjectDisplayValueDictionary();
            foreach (KeyValuePair<string, object> pair in list)
            {
                BusinessObject businessObject = pair.Value as BusinessObject;
                if (businessObject != null)
                {
                    TabPage page = new TabPage(pair.Key);
                    //page.Text =  ;
                    AddTabPage(page, businessObject);
                }
            }
            //foreach (BusinessObject bo in businessObjectCollection)
            //{
            //    TabPage page = new TabPage(bo.ToString());
            //    //page.Text =  ;
            //    AddTabPage(page, bo);
            //}
            if (_businessObjectCollection.Count > 0)
            {
                _tabControl.SelectedIndex = 0;
            }
            _tabControl.SelectedIndexChanged += TabChangedHandler;
            TabChanged();
        }

        /// <summary>
        /// This method returns the display values and their corresponding objects that will be used for the Tabs text and contents.
        /// </summary>
        /// <returns>A display value dictionary containing the values to be used for the Tab titles and their Objects.</returns>
        protected virtual Dictionary<string, object> GetBusinessObjectDisplayValueDictionary()
        {
            return BusinessObjectLookupList.CreateDisplayValueDictionary(_businessObjectCollection, false);
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
            ControlsHelper.SafeGui(_tabControl, delegate()
            {
                if (_tabControl.SelectedTab != null)
                {
                    _tabControl.SelectedTab.Controls.Clear();
                    _tabControl.SelectedTab.Controls.Add((Control)_boControl);
                    _boControl.SetBusinessObject(GetBo(_tabControl.SelectedTab));
                } 
            });
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
        public virtual BusinessObject GetBo(TabPage tabPage)
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
        /// Adds a tab page to represent the given business object
        /// </summary>
        /// <param name="page">The TabPage object to add</param>
        /// <param name="bo">The business ojbect to represent</param>
        protected virtual void AddTabPage(TabPage page, BusinessObject bo)
        {
            AddTabPageToEnd(page);
            AddBoPageIndexing(bo, page);
        }

        /// <summary>
        /// Adds the necessagry indexing for a Business Object and TabPage relationship.
        /// </summary>
        /// <param name="bo">The Business Object related to the Tab Page</param>
        /// <param name="page">The Tab Page related to the Business Object</param>
        protected virtual void AddBoPageIndexing(BusinessObject bo, TabPage page)
        {
            _pageBoTable.Add(page, bo);
            _boPageTable.Add(bo, page);
        }

        /// <summary>
        /// Adds a tab page to the end of the tab order
        /// </summary>
        /// <param name="page">The Tab Page to be added to the Tab Control</param>
        protected virtual void AddTabPageToEnd(TabPage page)
        {
            _tabControl.TabPages.Add(page);
        }

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public virtual TabPage GetTabPage(BusinessObject bo)
        {
            if (_boPageTable.ContainsKey(bo))
            {
                return (TabPage) _boPageTable[bo];
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

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public virtual BusinessObject CurrentBusinessObject
        {
            get { return GetBo(_tabControl.SelectedTab); }
            set { _tabControl.SelectedTab = GetTabPage(value); }
        }
    }
}
