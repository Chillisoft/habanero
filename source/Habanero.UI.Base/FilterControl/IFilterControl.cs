using System.Collections;
using Habanero.Base;

namespace Habanero.UI.Base
{
    public interface IFilterControl : IChilliControl
    {
        ITextBox AddStringFilterTextBox(string labelText, string propertyName);
        IFilterClause GetFilterClause();

        ICollection Controls { get; }
        IComboBox AddStringFilterComboBox(string labelText, string columnName, ICollection options, bool strictMatch);
    }
}