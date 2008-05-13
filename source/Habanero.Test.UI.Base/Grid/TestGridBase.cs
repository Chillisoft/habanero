using System;
using System.Collections.Generic;
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

            [Test, Ignore("Need to implement interfaces for DataGridViewCell etc not sure if worth it for ths test only if we implemente these interfaces later then make this test work")]
            public void TestRowIsRefreshed()
            {
                //---------------Set up test pack-------------------
                BusinessObjectCollection<MyBO> col;
                IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
                MyBO bo = col[0];

                //---------------Execute Test ----------------------
                string propName = "TestProp";
                bo.SetPropertyValue(propName, "UpdatedValue");
                
                //---------------Test Result -----------------------
                gridBase.SelectedBusinessObject = bo;
                System.Windows.Forms.DataGridViewRow row = (System.Windows.Forms.DataGridViewRow) gridBase.Rows[0];
                System.Windows.Forms.DataGridViewCell cell = row.Cells[propName];
                Assert.AreEqual("UpdatedValue", cell.Value);
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
            IChilliControl myGridBase = CreateGridBaseStub();

            //---------------Test Result -----------------------
            Assert.IsNotNull(myGridBase);
            Assert.IsTrue(myGridBase is IGridBase);
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void TestSetCollectionOnGrid_EmptyCollection()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            //---------------Execute Test ----------------------
            gridBase.SetCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Rows.Count);
            //Assert.AreEqual(classDef.PropDefcol.Count, myGridBase.Columns.Count);//There are 8 columns in the collection BO
            Assert.IsNull(gridBase.SelectedBusinessObject);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestSetCollectionOnGrid_NoOfRows()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
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
            SetupGridColumnsForMyBo(gridBase);

            //---------------Execute Test ----------------------
            gridBase.SetCollection(col);
            BusinessObject selectedBo = gridBase.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreSame(col[0], selectedBo);
            Assert.AreEqual(1, gridBase.SelectedBusinessObjects.Count);
        }

        [Test]
        public void TestSetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            SetupGridColumnsForMyBo(gridBase);
            MyBO boToSelect = col[1];
            //---------------Execute Test ----------------------

            gridBase.SelectedBusinessObject = boToSelect;

            //---------------Test Result -----------------------
            Assert.AreEqual(boToSelect, gridBase.SelectedBusinessObject);
        }

        [Test]
        public void TestGetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO boToSelect = col[1];
            gridBase.SelectedBusinessObject = boToSelect;
            //---------------Execute Test ----------------------
            BusinessObject selectedBusinessObject = gridBase.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreEqual(boToSelect, selectedBusinessObject);
        }

        [Test, Ignore("The old tests tested this but no longer works for giz or windows")]
        public void TestSelectedBusinessObject_SetsCurrentRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO boToSelect = col[1];

            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = boToSelect;
            //---------------Test Result -----------------------
            Assert.IsNotNull(gridBase.CurrentRow);
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
            Assert.IsNull(gridBase.CurrentRow);
                //The current row is never set see ignored test TestSelectedBusinessObject_SetsCurrentRow
        }

        [Test]
        public void TestGridFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            bool gridItemSelected = false;
            gridBase.SelectedBusinessObject = null;
            gridBase.BusinessObjectSelected += (delegate { gridItemSelected = true; });

            //---------------Execute Test ----------------------
            gridBase.SelectedBusinessObject = col[1];

            //---------------Test Result -----------------------
            Assert.IsTrue(gridItemSelected);
        }

        [Test]
        public void TestGrid_2GetSelectedObjects()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            MyBO boToSelect1 = col[1];
            gridBase.Rows[1].Selected = true;

            //---------------Execute Test ----------------------
            IList<BusinessObject> selectedObjects = gridBase.SelectedBusinessObjects;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, selectedObjects.Count); //The first row is auto selected and the second row
//            is being manually selected
            //Test that the correct items where returned
            Assert.AreSame(boToSelect1, selectedObjects[0]);
            Assert.AreSame(col[0], selectedObjects[1]);
        }

        [Test]
        public void TestGrid_GetSelectedObjects_3SelectedObjects()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            gridBase.Rows[1].Selected = true;
            gridBase.Rows[3].Selected = true;
            //---------------Execute Test ----------------------
            IList<BusinessObject> selectedObjects = gridBase.SelectedBusinessObjects;
            //---------------Test Result -----------------------
            Assert.AreEqual(3, selectedObjects.Count); //the first row plus the two new selected rows
        }

        [Test]
        public void TestGrid_Clear()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            gridBase.Clear();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Rows.Count); //The first row is auto selected and the second row
            //is being manually selected
            //Test that the correct items where returned
            Assert.AreEqual(0, gridBase.SelectedBusinessObjects.Count);
            Assert.AreEqual(null, gridBase.SelectedBusinessObject);
        }

        [Test]
        public void TestCollectionChanged()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            bool hasCollectionChangedFired = false;
            gridBase.CollectionChanged += delegate { hasCollectionChangedFired = true; };
            //---------------Execute Test ----------------------
            gridBase.SetCollection(col);
            //---------------Test Result -----------------------
            Assert.IsTrue(hasCollectionChangedFired, "CollectionChanged event should have fired.");
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetCollection()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            IBusinessObjectCollection collection = gridBase.GetCollection();
            //---------------Test Result -----------------------
            Assert.AreSame(col, collection);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestGetBusinessObjectAtRow()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            BusinessObject businessObject2 = gridBase.GetBusinessObjectAtRow(2);
            BusinessObject businessObject3 = gridBase.GetBusinessObjectAtRow(3);
            //---------------Test Result -----------------------
            Assert.AreSame(col[2], businessObject2);
            Assert.AreSame(col[3], businessObject3);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestNoOfColumns()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Test Result -----------------------
            Assert.AreEqual(0, gridBase.Columns.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestOneColumnAdded()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Execute Test ----------------------
            //IDataGridViewColumn column = new DataGridViewColumnStub();
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            //---------------Test Result -----------------------
            Assert.AreEqual(1, gridBase.Columns.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestTwoColumnsAdded()
        {
            //---------------Set up test pack-------------------
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Execute Test ----------------------
            //IDataGridViewColumn column = new DataGridViewColumnStub();
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            gridBase.Columns.Add(Guid.NewGuid().ToString("N"), "");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, gridBase.Columns.Count);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetCollectionNoColumnsAddedShouldReturnError()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            //---------------Execute Test ----------------------
            try
            {
                gridBase.SetCollection(col);
                Assert.Fail();
                //---------------Test Result -----------------------
            }
            catch (GridBaseSetUpException ex)
            {
                StringAssert.Contains("cannot call SetCollection if the grid's columns have not been set up", ex.Message);
            }

            //---------------Tear Down -------------------------
        }


        [Test, Ignore("This will need to work after the implementation of the dataTable behind the grid is improved.")]
        public void TestSetSortColumn()
        {
            //---------------Set up test pack-------------------

            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            gridBase.SetSortColumn("TestProp", true, true);
            //---------------Test Result -----------------------
            Assert.AreSame(col[3], gridBase.GetBusinessObjectAtRow(1),
                           "The object with testprop = 'a' should be at row 1");
            Assert.AreSame(col[0], gridBase.GetBusinessObjectAtRow(2),
                           "The object with testprop = 'b' should be at row 2");
            Assert.AreSame(col[2], gridBase.GetBusinessObjectAtRow(3),
                           "The object with testprop = 'c' should be at row 3");
            Assert.AreSame(col[1], gridBase.GetBusinessObjectAtRow(4),
                           "The object with testprop = 'd' should be at row 4");
            //---------------Tear Down -------------------------
        }
        [Test, Ignore("Peter what is this when is it used must we implement")]
        public void TestGetCollectionClone()
        {
            //---------------Set up test pack-------------------
            BusinessObjectCollection<MyBO> col;
            IGridBase gridBase = GetGridBaseWith_4_Rows(out col);
            //---------------Execute Test ----------------------
            ///To be implemented IBusinessObjectCollection cloneCol = gridBase.GetCollectionClone();
            //---------------Test Result -----------------------
            ////Assert.AreEqual( 4,cloneCol.Count);
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

        private IGridBase GetGridBaseWith_4_Rows(out BusinessObjectCollection<MyBO> col)
        {
            MyBO.LoadDefaultClassDef();
            col = CreateCollectionWith_4_Objects();
            IGridBase gridBase = CreateGridBaseStub();
            SetupGridColumnsForMyBo(gridBase);
            gridBase.SetCollection(col);
            return gridBase;
        }


        private static void SetupGridColumnsForMyBo(IGridBase gridBase)
        {
            gridBase.Columns.Add("TestProp", "TestProp");
        }
        internal class GridBaseGizStub : GridBaseGiz
        {
        }

        internal class GridBaseWinStub : GridBaseWin
        {
        }

        #endregion
    }


    internal class DataGridViewColumnStub : IDataGridViewColumn
    {
    }
}