using System.Collections.Generic;

namespace Habanero.UI.Base
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
    public interface ICollapsiblePanelGroupControl : IPanel
    {
        /// <summary>
        /// A List of all <see cref="ICollapsiblePanel"/>s that are being managed and displayed by this Control.
        /// </summary>
        List<ICollapsiblePanel> PanelsList { get; }
        /// <summary>
        /// The <see cref="IControlFactory"/> being used to create the <see cref="ICollapsiblePanel"/>s
        /// </summary>
        IControlFactory ControlFactory { get; }
        /// <summary>
        /// Returns the <see cref="ColumnLayoutManager"/> that is used for Laying out the <see cref="ICollapsiblePanel"/>s
        ///   on this control.
        /// </summary>
        ColumnLayoutManager ColumnLayoutManager { get; }
        /// <summary>
        /// Returns the Total Expanded Height of this Control. I.e. the total height of this control required
        /// if all the <see cref="ICollapsiblePanel"/> controls are fully expanded.
        /// </summary>
        int TotalExpandedHeight { get; }
        /// <summary>
        /// Sets whether all the <see cref="ICollapsiblePanel"/> controls are collapsed or expanded AllCollapsed = true will 
        ///   <see cref="ICollapsiblePanel.Collapsed"/> = true for all the <see cref="ICollapsiblePanel"/>s.
        /// </summary>
        bool AllCollapsed { set; }
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
        ICollapsiblePanel AddControl(IControlHabanero contentControl, string headingText, int minimumControlHeight);
    }
}