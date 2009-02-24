using System;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    /// <summary>
    /// Implements <see cref="ICollapsiblePanel"/> for Windows Forms.
    /// </summary>
    public class CollapsiblePanelWin : PanelWin, ICollapsiblePanel
    {
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

        public int ExpandedHeight
        {
            get { return _collapsiblePanelManager.ExpandedHeight; }
        }

        public IButton CollapseButton
        {
            get { return _collapsiblePanelManager.CollapseButton; }
        }

        public IControlHabanero ContentControl
        {
            get { return _collapsiblePanelManager.ContentControl; }
            set
            {
                _collapsiblePanelManager.ContentControl = value;
            }
        }

        public ILabel PinLabel
        {
            get { return _collapsiblePanelManager.PinLabel; }
        }

        public bool Collapsed
        {
            get { return _collapsiblePanelManager.Collapsed; }
            set
            {
                _collapsiblePanelManager.Collapsed = value;
            }
        }

        public bool Pinned
        {
            get { return _collapsiblePanelManager.Pinned; }
            set
            {
                _collapsiblePanelManager.Pinned = value;
            }
        }

        public void FireUncollapsedEvent()
        {
            if (Uncollapsed != null)
            {
                Uncollapsed(this, new EventArgs());
            }
        }

    }
}