namespace Habanero.UI.Base
{
    public interface IPanelFactoryInfo
    {
        /// <summary>
        /// Returns the panel object
        /// </summary>
        IPanel Panel { get; }

        /// <summary>
        /// Returns the collection of control mappers
        /// </summary>
        IControlMapperCollection ControlMappers { get; }

        /// <summary>
        /// Gets and sets the preferred height setting
        /// </summary>
        int PreferredHeight { get; set; }

        /// <summary>
        /// Gets and sets the preferred width setting
        /// </summary>
        int PreferredWidth { get; set; }

        /// <summary>
        /// Returns the first control to focus on in the user interface
        /// </summary>
        IControlChilli FirstControlToFocus { get; }

        IToolTip ToolTip { get; set; }

        int MinimumPanelHeight { get; set; }

        int MinumumPanelWidth { get; set; }
    }
}