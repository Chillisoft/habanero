using Habanero.UI.Base;

namespace Habanero.UI.Base
{
    public interface IPanelFactory
    {
        /// <summary>
        /// Creates a panel to display a business object
        /// </summary>
        /// <returns>Returns the object containing the panel</returns>
        IPanelFactoryInfo CreatePanel();
    }
}