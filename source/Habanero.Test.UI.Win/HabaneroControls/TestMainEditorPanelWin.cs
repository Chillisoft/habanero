using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestMainEditorPanelWin : TestMainEditorPanel
    {
        protected override IMainEditorPanel CreateControl(IControlFactory controlFactory)
        {
            return new MainEditorPanelWin(controlFactory);
        }

        protected override IControlFactory CreateNewControlFactory()
        {
            return new ControlFactoryWin();
        }
    }
}