using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestInputFormComboBoxVWG : TestInputFormComboBox
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryVWG();
        }

        [Ignore("Minimumsize doesn't work for VWG")]
        public override void Test_CreateOKCancelForm_ShouldSetMinimumSize()
        {

        }

    }
}