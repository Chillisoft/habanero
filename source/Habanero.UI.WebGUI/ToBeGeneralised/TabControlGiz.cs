using System;
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.WebGUI
{
    public class TabControlGiz : TabControl, ITabControl
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionGiz(base.Controls); }
        }
       
        //TODO: Convert dockstyles between Giz windows etc

        public new ITabPageCollection TabPages
        {
            get { return new TabPageCollectionGiz(base.TabPages); }
        }

        public ITabPage SelectedTab
        {
            get { return  (ITabPage) TabPages[base.SelectedIndex]; }
        }
    }

    internal class TabPageCollectionGiz : ITabPageCollection
    {
        private readonly TabPageCollection _tabPages;

        public TabPageCollectionGiz(TabPageCollection tabPages)
        {
            _tabPages = tabPages;
        }

        public int Add(ITabPage page)
        {
            return _tabPages.Add((TabPage) page);
        }

        public ITabPage this[int i]
        {
            get { return (ITabPage)_tabPages[i]; }
        }

        public int Count
        {
            get { return _tabPages.Count; }
        }
    }
}