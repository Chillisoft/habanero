using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public class BOTabControlMapper
    {
        private ITabControl _tabControl;
        private readonly IControlFactory _controlFactory;
        private IBusinessObject _currentBusinessObject;
        private Dictionary<ITabPage, BusinessObject> _pageBoTable;
        private Dictionary<BusinessObject, ITabPage> _boPageTable;
        private IBusinessObjectControl _boControl;
        private IBusinessObjectCollection _businessObjectCollection;

        public BOTabControlMapper(ITabControl tabControl, IControlFactory controlFactory)
        {
                 //BorderLayoutManager manager = new BorderLayoutManager(this);
            _tabControl = tabControl;
            _controlFactory = controlFactory;
            //manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _pageBoTable = new Dictionary<ITabPage, BusinessObject>();
            _boPageTable = new Dictionary<BusinessObject, ITabPage>();
        }

         //<summary>
         //Sets the boControl that will be displayed on each tab page.  This must be called
         //before the BoTabColControl can be used.
         //</summary>
         //<param name="boControl">The business object control that is
         //displaying the business object information in the tab page</param>
        public void SetBusinessObjectControl(IBusinessObjectControl boControl)
        {
            _boControl = boControl;
            if (boControl is IControlChilli)
            {
                BorderLayoutManager manager = _controlFactory.CreateBorderLayoutManager(TabControl);
                manager.AddControl(boControl, BorderLayoutManager.Position.Centre);
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

        private void ReloadCurrentCollection()
        {
            _tabControl.SelectedIndexChanged -= TabChangedHandler;
            ClearTabPages();
            Dictionary<string, object> list = GetBusinessObjectDisplayValueDictionary();
            foreach (KeyValuePair<string, object> pair in list)
            {
                BusinessObject businessObject = pair.Value as BusinessObject;
                if (businessObject != null)
                {
                    ITabPage page = _controlFactory.CreateTabPage(pair.Key);
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
                    _tabControl.SelectedTab.Controls.Add((IControlChilli)_boControl);
                    _boControl.SetBusinessObject(GetBo(_tabControl.SelectedTab));
                }

        }

        protected virtual Dictionary<string, object> GetBusinessObjectDisplayValueDictionary()
        {
            return BusinessObjectLookupList.CreateDisplayValueDictionary(_businessObjectCollection, false);
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
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public ITabPage GetTabPage(IBusinessObject bo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public IBusinessObject CurrentBusinessObject
        {
            get { return _currentBusinessObject; }
            set { _currentBusinessObject = value; }
        }

        public Dictionary<ITabPage, BusinessObject> PageBoTable
        {
            get { return _pageBoTable; }
        }

        public Dictionary<BusinessObject, ITabPage> BoPageTable
        {
            get { return _boPageTable; }
        }

        public IBusinessObjectControl BoControl
        {
            get { return _boControl; }
        }

        public IBusinessObjectCollection BusinessObjectCollection
        {
            get { return _businessObjectCollection; }
        }

        /// <summary>
        /// Adds a tab page to represent the given business object
        /// </summary>
        /// <param name="page">The TabPage object to add</param>
        /// <param name="bo">The business ojbect to represent</param>
        protected virtual void AddTabPage(ITabPage page, BusinessObject bo)
        {
            AddTabPageToEnd(page);
            AddBoPageIndexing(bo, page);
        }

        /// <summary>
        /// Adds the necessagry indexing for a Business Object and TabPage relationship.
        /// </summary>
        /// <param name="bo">The Business Object related to the Tab Page</param>
        /// <param name="page">The Tab Page related to the Business Object</param>
        protected virtual void AddBoPageIndexing(BusinessObject bo, ITabPage page)
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
        public virtual ITabPage GetTabPage(BusinessObject bo)
        {
            if (_boPageTable.ContainsKey(bo))
            {
                return (ITabPage)_boPageTable[bo];
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
