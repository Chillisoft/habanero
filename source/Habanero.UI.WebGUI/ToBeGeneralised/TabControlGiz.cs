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
        Base.DockStyle IControlChilli.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (Gizmox.WebGUI.Forms.DockStyle)value; }
        }

        //TODO: Convert dockstyles between Giz windows etc

        public new ITabPageCollection TabPages
        {
            get { return new TabPageCollectionGiz(base.TabPages); }
        }

        public ITabPage SelectedTab
        {
            get { return (ITabPage)TabPages[base.SelectedIndex]; }
            set { base.SelectedIndex = TabPages.IndexOf(value); }
        }
    }

    internal class TabPageCollectionGiz : ITabPageCollection
    {
        private readonly TabPageCollection _tabPages;

        public TabPageCollectionGiz(TabPageCollection tabPages)
        {
            _tabPages = tabPages;
        }

        public void Add(ITabPage page)
        {
            _tabPages.Add((TabPage) page);
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