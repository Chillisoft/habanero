using System;
using System.Collections;
using System.Windows.Forms;
using Habanero.Generic;
using Habanero.Ui.Generic;
using NUnit.Framework;

namespace Habanero.Test.Ui.Generic
{
    /// <summary>
    /// Summary description for TestFilterPanel.
    /// </summary>
    [TestFixture]
    public class TestFilterControl
    {
        FilterClauseFactory itsFilterClauseFactory;
        private bool itsIsFilterClauseChanged;
        FilterControl filterControl;
        FilterClause nullClause;

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
            FilterClause clause =
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
            filterControl.FilterClauseChanged += new EventHandler<FilterControlEventArgs>(FilterClauseChangedHandler);
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
            ComboBox cb = filterControl.AddStringFilterComboBox("t", "TestColumn", options);
            cb.SelectedIndex = 1;
            cb.SelectAll();
            FilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "1");
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
            filterControl.FilterClauseChanged += new EventHandler<FilterControlEventArgs>(FilterClauseChangedHandler);
            ComboBox cb = filterControl.AddStringFilterComboBox("Test:", "TestColumn", options);
            Assert.IsTrue(itsIsFilterClauseChanged, "Adding a new control should make the filter clause change");
            itsIsFilterClauseChanged = false;
            cb.SelectedIndex = 0;
            Assert.IsTrue(itsIsFilterClauseChanged, "Changing the selected item should make the filter clause change");
        }

        [Test]
        public void TestAddBooleanFilterCheckBox()
        {
            CheckBox cb = filterControl.AddStringFilterCheckBox("Test?", "TestColumn", true);
            FilterClause clause =
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
            DateTimePicker dtp1 = filterControl.AddStringFilterDateTimeEditor("test:", "testcolumn", testDate, true);
            DateTimePicker dtp2 = filterControl.AddStringFilterDateTimeEditor("test:", "testcolumn", testDate, false);
            FilterClause clause1 =
                itsFilterClauseFactory.CreateStringFilterClause("testcolumn", FilterClauseOperator.OpGreaterThanOrEqualTo, testDate.ToString("yyyy/MM/dd"));
            FilterClause clause2 =
                itsFilterClauseFactory.CreateStringFilterClause("testcolumn", FilterClauseOperator.OpLessThanOrEqualTo, testDate.ToString("yyyy/MM/dd"));
            FilterClause compClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestMultipleFilters()
        {
            TextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "testvalue";
            FilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");

            TextBox tb2 = filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            tb2.Text = "testvalue2";
            FilterClause clause2 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn2", FilterClauseOperator.OpLike, "testvalue2");

            FilterClause compositeClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(clause, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(compositeClause.GetFilterClauseString(),
                            filterControl.GetFilterClause().GetFilterClauseString());
        }
    }
}
