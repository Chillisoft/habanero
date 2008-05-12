using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Habanero.UI
{
    public enum ListBoxSelectionMode
    {
        MultiExtended,
        MultiSimple,
        None,
        One
    }
    public interface IListBox : IChilliControl
    {
        IListBoxObjectCollection Items { get; }

        int SelectedIndex { get; set; }

        object SelectedItem { get; }

        IListBoxSelectedObjectCollection SelectedItems { get; }

        ListBoxSelectionMode SelectionMode { get; set; }

        event EventHandler SelectedIndexChanged;
    }
}
