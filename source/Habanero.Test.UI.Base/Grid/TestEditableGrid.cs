using System;
using System.Collections.Generic;
using System.Text;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestEditableGrid : TestUsingDatabase
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
            base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract void AddControlToForm(IGridBase cntrl);


        //[TestFixture]
        //public class TestEditableGridWin : TestEditableGrid
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryWin();
        //    }

        //    //protected override IGridBase CreateGridBaseStub()
        //    //{
        //    //    GridBaseWinStub gridBase = new GridBaseWinStub();
        //    //    System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //    //    frm.Controls.Add(gridBase);
        //    //    return gridBase;
        //    //}

        //    private static System.Windows.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
        //                                                                 IGridBase gridBase)
        //    {
        //        System.Windows.Forms.DataGridView dgv = (System.Windows.Forms.DataGridView) gridBase;
        //        System.Windows.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
        //        return row.Cells[propName];
        //    }

        //    protected override void AddControlToForm(IGridBase gridBase)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        [TestFixture]
        public class TestEditableGridGiz : TestEditableGrid
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryGizmox();
            }

            //protected override IGridBase CreateGridBaseStub()
            //{
            //    GridBaseGizStub gridBase = new GridBaseGizStub();
            //    Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
            //    frm.Controls.Add(gridBase);
            //    return gridBase;
            //}

            protected override void AddControlToForm(IGridBase gridBase)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control) gridBase);
            }

            private static Gizmox.WebGUI.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
                                                                        IGridBase gridBase)
            {
                Gizmox.WebGUI.Forms.DataGridView dgv = (Gizmox.WebGUI.Forms.DataGridView) gridBase;
                Gizmox.WebGUI.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
                return row.Cells[propName];
            }

            private object GetCellValue(int rowIndex, IGridBase gridBase, string propName)
            {
                Gizmox.WebGUI.Forms.DataGridViewCell cell = GetCell(rowIndex, propName, gridBase);
                return cell.Value;
            }

            //public void TestCreateGridBaseGiz()
            //{
            //    //---------------Set up test pack-------------------
            //    //---------------Execute Test ----------------------
            //    IControlChilli grid = GetControlFactory().CreateEditableGridControl();
            //    ReadOnlyGridGiz readOnlyGrid = (EditableGridControl)grid;
            //    ////---------------Test Result -----------------------
            //    Assert.IsTrue(readOnlyGrid.ReadOnly);
            //    Assert.IsFalse(readOnlyGrid.AllowUserToAddRows);
            //    Assert.IsFalse(readOnlyGrid.AllowUserToDeleteRows);
            //    Assert.IsTrue(readOnlyGrid.SelectionMode == Gizmox.WebGUI.Forms.DataGridViewSelectionMode.FullRowSelect);
            //    //---------------Tear Down -------------------------   
            //}
        }


        [Test]
        public void TestConstructGrid()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            IControlChilli grid = GetControlFactory().CreateEditableGrid();
            //---------------Test Result -----------------------
            IEditableGrid editableGrid = (IEditableGrid) grid;
            Assert.IsNotNull(editableGrid);
            Assert.IsFalse(editableGrid.ReadOnly);
            Assert.IsTrue(editableGrid.AllowUserToAddRows);
            Assert.IsTrue(editableGrid.AllowUserToDeleteRows);
            //TODO: Should we test selection mode
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestSetCollectionOnGrid_NoOfRows()
        {
            //---------------Set up test pack-------------------
            MyBO.LoadDefaultClassDef();
            BusinessObjectCollection<MyBO> col = CreateCollectionWith_4_Objects();
            IEditableGrid editableGrid = GetControlFactory().CreateEditableGrid();
            AddControlToForm(editableGrid);
            SetupGridColumnsForMyBo(editableGrid);
            //---------------Execute Test ----------------------
            editableGrid.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.AreEqual(4, editableGrid.Rows.Count);
            //---------------Tear Down -------------------------    
        }

        [Test, Ignore("Currently working on this")]
        public void TestSetupColumnAsTextBoxType()
        {
//            ClassDef classDef = MyBO.LoadClassDefWithBoolean();
//            GetCol_BO_2Items(classDef);
//
//            Assert.AreSame(typeof (DataGridViewTextBoxColumn), grid.Columns[0].GetType());

            //---------------Set up test pack-------------------
            ClassDef classDef = MyBO.LoadClassDefWith_Grid_1TextboxColumn();
            BusinessObjectCollection<BusinessObject> colBOs = GetCol_BO_2Items(classDef);
            IEditableGrid grid = GetControlFactory().CreateEditableGrid();
            SetupGridColumnsForMyBo(grid);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, classDef.UIDefCol.Count);
            UIGrid uiGridDef = classDef.UIDefCol["default"].UIGrid;
            Assert.IsNotNull(uiGridDef);
            Assert.AreEqual(1,uiGridDef.Count);

            //---------------Execute Test ----------------------
            grid.SetBusinessObjectCollection(colBOs);
            //---------------Test Result -----------------------
            Assert.AreSame(typeof(IDataGridViewTextBoxColumn), grid.Columns[0].GetType());
            //---------------Tear Down -------------------------          
        }
        private static BusinessObjectCollection<BusinessObject> GetCol_BO_2Items(ClassDef classDef)
        {
            BusinessObjectCollection<BusinessObject> col = new BusinessObjectCollection<BusinessObject>(classDef);
            BusinessObject bo1 = classDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("TestProp", "Value1");
            bo1.SetPropertyValue("TestProp2", "Value2");
            BusinessObject bo2 = classDef.CreateNewBusinessObject();
            bo2.SetPropertyValue("TestProp", "2Value1");
            bo2.SetPropertyValue("TestProp2", "2Value2");
            col.Add(bo1);
            col.Add(bo2);
            return col;

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

        private static void SetupGridColumnsForMyBo(IEditableGrid readOnlyGrid)
        {
            readOnlyGrid.Columns.Add("TestProp", "TestProp");
        }
    }
}