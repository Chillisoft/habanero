
namespace Habanero.UI
{
    public interface IComboBox:IChilliControl
    {
        IComboBoxObjectCollection Items { get; }
        int SelectedIndex { get; set; }

        object SelectedItem { get; }

        int Height { get; set; }
    }
}