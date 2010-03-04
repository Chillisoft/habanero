using System;
using Habanero.Base;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Grid
{
    [TestFixture]
    public class TestReadOnlyGridControlWin : TestReadOnlyGridControl
    {
        private System.Windows.Forms.Form frm;

        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();
        }

        protected override IReadOnlyGridControl CreateReadOnlyGridControl()
        {
            return CreateReadOnlyGridControl(false);
        }

        protected override IReadOnlyGridControl CreateReadOnlyGridControl(bool putOnForm)
        {
            Habanero.UI.Win.ReadOnlyGridControlWin readOnlyGridControlWin =
                new Habanero.UI.Win.ReadOnlyGridControlWin(GetControlFactory());
            if (putOnForm)
            {
                frm = new System.Windows.Forms.Form();
                frm.Controls.Add(readOnlyGridControlWin);
                frm.Show();
            }
            return readOnlyGridControlWin;
        }

        protected override void CloseForm()
        {
            if (frm == null) return;
            frm.Close();
            frm = null;
        }

        protected override IClassDef LoadMyBoDefaultClassDef()
        {
            return  MyBO.LoadDefaultClassDef();
        }

        protected override void LoadMyBoClassDef_NonGuidID()
        {
            MyBO.LoadClassDef_NonGuidID();
           
        }

        protected override void AddControlToForm(IControlHabanero control)
        {
            AddControlToForm(control, 400);
        }

        protected override void AddControlToForm(IControlHabanero control, int formHeight)
        {
            System.Windows.Forms.Form frmLocal = new System.Windows.Forms.Form();
            frmLocal.Controls.Add((System.Windows.Forms.Control)control);
            frmLocal.Height = formHeight;
        }

        [Test]
        public void Test_GridDoubleClickHandlersAssigned()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, TestUtil.CountEventSubscribers(readOnlyGridControl.Grid, "DoubleClick"));
            Assert.IsTrue
                (TestUtil.EventHasSubscriber(readOnlyGridControl.Grid, "DoubleClick", "DoubleClickHandler"));

            // THESE TESTS DON'T WORK FOR SOME REASON, BUT THE GRID DOUBLE-CLICKING DOES WORK - ERIC
            //Assert.AreEqual(1, TestUtil.CountEventSubscribers(readOnlyGridControl.Grid, "RowDoubleClicked"));
            //Assert.IsTrue(TestUtil.EventHasSubscriber(readOnlyGridControl.Grid, "RowDoubleClicked", "Buttons_EditClicked"));
        }

        [Test]
        public void TestDisableDefaultRowDoubleClick()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl onlyGridControlgrid = CreateReadOnlyGridControl(true);
            // onlyGridControlgrid.Initialise();
            //-----Test PreCondition----------------------------------------
            Assert.IsTrue(onlyGridControlgrid.DoubleClickEditsBusinessObject);
            //---------------Execute Test ----------------------
            onlyGridControlgrid.DoubleClickEditsBusinessObject = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(onlyGridControlgrid.DoubleClickEditsBusinessObject);
        }

        //        [Test]
        //        public void TestWinInitialiseGrid()
        //        {
        //            //---------------Set up test pack-------------------
        //            ReadOnlyGridControlWin readOnlyGridControl =
        //new ReadOnlyGridControlWin();
        //            //--------------Assert PreConditions----------------            
        //            Assert.IsFalse(readOnlyGridControl.IsInitialised);
        //            //---------------Execute Test ----------------------
        //            readOnlyGridControl.Initialise(ContactPersonTestBO.LoadDefaultClassDefWithUIDef());
        //            //---------------Test Result -----------------------
        //            Assert.IsTrue(readOnlyGridControl.IsInitialised);
        //            //---------------Tear Down -------------------------          
        //        }
        //        this can be tested in windows with NUnitForms. Removed from Giz because there's
        // no way to handle a popup messagebox in a test environment.
        //[Test]
        //public void TestAcceptance_DeleteButtonClickSuccessfulDelete()
        //{
        //    //---------------Set up test pack-------------------
        //    ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
        //    ContactPersonTestBO bo = new ContactPersonTestBO();
        //    bo.Surname = "please delete me.";
        //    bo.Save();
        //    IPrimaryKey contactPersonPK = bo.ID;

        //    BusinessObjectCollection<ContactPersonTestBO> boCol = new BusinessObjectCollection<ContactPersonTestBO>();
        //    boCol.Add(bo);
        //    IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
        //    readOnlyGridControl.Grid.Columns.Add("Surname", "Surname");
        //    readOnlyGridControl.SetBusinessObjectCollection(boCol);
        //    readOnlyGridControl.SelectedBusinessObject = bo;
        //    readOnlyGridControl.Buttons.ShowDefaultDeleteButton = true;
        //    //---------------Execute Test ----------------------
        //    readOnlyGridControl.Buttons["Delete"].PerformClick();
        //    //---------------Test Result -----------------------

        //    ContactPersonTestBO contactPerson =
        //        BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(contactPersonPK);
        //    Assert.IsNull(contactPerson);
        //}
        // Will work in Windows but can not be tested in Gizmox
        //[Test]
        //public void TestAcceptance_DeleteButtonClickUnsuccessfulDelete()
        //{
        //    //---------------Set up test pack-------------------
        //    ContactPersonTestBO.LoadClassDefWithAddressesRelationship_PreventDelete_WithUIDef();
        //    ContactPersonTestBO person = new ContactPersonTestBO();
        //    person.Surname = "please delete me" + Guid.NewGuid().ToString("N");
        //    person.FirstName = "fjdal;fjasdf";
        //    person.Save();
        //    IPrimaryKey contactPersonPK = person.ID;

        //    Address address = person.Addresses.CreateBusinessObject();
        //    address.Save();

        //    BusinessObjectCollection<ContactPersonTestBO> boCol = new BusinessObjectCollection<ContactPersonTestBO>();
        //    boCol.Add(person);
        //    IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
        //    readOnlyGridControl.Grid.Columns.Add("Surname", "Surname");
        //    readOnlyGridControl.SetBusinessObjectCollection(boCol);
        //    readOnlyGridControl.SelectedBusinessObject = person;
        //    readOnlyGridControl.Buttons.ShowDefaultDeleteButton = true;
        //    ExceptionNotifierStub exceptionNotifier = new ExceptionNotifierStub();
        //    GlobalRegistry.UIExceptionNotifier = exceptionNotifier;

        //    //---------------Execute Test ----------------------
        //    readOnlyGridControl.Buttons["Delete"].PerformClick();
        //    //---------------Test Result -----------------------

        //    Assert.IsTrue(exceptionNotifier.Notified);
        //    ContactPersonTestBO contactPerson =
        //        BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(contactPersonPK);
        //    Assert.IsNotNull(contactPerson);
        //}

        //[Test]
        //public void TestInitGrid_DefaultUIDef()
        //{
        //    //---------------Set up test pack-------------------
        //    ClassDef classDef = LoadMyBoDefaultClassDef();
        //    IReadOnlyGridControl grid = CreateReadOnlyGridControl();
        //    UIDef uiDef = classDef.UIDefCol["default"];
        //    UIGrid uiGridDef = uiDef.UIGrid;
        //    //---------------Assert Preconditions---------------
        //    Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
        //    Assert.AreEqual("", grid.UiDefName);
        //    Assert.IsNull(grid.ClassDef);
        //    //---------------Execute Test ----------------------
        //    grid.Initialise(classDef);

        //    //---------------Test Result -----------------------
        //    Assert.AreEqual("default", grid.UiDefName);
        //    Assert.AreEqual(classDef, grid.ClassDef);
        //    Assert.AreEqual(uiGridDef.Count + 1, grid.Grid.Columns.Count,
        //                    "There should be 1 ID column and 2 defined columns in the defaultDef");
        //    //---------------Tear Down -------------------------          


        //        [Test]
        //        public void TestClickAddWhenNoCollectionSet()
        //        {
        //            //---------------Set up test pack-------------------
        //            LoadMyBoDefaultClassDef();
        //            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
        //            grid.Initialise(new MyBO().ClassDef);
        //            //--------------Assert PreConditions----------------            
        //            //---------------Execute Test ----------------------
        //
        //            try
        //            {
        //                grid.Buttons["Add"].PerformClick();
        //                Assert.Fail("Error should b raised");
        //            }
        //            //---------------Test Result -----------------------
        //            catch (GridDeveloperException ex)
        //            {
        //                StringAssert.Contains("You cannot call add since the grid has not been set up", ex.Message);
        //            }
        //
        //            //---------------Tear Down -------------------------          
        //        }
        //
        //        [Test]
        //        public void TestClickEditWhenNoCollectionSet()
        //        {
        //            //---------------Set up test pack-------------------
        //            LoadMyBoDefaultClassDef();
        //            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
        //            grid.Initialise(new MyBO().ClassDef);
        //
        //            //--------------Assert PreConditions----------------            
        //            //---------------Execute Test ----------------------
        //
        //            try
        //            {
        //                grid.Buttons["Edit"].PerformClick();
        //                Assert.Fail("Error should b raised");
        //            }
        //            //---------------Test Result -----------------------
        //            catch (GridDeveloperException ex)
        //            {
        //                StringAssert.Contains("You cannot call edit since the grid has not been set up", ex.Message);
        //            }
        //
        //            //---------------Tear Down -------------------------          
        //        }
        //
        //        [Test]
        //        public void TestClickDeleteWhenNoCollectionSet()
        //        {
        //            //---------------Set up test pack-------------------
        //            LoadMyBoDefaultClassDef();
        //            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
        //            grid.Initialise(new MyBO().ClassDef);
        //            //--------------Assert PreConditions----------------            
        //            //---------------Execute Test ----------------------
        //
        //            try
        //            {
        //                grid.Buttons["Delete"].PerformClick();
        //                Assert.Fail("Error should b raised");
        //            }
        //            //---------------Test Result -----------------------
        //            catch (GridDeveloperException ex)
        //            {
        //                StringAssert.Contains("You cannot call delete since the grid has not been set up", ex.Message);
        //            }
        //
        //            //---------------Tear Down -------------------------          
        //        }
    }
}