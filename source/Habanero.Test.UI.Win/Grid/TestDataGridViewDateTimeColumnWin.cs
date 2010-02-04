using System;
using Habanero.Test.UI.Base;
using Habanero.UI;
using Habanero.UI.Base;
using Habanero.UI.Win;
using Habanero.Util;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Grid
{

    [TestFixture]
    public class TestDataGridViewDateTimeColumnWin : TestDataGridViewDateTimeColumn
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
            IDataGridViewDateTimeColumn createdColumn = GetControlFactory().CreateDataGridViewDateTimeColumn();
            //---------------Test Result -----------------------
            DataGridViewColumnWin columnWin = (DataGridViewColumnWin)createdColumn;
            System.Windows.Forms.DataGridViewColumn column = columnWin.DataGridViewColumn;
            Assert.IsInstanceOfType(typeof(DataGridViewDateTimeColumn), column);
            Assert.IsInstanceOfType(typeof(DataGridViewDateTimeColumnWin), createdColumn);
            Assert.IsTrue(typeof(DataGridViewDateTimeColumn).IsSubclassOf(typeof(System.Windows.Forms.DataGridViewColumn)));
        }

        [Test]
        public void Test_CellTemplateIsCalendarCell()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            DataGridViewDateTimeColumn dtColumn = new DataGridViewDateTimeColumn();
            //---------------Test Result -----------------------
            Assert.IsInstanceOfType(typeof(CalendarCell), dtColumn.CellTemplate);
        }

        [Test]
        public void Test_SetCellTemplate()
        {
            //---------------Set up test pack-------------------
            DataGridViewDateTimeColumn dtColumn = new DataGridViewDateTimeColumn();
            CalendarCell calendarCell = new CalendarCell();
            //---------------Assert Precondition----------------
            Assert.AreNotSame(calendarCell, dtColumn.CellTemplate);
            //---------------Execute Test ----------------------
            dtColumn.CellTemplate = calendarCell;

            //---------------Test Result -----------------------
            Assert.AreSame(calendarCell, dtColumn.CellTemplate);
        }

        [Test]
        public void Test_SetCellTemplate_MustBeCalendarCell()
        {
            //---------------Set up test pack-------------------
            DataGridViewDateTimeColumn dtColumn = new DataGridViewDateTimeColumn();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            bool errorThrown = false;
            try
            {
                dtColumn.CellTemplate = new System.Windows.Forms.DataGridViewCheckBoxCell();
            }
            catch (InvalidCastException) { errorThrown = true; }
            //---------------Test Result -----------------------
            Assert.IsTrue(errorThrown, "Cell Template must be type of CalendarCell");
        }

        [Test]
        public void TestCalendarCell_HasCorrectSettings()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            CalendarCell calendarCell = new CalendarCell();
            //---------------Test Result -----------------------
            Assert.AreEqual("d", calendarCell.Style.Format);
            Assert.AreEqual(typeof(CalendarEditingControl), calendarCell.EditType);
            Assert.AreEqual(typeof(DateTime), calendarCell.ValueType);
            Assert.IsInstanceOfType(typeof(DateTime), calendarCell.DefaultNewRowValue);

            DateTime newRowValue = (DateTime)calendarCell.DefaultNewRowValue;
            Assert.IsTrue(DateTimeUtilities.CloseToDateTimeNow(newRowValue, 10));
        }

        [Test]
        public void TestCalendarEditingControl_HasCorrectSettings()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            CalendarEditingControl editingControl = new CalendarEditingControl();
            //---------------Test Result -----------------------
            Assert.AreEqual(System.Windows.Forms.DateTimePickerFormat.Short, editingControl.Format);
            Assert.IsFalse(editingControl.RepositionEditingControlOnValueChange);
            Assert.AreEqual(0, editingControl.EditingControlRowIndex);
            Assert.IsNull(editingControl.EditingControlDataGridView);
            Assert.IsFalse(editingControl.EditingControlValueChanged);
        }

        [Test]
        public void TestCalendarEditingControl_EditingControlFormattedValue()
        {
            //---------------Set up test pack-------------------
            CalendarEditingControl editingControl = new CalendarEditingControl();
            //---------------Assert Precondition----------------
            Assert.AreEqual(DateTime.Now.ToShortDateString(), editingControl.EditingControlFormattedValue);
            //Assert.AreEqual(DateTime.Now.ToShortDateString(), editingControl.GetEditingControlFormattedValue(null));
            //---------------Execute Test ----------------------

            // REQUIRES A PARENT GRID FOR DIRTY NOTIFICATION, NOT WORTH THE TROUBLE?
            //DateTime dtValue = new DateTime(2006, 5, 1, 3, 2, 1);
            //editingControl.EditingControlFormattedValue = dtValue.ToString();
            ////---------------Test Result -----------------------
            //Assert.AreEqual("2006/05/01", editingControl.EditingControlFormattedValue);
        }
    }
}