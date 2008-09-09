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

using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Manages a related set of tab pages
    /// </summary>
    public class TabControlWin : TabControl, ITabControl
    {
        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionWin(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return (Base.DockStyle)base.Dock; }
            set { base.Dock = (System.Windows.Forms.DockStyle)value; }
        }

        /// <summary>
        /// Gets the collection of tab pages in this tab control
        /// </summary>
        ITabPageCollection ITabControl.TabPages
        {
            get { return new TabPageCollectionWin(TabPages); }
        }

        /// <summary>
        /// Gets or sets the currently selected tab page
        /// </summary>
        public new ITabPage SelectedTab
        {
            get { return new TabPageCollectionWin(TabPages)[SelectedIndex]; }
            set { SelectedIndex = new TabPageCollectionWin(TabPages).IndexOf(value); }
        }
    }

    /// <summary>
    /// Contains the collection of controls that the TabPage uses
    /// </summary>
    internal class TabPageCollectionWin : ITabPageCollection
    {
        private readonly TabControl.TabPageCollection _tabPages;

        public TabPageCollectionWin(TabControl.TabPageCollection tabPages)
        {
            _tabPages = tabPages;
        }

        /// <summary>
        /// Adds a tab page to the collection
        /// </summary>
        public void Add(ITabPage page)
        {
            _tabPages.Add((TabPage)page);
        }

        /// <summary>
        /// Indicates the tab page at the specified indexed location in the collection
        /// </summary>
        public ITabPage this[int i]
        {
            get
            {
                if(i>-1) return (ITabPage)_tabPages[i];
                return null;
            }
        }

        /// <summary>
        /// Indicates the number of tab pages in the collection
        /// </summary>
        public int Count
        {
            get { return _tabPages.Count; }
        }

        /// <summary>
        /// Retrieves the index of the specified tab page in the collection
        /// </summary>
        /// <returns>A zero-based index value that represents the position of the specified
        /// tab page in the collection</returns>
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