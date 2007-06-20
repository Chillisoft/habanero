using System.Windows.Forms;
using Habanero.Bo.ClassDefinition;
using Habanero.Bo;
using Habanero.Test;
using Habanero.Ui.Application;
using Habanero.Ui.Generic;
using NUnit.Framework;

namespace Habanero.Test.Ui.Application
{
    /// <summary>
    /// Summary description for TestSimpleGrid.
    /// </summary>
    [TestFixture]
    public class TestSimpleGrid
    {
        private SimpleGrid grid;

        [SetUp]
        public void SetupTest()
        {
            grid = new SimpleGrid();
            ClassDef.GetClassDefCol.Clear();
        }

        [Test]
        public void TestSetBusinessObjectCollection()
        {
            ClassDef classDef = MyBo.LoadClassDefWithBoolean();
            SetupGrid(classDef);

            Assert.AreEqual(4, grid.DataTable.Columns.Count);
            Assert.AreEqual(2, grid.DataTable.Rows.Count);
        }

        private void SetupGrid(ClassDef classDef)
        {
            BusinessObjectCollection col = new BusinessObjectCollection(classDef);
            BusinessObject bo1 = classDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("TestProp", "Value1");
            bo1.SetPropertyValue("TestProp2", "Value2");
            BusinessObject bo2 = classDef.CreateNewBusinessObject();
            bo2.SetPropertyValue("TestProp", "2Value1");
            bo2.SetPropertyValue("TestProp2", "2Value2");
            col.Add(bo1);
            col.Add(bo2);
            grid.SetGridDataProvider(new SimpleGridDataProvider(col, bo1.GetUserInterfaceMapper().GetUIGridProperties()));
        }

        [Test]
        public void TestColumnTypes()
        {
            ClassDef classDef = MyBo.LoadClassDefWithBoolean();
            SetupGrid(classDef);

            Assert.AreSame(typeof (DataGridViewTextBoxColumn), grid.Columns[0].GetType());
            Assert.AreSame(typeof (DataGridViewTextBoxColumn), grid.Columns[1].GetType());
            Assert.AreSame(typeof (DataGridViewTextBoxColumn), grid.Columns[2].GetType());
            Assert.AreSame(typeof (DataGridViewCheckBoxColumn), grid.Columns[3].GetType());
        }


        [Test]
        public void TestColumnTypesCombo()
        {
            ClassDef classDef = MyBo.LoadDefaultClassDef();
            SetupGrid(classDef);

            Assert.AreSame(typeof (DataGridViewTextBoxColumn), grid.Columns[0].GetType());
            Assert.AreSame(typeof (DataGridViewTextBoxColumn), grid.Columns[1].GetType());
            Assert.AreSame(typeof (DataGridViewComboBoxColumn), grid.Columns[2].GetType());
        }
    }
}