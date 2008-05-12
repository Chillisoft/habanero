
namespace Habanero.UI.Base
{
    public interface IComboBox:IChilliControl
    {
        IComboBoxObjectCollection Items { get; }
        int SelectedIndex { get; set; }

        object SelectedItem { get; }

        int Height { get; set; }
    }
}