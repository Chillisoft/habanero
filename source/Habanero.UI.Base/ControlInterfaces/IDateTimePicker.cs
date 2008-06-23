using System;

namespace Habanero.UI.Base
{
    public interface IDateTimePicker : IControlChilli
    {
        DateTime Value { get; set; }

        DateTime? ValueOrNull { get; set; }

        string CustomFormat { get; set; }

        bool ShowUpDown { get; set; }

        bool ShowCheckBox { get; set; }

        bool Checked { get; set; }

        event EventHandler Enter;
    }
}