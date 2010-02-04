using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.StandardControls
{
    [TestFixture]
    public class TestPictureBoxVWG : TestPictureBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        protected override string GetUnderlyingSizeModeToString(IPictureBox pictureBox)
        {
            Gizmox.WebGUI.Forms.PictureBox control = (Gizmox.WebGUI.Forms.PictureBox)pictureBox;
            return control.SizeMode.ToString();
        }
    }
}