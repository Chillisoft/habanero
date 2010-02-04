using System;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestBOColTabControlWin : TestBOColTabControl
    {
        protected override IControlFactory GetControlFactory()
        {
            return new ControlFactoryWin();
        }

        protected override IBusinessObjectControl GetBusinessObjectControlStub()
        {
            return new BusinessObjectControlWinStub();
        }
        protected override Type ExpectedTypeOfBOControl()
        {
            return typeof(BusinessObjectControlWinStub);
        }
    }
}