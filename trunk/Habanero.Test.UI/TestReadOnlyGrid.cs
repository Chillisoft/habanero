using System.Data;
using System.Windows.Forms;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Test;
using Habanero.UI.Base;
using Habanero.UI.Grid;
using NUnit.Framework;

namespace Habanero.Test.Ui.Application
{
    /// <summary>
    /// Summary description for TestReadOnlyGrid.
    /// </summary>
    [TestFixture]
    public class TestReadOnlyGrid
    {
        private Form frm;
        private ReadOnlyGrid grid;
        private BusinessObject bo1;
        private BusinessObject bo2;
        private DataTable itsDataSource;

        [SetUp]
        public void SetupFixture()
        {
            grid = new ReadOnlyGrid();
            grid.Name = "GridControl";
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBo.LoadClassDefWithNoLookup();
            BusinessObjectCollection<BusinessObject> col = new BusinessObjectCollection<BusinessObject>(classDef);
            bo1 = new MyBo();
            bo1.SetPropertyValue("TestProp", "Value1");
            bo1.SetPropertyValue("TestProp2", "Value2");
            bo2 = new MyBo();
            bo2.SetPropertyValue("TestProp", "2Value1");
            bo2.SetPropertyValue("TestProp2", "2Value2");
            col.Add(bo1);
            col.Add(bo2);
            grid.SetCollection(col);
            frm = new Form();
            grid.Dock = DockStyle.Fill;
            frm.Controls.Add(grid);
            frm.Show();
            itsDataSource = grid.DataTable;
        }



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
            Assert.AreEqual("2Value1", selectedBo.Props["TestProp"].Value);
            Assert.AreEqual("2Value2", selectedBo.Props["TestProp2"].Value);
            Assert.AreSame(bo2, selectedBo);
        }

        [Test]
        public void TestRowIsRefreshed()
        {
            bo2.SetPropertyValue("TestProp", "UpdatedValue");
            Assert.AreEqual("UpdatedValue", itsDataSource.Rows[1][1]);
        }

        [Test]
        public void TestGetCollectionClone()
        {
            BusinessObjectCollection<BusinessObject> cloneCol = grid.GetCollectionClone();
            Assert.AreEqual(cloneCol.Count,2 );
        }
    }
}