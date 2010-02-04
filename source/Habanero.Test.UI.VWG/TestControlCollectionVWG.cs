using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG
{
    [TestFixture]
    public class TestControlCollectionVWG : TestControlCollection
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
        [Test]
        public void TestAddControl()
        {
            TextBoxVWG tb = (TextBoxVWG)GetControlFactory().CreateTextBox();
            IControlCollection col = new ControlCollectionVWG(new Gizmox.WebGUI.Forms.Control.ControlCollection(tb));
            IControlHabanero ctl = GetControlFactory().CreateControl();
            col.Add(ctl);
            Assert.AreSame(ctl, col[0], "Control added should be the same object.");
        }
    }
}