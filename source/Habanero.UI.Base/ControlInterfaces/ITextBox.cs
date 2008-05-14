using System;

namespace Habanero.UI.Base
{
    public interface ITextBox : IControlChilli
    {
        bool Multiline { get; set; }

        bool AcceptsReturn { get; set; }

        char PasswordChar { get; set; }

        //TODO_Port:ScrollBars ScrollBars { get; set; }
        event EventHandler DoubleClick;
    }
}