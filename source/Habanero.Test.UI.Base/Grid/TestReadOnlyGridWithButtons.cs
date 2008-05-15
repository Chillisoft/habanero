using System;
using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.UI.Grid
{
    [TestFixture]
    public abstract class TestReadonlyGridWithButtons : TestUsingDatabase
    {
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
        public void TestInitialisingObjectCreator()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridWithButtons readOnlyGridWithButtons = CreateReadOnlyGridWithButtons();
            SetupGridColumnsForMyBo(readOnlyGridWithButtons.Grid);
           
            //---------------Execute Test ----------------------
            readOnlyGridWithButtons.SetCollection(col);

            //---------------Test Result -----------------------

            Assert.IsNotNull(readOnlyGridWithButtons.BusinessObjectCreator);
            Assert.IsTrue(readOnlyGridWithButtons.BusinessObjectCreator is DefaultBOCreator);
            Assert.IsTrue(((DefaultBOCreator)readOnlyGridWithButtons.BusinessObjectCreator).CreateBusinessObject() is MyBO);
            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestInitialisingObjectEditor()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridWithButtons readOnlyGridWithButtons = CreateReadOnlyGridWithButtons();
            SetupGridColumnsForMyBo(readOnlyGridWithButtons.Grid);

            //---------------Execute Test ----------------------
            readOnlyGridWithButtons.SetCollection(col);
            //---------------Test Result -----------------------

            Assert.IsNotNull(readOnlyGridWithButtons.BusinessObjectEditor);
            Assert.IsTrue(readOnlyGridWithButtons.BusinessObjectEditor is DefaultBOEditor);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestInitialisingObjectDeletor()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IReadOnlyGridWithButtons readOnlyGridWithButtons = CreateReadOnlyGridWithButtons();
            SetupGridColumnsForMyBo(readOnlyGridWithButtons.Grid);

            //---------------Execute Test ----------------------
            readOnlyGridWithButtons.SetCollection(col);
            //---------------Test Result -----------------------

            Assert.IsNotNull(readOnlyGridWithButtons.BusinessObjectDeletor);
            Assert.IsTrue(readOnlyGridWithButtons.BusinessObjectDeletor is DefaultBODeletor);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_EditButtonClick_NoSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridWithButtons grid = GetGridWith_4_Rows(out col);
            grid.SelectedBusinessObject = null;
            ObjectEditorStub objectEditor = new ObjectEditorStub();
            grid.BusinessObjectEditor = objectEditor;

            //---------------Execute Test ----------------------
            grid.Buttons["Edit"].PerformClick();
            //---------------Test Result -----------------------

            Assert.IsFalse(objectEditor.HasBeenCalled);

        }

        [Test]
        public void TestEditButtonClick()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridWithButtons grid = GetGridWith_4_Rows(out col);
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

        [Test, Ignore("No support for uidef setting on grid yet.")]
        public void TestEditButtonClickUsingAlternateUIDef()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            col = CreateCollectionWith_4_Objects();
            IReadOnlyGridWithButtons readOnlyGridWithButtons = CreateReadOnlyGridWithButtons();
            SetupGridColumnsForMyBo(readOnlyGridWithButtons.Grid);

            //TODO: SET GRID's UIDEF : how?
            readOnlyGridWithButtons.SetCollection(col);
            readOnlyGridWithButtons.SelectedBusinessObject = col[2];
            ObjectEditorStub objectEditor = new ObjectEditorStub();
            readOnlyGridWithButtons.BusinessObjectEditor = objectEditor;

            //---------------Execute Test ----------------------
            readOnlyGridWithButtons.Buttons["Edit"].PerformClick();

            //---------------Test Result -----------------------
            Assert.AreSame("differentuidef", objectEditor.DefName);
        }
        //TODO: same test as above for add (TestAddButtonClickUsingAlternateUIDef)
        //TODO: successful edit button click should update the values in the grid on Windows.Forms


        [Test]
        public void TestAddButtonClick()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridWithButtons grid = GetGridWith_4_Rows(out col);
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
        public void TestDeleteButton()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridWithButtons grid = GetGridWith_4_Rows(out col);
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
        public void TestDeleteButtonWithNothingSelected()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IReadOnlyGridWithButtons grid = GetGridWith_4_Rows(out col);
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
        //TODO:
        // These two tests both write to the database.  If there is a way
        //   to mock these without writing then please change it, but I
        //   couldn't see how to mock a BO or a connection successfully
        [Test, Ignore("Ignore to be sorted out by brett")]
        public void TestAcceptance_DeleteButtonClickSuccessfulDelete()
        {
            //---------------Set up test pack-------------------
            ContactPerson bo = new ContactPerson();
            bo.Surname = "please delete me.";
            bo.Save();
            BOPrimaryKey contactPersonPK = bo.ID;

            BusinessObjectCollection<ContactPerson> boCol = new BusinessObjectCollection<ContactPerson>();
            boCol.Add(bo);
            IReadOnlyGridWithButtons readOnlyGridWithButtons = CreateReadOnlyGridWithButtons();
            readOnlyGridWithButtons.Grid.Columns.Add("Surname", "Surname");
            readOnlyGridWithButtons.SetCollection(boCol);
            readOnlyGridWithButtons.SelectedBusinessObject = bo;
            readOnlyGridWithButtons.Buttons.ShowDefaultDeleteButton = true;
            //---------------Execute Test ----------------------
            readOnlyGridWithButtons.Buttons["Delete"].PerformClick();
            //---------------Test Result -----------------------

            ContactPerson contactPerson = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(contactPersonPK);
            Assert.IsNull(contactPerson);
        }
        //TODO:
        [Test, Ignore("Ignore to be sorted out by brett")]
        public void TestAcceptance_DeleteButtonClickUnsuccessfulDelete()
        {
            ContactPerson person = new ContactPerson();
            person.Surname = "please delete me";
            person.Save();
            BOPrimaryKey contactPersonPK = person.ID;
            person.AddPreventDeleteRelationship();

            Address address = person.Addresses.CreateBusinessObject();
            address.Save();

            BusinessObjectCollection<ContactPerson> boCol = new BusinessObjectCollection<ContactPerson>();
            boCol.Add(person);
            IReadOnlyGridWithButtons readOnlyGridWithButtons = CreateReadOnlyGridWithButtons();
            readOnlyGridWithButtons.Grid.Columns.Add("Surname", "Surname");
            readOnlyGridWithButtons.SetCollection(boCol);
            readOnlyGridWithButtons.SelectedBusinessObject = person;
            readOnlyGridWithButtons.Buttons.ShowDefaultDeleteButton = true;
            ExceptionNotifierStub exceptionNotifier = new ExceptionNotifierStub();
            GlobalRegistry.UIExceptionNotifier = exceptionNotifier;

            //---------------Execute Test ----------------------
            readOnlyGridWithButtons.Buttons["Delete"].PerformClick();
            //---------------Test Result -----------------------

            Assert.IsTrue(exceptionNotifier.Notified);
            ContactPerson contactPerson = BOLoader.Instance.GetBusinessObjectByID<ContactPerson>(contactPersonPK);
            Assert.IsNotNull(contactPerson);

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
