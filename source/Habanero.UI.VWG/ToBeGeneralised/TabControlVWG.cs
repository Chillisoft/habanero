// ---------------------------------------------------------------------------------
//  Copyright (C) 2009 Chillisoft Solutions
//  
//  This file is part of the Habanero framework.
//  
//      Habanero is a free framework: you can redistribute it and/or modify
//      it under the terms of the GNU Lesser General Public License as published by
//      the Free Software Foundation, either version 3 of the License, or
//      (at your option) any later version.
//  
//      The Habanero framework is distributed in the hope that it will be useful,
//      but WITHOUT ANY WARRANTY; without even the implied warranty of
//      MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//      GNU Lesser General Public License for more details.
//  
//      You should have received a copy of the GNU Lesser General Public License
//      along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
// ---------------------------------------------------------------------------------
using System.Collections;
using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Manages a related set of tab pages
    /// </summary>
    public class TabControlVWG : TabControl, ITabControl
    {
        /// <summary>
        /// Gets or sets the anchoring style.
        /// </summary>
        /// <value></value>
        Base.AnchorStyles IControlHabanero.Anchor
        {
            get { return (Base.AnchorStyles)base.Anchor; }
            set { base.Anchor = (Gizmox.WebGUI.Forms.AnchorStyles)value; }
        }

        /// <summary>
        /// Gets the collection of controls contained within the control
        /// </summary>
        IControlCollection IControlHabanero.Controls
        {
            get { return new ControlCollectionVWG(base.Controls); }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent
        /// control and determines how a control is resized with its parent
        /// </summary>
        Base.DockStyle IControlHabanero.Dock
        {
            get { return DockStyleVWG.GetDockStyle(base.Dock); }
            set { base.Dock = DockStyleVWG.GetDockStyle(value); }
        }

        /// <summary>
        /// Gets the collection of tab pages in this tab control
        /// </summary>
        public new ITabPageCollection TabPages
        {
            get { return new TabPageCollectionGiz(base.TabPages); }
        }

        /// <summary>
        /// Gets or sets the currently selected tab page
        /// </summary>
        public ITabPage SelectedTab
        {
            get
            {
                if ((this.SelectedIndex >= TabPages.Count) || (this.SelectedIndex == -1))
                {
                    return null;
                }
                return TabPages[this.SelectedIndex];
            }
            set { this.SelectedIndex = TabPages.IndexOf(value); }
        }

        /// <summary>
        /// Gets or sets the index of the currently selected tab page
        /// </summary>
        public new int SelectedIndex
        {
            get
            {
                if (this.TabPages.Count == 0)
                {
                    return -1;
                }
                return base.SelectedIndex;
            }
            set { base.SelectedIndex = value; }
        }

        /// <summary>
        /// Adds an <see cref="IControlHabanero"/> to this control. The <paramref name="contentControl"/> is
        ///    wrapped in the appropriate Child Control Type.
        /// </summary>
        /// <param name="contentControl">The control that is being placed as a child within this control. The content control could be 
        ///  a Panel of <see cref="IBusinessObject"/>.<see cref="IBOProp"/>s or any other child control</param>
        /// <param name="headingText">The heading text that will be shown as the Header for this Group e.g. For a <see cref="ITabControl"/>
        ///   this will be the Text shown in the Tab for a <see cref="ICollapsiblePanelGroupControl"/> this will be the text shown
        ///   on the Collapse Panel and for an <see cref="IGroupBox"/> this will be the title of the Group Box.</param>
        /// <param name="minimumControlHeight">The minimum height that the <paramref name="contentControl"/> can be.
        ///   This height along with any other spacing required will be used as the minimum height for the ChildControlCreated</param>
        /// <param name="minimumControlWidth">The min width that the content control can be.</param>
        /// <returns></returns>
        public IControlHabanero AddControl(IControlHabanero contentControl, string headingText, int minimumControlHeight, int minimumControlWidth)
        {
            IControlFactory factory = GlobalUIRegistry.ControlFactory;
            if (factory == null)
            {
                const string errMessage = "There is a serious error since the GlobalUIRegistry.ControlFactory  has not been set up.";
                throw new HabaneroDeveloperException(errMessage, errMessage);
            }
            ITabPage childControl = factory.CreateTabPage(headingText);
            childControl.Width = minimumControlHeight;
            childControl.Height = minimumControlHeight;
            contentControl.Dock = Base.DockStyle.Fill;
            childControl.Controls.Add(contentControl);
            this.TabPages.Add(childControl);
            return childControl;
        }
    }

    /// <summary>
    /// Contains the collection of controls that the TabPage uses
    /// </summary>
    internal class TabPageCollectionGiz : ITabPageCollection
    {
        private readonly TabPageCollection _tabPages;

        public TabPageCollectionGiz(TabPageCollection tabPages)
        {
            _tabPages = tabPages;
        }

        /// <summary>
        /// Adds a tab page to the collection
        /// </summary>
        public void Add(ITabPage page)
        {
            _tabPages.Add((TabPage) page);
        }

        /// <summary>
        /// Indicates the tab page at the specified indexed location in the collection
        /// </summary>
        public ITabPage this[int index]
        {
            get { return (ITabPage)_tabPages[index]; }
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

        /// <summary>
        /// Clears all the tab pages.
        /// </summary>
        public void Clear()
        {
            _tabPages.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return _tabPages.GetEnumerator();
        }
    }
}