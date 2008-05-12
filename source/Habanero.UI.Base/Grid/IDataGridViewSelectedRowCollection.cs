using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Habanero.UI.Base
{

    public interface IDataGridViewSelectedRowCollection : IEnumerable
    {
        int Count { get; }

        IDataGridViewRow this[int index] { get; }
    }
}
