using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Habanero.UI.Base
{
    public interface IListBox : IChilliControl
    {
        IListBoxObjectCollection Items { get; }
    }
}
