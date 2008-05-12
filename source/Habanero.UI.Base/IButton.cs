using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI
{
    public interface IButton:IChilliControl
    {
        void PerformClick();

        event EventHandler Click;

        bool Enabled { get; set; }
    }
}
