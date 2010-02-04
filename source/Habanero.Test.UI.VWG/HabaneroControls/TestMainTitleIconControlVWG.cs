using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.VWG;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestMainTitleIconControlVWG : TestMainTitleIconControl
    {
        protected virtual IMainTitleIconControl CreateControl()
        {
            GetControlFactory();
            return new MainTitleIconControlVWG();
        }

        protected override IControlFactory GetControlFactory()
        {
            GlobalUIRegistry.ControlFactory = CreateNewControlFactory();
            return GlobalUIRegistry.ControlFactory;
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryVWG();
        }
    }
}