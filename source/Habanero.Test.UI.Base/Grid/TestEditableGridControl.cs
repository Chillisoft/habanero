using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public  abstract class TestEditableGridControl
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
           // base.SetupDBConnection();
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();
        protected abstract void AddControlToForm(IControlChilli cntrl);



        //[TestFixture]
        //public class TestEditableGridControlWin : TestEditableGridControl
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

        //    //private static System.Windows.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
        //    //                                                             IGridBase gridBase)
        //    //{
        //    //    System.Windows.Forms.DataGridView dgv = (System.Windows.Forms.DataGridView) gridBase;
        //    //    System.Windows.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
        //    //    return row.Cells[propName];
        //    //}

        //    //protected override void AddControlToForm(IGridBase gridBase)
        //    //{
        //    //    throw new NotImplementedException();
        //    //}
        //    protected override void AddControlToForm(IControlChilli cntrl)
        //    {
        //        System.Windows.Forms.Form frm = new System.Windows.Forms.Form();
        //        frm.Controls.Add((System.Windows.Forms.Control)cntrl);
        //    }
        //}

        [TestFixture]
        public class TestEditableGridControlGiz : TestEditableGridControl
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

            //private static Gizmox.WebGUI.Forms.DataGridViewCell GetCell(int rowIndex, string propName,
            //                                                IGridBase gridBase)
            //{
            //    Gizmox.WebGUI.Forms.DataGridView dgv = (Gizmox.WebGUI.Forms.DataGridView)gridBase;
            //    Gizmox.WebGUI.Forms.DataGridViewRow row = dgv.Rows[rowIndex];
            //    return row.Cells[propName];
            //}

            //private object GetCellValue(int rowIndex, IGridBase gridBase, string propName)
            //{
            //    Gizmox.WebGUI.Forms.DataGridViewCell cell = GetCell(rowIndex, propName, gridBase);
            //    return cell.Value;
            //}
            [Test]
            protected override void AddControlToForm(IControlChilli cntrl)
            {
                Gizmox.WebGUI.Forms.Form frm = new Gizmox.WebGUI.Forms.Form();
                frm.Controls.Add((Gizmox.WebGUI.Forms.Control)cntrl);
            }

            [Test]
            public void TestGizInitialise_SelectionEditMode()
            {
                //---------------Set up test pack-------------------
                IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
                MyBO.LoadDefaultClassDef();
                ClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
                //---------------Execute Test ----------------------
                gridControl.Initialise(def);
                //---------------Test Result -----------------------
                Assert.AreEqual(Gizmox.WebGUI.Forms.DataGridViewSelectionMode.CellSelect, ((Gizmox.WebGUI.Forms.DataGridView) gridControl.Grid).SelectionMode);
                Assert.AreEqual(Gizmox.WebGUI.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2, ((Gizmox.WebGUI.Forms.DataGridView)gridControl.Grid).EditMode);
                //---------------Tear Down -------------------------
            }

            [Test, Ignore("Currently working on this")]
            public void TestGiz_CheckBoxUIGridDef_Creates_CheckBoxColumn()
            {
                //---------------Set up test pack-------------------
                IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
                MyBO.LoadClassDefWithBoolean();
                ClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
                //--------------Assert PreConditions----------------            

                //---------------Execute Test ----------------------
                gridControl.Initialise(def);
                //---------------Test Result -----------------------
                IDataGridViewColumn column = gridControl.Grid.Columns["TestBoolean"];
                Assert.IsNotNull(column);
                Assert.IsInstanceOfType(typeof(DataGridViewCheckBoxColumnGiz), column);
                //---------------Tear Down -------------------------          
            }
        }

        [Test]
        public void TestConstructor()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            gridControl.Width = 200;
            gridControl.Height = 133;
            //---------------Test Result -----------------------
            Assert.IsNotNull(gridControl);
            Assert.IsNotNull(gridControl.Grid);
            Assert.AreSame(gridControl.Grid, gridControl.Grid);
            Assert.AreEqual(1, gridControl.Controls.Count);
            Assert.AreEqual(gridControl.Width, gridControl.Grid.Width);
            Assert.AreEqual(gridControl.Height, gridControl.Grid.Height);


            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestInitialise()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
            //---------------Execute Test ----------------------
            gridControl.Initialise(def);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, gridControl.Grid.Columns.Count);
            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void Test_SetCollection()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
            gridControl.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridControl.Grid.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.IsFalse(gridControl.Grid.ReadOnly);
            Assert.IsTrue(gridControl.Grid.AllowUserToAddRows);
            Assert.IsTrue(gridControl.Grid.AllowUserToDeleteRows);

            Assert.AreEqual(1, gridControl.Grid.Rows.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_Using_EditableDataSetProvider()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl gridControl = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
            gridControl.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            //--------------Assert PreConditions----------------            

            //---------------Execute Test ----------------------
            gridControl.Grid.SetBusinessObjectCollection(col);
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(EditableDataSetProvider), gridControl.Grid.DataSetProvider);
            //---------------Tear Down -------------------------          
        }

        [Test, Ignore("Cannot get this to work need to look at firing the events")]
        public void Test_EditInTextbox_FirstRow()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
            grid.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(1, grid.Grid.Rows.Count);
            //---------------Execute Test ----------------------
            grid.Grid.SetBusinessObjectCollection(col);
            string testvalue = "testvalue";
            grid.Grid.Rows[0].Cells[1].Value = testvalue;
//            grid.ApplyChangesToBusinessObject();
            //---------------Test Result -----------------------
            Assert.AreEqual(1, col.CreatedBusinessObjects.Count);
            MyBO newBo = col.CreatedBusinessObjects[0];
            Assert.AreEqual(testvalue, newBo.TestProp);
        }

        [Test, Ignore("Cannot get this to work need to look at firing the events")]
        public void Test_EditInTextbox_ExistingObject()
        {
            //---------------Set up test pack-------------------
            IEditableGridControl grid = GetControlFactory().CreateEditableGridControl();
            AddControlToForm(grid);
            MyBO.LoadDefaultClassDef();
            ClassDef def = ClassDef.ClassDefs[typeof(MyBO)];
            grid.Initialise(def);
            BusinessObjectCollection<MyBO> col = new BusinessObjectCollection<MyBO>();
            MyBO bo = new MyBO();
            bo.TestProp = "testPropValue";
            col.Add(bo);
            grid.Grid.SetBusinessObjectCollection(col);
            //--------------Assert PreConditions----------------            
            Assert.AreEqual(2, grid.Grid.Rows.Count, "Editable auto adds adding row");

            //---------------Execute Test ----------------------
            
            
            string testvalue = "new test value";
            grid.Grid.Rows[0].Cells[1].Value = testvalue;
            grid.Grid.Rows[1].Selected = true;
//            grid.ApplyChangesToBusinessObject();

            //---------------Test Result -----------------------
            Assert.AreEqual(testvalue, bo.TestProp);
        }




    }
}
