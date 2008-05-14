using System;

namespace Habanero.UI.Base
{
    public interface IDateTimePicker : IControlChilli
    {
        DateTime Value { get; set; }

        string CustomFormat { get; }

        event EventHandler Enter;
    }
}