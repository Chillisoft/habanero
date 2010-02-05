using Habanero.Test.UI.Base.Grid;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Grid
{
    [TestFixture]
    public class TestDataGridViewWin : TestDataGridView
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override string GetUnderlyingDataGridViewSelectionModeToString(IDataGridView dataGridView)
        {
            System.Windows.Forms.DataGridView control = (System.Windows.Forms.DataGridView)dataGridView;
            return control.SelectionMode.ToString();
        }

        protected override void AddToForm(IDataGridView dgv)
        {
            System.Windows.Forms.Form form = new System.Windows.Forms.Form();
            form.Controls.Add((System.Windows.Forms.Control)dgv);
        }
    }
}