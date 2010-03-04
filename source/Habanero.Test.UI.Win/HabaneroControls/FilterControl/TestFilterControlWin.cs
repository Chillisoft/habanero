using System;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.Test.UI.Base.FilterController;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestFilterControlWin : TestFilterControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void Test_FilterModeHidesButtonPanel()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            //---------------Execute Test ----------------------
            IFilterControl ctl = factory.CreateFilterControl();
            //---------------Test Result -----------------------
            Button filterButton = (Button)ctl.FilterButton;
            Assert.IsFalse(filterButton.Parent.Visible);
            //Assert.IsFalse(ctl.ClearButton.Visible);
            //---------------Tear Down ------------------------- 
        }

        [Test]
        public void Test_SetFilterModeSearch_MakesButtonPanelVisible()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            Control buttonControl = ((Button)ctl.FilterButton).Parent;

            //---------------Assert Preconditions --------------
            Assert.IsFalse(buttonControl.Visible);
            //---------------Execute Test ----------------------
            ctl.FilterMode = FilterModes.Search;
            //---------------Test Result -----------------------
            Assert.IsTrue(buttonControl.Visible);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void Test_SetFilterModeSearchSetsText()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            //---------------Assert Preconditions --------------
            Assert.AreEqual("Filter", ctl.FilterButton.Text);
            //---------------Execute Test ----------------------
            ctl.FilterMode = FilterModes.Search;
            //---------------Test Result -----------------------
            Assert.AreEqual("Search", ctl.FilterButton.Text);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestChangeTextBoxValueAppliesFilter()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            ITextBox textBox = ctl.AddStringFilterTextBox("test", "propname");
            string text = TestUtil.GetRandomString();

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            textBox.Text = text;
            //---------------Test Result -----------------------
            Assert.IsTrue(filterFired, "The filter event should have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeComboBoxTextAppliesFilter()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            string[] optionList = { "one", "two" };
            IComboBox comboBox = ctl.AddStringFilterComboBox("test", "propname", optionList, true);
            string text = TestUtil.GetRandomString();

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            comboBox.Text = text;
            //---------------Test Result -----------------------
            Assert.IsTrue(filterFired, "The filter event should have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeComboBoxIndexChangeAppliesFilter()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            string[] optionList = { "one", "oneone" };
            IComboBox comboBox = ctl.AddStringFilterComboBox("test", "propname", optionList, true);
            comboBox.Text = optionList[0];

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.AreEqual(1, comboBox.SelectedIndex);
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 2;
            //---------------Test Result -----------------------
            Assert.IsTrue(filterFired, "The filter event should have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeCheckBoxAppliesFilter()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            ICheckBox checkBox = ctl.AddBooleanFilterCheckBox("test", "propname", false);

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            checkBox.Checked = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(filterFired, "The filter event should have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeDateTimePickerAppliesFilter()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            IDateTimePicker dateTimePicker = ctl.AddDateFilterDateTimePicker("test", "propname", DateTime.Now, FilterClauseOperator.OpLessThan, true);

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            dateTimePicker.Value = DateTime.Now.AddMonths(-1);
            //---------------Test Result -----------------------
            Assert.IsTrue(filterFired, "The filter event should have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeDateRangeComboBoxAppliesFilter()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            IDateRangeComboBox dateRangeComboBox = ctl.AddDateRangeFilterComboBox("test", "propname", true, true);
            string text = TestUtil.GetRandomString();

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            dateRangeComboBox.Text = text;
            //---------------Test Result -----------------------
            Assert.IsTrue(filterFired, "The filter event should have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeTextBoxValueDoesNotApplyFilter_InSearchMode()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            ctl.FilterMode = FilterModes.Search;
            ITextBox textBox = ctl.AddStringFilterTextBox("test", "propname");
            string text = TestUtil.GetRandomString();

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.AreEqual(FilterModes.Search, ctl.FilterMode);
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            textBox.Text = text;
            //---------------Test Result -----------------------
            Assert.IsFalse(filterFired, "The filter event should not have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeComboBoxTextDoesNotApplyFilter_InSearchMode()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            ctl.FilterMode = FilterModes.Search;
            string[] optionList = { "one", "two" };
            IComboBox comboBox = ctl.AddStringFilterComboBox("test", "propname", optionList, true);
            string text = TestUtil.GetRandomString();

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.AreEqual(FilterModes.Search, ctl.FilterMode);
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            comboBox.Text = text;
            //---------------Test Result -----------------------
            Assert.IsFalse(filterFired, "The filter event should not have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeComboBoxIndexChangeDoesNotApplyFilter_InSearchMode()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            ctl.FilterMode = FilterModes.Search;
            string[] optionList = { "one", "oneone" };
            IComboBox comboBox = ctl.AddStringFilterComboBox("test", "propname", optionList, true);
            comboBox.Text = optionList[0];

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.AreEqual(FilterModes.Search, ctl.FilterMode);
            Assert.AreEqual(1, comboBox.SelectedIndex);
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 2;
            //---------------Test Result -----------------------
            Assert.IsFalse(filterFired, "The filter event should not have been fired when the text was changed.");
        }


        [Test]
        public void TestChangeCheckBoxDoesNotApplyFilter_InSearchMode()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            ctl.FilterMode = FilterModes.Search;
            ICheckBox checkBox = ctl.AddBooleanFilterCheckBox("test", "propname", false);

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.AreEqual(FilterModes.Search, ctl.FilterMode);
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            checkBox.Checked = true;
            //---------------Test Result -----------------------
            Assert.IsFalse(filterFired, "The filter event should not have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeDateTimePickerDoesNotApplyFilter_InSearchMode()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            ctl.FilterMode = FilterModes.Search;
            IDateTimePicker dateTimePicker = ctl.AddDateFilterDateTimePicker("test", "propname", DateTime.Now, FilterClauseOperator.OpLessThan, true);

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.AreEqual(FilterModes.Search, ctl.FilterMode);
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            dateTimePicker.Value = DateTime.Now.AddMonths(-1);
            //---------------Test Result -----------------------
            Assert.IsFalse(filterFired, "The filter event should not have been fired when the text was changed.");
        }

        [Test]
        public void TestChangeDateRangeComboBoxDoesNotApplyFilter_InSearchMode()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterControl ctl = factory.CreateFilterControl();
            ctl.FilterMode = FilterModes.Search;
            IDateRangeComboBox dateRangeComboBox = ctl.AddDateRangeFilterComboBox("test", "propname", true, true);
            string text = TestUtil.GetRandomString();

            bool filterFired = false;
            ctl.Filter += delegate { filterFired = true; };
            //---------------Assert Preconditions --------------
            Assert.AreEqual(FilterModes.Search, ctl.FilterMode);
            Assert.IsFalse(filterFired);
            //---------------Execute Test ----------------------
            dateRangeComboBox.Text = text;
            //---------------Test Result -----------------------
            Assert.IsFalse(filterFired, "The filter event should not have been fired when the text was changed.");
        }

        //
        //        [Test]
        //        public void TestAddStringFilterTextBoxTextChanged()
        //        {
        //            itsIsFilterClauseChanged = false;
        //            filterControl.SetAutomaticUpdate(true);
        //            filterControl.FilterClauseChanged += FilterClauseChangedHandler;
        //            TextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
        //            Assert.IsTrue(itsIsFilterClauseChanged, "Adding a new control should make the filter clause change");
        //            itsIsFilterClauseChanged = false;
        //            tb.Text = "change";
        //            Assert.IsTrue(itsIsFilterClauseChanged, "Changing the text should make the filter clause change");
        //        }
        //
        //        private void FilterClauseChangedHandler(object sender, FilterControlEventArgs e)
        //        {
        //            itsIsFilterClauseChanged = true;
        //        }
        //
        //
        //        [Test]
        //        public void TestAddStringFilterComboBoxTextChanged()
        //        {
        //            IList options = new ArrayList();
        //            options.Add("1");
        //            options.Add("2");
        //            itsIsFilterClauseChanged = false;
        //            filterControl.FilterClauseChanged += FilterClauseChangedHandler;
        //            ComboBox cb = filterControl.AddStringFilterComboBox("Test:", "TestColumn", options, true);
        //            Assert.IsTrue(itsIsFilterClauseChanged, "Adding a new control should make the filter clause change");
        //            itsIsFilterClauseChanged = false;
        //            cb.SelectedIndex = 0;
        //            Assert.IsTrue(itsIsFilterClauseChanged, "Changing the selected item should make the filter clause change");
        //        }
        //

        //
        //        [Test]
        //        public void TestAddStringFilterDateTimeEditor()
        //        {
        //            DateTime testDate = DateTime.Now;
        //            filterControl.AddStringFilterDateTimeEditor("test:", "testcolumn", testDate, true);
        //            filterControl.AddStringFilterDateTimeEditor("test:", "testcolumn", testDate, false);
        //            IFilterClause clause1 =
        //                itsFilterClauseFactory.CreateStringFilterClause("testcolumn", FilterClauseOperator.OpGreaterThanOrEqualTo, testDate.ToString("yyyy/MM/dd"));
        //            IFilterClause clause2 =
        //                itsFilterClauseFactory.CreateStringFilterClause("testcolumn", FilterClauseOperator.OpLessThanOrEqualTo, testDate.ToString("yyyy/MM/dd"));
        //            IFilterClause compClause =
        //                itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
        //            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        //        }
        //
        //        [Test]
        //        public void TestAddDateFilterDateTimePicker()
        //        {
        //            DateTime testDate = DateTime.Now;
        //            filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", testDate, FilterClauseOperator.OpGreaterThan, true);
        //            filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", testDate, FilterClauseOperator.OpEquals, false);
        //            IFilterClause clause1 = itsFilterClauseFactory.CreateDateFilterClause("testcolumn", FilterClauseOperator.OpGreaterThan, new DateTime(testDate.Year, testDate.Month, testDate.Day));
        //            IFilterClause clause2 = itsFilterClauseFactory.CreateDateFilterClause("testcolumn", FilterClauseOperator.OpEquals, testDate);
        //            IFilterClause compClause =
        //                itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
        //            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        //        }
        //
    }
}