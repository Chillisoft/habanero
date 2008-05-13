using System;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{
    public interface IButton:IChilliControl
    {
        void PerformClick();

        event EventHandler Click;
    }
}
