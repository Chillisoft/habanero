using Habanero.Bo;
using Habanero.Generic;

namespace Habanero.Ui.Base
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