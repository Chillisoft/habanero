using System.Windows.Forms;
using Habanero.Ui.Generic;
using NUnit.Framework;

namespace Chillisoft.Test.UI.Generic.v2
{
    [TestFixture]
    public class TestFlowLayoutManager
    {
        private FlowLayoutManager manager;
        private Control managedControl;

        [SetUp]
        public void SetupLayoutManager()
        {
            managedControl = new Control();
            managedControl.Width = 100;
            managedControl.Height = 100;
            manager = new FlowLayoutManager(managedControl);
            manager.GapSize = 5;
        }

        [Test]
        public void TestControl()
        {
            Assert.AreSame(managedControl, manager.ManagedControl);
        }

        [Test]
        public void TestAddControl()
        {
            Control ctlToAdd = CreateControl(10, 10);
            manager.AddControl(ctlToAdd);
            Assert.AreEqual(5, ctlToAdd.Left, "Added control's Left prop should be 5 because of the default border.");
            Assert.AreEqual(5, ctlToAdd.Top, "Added control's Top prop should be 5 because of the default border.");
            Assert.AreEqual(10, ctlToAdd.Width, "Added control's Width prop should be 10.");
            Assert.AreEqual(10, ctlToAdd.Height, "Added control's Height prop should be 10.");
        }

        [Test]
        public void TestAddTwoControls()
        {
            manager.AddControl(CreateControl(10, 10));
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl2);
            Assert.AreEqual(20, ctl2.Left, "Left should equal Border + First control's width + spacing");
            Assert.AreEqual(5, ctl2.Top, "Top should equal Border");
        }

        [Test]
        public void TestAddTwoControlsWithWrap()
        {
            managedControl.Width = 30;
            manager.AddControl(CreateControl(10, 10));
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl2);
            Assert.AreEqual(5, ctl2.Left, "Should wrap to the next line because it's right is outside the area.");
            Assert.AreEqual(20, ctl2.Top, "Top should equal Border + first line's max height + gap.");
        }

        [Test]
        public void TestAddThreeControlsOfDifferentHeightWithWrap()
        {
            managedControl.Width = 40;
            manager.AddControl(CreateControl(10, 10));
            manager.AddControl(CreateControl(10, 20));
            Control ctl = CreateControl(10, 10);
            manager.AddControl(ctl);
            Assert.AreEqual(5, ctl.Left, "Should wrap to next line because it doesn't fit on first line.");
            Assert.AreEqual(30, ctl.Top, "Top should equal border + first line's max height + gap.");
        }

        [Test]
        public void TestResizeWhenNothingHappens()
        {
            Control ctl1 = CreateControl(10, 10);
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            manager.ManagedControl.Height = 50;
            Assert.AreEqual(5, ctl1.Left, "Left should be the same after resize");
            Assert.AreEqual(5, ctl1.Top, "Top should be the same after resize.");
            Assert.AreEqual(20, ctl2.Left, "Left should be the same after resize.");
            Assert.AreEqual(5, ctl2.Top, "Top should be the same after resize.");
        }

        [Test]
        public void TestResizeWhenPositionsShouldChange()
        {
            Control ctl1 = CreateControl(10, 10);
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            manager.ManagedControl.Width = 30;
            Assert.AreEqual(5, ctl1.Left, "Left of ctl1 should be the same after resize");
            Assert.AreEqual(5, ctl1.Top, "Top of ctl1 should be the same after resize.");
            Assert.AreEqual(5, ctl2.Left, "Left of ctl2 should have wrapped to next line after resize.");
            Assert.AreEqual(20, ctl2.Top, "Top should be on the next row after resize.");
        }

        [Test]
        public void TestBorderSize()
        {
            Control ctl1 = CreateControl(10, 10);
            manager.BorderSize = 10;
            manager.AddControl(ctl1);
            Assert.AreEqual(10, ctl1.Left, "Left of ctl1 should equal bordersize.");
            Assert.AreEqual(10, ctl1.Top, "Top of ctl1 should equal bordersize.");
        }

        [Test]
        public void TestRefreshOnBorderSizeChange()
        {
            Control ctl1 = CreateControl(10, 10);
            Control ctl2 = CreateControl(10, 10);
            manager.ManagedControl.Width = 40;
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            manager.BorderSize = 10;
            Assert.AreEqual(10, ctl2.Left, "Left of ctl2 should equal bordersize because it should wrap.");
            Assert.AreEqual(25, ctl2.Top, "Top of ctl2 should equal bordersize + rowsize + gapsize.");
        }

        [Test]
        public void TestGapSize()
        {
            manager.GapSize = 2;
            manager.AddControl(CreateControl(10, 10));
            Control ctl = CreateControl(10, 10);
            manager.AddControl(ctl);
            Assert.AreEqual(17, ctl.Left, "Left of ctl should be bordersize + 10 + gapsize.");
        }

        [Test]
        public void TestRefreshOnGapSizeChange()
        {
            manager.ManagedControl.Width = 100;
            manager.BorderSize = 0;
            manager.AddControl(CreateControl(80, 20));
            Control ctl = CreateControl(50, 20);
            manager.AddControl(ctl);
            manager.GapSize = 2;
            Assert.AreEqual(22, ctl.Top, "Top of ctl should be 20 + gapsize.");
        }

        [Test]
        public void TestOnlyVisibleControlsIncluded()
        {
            manager.AddControl(CreateControl(10, 10));
            Control ctl1 = CreateControl(10, 10);
            ctl1.Visible = false;
            manager.AddControl(ctl1);
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl2);
            Assert.AreEqual(20, ctl2.Left, "Second control should be ignored because it is invisible.");
        }

        [Test]
        public void TestRefreshOnControlVisibilityChanged()
        {
            manager.AddControl(CreateControl(10, 10));
            Control ctl1 = CreateControl(10, 10);
            manager.AddControl(ctl1);
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl2);
            ctl1.Visible = false;
            Assert.AreEqual(20, ctl2.Left, "Control positions should be refreshed when one is made invisible.");
        }

        [Test]
        public void TestRefreshOnControlSizeChanged()
        {
            manager.AddControl(CreateControl(10, 10));
            Control ctl1 = CreateControl(10, 10);
            manager.AddControl(ctl1);
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl2);
            ctl1.Width = 20;
            Assert.AreEqual(45, ctl2.Left, "Control positions should be refreshed when one control's size is changed.");
        }

        [Test]
        public void TestRightAlignRows()
        {
            manager.Alignment = FlowLayoutManager.Alignments.Right;
            manager.ManagedControl.Width = 60;
            Control ctl = CreateControl(10, 10);
            manager.AddControl(ctl);
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl2);
            Assert.AreEqual(45, ctl.Left, "Control should be right aligned.");
            Assert.AreEqual(30, ctl2.Left, "Control should be right aligned.");
        }

        [Test]
        public void TestRightAlignRowsWithWrap()
        {
            manager.Alignment = FlowLayoutManager.Alignments.Right;
            manager.ManagedControl.Width = 30;
            Control ctl = CreateControl(10, 10);
            manager.AddControl(ctl);
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl2);
            Assert.AreEqual(15, ctl.Left, "Control should be right aligned.");
            Assert.AreEqual(15, ctl2.Left, "Control should be right aligned.");
        }

        [Test]
        public void TestRightAlignChangeShouldLayout()
        {
            manager.ManagedControl.Width = 60;
            Control ctl = CreateControl(10, 10);
            manager.AddControl(ctl);
            manager.Alignment = FlowLayoutManager.Alignments.Right;
            Assert.AreEqual(45, ctl.Left, "Control should be right aligned.");
        }

        [Test]
        public void TestCentreAlignRowOneControl()
        {
            manager.Alignment = FlowLayoutManager.Alignments.Centre;
            Control ctl = CreateControl(10, 10);
            manager.AddControl(ctl);
            Assert.AreEqual(45, ctl.Left, "Control should be centre aligned.");
        }

        [Test]
        public void TestCentreAlignRowTwoControls()
        {
            manager.Alignment = FlowLayoutManager.Alignments.Centre;
            Control ctl = CreateControl(10, 10);
            manager.AddControl(ctl);
            Control ctl2 = CreateControl(10, 10);
            manager.AddControl(ctl2);
            Assert.AreEqual(37, ctl.Left, "Control should be centre aligned.");
            Assert.AreEqual(52, ctl2.Left, "Control should be centre aligned.");
        }

        [Test]
        public void TestCentreAlignRowTwoRows()
        {
            manager.Alignment = FlowLayoutManager.Alignments.Centre;
            manager.ManagedControl.Width = 40;
            Control ctl = CreateControl(20, 10);
            manager.AddControl(ctl);
            Control ctl2 = CreateControl(20, 10);
            manager.AddControl(ctl2);
            Assert.AreEqual(10, ctl.Left, "Control should be centre aligned.");
            Assert.AreEqual(10, ctl2.Left, "Control should be centre aligned.");
            Assert.AreEqual(20, ctl2.Top, "Control should be in second row.");
        }

        [Test]
        public void TestCentreAlignRowWithInvisibleControls()
        {
            manager.Alignment = FlowLayoutManager.Alignments.Centre;
            Control ctl = CreateControl(20, 10);
            manager.AddControl(ctl);
            Control ctl2 = CreateControl(20, 10);
            ctl2.Visible = false;
            manager.AddControl(ctl2);
            Assert.AreEqual(40, ctl.Left, "Control should be centre aligned - other control is invisible.");
        }

        [Test]
        public void TestNewLine()
        {
            Control ctl = CreateControl(20, 10);
            manager.AddControl(ctl);
            Control ctl2 = CreateControl(20, 10);
            manager.NewLine();
            manager.AddControl(ctl2);
            Assert.AreEqual(5, ctl2.Left, "ctl2 should be on the far left of a new line");
            Assert.AreEqual(20, ctl2.Top, "ctl2 should be on a new line");
        }

        [Test]
        public void TestGlueSimpleCase()
        {
            manager.AddControl(CreateControl(40, 10));
            Control ctl = CreateControl(35, 10);
            manager.AddControl(ctl);
            Control ctl2 = CreateControl(35, 10);
            manager.AddGlue();
            manager.AddControl(ctl2);

            Assert.AreEqual(5, ctl.Left, "Glue should mean ctl moves with ctl2 (if space permits)");
            Assert.AreEqual(20, ctl.Top, "Glue should mean ctl moves to new row with ctl2 (if space permits)");
            Assert.AreEqual(45, ctl2.Left, "ctl2 should be to the right of ctl on a new line because of glue");
            Assert.AreEqual(20, ctl2.Top, "ctl2 should be in row 2");
        }

        [Test]
        public void TestGlueWithGluedControlAtLeftBorder()
        {
            Control ctl = CreateControl(30, 10);
            manager.AddControl(ctl);
            manager.AddGlue();
            Control ctl2 = CreateControl(90, 10);
            manager.AddControl(ctl2);

            Assert.AreEqual(5, ctl.Left, "ctl should stay where it is in spite of glue");
            Assert.AreEqual(5, ctl.Top, "ctl should stay where it is in spite of glue");
            Assert.AreEqual(5, ctl2.Left,
                            "ctl2 should move to next line by itself (since ctl and ctl2 can't fit on one line.");
            Assert.AreEqual(20, ctl2.Top, "ctl2 should be in row 2");
        }


        [Test]
        public void TestGlueWithGluedControlInvisible()
        {
            manager.AddControl(CreateControl(50, 10));
            Control ctl = CreateControl(30, 10);
            ctl.Visible = false;
            manager.AddControl(ctl);
            manager.AddGlue();
            Control ctl2 = CreateControl(50, 10);
            manager.AddControl(ctl2);

            Assert.AreEqual(0, ctl.Left, "ctl is invisible so should be ignored");
            Assert.AreEqual(0, ctl.Top, "ctl is invisible so should be ignored");
            Assert.AreEqual(5, ctl2.Left,
                            "ctl2 should move to next line but still be at left border because ctl is invisible ");
            Assert.AreEqual(20, ctl2.Top, "ctl2 should be in row 2");

            ctl.Visible = true;
            Assert.AreEqual(5, ctl.Left, "ctl be in second row because of glue");
            Assert.AreEqual(20, ctl.Top, "ctl be in second row because of glue");
            Assert.AreEqual(40, ctl2.Left, "ctl2 should move to next line and move accross because ctl is visible ");
            Assert.AreEqual(20, ctl2.Top, "ctl2 should be in row 2");
        }

        [Test]
        public void TestGlueWithGluedControlVeryWide()
        {
            manager.AddControl(CreateControl(30, 10));
            Control ctl = CreateControl(50, 10);
            manager.AddControl(ctl);
            manager.AddGlue();
            Control ctl2 = CreateControl(50, 10);
            manager.AddControl(ctl2);

            Assert.AreEqual(40, ctl.Left,
                            "ctl should stay where it is in spite of glue since it cant fit with ctl2 on one line");
            Assert.AreEqual(5, ctl.Top,
                            "ctl should stay where it is in spite of glue since it cant fit with ctl2 on one line");
            Assert.AreEqual(5, ctl2.Left,
                            "ctl2 should move to next line by itself (since ctl and ctl2 can't fit on one line.");
            Assert.AreEqual(20, ctl2.Top, "ctl2 should be in row 2");
        }
        
        [Test]
        public void TestRightAlignedTabOrdering()
        {
            manager.Alignment = FlowLayoutManager.Alignments.Right;
            Control ctl1 = CreateControl(20, 20);
            manager.AddControl(ctl1);
            Control ctl2 = CreateControl(20, 20);
            manager.AddControl(ctl2);
            //Assert.AreEqual(0, ctl2.TabIndex);
            Assert.AreEqual(1, ctl1.TabIndex);
        }
        
        [Test]
        public void TestRightAlignedTabOrderingMultipleRows()
        {
            manager.Alignment = FlowLayoutManager.Alignments.Right;
            Control ctl1 = CreateControl(50, 20);
            manager.AddControl(ctl1);
            manager.AddControl(CreateControl(20, 20));
            Control ctl2 = CreateControl(50, 20);
            manager.AddControl(ctl2);
            Assert.AreEqual(1, ctl1.TabIndex);
            Assert.AreEqual(2, ctl2.TabIndex);
        }


        public static Control CreateControl(int width, int height)
        {
            Control ctl = new Control();
            ctl.Width = width;
            ctl.Height = height;
            return ctl;
        }
    }
}