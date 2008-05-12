using Habanero.BO;

namespace Habanero.UI
{
    public interface IDataGridViewRow
    {
        bool Selected { get; set; }

       object DataBoundItem { get; }
    }
}