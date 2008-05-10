using System;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IGridBase
    {
        void SetCollection(IBusinessObjectCollection col);

        IDataGridViewRowCollection Rows { get; }

        IDataGridViewColumnCollection Columns { get; }

        bool AllowUserToAddRows { get; set; }

        object DataSource { get; set; }

        BusinessObject SelectedBusinessObject { get; set; }

        event EventHandler<BOEventArgs> BusinessObjectSelected;
        event EventHandler SelectionChanged;

    }
}