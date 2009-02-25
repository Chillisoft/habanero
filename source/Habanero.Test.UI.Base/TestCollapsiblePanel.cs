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

using Habanero.Base;
using Habanero.Console;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the CollapsiblePanel class.
    /// </summary>
     [TestFixture]
    public class TestBaseMethodsWin_CollapsiblePanel : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanel();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the CollapsiblePanel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_CollapsiblePanel : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanel();
        }
    }


    /// <summary>
    /// This test class tests the CollapsiblePanel for Win.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelWin : TestCollapsiblePanelVWG
    {
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.UI.Win.ControlFactoryWin();
            return GlobalUIRegistry.ControlFactory;
        }
    }
    /// <summary>
    /// This test class tests the CollapsiblePanel for VWG.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelVWG 
    {
        [SetUp]
        public virtual void SetupTest()
        {
            //Runs every time that any testmethod is executed
            GlobalRegistry.UIExceptionNotifier = new ConsoleExceptionNotifier();

        }
        protected virtual IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.UI.VWG.ControlFactoryVWG();
            return GlobalUIRegistry.ControlFactory;
        }
        protected virtual ICollapsiblePanel CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanel();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            //Code that is executed before any test is run in this class. If multiple tests
            // are executed then it will still only be called once.
        }

        [TearDown]
        public virtual void TearDownTest()
        {
            //runs every time any testmethod is complete
        }

        [Test]
        public void Test_CreateCollapsiblePanel()
        {
            //---------------Set up test pack-------------------
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ICollapsiblePanel panel = CreateControl();
            //---------------Test Result -----------------------
            Assert.IsFalse(panel.Collapsed);
            Assert.IsTrue(panel.Height > 0);
            Assert.AreEqual(1, panel.Controls.Count);
            Assert.AreEqual(0, panel.CollapseButton.Top);
            Assert.AreEqual(0, panel.CollapseButton.Left);
            Assert.AreEqual(panel.Width - panel.PinLabel.Width, panel.CollapseButton.Width);
            //Assert.AreEqual("Pin", panel.PinLabel.Text);

            //---------------Tear down -------------------------
        }

        [Test]
        public void TestCollapsePanel()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel panel = CreateControl();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            panel.Collapsed = true;
            //---------------Test Result -----------------------
            Assert.AreEqual(panel.CollapseButton.Height, panel.Height);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestUncollapsePanel()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel panel = CreateControl();
            int originalHeight = panel.Height;
            panel.Collapsed = true;
            //---------------Execute Test ----------------------
            panel.Collapsed = false;
            //---------------Test Result -----------------------
            Assert.AreEqual(originalHeight, panel.Height);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestCollapseButton()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel panel = CreateControl();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            panel.CollapseButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(panel.Collapsed);
        }

        [Test]
        public void TestCollapseButtonToUncollapse()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel panel = CreateControl();
            panel.CollapseButton.PerformClick();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            panel.CollapseButton.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(panel.Collapsed);
        }

        [Test]
        public void TestSetContentControl()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel collapsiblePanel = CreateControl();
            IPanel contentControl = GetControlFactory().CreatePanel();
            contentControl.Height = 10;
            contentControl.Width = 10;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            collapsiblePanel.ContentControl = contentControl;
            //---------------Test Result -----------------------
            Assert.AreSame(contentControl, collapsiblePanel.ContentControl);
            Assert.AreEqual(2, collapsiblePanel.Controls.Count);
            Assert.AreEqual(collapsiblePanel.Height - collapsiblePanel.CollapseButton.Height, contentControl.Height);

            Assert.AreEqual(collapsiblePanel.Width, contentControl.Width);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestChangeContentControl()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel collapsiblePanel = CreateControl();
            IPanel contentControl1 = GetControlFactory().CreatePanel();
            collapsiblePanel.ContentControl = contentControl1;
            IPanel contentControl2 = GetControlFactory().CreatePanel();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            collapsiblePanel.ContentControl = contentControl2;
            //---------------Test Result -----------------------
            Assert.AreSame(contentControl2, collapsiblePanel.ContentControl);
            Assert.AreEqual(2, collapsiblePanel.Controls.Count);
            Assert.AreEqual(collapsiblePanel.Height - collapsiblePanel.CollapseButton.Height, contentControl2.Height);
            Assert.AreEqual(collapsiblePanel.Width, contentControl2.Width);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestRaisesUncollapsedEvent()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel collapsiblePanel = CreateControl();
            bool uncollapsedEventFired = false;
            collapsiblePanel.Uncollapsed += delegate { uncollapsedEventFired = true; };
            collapsiblePanel.Collapsed = true;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            collapsiblePanel.Collapsed = false;
            //---------------Test Result -----------------------
            Assert.IsTrue(uncollapsedEventFired);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestPinWhenUncollapsed()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel collapsiblePanel = CreateControl();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            collapsiblePanel.Pinned = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(collapsiblePanel.Pinned);
            //Assert.AreEqual("Unpin", collapsiblePanel.PinLabel.Text);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestUnpinWhenUncollapsed()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel collapsiblePanel = CreateControl();
            collapsiblePanel.Pinned = true;
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            collapsiblePanel.Pinned = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(collapsiblePanel.Pinned);
            Assert.IsFalse(collapsiblePanel.Collapsed);
            // Assert.AreEqual("Pin", collapsiblePanel.PinLabel.Text);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestPinWhenCollapsed()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel collapsiblePanel = CreateControl();
            collapsiblePanel.Collapsed = true;
            bool uncollapsedEventFired = false;
            collapsiblePanel.Uncollapsed += delegate { uncollapsedEventFired = true; };
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            collapsiblePanel.Pinned = true;
            //---------------Test Result -----------------------
            Assert.IsFalse(collapsiblePanel.Collapsed);
            Assert.IsTrue(collapsiblePanel.Pinned);
            Assert.IsTrue(uncollapsedEventFired);
            //---------------Tear down -------------------------
        }

        [Test]
        public void TestCollapseWhenPinned()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel collapsiblePanel = CreateControl();
            collapsiblePanel.Pinned = true;
            //---------------Assert Precondition----------------
            Assert.IsFalse(collapsiblePanel.Collapsed);
            Assert.IsTrue(collapsiblePanel.Pinned);
            //---------------Execute Test ----------------------
            collapsiblePanel.Collapsed = true;
            //---------------Test Result -----------------------
            Assert.IsTrue(collapsiblePanel.Collapsed);
            Assert.IsFalse(collapsiblePanel.Pinned);
            //Assert.AreEqual("Pin", collapsiblePanel.PinLabel.Text);
            //---------------Tear down -------------------------
        }

        [Test, Ignore("The Pin Button has been Change to a Label and the performClick cannnot be tested.")]
        public void TestClickPinButtonPins()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel collapsiblePanel = CreateControl();
            //---------------Assert Precondition----------------
            Assert.IsFalse(collapsiblePanel.Pinned);
            //---------------Execute Test ----------------------
            // collapsiblePanel.PinLabel.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsTrue(collapsiblePanel.Pinned);
            //---------------Tear down -------------------------
        }

        [Test, Ignore("The Pin Button has been Change to a Label and the performClick cannnot be tested.")]
        public void TestClickPinButtonUnpins()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanel collapsiblePanel = CreateControl();
            //  collapsiblePanel.PinLabel.PerformClick();
            //---------------Assert Precondition----------------
            Assert.IsTrue(collapsiblePanel.Pinned);
            //---------------Execute Test ----------------------
            // collapsiblePanel.PinLabel.PerformClick();
            //---------------Test Result -----------------------
            Assert.IsFalse(collapsiblePanel.Pinned);
            //---------------Tear down -------------------------
        }
    }


}
