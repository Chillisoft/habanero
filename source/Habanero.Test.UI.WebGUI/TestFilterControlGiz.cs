using System;
using System.Collections;
using Habanero.UI.Base;
using Habanero.UI.WebGUI;
using NUnit.Framework;

namespace Habanero.Test.UI.WebGUI
{
    [TestFixture]
    public class TestFilterControlGiz 
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
        public void TestFilterEvent()
        {
            
            //---------------Set up test pack-------------------
            FilterControlGiz filterControl = new FilterControlGiz(new GizmoxControlFactory());
            bool filterEventFired = false;
            filterControl.Filter += delegate { filterEventFired = true; };
            //---------------Execute Test ----------------------
            filterControl.FilterButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(filterEventFired, "Filter event is not fired when filter button is clicked");
            //---------------Tear Down -------------------------          
        }

        [Test]
        public void TestClearFilters()
        {
            //---------------Set up test pack-------------------
            FilterControlGiz filterControl = new FilterControlGiz(new GizmoxControlFactory());
            ITextBox tb = filterControl.AddStringFilterTextBox("test", "prop");
            tb.Text = "sometext";
            IComboBox cb = filterControl.AddStringFilterComboBox("test2", "prop2", new object[] { "bob", "bob2" }, false);
            cb.SelectedIndex = 1;
            bool filterEventFired = false;
            filterControl.Filter += delegate { filterEventFired = true; };
            //---------------Execute Test ----------------------
            filterControl.ClearFilters();
            //---------------Test Result -----------------------
            Assert.AreEqual("", tb.Text, "Clear filters should clear textboxes/combos etc");
            Assert.AreEqual(-1, cb.SelectedIndex);
            Assert.IsTrue(filterEventFired, "Filter event is not fired when ClearFilters is called");
            //---------------Tear Down -------------------------          
        }

    }
}
