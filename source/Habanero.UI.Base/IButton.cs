using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    public interface IButton
    {
        void PerformClick();

        event EventHandler Click;

        bool Enabled { get; set; }
    }
}
