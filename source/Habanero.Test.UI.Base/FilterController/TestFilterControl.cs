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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Habanero.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base.FilterController
{
    public abstract class TestFilterControl
    {
        protected abstract IControlFactory GetControlFactory();

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
        public void TestSetLayoutManager()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IPanel panel = GetControlFactory().CreatePanel();
            GridLayoutManager layoutManager = new GridLayoutManager(panel, GetControlFactory());
            //---------------Execute Test ----------------------
            filterControl.LayoutManager = layoutManager;
            //---------------Test Result -----------------------
            Assert.AreEqual(layoutManager, filterControl.LayoutManager);
            Assert.IsNotNull(filterControl.FilterPanel);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void MultipleFilters()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "testvalue";
            IFilterClause clause =
                filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpLike, "testvalue");

            ITextBox tb2 = filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            tb2.Text = "testvalue2";
            //---------------Execute Test ----------------------

            string filterClause = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause2 =
                filterClauseFactory.CreateStringFilterClause("TestColumn2", FilterClauseOperator.OpLike, "testvalue2");

            IFilterClause compositeClause =
                filterClauseFactory.CreateCompositeFilterClause(clause, FilterClauseCompositeOperator.OpAnd, clause2);

            Assert.AreEqual(compositeClause.GetFilterClauseString(),
                            filterClause);
            //---------------Tear Down ------------------------- 
        }

        [Test]
        public void TestAdd_DateRangeFilterComboBox()
        {
            //---------------Set up test pack-------------------

            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            IDateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", true, true);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.CountOfFilters);
            Assert.IsTrue(filterControl.FilterPanel.Controls.Contains(dr1));
        }

        [Test]
        public void TestAdd_DateRangeFilterComboBoxInclusive()
        {
            //---------------Set up test pack-------------------

            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);

            //---------------Execute Test ----------------------
            IDateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", true, true);
            dr1.UseFixedNowDate = true;
            dr1.FixedNowDate = testDate;
            dr1.SelectedItem = "Today";
            IFilterClause clause1 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThanOrEqualTo,
                                                           new DateTime(2007, 1, 2, 0, 0, 0));
            IFilterClause clause2 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThanOrEqualTo,
                                                           new DateTime(2007, 1, 2, 3, 4, 5));
            IFilterClause compClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            //---------------Test Result -----------------------

            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAdd_DateRangeFilterComboBoxExclusive()
        {
            //---------------Set up test pack-------------------

            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            DateTime testDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);

            //---------------Execute Test ----------------------
            IDateRangeComboBox dr1 = filterControl.AddDateRangeFilterComboBox("test", "test", false, false);
            dr1.UseFixedNowDate = true;
            dr1.FixedNowDate = testDate;
            dr1.SelectedItem = "Today";
            IFilterClause clause1 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpGreaterThan,
                                                           new DateTime(2007, 1, 2, 0, 0, 0));
            IFilterClause clause2 =
                filterClauseFactory.CreateDateFilterClause("test", FilterClauseOperator.OpLessThan,
                                                           new DateTime(2007, 1, 2, 3, 4, 5));
            IFilterClause compClause =
                filterClauseFactory.CreateCompositeFilterClause(clause1, FilterClauseCompositeOperator.OpAnd, clause2);
            //---------------Test Result -----------------------

            Assert.AreEqual(compClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAdd_DateRangeFilterComboBoxOverload()
        {
            //---------------Set up test pack-------------------

            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            List<DateRangeOptions> options = new List<DateRangeOptions>();
            options.Add(DateRangeOptions.Today);
            options.Add(DateRangeOptions.Yesterday);

            //---------------Execute Test ----------------------
            IDateRangeComboBox dateRangeCombo =
                filterControl.AddDateRangeFilterComboBox("test", "test", options, true, false);
            //---------------Test Result -----------------------
            Assert.AreEqual(3, dateRangeCombo.Items.Count);
        }


        private static IComboBox GetFilterComboBox_2Items(IFilterControl filterControl)
        {
            IList options = new ArrayList();
            options.Add("1");
            options.Add("2");
            return filterControl.AddStringFilterComboBox("Test:", "TestColumn", options, true);
        }

        #region TextBoxFilter

        #region TestAddTextBox

        [Test]
        public void TestAddTextBox()
        {
            //---------------Set up test pack-------------------
            IFilterControl ctl = GetControlFactory().CreateFilterControl();

            //---------------Execute Test ----------------------
            ITextBox myTextBox = ctl.AddStringFilterTextBox("", "");

            //---------------Test Result -----------------------
            Assert.IsNotNull(myTextBox);

            //---------------Tear Down -------------------------          
        }

        #endregion

        #region TestAddStringFilterTextBox

        [Test]
        public void TestAddStringFilterTextBox()
        {
            //---------------Set up test pack-------------------
            IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            tb.Text = "";
            //---------------Test Result -----------------------
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());
            Assert.AreEqual(1, filterControl.FilterControls.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Test Result -----------------------
            Assert.AreEqual(2, filterControl.FilterControls.Count);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox_GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tbExpected = filterControl.AddStringFilterTextBox("Test:", "TestColumn");
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Execute Test ----------------------
            ITextBox tbReturned = (ITextBox) filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(tbExpected, tbReturned);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox_Combo_GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox tbExpected = filterControl.AddStringFilterComboBox("Test:", "TestColumn", new string[] {""}, false);
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Execute Test ----------------------
            IComboBox tbReturned = (IComboBox) filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(tbExpected, tbReturned);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox_DateTime__GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IDateTimePicker tbExpected =
                filterControl.AddDateFilterDateTimePicker("Test:", "TestColumn", DateTime.Now,
                                                          FilterClauseOperator.OpEquals, false);
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Execute Test ----------------------
            IDateTimePicker tbReturned = (IDateTimePicker) filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(tbExpected, tbReturned);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox_CheckBox__GetControl()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ICheckBox tbExpected = filterControl.AddBooleanFilterCheckBox("Test:", "TestColumn", false);
            filterControl.AddStringFilterTextBox("Test2:", "TestColumn2");
            //---------------Execute Test ----------------------
            ICheckBox tbReturned = (ICheckBox) filterControl.GetChildControl("TestColumn");
            //---------------Test Result -----------------------
            Assert.AreSame(tbExpected, tbReturned);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAdd_TwoStringFilterTextBox_CheckBox()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Execute Test ----------------------
            ICheckBox cb = filterControl.AddBooleanFilterCheckBox("Test:", "TestColumn", false);

            //---------------Test Result -----------------------
            Assert.AreEqual(1, filterControl.FilterPanel.Controls.Count);
            Assert.AreSame(cb, filterControl.FilterPanel.Controls[0]);
            //---------------Tear Down -------------------------          
        }

        #endregion

        [Test]
        public void TestGetTextBoxFilterClause()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
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
        public void TestGetTextBoxFilterClause_Equals()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn", FilterClauseOperator.OpEquals);

            //---------------Execute Test ----------------------
            tb.Text = "testvalue";
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause =
                itsFilterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "testvalue");
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestTwoStringTextBoxFilter()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory itsFilterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
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
        public void TestLabelAndTextBoxAreOnPanel()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();

            //---------------Assert Preconditions --------------
            Assert.AreEqual(0, filterControl.FilterPanel.Controls.Count);

            //---------------Execute Test ----------------------
            ITextBox tb = filterControl.AddStringFilterTextBox("Test:", "TestColumn");

            //---------------Test Result -----------------------

            Assert.AreEqual(2, filterControl.FilterPanel.Controls.Count);
            Assert.IsTrue(filterControl.FilterPanel.Controls.Contains(tb));
            //---------------Tear Down -------------------------          
        }
        #endregion

        #region ComboBoxFilter

        //------------------------COMBO BOX----------------------------------------------------------

        [Test]
        public void TestAddComboBox()
        {
            //---------------Set up test pack-------------------
            //IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            IComboBox cb = filterControl.AddStringFilterComboBox("t", "TestColumn", new ArrayList(), true);

            //---------------Test Result -----------------------
            Assert.IsNotNull(cb);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestAddStringFilterComboBox()
        {
            //---------------Set up test pack-------------------
            IFilterClause nullClause = new DataViewNullFilterClause();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            //---------------Execute Test ----------------------
            filterControl.AddStringFilterComboBox("Test:", "TestColumn", new ArrayList(), true);
            //---------------Test Result -----------------------
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterControl.GetFilterClause().GetFilterClauseString());

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetComboBoxAddSelectedItems()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IList options = new ArrayList();
            options.Add("1");
            options.Add("2");
            //---------------Execute Test ----------------------
            IComboBox comboBox = filterControl.AddStringFilterComboBox("Test:", "TestColumn", options, true);
            //---------------Test Result -----------------------
            int numOfItemsInCollection = 2;
            int numItemsExpectedInComboBox = numOfItemsInCollection + 1; //one extra for the null selected item
            Assert.AreEqual(numItemsExpectedInComboBox, comboBox.Items.Count);
        }

        [Test]
        public void TestSelectItem()
        {
            //---------------Set up test pack-------------------
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IList options = new ArrayList();
            options.Add("1");
            options.Add("2");
            IComboBox comboBox = filterControl.AddStringFilterComboBox("Test:", "TestColumn", options, true);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 1;
            //---------------Test Result -----------------------
            Assert.AreEqual("1", comboBox.SelectedItem.ToString());
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetComboBoxFilterClause()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox comboBox = GetFilterComboBox_2Items(filterControl);

            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 1;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();

            //---------------Test Result -----------------------
            IFilterClause clause =
                filterClauseFactory.CreateStringFilterClause("TestColumn", FilterClauseOperator.OpEquals, "1");
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);

            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetComboBoxFilterClauseNoSelection()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox comboBox = GetFilterComboBox_2Items(filterControl);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = -1;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause clause = filterClauseFactory.CreateNullFilterClause();
            Assert.AreEqual(clause.GetFilterClauseString(), filterClauseString);
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestGetComboBoxFilterClause_SelectDeselect()
        {
            //---------------Set up test pack-------------------
            IFilterClauseFactory filterClauseFactory = new DataViewFilterClauseFactory();
            IFilterControl filterControl = GetControlFactory().CreateFilterControl();
            IComboBox comboBox = GetFilterComboBox_2Items(filterControl);
            //---------------Execute Test ----------------------
            comboBox.SelectedIndex = 1;
            comboBox.SelectedIndex = -1;
            string filterClauseString = filterControl.GetFilterClause().GetFilterClauseString();
            //---------------Test Result -----------------------
            IFilterClause nullClause = filterClauseFactory.CreateNullFilterClause();
            Assert.AreEqual(nullClause.GetFilterClauseString(), filterClauseString);
            //---------------Tear Down -------------------------          
        }


        #endregion

        #region Nested type: TestFilterControlVWG

        [TestFixture]
        public class TestFilterControlVWG : TestFilterControl
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }


            [Test]
            public void Test_DefaultLayoutManager()
            {
                //---------------Set up test pack-------------------
                IControlFactory factory = GetControlFactory();

                //---------------Execute Test ----------------------
                //            IControlHabanero control = factory.CreatePanel();
                IFilterControl ctl = factory.CreateFilterControl();
                //---------------Test Result -----------------------
                Assert.IsInstanceOfType(typeof (FlowLayoutManager), ctl.LayoutManager);
            }

            [Test]
            public void Test_SetFilterHeader()
            {
                //---------------Set up test pack-------------------
                IFilterControl ctl = GetControlFactory().CreateFilterControl();
                //---------------Assert Preconditions---------------
                Assert.AreEqual("Filter the Grid", ctl.HeaderText);
                //---------------Execute Test ----------------------
                ctl.HeaderText = "Filter Assets";

                //---------------Test Result -----------------------
                Assert.AreEqual("Filter Assets", ctl.HeaderText);

                //---------------Tear Down -------------------------          
            }

            [Test]
            public void Test_SetFilterModeFilterSetsText()
            {
                //---------------Set up test pack-------------------
                IControlFactory factory = GetControlFactory();
                IFilterControl ctl = factory.CreateFilterControl();
                ctl.FilterMode = FilterModes.Search;
                //---------------Assert Preconditions --------------
                Assert.AreEqual("Search", ctl.FilterButton.Text);
                //---------------Execute Test ----------------------
                ctl.FilterMode = FilterModes.Filter;
                //---------------Test Result -----------------------
                Assert.AreEqual("Filter", ctl.FilterButton.Text);
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
            public void TestClearButtonAccessor()
            {
                //---------------Set up test pack-------------------

                //---------------Execute Test ----------------------
                IFilterControl filterControl = GetControlFactory().CreateFilterControl();

                //---------------Test Result -----------------------
                Assert.IsNotNull(filterControl.ClearButton);
                //---------------Tear Down -------------------------
            }

            [Test]
            public void TestFilterButtonAccessor()
            {
                //---------------Set up test pack-------------------

                //---------------Execute Test ----------------------
                IFilterControl filterControl = GetControlFactory().CreateFilterControl();

                //---------------Test Result -----------------------
                Assert.IsNotNull(filterControl.FilterButton);
                //---------------Tear Down -------------------------
            }
        }

        #endregion

        #region Nested type: TestFilterControlWin

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
                Button filterButton = (Button) ctl.FilterButton;
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
                Control buttonControl = ((Button) ctl.FilterButton).Parent;

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
                string text = TestUtil.CreateRandomString();

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
                string text = TestUtil.CreateRandomString();

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
                string text = TestUtil.CreateRandomString();

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
                string text = TestUtil.CreateRandomString();

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
                string text = TestUtil.CreateRandomString();

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
                string text = TestUtil.CreateRandomString();

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

        #endregion


    }
}