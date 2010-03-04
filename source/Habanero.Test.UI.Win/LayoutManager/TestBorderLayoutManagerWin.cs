using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.LayoutManager
{
    [TestFixture]
    public class TestBorderLayoutManagerWin : TestBorderLayoutManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        [Test]
        public void TestSplitter()
        {
            //---------------Set up test pack-------------------
            IControlHabanero managedControl = GetControlFactory().CreateControl();
            managedControl.Width = _STD_MANAGEDCONTROL_WIDTH;
            managedControl.Height = _STD_MANAGEDCONTROL_HEIGHT;
            IControlHabanero ctlEast = CreateControl(20, 20);
            IControlHabanero ctlCentre = CreateControl(1, 1);
            BorderLayoutManager manager = GetControlFactory().CreateBorderLayoutManager(managedControl);
            //---------------Execute Test ----------------------
            manager.AddControl(ctlEast, BorderLayoutManager.Position.East, true);
            manager.AddControl(ctlCentre, BorderLayoutManager.Position.Centre);
            //---------------Test Result -----------------------
            Assert.AreEqual(managedControl.Controls.Count, 3, "There should be 3 controls because of the splitter.");
            Assert.AreEqual(80, ctlEast.Left, "East positioned control doesn't change width when splitter is added.");
            Assert.AreEqual(80 - 3, ctlCentre.Width,
                            "Splitter is 3 wide, so centre control should be 3 less than it would be");
        }
    }
}