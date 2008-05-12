using System;

namespace Habanero.UI
{
    public interface IDateTimePicker : IChilliControl
    {
        DateTime Value { get; set; }
    }
}