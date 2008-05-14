using System;

namespace Habanero.UI.Base
{
    public enum ListBoxSelectionMode
    {
        MultiExtended,
        MultiSimple,
        None,
        One
    }
    public interface IListBox : IControlChilli
    {
        IListBoxObjectCollection Items { get; }

        int SelectedIndex { get; set; }

        object SelectedItem { get; }

        IListBoxSelectedObjectCollection SelectedItems { get; }

        ListBoxSelectionMode SelectionMode { get; set; }

        event EventHandler SelectedIndexChanged;
    }
}
