//---------------------------------------------------------------------------------
// Copyright (C) 2007 Chillisoft Solutions
// 
// This file is part of the Habanero framework.
// 
//     Habanero is a free framework: you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     The Habanero framework is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU Lesser General Public License for more details.
// 
//     You should have received a copy of the GNU Lesser General Public License
//     along with the Habanero framework.  If not, see <http://www.gnu.org/licenses/>.
//---------------------------------------------------------------------------------

using System.Data;
using System.Threading;
using System.Windows.Forms;
using Habanero.Base.Exceptions;
using Habanero.BO.ClassDefinition;
using Habanero.BO;
using Habanero.Test;
using Habanero.UI.Base;
using Habanero.UI.Grid;
using NUnit.Framework;

namespace Habanero.Test.UI.Grid
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
            ClassDef classDef = MyBO.LoadClassDefWithNoLookup();
            BusinessObjectCollection<BusinessObject> col = new BusinessObjectCollection<BusinessObject>(classDef);
            bo1 = new MyBO();
            bo1.SetPropertyValue("TestProp", "Value1");
            bo1.SetPropertyValue("TestProp2", "Value2");
            bo2 = new MyBO();
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
            IBusinessObjectCollection cloneCol = grid.GetCollectionClone();
            Assert.AreEqual(cloneCol.Count,2 );
        }

        /// <summary>
        /// The following few tests monitor the sorting done in Gridbase based
        /// on the "sortColumn" attribute and apply equally to EditableGrid
        /// </summary>
        [Test]
        public void TestSortColumnAttributeDefault()
        {
            Assert.IsNull(grid.SortedColumn);
            Assert.AreEqual(SortOrder.None, grid.SortOrder);
        }

        [Test]
        public void TestSortColumnAttributeSuccess()
        {
            grid.SetCollection(grid.GetCollection(), "Success1");
            Assert.AreEqual("TestProp", grid.SortedColumn.Name);
            Assert.AreEqual(SortOrder.Ascending, grid.SortOrder);

            grid.SetCollection(grid.GetCollection(), "Success2");
            Assert.AreEqual("TestProp", grid.SortedColumn.Name);
            Assert.AreEqual(SortOrder.Ascending, grid.SortOrder);

            grid.SetCollection(grid.GetCollection(), "Success3");
            Assert.AreEqual("TestProp", grid.SortedColumn.Name);
            Assert.AreEqual(SortOrder.Descending, grid.SortOrder);

            grid.SetCollection(grid.GetCollection(), "Success4");
            Assert.AreEqual("TestProp", grid.SortedColumn.Name);
            Assert.AreEqual(SortOrder.Descending, grid.SortOrder);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortColumnAttributeExceptionColumnName()
        {
            grid.SetCollection(grid.GetCollection(), "Error1");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortColumnAttributeExceptionColumnNameAndOrder()
        {
            grid.SetCollection(grid.GetCollection(), "Error2");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortColumnAttributeExceptionOrder()
        {
            grid.SetCollection(grid.GetCollection(), "Error3");
        }
    }
}