using System.Collections;
using System.Collections.Generic;
using Habanero.Base;

namespace Habanero.UI.Base
{
    public interface IFilterControl : IChilliControl
    {
        ITextBox AddStringFilterTextBox(string labelText, string propertyName);
        IFilterClause GetFilterClause();

        ICollection Controls { get; }
    }
}