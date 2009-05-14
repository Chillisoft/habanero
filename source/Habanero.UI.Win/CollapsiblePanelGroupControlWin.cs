using System;
using System.Collections.Generic;
using Habanero.Base;
using Habanero.UI.Base;

namespace Habanero.UI.Win
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
    public class CollapsiblePanelGroupControlWin : PanelWin, ICollapsiblePanelGroupControl
    {
        private readonly CollapsiblePanelGroupManager _collapsiblePanelGroupManager;

        /// <summary>
        /// Event handler for the Uncollapsed Event on any of the Collapsible Panels.
        /// </summary>
        public event EventHandler ItemSelected;

        /// <summary>
        /// A List of all <see cref="ICollapsiblePanel"/>s that are being managed and displayed by this Control.
        /// This must be treated as a ReadOnly List i.e. Never use PanelList.Add or PanelList.Remove.
        /// Since this will cause the Panel List to be out of sync with the ControlsCollection.
        /// </summary>
        public List<ICollapsiblePanel> PanelsList
        {
            get { return _collapsiblePanelGroupManager.PanelsList; }
        }

        /// <summary>
        /// Returns the <see cref="ICollapsiblePanelGroupControl.ColumnLayoutManager"/> that is used for Laying out the <see cref="ICollapsiblePanel"/>s
        ///   on this control.
        /// </summary>
        public ColumnLayoutManager ColumnLayoutManager
        {
            get { return _collapsiblePanelGroupManager.ColumnLayoutManager; }
        }

        /// <summary>
        /// Constructs the <see cref="CollapsiblePanelGroupControlWin"/>
        /// </summary>
        public CollapsiblePanelGroupControlWin()
        {
            _collapsiblePanelGroupManager = new CollapsiblePanelGroupManager(this, GlobalUIRegistry.ControlFactory);
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
            get { return _collapsiblePanelGroupManager.TotalExpandedHeight; }
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
        public ICollapsiblePanel AddControl
            (IControlHabanero contentControl, string headingText, int minimumControlHeight)
        {
            ICollapsiblePanel control = _collapsiblePanelGroupManager.AddControl
                (contentControl, headingText, minimumControlHeight);
            control.Uncollapsed += ((sender, e) => FireItemSelected(control));
            control.Uncollapsed += (sender, e) => FireItemSelectedIndexChanged(control);
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


        private void FireItemSelected(ICollapsiblePanel collapsiblePanel)
        {
            if (ItemSelected != null)
            {
                ItemSelected(collapsiblePanel, new EventArgs());
            }
        }

        /// <summary>
        /// Occurs when the SelectedIndex property is changed
        /// </summary>
        public event EventHandler SelectedIndexChanged;
        private void FireItemSelectedIndexChanged(ICollapsiblePanel collapsiblePanel)
        {
            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(collapsiblePanel, new EventArgs());
            }
        }
        /// <summary>
        /// Sets whether all the <see cref="ICollapsiblePanel"/> controls are collapsed or expanded AllCollapsed = true will 
        ///   <see cref="ICollapsiblePanel.Collapsed"/> = true for all the <see cref="ICollapsiblePanel"/>s.
        /// </summary>
        public bool AllCollapsed
        {
            set { _collapsiblePanelGroupManager.AllCollapsed = value; }
        }

        #region Implementation of IGroupControl

//        /// <summary>
//        /// Gets the collection of tab pages in this tab control
//        /// </summary>
//        IControlCollection IGroupControl.ChildControls
//        {
//            get { return ; }
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
//        /// Gets or sets the currently selected tab page
//        /// </summary>
//        IControlHabanero IGroupControl.SelectedChildControl
//        {
//            get { throw new System.NotImplementedException(); }
//            set { throw new System.NotImplementedException(); }
//        }

        /// <summary>
        /// Adds an <see cref="IControlHabanero"/> to this control. The <paramref name="contentControl"/> is
        ///    wrapped in the appropriate Child Control Type.
        /// </summary>
        /// <param name="contentControl">The control that is being placed as a child within this control. The content control could be 
        ///  a Panel of <see cref="IBusinessObject"/>.<see cref="IBOProp"/>s or any other child control</param>
        /// <param name="headingText">The heading text that will be shown as the Header for this Group e.g. For a <see cref="ITabControl"/>
        ///   this will be the Text shown in the Tab for a <see cref="ICollapsiblePanelGroupControl"/> this will be the text shown
        ///   on the Collapse Panel and for an <see cref="IGroupBox"/> this will be the title of the Group Box.</param>
        /// <param name="minimumControlHeight">The minimum height that the <paramref name="contentControl"/> can be. This height along with any other spacing required will be used as the minimum height for the ChildControlCreated</param>
        /// <param name="minimumControlWidth">The minimum width that the control can be.</param>
        /// <returns></returns>
        IControlHabanero IGroupControl.AddControl
            (IControlHabanero contentControl, string headingText, int minimumControlHeight, int minimumControlWidth)
        {
            return AddControl(contentControl, headingText, minimumControlHeight);
        }

        #endregion
    }
}