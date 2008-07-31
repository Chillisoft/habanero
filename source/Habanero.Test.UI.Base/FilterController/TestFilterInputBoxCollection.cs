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

//TODO: Ensure that these are working.

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Windows.Forms;
//using Habanero.Base;
//using Habanero.UI;
//using Habanero.UI.Base.FilterControl;
//using Habanero.UI.Forms;
//using Habanero.UI.Grid;
//using NUnit.Framework;

//namespace Habanero.Test.UI.Grid
//{
//    /// <summary>
//    /// Summary description for TestFilterInputBoxCollection.
//    /// </summary>
//    [TestFixture]
//    public class TestFilterInputBoxCollection
//    {
//        [Test]
//        public void TestNumberOfControls()
//        {
//            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());
//            Label testLabel = filterInputBoxCol.AddLabel("Test");
//            Assert.AreEqual(1, filterInputBoxCol.GetControls().Count);

//            TextBox testTextBox = filterInputBoxCol.AddStringFilterTextBox("a");
//            Assert.AreEqual(2, filterInputBoxCol.GetControls().Count);

//            DateTimePicker testDateTimePicker = filterInputBoxCol.AddStringFilterDateTimeEditor("test", null, true);
//            Assert.AreEqual(3, filterInputBoxCol.GetControls().Count);

//            DateTimePicker testDateTimePicker2 = filterInputBoxCol.AddDateFilterDateTimePicker("test", null, FilterClauseOperator.OpGreaterThan, false);
//            Assert.AreEqual(4, filterInputBoxCol.GetControls().Count);

//            CheckBox testCheckBox = filterInputBoxCol.AddBooleanFilterCheckBox("test", "textbox", true);
//            Assert.AreEqual(5, filterInputBoxCol.GetControls().Count);
            
//            ComboBox testComboBox = filterInputBoxCol.AddStringFilterComboBox("test", new ArrayList(), true);
//            Assert.AreEqual(6, filterInputBoxCol.GetControls().Count);

//            DateRangeComboBox testDRComboBox = filterInputBoxCol.AddDateRangeFilterComboBox("test", true, false);
//            Assert.AreEqual(7, filterInputBoxCol.GetControls().Count);

//            List<DateRangeComboBox.DateOptions> options = new List<DateRangeComboBox.DateOptions>();
//            options.Add(DateRangeComboBox.DateOptions.Today);
//            options.Add(DateRangeComboBox.DateOptions.Yesterday);
//            DateRangeComboBox testDRComboBox2 = filterInputBoxCol.AddDateRangeFilterComboBox("test", options, true, false);
//            Assert.AreEqual(8, filterInputBoxCol.GetControls().Count);
//            Assert.AreEqual(2, testDRComboBox2.OptionsToDisplay.Count);
            
//            Assert.AreSame(testLabel, filterInputBoxCol.GetControls()[0]);
//            Assert.AreSame(testTextBox, filterInputBoxCol.GetControls()[1]);
//            Assert.AreSame(testDateTimePicker, filterInputBoxCol.GetControls()[2]);
//            Assert.AreSame(testDateTimePicker2, filterInputBoxCol.GetControls()[3]);
//            Assert.AreSame(testCheckBox, filterInputBoxCol.GetControls()[4]);
//            Assert.AreSame(testComboBox, filterInputBoxCol.GetControls()[5]);
//            Assert.AreSame(testDRComboBox, filterInputBoxCol.GetControls()[6]);
//            Assert.AreSame(testDRComboBox2, filterInputBoxCol.GetControls()[7]);
//        }

//        [Test]
//        public void TestWidth()
//        {
//            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());
//            Assert.AreEqual(new TextBox().Width, filterInputBoxCol.GetFilterWidth());

//            filterInputBoxCol.SetFilterWidth(1000);
//            TextBox testTextBox = filterInputBoxCol.AddStringFilterTextBox("test");
//            Assert.AreEqual(1000, testTextBox.Width);
//        }

//        [Test]
//        public void TestDefaultValuesAndEnabled()
//        {
//            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());
//            Label testLabel = filterInputBoxCol.AddLabel("Test");
//            Assert.AreEqual("Test", testLabel.Text);

//            DateTime testTime = DateTime.Now;
//            DateTimePicker testDateTimePicker = filterInputBoxCol.AddStringFilterDateTimeEditor("test", testTime, true);
//            Assert.AreEqual(testTime, testDateTimePicker.Value);

//            DateTimePicker testDateTimePicker2 = filterInputBoxCol.AddDateFilterDateTimePicker("test", testTime, FilterClauseOperator.OpGreaterThan, false);
//            Assert.AreEqual(testTime, testDateTimePicker2.Value);

//            CheckBox testCheckBox = filterInputBoxCol.AddBooleanFilterCheckBox("test", "textbox", true);
//            Assert.AreEqual(true, testCheckBox.Checked);

//            ArrayList testOptions = new ArrayList();
//            testOptions.Add("testoption1");
//            ComboBox testComboBox = filterInputBoxCol.AddStringFilterComboBox("test", testOptions, true);
//            Assert.AreEqual("", testComboBox.Items[0]);
//            Assert.AreEqual("testoption1", testComboBox.Items[1]);

//            // DateRangeComboBox testDRCB = filterInputBoxCol.AddDateRangeFilterComboBox("test", true, false);
//            // default options already tested under TestDateRangeComboBox

//            filterInputBoxCol.DisableControls();
//            Assert.IsFalse(testLabel.Enabled);
//            Assert.IsFalse(testComboBox.Enabled);

//            filterInputBoxCol.EnableControls();
//            Assert.IsTrue(testLabel.Enabled);
//            Assert.IsTrue(testComboBox.Enabled);
//        }

//        [Test]
//        public void TestFilterClauses()
//        {
//            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());

//            TextBox testTextBox = filterInputBoxCol.AddStringFilterTextBox("test0");
//            testTextBox.Text = "testvalue";

//            DateTime testTime = DateTime.Now;
//            filterInputBoxCol.AddStringFilterDateTimeEditor("test1", testTime, true);

//            filterInputBoxCol.AddBooleanFilterCheckBox("test2", "textbox", true);
            
//            ArrayList testOptions = new ArrayList();
//            testOptions.Add("testoption1");
//            filterInputBoxCol.AddStringFilterComboBox("test3", testOptions, true);
//            ComboBox testComboBox2 = filterInputBoxCol.AddStringFilterComboBox("test4", testOptions, true);
//            testComboBox2.SelectedItem = "testoption1";

//            DateRangeComboBox testDRCB = filterInputBoxCol.AddDateRangeFilterComboBox("test5", true, false);
//            testDRCB.UseFixedNowDate = true;
//            testDRCB.FixedNowDate = new DateTime(2007, 1, 2, 3, 4, 5, 6);
//            testDRCB.SelectedItem = "Today";

//            string testFilterClause = "((((test0 like '*testvalue*')" +
//                                      " and (test1 >= '" + testTime.ToString("yyyy/MM/dd") + "'))" +
//                                      " and (test2 = 'true'))" +
//                                      " and (test4 = 'testoption1'))" +
//                                      " and ((test5 >= #02 Jan 2007 00:00:00#)" +
//                                      " and (test5 < #02 Jan 2007 03:04:05#))";
//            Assert.AreEqual(testFilterClause, filterInputBoxCol.GetFilterClause().GetFilterClauseString());
//        }

//        [Test]
//        public void TestDateFilters()
//        {
//            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());

//            DateTime testTime = DateTime.Now;
//            filterInputBoxCol.AddDateFilterDateTimePicker("test0", testTime, FilterClauseOperator.OpGreaterThan, false);
//            filterInputBoxCol.AddDateFilterDateTimePicker("test1", testTime, FilterClauseOperator.OpEquals, true);

//            string testFilterClause = "(test0 > #" + testTime.ToString("dd MMM yyyy HH:mm:ss") + "#)" +
//                                      " and (test1 = #" + testTime.ToString("dd MMM yyyy 00:00:00") + "#)";
//            Assert.AreEqual(testFilterClause, filterInputBoxCol.GetFilterClause().GetFilterClauseString());
//        }

//        [Test]
//        public void TestAutoUpdate()
//        {
//            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());
//            Assert.IsTrue(filterInputBoxCol.GetAutomaticUpdate());
//            filterInputBoxCol.SetAutomaticUpdate(false);
//            Assert.IsFalse(filterInputBoxCol.GetAutomaticUpdate());
//        }
//    }
//}