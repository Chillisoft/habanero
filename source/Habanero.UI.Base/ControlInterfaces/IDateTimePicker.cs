using System;

namespace Habanero.UI.Base
{
    public interface IDateTimePicker : IControlChilli
    {
        DateTime Value { get; set; }

        string CustomFormat { get; set; }

        bool ShowUpDown { get; set; }

        event EventHandler Enter;
    }
}