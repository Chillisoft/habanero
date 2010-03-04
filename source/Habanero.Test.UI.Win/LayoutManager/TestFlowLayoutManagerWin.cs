using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.LayoutManager
{
    [TestFixture]
    public class TestFlowLayoutManagerWin : TestFlowLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        //Note:this doesn't work in VWG in testing mode
        [Test]
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

        //Note:this doesn't work in VWG in testing mode
        [Test]
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

        //Note_:this doesn't work in VWG in testing mode
        [Test]
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
            Assert.AreEqual((managedControlWidth - controlWidth) / 2, ctl.Left, "Control should be centre aligned.");
            Assert.AreEqual((managedControlWidth - controlWidth) / 2, ctl2.Left, "Control should be centre aligned.");
            Assert.AreEqual(StdSecondRowTop(), ctl2.Top, "Control should be in second row.");
        }

        //Note:this doesn't work in VWG in testing mode
        [Test]
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
    }
}