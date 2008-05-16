using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Habanero.UI.Base
{
    public interface IButton:IControlChilli
    {
        void PerformClick();

        event EventHandler Click;
        void NotifyDefault(bool b);
    }
}

