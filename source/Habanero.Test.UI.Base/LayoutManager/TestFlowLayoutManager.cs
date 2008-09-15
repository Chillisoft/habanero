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

using Habanero.UI.Base;
using Habanero.UI.VWG;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Base
{

    public abstract class TestFlowLayoutManager
    {
        private const int _STD_ManagedControl_Width = 100;
        private const int _STD_CONTROL_HEIGHT = 10;
        private const int _STD_CONTROL_WIDTH = 11;
        private const int _DEFAULT_BORDER = 5;//setupIn LayoutManager
        private const int _STD_GAP = 4;
        private const int _STD_BORDER = 5;
        private const int _STD_ManagedControl_Height = 99;
        protected abstract IControlFactory GetControlFactory();

        [TestFixture]
        public class TestFlowLayoutManagerWin : TestFlowLayoutManager
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryWin();
            }
        }

        [TestFixture]
        public class TestFlowLayoutManagerVWG : TestFlowLayoutManager
        {
            protected override IControlFactory GetControlFactory()
            {
                return new ControlFactoryVWG();
            }
        }

        [SetUp]
        public void SetupLayoutManager()
        {

        }

        [Test]
        public void TestManagedControl()
        {            
            //---------------Set up test pack-------------------
            IControlHabanero  managedControl = GetControlFactory().CreateControl();
            //---------------Execute Test ----------------------
            FlowLayoutManager manager = new FlowLayoutManager(managedControl, null);
            //---------------Test Result -----------------------
            Assert.AreSame(managedControl, manager.ManagedControl);
            //---------------Tear Down -------------------------    
        }

        [Test]
        public void TestAddControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero  managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            IControlHabanero  ctlToAdd = CreateStandardControl();
            manager.AddControl(ctlToAdd);
            //---------------Test Result -----------------------
            Assert.AreEqual(stdFirstPosLeft(), ctlToAdd.Left, "Added control's Left prop should be 5 because of the default border.");
            Assert.AreEqual(stdFirstRowTop(), ctlToAdd.Top, "Added control's Top prop should be 5 because of the default border.");
            Assert.AreEqual(_STD_CONTROL_WIDTH, ctlToAdd.Width, "Added control's Width prop should be set.");
            Assert.AreEqual(_STD_CONTROL_HEIGHT, ctlToAdd.Height, "Added control's Height prop should be set.");
        }
        //TODO: This would b cool at moment add a label to left or right to simulate a diff border size when required.
        //[Test]
        //public void TestAddControl_VerticalBorderSize_15()
        //{
        //    //---------------Set up test pack-------------------
        //    IControlHabanero managedControl = CreateManagedControl();
        //    FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
        //    //---------------Execute Test ----------------------
        //    IControlChilli ctlToAdd = CreateStandardControl();
        //    manager
        //    manager.AddControl(ctlToAdd);
        //    //---------------Test Result -----------------------
        //    Assert.AreEqual(stdFirstPosLeft(), ctlToAdd.Left, "Added control's Left prop should be 5 because of the default border.");
        //    Assert.AreEqual(stdFirstRowTop(), ctlToAdd.Top, "Added control's Top prop should be 5 because of the default border.");
        //    Assert.AreEqual(_STD_CONTROL_WIDTH, ctlToAdd.Width, "Added control's Width prop should be set.");
        //    Assert.AreEqual(_STD_CONTROL_HEIGHT, ctlToAdd.Height, "Added control's Height prop should be set.");
        //}
        private IControlHabanero CreateStandardControl()
        {
            return CreateControl(_STD_CONTROL_WIDTH, _STD_CONTROL_HEIGHT);
        }

        [Test]
        public void TestAddTwoControls()
        {
            //---------------Set up test pack-------------------
            IControlHabanero  managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.AddControl(CreateStandardControl());
            IControlHabanero ctl2 = CreateStandardControl();
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual( _STD_BORDER + _STD_GAP + _STD_CONTROL_WIDTH, ctl2.Left, "Left should equal Border + First control's width + spacing");
            Assert.AreEqual(_STD_BORDER, ctl2.Top, "Top should equal Border");
        }

        [Test]
        public void TestAddTwoControlsWithWrap()
        {
            //---------------Set up test pack-------------------
            IControlHabanero  managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            managedControl.Width = 30;
            manager.AddControl(CreateStandardControl());
            IControlHabanero ctl2 = CreateStandardControl();
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(_STD_BORDER, ctl2.Left, "Should wrap to the next line because it's right is outside the area.");
            Assert.AreEqual(_STD_BORDER + _STD_GAP + _STD_CONTROL_HEIGHT, ctl2.Top, "Top should equal Border + first line's max height + gap.");
        }

        [Test]
        public void TestAddThreeControlsOfDifferentHeightWithWrap()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            IControlHabanero ctl2 = CreateControl(10, 20);
            //---------------Execute Test ----------------------
            managedControl.Width = 40;
            manager.AddControl(CreateStandardControl());
            manager.AddControl(ctl2);
            IControlHabanero ctl = CreateStandardControl();
            manager.AddControl(ctl);
            //---------------Test Result -----------------------
            Assert.AreEqual(_STD_BORDER, ctl.Left, "Should wrap to next line because it doesn't fit on first line.");
            Assert.AreEqual(stdFirstRowTop() + ctl2.Height + _STD_GAP, ctl.Top, "Top should equal border + first line's max height + gap.");
        }

        [Test]
        public void TestResizeWhenNothingHappens()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            IControlHabanero ctl1 = CreateStandardControl();
            IControlHabanero ctl2 = CreateStandardControl();
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            manager.ManagedControl.Height = 50;
            //---------------Test Result -----------------------
            Assert.AreEqual(_STD_BORDER, ctl1.Left, "Left should be the same after resize");
            Assert.AreEqual(_STD_BORDER, ctl1.Top, "Top should be the same after resize.");
            Assert.AreEqual(_STD_BORDER + _STD_CONTROL_WIDTH + _STD_GAP, ctl2.Left, "Left should be the same after resize.");
            Assert.AreEqual(_STD_BORDER, ctl2.Top, "Top should be the same after resize.");
        }

        [Test]
        public void TestResizeWhenPositionsShouldChange()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            IControlHabanero ctl1 = CreateStandardControl();
            IControlHabanero ctl2 = CreateStandardControl();
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            manager.ManagedControl.Width = 30;
            //---------------Test Result -----------------------
            Assert.AreEqual(FirstPositionLeft(), ctl1.Left, "Left of ctl1 should be the same after resize");
            Assert.AreEqual(FirstPositionTop(), ctl1.Top, "Top of ctl1 should be the same after resize.");
            Assert.AreEqual(FirstPositionLeft(), ctl2.Left, "Left of ctl2 should have wrapped to next line after resize.");
            Assert.AreEqual(StdSecondRowTop(), ctl2.Top, "Top should be on the next row after resize.");
        }

        [Test]
        public void TestBorderSize()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            IControlHabanero ctl1 = CreateStandardControl();
            manager.BorderSize = 10;
            manager.AddControl(ctl1);
            //---------------Test Result -----------------------
            Assert.AreEqual(manager.BorderSize, ctl1.Left, "Left of ctl1 should equal bordersize.");
            Assert.AreEqual(manager.BorderSize, ctl1.Top, "Top of ctl1 should equal bordersize.");
        }

        [Test]
        public void TestRefreshOnBorderSizeChange()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            IControlHabanero ctl1 = CreateStandardControl();
            IControlHabanero ctl2 = CreateStandardControl();
            manager.ManagedControl.Width = 40;
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            manager.BorderSize = 10;
            //---------------Test Result -----------------------
            Assert.AreEqual(manager.BorderSize, ctl2.Left, "Left of ctl2 should equal bordersize because it should wrap.");
            Assert.AreEqual(manager.BorderSize + _STD_CONTROL_HEIGHT + _STD_GAP, ctl2.Top, "Top of ctl2 should equal bordersize + rowsize + gapsize.");
        }

        [Test]
        public void TestGapSize()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.GapSize = 2;
            manager.AddControl(CreateStandardControl());
            IControlHabanero ctl = CreateStandardControl();
            manager.AddControl(ctl);
            //---------------Test Result -----------------------
            Assert.AreEqual(manager.GapSize + _STD_CONTROL_WIDTH + _STD_BORDER, ctl.Left, "Left of ctl should be bordersize + 10 + gapsize.");
        }

        [Test]
        public void TestRefreshOnGapSizeChange()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.ManagedControl.Width = 100;
            manager.BorderSize = 0;
            manager.AddControl(CreateControl(80, 20));
            IControlHabanero ctl = CreateControl(50, 20);
            manager.AddControl(ctl);
            manager.GapSize = 2;
            //---------------Test Result -----------------------
            Assert.AreEqual(22, ctl.Top, "Top of ctl should be 20 + gapsize. Border is zero");
        }

        [Test]
        public void TestOnlyVisibleControlsIncluded()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.AddControl(CreateStandardControl());
            IControlHabanero ctl1 = CreateStandardControl();
            ctl1.Visible = false;
            manager.AddControl(ctl1);
            IControlHabanero ctl2 = CreateStandardControl();
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(StdSecondPosLeft(), ctl2.Left, "Second Control should be ignored because it is invisible.");
        }

        [Test]
        public void TestRefreshOnControlVisibilityChanged_ChangeControlToNotVisibleControlToRightShiftLeft()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            manager.AddControl(CreateStandardControl());
            IControlHabanero ctl1 = CreateStandardControl();
            manager.AddControl(ctl1);
            IControlHabanero ctl2 = CreateStandardControl();
            manager.AddControl(ctl2);
            //---------------Execute Test ----------------------

            Assert.AreEqual(StdSecondPosLeft() + _STD_CONTROL_WIDTH + _STD_GAP, ctl2.Left);
            ctl1.Visible = false;
            //---------------Test Result -----------------------
            Assert.AreEqual(StdSecondPosLeft(), ctl2.Left, "Control positions should be refreshed when one is made invisible.");
        }

        [Test]
        public void TestRefreshOnControlSizeChanged()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.AddControl(CreateStandardControl());
            IControlHabanero ctl1 = CreateStandardControl();
            manager.AddControl(ctl1);
            IControlHabanero ctl2 = CreateStandardControl();
            manager.AddControl(ctl2);
            ctl1.Width = 20;
            //---------------Test Result -----------------------
            Assert.AreEqual(ctl1.Width + _STD_GAP + StdSecondPosLeft(), ctl2.Left, "Control positions should be refreshed when one control's size is changed.");
        }

        [Test]
        public void TestRightAlignRows()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            const int managedControlWidth = 60;
            manager.ManagedControl.Width = managedControlWidth;
            IControlHabanero ctl = CreateStandardControl();
            IControlHabanero ctl2 = CreateStandardControl();
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Right;
            manager.AddControl(ctl);
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(managedControlWidth - ctl.Width - manager.BorderSize, ctl.Left, "Control should be right aligned.");
            Assert.AreEqual(ctl.Left - _STD_GAP - ctl2.Width, ctl2.Left, "Control should be right aligned.");
        }

        [Test]
        public void TestRightAlignRowsWithWrap()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            const int ManagedControlWidth = 30;
            manager.ManagedControl.Width = ManagedControlWidth;
            IControlHabanero ctl = CreateStandardControl();
            IControlHabanero ctl2 = CreateStandardControl();
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Right;
            manager.AddControl(ctl);
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(ManagedControlWidth - _STD_BORDER - ctl.Width, ctl.Left, "Control should be right aligned.");
            Assert.AreEqual(ManagedControlWidth - _STD_BORDER - ctl2.Width, ctl2.Left, "Control should be right aligned.");
            Assert.AreEqual(StdSecondRowTop(), ctl2.Top, "Control should be right aligned.");
        }
        [Test]
        public void Test_ChangeToRightAlignChangeShouldLayout()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            const int managedControlWidth = 60;
            manager.ManagedControl.Width = managedControlWidth;
            IControlHabanero ctl = CreateStandardControl();
            manager.AddControl(ctl);
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Right;
            //---------------Test Result -----------------------
            Assert.AreEqual(managedControlWidth - ctl.Width - manager.BorderSize, ctl.Left, "Control should be right aligned.");
        }

        [Test,Ignore("does not work in giz")]
        public void TestCentreAlignRowOneControl()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            IControlHabanero ctl = CreateStandardControl();
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Centre;
            manager.AddControl(ctl);
            //---------------Test Result -----------------------
            Assert.AreEqual((_STD_ManagedControl_Width - _STD_CONTROL_WIDTH) / 2, ctl.Left, "Control should be centre aligned.");
        }

        [Test, Ignore("does not work in giz")]
        public void TestCentreAlignRowTwoControls()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            IControlHabanero ctl1 = CreateStandardControl();
            IControlHabanero ctl2 = CreateStandardControl();
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Centre;
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            const int ctl1LeftPos = (_STD_ManagedControl_Width - _STD_CONTROL_WIDTH - _STD_GAP - _STD_CONTROL_WIDTH) / 2;
            const int ctl2LeftPos = ctl1LeftPos + _STD_GAP + _STD_CONTROL_WIDTH;
            Assert.AreEqual(ctl1LeftPos, ctl1.Left, "Control should be centre aligned.");
            Assert.AreEqual(ctl2LeftPos, ctl2.Left, "Control should be centre aligned.");
        }

        [Test, Ignore("does not work in giz")]
        public void TestCentreAlignRowTwoRows()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            const int controlWidth = 20;
            const int managedControlWidth = 40;
            IControlHabanero ctl = CreateControl(controlWidth, 10);
            IControlHabanero ctl2 = CreateControl(controlWidth, 10);
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Centre;
            manager.ManagedControl.Width = managedControlWidth;
            manager.AddControl(ctl);
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual((managedControlWidth - controlWidth)/2, ctl.Left, "Control should be centre aligned.");
            Assert.AreEqual((managedControlWidth - controlWidth) / 2, ctl2.Left, "Control should be centre aligned.");
            Assert.AreEqual(StdSecondRowTop(), ctl2.Top, "Control should be in second row.");
        }

        [Test, Ignore("does not work in giz")]
        public void TestCentreAlignRowWithInvisibleControls()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = CreateManagedControl();
            FlowLayoutManager manager = CreateFlowLayoutManager(managedControl);
            const int controlWidth = 20;
            const int managedControlWidth = 100;
            IControlHabanero ctl = CreateControl(controlWidth, 10);
            IControlHabanero ctl2 = CreateControl(controlWidth, 10);
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Centre;
            manager.ManagedControl.Width = managedControlWidth;
            manager.AddControl(ctl);
            ctl2.Visible = false;
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual((managedControlWidth - controlWidth) / 2, ctl.Left, "Control should be centre aligned - other Control is invisible.");
        }

        [Test]
        public void TestNewLine()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            IControlHabanero ctl = CreateControl(20, 10);
            IControlHabanero ctl2 = CreateControl(20, 10);
            //---------------Execute Test ----------------------
            manager.AddControl(ctl);
            manager.NewLine();
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(stdFirstPosLeft(), ctl2.Left, "ctl2 should be on the far left of a new line");
            Assert.AreEqual(StdSecondRowTop(), ctl2.Top, "ctl2 should be on a new line");
        }

        [Test]
        public void TestGlueSimpleCase()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            IControlHabanero ctl1 = CreateControl(35, 10);
            IControlHabanero ctl2 = CreateControl(35, 10);
            //---------------Execute Test ----------------------
            manager.AddControl(CreateControl(40, 10));
            manager.AddControl(ctl1);
            manager.AddGlue();
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(stdFirstPosLeft(), ctl1.Left, "Glue should mean ctl moves with ctl2 (if space permits)");
            Assert.AreEqual(StdSecondRowTop(), ctl1.Top, "Glue should mean ctl moves to new row with ctl2 (if space permits)");
            Assert.AreEqual(stdFirstPosLeft()  + ctl1.Width + _STD_GAP , ctl2.Left, "ctl2 should be to the right of ctl on a new line because of glue");
            Assert.AreEqual(StdSecondRowTop(), ctl2.Top, "ctl2 should be in row 2");
        }

        [Test, Ignore("speak to peter why is it doing this")]
        public void TestGlueSimpleCase_DontAddSecondControlToGlue_Resize()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            //---------------Execute Test ----------------------
            manager.AddControl(CreateControl(40, 10));
            IControlHabanero gluedCtl = CreateControl(35, 10);
            manager.AddControl(gluedCtl);
            manager.AddGlue();
            manager.ManagedControl.Width = 50;
            //---------------Test Result -----------------------
            Assert.AreEqual(stdFirstPosLeft(), gluedCtl.Left, "Glue should mean ctl moves with ctl2 (if space permits)");
            Assert.AreEqual(StdSecondRowTop(), gluedCtl.Top, "Glue should mean ctl moves to new row with ctl2 (if space permits)");
        }

        [Test]
        public void TestGlueWithGluedControlAtLeftBorder()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            //---------------Execute Test ----------------------
            IControlHabanero ctl = CreateControl(30, 10);
            manager.AddControl(ctl);
            manager.AddGlue();
            IControlHabanero ctl2 = CreateControl(90, 10);
            manager.AddControl(ctl2);

            //---------------Test Result -----------------------
            Assert.AreEqual(5, ctl.Left, "ctl should stay where it is in spite of glue");
            Assert.AreEqual(5, ctl.Top, "ctl should stay where it is in spite of glue");
            Assert.AreEqual(5, ctl2.Left,
                            "ctl2 should move to next line by itself (since ctl and ctl2 can't fit on one line.");
            Assert.AreEqual(StdSecondRowTop(), ctl2.Top, "ctl2 should be in row 2");
        }

        [Test]
        public void TestGlueWithGluedControlInvisible_ChangedToVisible()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            IControlHabanero ctl = CreateControl(30, 10);
            IControlHabanero ctl2 = CreateControl(50, 10);
            //---------------Execute Test ----------------------
            manager.AddControl(CreateControl(50, 10));
            ctl.Visible = false;
            manager.AddControl(ctl);
            manager.AddGlue();
            manager.AddControl(ctl2);

            //---------------Test Result -----------------------
            Assert.AreEqual(0, ctl.Left, "ctl is invisible so should be ignored");
            Assert.AreEqual(0, ctl.Top, "ctl is invisible so should be ignored");
            Assert.AreEqual(stdFirstPosLeft(), ctl2.Left,
                            "ctl2 should move to next line but still be at left border because ctl is invisible ");
            Assert.AreEqual(StdSecondRowTop(), ctl2.Top, "ctl2 should be in row 2");

            //---------------Execute Test ----------------------
            ctl.Visible = true;
            //---------------Test Result -----------------------
            Assert.AreEqual(stdFirstPosLeft(), ctl.Left);
            Assert.AreEqual(StdSecondRowTop(), ctl.Top, "ctl be in second row because of glue");
            Assert.AreEqual(_STD_BORDER  + ctl.Width + _STD_GAP, ctl2.Left, "ctl2 should move accross because ctl is visible ");
            Assert.AreEqual(StdSecondRowTop(), ctl2.Top, "ctl2 should be in row 2");
        }

        [Test]
        public void TestGlueWithGluedControlTooWideToKeepOnSameLine()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            IControlHabanero ctl1 = CreateControl(30, 10);
            IControlHabanero ctl2 = CreateControl(50, 10);
            IControlHabanero ctl3 = CreateControl(50, 10);
            //---------------Execute Test ----------------------
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            manager.AddGlue();
            manager.AddControl(ctl3);

            //---------------Test Result -----------------------
            Assert.AreEqual(ctl1.Width + _STD_GAP + _STD_BORDER , ctl2.Left,
                            "ctl should stay where it is in spite of glue since it cant fit with ctl2 on one line");
            Assert.AreEqual(stdFirstRowTop(), ctl2.Top,
                            "ctl should stay where it is in spite of glue since it cant fit with ctl2 on one line");
            Assert.AreEqual(stdFirstPosLeft(), ctl3.Left,
                            "ctl2 should move to next line by itself (since ctl and ctl2 can't fit on one line.");
            Assert.AreEqual(StdSecondRowTop(), ctl3.Top, "ctl2 should be in row 2");
        }

        [Test]
        public void TestRightAlignedTabOrdering()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Right;
            IControlHabanero ctl1 = CreateControl(20, 20);
            manager.AddControl(ctl1);
            IControlHabanero ctl2 = CreateControl(20, 20);
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, ctl2.TabIndex);
            Assert.AreEqual(1, ctl1.TabIndex);
        }

        [Test]
        public void TestRightAlignedTabOrderingMultipleRows()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            IControlHabanero ctl1 = CreateControl(50, 20);
            IControlHabanero ctl2 = CreateControl(20, 20);
            IControlHabanero ctl3 = CreateControl(50, 20);            
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Right;

            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            manager.AddControl(ctl3);//will be on a new line due to size
            //---------------Test Result -----------------------
            Assert.AreEqual(1, ctl1.TabIndex);
            Assert.AreEqual(0, ctl2.TabIndex);
            Assert.AreEqual(2, ctl3.TabIndex);
        }

        [Test]
        public void TestLeftAlignedTabOrdering()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            IControlHabanero ctl1 = CreateStandardControl();
            IControlHabanero ctl2 = CreateStandardControl();
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Left;
            manager.AddControl(ctl1);
            manager.AddControl(ctl2);
            //---------------Test Result -----------------------
            Assert.AreEqual(0, ctl1.TabIndex);
            Assert.AreEqual(1, ctl2.TabIndex);
        }

        [Test, Ignore("does not work check with peter")]
        public void TestLeftAlignedTabOrderingMultipleRows()
        {
            //---------------Set up test pack-------------------
            FlowLayoutManager manager = CreateFlowLayoutManager();
            //---------------Execute Test ----------------------
            manager.Alignment = FlowLayoutManager.Alignments.Left;
            IControlHabanero ctl1 = CreateControl(50, 20);
            manager.AddControl(ctl1);
            manager.AddControl(CreateControl(20, 20));
            IControlHabanero ctl3 = CreateControl(50, 20);
            manager.AddControl(ctl3);
            //---------------Test Result -----------------------
            Assert.AreEqual(1, ctl1.TabIndex);
            Assert.AreEqual(3, ctl3.TabIndex);
        }

        private static int FirstPositionTop()
        {
            return _STD_BORDER;
        }

        private static int FirstPositionLeft()
        {
            return _STD_BORDER;
        }

        private static int StdSecondRowTop()
        {
            return _STD_BORDER + _STD_CONTROL_HEIGHT + _STD_GAP;
        }
        private static int StdSecondPosLeft()
        {
            return _STD_BORDER + _STD_CONTROL_WIDTH + _STD_GAP;
        }
        private static int stdFirstRowTop()
        {
            return _STD_BORDER;
        }
        private FlowLayoutManager CreateFlowLayoutManager()
        {
            IControlHabanero managedControl = CreateManagedControl();
            return CreateFlowLayoutManager(managedControl);
        }
        private static int stdFirstPosLeft()
        {
            return _DEFAULT_BORDER;
        }

        public IControlHabanero  CreateControl(int width, int height)
        {
            IControlHabanero  ctl = GetControlFactory().CreateControl();
            ctl.Width = width;
            ctl.Height = height;
            return ctl;
        }

        private static FlowLayoutManager CreateFlowLayoutManager(IControlHabanero managedControl)
        {
            FlowLayoutManager manager = new FlowLayoutManager(managedControl, null);
            manager.GapSize = _STD_GAP;
            return manager;
        }

        private IControlHabanero CreateManagedControl()
        {
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            managedControl.Width = _STD_ManagedControl_Width;
            managedControl.Height = _STD_ManagedControl_Height;
            return managedControl;
        }

    }
}