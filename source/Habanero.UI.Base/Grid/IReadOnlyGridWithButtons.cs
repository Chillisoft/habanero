using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IReadOnlyGridWithButtons : IChilliControl
    {
        
        IReadOnlyGrid Grid { get; }

        BusinessObject SelectedBusinessObject { get; set; }

        void SetCollection(IBusinessObjectCollection col);
    }
}