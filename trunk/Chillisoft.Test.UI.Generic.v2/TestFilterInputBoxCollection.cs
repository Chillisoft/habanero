using System.Windows.Forms;
using Chillisoft.UI.Generic.v2;
using NUnit.Framework;
using System;
using System.Collections;

namespace Chillisoft.Test.UI.Generic.v2
{
    /// <summary>
    /// Summary description for TestFilterInputBoxCollection.
    /// </summary>
    [TestFixture]
    public class TestFilterInputBoxCollection
    {
        [Test]
        public void TestNumberOfControls()
        {
            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());
            Label testLabel = filterInputBoxCol.AddLabel("Test");
            Assert.AreEqual(1, filterInputBoxCol.GetControls().Count);

            TextBox testTextBox = filterInputBoxCol.AddStringFilterTextBox("a");
            Assert.AreEqual(2, filterInputBoxCol.GetControls().Count);

            DateTimePicker testDateTimePicker = filterInputBoxCol.AddStringFilterDateTimeEditor("test", null, true);
            Assert.AreEqual(3, filterInputBoxCol.GetControls().Count);

            CheckBox testCheckBox = filterInputBoxCol.AddBooleanFilterCheckBox("test", "textbox", true);
            Assert.AreEqual(4, filterInputBoxCol.GetControls().Count);
            
            ComboBox testComboBox = filterInputBoxCol.AddStringFilterComboBox("test", new ArrayList());
            Assert.AreEqual(5, filterInputBoxCol.GetControls().Count);
            
            Assert.AreSame(testLabel, filterInputBoxCol.GetControls()[0]);
            Assert.AreSame(testTextBox, filterInputBoxCol.GetControls()[1]);
            Assert.AreSame(testDateTimePicker, filterInputBoxCol.GetControls()[2]);
            Assert.AreSame(testCheckBox, filterInputBoxCol.GetControls()[3]);
            Assert.AreSame(testComboBox, filterInputBoxCol.GetControls()[4]);
        }

        [Test]
        public void TestWidth()
        {
            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());
            Assert.AreEqual(new TextBox().Width, filterInputBoxCol.GetFilterWidth());

            filterInputBoxCol.SetFilterWidth(1000);
            TextBox testTextBox = filterInputBoxCol.AddStringFilterTextBox("test");
            Assert.AreEqual(1000, testTextBox.Width);
        }

        [Test]
        public void TestDefaultValuesAndEnabled()
        {
            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());
            Label testLabel = filterInputBoxCol.AddLabel("Test");
            Assert.AreEqual("Test", testLabel.Text);

            DateTime testTime = DateTime.Now;
            DateTimePicker testDateTimePicker = filterInputBoxCol.AddStringFilterDateTimeEditor("test", testTime, true);
            Assert.AreEqual(testTime, testDateTimePicker.Value);

            CheckBox testCheckBox = filterInputBoxCol.AddBooleanFilterCheckBox("test", "textbox", true);
            Assert.AreEqual(true, testCheckBox.Checked);

            ArrayList testOptions = new ArrayList();
            testOptions.Add("testoption1");
            ComboBox testComboBox = filterInputBoxCol.AddStringFilterComboBox("test", testOptions);
            Assert.AreEqual("", testComboBox.Items[0]);
            Assert.AreEqual("testoption1", testComboBox.Items[1]);

            filterInputBoxCol.DisableControls();
            Assert.IsFalse(testLabel.Enabled);
            Assert.IsFalse(testComboBox.Enabled);

            filterInputBoxCol.EnableControls();
            Assert.IsTrue(testLabel.Enabled);
            Assert.IsTrue(testComboBox.Enabled);
        }

        [Test]
        public void TestFilterClauses()
        {
            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());

            TextBox testTextBox = filterInputBoxCol.AddStringFilterTextBox("test0");
            testTextBox.Text = "testvalue";

            DateTime testTime = DateTime.Now;
            DateTimePicker testDateTimePicker = filterInputBoxCol.AddStringFilterDateTimeEditor("test1", testTime, true);

            CheckBox testCheckBox = filterInputBoxCol.AddBooleanFilterCheckBox("test2", "textbox", true);
            
            ArrayList testOptions = new ArrayList();
            testOptions.Add("testoption1");
            ComboBox testComboBox1 = filterInputBoxCol.AddStringFilterComboBox("test3", testOptions);
            ComboBox testComboBox2 = filterInputBoxCol.AddStringFilterComboBox("test4", testOptions);
            testComboBox2.SelectedItem = "testoption1";

            string testFilterClause = "(((test0 like '*testvalue*')" +
                                      " and (test1 >= '" + testTime.ToString("yyyy/MM/dd") + "'))" +
                                      " and (test2 = 'true'))" +
                                      " and (test4 = 'testoption1')";
            Assert.AreEqual(testFilterClause, filterInputBoxCol.GetFilterClause().GetFilterClauseString());
        }

        [Test]
        public void TestAutoUpdate()
        {
            FilterInputBoxCollection filterInputBoxCol = new FilterInputBoxCollection(new DataViewFilterClauseFactory());
            Assert.IsTrue(filterInputBoxCol.GetAutomaticUpdate());
            filterInputBoxCol.SetAutomaticUpdate(false);
            Assert.IsFalse(filterInputBoxCol.GetAutomaticUpdate());
        }
    }
}
