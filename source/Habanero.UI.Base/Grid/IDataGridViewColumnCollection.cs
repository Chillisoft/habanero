using System.Collections.Generic;

namespace Habanero.UI.Base
{
    public interface IDataGridViewColumnCollection : IEnumerable<IDataGridViewColumn>
    {
        int Count { get; }
        void Clear();
        //void Add(IDataGridViewColumn dataGridViewColumn);
        int Add(string columnName, string headerText);
    }
}
