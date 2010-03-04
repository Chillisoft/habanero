using System;
using Habanero.Test.UI.Base.Controllers;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    [TestFixture]
    public class TestBOColTabControlManagerWin : TestBOColTabControlManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();
        }

        protected override Type TypeOfBusinessObjectControl()
        {
            return typeof(BusinessObjectControlStubWin);
        }

        protected override IBusinessObjectControl GetBusinessObjectControl()
        {
            return new BusinessObjectControlStubWin();
        }
    }
}