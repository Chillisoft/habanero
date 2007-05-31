using System.Windows.Forms;
using Chillisoft.Bo.ClassDefinition.v2;
using Chillisoft.Bo.v2;
using Chillisoft.Test.Setup.v2;
using Chillisoft.UI.Application.v2;
using Chillisoft.UI.Generic.v2;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Application.v2
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
            ClassDef.GetClassDefCol().Clear();
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
            BusinessObjectBaseCollection col = new BusinessObjectBaseCollection(classDef);
            BusinessObjectBase bo1 = classDef.CreateNewBusinessObject();
            bo1.SetPropertyValue("TestProp", "Value1");
            bo1.SetPropertyValue("TestProp2", "Value2");
            BusinessObjectBase bo2 = classDef.CreateNewBusinessObject();
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