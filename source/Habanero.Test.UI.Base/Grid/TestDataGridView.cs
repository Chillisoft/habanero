//---------------------------------------------------------------------------------
// Copyright (C) 2009 Chillisoft Solutions
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

using System.ComponentModel;
using System.Data;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.Grid
{
    /// <summary>
    /// This test class tests the base inherited methods of the DataGridView class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_DataGridView : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateDataGridView();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the DataGridView class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_DataGridView : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateDataGridView();
        }
    }

    /// <summary>
    /// This test class tests the DataGridView class.
    /// </summary>
    public abstract class TestDataGridView
    {
        protected abstract IControlFactory GetControlFactory();

        private IDataGridView CreateDataGridView()
        {
            return GetControlFactory().CreateDataGridView();
        }

        [TestFixture]
        public class TestDataGridViewWin : TestDataGridView
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            protected override string GetUnderlyingDataGridViewSelectionModeToString(IDataGridView dataGridView)
            {
                System.Windows.Forms.DataGridView control = (System.Windows.Forms.DataGridView) dataGridView;
                return control.SelectionMode.ToString();
            }

            protected override void AddToForm(IDataGridView dgv)
            {
                System.Windows.Forms.Form form = new System.Windows.Forms.Form();
                form.Controls.Add((System.Windows.Forms.Control) dgv);
            }
        }

        [TestFixture]
        public class TestDataGridViewVWG : TestDataGridView
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }

            protected override string GetUnderlyingDataGridViewSelectionModeToString(IDataGridView dataGridView)
            {
                Gizmox.WebGUI.Forms.DataGridView control = (Gizmox.WebGUI.Forms.DataGridView) dataGridView;
                return control.SelectionMode.ToString();
            }

            protected override void AddToForm(IDataGridView dgv)
            {
                Gizmox.WebGUI.Forms.Form form = new Gizmox.WebGUI.Forms.Form();
                form.Controls.Add((Gizmox.WebGUI.Forms.Control) dgv);
            }
        }


        [Test]
        public void TestCreateDataGridView()
        {
            //---------------Set up test pack-------------------
            //---------------Execute Test ----------------------
            IDataGridView dataGridView = GetControlFactory().CreateDataGridView();
            //---------------Test Result -----------------------
            Assert.IsNotNull(dataGridView);
            //---------------Tear Down -------------------------   
        }

        [Test]
        public void Test_Rows_Remove()
        {
            //---------------Set up test pack-------------------
            IDataGridView dataGridView = GetControlFactory().CreateDataGridView();
            AddToForm(dataGridView);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TestColumn");
            dataTable.Rows.Add("TestValue");
            dataTable.AcceptChanges();
            dataGridView.DataSource = dataTable;
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, dataTable.Rows.Count);
            Assert.AreEqual(2, dataGridView.Rows.Count);
            //---------------Execute Test ----------------------
            dataGridView.Rows.Remove(dataGridView.Rows[0]);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, dataGridView.Rows.Count);
        }

        public class TestObject
        {
            public string TestProperty { get; set; }
        }

        protected abstract string GetUnderlyingDataGridViewSelectionModeToString(IDataGridView dataGridView);

        protected void AssertDataGridViewSelectionModesSame(IDataGridView dataGridView)
        {
            DataGridViewSelectionMode DataGridViewSelectionMode = dataGridView.SelectionMode;
            string DataGridViewSelectionModeToString = GetUnderlyingDataGridViewSelectionModeToString(dataGridView);
            Assert.AreEqual(DataGridViewSelectionMode.ToString(), DataGridViewSelectionModeToString);
        }

        [Test]
        public virtual void TestConversion_DataGridViewSelectionMode_CellSelect()
        {
            //---------------Set up test pack-------------------
            IDataGridView control = GetControlFactory().CreateDataGridView();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //---------------Test Result -----------------------
            Assert.AreEqual(DataGridViewSelectionMode.CellSelect, control.SelectionMode);
            AssertDataGridViewSelectionModesSame(control);
        }

        [Test]
        public virtual void TestConversion_DataGridViewSelectionMode_FullRowSelect()
        {
            //---------------Set up test pack-------------------
            IDataGridView control = GetControlFactory().CreateDataGridView();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //---------------Test Result -----------------------
            Assert.AreEqual(DataGridViewSelectionMode.FullRowSelect, control.SelectionMode);
            AssertDataGridViewSelectionModesSame(control);
        }

        [Test]
        public virtual void TestConversion_DataGridViewSelectionMode_FullColumnSelect()
        {
            //---------------Set up test pack-------------------
            IDataGridView control = GetControlFactory().CreateDataGridView();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
            //---------------Test Result -----------------------
            Assert.AreEqual(DataGridViewSelectionMode.FullColumnSelect, control.SelectionMode);
            AssertDataGridViewSelectionModesSame(control);
        }

        [Test]
        public virtual void TestConversion_DataGridViewSelectionMode_RowHeaderSelect()
        {
            //---------------Set up test pack-------------------
            IDataGridView control = GetControlFactory().CreateDataGridView();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            //---------------Test Result -----------------------
            Assert.AreEqual(DataGridViewSelectionMode.RowHeaderSelect, control.SelectionMode);
            AssertDataGridViewSelectionModesSame(control);
        }

        [Test]
        public virtual void TestConversion_DataGridViewSelectionMode_ColumnHeaderSelect()
        {
            //---------------Set up test pack-------------------
            IDataGridView control = CreateDataGridView();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            //---------------Test Result -----------------------
            Assert.AreEqual(DataGridViewSelectionMode.ColumnHeaderSelect, control.SelectionMode);
            AssertDataGridViewSelectionModesSame(control);
        }

        [Test]
        public void Test_Sort_Ascending()
        {
            //---------------Set up test pack-------------------
            IDataGridView dataGridView = CreateDataGridViewWithTestColumn();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            dataGridView.Sort(dataGridView.Columns[0], ListSortDirection.Ascending);
            //---------------Test Result -----------------------
            string sortColumn = ((DataView) dataGridView.DataSource).Sort;
            Assert.AreEqual("TestColumn ASC", sortColumn);
        }

        [Test]
        public void Test_Sort_Descending()
        {
            //---------------Set up test pack-------------------
            IDataGridView dataGridView = CreateDataGridViewWithTestColumn();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            dataGridView.Sort(dataGridView.Columns[0], ListSortDirection.Descending);
            //---------------Test Result -----------------------
            string sortColumn = ((DataView) dataGridView.DataSource).Sort;
            Assert.AreEqual("TestColumn DESC", sortColumn);
        }

        [Test]
        public void Test_SortColumn_UnSorted()
        {
            //---------------Set up test pack-------------------
            IDataGridView dataGridView = CreateDataGridViewWithTestColumn();
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, dataGridView.Columns.Count);
            //---------------Execute Test ----------------------
            IDataGridViewColumn sortedColumn = dataGridView.SortedColumn;
            //---------------Test Result -----------------------
            Assert.IsNull(sortedColumn);
        }

        [Test]
        [Ignore("Need to investigate how to get this working")]
        //TODO Mark 04 Mar 2009: Ignored Test - Need to investigate how to get this working
        public void Test_SortColumn_Sorted()
        {
            //---------------Set up test pack-------------------
            IDataGridView dataGridView = CreateDataGridViewWithTestColumn();
            dataGridView.Sort(dataGridView.Columns[0], ListSortDirection.Ascending);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            IDataGridViewColumn sortedColumn = dataGridView.SortedColumn;
            //---------------Test Result -----------------------
            Assert.IsNotNull(sortedColumn);
            Assert.AreEqual("TestColumn", sortedColumn.Name);
        }

        private IDataGridView CreateDataGridViewWithTestColumn()
        {
            IDataGridView dataGridView = CreateDataGridView();
            dataGridView.Columns.Add("TestColumn", "Test Column");
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TestColumn");
            dataTable.AcceptChanges();
            dataGridView.DataSource = dataTable.DefaultView;
            return dataGridView;
        }

        protected abstract void AddToForm(IDataGridView dgv);
    }
}