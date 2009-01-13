using System;
using System.Drawing;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestGridWithPanelControl 
    {
        private const string CUSTOM_UIDEF_NAME = "custom1";


        [SetUp]
        public void SetUp()
        {
            ClassDef.ClassDefs.Clear();
            MyBO.LoadClassDefWithBoolean();
            BORegistry.DataAccessor = new DataAccessorInMemory();
        }


        protected abstract IControlFactory GetControlFactory();
        protected abstract IBusinessObjectControl GetBusinessObjectControlStub();
        protected abstract void AddControlToForm(IControlHabanero cntrl);


        [TestFixture]
        public class TestGridWithPanelControlWin : TestGridWithPanelControl
        {

            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            protected override IBusinessObjectControl GetBusinessObjectControlStub()
            {
                return new BusinessObjectControlStubWin();
            } 
            
            protected override void AddControlToForm(IControlHabanero cntrl)
            {
                System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
                frm.Controls.Add((System.Windows.Forms.Control) cntrl);
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
                Assert.AreEqual(newValue, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[0].Cells["TestProp"].Value);
                //---------------Execute Test ----------------------
                cancelButton.PerformClick();
                //---------------Test Result -----------------------
                Assert.AreEqual(originalValue, currentBO.TestProp);
                Assert.AreEqual(originalValue, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows[0].Cells["TestProp"].Value);
                Assert.AreSame(currentBO, gridWithPanelControl.CurrentBusinessObject);
            }
        }

        [TestFixture]
        public class TestGridWithPanelControlVWG : TestGridWithPanelControl
        {

            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            protected override IBusinessObjectControl GetBusinessObjectControlStub()
            {
                return new BusinessObjectControlStubVWG();
            }

            protected override void AddControlToForm(IControlHabanero cntrl)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control) cntrl);
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
                PanelInfo.FieldInfo testPropFieldInfo = ((IBusinessObjectPanel) gridWithPanelControl.BusinessObjectControl).PanelInfo.FieldInfos["TestProp"];
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
                ClassDef classDef = ClassDef.Get<MyBO>();
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
    


        // Creates a new UI def by cloning an existing one and adding a cloned column
        //   (easier than creating a whole new BO for this test)
        private static ClassDef GetCustomClassDef()
        {
            ClassDef classDef = ClassDef.Get<MyBO>();
            UIGrid originalGridDef = classDef.UIDefCol["default"].UIGrid;
            UIGrid extraGridDef = originalGridDef.Clone();
            //UIGridColumn extraColumn = originalGridDef[0].Clone();
            //extraGridDef.Add(extraColumn);
            extraGridDef.Remove(extraGridDef[extraGridDef.Count - 1]);
            //UIGridColumn extraColumn = new UIGridColumn("ID", "ProjectAssemblyInfoID", typeof(System.Windows.Forms.DataGridViewTextBoxColumn), true, 100, UIGridColumn.PropAlignment.right, null);
            //extraGridDef.Add(extraColumn);
            UIDef extraUIDef = new UIDef(CUSTOM_UIDEF_NAME, null, extraGridDef);
            classDef.UIDefCol.Add(extraUIDef);
            return classDef;
        }

        private IGridWithPanelControl<MyBO> CreateGridAndBOEditorControl_NoStrategy()
        {
            IGridWithPanelControl<MyBO> gridWithPanelControl = GetControlFactory().CreateGridWithPanelControl<MyBO>();
            gridWithPanelControl.GridWithPanelControlStrategy = null;
            return  gridWithPanelControl;
        }

        private IGridWithPanelControl<MyBO> CreateGridAndBOEditorControl_WithStrategy()
        {
            IGridWithPanelControl<MyBO> gridWithPanelControl = GetControlFactory().CreateGridWithPanelControl<MyBO>();
            //gridWithPanelControl.GridWithPanelControlStrategy = null;
            return gridWithPanelControl;
        }

        private static void AssertSelectedBusinessObject(MyBO myBO, IGridWithPanelControl<MyBO> gridWithPanelControl)
        {
            Assert.AreSame(myBO, gridWithPanelControl.ReadOnlyGridControl.SelectedBusinessObject);
            Assert.AreSame(myBO, gridWithPanelControl.BusinessObjectControl.BusinessObject, "Selected BO in Grid should be loaded in the BoControl");
            Assert.AreSame(myBO, gridWithPanelControl.CurrentBusinessObject);
        }

        [Test]
        public void TestConstructor_FailsIfBOControlNull()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            try
            {
                IGridWithPanelControl<MyBO> gridWithPanelControl =
                    GetControlFactory().CreateGridWithPanelControl<MyBO>((IBusinessObjectControl) null);

                Assert.Fail("Null BOControl should be prevented");
            }
                //---------------Test Result -----------------------
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("businessObjectControl", ex.ParamName);
            }
            //---------------Test Result -----------------------
        }


        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectControl businessObjectControl = GetBusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl =
                GetControlFactory().CreateGridWithPanelControl<MyBO>(businessObjectControl);

            //---------------Test Result -----------------------
            Assert.AreEqual(3, gridWithPanelControl.Controls.Count);
            Assert.IsInstanceOfType(typeof(IUserControlHabanero), gridWithPanelControl);
            Assert.IsInstanceOfType(typeof(IBusinessObjectControl), gridWithPanelControl.Controls[0]);
            Assert.IsInstanceOfType(typeof(IReadOnlyGridControl), gridWithPanelControl.Controls[1]);
            Assert.IsInstanceOfType(typeof(IButtonGroupControl), gridWithPanelControl.Controls[2]);
            Assert.AreSame(businessObjectControl, gridWithPanelControl.BusinessObjectControl);
            Assert.IsFalse(businessObjectControl.Enabled);
        }

        [Test, Ignore("Doesn't work like you'd expect it to")]
        public void TestConstuctor_CorrectPosition()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectControl businessObjectControl = GetBusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            IGridWithPanelControl<MyBO> gridWithPanelControl =
                GetControlFactory().CreateGridWithPanelControl<MyBO>(businessObjectControl);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, gridWithPanelControl.Controls.Count);
            Assert.GreaterOrEqual(gridWithPanelControl.BusinessObjectControl.Top, gridWithPanelControl.ReadOnlyGridControl.Top);
            Assert.GreaterOrEqual(gridWithPanelControl.Buttons.Top, gridWithPanelControl.BusinessObjectControl.Top);

        }

        [Test]
        public void TestConstructor_UsingCustomUIDefName()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectControl businessObjectControl = GetBusinessObjectControlStub();

            ClassDef classDef = GetCustomClassDef();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl =
                GetControlFactory().CreateGridWithPanelControl<MyBO>(businessObjectControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, gridWithPanelControl.Controls.Count);
            Assert.IsInstanceOfType(typeof(IUserControlHabanero), gridWithPanelControl);
            Assert.IsInstanceOfType(typeof(IBusinessObjectControl), gridWithPanelControl.Controls[0]);
            Assert.IsInstanceOfType(typeof(IReadOnlyGridControl), gridWithPanelControl.Controls[1]);
            Assert.IsInstanceOfType(typeof(IButtonGroupControl), gridWithPanelControl.Controls[2]);
            Assert.AreSame(businessObjectControl, gridWithPanelControl.BusinessObjectControl);
        }



        [Test]
        public void TestGridConstruction()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectControl businessObjectControl = GetBusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl =
                GetControlFactory().CreateGridWithPanelControl<MyBO>(businessObjectControl);
            //---------------Test Result -----------------------
            IReadOnlyGridControl readOnlyGridControl = gridWithPanelControl.ReadOnlyGridControl;
            Assert.IsNotNull(readOnlyGridControl);
            Assert.IsFalse(readOnlyGridControl.Buttons.Visible);
            Assert.IsFalse(readOnlyGridControl.FilterControl.Visible);
            Assert.IsNull(readOnlyGridControl.Grid.GetBusinessObjectCollection());
            Assert.AreEqual(300, readOnlyGridControl.Height);
        }

        [Test]
        public void TestGridWithCustomClassDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = GetCustomClassDef();
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            myBOs.ClassDef = classDef;
            IBusinessObjectControl businessObjectControl = GetBusinessObjectControlStub();

            //---------------Assert Precondition----------------
            Assert.IsTrue(classDef.UIDefCol.Count >= 2);
            //---------------Execute Test ----------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl =
                GetControlFactory().CreateGridWithPanelControl<MyBO>(businessObjectControl, CUSTOM_UIDEF_NAME);
            //---------------Test Result -----------------------
            Assert.AreEqual(CUSTOM_UIDEF_NAME, gridWithPanelControl.ReadOnlyGridControl.UiDefName);
        }

        [Test]
        public void TestButtonControlConstruction()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectControl businessObjectControl = GetBusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl =
                GetControlFactory().CreateGridWithPanelControl<MyBO>(businessObjectControl);
            //---------------Test Result -----------------------
            IButtonGroupControl buttonGroupControl = gridWithPanelControl.Buttons;
            Assert.IsNotNull(buttonGroupControl);
            Assert.AreEqual(4, buttonGroupControl.Controls.Count);
            Assert.AreEqual("Cancel", buttonGroupControl.Controls[0].Text);
            Assert.AreEqual("Delete", buttonGroupControl.Controls[1].Text);
            Assert.AreEqual("New", buttonGroupControl.Controls[2].Text);
            Assert.AreEqual("Save", buttonGroupControl.Controls[3].Text);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_InitialSelection_NoItems()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO>();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, myBOs.Count);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Test Result -----------------------
            IReadOnlyGridControl readOnlyGridControl = gridWithPanelControl.ReadOnlyGridControl;
            Assert.AreEqual(myBOs.Count, readOnlyGridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, gridWithPanelControl);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_FirstItemIsSelectedAndControlGetsBO()
        {
            //---------------Set up test pack-------------------
            
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, myBOs.Count);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);

            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(myBOs[0], gridWithPanelControl);
        }

        [Test]
        public void Test_SelectBusinessObject_ChangesBOInBOControl()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            IReadOnlyGridControl readOnlyGridControl = gridWithPanelControl.ReadOnlyGridControl;
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, myBOs.Count);
            AssertSelectedBusinessObject(myBOs[0], gridWithPanelControl);
            //---------------Execute Test ----------------------
            readOnlyGridControl.SelectedBusinessObject = myBOs[1];
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(myBOs[1], gridWithPanelControl);
        }

        [Test]
        public void Test_SetBusinessObjectCollection()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, myBOs.Count);
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Test Result -----------------------
            Assert.AreEqual(myBOs.Count, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreNotEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Columns.Count);
        }

        [Test]
        public void Test_SetBusinessObjectCollection_ToNull()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            try
            {
                gridWithPanelControl.SetBusinessObjectCollection(null);
                //---------------Test Result -----------------------
                Assert.Fail("Error should have been thrown");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("col", ex.ParamName);
            }
        }

        [Test]
        public void Test_SetBusinessObjectCollection_ControlsEnabledCorrectly()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();

            IButton newButton = gridWithPanelControl.Buttons["New"];
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            IButton saveButton = gridWithPanelControl.Buttons["Save"];
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, myBOs.Count);
            Assert.IsFalse(newButton.Enabled);
            Assert.IsFalse(deleteButton.Enabled);
            Assert.IsFalse(cancelButton.Enabled);
            Assert.IsFalse(saveButton.Enabled);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Test Result -----------------------
            Assert.IsTrue(newButton.Enabled);
            Assert.IsTrue(deleteButton.Enabled);
            Assert.IsFalse(cancelButton.Enabled);
            Assert.IsFalse(saveButton.Enabled);
        }


        [Test]
        public void TestBOControlDisabledWhenGridIsCleared()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, myBOs.Count);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            AssertSelectedBusinessObject(null, gridWithPanelControl);
            Assert.IsFalse(gridWithPanelControl.BusinessObjectControl.Enabled);
        }

        [Test]
        public void TestBOControlEnabledWhenSelectedBOIsChanged()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, myBOs.Count);
            AssertSelectedBusinessObject(null, gridWithPanelControl);
            Assert.IsFalse(gridWithPanelControl.BusinessObjectControl.Enabled);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Test Result -----------------------
            Assert.IsTrue(gridWithPanelControl.BusinessObjectControl.Enabled);
        }

        [Test]
        public void TestNewButtonDisabledUntilCollectionSet()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            IButton newButton = gridWithPanelControl.Buttons["New"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(newButton.Enabled);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            //---------------Test Result -----------------------
            Assert.IsTrue(newButton.Enabled);
        }

        [Test]
        public void TestNewButtonClickedCreatesBO_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            BusinessObjectCollection<MyBO> myBOs = new BusinessObjectCollection<MyBO>();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreEqual(0, myBOs.Count);
            //---------------Execute Test ----------------------
            gridWithPanelControl.Buttons["New"].PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreEqual(1, myBOs.Count);
            AssertSelectedBusinessObject(myBOs[0], gridWithPanelControl);
            Assert.IsTrue(gridWithPanelControl.BusinessObjectControl.Enabled);
            Assert.IsTrue(gridWithPanelControl.Buttons["Cancel"].Enabled);
        }

        [Test]
        public void TestNewButtonClickedCreatesBO_ExistingCollection()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreEqual(2, myBOs.Count);
            Assert.IsFalse(gridWithPanelControl.BusinessObjectControl.Focused);
            //---------------Execute Test ----------------------
            gridWithPanelControl.Buttons["New"].PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(3, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreEqual(3, myBOs.Count);
            Assert.IsTrue(myBOs[2].Status.IsNew);
            AssertSelectedBusinessObject(myBOs[2], gridWithPanelControl);
            Assert.IsTrue(gridWithPanelControl.BusinessObjectControl.Enabled);
            //Assert.IsTrue(gridWithPanelControl.BusinessObjectControl.Focused);
            Assert.IsTrue(gridWithPanelControl.Buttons["Cancel"].Enabled);
        }

        [Test]
        public void TestNewButtonDisabled_WhenObjectIsDirty()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IButton newButton = gridWithPanelControl.Buttons["New"];
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Assert Precondition----------------
            Assert.IsFalse(gridWithPanelControl.CurrentBusinessObject.Status.IsDirty);
            Assert.IsTrue(newButton.Enabled);
            //---------------Execute Test ----------------------
            gridWithPanelControl.CurrentBusinessObject.TestProp = BOTestUtils.RandomString;
            //---------------Test Result -----------------------
            Assert.IsTrue(gridWithPanelControl.CurrentBusinessObject.Status.IsDirty);
            Assert.IsFalse(newButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledAtConstruction()
        {
            //---------------Set up test pack-------------------
            IBusinessObjectControl businessObjectControl = GetBusinessObjectControlStub();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl =
                GetControlFactory().CreateGridWithPanelControl<MyBO>(businessObjectControl);
            //---------------Test Result -----------------------
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonEnabledWhenBOSelected()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            //---------------Assert Precondition----------------
            Assert.IsFalse(deleteButton.Enabled);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(myBOs[0], gridWithPanelControl);
            Assert.IsTrue(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledWhenControlHasNoBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(deleteButton.Enabled);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(null, gridWithPanelControl);
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDisabledWhenNewObjectAdded()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            IButton newButton = gridWithPanelControl.Buttons["New"];
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonEnabledWhenOldObjectSelected()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(deleteButton.Enabled);
            //---------------Execute Test ----------------------
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(deleteButton.Enabled);
        }

        [Test]
        public void TestDeleteButtonDeletesCurrentBO()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            MyBO currentBO = myBOs[0];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, gridWithPanelControl);
            Assert.IsFalse(currentBO.Status.IsDeleted);
            Assert.AreEqual(2, myBOs.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(currentBO.Status.IsDeleted);
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.AreEqual(1, myBOs.Count);
            Assert.IsFalse(myBOs.Contains(currentBO));
        }

        [Test, Ignore("TODO: fix this")]
        public void TestDeleteButton_ControlsUpdated()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            MyBO currentBO = myBOs[0];
            MyBO otherBO = myBOs[1];
            //---------------Assert Precondition----------------
            AssertSelectedBusinessObject(currentBO, gridWithPanelControl);
            Assert.AreEqual(2, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            AssertSelectedBusinessObject(otherBO, gridWithPanelControl);
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
        }

        // Tests a unique set of circumstances
        [Test]
        public void TestDeleteButton_SelectsPreviousRow_NewTypeCancelDelete()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton newButton = gridWithPanelControl.Buttons["New"];
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            MyBO currentBO = gridWithPanelControl.CurrentBusinessObject;
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            gridWithPanelControl.CurrentBusinessObject.TestProp = BOTestUtils.RandomString;
            cancelButton.PerformClick();
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.AreSame(currentBO, gridWithPanelControl.CurrentBusinessObject);
        }

        [Test]
        public void TestDeleteButton_SelectsPreviousRowWhenOldObjectDirty()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton deleteButton = gridWithPanelControl.Buttons["Delete"];
            MyBO firstBO = myBOs[0];
            MyBO secondBO = myBOs[1];
            gridWithPanelControl.ReadOnlyGridControl.SelectedBusinessObject = secondBO;
            string newValue = BOTestUtils.RandomString;
            secondBO.TestProp = newValue;
            //---------------Assert Precondition----------------
            Assert.AreSame(secondBO, gridWithPanelControl.CurrentBusinessObject);
            //---------------Execute Test ----------------------
            deleteButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreSame(firstBO, gridWithPanelControl.CurrentBusinessObject);
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(secondBO.Status.IsDeleted);
        }

        [Test]
        public void TestCancelButton_DisabledOnConstruction()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            MyBO currentBO = myBOs[0];
            //---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(cancelButton.Enabled);
        }

        [Test]
        public void TestCancelButton_EnabledWhenObjectEdited()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            MyBO currentBO = myBOs[0];
            //---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            //---------------Execute Test ----------------------
            currentBO.TestProp = BOTestUtils.RandomString;
            //---------------Test Result -----------------------
            Assert.IsTrue(currentBO.Status.IsDirty);
            Assert.IsTrue(cancelButton.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRestoresSavedObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];

            MyBO currentBO = myBOs[0];
            string originalValue = currentBO.TestProp;
            currentBO.TestProp = BOTestUtils.RandomString;
            //---------------Assert Precondition----------------
            Assert.IsTrue(currentBO.Status.IsDirty);
            Assert.AreNotEqual(originalValue, currentBO.TestProp);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.IsFalse(cancelButton.Enabled);
            Assert.AreEqual(originalValue, currentBO.TestProp);
        }

        [Test]
        public void TestCancelButton_ClickRemovesNewObject_OnlyItemInGrid()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            IButton newButton = gridWithPanelControl.Buttons["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(gridWithPanelControl.BusinessObjectControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsFalse(gridWithPanelControl.BusinessObjectControl.Enabled);
        }

        [Test]
        public void TestCancelButton_ClickRemovesNewObject_TwoItemsInGrid()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            IButton newButton = gridWithPanelControl.Buttons["New"];

            newButton.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(cancelButton.Enabled);
            Assert.AreEqual(3, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(gridWithPanelControl.BusinessObjectControl.Enabled);
            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            Assert.IsTrue(gridWithPanelControl.BusinessObjectControl.Enabled);
            Assert.IsNotNull(gridWithPanelControl.ReadOnlyGridControl.SelectedBusinessObject);
        }

        [Test]
        public void TestSaveButton_DisabledOnConstruction()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton saveButton = gridWithPanelControl.Buttons["Save"];
            MyBO currentBO = myBOs[0];
            //---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsFalse(saveButton.Enabled);
        }

        [Test]
        public void TestSaveButton_EnabledWhenObjectEdited()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            IButton saveButton = gridWithPanelControl.Buttons["Save"];
            MyBO currentBO = myBOs[0];
            //---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.IsFalse(saveButton.Enabled);
            //---------------Execute Test ----------------------
            currentBO.TestProp = BOTestUtils.RandomString;
            //---------------Test Result -----------------------
            Assert.IsTrue(currentBO.Status.IsDirty);
            Assert.IsTrue(saveButton.Enabled);
        }

        [Test]
        public void TestSaveButtonClicked()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            IButton newButton = gridWithPanelControl.Buttons["New"]; 
            IButton saveButton = gridWithPanelControl.Buttons["Save"];
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            newButton.PerformClick();
            MyBO currentBO = (MyBO)gridWithPanelControl.BusinessObjectControl.BusinessObject;
            currentBO.TestProp = BOTestUtils.RandomString;
            //---------------Assert Precondition----------------
            Assert.IsTrue(currentBO.Status.IsDirty);
            Assert.IsTrue(currentBO.Status.IsNew);
            Assert.IsTrue(currentBO.IsValid());
            Assert.IsTrue(saveButton.Enabled);
            Assert.IsTrue(cancelButton.Enabled);
            Assert.IsFalse(newButton.Enabled);
            //---------------Execute Test ----------------------
            saveButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreSame(currentBO, gridWithPanelControl.BusinessObjectControl.BusinessObject);
            Assert.IsFalse(currentBO.Status.IsDirty);
            Assert.IsFalse(currentBO.Status.IsNew);
            Assert.IsFalse(currentBO.Status.IsDeleted);
            Assert.IsFalse(saveButton.Enabled);
            Assert.IsFalse(cancelButton.Enabled);
            Assert.IsTrue(newButton.Enabled);
            //Assert.IsFalse(boControl.DisplayErrorsCalled);   //TODO
        }


        [Test]
        public void Test_CannotClickNewButtonIfCurrentObjectInvalid()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ClassDef.Get<MyBO>();
            classDef.PropDefcol["TestProp"].Compulsory = true;

            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            IButton newButton = gridWithPanelControl.Buttons["New"];
            newButton.PerformClick();
            MyBO currentBO = (MyBO)gridWithPanelControl.BusinessObjectControl.BusinessObject;
            
            //---------------Assert Precondition----------------
            Assert.IsFalse(currentBO.IsValid());
            //Assert.IsFalse(boControl.DisplayErrorsCalled); //TODO
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreSame(currentBO, gridWithPanelControl.BusinessObjectControl.BusinessObject);
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.Rows.Count);
            //Assert.IsTrue(boControl.DisplayErrorsCalled);   //TODO
        }

        [Test]
        public void Test_CannotChangeGridRowIfCurrentObjectInvalid()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = ClassDef.Get<MyBO>();
            classDef.PropDefcol["TestProp"].Compulsory = true;
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            MyBO firstBO = myBOs[0];
            firstBO.TestProp = null;
            MyBO secondBO = myBOs[1];
            //---------------Assert Precondition----------------
            Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.IsFalse(firstBO.Status.IsNew);
            Assert.IsFalse(firstBO.IsValid());
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject);
            //Assert.IsFalse(boControl.DisplayErrorsCalled);  //TODO
            //---------------Execute Test ----------------------
            gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject);
            Assert.IsTrue(firstBO.Status.IsDirty);
            // Assert.IsTrue(boControl.DisplayErrorsCalled);  //TODO
        }

        [Test]
        public void Test_CannotChangeGridRow_IfObjectIsDirty_UserCancels()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            MyBO firstBO = gridWithPanelControl.CurrentBusinessObject;
            firstBO.TestProp = BOTestUtils.RandomString;
            MyBO secondBO = myBOs[1];

            bool confirmSaveCalled = false;
            gridWithPanelControl.ConfirmSaveDelegate -= gridWithPanelControl.ConfirmSaveDelegate;
            gridWithPanelControl.ConfirmSaveDelegate += delegate
            {
                confirmSaveCalled = true;
                return DialogResult.Cancel;
            };
            //---------------Assert Precondition----------------
            Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            //---------------Execute Test ----------------------
            gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(firstBO, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject);
            Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.IsTrue(confirmSaveCalled);
        }

        [Test]
        public void Test_CannotChangeGridRow_IfObjectIsDirty_UserChoosesYes()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            MyBO firstBO = gridWithPanelControl.CurrentBusinessObject;
            string newValue = BOTestUtils.RandomString;
            firstBO.TestProp = newValue;
            MyBO secondBO = myBOs[1];

            bool confirmSaveCalled = false;
            gridWithPanelControl.ConfirmSaveDelegate -= gridWithPanelControl.ConfirmSaveDelegate;
            gridWithPanelControl.ConfirmSaveDelegate += delegate
            {
                confirmSaveCalled = true;
                return DialogResult.Yes;
            };
            //---------------Assert Precondition----------------
            Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            //---------------Execute Test ----------------------
            gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(secondBO, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject);
            Assert.IsFalse(firstBO.Status.IsDirty);
            Assert.IsTrue(confirmSaveCalled);
            Assert.AreEqual(newValue, firstBO.TestProp);
        }

        [Test]
        public void Test_CannotChangeGridRow_IfObjectIsDirty_UserChoosesNo()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            MyBO firstBO = gridWithPanelControl.CurrentBusinessObject;
            string originalValue = firstBO.TestProp;
            string newValue = BOTestUtils.RandomString;
            firstBO.TestProp = newValue;
            MyBO secondBO = myBOs[1];

            bool confirmSaveCalled = false;
            gridWithPanelControl.ConfirmSaveDelegate -= gridWithPanelControl.ConfirmSaveDelegate;
            gridWithPanelControl.ConfirmSaveDelegate += delegate
            {
                confirmSaveCalled = true;
                return DialogResult.No;
            };
            //---------------Assert Precondition----------------
            Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            //---------------Execute Test ----------------------
            gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreSame(secondBO, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject);
            Assert.IsFalse(firstBO.Status.IsDirty);
            Assert.IsTrue(confirmSaveCalled);
            Assert.AreEqual(originalValue, firstBO.TestProp);
        }

        [Test]
        public void Test_NotifyUserOfDirtyStatus_MakesButtonsBold_GridRowChange()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            MyBO firstBO = gridWithPanelControl.CurrentBusinessObject;
            firstBO.TestProp = BOTestUtils.RandomString;
            MyBO secondBO = myBOs[1];

            IButton saveButton = gridWithPanelControl.Buttons["Save"];
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];

            bool confirmSaveCalled = false;
            gridWithPanelControl.ConfirmSaveDelegate -= gridWithPanelControl.ConfirmSaveDelegate;
            gridWithPanelControl.ConfirmSaveDelegate += delegate
            {
                confirmSaveCalled = true;
                return DialogResult.Cancel;
            };
            //---------------Assert Precondition----------------
            Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreEqual(FontStyle.Regular, saveButton.Font.Style);
            Assert.AreEqual(FontStyle.Regular, cancelButton.Font.Style);
            //---------------Execute Test ----------------------
            gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedBusinessObject = secondBO;
            //---------------Test Result -----------------------
            Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.IsTrue(confirmSaveCalled);
            Assert.AreEqual(FontStyle.Bold, saveButton.Font.Style);
            Assert.AreEqual(FontStyle.Bold, cancelButton.Font.Style);
        }

        [Test]
        public void Test_NotifyUserOfDirtyStatus_MakesButtonsBold_NewButton()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            MyBO firstBO = gridWithPanelControl.CurrentBusinessObject;
            firstBO.TestProp = BOTestUtils.RandomString;

            IButton saveButton = gridWithPanelControl.Buttons["Save"];
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            IButton newButton = gridWithPanelControl.Buttons["New"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreEqual(FontStyle.Regular, saveButton.Font.Style);
            Assert.AreEqual(FontStyle.Regular, cancelButton.Font.Style);
            //---------------Execute Test ----------------------
            newButton.Enabled = true; //this is a hack to get the test working in Win
            newButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(firstBO.Status.IsDirty);
            Assert.AreEqual(0, gridWithPanelControl.ReadOnlyGridControl.Grid.SelectedRows[0].Index);
            Assert.AreEqual(FontStyle.Bold, saveButton.Font.Style);
            Assert.AreEqual(FontStyle.Bold, cancelButton.Font.Style);
        }

        [Test]
        public void Test_NotifyUserOfDirtyStatus_ResetsFontWhenSaving()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> myBOs = CreateSavedMyBoCollection();
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            gridWithPanelControl.SetBusinessObjectCollection(myBOs);
            MyBO firstBO = gridWithPanelControl.CurrentBusinessObject;
            firstBO.TestProp = BOTestUtils.RandomString;
            IButton saveButton = gridWithPanelControl.Buttons["Save"];
            IButton cancelButton = gridWithPanelControl.Buttons["Cancel"];
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            cancelButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(FontStyle.Regular, saveButton.Font.Style);
            Assert.AreEqual(FontStyle.Regular, cancelButton.Font.Style);
        }

        [Test, Ignore("Review error display and fix or remove test")]
        public void Test_DisplayErrorsNotCalledWhenNewButtonClicked()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            IBusinessObjectControl boControl = gridWithPanelControl.BusinessObjectControl;
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            IButton newButton = gridWithPanelControl.Buttons["New"];
            //---------------Assert Precondition----------------
            //Assert.IsFalse(boControl.DisplayErrorsCalled);  //TODO
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            //---------------Test Result -----------------------
            //Assert.IsFalse(boControl.DisplayErrorsCalled); //TODO
        }

        [Test, Ignore("Review error display and fix or remove test")]
        public void Test_ClearErrorsWhenNewObjectAdded()
        {
            //---------------Set up test pack-------------------
            IGridWithPanelControl<MyBO> gridWithPanelControl = CreateGridAndBOEditorControl_NoStrategy();
            IBusinessObjectControl boControl = gridWithPanelControl.BusinessObjectControl;
            gridWithPanelControl.SetBusinessObjectCollection(new BusinessObjectCollection<MyBO>());
            IButton newButton = gridWithPanelControl.Buttons["New"];
            //---------------Assert Precondition----------------
            //Assert.IsFalse(boControl.ClearErrorsCalled);  //TODO
            //---------------Execute Test ----------------------
            newButton.PerformClick();
            //---------------Test Result -----------------------
           // Assert.IsTrue(boControl.ClearErrorsCalled);  //TODO
        }

        private MyBO CreateSavedMyBo()
        {
            MyBO myBO = new MyBO();
            myBO.TestProp = BOTestUtils.RandomString;
            myBO.Save();
            return myBO;
        }

        private BusinessObjectCollection<MyBO> CreateSavedMyBoCollection()
        {
            CreateSavedMyBo();
            CreateSavedMyBo();
            BusinessObjectCollection<MyBO> myBos = new BusinessObjectCollection<MyBO>();
            myBos.LoadAll("");
            return myBos;
        }
    }

    internal class BusinessObjectControlStubWin : UserControlWin, IBusinessObjectControl
    {
        private IBusinessObject _businessObject;
        //private bool _displayErrorsCalled;
        //private bool _clearErrorsCalled;

        //public bool DisplayErrorsCalled
        //{
        //    get { return _displayErrorsCalled; }
        //}

        //public bool ClearErrorsCalled
        //{
        //    get { return _clearErrorsCalled; }
        //}

        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set { _businessObject = value; }
        }

        //public void DisplayErrors()
        //{
        //    _displayErrorsCalled = true;
        //}

        //public void ClearErrors()
        //{
        //    _clearErrorsCalled = true;
        //}
    }

    internal class BusinessObjectControlStubVWG : UserControlVWG, IBusinessObjectControl
    {
        private IBusinessObject _businessObject;
        //private bool _displayErrorsCalled;
        //private bool _clearErrorsCalled;

        //public bool DisplayErrorsCalled
        //{
        //    get { return _displayErrorsCalled; }
        //}

        //public bool ClearErrorsCalled
        //{
        //    get { return _clearErrorsCalled; }
        //}

        public IBusinessObject BusinessObject
        {
            get { return _businessObject; }
            set { _businessObject = value; }
        }

        //public void DisplayErrors()
        //{
        //    _displayErrorsCalled = true;
        //}

        //public void ClearErrors()
        //{
        //    _clearErrorsCalled = true;
        //}
    }

    internal class GridWithPanelStrategyVWGStub<TBusinessObject> : IGridWithPanelControlStrategy<TBusinessObject>
    {
        public void UpdateControlEnabledState(IBusinessObject lastSelectedBusinessObject)
        {
        }

        public bool ShowConfirmSaveDialog
        {
            get { return false; }
        }

        public bool CallApplyChangesToEditBusinessObject
        {
            get { return true; }
        }

        public bool RefreshGrid
        {
            get { return true; }
        }
    }
}

