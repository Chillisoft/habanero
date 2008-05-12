using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IDataGridViewRow
    {
        bool Selected { get; set; }

       object DataBoundItem { get; }
    }
}