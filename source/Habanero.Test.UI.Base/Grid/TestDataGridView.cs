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

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;
using DataGridViewSelectionMode=Habanero.UI.Base.DataGridViewSelectionMode;

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

        [TestFixture]
        public class TestDataGridViewWin : TestDataGridView
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            protected override string GetUnderlyingDataGridViewSelectionModeToString(IDataGridView dataGridView)
            {
                System.Windows.Forms.DataGridView control = (System.Windows.Forms.DataGridView)dataGridView;
                return control.SelectionMode.ToString();
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
                Gizmox.WebGUI.Forms.DataGridView control = (Gizmox.WebGUI.Forms.DataGridView)dataGridView;
                return control.SelectionMode.ToString();
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
        [Ignore("Can't seem to get the precondition passing  TODO - Mark 02 Feb 2009 ")]
        [Test]//TODO - Mark 02 Feb 2009 : Get this working
        public void Test_Rows_Remove()
        {
            //---------------Set up test pack-------------------
            IDataGridView dataGridView = GetControlFactory().CreateDataGridView();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TestColumn");
            dataTable.Rows.Add("TestValue");
            dataTable.AcceptChanges();
            dataGridView.DataSource = dataTable;
            //-------------Assert Preconditions -------------
            Assert.AreEqual(1, dataTable.Rows.Count);
            Assert.AreEqual(2, dataGridView.Rows.Count);
            //---------------Execute Test ----------------------
            dataGridView.Rows.Remove(dataGridView.Rows[1]);
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
            IDataGridView control = GetControlFactory().CreateDataGridView();
            //-------------Assert Preconditions -------------
            //---------------Execute Test ----------------------
            control.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            //---------------Test Result -----------------------
            Assert.AreEqual(DataGridViewSelectionMode.ColumnHeaderSelect, control.SelectionMode);
            AssertDataGridViewSelectionModesSame(control);
        }
    }
}
