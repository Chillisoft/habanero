using System;

namespace Habanero.UI.Base
{
    public interface INumericUpDown : IControlChilli
    {
        event EventHandler Enter;

        int DecimalPlaces { get; }

        decimal Minimum { get; }

        decimal Maximum { get; }

        void Select(int i, object length);
    }
}