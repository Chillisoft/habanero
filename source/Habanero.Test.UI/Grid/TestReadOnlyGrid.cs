//---------------------------------------------------------------------------------
// Copyright (C) 2008 Chillisoft Solutions
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
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Base.Exceptions;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
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
        private Form _form;
        private ReadOnlyGrid _grid;
        private BusinessObject _bo1;
        private BusinessObject _bo2;
        private DataTable _dataSource;

        [SetUp]
        public void SetupFixture()
        {
            _grid = new ReadOnlyGrid();
            _grid.Name = "GridControl";
            ClassDef.ClassDefs.Clear();
            ClassDef classDef = MyBO.LoadClassDefWithNoLookup();
            BusinessObjectCollection<BusinessObject> col = new BusinessObjectCollection<BusinessObject>(classDef);
            _bo1 = new MyBO();
            _bo1.SetPropertyValue("TestProp", "Value1");
            _bo1.SetPropertyValue("TestProp2", "Value2");
            _bo2 = new MyBO();
            _bo2.SetPropertyValue("TestProp", "2Value1");
            _bo2.SetPropertyValue("TestProp2", "2Value2");
            col.Add(_bo1);
            col.Add(_bo2);
            _grid.SetCollection(col);
            _form = new Form();
            _grid.Dock = DockStyle.Fill;
            _form.Controls.Add(_grid);
            _form.Show();
            _dataSource = _grid.DataTable;
        }

        [TearDown]
        public void TearDown()
        {
            _form.Close();
            _form.Dispose();
        }

        #region Test Selection


        [Test]
        public void TestGetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            BusinessObject selectedBo = _grid.SelectedBusinessObject;
            //---------------Test Result -----------------------
            Assert.AreEqual("Value1", selectedBo.Props["TestProp"].Value);
            Assert.AreEqual("Value2", selectedBo.Props["TestProp2"].Value);
            Assert.AreSame(_bo1, selectedBo);
            Assert.AreEqual(0, _grid.CurrentRow.Index);
        }

        [Test]
        public void TestSetSelectedBusinessObject()
        {
            //---------------Set up test pack-------------------

            //---------------Execute Test ----------------------
            _grid.SelectedBusinessObject = _bo1;

            //---------------Test Result -----------------------
            Assert.AreEqual(_bo1, _grid.SelectedBusinessObject);
        }

        [Test]
        public void TestSetSelectedBusinessObject_ToNull()
        {
            //---------------Set up test pack-------------------
            
            //---------------Execute Test ----------------------
            _grid.SelectedBusinessObject = _bo1;
            _grid.SelectedBusinessObject = null;

            //---------------Test Result -----------------------
            Assert.IsNull(_grid.SelectedBusinessObject);
            Assert.IsNull(_grid.CurrentRow);
        }

        [Test]
        public void TestReadOnlyGridFiringItemSelected()
        {
            //---------------Set up test pack-------------------
            bool gridItemSelected = false;
            _grid.SelectedBusinessObject = null;
            _grid.AddItemSelectedDelegate(delegate
            {
                gridItemSelected = true;
            });

            //---------------Execute Test ----------------------
            _grid.SelectedBusinessObject = _bo1;

            //---------------Test Result -----------------------
            Assert.IsTrue(gridItemSelected);
        }


        #endregion //Test Selection


        [Test]
        public void TestRowIsRefreshed()
        {
            _bo2.SetPropertyValue("TestProp", "UpdatedValue");
            Assert.AreEqual("UpdatedValue", _dataSource.Rows[1][1]);
        }

        [Test]
        public void TestGetCollectionClone()
        {
            IBusinessObjectCollection cloneCol = _grid.GetCollectionClone();
            Assert.AreEqual(cloneCol.Count,2 );
        }

        /// <summary>
        /// The following few tests monitor the sorting done in Gridbase based
        /// on the "sortColumn" attribute and apply equally to EditableGrid
        /// </summary>
        [Test]
        public void TestSortColumnAttributeDefault()
        {
            Assert.IsNull(_grid.SortedColumn);
            Assert.AreEqual(SortOrder.None, _grid.SortOrder);
        }

        [Test]
        public void TestSortColumnAttributeSuccess()
        {
            _grid.SetCollection(_grid.GetCollection(), "Success1");
            Assert.AreEqual("TestProp", _grid.SortedColumn.Name);
            Assert.AreEqual(SortOrder.Ascending, _grid.SortOrder);

            _grid.SetCollection(_grid.GetCollection(), "Success2");
            Assert.AreEqual("TestProp", _grid.SortedColumn.Name);
            Assert.AreEqual(SortOrder.Ascending, _grid.SortOrder);

            _grid.SetCollection(_grid.GetCollection(), "Success3");
            Assert.AreEqual("TestProp", _grid.SortedColumn.Name);
            Assert.AreEqual(SortOrder.Descending, _grid.SortOrder);

            _grid.SetCollection(_grid.GetCollection(), "Success4");
            Assert.AreEqual("TestProp", _grid.SortedColumn.Name);
            Assert.AreEqual(SortOrder.Descending, _grid.SortOrder);
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortColumnAttributeExceptionColumnName()
        {
            _grid.SetCollection(_grid.GetCollection(), "Error1");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortColumnAttributeExceptionColumnNameAndOrder()
        {
            _grid.SetCollection(_grid.GetCollection(), "Error2");
        }

        [Test, ExpectedException(typeof(InvalidXmlDefinitionException))]
        public void TestSortColumnAttributeExceptionOrder()
        {
            _grid.SetCollection(_grid.GetCollection(), "Error3");
        }


        

        
    }
}