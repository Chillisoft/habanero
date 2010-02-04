using System;
using System.Collections.Generic;
using System.Text;
using Habanero.Base;

namespace Habanero.UI.Base
{
    public interface IBOColMultiselectorControl : IBOColSelectorControl
    {
        IBusinessObjectCollection SelectedBusinessObjectCollection { get; set; }
    }
}
