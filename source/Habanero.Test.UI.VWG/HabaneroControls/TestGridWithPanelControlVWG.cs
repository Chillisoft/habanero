using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestGridWithPanelControlVWG : TestGridWithPanelControl
    {

        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryVWG factory = new ControlFactoryVWG();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }

        protected override IBusinessObjectControl GetBusinessObjectControlStub()
        {
            return new BusinessObjectControlStubVWG();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)cntrl);
        }

        [Test]
        public void TestCancelButton_RestoreChangesGridText()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];

            MyBO currentBO = gridWithPanelControl.CurrentBusinessObject;
            string originalValue = currentBO.TestProp;
            string newValue = BOTestUtils.RandomString;
            currentBO.TestProp = newValue;
            //---------------Assert Precondition----------------
            Assert.AreNotEqual(originalValue, newValue);
            Assert.AreEqual(newValue, currentBO.TestProp);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(originalValue, currentBO.TestProp);
            Assert.AreEqual(originalValue, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[0].Cells["TestProp"].Value);
            Assert.AreSame(currentBO, gridWithPanelControl.CurrentBusinessObject);
        }

        [Test]
        public void TestStrategy_CorrectButtonStatus_ForInitialEmptyGrid()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_WithStrategy();

            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            IButton saveButton = gridWithPanelControl.Buttons["Save"];
            IButton newButton = gridWithPanelControl.Buttons["New"];
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(saveButton.Enabled);
            Assert.IsFalse(newButton.Enabled);
            Assert.IsFalse(deleteButton.Enabled);
            Assert.IsFalse(cancelButton.Enabled);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(saveButton.Enabled);
            Assert.IsTrue(newButton.Enabled);
            Assert.IsFalse(deleteButton.Enabled);
            Assert.IsFalse(cancelButton.Enabled);
        }

        [Test, Ignore("Still working on this.")]
        public void TestStrategy_CorrectButtonStatus_WhenGridCleared()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_WithStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);

            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            IButton saveButton = gridWithPanelControl.Buttons["Save"];
            IButton newButton = gridWithPanelControl.Buttons["New"];
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(saveButton.Enabled);
            Assert.IsTrue(newButton.Enabled);
            Assert.IsTrue(deleteButton.Enabled);
            Assert.IsTrue(cancelButton.Enabled);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(saveButton.Enabled);
            Assert.IsTrue(newButton.Enabled);
            Assert.IsFalse(deleteButton.Enabled);
            Assert.IsFalse(cancelButton.Enabled);
        }

        [Test]
        public void TestStrategy_DeleteButton_RemovesNewBO()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_WithStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            IButton newButton = gridWithPanelControl.Buttons["New"];
            newButton.PerformClick();
            MyBO currentBO = gridWithPanelControl.CurrentBusinessObject;
            //---------------Assert Precondition----------------
            Assert.IsTrue(deleteButton.Enabled);
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
        }

        [Test]
        public void TestStrategy_ConfirmSaveDialogAlwaysReturnsFalse()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GridWithPanelControlStrategyVWG<MyBO> strategyVWG = new GridWithPanelControlStrategyVWG<MyBO>(null);
            //---------------Test Result -----------------------
            Assert.IsFalse(strategyVWG.ShowConfirmSaveDialog);
        }

        [Test]
        public void TestStrategy_ApplyChangesReturnsFalse()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GridWithPanelControlStrategyVWG<MyBO> strategyVWG = new GridWithPanelControlStrategyVWG<MyBO>(null);
            //---------------Test Result -----------------------
            Assert.IsTrue(strategyVWG.CallApplyChangesToEditBusinessObject);
        }

        [Test]
        public void TestStrategy_RefreshGridReturnsTrue()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            GridWithPanelControlStrategyVWG<MyBO> strategyVWG = new GridWithPanelControlStrategyVWG<MyBO>(null);
            //---------------Test Result -----------------------
            Assert.IsTrue(strategyVWG.RefreshGrid);
        }


        [Test]
        public void TestSaveButton_UpdatesGridText()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_WithStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton saveButton = gridWithPanelControl.Buttons["Save"];

            MyBO currentBO = gridWithPanelControl.CurrentBusinessObject;
            string originalValue = currentBO.TestProp;
            string newValue = BOTestUtils.RandomString;
            PanelInfo.FieldInfo testPropFieldInfo = ((IBusinessObjectPanel)gridWithPanelControl.BusinessObjectControl).PanelInfo.FieldInfos["TestProp"];
            testPropFieldInfo.ControlMapper.Control.Text = newValue;

            //---------------Assert Precondition----------------
            Assert.AreNotEqual(originalValue, newValue);
            Assert.AreEqual(originalValue, currentBO.TestProp);
            Assert.AreEqual(originalValue, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[0].Cells["TestProp"].Value);
            //---------------Execute Test ----------------------
            saveButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(newValue, currentBO.TestProp);
            Assert.AreEqual(newValue,
                            gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[0].Cells["TestProp"].Value);
            Assert.AreSame(currentBO, gridWithPanelControl.CurrentBusinessObject);
        }

        [Test]
        public void TestGridRowChange_DoesNotChangeWhenBOInvalid()
        {
            //---------------Set up test pack-------------------
            IClassDef classDef = ClassDef.Get<MyBO>();
            classDef.PropDefcol["TestProp"].Compulsory = true;
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_WithStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            MyBO firstBO = myBOs[0];
            MyBO secondBO = myBOs[1];

            PanelInfo.FieldInfo testPropFieldInfo = ((IBusinessObjectPanel)gridWithPanelControl.BusinessObjectControl).PanelInfo.FieldInfos["TestProp"];
            testPropFieldInfo.ControlMapper.Control.Text = "";
            //---------------Assert Precondition----------------
            //Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.IsFalse(firstBO.Status.IsNew);
            //Assert.IsFalse(firstBO.IsValid());
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject);
            //---------------Execute Test ----------------------
            gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject = secondBO;
            //gridWithPanelControl.ReadOnlyGridControl.Grid.CurrentCell =
            //    gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[1].Cells[1];
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject);
            Assert.IsTrue(firstBO.Status.IsDirty);
        }

        [Test]
        public void TestCancelButton_BOAndControlsAreSynchronised()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_WithStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            MyBO firstBO = myBOs[0];
            string originalValue = firstBO.TestProp;
            string newValue = BOTestUtils.RandomString;

            PanelInfo.FieldInfo testPropFieldInfo = ((IBusinessObjectPanel)gridWithPanelControl.BusinessObjectControl).PanelInfo.FieldInfos["TestProp"];
            testPropFieldInfo.ControlMapper.Control.Text = newValue;
            //---------------Assert Precondition----------------
            Assert.IsFalse(firstBO.Status.IsNew);
            Assert.IsFalse(firstBO.Status.IsDirty);
            Assert.AreNotEqual(originalValue, newValue);
            Assert.AreNotEqual(newValue, firstBO.TestProp);   //vwg doesn't synchronise by default
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(firstBO.Status.IsNew);
            Assert.IsFalse(firstBO.Status.IsDirty);
            Assert.AreEqual(originalValue, firstBO.TestProp);
            Assert.AreEqual(originalValue, testPropFieldInfo.ControlMapper.Control.Text);
        }
    }
}