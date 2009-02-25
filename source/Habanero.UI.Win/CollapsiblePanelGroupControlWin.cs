using System.Collections.Generic;
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
        public List<ICollapsiblePanel> PanelsList
        {
            get { return _collapsiblePanelGroupManager.PanelsList; }
        }

        public ColumnLayoutManager ColumnLayoutManager { get { return _collapsiblePanelGroupManager.ColumnLayoutManager; } }

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
            get
            {
               return _collapsiblePanelGroupManager.TotalExpandedHeight;
            }
        }

        public ICollapsiblePanel AddControl(IControlHabanero contentControl, string headingText, int minimumControlHeight)
        {
            return _collapsiblePanelGroupManager.AddControl(contentControl, headingText, minimumControlHeight);
        }


        public bool AllCollapsed
        {
            set
            {
                _collapsiblePanelGroupManager.AllCollapsed = value;
            }
        }
    }
}