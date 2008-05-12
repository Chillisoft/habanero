using System;

namespace Habanero.UI.Base
{
    public interface IDateTimePicker : IChilliControl
    {
        DateTime Value { get; set; }
    }
}