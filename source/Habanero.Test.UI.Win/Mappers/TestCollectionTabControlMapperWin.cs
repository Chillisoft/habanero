using Habanero.Base;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.Win.Mappers
{
    [TestFixture]
    public class TestCollectionTabControlMapperWin : TestCollectionTabControlMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.Win.ControlFactoryWin();
        }

        protected override IBusinessObjectControl CreateBusinessObjectControl()
        {
            return new BusinessObjectControlWin();
        }

        class BusinessObjectControlWin : Habanero.UI.Win.ControlWin, IBusinessObjectControl
        {

            #region Implementation of IBusinessObjectControl
            // ReSharper disable ValueParameterNotUsed
            /// <summary>
            /// Gets or sets the business object being represented
            /// </summary>
            public IBusinessObject BusinessObject
            {
                get { return null; }
                set { }
            }
            // ReSharper restore ValueParameterNotUsed

            #endregion
        }
    }
}