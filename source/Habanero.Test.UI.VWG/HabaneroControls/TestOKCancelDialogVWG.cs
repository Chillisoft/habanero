using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestOKCancelDialogVWG : TestOKCancelDialog
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}