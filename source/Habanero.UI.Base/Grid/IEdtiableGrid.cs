using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO.ClassDefinition;

namespace Habanero.UI.Base
{
    public interface IEditableGrid
    {
        IDataGridViewColumnCollection Columns { get; }

        bool ReadOnly { get; }

        bool AllowUserToAddRows { get; }

        bool AllowUserToDeleteRows { get; }
    }
}
