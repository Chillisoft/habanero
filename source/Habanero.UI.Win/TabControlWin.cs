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
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (System.Windows.Forms.DockStyle)value; }
        }

        //TODO: Convert dockstyles between Giz windows etc

        public new ITabPageCollection TabPages
        {
            get { return new TabPageCollectionWin(base.TabPages); }
        }

        public ITabPage SelectedTab
        {
            get { return (ITabPage)TabPages[base.SelectedIndex]; }
            set { base.SelectedIndex = TabPages.IndexOf(value); }
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

        public int IndexOf(ITabPage page)
        {
            for (int pagePos = 0; pagePos < _tabPages.Count; pagePos++)
            {
                if (page == _tabPages[pagePos])
                {
                    return pagePos;
                }
            }
            return -1;
        }
    }
}