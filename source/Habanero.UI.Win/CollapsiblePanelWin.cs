using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Implements <see cref="ICollapsiblePanel"/> for Windows Forms.
    /// </summary>
    public class CollapsiblePanelWin : PanelWin, ICollapsiblePanel
    {
        /// <summary>
        /// Event that is raised when this panel is uncollapsed.
        /// </summary>
        public event EventHandler Uncollapsed;

        private readonly CollapsiblePanelManager _collapsiblePanelManager;

        ///<summary>
        /// Creates an <see cref="CollapsiblePanelWin"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        public CollapsiblePanelWin(IControlFactory controlFactory)
        {
            _collapsiblePanelManager = new CollapsiblePanelManager(this, controlFactory);
        }

        /// <summary>
        /// Returns the Height required by the Panel when it is Expanded.
        /// </summary>
        public int ExpandedHeight
        {
            get { return _collapsiblePanelManager.ExpandedHeight; }
        }

        /// <summary>
        /// Returns the button that is available at the top of the <see cref="ICollapsiblePanel"/> that when
        /// clicked collapses or Expands the <see cref="IPanel"/>.
        /// </summary>
        public IButton CollapseButton
        {
            get { return _collapsiblePanelManager.CollapseButton; }
        }

        /// <summary>
        /// Gets and Sets the <see cref="IControlHabanero"/> that is placed on the Panel.
        /// </summary>
        public IControlHabanero ContentControl
        {
            get { return _collapsiblePanelManager.ContentControl; }
            set
            {
                _collapsiblePanelManager.ContentControl = value;
            }
        }

        /// <summary>
        /// Returns the PinLabel <see cref="ILabel"/> so that the Image can be changed on it for styling.
        /// </summary>
        public ILabel PinLabel
        {
            get { return _collapsiblePanelManager.PinLabel; }
        }

        ///<summary>
        /// Gets and Sets whether the <see cref="IPanel"/> is collapsed or expanded.
        ///</summary>
        public bool Collapsed
        {
            get { return _collapsiblePanelManager.Collapsed; }
            set
            {
                _collapsiblePanelManager.Collapsed = value;
            }
        }

        /// <summary>
        /// Gets and Sets whether the Panel is Pinned or not.
        /// </summary>
        public bool Pinned
        {
            get { return _collapsiblePanelManager.Pinned; }
            set
            {
                _collapsiblePanelManager.Pinned = value;
            }
        }

        ///<summary>
        /// Fires the Uncollapsed event this is used by the <see cref="CollapsiblePanelManager"/>
        ///   and is not expected to be used outside of this context.
        ///</summary>
        public void FireUncollapsedEvent()
        {
            if (Uncollapsed != null)
            {
                Uncollapsed(this, new EventArgs());
            }
        }

    }
}