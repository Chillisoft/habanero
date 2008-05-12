using System;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.Base.FilterControl;
using Habanero.UI.WebGUI;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    [TestFixture]
    public class TestFilterControlDate //:TestBase
    {
        [SetUp]
        public
            void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
        }

        [TestFixtureSetUp]
        public
            void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public
            void TearDownTest()
        {
            //runs every time any testmethod is complete
            //base.TearDownTest();
        }


        //[Test]
        //public void TestAddDateRangeFilterComboBoxInclusive()
        //{
        //    DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);
        //    DateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", true, true);
        //    dr1.UseFixedNowDate = true;
        //    dr1.FixedNowDate = testDate;
        //    dr1.SelectedItem = "Today";

        //    IFilterClause clause1 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThanOrEqualTo, new DateTime(2007, 1, 2, 0, 0, 0));
        //    IFilterClause clause2 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThanOrEqualTo, new DateTime(2007, 1, 2, 3, 4, 5));
        //    //IFilterClause clause1 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpGreaterThanOrEqualTo, "2007/01/02 12:00:00 AM");
        //    //IFilterClause clause2 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpLessThanOrEqualTo, "2007/01/02 03:04:05 AM");
        //    IFilterClause compClause = itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
        //    Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        //}

        //[Test]
        //public void TestAddDateRangeFilterComboBoxExclusive()
        //{
        //    DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);
        //    DateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", false, false);
        //    dr1.UseFixedNowDate = true;
        //    dr1.FixedNowDate = testDate;
        //    dr1.SelectedItem = "Today";

        //    IFilterClause clause1 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThan, new DateTime(2007, 1, 2, 0, 0, 0));
        //    IFilterClause clause2 = itsFilterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThan, new DateTime(2007, 1, 2, 3, 4, 5));
        //    //IFilterClause clause1 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpGreaterThan, "2007/01/02 12:00:00 AM");
        //    //IFilterClause clause2 = itsFilterClauseFactory.CreateStringFilterClause("test", FilterClauseOperator.OpLessThan, "2007/01/02 03:04:05 AM");
        //    IFilterClause compClause = itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
        //    Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        //}

        //[Test]
        //public void TestAddDateRangeFilterComboBoxCollectionOverload()
        //{
        //    List<DateRangeComboBox.DateOptions> options = new List<DateRangeComboBox.DateOptions>();
        //    options.Add(DateRangeComboBox.DateOptions.Today);
        //    options.Add(DateRangeComboBox.DateOptions.Yesterday);
        //    DateRangeComboBox testDRComboBox2 = filterControl.AddDateRangeFilterComboBox("test", "test", options, true, false);
        //    Assert.AreEqual(2, testDRComboBox2.OptionsToDisplay.Count);
        //}



//---------------------------==============================================-------------------------------------------
        //[Test]
        //public void TestAddDateFilterDateTimePicker()
        //{
        //    DateTime testDate = DateTime.Now;
        //    filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", testDate, FilterClauseOperator.OpGreaterThan, true);
        //    filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", testDate, FilterClauseOperator.OpEquals, false);
        //    IFilterClause clause1 = itsFilterClauseFactory.CreateDateFilterClause("testcolumn", FilterClauseOperator.OpGreaterThan, new DateTime(testDate.Year, testDate.Month, testDate.Day));
        //    IFilterClause clause2 = itsFilterClauseFactory.CreateDateFilterClause("testcolumn", FilterClauseOperator.OpEquals, testDate);
        //    IFilterClause compClause =
        //        itsFilterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
        //    Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        //}

        [Test]
        public void TestAddDatePickerWinForms()
        {
            TestAddDatePicker(new WinControlFactory());
        }
        [Test]
        public void TestAddDatePickerGiz()
        {
            TestAddDatePicker(new GizmoxControlFactory());
        }

        public void TestAddDatePicker(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            DateTime testDate = DateTime.Now;
            IFilterControl filterControl = factory.CreateFilterControl();

            //---------------Execute Test ----------------------
            IChilliControl dtPicker = filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", testDate,FilterClauseOperator.OpGreaterThan,true, true);

            //---------------Test Result -----------------------
            Assert.IsNotNull(dtPicker);
            Assert.IsTrue(dtPicker is IDateTimePicker);
        }
        [Test]
        public void TestAddDateFilterDateTimePickerGiz()
        {
            TestAddDateFilterDateTimePicker(new GizmoxControlFactory());
        }
        [Test]
        public void TestAddDateFilterDateTimePickerWinForms()
        {
            TestAddDateFilterDateTimePicker(new WinControlFactory());
        }
        public void TestAddDateFilterDateTimePicker(IControlFactory factory)
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Today.AddDays(-2);
            //---------------Execute Test ----------------------
            filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpGreaterThan, true, false);
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpGreaterThan, new DateTime(testDate.Year, testDate.Month, testDate.Day));

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);

            //---------------Tear Down -------------------------          
        }
        //todo : TEST Nullable dtPicker
    }
}