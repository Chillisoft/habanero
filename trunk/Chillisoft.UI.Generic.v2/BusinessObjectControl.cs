using Chillisoft.Bo.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// An interface for controls that manage business objects
    /// </summary>
    public interface BusinessObjectControl
    {
        /// <summary>
        /// Specifies the business object being represented
        /// </summary>
        /// <param name="bo">The business object</param>
        void SetBusinessObject(BusinessObject bo);
    }
}