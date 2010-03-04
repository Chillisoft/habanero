using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.StandardControls
{
    [TestFixture]
    public class TestPictureBoxWin : TestPictureBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override string GetUnderlyingSizeModeToString(IPictureBox pictureBox)
        {
            System.Windows.Forms.PictureBox control = (System.Windows.Forms.PictureBox)pictureBox;
            return control.SizeMode.ToString();
        }
    }
}