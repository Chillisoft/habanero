using System;
using Habanero.Base;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using Habanero.UI.Win;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.HabaneroControls
{
    public class BusinessObjectControlWinStub : ControlWin, IBusinessObjectControl
    {
        private IBusinessObject _bo;

        /// <summary>
        /// Specifies the business object being represented
        /// </summary>
        /// <param name="value">The business object</param>
        public IBusinessObject BusinessObject
        {
            get { return _bo; }
            set { _bo = value; }
        }
    }

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