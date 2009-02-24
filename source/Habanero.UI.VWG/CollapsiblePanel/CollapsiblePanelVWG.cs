using System;
using Habanero.UI.Base;

namespace Habanero.UI.VWG
{
    /// <summary>
    /// Implements <see cref="ICollapsiblePanel"/> for Visual Web Gui.
    /// </summary>
    public class CollapsiblePanelVWG : PanelVWG, ICollapsiblePanel
    {
        public event EventHandler Uncollapsed;

        private readonly CollapsiblePanelManager _collapsiblePanelManager;

        ///<summary>
        /// Creates an <see cref="CollapsiblePanelVWG"/>
        ///</summary>
        ///<param name="controlFactory"></param>
        public CollapsiblePanelVWG(IControlFactory controlFactory)
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