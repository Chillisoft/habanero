
// ------------------------------------------------------------------------------
// This partial class was auto-generated for use with the Habanero Architecture.
// ------------------------------------------------------------------------------

using Habanero.Base;
using Habanero.BO;
using Habanero.BO.ClassDefinition;

namespace CustomerExample.BO
{
    public partial class Customer
    {
//        protected internal Customer(ClassDef def)
//            : base(def)
//        {
//            IBOPropAuthorisation propAuthorisationStub = new CustomerNameAuthorisationPolicy();
//            BOProp propCustomerName = (BOProp)this.Props["CustomerName"];
//            propCustomerName.SetAuthorisationRules(propAuthorisationStub);
//
//            PropDef propDef = (PropDef) propCustomerName.PropDef;
//            propDef.AddPropRule(new MyPropRule(this, propCustomerName));
//        }

        /// <summary>
        /// This method has been added and made public purely for the purposes of testing.
        /// </summary>
        /// <param name="authorisation"></param>
        public void SetAuthorisation(IBusinessObjectAuthorisation authorisation)
        {
            SetAuthorisationRules(authorisation);
        }
    }
}
