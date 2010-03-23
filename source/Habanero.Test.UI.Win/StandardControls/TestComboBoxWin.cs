using System.Collections.Generic;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.StandardControls
{
    [TestFixture]
    public class TestComboBoxWin : TestComboBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        [Test]
        public void Test_NoSolutionSelected_ShouldNotFireSolutionSelected_WhenLoadButtonClicked()
        {
            //---------------Set up test pack-------------------
            FormWin form = new FormWin();
            List<string> defs = new List<string>();
            defs.Add("AA");
            defs.Add("BBB");
            IComboBox selector = GetControlFactory().CreateComboBox();
            form.Controls.Add((System.Windows.Forms.Control)selector);
            System.Windows.Forms.ComboBox winCombo = (System.Windows.Forms.ComboBox)selector;
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, winCombo.Items.Count);
            Assert.AreEqual(0, selector.Items.Count);
            //---------------Execute Test ----------------------
            selector.DataSource = defs;
            //---------------Test Result -----------------------
            Assert.AreEqual(2, winCombo.Items.Count);
            Assert.AreEqual(2, selector.Items.Count);
        }
        protected override string GetUnderlyingAutoCompleteSourceToString(IComboBox controlHabanero)
        {
            System.Windows.Forms.ComboBox control = (System.Windows.Forms.ComboBox)controlHabanero;
            return control.AutoCompleteSource.ToString();
        }

        [Test, Ignore("Need to figure out how to run tests in STAThread for this test")]
        public override void TestConversion_AutoCompleteSource_None()
        {
            base.TestConversion_AutoCompleteSource_None();
        }

        [Test, Ignore("Need to figure out how to run tests in STAThread for this test")]
        public override void TestConversion_AutoCompleteSource_AllSystemSources()
        {
            base.TestConversion_AutoCompleteSource_AllSystemSources();
        }

        [Test, Ignore("Need to figure out how to run tests in STAThread for this test")]
        public override void TestConversion_AutoCompleteSource_AllUrl()
        {
            base.TestConversion_AutoCompleteSource_AllUrl();
        }

        [Test, Ignore("Need to figure out how to run tests in STAThread for this test")]
        public override void TestConversion_AutoCompleteSource_CustomSource()
        {
            base.TestConversion_AutoCompleteSource_CustomSource();
        }

        [Test, Ignore("Need to figure out how to run tests in STAThread for this test")]
        public override void TestConversion_AutoCompleteSource_FileSystem()
        {
            base.TestConversion_AutoCompleteSource_FileSystem();
        }

        [Test, Ignore("Need to figure out how to run tests in STAThread for this test")]
        public override void TestConversion_AutoCompleteSource_FileSystemDirectories()
        {
            base.TestConversion_AutoCompleteSource_FileSystemDirectories();
        }

        [Test, Ignore("Need to figure out how to run tests in STAThread for this test")]
        public override void TestConversion_AutoCompleteSource_HistoryList()
        {
            base.TestConversion_AutoCompleteSource_HistoryList();
        }

        [Test, Ignore("Need to figure out how to run tests in STAThread for this test")]
        public override void TestConversion_AutoCompleteSource_ListItems()
        {
            base.TestConversion_AutoCompleteSource_ListItems();
        }

        [Test, Ignore("Need to figure out how to run tests in STAThread for this test")]
        public override void TestConversion_AutoCompleteSource_RecentlyUsedList()
        {
            base.TestConversion_AutoCompleteSource_RecentlyUsedList();
        }
    }
}