using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Gizmox;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    [TestFixture]
    public class TestFilterControl
    {
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
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
            //runs every time any testmethod is complete
        }


        [Test]
        public void TestAddTextBoxGizmox()
        {
            TestAddTextBox(new GizmoxControlFactory());
        }

        [Test]
        public void TestAddTestBoxWinForms()
        {
            TestAddTextBox(new WinControlFactory());
        }

        public void TestAddTextBox(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterControl ctl = factory.CreateFilterControl();

            //---------------Execute Test ----------------------
            ITextBox myTextBox = ctl.AddStringFilterTextBox("", "");

            //---------------Test Result -----------------------
            Assert.IsNotNull(myTextBox);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestAddStringFilterTextBoxWinForms()
        {
            TestAddStringFilterTextBox(new WinControlFactory());
        }

        [Test]
        public void TestAddStringFilterTextBoxGiz()
        {
            TestAddStringFilterTextBox(new GizmoxControlFactory());
        }

        public void TestAddStringFilterTextBox(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = factory.CreateFilterControl();
            //---------------Execute Test ----------------------
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "";
            //---------------Test Result -----------------------
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetTextBoxFilterClauseWinForms()
        {
            TestGetTextBoxFilterClause(new WinControlFactory());
        }

        [Test]
        public void TestGetTextBoxFilterClauseGiz()
        {
            TestGetTextBoxFilterClause(new GizmoxControlFactory());
        }

        public void TestGetTextBoxFilterClause(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");

            //---------------Execute Test ----------------------
            tb.Text = "testvalue";
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestTwoStringTextBoxFilterWinForms()
        {
            TestTwoStringTextBoxFilter(new WinControlFactory());
        }

        [Test]
        public void TestTwoStringTextBoxFilterGiz()
        {
            TestTwoStringTextBoxFilter(new GizmoxControlFactory());
        }

        public void TestTwoStringTextBoxFilter(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "testvalue";
            ITextBox tb2 = filterControl.AddStringFilterTextBox("Test:", "TestColumn2");
            tb2.Text = "testvalue2";

            //---------------Execute Test ----------------------
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause1 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");
            IFilterClause clause2 =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn2", FilterClauseOperator.OpLike, "testvalue2");
            IFilterClause fullClause =
                itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            Assert.AreEqual(fullClause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }


        [Test]
        public void TestLabelAndTextBoxAreOnPanelWinForms()
        {
            TestLabelAndTextBoxAreOnPanel(new WinControlFactory());
        }

        [Test]
        public void TestLabelAndTextBoxAreOnPanelGiz()
        {
            TestLabelAndTextBoxAreOnPanel(new GizmoxControlFactory());
        }

        public void TestLabelAndTextBoxAreOnPanel(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = factory.CreateFilterControl();

            //---------------Execute Test ----------------------
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");

            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.Controls.Count);
            Assert.Contains(tb, filterControl.Controls);
            //---------------Tear Down -------------------------          
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
//        [Test]
//        public void TestAddStringFilterComboBox()
//        {
//            IList options = new ArrayList();
//            options.Add("1");
//            options.Add("2");
//            ComboBox cb = filterControl.AddStringFilterComboBox("t", "TestColumn", options, true);
//            cb.SelectedIndex = 1;
//            cb.SelectAll();
//            IFilterClause clause =
//                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "1");
//            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
//            cb.SelectedIndex = -1;
//            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
//
//            cb = filterControl.AddStringFilterComboBox("t", "TestColumn", options, false);
//            cb.SelectedIndex = 1;
//            cb.SelectAll();
//            clause =
//                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "1");
//            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
//            cb.SelectedIndex = -1;
//            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
//        }
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
//        [Test]
//        public void TestAddBooleanFilterCheckBox()
//        {
//            CheckBox cb = filterControl.AddStringFilterCheckBox("Test?", "TestColumn", true);
//            IFilterClause clause =
//                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "true");
//            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
//            cb.Checked = false;
//            clause =
//                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "false");
//            Assert.AreEqual(clause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
//        }
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
//        [Test]
//        public void TestAddDateRangeFilterComboBoxInclusive()
//        {
//            DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);
//            DateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", true, true);
//            dr1.UseFixedNowDate = true;
//            dr1.FixedNowDate = testDate;
//            dr1.SelectedItem = "Today";
//
//            IFilterClause clause1 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThanOrEqualTo, new DateTime(2007, 1, 2, 0, 0, 0));
//            IFilterClause clause2 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThanOrEqualTo, new DateTime(2007, 1, 2, 3, 4, 5));
//            //IFilterClause clause1 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpGreaterThanOrEqualTo, "2007/01/02 12:00:00 AM");
//            //IFilterClause clause2 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpLessThanOrEqualTo, "2007/01/02 03:04:05 AM");
//            IFilterClause compClause = itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
//            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
//        }
//
//        [Test]
//        public void TestAddDateRangeFilterComboBoxExclusive()
//        {
//            DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);
//            DateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", false, false);
//            dr1.UseFixedNowDate = true;
//            dr1.FixedNowDate = testDate;
//            dr1.SelectedItem = "Today";
//
//            IFilterClause clause1 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThan, new DateTime(2007, 1, 2, 0, 0, 0));
//            IFilterClause clause2 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThan, new DateTime(2007, 1, 2, 3, 4, 5));
//            //IFilterClause clause1 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpGreaterThan, "2007/01/02 12:00:00 AM");
//            //IFilterClause clause2 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpLessThan, "2007/01/02 03:04:05 AM");
//            IFilterClause compClause = itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
//            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
//        }
//
//        [Test]
//        public void TestAddDateRangeFilterComboBoxCollectionOverload()
//        {
//            List<DateRangeComboBox.DateOptions> options = new List<DateRangeComboBox.DateOptions>();
//            options.Add(DateRangeComboBox.DateOptions.Today);
//            options.Add(DateRangeComboBox.DateOptions.Yesterday);
//            DateRangeComboBox testDRComboBox2 = filterControl.AddDateRangeFilterComboBox("test", "test", options, true, false);
//            Assert.AreEqual(2, testDRComboBox2.OptionsToDisplay.Count);
//        }
//
//        [Test]
//        public void TestMultipleFilters()
//        {
//            TextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
//            tb.Text = "testvalue";
//            IFilterClause clause =
//                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");
//
//            TextBox tb2 = filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
//            tb2.Text = "testvalue2";
//            IFilterClause clause2 =
//                itsFilterClauseFactory.CreateStringFilterClause("TestColumn2", FilterClauseOperator.OpLike, "testvalue2");
//
//            IFilterClause compositeClause =
//                itsFilterClauseFactory.CreateCompositeFilterClause(clause, FilterClauseCompositeOperator.OpAnd, clause2);
//            Assert.AreEqual(compositeClause.GetFilterClauseString(),
//                            filterControl.GetFilterClause().GetFilterClauseString());
//        }

//
    }
}