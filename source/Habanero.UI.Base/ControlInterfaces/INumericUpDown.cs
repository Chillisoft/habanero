using System;

namespace Habanero.UI.Base
{
    public interface INumericUpDown : IControlChilli
    {
        event EventHandler Enter;

        int DecimalPlaces { get; set; }

        decimal Minimum { get; set; }

        decimal Maximum { get; set; }

        decimal Value { get; set; }

        void Select(int i, int length);
    }
}