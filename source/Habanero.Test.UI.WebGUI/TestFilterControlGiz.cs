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
