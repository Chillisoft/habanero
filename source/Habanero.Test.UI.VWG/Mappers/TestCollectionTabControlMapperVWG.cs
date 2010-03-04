using Habanero.Base;
using Habanero.Test.UI.Base;
using Habanero.UI.Base;
using NUnit.Framework;

namespace Habanero.Test.UI.VWG.Mappers
{
    [TestFixture]
    public class TestCollectionTabControlMapperVWG : TestCollectionTabControlMapper
    {
        protected override IControlFactory GetControlFactory()
        {
            return new Habanero.UI.VWG.ControlFactoryVWG();
        }

        protected override IBusinessObjectControl CreateBusinessObjectControl()
        {
            return new BusinessObjectControlVWG();
        }
        class BusinessObjectControlVWG : Habanero.UI.VWG.ControlVWG, IBusinessObjectControl
        {

            //        /// <summary>
            //        /// Specifies the business object being represented
            //        /// </summary>
            //        /// <param name="bo">The business object</param>
            //        public void SetBusinessObject(IBusinessObject bo)
            //        {
            //            
            //        }

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