using System;

namespace Habanero.UI.Base
{
    public interface ITabControl : IControlChilli
    {
        ITabPageCollection TabPages { get; }

        int SelectedIndex { get; set; }

        ITabPage SelectedTab { get; set; }

        event EventHandler SelectedIndexChanged;
    }
}