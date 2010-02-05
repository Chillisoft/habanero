using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Grid
{
    [TestFixture]
    public class TestReadOnlyGridControlVWG : TestReadOnlyGridControl
    {
        public TestReadOnlyGridControlVWG()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.UI.VWG.ControlFactoryVWG();
        }

        protected override void AddControlToForm(IControlHabanero control, int formHeight)
        {
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)control);
            frm.Height = formHeight;
        }

        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }

        protected override IReadOnlyGridControl CreateReadOnlyGridControl()
        {
            return CreateReadOnlyGridControl(true);
        }

        protected override IReadOnlyGridControl CreateReadOnlyGridControl(bool putOnForm)
        {
            //ALWAYS PUT ON FORM FOR GIZ
            Habanero.UI.VWG.ReadOnlyGridControlVWG readOnlyGridControlVWG =
                new Habanero.UI.VWG.ReadOnlyGridControlVWG(GetControlFactory());
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add(readOnlyGridControlVWG);
            return readOnlyGridControlVWG;
        }

        protected override void CloseForm()
        {
        }

        protected override void AddControlToForm(IControlHabanero control)
        {
            AddControlToForm(control, 200);
        }


        [Test]
        public void TestVWGInitialiseGrid()
        {
            //---------------Set up test pack-------------------
            Habanero.UI.VWG.ReadOnlyGridControlVWG readOnlyGridControlVWG =
                new Habanero.UI.VWG.ReadOnlyGridControlVWG(GlobalUIRegistry.ControlFactory);
            //--------------Assert PreConditions----------------    
            Assert.IsFalse(readOnlyGridControlVWG.IsInitialised);
            //---------------Execute Test ----------------------
            readOnlyGridControlVWG.Initialise(MyBO.LoadClassDefWithBoolean());
            //---------------Test Result -----------------------
            Assert.IsTrue(readOnlyGridControlVWG.IsInitialised);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void Test_Acceptance_Filter_When_On_Page2_Of_Pagination()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl readOnlyGridControl = GetGridWith_4_Rows(out col, true);
            AddControlToForm(readOnlyGridControl);
            ITextBox tb = readOnlyGridControl.FilterControl.AddStringFilterTextBox("Test Prop", "TestProp");
            //Set items per page to 3 items
            readOnlyGridControl.Grid.ItemsPerPage = 3;
            //Go to page 2 (pagination page)
            readOnlyGridControl.Grid.CurrentPage = 2;

            //--------------Assert PreConditions ---------------
            Assert.AreEqual(2, readOnlyGridControl.Grid.CurrentPage);
            //---------------Execute Test ----------------------
            //enter data in filter for 1 item
            tb.Text = "b";
            readOnlyGridControl.FilterControl.ApplyFilter();
            //---------------Test Result -----------------------
            // verify that grid has moved back to page 1
            Assert.AreEqual(1, readOnlyGridControl.Grid.CurrentPage);
            //---------------Tear Down -------------------------          
        }

        [Ignore("This is no longer a requirement")]
        [Test]
        public void Test_AddClicked_RowNotAddedBeforeEditorFinished()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl readOnlyGridControl = GetGridWith_4_Rows(out col, true);
            //AddControlToForm(readOnlyGridControl);
            IButton button = readOnlyGridControl.Buttons["Add"];
            int rowCountInEditorMethod = -1;
            readOnlyGridControl.BusinessObjectEditor = new DelegatedBusinessObjectEditor((bo, uiName, postEditAction) =>
                                                                                             {
                                                                                                 rowCountInEditorMethod = readOnlyGridControl.Grid.Rows.Count;
                                                                                                 return true;
                                                                                             });
            //-------------Assert Preconditions -------------
            Assert.IsNotNull(button);
            Assert.AreEqual(4, readOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            button.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(4, rowCountInEditorMethod);
        }

        [Ignore("This is no longer a requirement")]
        [Test]
        public void Test_AddClicked_RowNotAddedBeforePostEditActionCalled()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl readOnlyGridControl = GetGridWith_4_Rows(out col, true);
            //AddControlToForm(readOnlyGridControl);
            IButton button = readOnlyGridControl.Buttons["Add"];
            readOnlyGridControl.BusinessObjectEditor = new DelegatedBusinessObjectEditor<MyBO>(
                (obj, uiDefName, postEditAction) => true);
            //-------------Assert Preconditions -------------
            Assert.IsNotNull(button);
            Assert.AreEqual(4, readOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            button.PerformClick();
            //---------------Test Result -----------------------
            Assert.AreEqual(4, readOnlyGridControl.Grid.Rows.Count);
        }

        [Ignore("This is no longer a requirement")]
        [Test]
        public void Test_AddClicked_RowAddedAfterPostEditActionCalled()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl readOnlyGridControl = GetGridWith_4_Rows(out col, true);
            //AddControlToForm(readOnlyGridControl);
            IButton button = readOnlyGridControl.Buttons["Add"];
            MyBO myNewBo = null;
            PostObjectEditDelegate editorPostEditAction = null;
            readOnlyGridControl.BusinessObjectEditor = new DelegatedBusinessObjectEditor<MyBO>(
                delegate(MyBO obj, string uiDefName, PostObjectEditDelegate postEditAction)
                    {
                        myNewBo = obj;
                        editorPostEditAction = postEditAction;
                        return true;
                    });
            button.PerformClick();
            //-------------Assert Preconditions -------------
            Assert.IsNotNull(editorPostEditAction);
            Assert.IsNotNull(myNewBo);
            Assert.AreEqual(4, readOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            editorPostEditAction(myNewBo, true);
            //---------------Test Result -----------------------
            Assert.AreEqual(5, readOnlyGridControl.Grid.Rows.Count);
        }

        [Ignore("This does not work in VWG")] //TODO Brett 04 Jan 2010: Ignored Test - This does not work in VWG
        [Test]
        public override void Test_AllowUsersToAddBo_WhenFalse_ShouldHideButton()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl(false);
            IButton addButton = readOnlyGridControl.Buttons["Add"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(readOnlyGridControl.AllowUsersToAddBO);
            Assert.IsTrue(addButton.Visible);
            //---------------Execute Test ----------------------
            readOnlyGridControl.AllowUsersToAddBO = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(readOnlyGridControl.AllowUsersToAddBO);
            Assert.IsFalse(addButton.Visible);
        }
        [Ignore("This does not work in VWG")] //TODO Brett 04 Jan 2010: Ignored Test - This does not work in VWG
        [Test]
        public override void Test_AllowUsersToAddBo_WhenTrue_ShouldHideButton()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl(false);
            IButton addButton = readOnlyGridControl.Buttons["Add"];
            readOnlyGridControl.AllowUsersToAddBO = false;
            //---------------Assert Precondition----------------
            Assert.IsFalse(readOnlyGridControl.AllowUsersToAddBO);
            Assert.IsFalse(addButton.Visible);
            //---------------Execute Test ----------------------
            readOnlyGridControl.AllowUsersToAddBO = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(readOnlyGridControl.AllowUsersToAddBO);
            Assert.IsTrue(addButton.Visible);
        }
        [Ignore("This does not work in VWG")] //TODO Brett 04 Jan 2010: Ignored Test - This does not work in VWG
        [Test]
        public override void Test_AllowUsersToEditBo_WhenFalse_ShouldHideButton()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl(false);
            IButton editButton = readOnlyGridControl.Buttons["Edit"];
            //---------------Assert Precondition----------------
            Assert.IsTrue(readOnlyGridControl.AllowUsersToEditBO);
            Assert.IsTrue(editButton.Visible);
            //---------------Execute Test ----------------------
            readOnlyGridControl.AllowUsersToEditBO = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(readOnlyGridControl.AllowUsersToEditBO);
            Assert.IsFalse(editButton.Visible);
        }
        [Ignore("This does not work in VWG")] //TODO Brett 04 Jan 2010: Ignored Test - This does not work in VWG
        [Test]
        public override void Test_AllowUsersToEditBo_WhenTrue_ShouldHideButton()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl(false);
            IButton editButton = readOnlyGridControl.Buttons["Edit"];
            readOnlyGridControl.AllowUsersToEditBO = false;
            //---------------Assert Precondition----------------
            Assert.IsFalse(readOnlyGridControl.AllowUsersToEditBO);
            Assert.IsFalse(editButton.Visible);
            //---------------Execute Test ----------------------
            readOnlyGridControl.AllowUsersToEditBO = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(readOnlyGridControl.AllowUsersToEditBO);
            Assert.IsTrue(editButton.Visible);
        }

        [Ignore("This does not work in VWG")] //TODO Brett 04 Jan 2010: Ignored Test - This does not work in VWG
        [Test]
        public override void Test_AllowUsersToDeleteBo_WhenFalse_ShouldHideButton()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl(false);
            IButton deleteBtn = readOnlyGridControl.Buttons["Delete"];
            readOnlyGridControl.AllowUsersToDeleteBO = true;
            //---------------Assert Precondition----------------
            Assert.IsTrue(readOnlyGridControl.AllowUsersToDeleteBO);
            Assert.IsTrue(deleteBtn.Visible);
            //---------------Execute Test ----------------------
            readOnlyGridControl.AllowUsersToDeleteBO = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(readOnlyGridControl.AllowUsersToDeleteBO);
            Assert.IsFalse(deleteBtn.Visible);
        }
        [Ignore("This does not work in VWG")] //TODO Brett 04 Jan 2010: Ignored Test - This does not work in VWG
        [Test]
        public override void Test_AllowUsersToDeleteBo_WhenTrue_ShouldHideButton()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl(false);
            IButton button = readOnlyGridControl.Buttons["Delete"];

            //---------------Assert Precondition----------------
            Assert.IsFalse(readOnlyGridControl.AllowUsersToDeleteBO);
            Assert.IsFalse(button.Visible);
            //---------------Execute Test ----------------------
            readOnlyGridControl.AllowUsersToDeleteBO = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(readOnlyGridControl.AllowUsersToDeleteBO);
            Assert.IsTrue(button.Visible);
        }

        protected override IClassDef LoadMyBoDefaultClassDef()
        {
            return MyBO.LoadDefaultClassDefVWG();
                
        }

        protected override void LoadMyBoClassDef_NonGuidID()
        {
             MyBO.LoadClassDefVWG_NonGuidID();
                
        }
    }
}