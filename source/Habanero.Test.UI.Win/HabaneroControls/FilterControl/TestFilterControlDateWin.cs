using Habanero.Test.UI.Base.FilterController;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestFilterControlDateWin : TestFilterControlDate
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }
        //[Test]
        //public void TestLabelAndDateTimePickerAreOnPanel()
        //{
        //    TestLabelAndDateTimePickerAreOnPanel(2);
        //}
    }
}