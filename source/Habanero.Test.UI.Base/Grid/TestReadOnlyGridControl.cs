using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.Test.BO;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.UI.Grid
{
    public abstract class TestReadonlyGridControl : TestUsingDatabase
    {
        //TODO: Move dataView Logic out of GridBase
        //TODO: Tests that if init not called throws sensible errors
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            base.SetupDBConnection();
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract void AddControlToForm(IControlChilli control);
        protected abstract void AddControlToForm(IControlChilli control, int formHeight);
        protected abstract IControlFactory GetControlFactory();
        protected abstract IReadOnlyGridControl CreateReadOnlyGridControl();

        //[TestFixture]
        //public class TestreadOnlyGridControlWin : TestReadonlyGridControl
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryWin();
        //    }
        //    protected override IReadOnlyGridControl CreateReadOnlyGridControl()
        //    {
        //        ReadOnlyGridControlWin readOnlyGridControlWin = new ReadOnlyGridControlWin();
        //        System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //        frm.Controls.Add(readOnlyGridControlWin);
        //        return readOnlyGridControlWin;
        //    }
//        [Test]
//        public void TestWinInitialiseGrid()
//        {
//            //---------------Set up test pack-------------------
//            ReadOnlyGridControlWin readOnlyGridControlGiz =
//new ReadOnlyGridControlGiz();
//            //--------------Assert PreConditions----------------            
//            Assert.IsFalse(readOnlyGridControlGiz.IsInitialised);
//            //---------------Execute Test ----------------------
//            readOnlyGridControlGiz.Initialise(ContactPersonTestBO.LoadDefaultClassDefWithUIDef());
//            //---------------Test Result -----------------------
//            Assert.IsTrue(readOnlyGridControlGiz.IsInitialised);
//            //---------------Tear Down -------------------------          
//        }
        //}
        [TestFixture]
        public class TestreadOnlyGridControlGiz : TestReadonlyGridControl
        {
            protected override void AddControlToForm(IControlChilli control, int formHeight)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control) control);
                frm.Height = formHeight;
            }

            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }

            protected override IReadOnlyGridControl CreateReadOnlyGridControl()
            {
                ReadOnlyGridControlGiz readOnlyGridControlGiz =
                    new ReadOnlyGridControlGiz();
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add(readOnlyGridControlGiz);
                return readOnlyGridControlGiz;
            }

            [Test]
            public void TestGizInitialiseGrid()
            {
                //---------------Set up test pack-------------------
                ReadOnlyGridControlGiz readOnlyGridControlGiz =
                    new ReadOnlyGridControlGiz();
                //--------------Assert PreConditions----------------    
                Assert.IsFalse(readOnlyGridControlGiz.IsInitialised);
                //---------------Execute Test ----------------------
                readOnlyGridControlGiz.Initialise(MyBO.LoadClassDefWithBoolean());
                //---------------Test Result -----------------------
                Assert.IsTrue(readOnlyGridControlGiz.IsInitialised);
                //---------------Tear Down -------------------------          
            }

            protected override void AddControlToForm(IControlChilli control)
            {
                AddControlToForm(control, 200);
            }
        }


        [Test]
        public void TestAcceptance_FilterGrid()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl readOnlyGridControl = GetGridWith_4_Rows(out col);
            AddControlToForm(readOnlyGridControl);
            ITextBox tb = readOnlyGridControl.FilterControl.AddStringFilterTextBox("Test Prop", "TestProp");
            //--------------Assert PreConditions
            Assert.AreEqual(4, readOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            //enter data in filter for 1 item
            tb.Text = "b";
            readOnlyGridControl.FilterControl.ApplyFilter();
            //---------------Assert Result -----------------------
            // verify that grid has only 1 item in it  
            Assert.AreEqual(1, readOnlyGridControl.Grid.Rows.Count);
        }

        [Test]
        public void Test_Acceptance_Filter_When_On_Page2_Of_Pagination()
        {
            //---------------Set up test pack-------------------
            //Get Grid with 4 items
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl readOnlyGridControl = GetGridWith_4_Rows(out col);
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

        [Test]
        public void TestAcceptance_DeleteButtonClickSuccessfulDelete()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            ContactPersonTestBO bo = new ContactPersonTestBO();
            bo.Surname = "please delete me.";
            bo.Save();
            BOPrimaryKey contactPersonPK = bo.ID;

            BusinessObjectCollection<ContactPersonTestBO> boCol = new BusinessObjectCollection<ContactPersonTestBO>();
            boCol.Add(bo);
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            readOnlyGridControl.Grid.Columns.Add("Surname", "Surname");
            readOnlyGridControl.SetBusinessObjectCollection(boCol);
            readOnlyGridControl.SelectedBusinessObject = bo;
            readOnlyGridControl.Buttons.ShowDefaultDeleteButton = true;
            //---------------Execute Test ----------------------
            readOnlyGridControl.Buttons["Delete"].PerformClick();
            //---------------Test Result -----------------------

            ContactPersonTestBO contactPerson =
                BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(contactPersonPK);
            Assert.IsNull(contactPerson);
        }

        [Test]
        public void TestAcceptance_DeleteButtonClickUnsuccessfulDelete()
        {
            //---------------Set up test pack-------------------
            ContactPersonTestBO.LoadClassDefWithAddressesRelationship_PreventDelete_WithUIDef();
            ContactPersonTestBO person = new ContactPersonTestBO();
            person.Surname = "please delete me" + Guid.NewGuid().ToString("N");
            person.FirstName = "fjdal;fjasdf";
            person.Save();
            BOPrimaryKey contactPersonPK = person.ID;

            Address address = person.Addresses.CreateBusinessObject();
            address.Save();

            BusinessObjectCollection<ContactPersonTestBO> boCol = new BusinessObjectCollection<ContactPersonTestBO>();
            boCol.Add(person);
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            readOnlyGridControl.Grid.Columns.Add("Surname", "Surname");
            readOnlyGridControl.SetBusinessObjectCollection(boCol);
            readOnlyGridControl.SelectedBusinessObject = person;
            readOnlyGridControl.Buttons.ShowDefaultDeleteButton = true;
            ExceptionNotifierStub exceptionNotifier = new ExceptionNotifierStub();
            GlobalRegistry.UIExceptionNotifier = exceptionNotifier;

            //---------------Execute Test ----------------------
            readOnlyGridControl.Buttons["Delete"].PerformClick();
            //---------------Test Result -----------------------

            Assert.IsTrue(exceptionNotifier.Notified);
            ContactPersonTestBO contactPerson =
                BOLoader.Instance.GetBusinessObjectByID<ContactPersonTestBO>(contactPersonPK);
            Assert.IsNotNull(contactPerson);
        }

        //TODO: Date searchby
        [Test]
        public void TestAcceptance_SearchGridSearchesTheGrid()
        {
            //---------------Set up test pack-------------------
            //Clear all contact people from the DB
            ContactPerson.DeleteAllContactPeople();
            ClassDef classDef = ContactPersonTestBO.LoadDefaultClassDefWithUIDef();
            //Create data in the database with the 5 contact people two with Search in surname
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            CreateContactPersonInDB_With_SSSSS_InSurname();
            //Create grid setup for search
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            ITextBox txtbox = readOnlyGridControl.FilterControl.AddStringFilterTextBox("Surname", "Surname");
            readOnlyGridControl.Initialise(classDef);
            readOnlyGridControl.FilterMode = FilterModes.Search;
            //--------------Assert PreConditions----------------            
            //No items in the grid
            Assert.AreEqual(0, readOnlyGridControl.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            //set data in grid to a value that should return 2 people
            string filterByValue = "SSSSS";
            txtbox.Text = filterByValue;
            //grid.filtercontrols.searchbutton.click
            readOnlyGridControl.FilterControl.ApplyFilter();
            //---------------Test Result -----------------------
            StringAssert.Contains(filterByValue,readOnlyGridControl.FilterControl.GetFilterClause().GetFilterClauseString());
            //verify that there are 2 people in the grid.
            Assert.AreEqual(2, readOnlyGridControl.Grid.Rows.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_ReadOnlyGridDefaultsToFilter()
        {
            //---------------Set up test pack-------------------

            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            //---------------Test Result -----------------------
            Assert.AreEqual(FilterModes.Filter, readOnlyGridControl.FilterMode);
            Assert.AreEqual(FilterModes.Filter, readOnlyGridControl.FilterControl.FilterMode);
            //---------------Tear Down -------------------------          
        }
        [Test]
        public void Test_ReadOnlyGrid_SetToSearchSetsToSearchMode()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(FilterModes.Filter, readOnlyGridControl.FilterMode);
            Assert.AreEqual(FilterModes.Filter, readOnlyGridControl.FilterControl.FilterMode);
            //---------------Execute Test ----------------------
            readOnlyGridControl.FilterMode = FilterModes.Search;
            //---------------Test Result -----------------------
            Assert.AreEqual(FilterModes.Search, readOnlyGridControl.FilterMode);
            Assert.AreEqual(FilterModes.Search, readOnlyGridControl.FilterControl.FilterMode);
            //---------------Tear Down -------------------------          
        }
        
        private static ContactPersonTestBO CreateContactPersonInDB()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = Guid.NewGuid().ToString("N");
            contactPersonTestBO.Save();
            return contactPersonTestBO;
        }
        private static ContactPersonTestBO CreateContactPersonInDB_With_SSSSS_InSurname()
        {
            ContactPersonTestBO contactPersonTestBO = new ContactPersonTestBO();
            contactPersonTestBO.Surname = Guid.NewGuid().ToString("N") + "SSSSS";
            contactPersonTestBO.Save();
            return contactPersonTestBO;
        }

        [Test]
        public void TestCreatereadOnlyGridControl()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlChilli grid = CreateReadOnlyGridControl();

            ////---------------Test Result -----------------------
            Assert.IsNotNull(grid);
            Assert.IsTrue(grid is IReadOnlyGridControl);
            IReadOnlyGridControl readOnlyGrid = (IReadOnlyGridControl) grid;
            Assert.IsNotNull(readOnlyGrid.Grid);
            Assert.IsNotNull(readOnlyGrid.Buttons);
            Assert.AreEqual("", readOnlyGrid.UiDefName);
            Assert.IsNull(readOnlyGrid.ClassDef);
            Assert.IsTrue(readOnlyGrid.Grid.ReadOnly);
        }

        [Test]
        public void TestreadOnlyGridControl_AddColumn_AsString()
        {
            //---------------Set up test pack-------------------
            //MyBO.LoadDefaultClassDef();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            IReadOnlyGrid readOnlyGrid = readOnlyGridControl.Grid;

            //---------------Execute Test ----------------------
            readOnlyGrid.Columns.Add("TestProp", "TestProp");
            ////---------------Test Result -----------------------
            Assert.AreEqual(1, readOnlyGrid.Columns.Count);
        }

        [Test, Ignore("Cant get this work not resizing here but doing it on the form.")]
        public void Test_MakeButtons_Not_Visible()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            IReadOnlyGrid readOnlyGrid = readOnlyGridControl.Grid;
            int frmHeight = 100;
            AddControlToForm(readOnlyGridControl, frmHeight);

            //---------------Verify PreConditions --------------
            Assert.AreEqual(67, readOnlyGrid.Height);
            //---------------Execute Test ----------------------
            readOnlyGridControl.Buttons.Visible = false;
            //---------------Verify Resulst --------------------
            Assert.AreEqual(95, readOnlyGrid.Height);
        }


        [Test]
        public void TestInitGrid_DefaultUIDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            UIDef uiDef = classDef.UIDefCol["default"];
            UIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
            Assert.AreEqual("", grid.UiDefName);
            Assert.IsNull(grid.ClassDef);
            //---------------Execute Test ----------------------
            grid.Initialise(classDef);

            //---------------Test Result -----------------------
            Assert.AreEqual("default", grid.UiDefName);
            Assert.AreEqual(classDef, grid.ClassDef);
            Assert.AreEqual(uiGridDef.Count + 1, grid.Grid.Columns.Count,
                            "There should be 1 ID column and 2 defined columns in the defaultDef");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitGrid_DefaultUIDef_VerifyColumnsSetupCorrectly()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            UIDef uiDef = classDef.UIDefCol["default"];
            UIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "2 defined columns in the defaultDef");
            UIGridColumn columnDef1 = uiGridDef[0];
            Assert.AreEqual("TestProp", columnDef1.PropertyName);
            UIGridColumn columnDef2 = uiGridDef[1];
            Assert.AreEqual("TestProp2", columnDef2.PropertyName);
            //---------------Execute Test ----------------------
            grid.Initialise(classDef);

            //---------------Test Result -----------------------
            IDataGridViewColumn idColumn = grid.Grid.Columns[0];
            AssertVerifyIDFieldSetUpCorrectly(idColumn);

            IDataGridViewColumn dataColumn1 = grid.Grid.Columns[1];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef1, dataColumn1);

            IDataGridViewColumn dataColumn2 = grid.Grid.Columns[2];
            AssertThatDataColumnSetupCorrectly(classDef, columnDef2, dataColumn2);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitGrid_WithNonDefaultUIDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            string alternateUIDefName = "Alternate";
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            UIDef uiDef = classDef.UIDefCol[alternateUIDefName];
            UIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(1, uiGridDef.Count, "1 defined column in the alternateUIDef");
            //---------------Execute Test ----------------------
            grid.Initialise(classDef, alternateUIDefName);

            //---------------Test Result -----------------------
            Assert.AreEqual(alternateUIDefName, grid.UiDefName);
            Assert.AreEqual(classDef, grid.ClassDef);
            Assert.AreEqual(uiGridDef.Count + 1, grid.Grid.Columns.Count,
                            "There should be 1 ID column and 1 defined column in the alternateUIDef");
            //---------------Tear Down -------------------------          
        }

        //Note: this can be changed to allow the grid to reinitialise everything if initialise called a second time.
        // this may be necessary e.g. to use the same grid but swap out uidefs etc.
        public void TestInitGrid_Twice_Fail()
        {
            //---------------Set up test pack-------------------
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            ClassDef classDef = LoadMyBoDefaultClassDef();
            //---------------Assert Preconditions---------------
            //---------------Execute Test ----------------------
            grid.Initialise(classDef);
            try
            {
                grid.Initialise(classDef);
                Assert.Fail("You should not be able to call initialise twice on a grid");
            }
                //---------------Test Result -----------------------
            catch (GridBaseSetUpException ex)
            {
                StringAssert.Contains("You cannot initialise the grid more than once", ex.Message);
            }
        }

        public void TestInitGrid_AddColumnsManually_And_Initialise()
        {
            //---------------Set up test pack-------------------

            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            ClassDef classDef = LoadMyBoDefaultClassDef();
            UIDef uiDef = classDef.UIDefCol["default"];
            UIGrid uiGridDef = uiDef.UIGrid;
            //---------------Assert Preconditions---------------
            Assert.AreEqual(2, uiGridDef.Count, "Precondition: 1 defined column in the default def");
            //---------------Execute Test ----------------------
            grid.Grid.Columns.Add("ManualColumn", "mm");
            grid.Initialise(classDef);

            //---------------Test Result -----------------------
            Assert.AreEqual(uiGridDef.Count + 1, grid.Grid.Columns.Count,
                            "There should be 1 ID column and 2 defined columns in the defaultDef");
        }

        [Test]
        public void TestInitGrid_WithInvalidUIDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();

            //---------------Execute Test ----------------------
            try
            {
                grid.Initialise(classDef, "NonExistantUIDef");
                Assert.Fail("Should raise an error if the class def does not the UIDef");
                //---------------Test Result -----------------------
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(" does not contain a definition for UIDef ", ex.Message);
            }
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitGrid_With_NoGridDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            //---------------Execute Test ----------------------
            try
            {
                grid.Initialise(classDef, "AlternateNoGrid");
                Assert.Fail("Should raise an error if the class def does not the GridDef");
                //---------------Test Result -----------------------
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(
                    " does not contain a grid definition for UIDef AlternateNoGrid for the class def ", ex.Message);
            }
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_AddInvalidColumn()
        {
            //This cannot be enforced since it is the grids underlying behaviour
        }

        [Test]
        public void TestSetCollection_IncorrectClassDef()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control) readOnlyGridControl);

            //---------------Execute Test ----------------------
            readOnlyGridControl.Initialise(Sample.CreateClassDefGiz());
            try
            {
                readOnlyGridControl.SetBusinessObjectCollection(col);
                Assert.Fail(
                    "You cannot call set collection for a collection that has a different class def than is initialised");
                ////---------------Test Result -----------------------
            }
            catch (ArgumentException ex)
            {
                StringAssert.Contains(
                    "You cannot call set collection for a collection that has a different class def than is initialised",
                    ex.Message);
            }
        }

        [Test]
        public void TestSetCollection_InitialisesGridIfNotPreviouslyInitialised()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();

            //---------------Execute Test ----------------------
            readOnlyGridControl.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.AreEqual("default", readOnlyGridControl.UiDefName);
            Assert.AreEqual(col.ClassDef, readOnlyGridControl.ClassDef);
        }

        [Test]
        public void TestSetCollection_NotInitialiseGrid_IfPreviouslyInitialised()
        {
            //Verify that setting the collection for a grid that is already initialised
            //does not cause it to be reinitialised.
            //---------------Set up test pack-------------------
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            string alternateUIDefName = "Alternate";
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();

            grid.Initialise(classDef, alternateUIDefName);
            //---------------Execute Test ----------------------
            grid.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.AreEqual(alternateUIDefName, grid.UiDefName);
        }

        [Test]
        public void TestSetCollection_NumberOfGridRows_Correct()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();

            AddControlToForm(readOnlyGridControl);
            IReadOnlyGrid readOnlyGrid = readOnlyGridControl.Grid;

            readOnlyGrid.Columns.Add("TestProp", "TestProp");
            //---------------Execute Test ----------------------
            readOnlyGridControl.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.AreEqual(4, readOnlyGrid.Rows.Count);
        }

        [Test]
        public void TestSetCollection_DefaultEditorsSetUp()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();

            AddControlToForm(readOnlyGridControl);

            readOnlyGridControl.Grid.Columns.Add("TestProp", "TestProp");
            //---------------Assert Preconditions ----------------------
            Assert.IsNull(readOnlyGridControl.BusinessObjectEditor);
            Assert.IsNull(readOnlyGridControl.BusinessObjectCreator);
            Assert.IsNull(readOnlyGridControl.BusinessObjectDeletor);
            //---------------Execute Test ----------------------
            readOnlyGridControl.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.IsTrue(readOnlyGridControl.BusinessObjectEditor is DefaultBOEditor);
            Assert.IsTrue(readOnlyGridControl.BusinessObjectCreator is DefaultBOCreator);
            Assert.IsTrue(readOnlyGridControl.BusinessObjectDeletor is DefaultBODeletor);
        }

        [Test]
        public void TestSetCollection_NonDefaultEditorsNotOverridden()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();

            AddControlToForm(readOnlyGridControl);

            readOnlyGridControl.Grid.Columns.Add("TestProp", "TestProp");
            readOnlyGridControl.BusinessObjectEditor = new ObjectEditorStub();
            readOnlyGridControl.BusinessObjectCreator = new ObjectCreatorStub();
            readOnlyGridControl.BusinessObjectDeletor = new ObjectDeletorStub();
            //---------------Assert Preconditions ----------------------
            Assert.IsTrue(readOnlyGridControl.BusinessObjectEditor is ObjectEditorStub);
            Assert.IsTrue(readOnlyGridControl.BusinessObjectCreator is ObjectCreatorStub);
            Assert.IsTrue(readOnlyGridControl.BusinessObjectDeletor is ObjectDeletorStub);
            //---------------Execute Test ----------------------
            readOnlyGridControl.SetBusinessObjectCollection(col);
            ////---------------Test Result -----------------------
            Assert.IsTrue(readOnlyGridControl.BusinessObjectEditor is ObjectEditorStub);
            Assert.IsTrue(readOnlyGridControl.BusinessObjectCreator is ObjectCreatorStub);
            Assert.IsTrue(readOnlyGridControl.BusinessObjectDeletor is ObjectDeletorStub);
        }

        [Test]
        public void TestSetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl readOnlyGridControl = GetGridWith_4_Rows(out col);
            BusinessObject bo = col[0];
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control) readOnlyGridControl);
            //---------------Execute Test ----------------------
            readOnlyGridControl.SelectedBusinessObject = bo;

            //---------------Test Result -----------------------
            Assert.AreEqual(bo, readOnlyGridControl.SelectedBusinessObject);
        }

        [Test]
        public void TestSetSelectedBusinessObject_ToNull()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl grid = GetGridWith_4_Rows(out col);
            BusinessObject bo = col[0];

            //---------------Execute Test ----------------------
            grid.SelectedBusinessObject = bo;
            grid.SelectedBusinessObject = null;

            //---------------Test Result -----------------------
            Assert.IsNull(grid.SelectedBusinessObject);
            Assert.IsNull(grid.Grid.CurrentRow);
        }

        [Test]
        public void TestInitialisingObjectCreator()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            SetupGridColumnsForMyBo(readOnlyGridControl.Grid);

            //---------------Execute Test ----------------------
            readOnlyGridControl.SetBusinessObjectCollection(col);

            //---------------Test Result -----------------------

            Assert.IsNotNull(readOnlyGridControl.BusinessObjectCreator);
            Assert.IsTrue(readOnlyGridControl.BusinessObjectCreator is DefaultBOCreator);
            Assert.IsTrue(
                ((DefaultBOCreator) readOnlyGridControl.BusinessObjectCreator).CreateBusinessObject() is MyBO);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestInitialisingObjectEditor()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            SetupGridColumnsForMyBo(readOnlyGridControl.Grid);

            //---------------Execute Test ----------------------
            readOnlyGridControl.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------

            Assert.IsNotNull(readOnlyGridControl.BusinessObjectEditor);
            Assert.IsTrue(readOnlyGridControl.BusinessObjectEditor is DefaultBOEditor);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitialisingObjectDeletor()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            SetupGridColumnsForMyBo(readOnlyGridControl.Grid);

            //---------------Execute Test ----------------------
            readOnlyGridControl.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------

            Assert.IsNotNull(readOnlyGridControl.BusinessObjectDeletor);
            Assert.IsTrue(readOnlyGridControl.BusinessObjectDeletor is DefaultBODeletor);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_EditButtonClick_NoSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl grid = GetGridWith_4_Rows(out col);
            grid.SelectedBusinessObject = null;
            ObjectEditorStub objectEditor = new ObjectEditorStub();
            grid.BusinessObjectEditor = objectEditor;

            //---------------Execute Test ----------------------
            grid.Buttons["Edit"].PerformClick();
            //---------------Test Result -----------------------

            Assert.IsFalse(objectEditor.HasBeenCalled);
        }

        [Test]
        public void TestEditButtonClick_CallsObjectEditor()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl grid = GetGridWith_4_Rows(out col);
            grid.SelectedBusinessObject = col[2];
            ObjectEditorStub objectEditor = new ObjectEditorStub();
            grid.BusinessObjectEditor = objectEditor;

            //---------------Asserting Preconditions------------
            Assert.IsFalse(objectEditor.HasBeenCalled);

            //---------------Execute Test ----------------------
            grid.Buttons["Edit"].PerformClick();

            //---------------Test Result -----------------------
            Assert.IsTrue(objectEditor.HasBeenCalled);
            Assert.AreSame(col[2], objectEditor.Bo);
            Assert.AreSame("default", objectEditor.DefName);
        }

        private ClassDef LoadMyBoDefaultClassDef()
        {
            ClassDef classDef;
            if (GetControlFactory() is ControlFactoryGizmox)
            {
                classDef = MyBO.LoadDefaultClassDefGizmox();
            }
            else
            {
                classDef = MyBO.LoadDefaultClassDef();
            }
            return classDef;
        }

        [Test]
        public void TestEditButtonClickUsingAlternateUIDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            string alternateUIDefName = "Alternate";
            BusinessObjectCollection<MyBO> col;
            col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            readOnlyGridControl.Initialise(classDef, alternateUIDefName);

            readOnlyGridControl.SetBusinessObjectCollection(col);
            readOnlyGridControl.SelectedBusinessObject = col[2];
            ObjectEditorStub objectEditor = new ObjectEditorStub();
            readOnlyGridControl.BusinessObjectEditor = objectEditor;

            //---------------Execute Test ----------------------
            readOnlyGridControl.Buttons["Edit"].PerformClick();

            //---------------Test Result -----------------------
            Assert.AreSame(alternateUIDefName, objectEditor.DefName);
        }

        [Test]
        public void TestAddButtonClickUsingAlternateUIDef()
        {
            //---------------Set up test pack-------------------
            ClassDef classDef = LoadMyBoDefaultClassDef();
            string alternateUIDefName = "Alternate";
            BusinessObjectCollection<MyBO> col;
            col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            readOnlyGridControl.Initialise(classDef, alternateUIDefName);

            readOnlyGridControl.SetBusinessObjectCollection(col);
            readOnlyGridControl.SelectedBusinessObject = col[2];
            ObjectEditorStub objectEditor = new ObjectEditorStub();
            readOnlyGridControl.BusinessObjectEditor = objectEditor;

            //---------------Execute Test ----------------------
            readOnlyGridControl.Buttons["Add"].PerformClick();

            //---------------Test Result -----------------------
            Assert.AreSame(alternateUIDefName, objectEditor.DefName);
        }


        [Test]
        public void TestAddButtonClick_CallsObjectEditorAndCreator()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl grid = GetGridWith_4_Rows(out col);
            ObjectEditorStub objectEditor = new ObjectEditorStub();
            grid.BusinessObjectEditor = objectEditor;
            ObjectCreatorStub objectCreator = new ObjectCreatorStub();
            grid.BusinessObjectCreator = objectCreator;
            //---------------Asserting Preconditions------------
            Assert.IsFalse(objectCreator.HasBeenCalled);
            Assert.IsFalse(objectEditor.HasBeenCalled);

            //---------------Execute Test ----------------------
            grid.Buttons["Add"].PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(objectCreator.HasBeenCalled);
            Assert.IsTrue(objectEditor.HasBeenCalled);
            Assert.AreSame(objectCreator.MyBO, objectEditor.Bo);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDeleteButton_CallsObjectDeletor()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl grid = GetGridWith_4_Rows(out col);
            grid.Buttons.ShowDefaultDeleteButton = true;
            grid.SelectedBusinessObject = col[2];
            ObjectDeletorStub objectDeletor = new ObjectDeletorStub();
            grid.BusinessObjectDeletor = objectDeletor;
            //---------------Execute Test ----------------------
            grid.Buttons["Delete"].PerformClick();
            //---------------Test Result -----------------------

            Assert.IsTrue(objectDeletor.HasBeenCalled);
            Assert.AreSame(col[2], objectDeletor.Bo);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestDeleteButtonWithNothingSelected_DoesNothing()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridControl grid = GetGridWith_4_Rows(out col);
            grid.Buttons.ShowDefaultDeleteButton = true;
            grid.SelectedBusinessObject = null;
            ObjectDeletorStub objectDeletor = new ObjectDeletorStub();
            grid.BusinessObjectDeletor = objectDeletor;
            //---------------Execute Test ----------------------
            grid.Buttons["Delete"].PerformClick();
            //---------------Test Result -----------------------

            Assert.IsFalse(objectDeletor.HasBeenCalled);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestClickAddWhenNoCollectionSet()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            grid.Initialise(new MyBO().ClassDef);
            //--------------Assert PreConditions----------------            
            //---------------Execute Test ----------------------

            try
            {
                grid.Buttons["Add"].PerformClick();
                Assert.Fail("Error should b raised");
            }
                //---------------Test Result -----------------------
            catch (GridDeveloperException ex)
            {
                StringAssert.Contains("You cannot call add since the grid has not been set up", ex.Message);
            }

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestClickEditWhenNoCollectionSet()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            grid.Initialise(new MyBO().ClassDef);

            //--------------Assert PreConditions----------------            
            //---------------Execute Test ----------------------

            try
            {
                grid.Buttons["Edit"].PerformClick();
                Assert.Fail("Error should b raised");
            }
                //---------------Test Result -----------------------
            catch (GridDeveloperException ex)
            {
                StringAssert.Contains("You cannot call edit since the grid has not been set up", ex.Message);
            }

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestClickDeleteWhenNoCollectionSet()
        {
            //---------------Set up test pack-------------------
            LoadMyBoDefaultClassDef();
            IReadOnlyGridControl grid = CreateReadOnlyGridControl();
            grid.Initialise(new MyBO().ClassDef);
            //--------------Assert PreConditions----------------            
            //---------------Execute Test ----------------------

            try
            {
                grid.Buttons["Delete"].PerformClick();
                Assert.Fail("Error should b raised");
            }
                //---------------Test Result -----------------------
            catch (GridDeveloperException ex)
            {
                StringAssert.Contains("You cannot call delete since the grid has not been set up", ex.Message);
            }

            //---------------Tear Down -------------------------          
        }

        #region stubs

        public class ExceptionNotifierStub : IExceptionNotifier
        {
            private bool _notified;

            public void Notify(Exception ex, string furtherMessage, string title)
            {
                _notified = true;
            }

            public bool Notified
            {
                get { return _notified; }
            }
        }

        internal class ObjectCreatorStub : IBusinessObjectCreator
        {
            private bool _hasBeenCalled;
            private MyBO _myBO;

            /// <summary>
            /// Just creates the object, without editing or saving it.
            /// </summary>
            /// <returns></returns>
            public IBusinessObject CreateBusinessObject()
            {
                _myBO = new MyBO();
                _hasBeenCalled = true;
                return _myBO;
            }

            public bool HasBeenCalled
            {
                get { return _hasBeenCalled; }
            }


            public MyBO MyBO
            {
                get { return _myBO; }
            }
        }

        internal class ObjectEditorStub : IBusinessObjectEditor
        {
            private IBusinessObject _bo;
            private string _defName;
            private bool _hasBeenCalled;

            /// <summary>
            /// Edits the given object
            /// </summary>
            /// <param name="obj">The object to edit</param>
            /// <param name="uiDefName">The name of the set of ui definitions
            /// used to design the edit form. Setting this to an empty string
            /// will use a ui definition with no name attribute specified.</param>
            /// <returns>Returs true if edited successfully of false if the edits
            /// were cancelled</returns>
            public bool EditObject(IBusinessObject obj, string uiDefName)
            {
                _bo = obj;
                _defName = uiDefName;
                _hasBeenCalled = true;
                return true;
            }

            /// <summary>
            /// Edits the given object
            /// </summary>
            /// <param name="obj">The object to edit</param>
            /// <param name="uiDefName">The name of the set of ui definitions
            /// used to design the edit form. Setting this to an empty string
            /// will use a ui definition with no name attribute specified.</param>
            /// <returns>Returs true if edited successfully of false if the edits
            /// were cancelled</returns>
            /// <param name="postEditAction">The delete to be executeActionOn After The edit is saved.
            /// will be the object that the method is called on</param>
            public bool EditObject(IBusinessObject obj, string uiDefName, PostObjectPersistingDelegate postEditAction)
            {
                return EditObject(obj, uiDefName);
            }


            public IBusinessObject Bo
            {
                get { return _bo; }
            }

            public string DefName
            {
                get { return _defName; }
            }

            public bool HasBeenCalled
            {
                get { return _hasBeenCalled; }
            }
        }

        public class ObjectDeletorStub : IBusinessObjectDeletor
        {
            private bool _hasBeenCalled;
            private IBusinessObject _bo;

            public bool HasBeenCalled
            {
                get { return _hasBeenCalled; }
            }

            public IBusinessObject Bo
            {
                get { return _bo; }
            }

            public void DeleteBusinessObject(IBusinessObject businessObject)
            {
                _bo = businessObject;
                _hasBeenCalled = true;
            }
        }

        #endregion

        #region Utility Methods

        private static void AssertThatDataColumnSetupCorrectly(ClassDef classDef, UIGridColumn columnDef1,
                                                               IDataGridViewColumn dataColumn1)
        {
            Assert.AreEqual(columnDef1.PropertyName, dataColumn1.DataPropertyName); //Test Prop
            Assert.AreEqual(columnDef1.PropertyName, dataColumn1.Name);
            Assert.AreEqual(columnDef1.GetHeading(), dataColumn1.HeaderText);
            Assert.IsTrue(dataColumn1.Visible);
            Assert.IsTrue(dataColumn1.ReadOnly);
            Assert.AreEqual(columnDef1.Width, dataColumn1.Width);
            PropDef propDef = GetPropDef(classDef, columnDef1);
            Assert.AreEqual(propDef.PropertyType, dataColumn1.ValueType);
        }

        private static PropDef GetPropDef(ClassDef classDef, UIGridColumn gridColumn)
        {
            PropDef propDef = null;
            if (classDef.PropDefColIncludingInheritance.Contains(gridColumn.PropertyName))
            {
                propDef = classDef.PropDefColIncludingInheritance[gridColumn.PropertyName];
            }
            return propDef;
        }

        private static void AssertVerifyIDFieldSetUpCorrectly(IDataGridViewColumn column)
        {
            string idPropertyName = "ID";
            Assert.AreEqual(idPropertyName, column.Name);
            Assert.AreEqual(idPropertyName, column.HeaderText);
            Assert.AreEqual(idPropertyName, column.DataPropertyName);
            Assert.IsTrue(column.ReadOnly);
            Assert.IsFalse(column.Visible);
            Assert.AreEqual(typeof (string), column.ValueType);
        }

        private static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO();
            cp.TestProp = "b";
            MyBO cp2 = new MyBO();
            cp2.TestProp = "d";
            MyBO cp3 = new MyBO();
            cp3.TestProp = "c";
            MyBO cp4 = new MyBO();
            cp4.TestProp = "a";
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(cp, cp2, cp3, cp4);
            return col;
        }

        private IReadOnlyGridControl GetGridWith_4_Rows(out BusinessObjectCollection<MyBO> col)
        {
            LoadMyBoDefaultClassDef();
            col = CreateCollectionWith_4_Objects();
            IReadOnlyGridControl readOnlyGridControl = CreateReadOnlyGridControl();
            SetupGridColumnsForMyBo(readOnlyGridControl.Grid);
            readOnlyGridControl.SetBusinessObjectCollection(col);
            return readOnlyGridControl;
        }

        private static void SetupGridColumnsForMyBo(IReadOnlyGrid gridBase)
        {
            gridBase.Columns.Add("TestProp", "TestProp");
        }

        #endregion //Utility Methods
    }
}