
namespace Habanero.UI.Base
{
    public interface IComboBox
    {
        IComboBoxObjectCollection Items { get; }
        int SelectedIndex { get; set; }

        object SelectedItem { get; }

        int Height { get; set; }
    }
}