using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestDefaultBOEditorFormWin : TestDefaultBOEditorForm
    {
        protected override IControlFactory GetControlFactory()
        {
            ControlFactoryWin factory = new Habanero.UI.Win.ControlFactoryWin();
            GlobalUIRegistry.ControlFactory = factory;
            return factory;
        }
    }
}