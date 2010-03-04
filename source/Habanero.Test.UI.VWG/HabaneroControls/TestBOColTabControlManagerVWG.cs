using System;
using Habanero.Test.UI.Base.Controllers;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.HabaneroControls
{
    [TestFixture]
    public class TestBOColTabControlManagerVWG : TestBOColTabControlManager
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }
        protected override IBusinessObjectControl GetBusinessObjectControl()
        {
            return new BusinessObjectControlStubVWG();
        }

        protected override Type TypeOfBusinessObjectControl()
        {
            return typeof(BusinessObjectControlStubVWG);
        }
    }
}