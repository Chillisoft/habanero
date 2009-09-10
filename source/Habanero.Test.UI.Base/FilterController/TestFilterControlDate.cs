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
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    public abstract class TestFilterControlDate //:TestBase
    {
		protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestFilterControlDateWin : TestFilterControlDate
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
            //[Test]
            //public void TestLabelAndDateTimePickerAreOnPanel()
            //{
            //    TestLabelAndDateTimePickerAreOnPanel(2);
            //}
        }

        [TestFixture]
        public class TestFilterControlDateVWG : TestFilterControlDate
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }
            //[Test]
            //public void TestLabelAndDateTimePickerAreOnPanel()
            //{
            //    TestLabelAndDateTimePickerAreOnPanel(3);
            //}
        }
        
        [SetUp]
        public void SetupTest()
        {
            //Runs every time that any testmethod is executed
            //base.SetupTest();
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
            //base.TearDownTest();
        }

		[Test]
        public void TestAddDatePicker()
        {
            //---------------Set up test pack-------------------
			IControlFactory factory = GetControlFactory();
            DateTime testDate = DateTime.Now;
            IFilterControl filterControl = factory.CreateFilterControl();

            //---------------Execute Test ----------------------
            IControlHabanero dtPicker = filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", testDate,FilterClauseOperator.OpGreaterThan, true);

            //---------------Test Result -----------------------
            Assert.IsNotNull(dtPicker);
            Assert.IsTrue(dtPicker is IDateTimePicker);
        }

        [Test]
        public void TestAddDatePicker_NullDefaultValue()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            DateTime testDate = DateTime.Now;
            IFilterControl filterControl = factory.CreateFilterControl();

            //---------------Execute Test ----------------------
            IDateTimePicker dtPicker = filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", null, FilterClauseOperator.OpGreaterThan, true);

            //---------------Test Result -----------------------
            Assert.IsNotNull(dtPicker);
            Assert.AreEqual(null, dtPicker.ValueOrNull);
        }
        
        
        [Test]
        public void TestLabelAndDateTimePickerAreOnPanel()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Execute Test ----------------------
            DateTime testDate = DateTime.Today.AddDays(+4);
            IDateTimePicker dtePicker = filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpGreaterThan, false);


            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.Controls.Count);
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            //Assert.IsTrue(filterControl.FilterControls.Contains(dtePicker));
            Assert.AreEqual(2, filterControl.FilterPanel.Controls.Count);
            Assert.IsTrue(filterControl.FilterPanel.Controls.Contains(dtePicker));
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddDateFilterDateTimePicker_DefaultDateCorrect()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Today.AddDays(-2);
            //---------------Execute Test ----------------------
            filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpGreaterThan, false);
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpGreaterThan, new DateTime(testDate.Year, testDate.Month, testDate.Day));

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddDateFilterDateTimePicker_ChangeDateCorrect()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Today.AddDays(-2);
            IDateTimePicker dtePicker = filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpGreaterThan, false);
            //---------------Execute Test ----------------------
            DateTime newDateTime = DateTime.Today.AddDays(+4);
            dtePicker.Value = newDateTime;
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpGreaterThan, new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day));

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);

            //---------------Tear Down -------------------------          
        }
        
        //[Test] //No label is being created
        //public void TestAddDateFilterDateTimePicker_LabelTextCorrect()
        //{
        //    //---------------Set up test pack-------------------
        //    IControlFactory factory = GetControlFactory();
        //    IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
        //    IFilterControl filterControl = factory.CreateFilterControl();
        //    IDateTimePicker dtePicker = filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", DateTime.Now, FilterClauseOperator.OpGreaterThan, false);
        //    //---------------Execute Test ----------------------
        //    string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
        //    //---------------Test Result -----------------------
        //    Assert.IsTrue(filterControl.Controls[0] is ILabel);
        //    Assert.AreEqual("test:", ((ILabel)filterControl.Controls[0]).Text);
        //    //---------------Tear Down -------------------------          
        //    TestAddDatePicker();
        //}

        [Test]
        public void TestAddDateFilterDateTimePicker_EqualOperator()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Today.AddDays(-2);
            IDateTimePicker dtePicker = filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpEquals, false);
            //---------------Execute Test ----------------------
            DateTime newDateTime = DateTime.Today.AddDays(+4);
            dtePicker.Value = newDateTime;
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpEquals, new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day));

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddDateFilterDateTimePicker_LikeOperator()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Today.AddDays(-2);
            IDateTimePicker dtePicker = filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpGreaterThan, false);
            //---------------Execute Test ----------------------
            DateTime newDateTime = DateTime.Today.AddDays(+4);
            dtePicker.Value = newDateTime;
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpGreaterThan, new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day));
            filterControl.AddDateFilterDateTimePicker("test:", "testcolumn", testDate, FilterClauseOperator.OpGreaterThan, true);

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddDateFilterDateTimePicker_OpGreaterThanOrEqualToOperator()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Today.AddDays(-2);
            IDateTimePicker dtePicker = filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpGreaterThanOrEqualTo, false);
            //---------------Execute Test ----------------------
            DateTime newDateTime = DateTime.Today.AddDays(+4);
            dtePicker.Value = newDateTime;
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpGreaterThanOrEqualTo, new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day));

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddDateFilterDateTimePicker_OpLessThanOrEqualToOperator()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Today.AddDays(-2);
            IDateTimePicker dtePicker = filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpLessThanOrEqualTo, false);
            //---------------Execute Test ----------------------
            DateTime newDateTime = DateTime.Today.AddDays(+4);
            dtePicker.Value = newDateTime;
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpLessThanOrEqualTo, new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day));

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestAddDateFilterDateTimePicker_OpGreaterThanOperator()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Today.AddDays(-2);
            IDateTimePicker dtePicker = filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpGreaterThan, false);
            //---------------Execute Test ----------------------
            DateTime newDateTime = DateTime.Today.AddDays(+4);
            dtePicker.Value = newDateTime;
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpGreaterThan, new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day));

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);

            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestAddDateFilterDateTimePicker_OpLessThanOperator()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Today.AddDays(-2);
            IDateTimePicker dtePicker = filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpLessThan, false);
            //---------------Execute Test ----------------------
            DateTime newDateTime = DateTime.Today.AddDays(+4);
            dtePicker.Value = newDateTime;
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpLessThan, new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day));

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);

            //---------------Tear Down -------------------------          
        }
        
		[Test]
        public void TestAddDateFilterDateTimePicker_IgnoresTime()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate = DateTime.Now;
            filterControl.AddDateFilterDateTimePicker("test:", "TestColumn", testDate, FilterClauseOperator.OpLessThan, false);
            //---------------Execute Test ----------------------
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateDateFilterClause("TestColumn", FilterClauseOperator.OpLessThan, new DateTime(testDate.Year, testDate.Month, testDate.Day, 0, 0, 0, 0));

            Assert.AreEqual(clause.GetFilterClauseString(), expectedFilterClause);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddDateFilterDateTimePicker_Composites()
        {
            //---------------Set up test pack-------------------
            IControlFactory factory = GetControlFactory();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = factory.CreateFilterControl();
            DateTime testDate1 = DateTime.Now;
            DateTime testDate2 = testDate1.AddDays(1);
            filterControl.AddDateFilterDateTimePicker("test1:", "TestColumn1", testDate1, FilterClauseOperator.OpLessThan, false);
            filterControl.AddDateFilterDateTimePicker("test2:", "TestColumn2", testDate2, FilterClauseOperator.OpLessThan, false);
            
            //---------------Execute Test ----------------------
            string expectedFilterClause = filterControl.GetFilterClause().GetFilterClauseString();
            
            //---------------Test Result -----------------------
            IFilterClause clause1 =
                filterClauseFactory.CreateDateFilterClause("TestColumn1", FilterClauseOperator.OpLessThan, new DateTime(testDate1.Year, testDate1.Month, testDate1.Day));
            IFilterClause clause2 =
                filterClauseFactory.CreateDateFilterClause("TestColumn2", FilterClauseOperator.OpLessThan, new DateTime(testDate2.Year, testDate2.Month, testDate2.Day));
            IFilterClause compClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);

            Assert.AreEqual(compClause.GetFilterClauseString(), expectedFilterClause);

            //---------------Tear Down -------------------------          
        }

        //todo : TEST Nullable dtPicker

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
    }
}