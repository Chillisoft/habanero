using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class TabControlWin : TabControl, ITabControl
    {
        IControlCollection IControlChilli.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        //TODO: Convert dockstyles between Giz windows etc

        public new ITabPageCollection TabPages
        {
            get { return new TabPageCollectionWin(base.TabPages); }
        }

        public ITabPage SelectedTab
        {
            get { return (ITabPage)TabPages[base.SelectedIndex]; }
        }
    }

    internal class TabPageCollectionWin : ITabPageCollection
    {
        private readonly TabControl.TabPageCollection _tabPages;

        public TabPageCollectionWin(TabControl.TabPageCollection tabPages)
        {
            _tabPages = tabPages;
        }

        public void Add(ITabPage page)
        {
            _tabPages.Add((TabPage)page);
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