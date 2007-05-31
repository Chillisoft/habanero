using System;
using System.Collections;
using System.Windows.Forms;
using Chillisoft.Bo.v2;
using Chillisoft.UI.Generic.v2;

namespace Chillisoft.UI.Application.v2
{
    /// <summary>
    /// Manages a collection of tab pages that hold business object controls
    /// </summary>
    public class BoTabColControl : UserControl
    {
        //private delegate void AddTabPageDelegate(TabPage page, BusinessObjectBase bo);
        //private delegate void TabChangedDelegate();

        private readonly BusinessObjectControl itsBoControl;
        private TabControl itsTabControl;
        private Hashtable itsPageBoTable;
        private Hashtable itsBoPageTable;

        //private AddTabPageDelegate itsAddTabPageDelegate;
        //private TabChangedDelegate itsTabChanged; 

        private EventHandler tabChangedHandler;

        /// <summary>
        /// Constructor to initialise a new tab control
        /// </summary>
        /// <param name="boControl">The business object control that is
        /// displaying the business object information in the tab page</param>
        public BoTabColControl(BusinessObjectControl boControl)
        {
            itsBoControl = boControl;
            if (boControl is Control)
            {
                ((Control) itsBoControl).Dock = DockStyle.Fill;
            }
            else
            {
                throw new ArgumentException("boControl must be of type Control or one of its subtypes.");
            }
            BorderLayoutManager manager = new BorderLayoutManager(this);
            itsTabControl = new TabControl();
            manager.AddControl(itsTabControl, BorderLayoutManager.Position.Centre);
            itsPageBoTable = new Hashtable();
            itsBoPageTable = new Hashtable();

            tabChangedHandler = new EventHandler(TabChangedHandler);

            //itsAddTabPageDelegate = new AddTabPageDelegate(AddTabPageInSTAThread);
            //itsTabChanged = new TabChangedDelegate(TabChangedInSTAThread) ;
        }

        /// <summary>
        /// Sets the collection of tab pages for the collection of business
        /// objects provided
        /// </summary>
        /// <param name="col">The business object collection to create tab pages
        /// for</param>
        public void SetCollection(IList col)
        {
            itsTabControl.SelectedIndexChanged -= tabChangedHandler;
            ClearTabPages();
            foreach (BusinessObjectBase bo in col)
            {
                TabPage page = new TabPage(bo.ToString());
                //page.Text =  ;
                AddTabPage(page, bo);
            }


            if (col.Count > 0)
            {
                itsTabControl.SelectedIndex = 0;
            }
            itsTabControl.SelectedIndexChanged += tabChangedHandler;
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
            if (itsTabControl.SelectedTab != null)
            {
                //BeginInvoke(itsTabChanged, new object[] {});
                itsTabControl.SelectedTab.Controls.Clear();
                itsTabControl.SelectedTab.Controls.Add((Control) itsBoControl);
                itsBoControl.SetBusinessObject(GetBo(itsTabControl.SelectedTab));
            }
        }

        /// <summary>
        /// Carries out additional steps when the user selects a different tab
        /// </summary>
        /// TODO ERIC - sta thread?
        private void TabChangedInSTAThread()
        {
            itsTabControl.SelectedTab.Controls.Clear();
            itsTabControl.SelectedTab.Controls.Add((Control) itsBoControl);
            itsBoControl.SetBusinessObject(GetBo(itsTabControl.SelectedTab));
        }

        /// <summary>
        /// Returns the TabControl object
        /// </summary>
        public TabControl TabControl
        {
            get { return itsTabControl; }
        }

        /// <summary>
        /// Returns the business object represented in the specified tab page
        /// </summary>
        /// <param name="tabPage">The tab page</param>
        /// <returns>Returns the business object, or null if not available
        /// </returns>
        public BusinessObjectBase GetBo(TabPage tabPage)
        {
            if (tabPage == null) return null;

            if (itsPageBoTable.ContainsKey(tabPage))
            {
                return (BusinessObjectBase) itsPageBoTable[tabPage];
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
        private void AddTabPage(TabPage page, BusinessObjectBase bo)
        {
            itsTabControl.TabPages.Add(page);
            itsPageBoTable.Add(page, bo);
            itsBoPageTable.Add(bo, page);
//			try
//			{
//				BeginInvoke(itsAddTabPageDelegate, new object[] {page, bo}); // needed to do the call on the Forms thread.  See info about STA thread model.
//			}
//			catch (InvalidOperationException)
//			{
//				this.AddTabPageInSTAThread(page, bo);
//			}
        }


//		private void AddTabPageInSTAThread(TabPage page, BusinessObjectBase bo)
//		{
//			itsTabControl.TabPages.Add(page);
//			itsPageBoTable.Add(page, bo);
//			itsBoPageTable.Add(bo, page);
//		}

        /// <summary>
        /// Returns the TabPage object that is representing the given
        /// business object
        /// </summary>
        /// <param name="bo">The business object being represented</param>
        /// <returns>Returns the TabPage object, or null if not found</returns>
        public TabPage GetTabPage(BusinessObjectBase bo)
        {
            if (itsBoPageTable.ContainsKey(bo))
            {
                return (TabPage) itsBoPageTable[bo];
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
            itsTabControl.Controls.Clear();
            //itsTabControl.TabPages.Clear() ;
            itsPageBoTable.Clear();
            itsBoPageTable.Clear();
        }

        /// <summary>
        /// Returns the business object represented in the currently
        /// selected tab page
        /// </summary>
        public BusinessObjectBase CurrentBusinessObject
        {
            get { return GetBo(itsTabControl.SelectedTab); }
            set { itsTabControl.SelectedTab = GetTabPage(value); }
        }
    }
}