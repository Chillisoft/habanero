using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG
{
    [TestFixture]
    public class TestCalendarCellVWG : TestCalendarCell
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }
    }
}