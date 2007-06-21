using Habanero.Bo;

namespace Habanero.Ui.Generic
{
    /// <summary>
    /// An interface for controls that manage business objects
    /// </summary>
    public interface IBusinessObjectControl
    {
        /// <summary>
        /// Specifies the business object being represented
        /// </summary>
        /// <param name="bo">The business object</param>
        void SetBusinessObject(BusinessObject bo);
    }
}