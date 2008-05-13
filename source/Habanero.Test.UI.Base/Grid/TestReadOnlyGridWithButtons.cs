using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
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
        //        return new WinControlFactory();
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
                return new GizmoxControlFactory();
            }
            protected override IReadOnlyGridWithButtons CreateReadOnlyGridWithButtons()
            {
                ReadOnlyGridWithButtonsGiz readOnlyGridWithButtonsGiz = new ReadOnlyGridWithButtonsGiz();
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
            IChilliControl grid = CreateReadOnlyGridWithButtons();

            ////---------------Test Result -----------------------
            Assert.IsNotNull(grid);
            Assert.IsTrue(grid is IReadOnlyGridWithButtons);
            IReadOnlyGridWithButtons readOnlyGrid = (IReadOnlyGridWithButtons) grid;
            Assert.IsNotNull(readOnlyGrid.Grid);
        }

        [Test]
        public void TestReadOnlyGridWithButtons_WithColums()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
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
