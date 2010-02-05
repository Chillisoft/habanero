using Habanero.Test.UI.Base.Grid;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Grid
{
    [TestFixture]
    public class TestDataGridViewVWG : TestDataGridView
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override string GetUnderlyingDataGridViewSelectionModeToString(IDataGridView dataGridView)
        {
            Gizmox.WebGUI.Forms.DataGridView control = (Gizmox.WebGUI.Forms.DataGridView)dataGridView;
            return control.SelectionMode.ToString();
        }

        protected override void AddToForm(IDataGridView dgv)
        {
            Gizmox.WebGUI.Forms.Form form = new Gizmox.WebGUI.Forms.Form();
            form.Controls.Add((Gizmox.WebGUI.Forms.Control)dgv);
        }
    }
}