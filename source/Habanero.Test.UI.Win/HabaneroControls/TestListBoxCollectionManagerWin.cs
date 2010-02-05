using Habanero.Test.UI.Base.Controllers;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestListBoxCollectionManagerWin : TestListBoxCollectionManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();
        }
    }
}