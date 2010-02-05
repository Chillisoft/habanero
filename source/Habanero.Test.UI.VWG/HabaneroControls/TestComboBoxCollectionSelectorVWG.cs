using Habanero.Test.UI.Base.Controllers;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestComboBoxCollectionSelectorVWG : TestComboBoxCollectionSelector
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }
    }
}