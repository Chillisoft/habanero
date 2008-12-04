namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a panel containing a PanelInfo used to edit a single business object.
    /// </summary>
    public interface IBusinessObjectPanel : IBusinessObjectControl
    {
        /// <summary>
        /// Gets and sets the PanelInfo object created by the control
        /// </summary>
        IPanelInfo PanelInfo { get; set; }
    }
}