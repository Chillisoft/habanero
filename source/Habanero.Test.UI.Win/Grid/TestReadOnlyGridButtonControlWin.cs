using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Grid
{
    [TestFixture]
    public class TestReadOnlyGridButtonControlWin : TestReadOnlyGridButtonControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();
        }

        protected override void AddControlToForm(IControlHabanero cntrl)
        {

        }

    }
}