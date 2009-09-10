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
using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// An interface for a Group on <see cref="ICollapsiblePanel"/>s. 
    ///   The collapsible Panels are placed one above the other and when the one opens the panels below
    ///   are moved down.
    /// The Interface works simply, you add the controls (AddControl) that will be displayed on 
    ///     an <see cref="ICollapsiblePanel"/> on this control in the order that you want them displayed.
    /// This control will then create the <see cref="ICollapsiblePanel"/> with the appropriate heading text and
    ///     the appropriate Expanded (<see cref="ICollapsiblePanel.ExpandedHeight"/> and Collapsed height.
    /// </summary>
    public class CollapsiblePanelGroupControlVWG : PanelVWG, ICollapsiblePanelGroupControl
    {
        private readonly CollapsiblePanelGroupManager _collapsiblePanelGroupManager;

        /// <summary>
        /// Returns the <see cref="ICollapsiblePanelGroupControl.ColumnLayoutManager"/> that is used for Laying out the <see cref="ICollapsiblePanel"/>s
        ///   on this control.
        /// </summary>
        public ColumnLayoutManager ColumnLayoutManager { get { return _collapsiblePanelGroupManager.ColumnLayoutManager; } }

        /// <summary>
        /// Constructs the <see cref="CollapsiblePanelGroupControlVWG"/>
        /// </summary>
        public CollapsiblePanelGroupControlVWG()
        {
            _collapsiblePanelGroupManager = new CollapsiblePanelGroupManager(this, GlobalUIRegistry.ControlFactory);
        }

        /// <summary>
        /// A List of all <see cref="ICollapsiblePanel"/>s that are being managed and displayed by this Control.
        /// </summary>
        public List<ICollapsiblePanel> PanelsList
        {
            get { return _collapsiblePanelGroupManager.PanelsList; }
        }

        /// <summary>
        /// The <see cref="IControlFactory"/> being used to create the <see cref="ICollapsiblePanel"/>s
        /// </summary>
        public IControlFactory ControlFactory
        {
            get { return _collapsiblePanelGroupManager.ControlFactory; }
        }

        /// <summary>
        /// Returns the Total Expanded Height of this Control. I.e. the total height of this control required
        /// if all the <see cref="ICollapsiblePanel"/> controls are fully expanded.
        /// </summary>
        public int TotalExpandedHeight
        {
            get
            {
               return _collapsiblePanelGroupManager.TotalExpandedHeight;
            }
        }

        /// <summary>
        /// Adds an <see cref="IControlHabanero"/> to this control. The <paramref name="contentControl"/> is
        ///    wrapped in an <see cref="ICollapsiblePanel"/> control.
        /// </summary>
        /// <param name="contentControl"></param>
        /// <param name="headingText"></param>
        /// <param name="minimumControlHeight">The minimum height that the <paramref name="contentControl"/> can be.
        ///   This height along with the <see cref="ICollapsiblePanel.CollapseButton"/>.Height are give the 
        ///   <see cref="ICollapsiblePanel.ExpandedHeight"/> that the <see cref="ICollapsiblePanel"/> will be when it is 
        ///   expanded </param>
        /// <returns></returns>
        public ICollapsiblePanel AddControl(IControlHabanero contentControl, string headingText, int minimumControlHeight)
        {
            ICollapsiblePanel control = _collapsiblePanelGroupManager.AddControl(contentControl, headingText, minimumControlHeight);
            control.Uncollapsed += ((sender, e) => FireItemSelected(control));
            return control;
        }

        /// <summary>
        /// Adds an <see cref="ICollapsiblePanel"/> to this control. The <paramref name="collapsiblePanel"/> is
        ///   added to this <see cref="ICollapsiblePanelGroupControl"/>
        /// </summary>
        /// <param name="collapsiblePanel"></param>
        /// <returns>The collapsible Panel</returns>
        public ICollapsiblePanel AddControl(ICollapsiblePanel collapsiblePanel)
        {
            ICollapsiblePanel control = _collapsiblePanelGroupManager.AddControl(collapsiblePanel);
            control.Uncollapsed += ((sender, e) => FireItemSelected(control));
            return control;
        }


        /// <summary>
        /// Event handler for the Uncollapsed Event.
        /// </summary>
        public event EventHandler ItemSelected;

        private void FireItemSelected(ICollapsiblePanel collapsiblePanel)
        {
            if (ItemSelected != null)
            {
                ItemSelected(collapsiblePanel, new EventArgs());
            }
        }
        /// <summary>
        /// Sets whether all the <see cref="ICollapsiblePanel"/> controls are collapsed or expanded AllCollapsed = true will 
        ///   <see cref="ICollapsiblePanel.Collapsed"/> = true for all the <see cref="ICollapsiblePanel"/>s.
        /// </summary>
        public bool AllCollapsed
        {
            set
            {
                _collapsiblePanelGroupManager.AllCollapsed = value;
            }
        }

//        /// <summary>
//        /// Gets the collection of tab pages in this tab control
//        /// </summary>
//        IControlCollection IGroupControl.ChildControls
//        {
//            get { return new ControlCollectionVWG(this.Controls); }
//        }

//        /// <summary>
//        /// Gets or sets the index of the currently selected ChildControl
//        /// </summary>
//        int IGroupControl.SelectedIndex
//        {
//            get { throw new System.NotImplementedException(); }
//            set { throw new System.NotImplementedException(); }
//        }
//
//        /// <summary>
//        /// Gets or sets the currently selected ChildControl
//        /// </summary>
//        IControlHabanero IGroupControl.SelectedChildControl
//        {
//            get { throw new System.NotImplementedException(); }
//            set { throw new System.NotImplementedException(); }
//        }

//        /// <summary>
//        /// Occurs when the SelectedIndex property is changed
//        /// </summary>
//        public event EventHandler SelectedIndexChanged;

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
        /// <param name="minimumControlWidth">The minimum width that the <paramref name="contentControl"/> can be</param>
        /// <returns></returns>
        IControlHabanero IGroupControl.AddControl(IControlHabanero contentControl, string headingText, int minimumControlHeight, int minimumControlWidth)
        {
            return AddControl(contentControl, headingText, minimumControlHeight);
        }
    }
}