using System;

namespace Habanero.UI.Base
{
    public interface ITabControl : IControlChilli
    {
        IDockStyle Dock { get; set; }

        ITabPageCollection TabPages { get; }

        int SelectedIndex { get; set; }

        ITabPage SelectedTab { get; }

        event EventHandler SelectedIndexChanged;
    }
}