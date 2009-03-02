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
    /// The Interface works simply, you add the controls (<see cref="AddControl"/>) that will be displayed on 
    ///     an <see cref="ICollapsiblePanel"/> on this control in the order that you want them displayed.
    /// This control will then create the <see cref="ICollapsiblePanel"/> with the appropriate heading text and
    ///     the appropriate Expanded (<see cref="ICollapsiblePanel.ExpandedHeight"/> and Collapsed height.
    /// </summary>
    public class CollapsiblePanelGroupControlWin : PanelWin, ICollapsiblePanelGroupControl
    {
        private readonly CollapsiblePanelGroupManager _collapsiblePanelGroupManager;

        public event EventHandler ItemSelected;

        public List<ICollapsiblePanel> PanelsList
        {
            get { return _collapsiblePanelGroupManager.PanelsList; }
            //            set { _collapsiblePanelGroupManager.PanelsList = value; }
        }

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

        public IControlFactory ControlFactory
        {
            get { return _collapsiblePanelGroupManager.ControlFactory; }
        }

        public int TotalExpandedHeight
        {
            get { return _collapsiblePanelGroupManager.TotalExpandedHeight; }
        }

        public ICollapsiblePanel AddControl
            (IControlHabanero contentControl, string headingText, int minimumControlHeight)
        {
            ICollapsiblePanel control = _collapsiblePanelGroupManager.AddControl
                (contentControl, headingText, minimumControlHeight);
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
        /// Occurs when the SelectedIndex property is changed
        /// </summary>
        public event EventHandler SelectedIndexChanged;

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
        /// <returns></returns>
        IControlHabanero IGroupControl.AddControl
            (IControlHabanero contentControl, string headingText, int minimumControlHeight, int minimumControlWidth)
        {
            return AddControl(contentControl, headingText, minimumControlHeight);
        }

        #endregion
    }
}