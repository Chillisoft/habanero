using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Bo;
using Habanero.Ui.Generic;

namespace Habanero.Ui.Application
{
    /// <summary>
    /// Manages a collection of tab pages that hold business object controls
    /// </summary>
    public class BoTabColControl : UserControl
    {
        //private delegate void AddTabPageDelegate(TabPage page, BusinessObjectBase bo);
        //private delegate void TabChangedDelegate();

        private readonly BusinessObjectControl _boControl;
        private TabControl _tabControl;
        private Hashtable _pageBoTable;
        private Hashtable _boPageTable;

        //private AddTabPageDelegate _addTabPageDelegate;
        //private TabChangedDelegate _tabChanged; 

        private EventHandler tabChangedHandler;

        /// <summary>
        /// Constructor to initialise a new tab control
        /// </summary>
        /// <param name="boControl">The business object control that is
        /// displaying the business object information in the tab page</param>
        public BoTabColControl(BusinessObjectControl boControl)
        {
            _boControl = boControl;
            if (boControl is Control)
            {
                ((Control) _boControl).Dock = DockStyle.Fill;
            }
            else
            {
                throw new ArgumentException("boControl must be of type Control or one of its subtypes.");
            }
            BorderLayoutManager manager = new BorderLayoutManager(this);
            _tabControl = new TabControl();
            manager.AddControl(_tabControl, BorderLayoutManager.Position.Centre);
            _pageBoTable = new Hashtable();
            _boPageTable = new Hashtable();

            tabChangedHandler = new EventHandler(TabChangedHandler);

            //_addTabPageDelegate = new AddTabPageDelegate(AddTabPageInSTAThread);
            //_tabChanged = new TabChangedDelegate(TabChangedInSTAThread) ;
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="col">The business object collection to create tab pages
        /// for</param>
        public void SetCollection(IList col)
        {
            _tabControl.SelectedIndexChanged -= tabChangedHandler;
            ClearTabPages();
            foreach (BusinessObject bo in col)
            {
                TabPage page = new TabPage(bo.ToString());
                //page.Text =  ;
                AddTabPage(page, bo);
            }


            if (col.Count > 0)
            {
                _tabControl.SelectedIndex = 0;
            }
            _tabControl.SelectedIndexChanged += tabChangedHandler;
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
        public void TabChanged()
        {
            if (_tabControl.SelectedTab != null)
            {
                //BeginInvoke(_tabChanged, new object[] {});
                _tabControl.SelectedTab.Controls.Clear();
                _tabControl.SelectedTab.Controls.Add((Control) _boControl);
                _boControl.SetBusinessObject(GetBo(_tabControl.SelectedTab));
            }
        }

        /// <summary>
        /// Carries out additional steps when the user selects a different tab
        /// </summary>
        /// TODO ERIC - sta thread?
        private void TabChangedInSTAThread()
        {
            _tabControl.SelectedTab.Controls.Clear();
            _tabControl.SelectedTab.Controls.Add((Control) _boControl);
            _boControl.SetBusinessObject(GetBo(_tabControl.SelectedTab));
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
            if (tabPage == null) return null;

            if (_pageBoTable.ContainsKey(tabPage))
            {
                return (BusinessObject) _pageBoTable[tabPage];
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
        private void AddTabPage(TabPage page, BusinessObject bo)
        {
            _tabControl.TabPages.Add(page);
            _pageBoTable.Add(page, bo);
            _boPageTable.Add(bo, page);
//			try
//			{
//				BeginInvoke(_addTabPageDelegate, new object[] {page, bo}); // needed to do the call on the Forms thread.  See info about STA thread model.
//			}
//			catch (InvalidOperationException)
//			{
//				this.AddTabPageInSTAThread(page, bo);
//			}
        }


//		private void AddTabPageInSTAThread(TabPage page, BusinessObjectBase bo)
//		{
//			_tabControl.TabPages.Add(page);
//			_pageBoTable.Add(page, bo);
//			_boPageTable.Add(bo, page);
//		}

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public TabPage GetTabPage(BusinessObject bo)
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
        private void ClearTabPages()
        {
            _tabControl.Controls.Clear();
            //_tabControl.TabPages.Clear() ;
            _pageBoTable.Clear();
            _boPageTable.Clear();
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public BusinessObject CurrentBusinessObject
        {
            get { return GetBo(_tabControl.SelectedTab); }
            set { _tabControl.SelectedTab = GetTabPage(value); }
        }
    }
}