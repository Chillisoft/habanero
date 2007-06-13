using Chillisoft.Bo.v2;
using Chillisoft.Generic.v2;

namespace Chillisoft.UI.Generic.v2
{
    /// <summary>
    /// An interface to model a provider of data to a form
    /// </summary>
    public interface IFormDataProvider
    {
        /// <summary>
        /// Returns the business object collection being represented
        /// </summary>
        /// <returns>Returns the business object collection</returns>
        BusinessObjectCollection GetCollection();

        /// <summary>
        /// Returns the UIFormDef object
        /// </summary>
        /// <returns>Returns the UIFormDef object</returns>
        UIFormDef GetUIFormDef();
    }
}