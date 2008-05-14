using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.UI.Grid
{
    [TestFixture]
    public abstract class TestReadonlyGridWithButtons
    {
        [SetUp]
        public void SetupTest()
        {
            ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons();

        //[TestFixture]
        //public class TestReadonlyGridWithButtonsWin : TestReadonlyGridWithButtons
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryWin();
        //    }
        //    protected override IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons()
        //    {
        //        ReadOnlyGridWithButtonsWin readOnlyGridWithButtonsWin = new ReadOnlyGridWithButtonsWin();
        //        System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //        frm.Controls.Add(readOnlyGridWithButtonsWin);
        //        return readOnlyGridWithButtonsWin;
        //    }
        //}
        [TestFixture]
        public class TestReadonlyGridWithButtonsGiz : TestReadonlyGridWithButtons
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }
            protected override IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons()
            {
                ReadOnlyGridWithButtonsGiz readOnlyGridWithButtonsGiz = new ReadOnlyGridWithButtonsGiz(GetControlFactory());
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add(readOnlyGridWithButtonsGiz);
                return readOnlyGridWithButtonsGiz;
            }
        }

        [Test]
        public void TestCreateReadOnlyGridWithButtons()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IControlChilli grid = CreateReadOnlyGridWithButtons();

            ////---------------Test Result -----------------------
            Assert.IsNotNull(grid);
            Assert.IsTrue(grid is IReadOnlyGridWithButtons);
            IReadOnlyGridWithButtons readOnlyGrid = (IReadOnlyGridWithButtons) grid;
            Assert.IsNotNull(readOnlyGrid.Grid);
            Assert.IsNotNull(readOnlyGrid.Buttons);
        }

        [Test]
        public void TestReadOnlyGridWithButtons_WithColumns()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            //CreateCollectionWith_4_Objects();
            IReadOnlyGridWithButtons readOnlyGridWithButtons = CreateReadOnlyGridWithButtons();
            
            //---------------Execute Test ----------------------
            IReadOnlyGrid readOnlyGrid = readOnlyGridWithButtons.Grid;
            readOnlyGrid.Columns.Add("TestProp", "TestProp");
            ////---------------Test Result -----------------------
            Assert.AreEqual(1, readOnlyGrid.Columns.Count);
        }

        [Test]
        public void TestSetCollection_NumberOfRows()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridWithButtons readOnlyGridWithButtons = CreateReadOnlyGridWithButtons();
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)readOnlyGridWithButtons);
            IReadOnlyGrid readOnlyGrid = readOnlyGridWithButtons.Grid;

            readOnlyGrid.Columns.Add("TestProp", "TestProp");
            //---------------Execute Test ----------------------
            readOnlyGridWithButtons.SetCollection(col);
            ////---------------Test Result -----------------------
            Assert.AreEqual(4, readOnlyGrid.Rows.Count);
        }
        [Test]
        public void TestSetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridWithButtons readOnlyGridWithButtons = GetGridWith_4_Rows(out col);
            BusinessObject bo = col[0];
            Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            frm.Controls.Add((Gizmox.WebGUI.Forms.Control)readOnlyGridWithButtons);
            //---------------Execute Test ----------------------
            readOnlyGridWithButtons.SelectedBusinessObject = bo;

            //---------------Test Result -----------------------
            Assert.AreEqual(bo, readOnlyGridWithButtons.SelectedBusinessObject);
        }

        [Test]
        public void TestSetSelectedBusinessObject_ToNull()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridWithButtons grid = GetGridWith_4_Rows(out col);
            BusinessObject bo = col[0];

            //---------------Execute Test ----------------------
            grid.SelectedBusinessObject = bo;
            grid.SelectedBusinessObject = null;

            //---------------Test Result -----------------------
            Assert.IsNull(grid.SelectedBusinessObject);
            Assert.IsNull(grid.Grid.CurrentRow);
        }

        [Test]
        public void Test_EditButtonClick_NoSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridWithButtons grid = GetGridWith_4_Rows(out col);
            grid.SelectedBusinessObject = null;

            //---------------Execute Test ----------------------
            grid.Buttons["Edit"].PerformClick();
            //---------------Test Result -----------------------

        }

        //[Test]
        //public void TestEditButtonClickSuccessfulEdit()
        //{
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo, new object[] {});
        //    //itsGridMock.ExpectAndReturn("UIName", "default", new object[] { });
        //    //itsObjectEditorMock.ExpectAndReturn("EditObject", true, new object[] { bo, "default" });
        //    ////itsGridMock.Expect("RefreshRow", new object[] { bo }) ;
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Edit");
        //    //itsObjectEditorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestEditButtonClickUnsuccessfulEdit()
        //{
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo, new object[] {});
        //    //itsGridMock.ExpectAndReturn("UIName", "default", new object[] { });
        //    //itsObjectEditorMock.ExpectAndReturn("EditObject", false, new object[] { bo, "default" });
        //    ////itsGridMock.ExpectNoCall("RefreshRow", new Type[] {typeof(object)});
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Edit");
        //    //itsObjectEditorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestEditButtonClickNothingSelected()
        //{
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", null, new object[] {});
        //    //itsObjectEditorMock.ExpectNoCall("EditObject", new Type[] {typeof (object), typeof(string)});
        //    ////itsGridMock.ExpectNoCall("RefreshRow", new Type[] {typeof(object)});
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Edit");
        //    //itsObjectEditorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestAddButtonClickSuccessfulAdd()
        //{
        //    //itsGridMock.ExpectAndReturn("UIName", "default", new object[]{} );
        //    //itsObjectCreatorMock.ExpectAndReturn("CreateObject", bo, new object[] {itsEditor, null, "default"});
        //    //itsGridMock.Expect("AddBusinessObject", new object[] {bo});
        //    //itsButtons.ObjectCreator = itsCreator;
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Add");
        //    //itsObjectCreatorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestAddButtonClickUnsuccessfulAdd()
        //{
        //    //itsGridMock.ExpectAndReturn("UIName", "default", new object[] { });
        //    //itsObjectCreatorMock.ExpectAndReturn("CreateObject", null, new object[] {itsEditor, null, "default"});
        //    //itsGridMock.ExpectNoCall("AddBusinessObject", new Type[] {typeof (object)});
        //    //itsButtons.ObjectCreator = itsCreator;
        //    //itsButtons.ObjectEditor = itsEditor;

        //    //itsButtons.ClickButton("Add");
        //    //itsObjectCreatorMock.Verify();
        //    //itsGridMock.Verify();
        //}

        //[Test]
        //public void TestDeletionProperties()
        //{
        //    //Assert.IsFalse(itsButtons.ShowDefaultDeleteButton);
        //    //itsButtons.ShowDefaultDeleteButton = true;
        //    //Assert.IsTrue(itsButtons.ShowDefaultDeleteButton);

        //    //Assert.IsTrue(itsButtons.ConfirmDeletion);
        //    //itsButtons.ConfirmDeletion = false;
        //    //Assert.IsFalse(itsButtons.ConfirmDeletion);
        //}

        //// These two tests both write to the database.  If there is a way
        ////   to mock these without writing then please change it, but I
        ////   couldn't see how to mock a BO or a connection successfully
        //[Test]
        //public void TestDeleteButtonClickSuccessfulDelete()
        //{
        //    //ContactPerson bo = new ContactPerson();
        //    //bo.Surname = "please delete me.";
        //    //bo.Save();
        //    //itsContactPersonID = bo.ContactPersonID.Value;

        //    //BusinessObjectCollection<ContactPerson> boCol = new BusinessObjectCollection<ContactPerson>();
        //    //boCol.Add(bo);

        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObjects", boCol);
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo);
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObject", bo);

        //    //itsButtons.ShowDefaultDeleteButton = true;
        //    //itsButtons.ConfirmDeletion = false;
        //    //itsButtons.ClickButton("Delete");
        //    //itsGridMock.Verify();

        //    //ContactPerson contactPerson = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(itsContactPersonID);
        //    //Assert.IsNull(contactPerson);
        //}

        //[Test]
        //public void TestDeleteButtonClickUnsuccessfulDelete()
        //{
        //    //ContactPerson person = new ContactPerson();
        //    //person.Surname = "please delete me";
        //    //person.Save();
        //    //itsContactPersonID = person.ContactPersonID.Value;
        //    //person.AddPreventDeleteRelationship();

        //    //Address address = new Address();
        //    //address.ContactPersonID = itsContactPersonID;
        //    //address.Save();
        //    //itsAddressID = address.AddressID;

        //    //BusinessObjectCollection<ContactPerson> boCol = new BusinessObjectCollection<ContactPerson>();
        //    //boCol.Add(person);

        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObjects", boCol);
        //    //itsExceptionNotifierMock.Expect("Notify", new IsAnything(), new IsAnything(), new IsAnything());
        //    //itsGridMock.ExpectNoCall("SelectedBusinessObject");

        //    //itsButtons.ShowDefaultDeleteButton = true;
        //    //itsButtons.ConfirmDeletion = false;
        //    //itsButtons.ClickButton("Delete");
        //    //itsGridMock.Verify();

        //    //ContactPerson contactPerson = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(itsContactPersonID);
        //    //Assert.IsNotNull(contactPerson);
        //}

        //[Test]
        //public void TestDeleteButtonClickNothingSelected()
        //{
        //    //itsGridMock.ExpectAndReturn("SelectedBusinessObjects", new BusinessObjectCollection<MyBO>());
        //    //itsGridMock.ExpectNoCall("SelectedBusinessObject");

        //    //itsButtons.ShowDefaultDeleteButton = true;
        //    //itsButtons.ConfirmDeletion = false;
        //    //itsButtons.ClickButton("Delete");
        //    //itsGridMock.Verify();
        //}





        #region Utility Methods
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

        private IReadOnlyGridWithButtons GetGridWith_4_Rows(out BusinessObjectCollection<MyBO> col)
        {
            MyBO.LoadDefaultClassDef();
            col = CreateCollectionWith_4_Objects();
            IReadOnlyGridWithButtons readOnlyGridWithButtons = CreateReadOnlyGridWithButtons();
            SetupGridColumnsForMyBo(readOnlyGridWithButtons.Grid);
            readOnlyGridWithButtons.SetCollection(col);
            return readOnlyGridWithButtons;
        }

        private static void SetupGridColumnsForMyBo(IReadOnlyGrid gridBase)
        {
            gridBase.Columns.Add("TestProp", "TestProp");
        }

        #endregion //Utility Methods
    }

}
