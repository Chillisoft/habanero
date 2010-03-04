using Gizmox.WebGUI.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;
using DataGridViewSelectionMode=Gizmox.WebGUI.Forms.DataGridViewSelectionMode;

namespace Habanero.Test.UI.VWG.Grid
{
    [TestFixture]
    public class TestEditableGridControlVWG : TestEditableGridControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
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

        protected override void AssertComboBoxItemCount(IDataGridViewColumn dataGridViewColumn, int expectedCount)
        {
            //TODO: get this code working again when the Gizmox bug is fixed in GridInitialiser
            //                DataGridViewColumnVWG dataGridViewColumnVWG = (DataGridViewColumnVWG)dataGridViewColumn;
            //                Assert.AreEqual(expectedCount,
            //                    ((Gizmox.WebGUI.Forms.DataGridViewComboBoxColumn)dataGridViewColumnVWG.DataGridViewColumn).Items.Count);
        }

        protected override void AssertMainControlsOnForm(IFormHabanero form)
        {
            Form formVWG = (Form)form;
            Assert.AreEqual(3, formVWG.Controls[0].Controls.Count);
            Assert.IsInstanceOf(typeof(IFilterControl), formVWG.Controls[0].Controls[1]);
            Assert.IsInstanceOf(typeof(IEditableGrid), formVWG.Controls[0].Controls[0]);
            Assert.IsInstanceOf(typeof(IButtonGroupControl), formVWG.Controls[0].Controls[2]);
        }

        protected override IEditableGridControl CreateEditableGridControl()
        {
            EditableGridControlVWG editableGridControlVWG = new EditableGridControlVWG(GetControlFactory());
            Form frm = new Form();
            frm.Controls.Add(editableGridControlVWG);
            return editableGridControlVWG;
        }

        [Test]
        protected override IFormHabanero AddControlToForm(IControlHabanero cntrl)
        {
            FormVWG form = (FormVWG)GetControlFactory().CreateForm();
            Form formVWG = form;
            formVWG.Controls.Add((Control)cntrl);

            return form;
        }

        //TODO_: if pagination gets introduced into Win, then move this test back out into the parent
        [Test]
        public void Test_Acceptance_Filter_When_On_Page2_Of_Pagination()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IEditableGridControl gridControl = GetGridWith_5_Rows(out col);
            AddControlToForm(gridControl);
            ITextBox tb = gridControl.FilterControl.AddStringFilterTextBox("Test Prop", "TestProp");
            //Set items per page to 3 items
            gridControl.Grid.ItemsPerPage = 3;
            //Go to page 2 (pagination page)
            gridControl.Grid.CurrentPage = 2;

            //--------------Assert PreConditions ---------------
            Assert.AreEqual(2, gridControl.Grid.CurrentPage);
            //---------------Execute Test ----------------------
            //enter data in filter for 1 item
            tb.Text = "b";
            gridControl.FilterControl.ApplyFilter();
            //---------------Test Result -----------------------
            // verify that grid has moved back to page 1
            Assert.AreEqual(1, gridControl.Grid.CurrentPage);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_RaiseErrorIfControlFactoryNull()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            try
            {
                new EditableGridControlVWG(null);
                Assert.Fail("expected Err");
            }
            //---------------Test Result -----------------------
            catch (HabaneroArgumentException ex)
            {
                StringAssert.Contains
                    ("Cannot create an editable grid control if the control factory is null", ex.Message);
            }
        }

        [Test, Ignore("Currently working on this - June 2008")]
        public void TestVWG_CheckBoxUIGridDef_Creates_CheckBoxColumn()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadClassDefWithBoolean();
            IClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridControl.Initialise(def);
            //---------------Test Result -----------------------
            IDataGridViewColumn column = gridControl.Grid.Columns["TestBoolean"];
            Assert.IsNotNull(column);
            Assert.IsInstanceOf(typeof(DataGridViewCheckBoxColumnVWG), column);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestVWGInitialise_SelectionEditMode()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            IClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Execute Test ----------------------
            gridControl.Initialise(def);
            //---------------Test Result -----------------------
            Assert.AreEqual
                (DataGridViewSelectionMode.RowHeaderSelect,
                 ((DataGridView)gridControl.Grid).SelectionMode);
            Assert.AreEqual
                (DataGridViewEditMode.EditOnKeystrokeOrF2,
                 ((DataGridView)gridControl.Grid).EditMode);
            //---------------Tear Down -------------------------
        }
        [Test]
        public void TestButtonsControl_ClickCancelRestoresGridToOriginalState()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IEditableGridControl gridControl = GetGridWith_5_Rows(out col);
            col.SaveAll();
            AddControlToForm(gridControl);
            //---------------Assert Precondition----------------
            Assert.AreEqual(5, gridControl.Grid.Rows.Count);
            Assert.AreEqual("b", gridControl.Grid.Rows[0].Cells[1].Value);
            //---------------Execute Test ----------------------
            gridControl.Grid.Rows[0].Cells[1].Value = "test";
            //---------------Assert Precondition----------------
            Assert.AreEqual("test", gridControl.Grid.Rows[0].Cells[1].Value);
            //---------------Execute Test ----------------------
            gridControl.Buttons["Cancel"].PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual("b", gridControl.Grid.Rows[0].Cells[1].Value);
        }

        protected override IClassDef LoadMyBoDefaultClassDef()
        {
             return MyBO.LoadDefaultClassDefVWG();
        }
    }
}