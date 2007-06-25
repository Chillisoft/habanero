using System.Data;
using System.Windows.Forms;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Test;
using Habanero.Ui.Base;
using Habanero.Ui.Grid;
using NUnit.Framework;

namespace Habanero.Test.Ui.Application
{
    /// <summary>
    /// Summary description for TestSimpleReadOnlyGrid.
    /// </summary>
    [TestFixture]
    public class TestSimpleReadOnlyGrid
    {
        private Form frm;
        private SimpleReadOnlyGrid grid;
        private BusinessObject bo1;
        private BusinessObject bo2;
        private DataTable itsDataSource;
        //private BusinessObjectBase itsClickedBo;

        [SetUp]
        public void SetupFixture()
        {
            grid = new SimpleReadOnlyGrid();
            grid.Name = "GridControl";
            ClassDef.GetClassDefCol.Clear();
            ClassDef classDef = MyBo.LoadClassDefWithNoLookup();
            BusinessObjectCollection col = new BusinessObjectCollection(classDef);
			bo1 = MyBo.Create(); //classDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("TestProp", "Value1");
            bo1.SetPropertyValue("TestProp2", "Value2");
            bo2 = MyBo.Create(); //classDef.CreateNewBusinessObject();
            bo2.SetPropertyValue("TestProp", "2Value1");
            bo2.SetPropertyValue("TestProp2", "2Value2");
            col.Add(bo1);
            col.Add(bo2);
            grid.SetGridDataProvider(new SimpleGridDataProvider(col, bo1.GetUserInterfaceMapper().GetUIGridProperties()));
            frm = new Form();
            grid.Dock = DockStyle.Fill;
            frm.Controls.Add(grid);
            //grid.DoubleClick += new EventHandler(frmdoubleclicked) ;
            frm.Show();
            itsDataSource = grid.DataTable;
        }

        //		private void frmdoubleclicked(object sender, EventArgs e) {
        //			Point p = grid.PointToClient(Control.MousePosition );
        //			MessageBox.Show(p.X + ", " + p.Y ); 
        //		}

        [TearDown]
        public void TearDown()
        {
            frm.Close();
            frm.Dispose();
        }

        [Test]
        public void TestSelectedBusinessObject()
        {
            grid.SelectedBusinessObject = bo2;
            BusinessObject selectedBo = grid.SelectedBusinessObject;
            Assert.AreEqual("2Value1", selectedBo.GetPropertyValueString("TestProp"));
            Assert.AreEqual("2Value2", selectedBo.GetPropertyValueString("TestProp2"));
            Assert.AreSame(bo2, selectedBo);
        }

        [Test]
        public void TestRowIsRefreshed()
        {
            bo2.SetPropertyValue("TestProp", "UpdatedValue");
            //grid.RefreshRow(bo2);
            Assert.AreEqual("UpdatedValue", itsDataSource.Rows[1][1]);
        }

        //		[Test]
        //		public void TestDoubleClick() {
        //			ControlTester gridControlTester = new ControlTester("GridControl") ;
        //			grid.RowDoubleClicked += new RowDoubleClickedHandler(RowDoubleClicked) ;
        //			gridControlTester.MouseController().DoubleClick(60, 50) ;
        //		//	Thread.Sleep(1000) ;
        //		//	Assert.AreSame(bo1, itsClickedBo);
        //
        //		}
        //
        //		private void RowDoubleClicked(object sender, BOEventArgs e) {
        //			itsClickedBo = e.BusinessObject ;
        //			Console.Out.WriteLine("selected");
        //		}
    }
}