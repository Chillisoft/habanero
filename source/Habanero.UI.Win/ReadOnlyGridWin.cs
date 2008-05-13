using System.Windows.Forms;
using Habanero.UI.Base;

namespace Habanero.UI.Win
{
    public class ReadOnlyGridWin : GridBaseWin, IReadOnlyGrid
    {
        public ReadOnlyGridWin()
        {
            this.ReadOnly = true;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
    }
}