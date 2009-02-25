using System;

namespace Habanero.UI.Base
{
    /// <summary>
    /// Provides an interface for a <see cref="IPanel"/> that has Collapsible functionality.
    /// This is essentially a <see cref="IPanel"/> that has a button at the Top.
    /// This button toggles whether the panel is collapsed or expanded.
    /// The <see cref="IPanel"/> can also be pinned.
    /// </summary>
    public interface ICollapsiblePanel : IPanel
    {
        event EventHandler Uncollapsed;
        ///<summary>
        /// Gets and Sets whether the <see cref="IPanel"/> is collapsed or expanded.
        ///</summary>
        bool Collapsed { get; set; }
        /// <summary>
        /// Returns the button that is available at the top of the <see cref="ICollapsiblePanel"/> that when
        /// clicked collapses or Expands the <see cref="IPanel"/>.
        /// </summary>
        IButton CollapseButton { get; }
        /// <summary>
        /// Gets and Sets the <see cref="IControlHabanero"/> that is placed on the Panel.
        /// </summary>
        IControlHabanero ContentControl { get; set; }
        /// <summary>
        /// Gets and Sets whether the Panel is Pinned or not.
        /// </summary>
        bool Pinned { get; set; }
        /// <summary>
        /// Returns the PinLabel <see cref="ILabel"/> so that the Image can be changed on it for styling.
        /// </summary>
        ILabel PinLabel { get; }
        /// <summary>
        /// Returns the Height required by the Panel when it is Expanded.
        /// </summary>
        int ExpandedHeight { get; }

        ///<summary>
        /// Fires the Uncollapsed event this is used by the <see cref="CollapsiblePanelManager"/>
        ///   and is not expected to be used outside of this context.
        ///</summary>
        void FireUncollapsedEvent();
    }
}