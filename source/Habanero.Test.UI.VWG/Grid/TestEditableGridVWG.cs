using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Grid
{
    public class TestEditableGridVWG : TestEditableGrid
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override IEditableGrid CreateEditableGrid()
        {
            EditableGridVWG editableGridVWG = new EditableGridVWG();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add(editableGridVWG);
            return editableGridVWG;
        }

        protected override IFormHabanero AddControlToForm(IGridBase gridBase)
        {
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)gridBase);
            return null;
        }


        protected override void LoadMyBoDefaultClassDef()
        {
            MyBO.LoadDefaultClassDefVWG();
        }


        protected override void AssertIsTextBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG)dataGridViewColumn;
            Assert.IsInstanceOfType
                (typeof(Gizmox.WebGUI.Forms.DataGridViewTextBoxColumn), dataGridViewColumnVWG.DataGridViewColumn);
        }

        protected override void AssertIsCheckBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG)dataGridViewColumn;
            Assert.IsInstanceOfType
                (typeof(Gizmox.WebGUI.Forms.DataGridViewCheckBoxColumn), dataGridViewColumnVWG.DataGridViewColumn);
        }

        protected override void AssertIsComboBoxColumnType(IDataGridViewColumn dataGridViewColumn)
        {
            DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG)dataGridViewColumn;
            Assert.IsInstanceOfType
                (typeof(Gizmox.WebGUI.Forms.DataGridViewComboBoxColumn), dataGridViewColumnVWG.DataGridViewColumn);
        }

        protected override void AssertCorrectSelectionMode(IGridBase dataGridView)
        {
            Gizmox.WebGUI.Forms.DataGridView grid = (Gizmox.WebGUI.Forms.DataGridView)dataGridView;
            Assert.AreEqual(Gizmox.WebGUI.Forms.DataGridViewSelectionMode.RowHeaderSelect, grid.SelectionMode);
        }
    }
}