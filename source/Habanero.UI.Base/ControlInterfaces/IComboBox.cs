
namespace Habanero.UI.Base
{
    public interface IComboBox:IControlChilli
    {
        IComboBoxObjectCollection Items { get; }
        int SelectedIndex { get; set; }

        object SelectedItem { get; set; }

        int Height { get; set; }

        int DropDownWidth { get; set; }
    }
}