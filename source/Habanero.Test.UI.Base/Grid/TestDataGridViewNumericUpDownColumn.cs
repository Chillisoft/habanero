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

using System;
using Habanero.UI;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    public abstract class TestDataGridViewNumericUpDownColumn
    {
        [SetUp]
        public void SetupTest()
        {
            //ClassDef.ClassDefs.Clear();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public void TearDownTest()
        {
        }

        protected abstract IControlFactory GetControlFactory();


        [TestFixture]
        public class TestDataGridViewNumericUpDownColumnWin : TestDataGridViewNumericUpDownColumn
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }

            [Test]
            public void Test_CreateNewColumn_CorrectType()
            {
                //---------------Set up test pack-------------------
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                IDataGridViewNumericUpDownColumn createdColumn = GetControlFactory().CreateDataGridViewNumericUpDownColumn();
                //---------------Test Result -----------------------
                DataGridViewColumnWin columnWin = (DataGridViewColumnWin)createdColumn;
                System.Windows.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
                Assert.IsInstanceOfType(typeof(DataGridViewNumericUpDownColumn), column);
                Assert.IsInstanceOfType(typeof(DataGridViewNumericUpDownColumnWin), createdColumn);
                Assert.IsTrue(typeof(DataGridViewNumericUpDownColumn).IsSubclassOf(typeof(System.Windows.Forms.DataGridViewColumn)));
            }

            [Test]
            public void Test_CellTemplateIsNumericUpDownCell()
            {
                //---------------Set up test pack-------------------
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                DataGridViewNumericUpDownColumn dtColumn = new DataGridViewNumericUpDownColumn();
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof(NumericUpDownCell), dtColumn.CellTemplate);
            }

            [Test]
            public void Test_SetCellTemplate()
            {
                //---------------Set up test pack-------------------
                DataGridViewNumericUpDownColumn dtColumn = new DataGridViewNumericUpDownColumn();
                NumericUpDownCell NumericUpDownCell = new NumericUpDownCell();
                //---------------Assert Precondition----------------
                Assert.AreNotSame(NumericUpDownCell, dtColumn.CellTemplate);
                //---------------Execute Test ----------------------
                dtColumn.CellTemplate = NumericUpDownCell;

                //---------------Test Result -----------------------
                Assert.AreSame(NumericUpDownCell, dtColumn.CellTemplate);
            }

            [Test]
            public void Test_SetCellTemplate_MustBeNumericUpDownCell()
            {
                //---------------Set up test pack-------------------
                DataGridViewNumericUpDownColumn dtColumn = new DataGridViewNumericUpDownColumn();
                //---------------Assert Precondition----------------

                //---------------Execute Test ----------------------
                bool errorThrown = false;
                try
                {
                    dtColumn.CellTemplate = new System.Windows.Forms.DataGridViewCheckBoxCell();
                }
                catch (InvalidCastException) { errorThrown = true; }
                //---------------Test Result -----------------------
                Assert.IsTrue(errorThrown, "Cell Template must be type of NumericUpDownCell");
            }

            [Test]
            public void TestNumericUpDownCell_HasCorrectSettings()
            {
                //---------------Set up test pack-------------------
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                NumericUpDownCell numericUpDownCell = new NumericUpDownCell();
                //---------------Test Result -----------------------
                Assert.AreEqual("0.00", numericUpDownCell.Style.Format);
                Assert.AreEqual(typeof(NumericUpDownEditingControl), numericUpDownCell.EditType);
                Assert.AreEqual(typeof(Decimal), numericUpDownCell.ValueType);
                Assert.IsInstanceOfType(typeof(Decimal), numericUpDownCell.DefaultNewRowValue);

                Decimal newRowValue = (Decimal)numericUpDownCell.DefaultNewRowValue;
                Assert.AreEqual(0D, newRowValue);
            }

            [Test]
            public void TestNumericUpDownEditingControl_HasCorrectSettings()
            {
                //---------------Set up test pack-------------------
                //---------------Assert Precondition----------------
                //---------------Execute Test ----------------------
                NumericUpDownEditingControl editingControl = new NumericUpDownEditingControl();
                //---------------Test Result -----------------------
                Assert.AreEqual(2, editingControl.DecimalPlaces);
                Assert.IsFalse(editingControl.RepositionEditingControlOnValueChange);
                Assert.AreEqual(0, editingControl.EditingControlRowIndex);
                Assert.IsNull(editingControl.EditingControlDataGridView);
                Assert.IsFalse(editingControl.EditingControlValueChanged);
            }

            [Test]
            public void TestNumericUpDownEditingControl_EditingControlFormattedValue()
            {
                //---------------Set up test pack-------------------
                NumericUpDownEditingControl editingControl = new NumericUpDownEditingControl();
                //---------------Assert Precondition----------------
                string defaultValueString = 0D.ToString("0.00");
                Assert.AreEqual(defaultValueString, editingControl.EditingControlFormattedValue);
                //Assert.AreEqual(defaultValueString, editingControl.GetEditingControlFormattedValue(null));
                //---------------Execute Test ----------------------

                // REQUIRES A PARENT GRID FOR DIRTY NOTIFICATION, NOT WORTH THE TROUBLE?
                //Decimal dtValue = 12.345;
                //editingControl.EditingControlFormattedValue = dtValue.ToString();
                ////---------------Test Result -----------------------
                //Assert.AreEqual(dtValue.ToString("0.00"), editingControl.EditingControlFormattedValue);
            }
        }

        //TODO: look at creating a NumericUpDown column for VWG
        //[TestFixture]
        //public class TestDataGridViewNumericUpDownColumnVWG : TestDataGridViewNumericUpDownColumn
        //{
        //    protected override IControlFactory GetControlFactory()
        //    {
        //        return new ControlFactoryVWG();
        //    }
        //}
    }
}
