using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestGridBase
    {
        [SetUp]
        public  void SetupTest()
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
        public  void TearDownTest()
        {
        }
        protected abstract IControlFactory GetControlFactory();
        protected abstract IGridBase CreateGridBaseStub();

        [TestFixture]
        public class TestGridBaseWin : TestGridBase
        {
            protected override IControlFactory GetControlFactory()
            {
                return new WinControlFactory();
            }

            protected override IGridBase CreateGridBaseStub()
            {
                GridBaseWinStub gridBase = new GridBaseWinStub();
                System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
                frm.Controls.Add(gridBase);
                return gridBase;
            }
        }

        [TestFixture]
        public class TestGridBaseGiz : TestGridBase
        {
            protected override IControlFactory GetControlFactory()
            {
                return new GizmoxControlFactory();
            }

            protected override IGridBase CreateGridBaseStub()
            {
                GridBaseGizStub gridBase = new GridBaseGizStub();
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add(gridBase);
                return gridBase;
            }
        }

        [Test]
        public void TestCreateGridBase()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IGridBase myGridBase = CreateGridBaseStub();

            //---------------Test Result -----------------------
            Assert.IsNotNull(myGridBase);

            //---------------Tear Down -------------------------   
        }
        [Test]
        public void TestSetCollectionOnGrid_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            IGridBase myGridBase = CreateGridBaseStub();
            //---------------Execute Test ----------------------
            myGridBase.SetCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, myGridBase.Rows.Count);
            Assert.AreEqual(3, myGridBase.Columns.Count);//ID plus 2 other columns
            Assert.IsNull(myGridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetCollectionOnGrid_NoOfRows()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Execute Test ----------------------
            gridBase.SetCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, gridBase.Rows.Count);
            //---------------Tear Down -------------------------    

        }

        [Test]
        public void Test_SelectedBusinessObject_FirstRowIsSelected()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();

            //---------------Execute Test ----------------------
            gridBase.SetCollection(col); 

            //---------------Test Result -----------------------
            BusinessObject selectedBo = gridBase.SelectedBusinessObject;
            Assert.AreSame(col[0], selectedBo);
        }

        [Test]
        public void TestSetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO boToSelect = col[1];
            //---------------Execute Test ----------------------
            
            gridBase.SelectedBusinessObject = boToSelect;

            //---------------Test Result -----------------------
            Assert.AreEqual(boToSelect, gridBase.SelectedBusinessObject);
        }

        private IGridBase GetGridBaseWith_4_Rows(out BusinessObjectCollection<MyBO> col)
        {
            MyBO.LoadDefaultClassDef();
            col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            gridBase.SetCollection(col);
            return gridBase;
        }

        [Test]
        public void TestSetSelectedBusinessObject_ToNull()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = col[2];
            gridBase.SelectedBusinessObject = null;

            //---------------Test Result -----------------------
            Assert.IsNull(gridBase.SelectedBusinessObject);
        }

        [Test]
        public void TestReadOnlyGridFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            bool gridItemSelected = false;
            gridBase.SelectedBusinessObject = null;
            gridBase.BusinessObjectSelected += (delegate
            {
                gridItemSelected = true;
            });

            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = col[1];

            //---------------Test Result -----------------------
            Assert.IsTrue(gridItemSelected);
        }

        private static BusinessObjectCollection<MyBO> CreateCollectionWith_4_Objects()
        {
            MyBO cp = new MyBO();
            MyBO cp2 = new MyBO();
            MyBO cp3 = new MyBO();
            MyBO cp4 = new MyBO();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            col.Add(cp, cp2, cp3, cp4);
            return col;
        }

        internal class GridBaseGizStub : GridBaseGiz
        {
        }

        internal class GridBaseWinStub : GridBaseWin
        {
        }
    }


}