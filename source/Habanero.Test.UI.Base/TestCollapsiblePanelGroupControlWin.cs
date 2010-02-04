using System;
using System.Drawing;
using Habanero.BO;
using Habanero.BO.ClassDefinition;
using Habanero.BO.Loaders;
using Habanero.Test.Structure;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{
    /// <summary>
    /// This test class tests the base inherited methods of the CollapsiblePanel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsWin_CollapsiblePanelGroupControll : TestBaseMethods.TestBaseMethodsWin
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanelGroupControl();
        }
    }

    /// <summary>
    /// This test class tests the base inherited methods of the CollapsiblePanel class.
    /// </summary>
    [TestFixture]
    public class TestBaseMethodsVWG_CollapsiblePanelGroupControl : TestBaseMethods.TestBaseMethodsVWG
    {
        protected override IControlHabanero CreateControl()
        {
            return GetControlFactory().CreateCollapsiblePanelGroupControl();
        }
    }


    /// <summary>
    /// This test class tests the CollapsiblePanel for Win.
    /// </summary>
    [TestFixture]
    public class TestCollapsiblePanelGroupControlVWG : TestCollapsiblePanelGroupControlWin
    {
        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.UI.VWG.ControlFactoryVWG();
            return GlobalUIRegistry.ControlFactory;
        }
    }
    [TestFixture]
    public class TestCollapsiblePanelGroupControlWin
    {
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            ClassDef.ClassDefs.Clear();
            ClassDef.ClassDefs.Add(new XmlClassDefsLoader(BOBroker.GetClassDefsXml(), new DtdLoader(), new DefClassFactory()).LoadClassDefs());
            GlobalUIRegistry.ControlFactory = new ControlFactoryWin();
        }

        [SetUp]
        public virtual void SetupTest()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
//            BORegistry.DataAccessor = new DataAccessorInMemory();
        }

        protected virtual IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = new Habanero.UI.Win.ControlFactoryWin();
            return GlobalUIRegistry.ControlFactory;
        }

        protected virtual ICollapsiblePanelGroupControl CreateCollapsiblePanelGroupControlWin()
        {
                        return GetControlFactory().CreateCollapsiblePanelGroupControl();
        }

        [Test]
        public void Test_CreateControl()
        {
            //---------------Set up test pack-------------------

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            //---------------Test Result -----------------------
            Assert.IsNotNull(control);
            Assert.IsNotNull(control.PanelsList);
            Assert.IsNotNull(control.ControlFactory);
            Assert.IsNotNull(control.ColumnLayoutManager);
        }

        [Test]
        public void Test_CollapsiblePanelExpandedHeight()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            ICollapsiblePanel collapsiblePanel = control.AddControl(GetControlFactory().CreatePanel(), "", 53);
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, control.PanelsList.Count);
            Assert.AreEqual(1, control.Controls.Count);
            Assert.AreSame(collapsiblePanel, control.PanelsList[0]);
            int expectedExpandedHeight = 53 + collapsiblePanel.CollapseButton.Height;
            Assert.AreEqual(expectedExpandedHeight, collapsiblePanel.ExpandedHeight);
            //---------------Execute Test ----------------------
            collapsiblePanel.Collapsed = true;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedExpandedHeight, collapsiblePanel.ExpandedHeight);
        }

        [Test]
        public void Test_AddControl()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, control.PanelsList.Count);
            //---------------Execute Test ----------------------
            ICollapsiblePanel collapsiblePanel = control.AddControl(GetControlFactory().CreatePanel(), "", 53);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, control.PanelsList.Count);
            Assert.AreEqual(1, control.Controls.Count);
            Assert.AreSame(collapsiblePanel, control.PanelsList[0]);
            Assert.AreEqual(53 + collapsiblePanel.CollapseButton.Height, collapsiblePanel.ExpandedHeight);
        }

        [Test]
        public void Test_AddTwoControl()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content2 = GetControlFactory().CreatePanel();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, control.PanelsList.Count);
            //---------------Execute Test ----------------------
            control.AddControl(GetControlFactory().CreatePanel(), "", 53);
            control.AddControl(content2, "", 53);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, control.PanelsList.Count);
            Assert.AreEqual(2, control.Controls.Count);
            ICollapsiblePanel cp2 = control.PanelsList[1];
            Assert.AreSame(content2, cp2.ContentControl);
        }
        [Test]
        public void Test_AddControl_WhenControlIsCollapsiblePanel_ShouldAdd()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl groupControl = CreateCollapsiblePanelGroupControlWin();
            ICollapsiblePanel collapsiblePanel = GetControlFactory().CreateCollapsiblePanel("Name");
            collapsiblePanel.MinimumSize = new Size(123, 76);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, groupControl.PanelsList.Count);
            Assert.AreEqual("Name", collapsiblePanel.Name);
            Assert.AreEqual("Name", collapsiblePanel.CollapseButton.Text);
            Assert.AreEqual("Name", collapsiblePanel.CollapseButton.Name);
            //---------------Execute Test ----------------------
            //            ICollapsiblePanel collapsiblePanel = groupControl.AddControl(GetControlFactory().CreatePanel(), "", 53);
            ICollapsiblePanel returnedCollapsiblePanel = groupControl.AddControl(collapsiblePanel);

            //---------------Test Result -----------------------
            Assert.AreSame(collapsiblePanel, returnedCollapsiblePanel);
            Assert.AreEqual(1, groupControl.PanelsList.Count);
            Assert.AreEqual(1, groupControl.Controls.Count);
            Assert.AreSame(collapsiblePanel, groupControl.PanelsList[0]);
            Assert.AreEqual(53 + collapsiblePanel.CollapseButton.Height, collapsiblePanel.ExpandedHeight);
            Assert.AreEqual("Name", collapsiblePanel.CollapseButton.Text);
            Assert.AreEqual("Name", collapsiblePanel.CollapseButton.Name);
        }

        [Test]
        public void Test_AddTwoControl__WhenControlIsCollapsiblePanel_ShouldAdd()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            ICollapsiblePanel collapsiblePanel1 = GetControlFactory().CreateCollapsiblePanel("Panel1");
            collapsiblePanel1.MinimumSize = new Size(123, 76);
            ICollapsiblePanel collapsiblePanel2 = GetControlFactory().CreateCollapsiblePanel("Panel2");
            collapsiblePanel2.MinimumSize = new Size(123, 55);
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, control.PanelsList.Count);
            //---------------Execute Test ----------------------
            control.AddControl(collapsiblePanel1);
            control.AddControl(collapsiblePanel2);
            //---------------Test Result -----------------------
            Assert.AreEqual(2, control.PanelsList.Count);
            Assert.AreEqual(2, control.Controls.Count);
            ICollapsiblePanel cp2 = control.PanelsList[1];
            Assert.AreSame(collapsiblePanel2, cp2);
        }

        [Test]
        public void Test_AddControl_TestMinHeight_EqualsButtonHeight_Plus_ControlMinHeight()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content = GetControlFactory().CreatePanel();
            const int contentControlMinimunHeight = 53;
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, control.PanelsList.Count);
            //---------------Execute Test ----------------------
            control.AddControl(content, "", contentControlMinimunHeight);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, control.PanelsList.Count);
            ICollapsiblePanel cp1 = control.PanelsList[0];
            Assert.AreEqual(contentControlMinimunHeight + cp1.CollapseButton.Height, cp1.ExpandedHeight);
            Assert.AreEqual(cp1.CollapseButton.Height, cp1.Height);
            Assert.IsTrue(cp1.Collapsed);
        }

        [Test]
        public void Test_AddControl_TotalExpandedHeightEquals_CP1_ExpandedHeight()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content = GetControlFactory().CreatePanel();
            ICollapsiblePanel cp1 = control.AddControl(content, "", 53);
            ColumnLayoutManager layoutManager = control.ColumnLayoutManager;
            //---------------Assert Precondition----------------
            Assert.AreEqual(1, control.PanelsList.Count);
            //---------------Execute Test ----------------------
            int totalExpandedHeight = control.TotalExpandedHeight;
            //---------------Test Result -----------------------
            Assert.AreEqual(layoutManager.BorderSize + cp1.ExpandedHeight + layoutManager.GapSize, totalExpandedHeight);
        }

        [Test]
        public void Test_AddTwoControl_TotalExpandedHeightEquals_CP1_CP2_ExpandedHeight()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            ICollapsiblePanel cp1 = control.AddControl(content1, "", 99);
            ICollapsiblePanel cp2 = control.AddControl(content2, "", 53);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, control.PanelsList.Count);
            ColumnLayoutManager layoutManager = control.ColumnLayoutManager;
            int expectedTotalHeight = layoutManager.BorderSize + cp1.ExpandedHeight + layoutManager.GapSize + cp2.ExpandedHeight + layoutManager.GapSize;
            //---------------Execute Test ----------------------
            int actualTotalHeight = control.TotalExpandedHeight;
            //---------------Test Result -----------------------
            Assert.AreEqual(expectedTotalHeight, actualTotalHeight);
        }

        [Test]
        public void Test_AddControl_PlacesTextOnButton()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content = GetControlFactory().CreatePanel();
            const string headingText = "some text";
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, control.PanelsList.Count);
            //---------------Execute Test ----------------------
            control.AddControl(content, headingText, 53);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, control.PanelsList.Count);
            ICollapsiblePanel cp2 = control.PanelsList[0];
            Assert.AreEqual(headingText, cp2.CollapseButton.Text);
        }

        [Test]
        public void Test_CollapsiblePanels_HasColumnLayout()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            //---------------Assert Precondition----------------
            Assert.AreEqual(0, control.PanelsList.Count);
            //---------------Execute Test ----------------------
            ICollapsiblePanel cp1 = control.AddControl(content1, "", 99);
            ICollapsiblePanel cp2 = control.AddControl(content2, "", 53);
            //---------------Test Result -----------------------
            ColumnLayoutManager layoutManager = control.ColumnLayoutManager;
            int expectedCP2_Top = cp1.Height + layoutManager.BorderSize + layoutManager.GapSize;
            Assert.AreEqual(expectedCP2_Top, cp2.Top);
        }
        [Test]
        public void Test_UncollapseCP1_MovesCP2_Top()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            ICollapsiblePanel cp1 = control.AddControl(content1, "", 99);
            ICollapsiblePanel cp2 = control.AddControl(content2, "", 53);
            //---------------Assert Precondition----------------
            Assert.AreEqual(2, control.PanelsList.Count);
            ColumnLayoutManager layoutManager = control.ColumnLayoutManager;
            int expected_Start_CP2_Top = cp1.CollapseButton.Height + layoutManager.BorderSize + layoutManager.GapSize;
            Assert.AreEqual(expected_Start_CP2_Top, cp2.Top);
            //---------------Execute Test ----------------------
            cp1.CollapseButton.PerformClick();
            //---------------Test Result -----------------------
            int expected_Finish_CP2_Top = cp1.ExpandedHeight + layoutManager.BorderSize + layoutManager.GapSize;
            Assert.AreEqual(expected_Finish_CP2_Top, cp2.Top);
        }


        [Test]
        public void TestUncollapsingPanelCollapsingAllOtherPanels()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            ICollapsiblePanel cp1 = control.AddControl(content1, "", 99);
            ICollapsiblePanel cp2 = control.AddControl(content2, "", 53);
            cp1.Collapsed = false;
            //---------------Assert Precondition----------------
            Assert.IsTrue(cp2.Collapsed);
            Assert.IsFalse(cp1.Collapsed);
            //---------------Execute Test ----------------------
            cp2.Collapsed = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(cp2.Collapsed);
            Assert.IsTrue(cp1.Collapsed);
            //---------------Tear down -------------------------
        }


        [Test]
        public void TestUncollapsingPanelCollapsingAllOtherPanelsExceptPinnedPanels()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            IPanel content3 = GetControlFactory().CreatePanel();
            ICollapsiblePanel collapsiblePanel1 = control.AddControl(content1, "", 99);
            ICollapsiblePanel collapsiblePanel2 = control.AddControl(content2, "", 53);
            ICollapsiblePanel collapsiblePanel3 = control.AddControl(content3, "", 53);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            collapsiblePanel2.Pinned = true;
            collapsiblePanel1.Collapsed = false;
            collapsiblePanel3.Collapsed = false;
            //---------------Test Result -----------------------
            Assert.IsFalse(collapsiblePanel2.Collapsed);
            Assert.IsTrue(collapsiblePanel2.Pinned);
            Assert.IsTrue(collapsiblePanel1.Collapsed);
            Assert.IsFalse(collapsiblePanel3.Collapsed);
        }

        [Test]
        public void TestUnCollapseAll()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            IPanel content3 = GetControlFactory().CreatePanel();
            control.AddControl(content1, "", 99);
            control.AddControl(content2, "", 53);
            control.AddControl(content3, "", 53);
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            control.AllCollapsed = false;

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(3, control.Controls.Count);
            foreach (ICollapsiblePanel collapsiblePanel in control.Controls)
            {
                Assert.IsTrue(collapsiblePanel.Pinned);
                Assert.IsFalse(collapsiblePanel.Collapsed);
                Assert.AreEqual(collapsiblePanel.ExpandedHeight, collapsiblePanel.Height);
            }
            //---------------Tear down -------------------------
        }
        [Test]
        public void TestCollapseAll()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            IPanel content3 = GetControlFactory().CreatePanel();
            control.AddControl(content1, "", 99);
            ICollapsiblePanel collapsiblePanel2 = control.AddControl(content2, "", 53);
            control.AddControl(content3, "", 53);
            control.AllCollapsed = false;
            //---------------Assert Precondition----------------
            foreach (ICollapsiblePanel collapsiblePanel in control.Controls)
            {
                Assert.IsTrue(collapsiblePanel.Pinned);
                Assert.IsFalse(collapsiblePanel.Collapsed);
                Assert.AreEqual(collapsiblePanel.ExpandedHeight, collapsiblePanel.Height);
            }
            //---------------Execute Test ----------------------
            control.AllCollapsed = true;

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.AreEqual(3, control.Controls.Count);
            foreach (ICollapsiblePanel collapsiblePanel in control.Controls)
            {
                Assert.IsFalse(collapsiblePanel.Pinned);
                Assert.IsTrue(collapsiblePanel.Collapsed);
                Assert.Less(collapsiblePanel.Height, collapsiblePanel.ExpandedHeight);
                Assert.AreNotEqual(collapsiblePanel2.ExpandedHeight, collapsiblePanel2.Height);
            }
            //---------------Tear down -------------------------
        }


        [Test]
        public void TestCollapseOneAfterAnother()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            IPanel content3 = GetControlFactory().CreatePanel();
            ICollapsiblePanel collapsiblePanel1 = control.AddControl(content1, "", 99);
            ICollapsiblePanel collapsiblePanel2 = control.AddControl(content2, "", 53);
            ICollapsiblePanel collapsiblePanel3 = control.AddControl(content3, "", 53);

            //---------------Execute Test ----------------------
            collapsiblePanel1.Collapsed = true;
            collapsiblePanel2.Collapsed = true;
            collapsiblePanel3.Collapsed = true;
            collapsiblePanel2.Collapsed = false;
            collapsiblePanel1.Collapsed = false;

            //---------------Test Result -----------------------
            //panel1 should be the same as its original height after the above steps
            Assert.AreEqual(collapsiblePanel1.ExpandedHeight, collapsiblePanel1.Height);
            //---------------Tear Down -------------------------
        }

        [Test]
        public void TestCollapseAll_DoesNotPreventControlFromExpandingAgain_FixBug()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            IPanel content3 = GetControlFactory().CreatePanel();
            control.AddControl(content1, "", 99);
            ICollapsiblePanel collapsiblePanel2 = control.AddControl(content2, "", 53);
            control.AddControl(content3, "", 53);
            control.AllCollapsed = true;
            //---------------Assert Precondition----------------
            Assert.AreSame(collapsiblePanel2, control.PanelsList[1]);
            Assert.AreNotEqual(collapsiblePanel2.ExpandedHeight, collapsiblePanel2.Height);
            //---------------Execute Test ----------------------
            collapsiblePanel2.Collapsed = false;
            //---------------Test Result -----------------------
            Assert.AreEqual(collapsiblePanel2.ExpandedHeight, collapsiblePanel2.Height);
        }
        [Test]
        public void Test_UncollapsePanel_FiresItemSelected()
        {
            //---------------Set up test pack-------------------
            ICollapsiblePanelGroupControl control = CreateCollapsiblePanelGroupControlWin();
            IPanel content1 = GetControlFactory().CreatePanel();
            IPanel content2 = GetControlFactory().CreatePanel();
            IPanel content3 = GetControlFactory().CreatePanel();
            control.AddControl(content1, "", 99);
            ICollapsiblePanel collapsiblePanel2 = control.AddControl(content2, "", 53);
            control.AddControl(content3, "", 53);
            control.AllCollapsed = true;
            bool itemSelected = false;
            control.ItemSelected += delegate { itemSelected = true; };
            //---------------Assert Precondition----------------
            Assert.AreSame(collapsiblePanel2, control.PanelsList[1]);
            Assert.AreNotEqual(collapsiblePanel2.ExpandedHeight, collapsiblePanel2.Height);
            Assert.IsFalse(itemSelected);
            //---------------Execute Test ----------------------
            collapsiblePanel2.Collapsed = false;
            //---------------Test Result -----------------------
            Assert.IsTrue(itemSelected);
        }

    }
}