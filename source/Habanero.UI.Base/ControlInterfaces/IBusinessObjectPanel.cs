namespace Habanero.UI.Base
{
    /// <summary>
    /// Represents a panel containing a PanelInfo used to edit a single business object.
    /// </summary>
    public interface IBusinessObjectPanel : IBusinessObjectControl
    {
        IPanelInfo PanelInfo { get; set; }
    }
}