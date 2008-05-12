using System;
using System.Collections;
using System.Collections.Generic;
using Habanero.BO;

namespace Habanero.UI.Base
{
    public interface IGridBase:IChilliControl
    {
        void SetCollection(IBusinessObjectCollection col);

        IDataGridViewRowCollection Rows { get; }

        IDataGridViewColumnCollection Columns { get; }

        bool AllowUserToAddRows { get; set; }

        object DataSource { get; set; }

        BusinessObject SelectedBusinessObject { get; set; }

        IList<BusinessObject> SelectedBusinessObjects { get; }

        IDataGridViewSelectedRowCollection SelectedRows { get; }

        event EventHandler<BOEventArgs> BusinessObjectSelected;
        event EventHandler SelectionChanged;

        void Clear();
    }
}