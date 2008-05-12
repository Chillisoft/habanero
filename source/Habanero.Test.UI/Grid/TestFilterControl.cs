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


using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI;
using Habanero.UI.Forms;
using Habanero.UI.Grid;
using NUnit.Framework;

namespace Habanero.Test.UI.Grid
{
    /// <summary>
    /// Summary description for TestFilterPanel.
    /// </summary>
    [TestFixture]
    public class TestFilterControl
    {
        IFilterClauseFactory itsFilterClauseFactory;
        private bool itsIsFilterClauseChanged;
        FilterControl filterControl;
        IFilterClause nullClause;

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            itsFilterClauseFactory = new DataViewFilterClauseFactory();
            nullClause = new DataViewNullFilterClause();
        }

        [SetUp]
        public void Setup()
        {
            itsIsFilterClauseChanged = false;
            filterControl = new FilterControl(itsFilterClauseFactory);
        }

        [Test]
        public void TestAddStringFilterTextBox()
        {
            TextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "testvalue";
            IFilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");
            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
            tb.Text = "";
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAddStringFilterTextBoxTextChanged()
        {
            itsIsFilterClauseChanged = false;
            filterControl.SetAutomaticUpdate(true);
            filterControl.FilterClauseChanged += FilterClauseChangedHandler;
            TextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            Assert.IsTrue(itsIsFilterClauseChanged, "Adding a new control should make the filter clause change");
            itsIsFilterClauseChanged = false;
            tb.Text = "change";
            Assert.IsTrue(itsIsFilterClauseChanged, "Changing the text should make the filter clause change");
        }

        private void FilterClauseChangedHandler(object sender, FilterControlEventArgs e)
        {
            itsIsFilterClauseChanged = true;
        }

        [Test]
        public void TestAddStringFilterComboBox()
        {
            IList options = new ArrayList();
            options.Add("1");
            options.Add("2");
            ComboBox cb = filterControl.AddStringFilterComboBox("t", "TestColumn", options, true);
            cb.SelectedIndex = 1;
            cb.SelectAll();
            IFilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "1");
            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
            cb.SelectedIndex = -1;
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());

            cb = filterControl.AddStringFilterComboBox("t", "TestColumn", options, false);
            cb.SelectedIndex = 1;
            cb.SelectAll();
            clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "1");
            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
            cb.SelectedIndex = -1;
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAddStringFilterComboBoxTextChanged()
        {
            IList options = new ArrayList();
            options.Add("1");
            options.Add("2");
            itsIsFilterClauseChanged = false;
            filterControl.FilterClauseChanged += FilterClauseChangedHandler;
            ComboBox cb = filterControl.AddStringFilterComboBox("Test:", "TestColumn", options, true);
            Assert.IsTrue(itsIsFilterClauseChanged, "Adding a new control should make the filter clause change");
            itsIsFilterClauseChanged = false;
            cb.SelectedIndex = 0;
            Assert.IsTrue(itsIsFilterClauseChanged, "Changing the selected item should make the filter clause change");
        }

        [Test]
        public void TestAddBooleanFilterCheckBox()
        {
            CheckBox cb = filterControl.AddStringFilterCheckBox("Test?", "TestColumn", true);
            IFilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "true");
            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
            cb.Checked = false;
            clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "false");
            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAddStringFilterDateTimeEditor()
        {
            DateTime testDate = DateTime.Now;
            filterControl.AddStringFilterDateTimeEditor("test:", "testcolumn", testDate, true);
            filterControl.AddStringFilterDateTimeEditor("test:", "testcolumn", testDate, false);
            IFilterClause clause1 =
                itsFilterClauseFactory.CreateStringFilterClause("testcolumn", FilterClauseOperator.OpGreaterThanOrEqualTo, testDate.ToString("yyyy/MM/dd"));
            IFilterClause clause2 =
                itsFilterClauseFactory.CreateStringFilterClause("testcolumn", FilterClauseOperator.OpLessThanOrEqualTo, testDate.ToString("yyyy/MM/dd"));
            IFilterClause compClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAddDateFilterDateTimePicker()
        {
            DateTime testDate = DateTime.Now;
            filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", testDate, FilterClauseOperator.OpGreaterThan, true);
            filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", testDate, FilterClauseOperator.OpEquals, false);
            IFilterClause clause1 = itsFilterClauseFactory.CreateDateFilterClause("testcolumn", FilterClauseOperator.OpGreaterThan, new DateTime(testDate.Year, testDate.Month, testDate.Day));
            IFilterClause clause2 = itsFilterClauseFactory.CreateDateFilterClause("testcolumn", FilterClauseOperator.OpEquals, testDate);
            IFilterClause compClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAddDateRangeFilterComboBoxInclusive()
        {
            DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);
            DateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", true, true);
            dr1.UseFixedNowDate = true;
            dr1.FixedNowDate = testDate;
            dr1.SelectedItem = "Today";

            IFilterClause clause1 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThanOrEqualTo, new DateTime(2007, 1, 2, 0, 0, 0));
            IFilterClause clause2 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThanOrEqualTo, new DateTime(2007, 1, 2, 3, 4, 5));
            //IFilterClause clause1 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpGreaterThanOrEqualTo, "2007/01/02 12:00:00 AM");
            //IFilterClause clause2 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpLessThanOrEqualTo, "2007/01/02 03:04:05 AM");
            IFilterClause compClause = itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAddDateRangeFilterComboBoxExclusive()
        {
            DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);
            DateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", false, false);
            dr1.UseFixedNowDate = true;
            dr1.FixedNowDate = testDate;
            dr1.SelectedItem = "Today";

            IFilterClause clause1 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThan, new DateTime(2007, 1, 2, 0, 0, 0));
            IFilterClause clause2 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThan, new DateTime(2007, 1, 2, 3, 4, 5));
            //IFilterClause clause1 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpGreaterThan, "2007/01/02 12:00:00 AM");
            //IFilterClause clause2 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpLessThan, "2007/01/02 03:04:05 AM");
            IFilterClause compClause = itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAddDateRangeFilterComboBoxCollectionOverload()
        {
            List<DateRangeComboBox.DateOptions> options = new List<DateRangeComboBox.DateOptions>();
            options.Add(DateRangeComboBox.DateOptions.Today);
            options.Add(DateRangeComboBox.DateOptions.Yesterday);
            DateRangeComboBox testDRComboBox2 = filterControl.AddDateRangeFilterComboBox("test", "test", options, true, false);
            Assert.AreEqual(2, testDRComboBox2.OptionsToDisplay.Count);
        }

        [Test]
        public void TestMultipleFilters()
        {
            TextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "testvalue";
            IFilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");

            TextBox tb2 = filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            tb2.Text = "testvalue2";
            IFilterClause clause2 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn2", FilterClauseOperator.OpLike, "testvalue2");

            IFilterClause compositeClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(clause, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(compositeClause.GetFilterClauseString(),
                            filterControl.GetFilterClause().GetFilterClauseString());
        }
    }
}